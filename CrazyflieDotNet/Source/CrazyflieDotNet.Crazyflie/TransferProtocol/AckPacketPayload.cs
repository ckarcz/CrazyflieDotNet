namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class AckPacketPayload<TResponse>
		: PacketPayload where TResponse : IProvideBytes
	{
		public AckPacketPayload(TResponse response)
		{
			Response = response;
		}

		public TResponse Response { get; private set; }

		protected override byte[] GetPacketPayloadBytes()
		{
			return Response != null ? Response.GetBytes() : null;
		}
	}
}