namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	///     Chosen default values for the Crazyradio.
	///     These might be the actual defaults for the Crazyradio, but there's no
	///     way to tell. So on initiliazation of Crazyradio driver instance, set all defaults.
	/// </summary>
	public static class CrazyradioDefault
	{
		/// <summary>
		///     The default mode the Crazyradio uses. (NormalFlightMode).
		/// </summary>
		public static readonly RadioMode Mode = RadioMode.NormalFlightMode;

		/// <summary>
		///     The default channel the Crazyradio uses. (Channel2).
		/// </summary>
		public static readonly RadioChannel Channel = RadioChannel.Channel2;

		/// <summary>
		///     The default address the Crazyradio uses. (0xE7-0xE7-0xE7-0xE7-0xE7).
		/// </summary>
		public static readonly RadioAddress Address = new RadioAddress(0xE7, 0xE7, 0xE7, 0xE7, 0xE7);

		/// <summary>
		///     The default data rate the Crazyradio uses. (DataRate2Mps).
		/// </summary>
		public static readonly RadioDataRate DataRate = RadioDataRate.DataRate2Mps;

		/// <summary>
		///     The default power level the Crazyradio uses. (TODO).
		/// </summary>
		public static readonly RadioPowerLevel PowerLevel = RadioPowerLevel.ZeroDbm;

		/// <summary>
		///     The default ACK mode the Crazyflie uses. (AutoAckOn).
		/// </summary>
		public static MessageAckMode AckMode = MessageAckMode.AutoAckOn;

		/// <summary>
		///     The default ACK retry count the Crazyradio uses. (Retry3Times).
		/// </summary>
		public static readonly MessageAckRetryCount AckRetryCount = MessageAckRetryCount.Retry3Times;

		/// <summary>
		///     The default ACK retry delay the Crazyradio uses. (UseAckPacketMethod).
		/// </summary>
		public static readonly MessageAckRetryDelay AckRetryDelay = MessageAckRetryDelay.UseAckPacketMethod;

		/// <summary>
		///     The default ACK payload length the Crazyradio uses. (Length32Bytes).
		/// </summary>
		public static readonly MessageAckPayloadLength AckPayloadLength = MessageAckPayloadLength.Length32Bytes;
	}
}