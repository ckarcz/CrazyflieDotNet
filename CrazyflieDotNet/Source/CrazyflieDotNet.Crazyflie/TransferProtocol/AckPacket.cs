using System;
using System.Linq;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class AckPacket
		: Packet<IAckPacketHeader>, IAckPacket
	{

		public AckPacket(IAckPacketHeader header)
			: base(header)
		{
		}

		public AckPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		protected override IAckPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new AckPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return default(IAckPacketHeader);
		}

		public override string ToString()
		{
			return string.Format("AckReceived: {0}, PowerDetector: {1}, RetryCount: {2}. Bytes: {3}.", Header.AckRecieved, Header.PowerDetector, Header.RetryCount, BitConverter.ToString(GetBytes()));
		}
	}

	public class AckPacket<TResponse>
		: Packet<IAckPacketHeader, TResponse>, IAckPacket<TResponse> where TResponse : IProvideBytes
	{
		readonly Func<byte[], TResponse> ackPayloadBuilder;

		public AckPacket(IAckPacketHeader header, Func<byte[], TResponse> ackPayloadBuilder)
			: base(header, default(TResponse))
		{
			if (ackPayloadBuilder == null)
			{
				ackPayloadBuilder = (arg) => default(TResponse);
			}

			this.ackPayloadBuilder = ackPayloadBuilder;
		}

		public AckPacket(byte[] packetBytes, Func<byte[], TResponse> ackPayloadBuilder)
			: base(packetBytes)
		{
			if (ackPayloadBuilder == null)
			{
				ackPayloadBuilder = (arg) => default(TResponse);
			}

			this.ackPayloadBuilder = ackPayloadBuilder;
		}

		protected override IAckPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new AckPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return default(IAckPacketHeader);
		}

		protected override TResponse ParsePayload(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length > 0)
			{
				var packetHeader = ackPayloadBuilder(packetBytes.Skip(1).ToArray());
				return packetHeader;
			}

			return default(TResponse);
		}

		public override string ToString()
		{
			return string.Format("AckReceived: {0}, PowerDetector: {1}, RetryCount: {2}. Bytes: {3}.", Header.AckRecieved, Header.PowerDetector, Header.RetryCount, BitConverter.ToString(GetBytes()));
		}
	}
}