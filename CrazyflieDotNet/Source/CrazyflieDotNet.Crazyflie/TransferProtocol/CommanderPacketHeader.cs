namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class CommanderPacketHeader
		: OutputPacketHeader, ICommanderPacketHeader
	{
		public CommanderPacketHeader(byte headerByte)
			: base(headerByte)
		{
		}

		public CommanderPacketHeader(Channel channel = DefaultChannel)
			: base(Port.Commander, channel)
		{
		}
	}
}