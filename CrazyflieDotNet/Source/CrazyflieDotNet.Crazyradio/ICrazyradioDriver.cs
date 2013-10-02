#region Imports

using System;
using System.Collections.Generic;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   A Crazyradio USB dongle driver that provides an abstraction to the low level usb API.
	/// </summary>
	public interface ICrazyradioDriver
		: IEquatable<ICrazyradioDriver>
	{
		/// <summary>
		///   Returns the unique serial number of the Crazyradio USB dongle.
		/// </summary>
		string SerialNumber { get; }

		/// <summary>
		///   Returns the Crazyradio USB dongle's firmware version.
		/// </summary>
		FirmwareVersion FirmwareVersion { get; }

		/// <summary>
		///   The mode in which the Crazyradio USB dongle will operate.
		/// </summary>
		RadioMode? Mode { get; set; }

		/// <summary>
		///   The radio channel to use when communicating with Crazyflie quadcopters.
		/// </summary>
		RadioChannel? Channel { get; set; }

		/// <summary>
		///   The radio address to use when communicating with Crazyflie quadcopters.
		/// </summary>
		RadioAddress Address { get; set; }

		/// <summary>
		///   The data rate to use when communicating with Crazyflie quadcopters.
		/// </summary>
		RadioDataRate? DataRate { get; set; }

		/// <summary>
		///   The power level to use when communicating with Crazyflie quadcopters.
		/// </summary>
		RadioPowerLevel? PowerLevel { get; set; }

		/// <summary>
		///   The mode in which Crazyradio will handle packet acknowledgement (ACK) from Crazyflie quadcopters.
		/// </summary>
		MessageAckMode? AckMode { get; set; }

		/// <summary>
		///   The number of times to retry sending data after an acknowledgement packagte (ACK) is not received when communicating with Crazyflie quadcopters.
		/// </summary>
		MessageAckRetryCount? AckRetryCount { get; set; }

		/// <summary>
		///   The delay before trying to resend data when an acknowledgement packet (ACK) is not received when communicating with Crazyflie quadcopters.
		///   If set to UseAckPacketMethod, AckPayloadLength is used for delay time, where time is length in seconds of the AckPayloadLength.
		/// </summary>
		MessageAckRetryDelay? AckRetryDelay { get; set; }

		/// <summary>
		///   The length in bytes of the acknowledgement packet (ACK) used when communicating with Crazyflie quadcopters.
		///   If AckRetryDelay is set to UseAckPacketMethod, AckPayloadLength is used for retry delay time, where time is length in seconds of the AckPayloadLength.
		/// </summary>
		MessageAckPayloadLength? AckPayloadLength { get; set; }

		/// <summary>
		///   Returns true if the Crazyradio USB dongle is open and ready for communication.
		/// </summary>
		bool IsOpen { get; }

		/// <summary>
		///   Initializes and opens the Crazyradio USB dongle driver for communication. This must be done before using the driver.
		/// </summary>
		void Open();

		/// <summary>
		///   Closes the Crazyradio USB dongle from communication.
		/// </summary>
		void Close();

		/// <summary>
		///   Sets the crazy radio driver settings to defaults.
		/// </summary>
		void SetsToDefaults();

		/// <summary>
		///   Scans for available Crazyradio USB dongle channels on all DataRates within the given range (or full range of channels).
		///   Starting channel must be a lower channel number than stop channel number.
		/// </summary>
		/// <param name="start"> The starting channel number. Inclusive. </param>
		/// <param name="stop"> The ending channel number. Inclusive. </param>
		IEnumerable<ScanChannelsResult> ScanChannels(RadioChannel channelStart = RadioChannel.Channel1, RadioChannel channelStop = RadioChannel.Channel125);

		/// <summary>
		///   Scans for available Crazyradio USB dongle channels on given data rate within the given range (or full range of channels).
		///   Starting channel must be a lower channel number than stop channel number.
		/// </summary>
		/// <param name="start"> The starting channel number. Inclusive. </param>
		/// <param name="stop"> The ending channel number. Inclusive. </param>
		ScanChannelsResult ScanChannels(RadioDataRate dataRate, RadioChannel channelStart = RadioChannel.Channel1, RadioChannel channelStop = RadioChannel.Channel125);

		/// <summary>
		///   TODO
		/// </summary>
		byte[] SendData(byte[] packetData);
	}
}