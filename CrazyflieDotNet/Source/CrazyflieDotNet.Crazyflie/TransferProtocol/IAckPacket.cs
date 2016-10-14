namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IAckPacket<TPacketPayload>
		: IPacket<IAckPacketHeader, TPacketPayload> where TPacketPayload : IProvideBytes
	{
	}

	public interface IAckPacket
	: IPacket<IAckPacketHeader>
	{
	}
}