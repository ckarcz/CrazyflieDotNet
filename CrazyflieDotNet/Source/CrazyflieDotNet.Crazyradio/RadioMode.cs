namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   The mode in which the Crazyradio USB dongle will operate.
	/// </summary>
	public enum RadioMode
	{
		/// <summary>
		///   This is normal flight.
		/// </summary>
		NormalFlightMode = 0,

		/// <summary>
		///   This is a testing mode in which a continuous non-modulated sine wave is emitted. This allows testing the radio. No packets are transmitted.
		/// </summary>
		ContinuousCarrierMode = 1
	}
}