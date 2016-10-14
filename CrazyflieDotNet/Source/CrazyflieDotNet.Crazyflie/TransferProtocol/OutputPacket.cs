namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class OutputPacket<TOutputPacketHeader, TOutputPacketPayload>
		: Packet<TOutputPacketHeader, TOutputPacketPayload>, IOutputPacket<TOutputPacketHeader, TOutputPacketPayload> where TOutputPacketHeader : IOutputPacketHeader where TOutputPacketPayload : IOutputPacketPayload
	{
		protected OutputPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		protected OutputPacket(TOutputPacketHeader header, TOutputPacketPayload payload)
			: base(header, payload)
		{
		}
	}
}