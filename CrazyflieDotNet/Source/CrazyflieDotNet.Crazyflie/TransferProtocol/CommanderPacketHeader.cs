namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class CommanderPacketHeader
		: OutputPacketHeader, ICommanderPacketHeader
	{
		public CommanderPacketHeader(byte headerByte)
			: base(headerByte)
		{
		}

		public CommanderPacketHeader(CommunicationChannel channel = DefaultChannel)
			: base(CommunicationPort.Commander, channel)
		{
		}
	}
}