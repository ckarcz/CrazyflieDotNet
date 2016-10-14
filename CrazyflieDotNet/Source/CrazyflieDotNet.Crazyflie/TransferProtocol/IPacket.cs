namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IPacket<out TPacketHeader, out TPacketPayload>
		: IPacket<TPacketHeader> where TPacketHeader : IProvideBytes where TPacketPayload : IProvideBytes
	{
		new TPacketPayload Payload { get; }
	}

	public interface IPacket<out TPacketHeader>
		: IPacket where TPacketHeader : IProvideBytes
	{
		new TPacketHeader Header { get; }
	}

	public interface IPacket
		: IProvideBytes
	{
		IProvideBytes Header { get; }
		IProvideBytes Payload { get; }
	}
}