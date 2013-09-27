namespace CrazyflieDotNet.Crazyradio.CRTP
{
	public interface ICRTPPacket
	{
		byte Header { get; }

		byte[] DataBytes { get; }

		byte[] PacketBytes { get; }
	}
}