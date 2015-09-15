using System;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public sealed class PingPacketHeader
		: PacketHeader, IPingPacketHeader
	{
        private readonly byte pingPacketHeaderByte = 0xff;

        public PingPacketHeader(byte headerByte)
		{
            pingPacketHeaderByte = headerByte;
        }

        protected override byte? GetPacketHeaderByte()
        {
            return pingPacketHeaderByte;
        }
    }
}