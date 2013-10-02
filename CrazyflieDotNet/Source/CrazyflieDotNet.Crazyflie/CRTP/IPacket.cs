namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public interface IPacket
	{
		PacketHeader Header { get; }

		byte[] Data { get; }

		byte[] FullPacketBytes { get; }
	}

	public class Packet
		: IPacket
	{
		public PacketHeader Header { get; private set; }

		public byte[] Data { get; private set; }

		public byte[] FullPacketBytes { get; private set; }
	}
}