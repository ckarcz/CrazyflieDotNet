namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class PingPacket
		: OutputPacket<IPingPacketHeader>, IPingPacket
	{
		public PingPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		public PingPacket(IPingPacketHeader header)
			: base(header)
		{
		}

		public PingPacket(Channel channel = Channel.Channel0)
			: this(new PingPacketHeader(channel))
		{
		}

		protected override IPingPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new PingPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return null;
		}
	}
}