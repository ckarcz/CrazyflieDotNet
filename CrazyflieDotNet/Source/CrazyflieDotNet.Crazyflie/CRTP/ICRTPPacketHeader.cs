using CrazyflieDotNet.Crazyradio;

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public interface ICRTPPacketHeader
	{
		RadioChannel Channel { get; }

		CRTPPort Port { get; }

		byte HeaderByte { get; }
	}
}