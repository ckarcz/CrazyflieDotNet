#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using log4net;

#endregion

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	///   A Crazyradio USB dongle driver that provides an abstraction to the low level USB API.
	/// </summary>
	public class CrazyradioDriver
		: ICrazyradioDriver, IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CrazyradioDriver));

		private static readonly FirmwareVersion MinimumCrazyradioFirmwareVersionRequired = new FirmwareVersion(0, 3, 0);
		private static readonly FirmwareVersion MinimumCrazyradioFastFirmwareChannelScanFirmware = new FirmwareVersion(0, 5, 0);

		private readonly UsbDevice _crazyradioUsbDevice;
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
		private bool _propertiesInitializedToDefaults = false;

		/// <summary>
		///   Creates and initializes an instance of a Crazyradio USB dongle driver.
		/// </summary>
		/// <param name="crazyradioUsbDevice"> The UsbDevice to use in this driver. </param>
		public CrazyradioDriver(UsbDevice crazyradioUsbDevice)
		{
			Log.Debug("Received UsbDevice to use in CrazyradioDriver.");

			if (crazyradioUsbDevice == null)
			{
				Log.Error("UsbDevice is null.");
				throw new ArgumentNullException("crazyradioUsbDevice");
			}

			_crazyradioUsbDevice = crazyradioUsbDevice;

			if (IsCrazyradioUsbDongle(_crazyradioUsbDevice))
			{
				Log.Debug("UsbDevice is in fact a Crazyradio USB dongle.");

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
				Log.Debug("UsbDevice is open. Closing until user opens to prevent inconsistent driver state.");

				_crazyradioUsbDevice.Close();
			}
		}

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
				Log.DebugFormat("Attempting to set Crazyradio USB dongle Mode to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio Mode cannot be set to null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				SetMode(value.Value);
				_mode = value;
			}
		}

		public RadioChannel? Channel
		{
			get { return _channel; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle Channel to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio Channel cannot be set to null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				SetChannel(value.Value);
				_channel = value;
			}
		}

		public RadioAddress Address
		{
			get { return _address; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle Address to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio Address cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				SetAddress(value);
				_address = value;
			}
		}

		public RadioDataRate? DataRate
		{
			get { return _dataRate; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle DataRate to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio DataRate cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				SetDataRate(value.Value);
				_dataRate = value;
			}
		}

		public RadioPowerLevel? PowerLevel
		{
			get { return _powerLevel; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle PowerLevel to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio PowerLevel cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				SetPowerLevel(value.Value);
				_powerLevel = value;
			}
		}

		public MessageAckMode? AckMode
		{
			get { return _ackMode; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckMode to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio AckMode cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				SetAckMode(value.Value);
				_ackMode = value;
			}
		}

		public MessageAckRetryCount? AckRetryCount
		{
			get { return _ackRetryCount; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckRetryCount to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio AckRetryCount cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				SetAckRetryCount(value.Value);
				_ackRetryCount = value;
			}
		}

		public MessageAckRetryDelay? AckRetryDelay
		{
			get { return _ackRetryDelay; }
			set
			{
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckRetryDelay to {0}...", value);

				if (value == null)
				{
					const string message = "Crazyradio AckRetryDelay cannot be null.";
					Log.Error(message);
					throw new CrazyradioDriverException(message);
				}

				if (value != MessageAckRetryDelay.UseAckPacketMethod)
				{
					SetAckRetryDelay(value.Value);
					_ackPayloadLength = MessageAckPayloadLength.UseAckRetryDelayMethod;
				}
				else
				{
					AckPayloadLength = CrazyradioDefaults.DefaultAckPayloadLength;
				}

				_ackRetryDelay = value;
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
					SetAckPayloadLength(value.Value);
					_ackRetryDelay = MessageAckRetryDelay.UseAckPacketMethod;
				}
				else
				{
					// Default is using package method, hence below settings.
					AckRetryDelay = CrazyradioDefaults.DefaultAckRetryDelay;
					AckPayloadLength = CrazyradioDefaults.DefaultAckPayloadLength;
				}

				_ackPayloadLength = value;
			}
		}

		public bool IsOpen
		{
			get { return _crazyradioUsbDevice.IsOpen; }
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
				results.Add(results250Kps);
			}

			var results2Mps = ScanChannelsUsingDataRate(RadioDataRate.DataRate2Mps, channelStart, channelStop);
			if (results2Mps.Channels.Any())
			{
				results.Add(results250Kps);
			}

			Log.DebugFormat("Results of scanning channels in range. Found: {0}. StartChannel: {1}, StopChannel: {2}.", results.Count(), channelStart, channelStop);
			Log.Debug("Reverting data rate and channel to original values.");

			Channel = channelBackup;
			DataRate = dataRateBackup;

			return results;
		}

		public ScanChannelsResult ScanChannels(RadioDataRate dataRate, RadioChannel channelStart = RadioChannel.Channel1, RadioChannel channelStop = RadioChannel.Channel125)
		{
			if (channelStart < channelStop)
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

		public byte[] SendData(byte[] packetData)
		{
			if (!IsOpen)
			{
				const string message = "Crazyradio USB dongle is not open.";
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}

			var lengthTransferred = -1;
			var sendPacketErrorCode = _crazyradioDataEndpointWriter.Write(packetData, 0, packetData.Length, 1000, out lengthTransferred);
			var sendPacketFailed = sendPacketErrorCode != ErrorCode.None;

			if (sendPacketFailed)
			{
				var message = string.Format("Error sending packet to Crazyradio USB dongle. ErrorCode: {0}, LengthTransferred: {1}.", sendPacketErrorCode, lengthTransferred);
				Log.Error(message);
				throw new CrazyradioDriverException(UsbDevice.LastErrorString);
			}
			else
			{
				Log.DebugFormat("Succesfully sent packet to Crazyradio USB dongle. PacketData: {0}, LengthTransferred: {1}.", BitConverter.ToString(packetData), lengthTransferred);

				var readAckBuffer = new byte[32];
				var readAckErrorCode = _crazyradioDataEndpointReader.Read(readAckBuffer, 100, out lengthTransferred);
				var readAckFailed = readAckErrorCode != ErrorCode.None;
				var ackPacketData = readAckFailed ? null : readAckBuffer.Take(lengthTransferred).ToArray();

				Log.DebugFormat("Succesfully read ACK packet from Crazyradio USB dongle. PacketData: {0}, LengthTransferred: {1}.", ackPacketData == null ? "NULL" : BitConverter.ToString(ackPacketData), lengthTransferred);

				return ackPacketData;
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
				Log.Debug("Opening/initializing Crazyradio USB dongle for communication...");

				var wholeUsbDevice = _crazyradioUsbDevice as IUsbDevice;
				if (!ReferenceEquals(wholeUsbDevice, null))
				{
					wholeUsbDevice.Open();

					Log.Debug("Opened Crazyradio USB dongle.");

					wholeUsbDevice.SetConfiguration(1);

					Log.Debug("Set Crazyradio USB dongle configuration to 1.");

					wholeUsbDevice.ClaimInterface(0);

					Log.Debug("Claimed interface 0 of Crazyradio USB dongle.");
				}

				if (!_propertiesInitializedToDefaults)
				{
					SetsToDefaults();
				}

				_crazyradioDataEndpointReader = _crazyradioUsbDevice.OpenEndpointReader(CrazyradioDataEndpointId.DataReadEndpointId);
				_crazyradioDataEndpointWriter = _crazyradioUsbDevice.OpenEndpointWriter(CrazyradioDataEndpointId.DataWriteEndpointId);

				Log.Debug("Successfully opened/initializing Crazyradio USB dongle for communication.");
			}
			catch (Exception ex)
			{
				const string message = "Error opening/initializing Crazyradio USB dongle for communication.";
				Log.Error(message, ex);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		public void Close()
		{
			try
			{
				Log.Debug("Closing Crazyradio USB dongle from communication...");

				if (_crazyradioDataEndpointReader != null && !_crazyradioDataEndpointReader.IsDisposed)
				{
					_crazyradioDataEndpointReader.Dispose();
				}

				if (_crazyradioDataEndpointWriter != null && !_crazyradioDataEndpointWriter.IsDisposed)
				{
					_crazyradioDataEndpointWriter.Dispose();
				}

				if (_crazyradioUsbDevice.IsOpen)
				{
					_crazyradioUsbDevice.Close();

					Log.Debug("Successfully closed Crazyradio USB dongle from communication.");
				}
				else
				{
					Log.DebugFormat("Crazyradio USB dongle is already closed.");
				}
			}
			catch (Exception ex)
			{
				const string message = "Error closing Crazyradio USB dongle from communication.";
				Log.Error(message, ex);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		public void SetsToDefaults()
		{
			Log.Debug("Resetting Crazyradio USB dongle to default settings...");

			Mode = CrazyradioDefaults.DefaultMode;
			Channel = CrazyradioDefaults.DefaultChannel;
			Address = CrazyradioDefaults.DefaultAddress;
			DataRate = CrazyradioDefaults.DefaultDataRate;
			PowerLevel = CrazyradioDefaults.DefaultPowerLevel;
			AckMode = CrazyradioDefaults.DefaultAckMode;
			AckRetryCount = CrazyradioDefaults.DefaultAckRetryCount;
			AckRetryDelay = CrazyradioDefaults.DefaultAckRetryDelay;
			AckPayloadLength = CrazyradioDefaults.DefaultAckPayloadLength;

			_propertiesInitializedToDefaults = true;

			Log.Debug("Successfully reset Crazyradio USB dongle to default settings.");
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			Close();
		}

		#endregion

		/// <summary>
		///   Destructor that disposes resources used in this Crazyradio USB dongle driver.
		/// </summary>
		~CrazyradioDriver()
		{
			((IDisposable) this).Dispose();
		}

		public static bool IsCrazyradioUsbDongle(UsbDevice USBDevice)
		{
			if (USBDevice == null)
			{
				return false;
			}

			return USBDevice.Info.Descriptor.VendorID == CrazyradioDeviceId.VendorId
			       && USBDevice.Info.Descriptor.ProductID == CrazyradioDeviceId.ProductId;
		}

		public static IEnumerable<ICrazyradioDriver> GetCrazyradios()
		{
			Log.Debug("Looking for Crazyradio USB dongles...");

			var crazyRadioDrivers = new List<ICrazyradioDriver>();

			var crazyRadiosRegDeviceList = UsbDevice.AllDevices.FindAll(new UsbDeviceFinder(CrazyradioDeviceId.VendorId, CrazyradioDeviceId.ProductId));
			if (crazyRadiosRegDeviceList.Any())
			{
				Log.DebugFormat("Found {0} Crazyradio USB dongle(s).", crazyRadiosRegDeviceList.Count);

				foreach (UsbRegistry crazyRadioUsbDevice in crazyRadiosRegDeviceList)
				{
					crazyRadioDrivers.Add(new CrazyradioDriver(crazyRadioUsbDevice.Device));
				}
			}
			else
			{
				Log.Warn("Found no Crazyradio USB dongles.");
			}

			return crazyRadioDrivers;
		}

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
			return string.Format("Serial#: {0}. Open: {1}.", SerialNumber, IsOpen);
		}

		private void CheckFirmwareVersion()
		{
			Log.DebugFormat("Crazyradio USB dongle version is {0}.", FirmwareVersion);

			if (FirmwareVersion.CompareTo(MinimumCrazyradioFirmwareVersionRequired) < 0)
			{
				var message = string.Format("Mininum firmware version required for this version of CrazyradioDriver is {0}.", MinimumCrazyradioFirmwareVersionRequired);
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}
		}

		private void SetMode(RadioMode mode)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle Mode to {0}.", mode);

				var enableContinuousCarrierMode = (mode == RadioMode.ContinuousCarrierMode) ? 1 : 0;
				ControlTransferOut(CrazyradioRequest.SetContinuousCarrierMode, (short) enableContinuousCarrierMode, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle Mode to {0}.", mode);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle mode to {0}.", mode);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetChannel(RadioChannel channel)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle Channel to {0}.", channel);

				ControlTransferOut(CrazyradioRequest.SetChannel, (short) channel, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle Channel to {0}.", channel);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle channel to {0}.", channel);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetAddress(RadioAddress address)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle Address to {0}.", address);

				ControlTransferOut(CrazyradioRequest.SetAddress, 0, 0, 5, address.Bytes);

				Log.DebugFormat("Successfully set Crazyradio USB dongle Address to {0}.", address);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle address to {0}.", address);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetDataRate(RadioDataRate dataRate)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle DataRate to {0}.", dataRate);

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) dataRate, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle DataRate to {0}.", dataRate);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle data rate to {0}.", dataRate);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetPowerLevel(RadioPowerLevel powerLevel)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle PowerLevel to {0}.", powerLevel);

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) powerLevel, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle PowerLevel to {0}.", powerLevel);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle power level to {0}", powerLevel);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetAckMode(MessageAckMode ackMode)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle AckMode to {0}.", ackMode);

				var enableAutoAck = (ackMode == MessageAckMode.AutoAckOn) ? 1 : 0;
				ControlTransferOut(CrazyradioRequest.SetAutoActEnabled, (short) enableAutoAck, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle AckMode to {0}.", ackMode);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle auto ack mode to {0}", ackMode);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetAckRetryCount(MessageAckRetryCount ackRetryCount)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle AckRetryCount to {0}.", ackRetryCount);

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) ackRetryCount, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle AckRetryCount to {0}.", ackRetryCount);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle ack retry count to {0}", ackRetryCount);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetAckRetryDelay(MessageAckRetryDelay ackRetryDelay)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle AckRetryDelay to {0}.", ackRetryDelay);

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) ackRetryDelay, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle AckRetryDelay to {0}.", ackRetryDelay);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle ack retry delay to {0}", ackRetryDelay);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void SetAckPayloadLength(MessageAckPayloadLength ackPayloadLength)
		{
			try
			{
				Log.DebugFormat("Setting Crazyradio USB dongle AckPayloadLength to {0}.", ackPayloadLength);

				var value = (byte) ((byte) ackPayloadLength | 0x80); // To set the ACK payload length the bit 7 of ARD must be set (length | 0x80).
				ControlTransferOut(CrazyradioRequest.SetDataRate, value, 0, 0, new byte[0]);

				Log.DebugFormat("Successfully set Crazyradio USB dongle AckPayloadLength to {0}.", ackPayloadLength);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed setting Crazyradio USB dongle ack payload length to {0}", ackPayloadLength);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private ScanChannelsResult ScanChannelsUsingDataRate(RadioDataRate dataRate, RadioChannel channelStart, RadioChannel channelStop)
		{
			var results = new List<RadioChannel>();

			DataRate = dataRate;

			if (FirmwareVersion.CompareTo(MinimumCrazyradioFastFirmwareChannelScanFirmware) >= 0)
			{
				StartRadioScanningChannels(channelStart, channelStop);
				results.AddRange(GetRadioChannelScanningResults());
			}
			else // slow pc level channel scan
			{
				results.AddRange(ManuallyScanForChannels(channelStart, channelStop));
			}

			return new ScanChannelsResult(dataRate, results);
		}

		private void StartRadioScanningChannels(RadioChannel channelStart, RadioChannel channelStop)
		{
			try
			{
				Log.DebugFormat("Starting firmware level Crazyradio USB dongle ChannelScan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);

				ControlTransferOut(CrazyradioRequest.ScanChannels, (short) channelStart, (short) channelStop, 1, new byte[] {0xFF});

				Log.DebugFormat("Successfully started firmware level Crazyradio USB dongle ChannelScan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed starting firmware level Crazyradio USB dongle channel scan. Start Channel: {0}, Stop Channel: {1}.", channelStart, channelStop);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private IEnumerable<RadioChannel> GetRadioChannelScanningResults()
		{
			try
			{
				Log.Debug("Getting firmware level Crazyradio USB dongle ChannelScan results.");

				var data = new byte[63];
				ControlTransferIn(CrazyradioRequest.ScanChannels, 0, 0, (short) data.Length, data);

				Log.Debug("Successfully got firmware level Crazyradio USB dongle ChannelScan results.");

				return data.Where(a => a != 0).Select(b => (RadioChannel) b);
			}
			catch (Exception ex)
			{
				const string message = "Failed getting firmware level Crazyradio USB dongle channel scan results.";
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private IEnumerable<RadioChannel> ManuallyScanForChannels(RadioChannel channelStart, RadioChannel channelStop)
		{
			try
			{
				Log.DebugFormat("Starting manual Crazyradio USB dongle ChannelScan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);

				var results = new List<RadioChannel>();

				var ackPacket = new byte[] {0xFF};
				for (var currentChannel = channelStart; currentChannel <= channelStop; currentChannel++)
				{
					Channel = currentChannel;
					var result = SendData(ackPacket);
				}

				Log.Debug("Manual Crazyradio USB dongle ChannelScan completed.");

				return results;
			}
			catch (Exception ex)
			{
				var message = string.Format("Failed running manual Crazyradio USB dongle channel scan. Start Channel: {0}, Stop Channel: {1}.", channelStart, channelStop);
				Log.Error(message);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		private void ControlTransferOut(CrazyradioRequest request, short value, short index, short length, byte[] data)
		{
			ControlTransfer(UsbRequestType.TypeVendor, UsbRequestRecipient.RecipDevice, UsbEndpointDirection.EndpointOut, (byte) request, value, index, length, data);
		}

		private void ControlTransferIn(CrazyradioRequest request, short value, short index, short length, byte[] data)
		{
			ControlTransfer(UsbRequestType.TypeVendor, UsbRequestRecipient.RecipDevice, UsbEndpointDirection.EndpointIn, (byte) request, value, index, length, data);
		}

		private void ControlTransfer(UsbRequestType requestType, UsbRequestRecipient requestRecipient, UsbEndpointDirection requestDirection, byte request, short value, short index, short length, byte[] data)
		{
			if (!IsOpen)
			{
				const string message = "Crazyradio USB dongle is not open.";
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}

			var lengthTransferred = -1;
			var messageFailed = true;

			var requestTypeByte = (byte) requestType;
			var requestRecipientByte = (byte) requestRecipient;
			var requestDirectionByte = (byte) requestDirection;
			var fullRequestTypeByte = (byte) (requestTypeByte | requestRecipientByte | requestDirectionByte);
			var setupPacket = new UsbSetupPacket(fullRequestTypeByte, request, value, index, length);

			var controlTransferInputsString = string.Format("Control transfer inputs: \n\tFullRequestTypeByte: 0x{0:x2} => (0x{1:x2} (RequestType: {2}) | 0x{3:x2} (RequestRecipient: {4}) | 0x{5:x2} (RequestDirection: {6})), \n\tRequest: 0x{7:x2}, \n\tValue: {8}, \n\tIndex: {9}, \n\tLength: {10}, \n\tData: [{11}].", fullRequestTypeByte, requestTypeByte, requestType, requestRecipientByte, requestRecipient, requestDirectionByte, requestDirection, request, value, index, length, BitConverter.ToString(data));
			Log.Debug(controlTransferInputsString);

			Log.Debug("Sending Crazyradio USB dongle a control transfer message.");
			messageFailed = !_crazyradioUsbDevice.ControlTransfer(ref setupPacket, data, data.Length, out lengthTransferred);

			if (messageFailed)
			{
				var message = string.Format("Failed sending Crazyradio USB dongle a control message. {0}\nUsbErrorMessage:\n{1}", controlTransferInputsString, UsbDevice.LastErrorString);
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}
			else
			{
				Log.DebugFormat("Successfully sent Crazyradio USB dongle a control message. \n\tLength transferred: {0}, \n\tData: [{1}].", lengthTransferred, BitConverter.ToString(data));
			}
		}
	}
}