namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Output packet.
	/// </summary>
	public interface IOutputPacket<out TDataPacketHeader, out TDataPacketPayload>
		: IPacket<TDataPacketHeader, TDataPacketPayload> where TDataPacketHeader : IOutputPacketHeader where TDataPacketPayload : IOutputPacketPayload
	{
	}

	/// <summary>
	/// Output packet.
	/// </summary>
	public interface IOutputPacket<out TDataPacketHeader>
		: IOutputPacket, IPacket<TDataPacketHeader>
		where TDataPacketHeader : IOutputPacketHeader
	{
	}

	/// <summary>
	/// Output packet.
	/// </summary>
	public interface IOutputPacket
		: IPacket
	{
	}
}