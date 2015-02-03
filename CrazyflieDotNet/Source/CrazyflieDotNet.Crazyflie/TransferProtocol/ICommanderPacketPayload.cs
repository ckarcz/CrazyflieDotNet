namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface ICommanderPacketPayload
		: IOutputPacketPayload
	{
		float Roll { get; }

		float Pitch { get; }

		float Yaw { get; }

		ushort Thurst { get; }
	}
}