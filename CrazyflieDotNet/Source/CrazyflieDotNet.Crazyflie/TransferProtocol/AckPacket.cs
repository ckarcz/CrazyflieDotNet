using System;

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

			return null;
		}

        public override string ToString()
        {
            return string.Format("AckReceived: {0}, PowerDetector: {1}, RetryCount: {2}. Bytes: {3}.", Header.AckRecieved, Header.PowerDetector, Header.RetryCount, BitConverter.ToString(GetBytes()));
        }
    }
}