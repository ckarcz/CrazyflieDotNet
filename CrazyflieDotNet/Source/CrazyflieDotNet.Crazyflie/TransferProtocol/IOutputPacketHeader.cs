namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IOutputPacketHeader
		: IPacketHeader
	{
		CommunicationPort Port { get; }
		CommunicationChannel Channel { get; }
	}
}