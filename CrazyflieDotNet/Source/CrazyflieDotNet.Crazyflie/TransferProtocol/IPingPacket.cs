namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Ping packet.
	/// </summary>
	public interface IPingPacket
		: IPacket<IPingPacketHeader>
	{
	}
}