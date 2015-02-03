namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IOutputPacketHeader
		: IPacketHeader
	{
		Port Port { get; }

		Channel Channel { get; }
	}
}