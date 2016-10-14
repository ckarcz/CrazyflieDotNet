using System;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public sealed class PingPacket
		: Packet<IPingPacketHeader, IProvideBytes>, IPingPacket
	{
		private static PingPacket _instance;

		public PingPacket()
			: base(new PingPacketHeader(0xff), null)
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

		protected override IProvideBytes ParsePayload(byte[] packetBytes)
		{
			return null;
		}

		public override string ToString()
		{
			return string.Format("Null/Ping Packet (0xff). Bytes: {0}.", BitConverter.ToString(GetBytes()));
		}

		public static PingPacket Instance
		{
			get { return _instance ?? (_instance = new PingPacket()); }
		}
	}
}