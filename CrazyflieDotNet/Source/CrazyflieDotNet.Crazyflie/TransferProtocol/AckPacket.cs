using System.Linq;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public delegate TAckPacketPayload CreateAckPayload<TAckPacketPayload>(byte[] payloadBytes);

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
		private readonly CreateAckPayload<TAckPacketPayload> ackPayloadBuilder;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket`1"/> class.
		/// </summary>
		/// <param name="header">Ack packet header.</param>
		/// <param name="payloadFactory">Ack payload builder/delegate.</param>
		public AckPacket(IAckPacketHeader header, CreateAckPayload<TAckPacketPayload> payloadFactory)
			: base(header, default(TAckPacketPayload))
		{
			if (payloadFactory == null)
			{
				payloadFactory = (arg) => default(TAckPacketPayload);
			}

			this.ackPayloadBuilder = payloadFactory;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.AckPacket`1"/> class.
		/// </summary>
		/// <param name="packetBytes">Ack packet bytes.</param>
		/// <param name="payloadFactory">Ack payload builder/delegate.</param>
		public AckPacket(byte[] packetBytes, CreateAckPayload<TAckPacketPayload> payloadFactory)
			: base(packetBytes)
		{
			if (payloadFactory == null)
			{
				payloadFactory = (arg) => default(TAckPacketPayload);
			}

			this.ackPayloadBuilder = payloadFactory;
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
				var packetHeader = new AckPacketHeader(packetBytes.First());
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
			if (packetBytes != null && ackPayloadBuilder != null && packetBytes.Length > 1)
			{
				var packetHeader = ackPayloadBuilder(packetBytes.Skip(1).ToArray());
				return packetHeader;
			}

			return default(TAckPacketPayload);
		}
	}
}