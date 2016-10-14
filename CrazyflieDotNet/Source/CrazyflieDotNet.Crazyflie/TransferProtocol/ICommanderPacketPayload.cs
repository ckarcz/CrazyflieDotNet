namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Commander packet payload.
	/// </summary>
	public interface ICommanderPacketPayload
		: IOutputPacketPayload
	{
		/// <summary>
		/// Gets the roll.
		/// </summary>
		/// <value>The roll.</value>
		float Roll { get; }

		/// <summary>
		/// Gets the pitch.
		/// </summary>
		/// <value>The pitch.</value>
		float Pitch { get; }

		/// <summary>
		/// Gets the yaw.
		/// </summary>
		/// <value>The yaw.</value>
		float Yaw { get; }

		/// <summary>
		/// Gets the thrust.
		/// </summary>
		/// <value>The thrust.</value>
		ushort Thrust { get; }
	}
}