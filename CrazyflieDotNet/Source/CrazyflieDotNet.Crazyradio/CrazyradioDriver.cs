/* 
 *						 _ _  _     
 *		       ____ ___  ___  __________(_|_)(_)____
 *		      / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *		     / / / / / /  __(__  |__  ) /  __/ /    
 *		    /_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *		    Copyright 2013 - http://www.messier.com
 *
 *	This Source Code Form is subject to the terms of the Mozilla Public
 *	License, v. 2.0. If a copy of the MPL was not distributed with this
 *	file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#region Imports

using LibUsbDotNet;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   A Crazyradio USB dongle driver that provides an abstraction to the low level usb API.
	/// </summary>
	public class CrazyradioDriver
		: ICrazyradioDriver, IDisposable
	{
		#region Fields

		private static readonly ILog Log = LogManager.GetLogger(typeof (CrazyradioDriver));

		private static readonly FirmwareVersion MinimumCrazyradioFirmwareVersionRequired = new FirmwareVersion(0, 3, 0);
		private static readonly FirmwareVersion MinimumCrazyradioFastFirmwareChannelScanFirmware = new FirmwareVersion(0, 5, 0);

		private readonly UsbDevice _crazyradioUsbDevice;

		private UsbEndpointReader _crazyradioDataEndpointReader;
		private UsbEndpointWriter _crazyradioDataEndpointWriter;

		private bool _propertiesInitializedToDefaults = false;

		private RadioMode? _mode;
		private RadioChannel? _channel;
		private RadioAddress _address;
		private RadioDataRate? _dataRate;
		private RadioPowerLevel? _powerLevel;
		private MessageAckMode? _ackMode;
		private MessageAckRetryCount? _ackRetryCount;
		private MessageAckRetryDelay? _ackRetryDelay;
		private MessageAckPayloadLength? _ackPayloadLength;

		#endregion Fields

		#region Constructors / Destructors

		/// <summary>
		///   Creates and initializes an instance of a Crazyradio USB dongle driver.
		/// </summary>
		/// <param name="crazyradioUsbDevice"> The UsbDevice to use in this driver. </param>
		public CrazyradioDriver(UsbDevice crazyradioUsbDevice)
		{
#if DEBUG
			Log.DebugFormat("Received UsbDevice to use in CrazyradioDriver.");
#endif

			if (crazyradioUsbDevice == null)
			{
				Log.Error("UsbDevice is null.");
				throw new ArgumentNullException("crazyradioUsbDevice");
			}

			_crazyradioUsbDevice = crazyradioUsbDevice;

			if (_crazyradioUsbDevice.IsOpen)
			{
#if DEBUG
				Log.DebugFormat("UsbDevice is open. Closing until user opens to prevent inconsistent driver state.");
#endif

				_crazyradioUsbDevice.Close();
			}

			if (IsCrazyradioUsbDongle(_crazyradioUsbDevice))
			{
#if DEBUG
				Log.DebugFormat("UsbDevice is in fact a Crazyradio USB dongle.");
#endif

				CheckFirmwareVersion();
				SetsToDefaults();
			}
			else
			{
				const string message = "UsbDevice is not a Crazyradio USB dongle.";
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}
		}

		/// <summary>
		///   Destructor that disposes resources used in this Crazyradio USB dongle driver.
		/// </summary>
		~CrazyradioDriver()
		{
			((IDisposable) this).Dispose();
		}

		#endregion Constructors / Destructors

		#region Properties

		public string SerialNumber
		{
			get { return _crazyradioUsbDevice.Info.SerialString; }
		}

		public FirmwareVersion FirmwareVersion
		{
			get { return new FirmwareVersion(_crazyradioUsbDevice.Info.Descriptor.BcdDevice); }
		}

		public RadioMode? Mode
		{
			get { return _mode; }
			set
			{
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle Mode to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle Channel to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle Address to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle DataRate to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle PowerLevel to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckMode to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckRetryCount to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckRetryDelay to {0}...", value);
#endif

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
#if DEBUG
				Log.DebugFormat("Attempting to set Crazyradio USB dongle AckPayloadLength to {0}...", value);
#endif

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

		#endregion Properties

		#region Public Methods

		public static bool IsCrazyradioUsbDongle(UsbDevice usbDevice)
		{
			if (usbDevice == null)
			{
				return false;
			}

			return usbDevice.Info.Descriptor.VendorID == CrazyradioDeviceId.VendorId
			       && usbDevice.Info.Descriptor.ProductID == CrazyradioDeviceId.ProductId;
		}

		public static IEnumerable<ICrazyradioDriver> GetCrazyradios()
		{
#if DEBUG
			Log.DebugFormat("Looking for Crazyradio USB dongles...");
#endif

			var crazyRadioDrivers = new List<ICrazyradioDriver>();

			var crazyRadiosRegDeviceList = UsbDevice.AllDevices.FindAll(new UsbDeviceFinder(CrazyradioDeviceId.VendorId, CrazyradioDeviceId.ProductId));
			if (crazyRadiosRegDeviceList.Any())
			{
#if DEBUG
				Log.DebugFormat("Found {0} Crazyradio USB dongle(s).", crazyRadiosRegDeviceList.Count);
#endif

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

		public IEnumerable<ScanChannelsResult> ScanChannels(RadioChannel channelStart = RadioChannel.Channel1, RadioChannel channelStop = RadioChannel.Channel125)
		{
			if (channelStop < channelStart)
			{
				const string message = "Stop channel must be a higher channel number than start channel.";
				Log.Error(message);
				throw new ArgumentException(message);
			}

#if DEBUG
			Log.DebugFormat("Attemping to scan channels in range. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);
#endif

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

#if DEBUG
			Log.DebugFormat("Results of scanning channels in range. Found: {0}. StartChannel: {1}, StopChannel: {2}.", results.Count(), channelStart, channelStop);
#endif
#if DEBUG
			Log.Debug("Reverting data rate and channel to original values.");
#endif

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

#if DEBUG
			Log.DebugFormat("About to scan channels for data rate in channel range. StartChannel: {0}, StopChannel: {1}, DataRate: {2}.", channelStart, channelStop);
#endif

			var result = ScanChannelsUsingDataRate(dataRate, channelStart, channelStop);

#if DEBUG
			Log.DebugFormat("Results of scanning channels for data rate in range. Found: {0}. StartChannel: {1}, StopChannel: {2}, DataRate: {3}.", result.Channels.Count(), channelStart, channelStop, dataRate);
#endif
#if DEBUG
			Log.Debug("Reverting data rate and channel to original values.");
#endif

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
				throw new Exception(UsbDevice.LastErrorString);
			}
			else
			{
#if DEBUG
				Log.DebugFormat("Succesfully sent packet to Crazyradio USB dongle. PacketData: {0}, LengthTransferred: {1}.", BitConverter.ToString(packetData), lengthTransferred);
#endif

				var readAckBuffer = new byte[32];
				var readAckErrorCode = _crazyradioDataEndpointReader.Read(readAckBuffer, 100, out lengthTransferred);
				var readAckFailed = readAckErrorCode != ErrorCode.None;
				var ackPacketData = readAckFailed ? null : readAckBuffer.Take(lengthTransferred).ToArray();

#if DEBUG
				Log.DebugFormat("Succesfully read ACK packet from Crazyradio USB dongle. PacketData: {0}, LengthTransferred: {1}.", ackPacketData == null ? "NULL" : BitConverter.ToString(ackPacketData), lengthTransferred);
#endif

				return ackPacketData;
			}
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ICrazyradioDriver);
		}

		public bool Equals(ICrazyradioDriver other)
		{
			if (other == null)
			{
				return false;
			}

			return SerialNumber.Equals(other.SerialNumber);
		}

		public override int GetHashCode()
		{
			return SerialNumber.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Serial#: {0}. Open: {1}.", SerialNumber, IsOpen);
		}

		#region Setup / Teardown

		public void Open()
		{
			try
			{
#if DEBUG
				Log.DebugFormat("Opening/initializing Crazyradio USB dongle for communication...");
#endif

				var wholeUsbDevice = _crazyradioUsbDevice as IUsbDevice;
				if (!ReferenceEquals(wholeUsbDevice, null))
				{
					wholeUsbDevice.Open();

#if DEBUG
					Log.DebugFormat("Opened Crazyradio USB dongle.");
#endif

					wholeUsbDevice.SetConfiguration(1);

#if DEBUG
					Log.DebugFormat("Set Crazyradio USB dongle configuration to 1.");
#endif

					wholeUsbDevice.ClaimInterface(0);

#if DEBUG
					Log.DebugFormat("Claimed interface 0 of Crazyradio USB dongle.");
#endif
				}

				if (!_propertiesInitializedToDefaults)
				{
					SetsToDefaults();
				}

				_crazyradioDataEndpointReader = _crazyradioUsbDevice.OpenEndpointReader(CrazyradioDataEndpointId.DataReadEndpointId);
				_crazyradioDataEndpointWriter = _crazyradioUsbDevice.OpenEndpointWriter(CrazyradioDataEndpointId.DataWriteEndpointId);

#if DEBUG
				Log.DebugFormat("Successfully opened/initializing Crazyradio USB dongle for communication.");
#endif
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
#if DEBUG
				Log.DebugFormat("Closing Crazyradio USB dongle from communication...");
#endif

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

#if DEBUG
					Log.DebugFormat("Successfully closed Crazyradio USB dongle from communication.");
#endif
				}
				else
				{
#if DEBUG
					Log.DebugFormat("Crazyradio USB dongle is already closed.");
#endif
				}
			}
			catch (Exception ex)
			{
				const string message = "Error closing Crazyradio USB dongle from communication.";
				Log.Error(message, ex);
				throw new CrazyradioDriverException(message, ex);
			}
		}

		void IDisposable.Dispose()
		{
			Close();
		}

		public void SetsToDefaults()
		{
#if DEBUG
			Log.DebugFormat("Resetting Crazyradio USB dongle to default settings...");
#endif

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

#if DEBUG
			Log.DebugFormat("Successfully reset Crazyradio USB dongle to default settings.");
#endif
		}

		#endregion Setup / Teardown

		#endregion Public Methods

		#region Helpers Methods

		private void CheckFirmwareVersion()
		{
#if DEBUG
			Log.DebugFormat("Crazyradio USB dongle version is {0}.", FirmwareVersion);
#endif

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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle Mode to {0}.", mode);
#endif

				var enableContinuousCarrierMode = (mode == RadioMode.ContinuousCarrierMode) ? 1 : 0;
				ControlTransferOut(CrazyradioRequest.SetContinuousCarrierMode, (short) enableContinuousCarrierMode, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle Mode to {0}.", mode);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle Channel to {0}.", channel);
#endif

				ControlTransferOut(CrazyradioRequest.SetChannel, (short) channel, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle Channel to {0}.", channel);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle Address to {0}.", address);
#endif

				ControlTransferOut(CrazyradioRequest.SetAddress, 0, 0, 5, address.Bytes);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle Address to {0}.", address);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle DataRate to {0}.", dataRate);
#endif

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) dataRate, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle DataRate to {0}.", dataRate);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle PowerLevel to {0}.", powerLevel);
#endif

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) powerLevel, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle PowerLevel to {0}.", powerLevel);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle AckMode to {0}.", ackMode);
#endif

				var enableAutoAck = (ackMode == MessageAckMode.AutoAckOn) ? 1 : 0;
				ControlTransferOut(CrazyradioRequest.SetAutoActEnabled, (short) enableAutoAck, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle AckMode to {0}.", ackMode);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle AckRetryCount to {0}.", ackRetryCount);
#endif

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) ackRetryCount, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle AckRetryCount to {0}.", ackRetryCount);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle AckRetryDelay to {0}.", ackRetryDelay);
#endif

				ControlTransferOut(CrazyradioRequest.SetDataRate, (short) ackRetryDelay, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle AckRetryDelay to {0}.", ackRetryDelay);
#endif
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
#if DEBUG
				Log.DebugFormat("Setting Crazyradio USB dongle AckPayloadLength to {0}.", ackPayloadLength);
#endif

				var value = (byte) ((byte) ackPayloadLength | 0x80); // To set the ACK payload length the bit 7 of ARD must be set (length | 0x80).
				ControlTransferOut(CrazyradioRequest.SetDataRate, value, 0, 0, new byte[0]);

#if DEBUG
				Log.DebugFormat("Successfully set Crazyradio USB dongle AckPayloadLength to {0}.", ackPayloadLength);
#endif
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
#if DEBUG
				Log.DebugFormat("Starting firmware level Crazyradio USB dongle ChannelScan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);
#endif

				ControlTransferOut(CrazyradioRequest.ScanChannels, (short) channelStart, (short) channelStop, 1, new byte[] {0xFF});

#if DEBUG
				Log.DebugFormat("Successfully started firmware level Crazyradio USB dongle ChannelScan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);
#endif
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
#if DEBUG
				Log.DebugFormat("Getting firmware level Crazyradio USB dongle ChannelScan results.");
#endif

				var data = new byte[63];
				ControlTransferIn(CrazyradioRequest.ScanChannels, 0, 0, (short) data.Length, data);

#if DEBUG
				Log.DebugFormat("Successfully got firmware level Crazyradio USB dongle ChannelScan results.");
#endif

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
#if DEBUG
				Log.DebugFormat("Starting manual Crazyradio USB dongle ChannelScan. StartChannel: {0}, StopChannel: {1}.", channelStart, channelStop);
#endif

				var results = new List<RadioChannel>();

				var ackPacket = new byte[] {0xFF};
				for (var currentChannel = channelStart; currentChannel <= channelStop; currentChannel++)
				{
					Channel = currentChannel;
					var result = SendData(ackPacket);
				}

#if DEBUG
				Log.DebugFormat("Manual Crazyradio USB dongle ChannelScan completed.");
#endif

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
#if DEBUG
			Log.Debug(controlTransferInputsString);
#endif

#if DEBUG
			Log.Debug("Sending Crazyradio USB dongle a control transfer message.");
#endif
			messageFailed = !_crazyradioUsbDevice.ControlTransfer(ref setupPacket, data, data.Length, out lengthTransferred);

			if (messageFailed)
			{
				var message = string.Format("Failed sending Crazyradio USB dongle a control message. {0}\nUsbErrorMessage:\n{1}", controlTransferInputsString, UsbDevice.LastErrorString);
				Log.Error(message);
				throw new CrazyradioDriverException(message);
			}
			else
			{
#if DEBUG
				Log.DebugFormat("Successfully sent Crazyradio USB dongle a control message. \n\tLength transferred: {0}, \n\tData: [{1}].", lengthTransferred, BitConverter.ToString(data));
#endif
			}
		}

		#endregion Helpers Methods
	}
}