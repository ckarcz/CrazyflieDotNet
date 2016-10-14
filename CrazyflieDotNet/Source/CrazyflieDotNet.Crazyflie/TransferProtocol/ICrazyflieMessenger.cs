using System;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface ICrazyflieMessenger
	{
		IAckPacket SendMessage<TPacket>(TPacket packet) where TPacket : IProvideBytes;

		IAckPacket<TPacket> SendMessage<TPacket>(TPacket packet, Func<byte[], TPacket> responseBuilder) where TPacket : IProvideBytes;

	}
}