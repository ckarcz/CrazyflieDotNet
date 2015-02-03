namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class PingPacketHeader
		: OutputPacketHeader, IPingPacketHeader
	{
		public PingPacketHeader(byte headerByte)
			: base(headerByte)
		{
		}

		public PingPacketHeader(Channel channel = DefaultChannel)
			: this(Port.All, channel)
		{
		}

		public PingPacketHeader(Port port, Channel channel = DefaultChannel)
			: base(port, channel)
		{
		}
	}
}