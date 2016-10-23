#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using LibUsbDotNet;
using LibUsbDotNet.Main;

#endregion

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	///     A Crazyradio USB dongle driver that provides an abstraction to the low level USB API.
	/// </summary>
	public class CrazyradioDriver
		: ICrazyradioDriver, IDisposable
	{
		#region Private Members

		private static readonly ILog Log = LogManager.GetLogger(typeof(CrazyradioDriver));
		private static readonly FirmwareVersion MinimumCrazyradioFirmwareVersionRequired = new FirmwareVersion(0, 3, 0);
		private static readonly FirmwareVersion MinimumCrazyradioFastFirmwareChannelScanFirmware = new FirmwareVersion(0, 5, 0);
		private MessageAckMode? _ackMode;
		private MessageAckPayloadLength? _ackPayloadLength;
		private MessageAckRetryCount? _ackRetryCount;
		private MessageAckRetryDelay? _ackRetryDelay;
		private RadioAddress _address;
		private RadioChannel? _channel;
		private UsbEndpointReader _crazyradioDataEndpointReader;
		private UsbEndpointWriter _crazyradioDataEndpointWriter;
		private RadioDataRate? _dataRate;
		private RadioMode? _mode;
		private RadioPowerLevel? _powerLevel;
		private readonly UsbDevice _crazyradioUsbDevice;

		#endregion Private Members

		#region Constructors/Destructors

		/// <summary>
		///     Creates and initializes an instance of a Crazyradio USB dongle driver.
		/// </summary>
		/// <param name="crazyradioUsbDevice"> The UsbDevice to use in this CrazyradioDriver. </param>
		public CrazyradioDriver(UsbDevice crazyradioUsbDevice)
		{
			Log.DebugFormat("Received UsbDevice for use: {0}", crazyradioUsbDevice);

			if (crazyradioUsbDevice == null)
			{
				Log.Error("UsbDevice is null.");
				throw new ArgumentNullException(nameof(crazyradioUsbDevice));
			}

			_crazyradioUsbDevice = crazyradioUsbDevice;

			if (IsCrazyradioUsbDongle(_crazyradioUsbDevice))
			{
				Log.Debug("UsbDevice is a Crazyradio USB dongle.");

				CheckFirmwareVersion();
			}
			else
			{
				const string message = "UsbDevice is not a Crazyradio USB dongle.";
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}

			if (_crazyradioUsbDevice.IsOpen)
			{
				Log.Debug("UsbDevice is open. Closing until user opens to prevent inconsistent CrazyradioDriver state.");

				_crazyradioUsbDevice.Close();
			}

			Log.DebugFormat("Initialized with Crazyradio dongle UsbDevice - Manufacturer: {0}, Product: {1}, Serial#: {2}, VendorId: {3} ProductId:D {4}.", _crazyradioUsbDevice.Info.ManufacturerString, _crazyradioUsbDevice.Info.ProductString, _crazyradioUsbDevice.Info.SerialString, crazyradioUsbDevice.Info.Descriptor.VendorID, crazyradioUsbDevice.Info.Descriptor.ProductID);
		}

		~CrazyradioDriver()
		{
			Log.Debug("Finalizer called.");

			Close();
		}

		#endregion Constructors/Destructors

		#region Public Static Methods

		public static bool IsCrazyradioUsbDongle(UsbDevice USBDevice)
		{
			if (USBDevice == null)
			{
				return false;
			}

			return USBDevice.Info.Descriptor.VendorID == CrazyradioDeviceId.VendorId &&
				   USBDevice.Info.Descriptor.ProductID == CrazyradioDeviceId.ProductId;
		}

		public static IEnumerable<ICrazyradioDriver> GetCrazyradios()
		{
			return GetCrazyradios(CrazyradioDeviceId.VendorId, CrazyradioDeviceId.ProductId);
		}

		public static IEnumerable<ICrazyradioDriver> GetCrazyradios(int vendorId, int productId)
		{
			Log.InfoFormat("Looking for Crazyradio USB dongles with vendor id {0} and product id {1}.", vendorId, productId);

			var crazyRadioDrivers = new List<ICrazyradioDriver>();

			var crazyRadiosRegDeviceList = UsbDevice.AllDevices.FindAll(new UsbDeviceFinder(vendorId, productId));
			if (crazyRadiosRegDeviceList.Any())
			{
				Log.DebugFormat("Found {0} usb devices with vendor id {1} and product id {2}.", crazyRadiosRegDeviceList.Count, vendorId, productId);

				foreach (UsbRegistry crazyRadioUsbDevice in crazyRadiosRegDeviceList)
				{
					if (crazyRadioUsbDevice.Device != null)
					{
						Log.DebugFormat("Device registry enty - Manufacturer: {0}, Product: {1}, Serial#: {2}, VendorId: {3} ProductId:D {4}.", crazyRadioUsbDevice.Device.Info.ManufacturerString, crazyRadioUsbDevice.Device.Info.ProductString, crazyRadioUsbDevice.Device.Info.SerialString, vendorId, productId);

						crazyRadioDrivers.Add(new CrazyradioDriver(crazyRadioUsbDevice.Device));
					}
					else
					{
						Log.WarnFormat("Device registry entry has a null Device property.");
					}
				}
			}
			else
			{
				Log.WarnFormat("Found no Crazyradio USB dongles with vendor id {0} and product id {1}.", vendorId, productId);
			}

			Log.InfoFormat("Found {0} Crazyradio USB dongle(s) with vendor id {1} and product id {2}.", crazyRadioDrivers.Count, vendorId, productId);

			return crazyRadioDrivers;
		}

		#endregion Public Static Methods

		#region ICrazyradioDriver Members

		public string SerialNumber
		{
			get { return _crazyradioUsbDevice.Info.SerialString; }
		}

		public FirmwareVersion FirmwareVersion
		{
			get { return new FirmwareVersion(_crazyradioUsbDevice.Info.Descriptor); }
		}

		public RadioMode? Mode
		{
			get { return _mode; }
			set
			{
				Log.DebugFormat("Attempting to set Mode to {0}...", value);

				if (value == null)
				{
					const string message = "Mode cannot be set to null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				try
				{
					Log.DebugFormat("Setting Mode to {0}.", value);

					var enableContinuousCarrierMode = (value == RadioMode.ContinuousCarrierMode) ? 1 : 0;
					ControlTransferOut(CrazyradioRequest.SetContinuousCarrierMode, (short)enableContinuousCarrierMode, 0, 0, new byte[0]);
					_mode = value;

					Log.InfoFormat("Mode set to {0}.", value);
				}
				catch (Exception ex)
				{
					var message = string.Format("Failed setting Mode to {0}.", value);
					Log.Error(message);
					throw new CrazyradioDriverException(message, ex);
				}
			}
		}

		public RadioChannel? Channel
		{
			get { return _channel; }
			set
			{
				Log.DebugFormat("Attempting to set Channel to {0}...", value);

				if (value == null)
				{
					const string message = "Channel cannot be set to null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				try
				{
					Log.DebugFormat("Setting Channel to {0}.", value);

					ControlTransferOut(CrazyradioRequest.SetChannel, (short)value, 0, 0, new byte[0]);
					_channel = value;

					Log.InfoFormat("Channel set to {0}.", value);
				}
				catch (Exception ex)
				{
					var message = string.Format("Failed setting channel to {0}.", value);
					Log.Error(message);
					throw new CrazyradioDriverException(message, ex);
				}
			}
		}

		public RadioAddress Address
		{
			get { return _address; }
			set
			{
				Log.DebugFormat("Attempting to set Address to {0}...", value);

				if (value == null)
				{
					const string message = "Address cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				try
				{
					Log.DebugFormat("Setting Address to {0}.", value);

					ControlTransferOut(CrazyradioRequest.SetAddress, 0, 0, 5, value.Bytes);
					_address = value;

					Log.InfoFormat("Address set to {0}.", value);
				}
				catch (Exception ex)
				{
					var message = string.Format("Failed setting Address to {0}.", value);
					Log.Error(message);
					throw new CrazyradioDriverException(message, ex);
				}
			}
		}

		public RadioDataRate? DataRate
		{
			get { return _dataRate; }
			set
			{
				Log.DebugFormat("Attempting to set DataRate to {0}...", value);

				if (value == null)
				{
					const string message = "DataRate cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				try
				{
					Log.DebugFormat("Setting DataRate to {0}.", value);

					ControlTransferOut(CrazyradioRequest.SetDataRate, (short)value, 0, 0, new byte[0]);
					_dataRate = value;

					Log.InfoFormat("DataRate set to {0}.", value);
				}
				catch (Exception ex)
				{
					var message = string.Format("Failed setting DataRate to {0}.", value);
					Log.Error(message);
					throw new CrazyradioDriverException(message, ex);
				}
			}
		}

		public RadioPowerLevel? PowerLevel
		{
			get { return _powerLevel; }
			set
			{
				Log.DebugFormat("Attempting to set PowerLevel to {0}...", value);

				if (value == null)
				{
					const string message = "PowerLevel cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				try
				{
					Log.DebugFormat("Setting PowerLevel to {0}.", value);

					ControlTransferOut(CrazyradioRequest.SetPowerLevel, (short)value, 0, 0, new byte[0]);
					_powerLevel = value;

					Log.InfoFormat("PowerLevel set to {0}.", value);
				}
				catch (Exception ex)
				{
					var message = string.Format("Failed setting PowerLevel to {0}", value);
					Log.Error(message);
					throw new CrazyradioDriverException(message, ex);
				}
			}
		}

		public MessageAckMode? AckMode
		{
			get { return _ackMode; }
			set
			{
				Log.DebugFormat("Attempting to set AckMode to {0}...", value);

				if (value == null)
				{
					const string message = "AckMode cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				try
				{
					Log.DebugFormat("Setting AckMode to {0}.", value);

					var enableAutoAck = (value == MessageAckMode.AutoAckOn) ? 1 : 0;
					ControlTransferOut(CrazyradioRequest.SetAutoActEnabled, (short)enableAutoAck, 0, 0, new byte[0]);
					_ackMode = value;

					Log.InfoFormat("AckMode set to {0}.", value);
				}
				catch (Exception ex)
				{
					var message = string.Format("Failed setting AckMode to {0}", value);
					Log.Error(message);
					throw new CrazyradioDriverException(message, ex);
				}
			}
		}

		public MessageAckRetryCount? AckRetryCount
		{
			get { return _ackRetryCount; }
			set
			{
				Log.DebugFormat("Attempting to set AckRetryCount to {0}...", value);

				if (value == null)
				{
					const string message = "AckRetryCount cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				try
				{
					Log.DebugFormat("Setting AckRetryCount to {0}.", value);

					ControlTransferOut(CrazyradioRequest.SetAckRetryCount, (short)value, 0, 0, new byte[0]);
					_ackRetryCount = value;

					Log.InfoFormat("AckRetryCount set to {0}.", value);
				}
				catch (Exception ex)
				{
					var message = string.Format("AckRetryCount count to {0}", value);
					Log.Error(message);
					throw new CrazyradioDriverException(message, ex);
				}
			}
		}

		public MessageAckRetryDelay? AckRetryDelay
		{
			get { return _ackRetryDelay; }
			set
			{
				Log.DebugFormat("Attempting to set AckRetryDelay to {0}...", value);

				if (value == null)
				{
					const string message = "AckRetryDelay cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				if (value != MessageAckRetryDelay.UseAckPacketMethod)
				{
					try
					{
						Log.DebugFormat("Setting AckRetryDelay to {0}.", value);

						ControlTransferOut(CrazyradioRequest.SetAckRetryDelay, (short)value, 0, 0, new byte[0]);
						_ackRetryDelay = value;

						Log.InfoFormat("AckRetryDelay set to {0}.", value);
					}
					catch (Exception ex)
					{
						var message = string.Format("Failed setting AckRetryDelay to {0}", value);
						Log.Error(message);
						throw new CrazyradioDriverException(message, ex);
					}

					if (AckPayloadLength != MessageAckPayloadLength.UseAckRetryDelayMethod)
					{
						Log.DebugFormat("Updating AckPayloadLength to UseAckRetryDelayMethod for consistency.");

						_ackPayloadLength = MessageAckPayloadLength.UseAckRetryDelayMethod;
					}
				}
				else
				{
					_ackRetryDelay = value;

					Log.InfoFormat("AckRetryDelay set to {0}.", value);

					if (AckPayloadLength == MessageAckPayloadLength.UseAckRetryDelayMethod && AckPayloadLength != CrazyradioDefault.AckPayloadLength)
					{
						Log.DebugFormat("UseAckPacketMethod provided as value for AckRetryDelay and value of AckPayloadLength is UseAckRetryDelayMethod, so setting AckPayloadLength to default value of {0}.", CrazyradioDefault.AckPayloadLength);

						AckPayloadLength = CrazyradioDefault.AckPayloadLength;
					}
				}
			}
		}

		public MessageAckPayloadLength? AckPayloadLength
		{
			get { return _ackPayloadLength; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckPayloadLength to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio AckPayloadLength cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				if (value != MessageAckPayloadLength.UseAckRetryDelayMethod)
				{
					try
					{
						Log.DebugFormat("Setting AckPayloadLength to {0}.", value);

						var formattedValue = (byte)((byte)value | 0x80); // To set the ACK payload length the bit 7 of ARD must be set (length | 0x80).
						ControlTransferOut(CrazyradioRequest.SetAckRetryDelay, formattedValue, 0, 0, new byte[0]);
						_ackPayloadLength = value;

						Log.InfoFormat("AckPayloadLength set to {0}.", value);
					}
					catch (Exception ex)
					{
						var message = string.Format("Failed setting AckPayloadLength to {0}", value);
						Log.Error(message);
						throw new CrazyradioDriverException(message, ex);
					}

					if (AckRetryDelay != MessageAckRetryDelay.UseAckPacketMethod)
					{
						Log.DebugFormat("Updating AckRetryDelay to UseAckPacketMethod for consistency.");

						_ackRetryDelay = MessageAckRetryDelay.UseAckPacketMethod;
					}
				}
				else
				{
					_ackPayloadLength = value;

					Log.InfoFormat("AckPayloadLength set to {0}.", value);

					if (AckRetryDelay == MessageAckRetryDelay.UseAckPacketMethod && AckRetryDelay != CrazyradioDefault.AckRetryDelay)
					{
						Log.DebugFormat("UseAckPacketMethod provided as value for AckRetryDelay and value of AckPayloadLength is UseAckRetryDelayMethod, so setting AckPayloadLength to default value of {0}.", CrazyradioDefault.AckPayloadLength);

						AckRetryDelay = CrazyradioDefault.AckRetryDelay;
					}
				}
			}
		}

		public bool IsOpen
		{
			get { return _crazyradioUsbDevice.IsOpen; }
		}

		public bool IsClosed
		{
			get { return !IsOpen; }
		}

		public void ResetDriver()
		{
			Log.DebugFormat("Driver reset starting.");

			Close();
			Open();

			Log.DebugFormat("Driver reset completed.");
		}

		public IEnumerable<ScanChannelsResult> ScanChannels(RadioChannel channelStart = RadioChannel.Channel1, RadioChannel channelStop = RadioChannel.Channel125)
		{
			if (channelStop < channelStart)
			{
				const string message = "Stop channel must be a higher channel number than start channel.";
				Log.Error(message);
				throw new ArgumentException(message);
			}

			Log.DebugFormat("Attemping to scan channels in range. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);

			var channelBackup = Channel;
			var dataRateBackup = DataRate;

			var results = new List<ScanChannelsResult>();

			var results250Kps = ScanChannelsUsingDataRate(RadioDataRate.DataRate250Kps, channelStart, channelStop);
			if (results250Kps.Channels.Any())
			{
				results.Add(results250Kps);
			}

			var results1Mps = ScanChannelsUsingDataRate(RadioDataRate.DataRate1Mps, channelStart, channelStop);
			if (results1Mps.Channels.Any())
			{
				results.Add(results1Mps);
			}

			var results2Mps = ScanChannelsUsingDataRate(RadioDataRate.DataRate2Mps, channelStart, channelStop);
			if (results2Mps.Channels.Any())
			{
				results.Add(results2Mps);
			}

			Log.DebugFormat("Results of scanning channels in range. Found: {0}. StartChannel: {1}, StopChannel: {2}.", results.Count(), channelStart, channelStop);
			Log.Debug("Reverting data rate and channel to original values.");

			Channel = channelBackup;
			DataRate = dataRateBackup;

			return results;
		}

		public ScanChannelsResult ScanChannels(RadioDataRate dataRate, RadioChannel channelStart = RadioChannel.Channel1, RadioChannel channelStop = RadioChannel.Channel125)
		{
			if (channelStart > channelStop)
			{
				const string message = "Stop channel must be a higher channel number than start channel.";
				Log.Error(message);
				throw new ArgumentException(message);
			}

			var channelBackup = Channel;
			var dataRateBackup = DataRate;

			Log.DebugFormat("About to scan channels for data rate in channel range. StartChannel: {0}, StopChannel: {1}, DataRate: {2}.", channelStart, channelStop);

			var result = ScanChannelsUsingDataRate(dataRate, channelStart, channelStop);

			Log.DebugFormat("Results of scanning channels for data rate in range. Found: {0}. StartChannel: {1}, StopChannel: {2}, DataRate: {3}.", result.Channels.Count(), channelStart, channelStop, dataRate);
			Log.Debug("Reverting data rate and channel to original values.");

			Channel = channelBackup;
			DataRate = dataRateBackup;

			return result;
		}

		public byte[] SendData(byte[] data)
		{
			if (!IsOpen)
			{
				const string message = "CrazyradioDriver is not open for communication.";
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}

			Log.Debug("CrazyradioDriver is open for communication.");

			var lengthTransferred = -1;
			var dataLength = data != null ? data.Length : 0;

			Log.DebugFormat("Writing data to UsbDevice. Data: {0}, DataLength: {1}.", data == null ? "NULL" : BitConverter.ToString(data), dataLength);

			var sendErrorCode = _crazyradioDataEndpointWriter.Write(data, 0, dataLength, 1000, out lengthTransferred);
			var sendFailed = sendErrorCode != ErrorCode.None;

			Log.DebugFormat("Write data success? {0}. LengthTransferred: {1}. ErrorCode: {2}.", !sendFailed, lengthTransferred, sendErrorCode);

			if (sendFailed)
			{
				var message = string.Format("Error writing data to UsbDevice. ErrorCode: {0}, LengthTransferred: {1}.", sendErrorCode, lengthTransferred);
				Log.Error(message);
				throw new CrazyradioDriverException(UsbDevice.LastErrorString);
			}
			else
			{
				Log.DebugFormat("Succesfully wrote data to UsbDevice. Data: {0}, LengthTransferred: {1}.", data == null ? "NULL" : BitConverter.ToString(data), lengthTransferred);

				var responseBuffer = new byte[32];

				Log.DebugFormat("Reading data from UsbDevice.");

				var readErrorCode = _crazyradioDataEndpointReader.Read(responseBuffer, 100, out lengthTransferred);
				var readFailed = readErrorCode != ErrorCode.None;
				var response = readFailed ? null : responseBuffer.Take(lengthTransferred).ToArray();

				Log.DebugFormat("Read data success? {0}. ErrorCode: {1}.", !readFailed, readErrorCode);

				if (readFailed)
				{
					Log.WarnFormat("Error reading data from UsbDevice. ErrorCode: {0}.", readErrorCode);

					return null;
				}
				else
				{
					Log.DebugFormat("Succesfully read data from UsbDevice. Data: {0}, LengthTransferred: {1}.", response == null ? "NULL" : BitConverter.ToString(response), lengthTransferred);

					return response;
				}
			}
		}

		public bool Equals(ICrazyradioDriver other)
		{
			if (other == null)
			{
				return false;
			}

			return SerialNumber.Equals(other.SerialNumber);
		}

		public void Open()
		{
			try
			{
				Log.Debug("Opening CrazyradioDriver for communication...");

				var wholeUsbDevice = _crazyradioUsbDevice as IUsbDevice;
				if (!ReferenceEquals(wholeUsbDevice, null))
				{
					wholeUsbDevice.Open();

					Log.Debug("Opened UsbDevice for communication.");

					wholeUsbDevice.SetConfiguration(1);

					Log.Debug("Set UsbDevice configuration to 1.");

					wholeUsbDevice.ClaimInterface(0);

					Log.Debug("Claimed interface 0 of UsbDevice.");
				}

				_crazyradioDataEndpointReader = _crazyradioUsbDevice.OpenEndpointReader(CrazyradioDataEndpointId.Read);
				_crazyradioDataEndpointWriter = _crazyradioUsbDevice.OpenEndpointWriter(CrazyradioDataEndpointId.Write);

				ResetSettings();

				Log.Debug("Successfully opened CrazyradioDriver for communication.");
			}
			catch (Exception ex)
			{
				const string message = "Error opening/initializing CrazyradioDriver for communication.";
				Log.Error(message, ex);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		public void Close()
		{
			try
			{
				Log.Debug("Closing CrazyradioDriver from communication...");

				if (_crazyradioDataEndpointReader != null && !_crazyradioDataEndpointReader.IsDisposed)
				{
					Log.Debug("Closing UsbDevice endpoint reader.");

					_crazyradioDataEndpointReader.Dispose();
				}
				else
				{
					Log.Debug("UsbDevice endpoint reader already closed.");
				}

				if (_crazyradioDataEndpointWriter != null && !_crazyradioDataEndpointWriter.IsDisposed)
				{
					Log.Debug("Closing UsbDevice endpoint writer.");

					_crazyradioDataEndpointWriter.Dispose();
				}
				else
				{
					Log.Debug("UsbDevice endpoint writer already closed.");
				}

				if (_crazyradioUsbDevice.IsOpen)
				{

					Log.Debug("Closing UsbDevice.");

					_crazyradioUsbDevice.Close();
				}
				else
				{
					Log.Debug("UsbDevice is already closed.");
				}
			}
			catch (Exception ex)
			{
				const string message = "Error closing Crazyradio USB dongle from communication.";
				Log.Error(message, ex);
				throw new CrazyradioDriverException(message, ex);
			}

			Log.Debug("Closed CrazyradioDriver from communication.");
		}

		public void ResetSettings()
		{
			Log.Debug("Resetting Crazyradio to default settings...");

			Mode = CrazyradioDefault.Mode;
			Channel = CrazyradioDefault.Channel;
			Address = CrazyradioDefault.Address;
			DataRate = CrazyradioDefault.DataRate;
			PowerLevel = CrazyradioDefault.PowerLevel;
			AckMode = CrazyradioDefault.AckMode;
			AckRetryCount = CrazyradioDefault.AckRetryCount;
			AckRetryDelay = CrazyradioDefault.AckRetryDelay;
			AckPayloadLength = CrazyradioDefault.AckPayloadLength;

			Log.Debug("Successfully reset Crazyradio to default settings.");
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			Log.Debug("Disposing CrazyradioDriver...");

			Close();
		}

		#endregion

		#region Equality and Formatting Overrides

		public override bool Equals(object obj)
		{
			return Equals(obj as ICrazyradioDriver);
		}

		public override int GetHashCode()
		{
			return SerialNumber.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Serial#: {0} (Open: {1})", SerialNumber, IsOpen);
		}

		#endregion Equality and Formatting Overrides

		#region Private Methods

		private void CheckFirmwareVersion()
		{
			Log.DebugFormat("Checking Crazyradio USB dongle firmware version {0}...", FirmwareVersion);

			if (FirmwareVersion.CompareTo(MinimumCrazyradioFirmwareVersionRequired) < 0)
			{
				var message = string.Format("Mininum firmware version required for this version of CrazyradioDriver is {0}.", MinimumCrazyradioFirmwareVersionRequired);
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}

			Log.DebugFormat("Crazyradio USB dongle version {0} is acceptable for use in CrazyradioDriver.", FirmwareVersion);
		}

		private ScanChannelsResult ScanChannelsUsingDataRate(RadioDataRate dataRate, RadioChannel channelStart, RadioChannel channelStop)
		{
			var allResults = new List<RadioChannel>();

			DataRate = dataRate;

			if (FirmwareVersion.CompareTo(MinimumCrazyradioFastFirmwareChannelScanFirmware) >= 0)
			{
				StartRadioScanningChannels(channelStart, channelStop);
				var results = GetRadioChannelScanningResults();
				allResults.AddRange(results);
			}
			else // slow pc level channel scan
			{
				var results = ManuallyScanForChannels(channelStart, channelStop);
				allResults.AddRange(results);
			}

			return new ScanChannelsResult(dataRate, allResults);
		}

		private void StartRadioScanningChannels(RadioChannel channelStart, RadioChannel channelStop)
		{
			try
			{
				Log.DebugFormat("Starting firmware level channel scan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);

				ControlTransferOut(CrazyradioRequest.ScanChannels, (short)channelStart, (short)channelStop, 1, new byte[] { 0xFF });

				Log.DebugFormat("Successfully started firmware level channel scan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed starting firmware level channel scan. Start Channel: {0}, Stop Channel: {1}.", channelStart, channelStop);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private IEnumerable<RadioChannel> GetRadioChannelScanningResults()
		{
			try
			{
				Log.Debug("Getting firmware level chanel scan results.");

				var data = new byte[63];
				ControlTransferIn(CrazyradioRequest.ScanChannels, 0, 0, (short)data.Length, data);

				Log.Debug("Successfully got firmware level channel scan results.");

				return data.Where(a => a != 0).Select(b => (RadioChannel)b);
			}
			catch (Exception ex)
			{
				const string message = "Failed getting firmware level channel scan results.";
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private IEnumerable<RadioChannel> ManuallyScanForChannels(RadioChannel channelStart, RadioChannel channelStop)
		{
			try
			{
				Log.DebugFormat("Starting manual channel scan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);

				var results = new List<RadioChannel>();

				var ping = new byte[] { 0xFF };
				for (var currentChannel = channelStart; currentChannel <= channelStop; currentChannel++)
				{
					Channel = currentChannel;

					Log.DebugFormat("Sending data {0} on channel data {1}.", BitConverter.ToString(ping), currentChannel);
					var ack = SendData(ping);
					Log.DebugFormat("Received data {0} on channel {1}.", BitConverter.ToString(ping), currentChannel);

					if (ack != null && ack.Length > 0)
					{
						var ackReceivedBytes = (byte)(ack[0] & 0x01);
						var ackReceived = Convert.ToBoolean(ackReceivedBytes);

						Log.DebugFormat("AckReceived? {0} (Bytes: {1}) on channel {2}.", ackReceived, BitConverter.ToString(new[] { ackReceivedBytes }), currentChannel);

						if (ackReceived)
						{
							Log.DebugFormat("Adding channel {0} to results.", currentChannel);
							results.Add(currentChannel);
						}
					}
				}

				Log.DebugFormat("Manual channel scan completed with {0} results. StartChannel: {1}, StopChannel: {2}.", results, channelStart, channelStop);

				return results;
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed running manual Crazyradio channel scan. Start Channel: {0}, Stop Channel: {1}.", channelStart, channelStop);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void ControlTransferOut(CrazyradioRequest request, short value, short index, short length, byte[] data)
		{
			ControlTransfer(UsbRequestType.TypeVendor, UsbRequestRecipient.RecipDevice, UsbEndpointDirection.EndpointOut, (byte)request, value, index, length, data);
		}

		private void ControlTransferIn(CrazyradioRequest request, short value, short index, short length, byte[] data)
		{
			ControlTransfer(UsbRequestType.TypeVendor, UsbRequestRecipient.RecipDevice, UsbEndpointDirection.EndpointIn, (byte)request, value, index, length, data);
		}

		private void ControlTransfer(UsbRequestType requestType, UsbRequestRecipient requestRecipient, UsbEndpointDirection requestDirection, byte request, short value, short index, short length, byte[] data)
		{
			if (!IsOpen)
			{
				const string message = "CrazyradioDriver is not open.";
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}

			var lengthTransferred = -1;
			var messageFailed = true;

			var requestTypeByte = (byte)requestType;
			var requestRecipientByte = (byte)requestRecipient;
			var requestDirectionByte = (byte)requestDirection;
			var fullRequestTypeByte = (byte)(requestTypeByte | requestRecipientByte | requestDirectionByte);
			var setupPacket = new UsbSetupPacket(fullRequestTypeByte, request, value, index, length);

			var controlTransferInputsString = string.Format("Control transfer inputs: \n\tFullRequestTypeByte: 0x{0:x2} => (0x{1:x2} (RequestType: {2}) | 0x{3:x2} (RequestRecipient: {4}) | 0x{5:x2} (RequestDirection: {6})), \n\tRequest: 0x{7:x2}, \n\tValue: {8}, \n\tIndex: {9}, \n\tLength: {10}, \n\tData: [{11}].", fullRequestTypeByte, requestTypeByte, requestType, requestRecipientByte, requestRecipient, requestDirectionByte, requestDirection, request, value, index, length, BitConverter.ToString(data));
			Log.Debug(controlTransferInputsString);

			Log.Debug("Sending UsbDevice a control transfer message.");
			messageFailed = !_crazyradioUsbDevice.ControlTransfer(ref setupPacket, data, data.Length, out lengthTransferred);

			if (messageFailed)
			{
				var message = string.Format("Failed sending UsbDevice a control message. {0}\nUsbErrorMessage:\n{1}", controlTransferInputsString, UsbDevice.LastErrorString);
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}
			else
			{
				Log.DebugFormat("Successfully sent UsbDevice a control message. \n\tLength transferred: {0}, \n\tData: [{1}].", lengthTransferred, BitConverter.ToString(data));
			}
		}

		#endregion Private Methods
	}
}
