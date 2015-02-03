namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IPacket<out TPacketHeader, out TPacketPayload>
		: IPacket<TPacketHeader>
		where TPacketHeader : IPacketHeader
		where TPacketPayload : IPacketPayload
	{
		new TPacketPayload Payload { get; }
	}

	public interface IPacket<out TPacketHeader>
		: IPacket
		where TPacketHeader : IPacketHeader
	{
		new TPacketHeader Header { get; }
	}

	public interface IPacket
	{
		IPacketHeader Header { get; }

		IPacketPayload Payload { get; }

		byte[] GetBytes();
	}
}