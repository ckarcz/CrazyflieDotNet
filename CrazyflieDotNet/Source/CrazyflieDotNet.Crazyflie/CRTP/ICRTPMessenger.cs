namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public interface ICRTPMessenger
	{
		CRTPPacket SendMessage(CRTPPacket packet);
	}
}