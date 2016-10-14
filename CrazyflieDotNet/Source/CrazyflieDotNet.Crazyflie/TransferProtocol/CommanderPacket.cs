using System;
using System.Linq;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Commander packet.
	/// </summary>
	public class CommanderPacket
		: OutputPacket<ICommanderPacketHeader, ICommanderPacketPayload>, ICommanderPacket
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacket"/> class.
		/// </summary>
		/// <param name="packetBytes">Packet bytes.</param>
		public CommanderPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacket"/> class.
		/// </summary>
		/// <param name="header">Header.</param>
		/// <param name="payload">Payload.</param>
		public CommanderPacket(ICommanderPacketHeader header, ICommanderPacketPayload payload)
			: base(header, payload)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.CommanderPacket"/> class.
		/// </summary>
		/// <param name="roll">Roll.</param>
		/// <param name="pitch">Pitch.</param>
		/// <param name="yaw">Yaw.</param>
		/// <param name="thrust">Thrust.</param>
		/// <param name="channel">Channel.</param>
		public CommanderPacket(float roll, float pitch, float yaw, ushort thrust, CommunicationChannel channel = OutputPacketHeader.DefaultChannel)
			: this(new CommanderPacketHeader(channel), new CommanderPacketPayload(roll, pitch, yaw, thrust))
		{
		}

		/// <summary>
		/// Parses the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override ICommanderPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new CommanderPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return null;
		}

		/// <summary>
		/// Parses the payload.
		/// </summary>
		/// <returns>The payload.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override ICommanderPacketPayload ParsePayload(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetPayload = new CommanderPacketPayload(packetBytes.Skip(1).ToArray());
				return packetPayload;
			}

			return null;
		}
	}
}