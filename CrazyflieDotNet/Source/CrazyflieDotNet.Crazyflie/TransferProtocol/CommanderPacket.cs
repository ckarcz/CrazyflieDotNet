namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class CommanderPacket
		: OutputPacket<ICommanderPacketHeader, ICommanderPacketPayload>, ICommanderPacket
	{
		public CommanderPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		public CommanderPacket(ICommanderPacketHeader header, ICommanderPacketPayload payload)
			: base(header, payload)
		{
		}

		public CommanderPacket(float roll, float pitch, float yaw, ushort thrust, Channel channel = OutputPacketHeader.DefaultChannel)
			: this(new CommanderPacketHeader(channel), new CommanderPacketPayload(roll, pitch, yaw, thrust))
		{
		}

		protected override ICommanderPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new CommanderPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return null;
		}

		protected override ICommanderPacketPayload ParsePayload(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetPayload = new CommanderPacketPayload(packetBytes);
				return packetPayload;
			}

			return null;
		}
	}
}