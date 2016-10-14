namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Commander packet.
	/// </summary>
	public interface ICommanderPacket
		: IOutputPacket<ICommanderPacketHeader, ICommanderPacketPayload>
	{
	}
}