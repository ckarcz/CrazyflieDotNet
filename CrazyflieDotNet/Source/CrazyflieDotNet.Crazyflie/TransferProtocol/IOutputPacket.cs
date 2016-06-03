namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IOutputPacket<out TDataPacketHeader, out TDataPacketPayload>
		: IPacket<TDataPacketHeader, TDataPacketPayload> where TDataPacketHeader : IOutputPacketHeader where TDataPacketPayload : IOutputPacketPayload
	{
	}

	public interface IOutputPacket<out TDataPacketHeader>
		: IOutputPacket, IPacket<TDataPacketHeader>
		where TDataPacketHeader : IOutputPacketHeader
	{
	}

	public interface IOutputPacket
		: IPacket
	{
	}
}