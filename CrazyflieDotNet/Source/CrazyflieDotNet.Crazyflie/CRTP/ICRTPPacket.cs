namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public interface ICRTPPacket
	{
		ICRTPPacketHeader Header { get; }

		byte[] DataBytes { get; }

		byte[] FullPacketBytes { get; }
	}
}