namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface ICrazyflieMessenger
	{
		IAckPacket SendMessage(IPacket packet);
	}
}