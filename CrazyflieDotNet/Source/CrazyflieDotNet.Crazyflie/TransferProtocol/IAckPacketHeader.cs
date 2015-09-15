using CrazyflieDotNet.Crazyradio.Driver;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IAckPacketHeader
		: IPacketHeader
	{
        bool AckRecieved { get; }

        bool PowerDetector { get; }

        int RetryCount { get; }
    }
}