namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Ack packet.
	/// </summary>
	public interface IAckPacket<TPacketPayload>
		: IAckPacket, IPacket<IAckPacketHeader, TPacketPayload> where TPacketPayload : IProvideBytes
	{
	}

	/// <summary>
	/// Ack packet.
	/// </summary>
	public interface IAckPacket
	: IPacket<IAckPacketHeader>
	{
	}
}