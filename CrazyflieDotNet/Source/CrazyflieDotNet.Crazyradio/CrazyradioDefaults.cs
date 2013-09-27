﻿namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   Default values for the Crazyradio.
	/// </summary>
	public static class CrazyradioDefaults
	{
		/// <summary>
		///   The default mode the Crazyradio uses. (NormalFlightMode).
		/// </summary>
		public static readonly RadioMode DefaultMode = RadioMode.NormalFlightMode;

		/// <summary>
		///   The default channel the Crazyradio uses. (Channel2).
		/// </summary>
		public static readonly RadioChannel DefaultChannel = RadioChannel.Channel2;

		/// <summary>
		///   The default address the Crazyradio uses. (0xE7-0xE7-0xE7-0xE7-0xE7).
		/// </summary>
		public static readonly IRadioAddress DefaultAddress = new RadioAddress(0xE7, 0xE7, 0xE7, 0xE7, 0xE7);

		/// <summary>
		///   The default data rate the Crazyradio uses. (DataRate2Mps).
		/// </summary>
		public static readonly RadioDataRate DefaultDataRate = RadioDataRate.DataRate2Mps;

		/// <summary>
		///   The default power level the Crazyradio uses. (TODO).
		/// </summary>
		public static readonly RadioPowerLevel DefaultPowerLevel = RadioPowerLevel.ZeroDbm;

		/// <summary>
		///   The default ACK mode the Crazyflie uses. (AutoAckOn).
		/// </summary>
		public static MessageAckMode DefaultAckMode = MessageAckMode.AutoAckOn;

		/// <summary>
		///   The default ACK retry count the Crazyradio uses. (Retry3Times).
		/// </summary>
		public static readonly MessageAckRetryCount DefaultAckRetryCount = MessageAckRetryCount.Retry3Times;

		/// <summary>
		///   The default ACK retry delay the Crazyradio uses. (UseAckPacketMethod).
		/// </summary>
		public static readonly MessageAckRetryDelay DefaultAckRetryDelay = MessageAckRetryDelay.UseAckPacketMethod;

		/// <summary>
		///   The default ACK payload length the Crazyradio uses. (Length32Bytes).
		/// </summary>
		public static readonly MessageAckPayloadLength DefaultAckPayloadLength = MessageAckPayloadLength.Length32Bytes;
	}
}