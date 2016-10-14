using System.Linq;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public delegate TAckPacketPayload BuildAckPayload<TAckPacketPayload>(byte[] payloadBytes);

	/// <summary>
	/// Ack packet with header but no payload.
	/// </summary>
	public class AckPacket
		: AckPacket<IProvideBytes>, IAckPacket
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket"/> class.
		/// </summary>
		/// <param name="header">Header.</param>
		public AckPacket(IAckPacketHeader header)
			: base(header, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket"/> class.
		/// </summary>
		/// <param name="packetBytes">Packet bytes.</param>
		public AckPacket(byte[] packetBytes)
			: base(packetBytes, null)
		{
		}
	}

	/// <summary>
	/// Ack packet with header and payload.
	/// </summary>
	public class AckPacket<TAckPacketPayload>
		: Packet<IAckPacketHeader, TAckPacketPayload>, IAckPacket<TAckPacketPayload> where TAckPacketPayload : IProvideBytes
	{
		private readonly BuildAckPayload<TAckPacketPayload> ackPayloadBuilder;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket`1"/> class.
		/// </summary>
		/// <param name="header">Ack packet header.</param>
		/// <param name="ackPayloadBuilder">Ack payload builder/delegate.</param>
		public AckPacket(IAckPacketHeader header, BuildAckPayload<TAckPacketPayload> ackPayloadBuilder)
			: base(header, default(TAckPacketPayload))
		{
			if (ackPayloadBuilder == null)
			{
				ackPayloadBuilder = (arg) => default(TAckPacketPayload);
			}

			this.ackPayloadBuilder = ackPayloadBuilder;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket`1"/> class.
		/// </summary>
		/// <param name="packetBytes">Ack packet bytes.</param>
		/// <param name="ackPayloadBuilder">Ack payload builder/delegate.</param>
		public AckPacket(byte[] packetBytes, BuildAckPayload<TAckPacketPayload> ackPayloadBuilder)
			: base(packetBytes)
		{
			if (ackPayloadBuilder == null)
			{
				ackPayloadBuilder = (arg) => default(TAckPacketPayload);
			}

			this.ackPayloadBuilder = ackPayloadBuilder;
		}

		/// <summary>
		/// Parses the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override IAckPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new AckPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return default(IAckPacketHeader);
		}

		/// <summary>
		/// Parses the payload.
		/// </summary>
		/// <returns>The payload.</returns>
		/// <param name="packetBytes">Packet bytes.</param>
		protected override TAckPacketPayload ParsePayload(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length > 0)
			{
				var packetHeader = ackPayloadBuilder(packetBytes.Skip(1).ToArray());
				return packetHeader;
			}

			return default(TAckPacketPayload);
		}
	}
}