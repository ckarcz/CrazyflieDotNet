namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Output packet header.
	/// </summary>
	public interface IOutputPacketHeader
		: IPacketHeader
	{
		/// <summary>
		/// Gets the port.
		/// </summary>
		/// <value>The port.</value>
		CommunicationPort Port { get; }

		/// <summary>
		/// Gets the channel.
		/// </summary>
		/// <value>The channel.</value>
		CommunicationChannel Channel { get; }
	}
}