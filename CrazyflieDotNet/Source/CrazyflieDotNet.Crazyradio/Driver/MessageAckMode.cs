namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	///     The mode in which Crazyradio will handle packet acknowledgement (ACK) from Crazyflie quadcopters.
	/// </summary>
	public enum MessageAckMode
	{
		/// <summary>
		///     The Crazyradio will automatically wait for acknowledgement packets after sending messages.
		/// </summary>
		AutoAckOn = 0,

		/// <summary>
		///     The Crazyradio will not wait for acknowledgement packets after sending messages. There will be no garauntee that
		///     the messages are received.
		/// </summary>
		AutoAckOff
	}
}