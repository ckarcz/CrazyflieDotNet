namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface ICommanderPacket
		: IOutputPacket<ICommanderPacketHeader, ICommanderPacketPayload>
	{
	}
}