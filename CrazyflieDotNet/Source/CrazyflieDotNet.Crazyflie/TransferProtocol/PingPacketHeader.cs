namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class PingPacketHeader
		: OutputPacketHeader, IPingPacketHeader
	{
		public PingPacketHeader(byte headerByte)
			: base(headerByte)
		{
		}

		public PingPacketHeader(CommunicationChannel channel = DefaultChannel)
			: this(CommunicationPort.All, channel)
		{
		}

		public PingPacketHeader(CommunicationPort port, CommunicationChannel channel = DefaultChannel)
			: base(port, channel)
		{
		}
	}
}