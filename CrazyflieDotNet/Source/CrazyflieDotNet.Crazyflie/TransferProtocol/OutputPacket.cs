namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Output packet.
	/// </summary>
	public abstract class OutputPacket<TOutputPacketHeader, TOutputPacketPayload>
		: Packet<TOutputPacketHeader, TOutputPacketPayload>, IOutputPacket<TOutputPacketHeader, TOutputPacketPayload> where TOutputPacketHeader : IOutputPacketHeader where TOutputPacketPayload : IOutputPacketPayload
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.OutputPacket`2"/> class.
		/// </summary>
		/// <param name="packetBytes">Packet bytes.</param>
		protected OutputPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CrazyflieDotNet.Crazyflie.TransferProtocol.OutputPacket`2"/> class.
		/// </summary>
		/// <param name="header">Header.</param>
		/// <param name="payload">Payload.</param>
		protected OutputPacket(TOutputPacketHeader header, TOutputPacketPayload payload)
			: base(header, payload)
		{
		}
	}
}