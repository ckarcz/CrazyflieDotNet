using System.Collections.Generic;

namespace CrazyflieDotNet.Crazyradio.Driver
{
	public interface IRadioPacket
	{
		
	}

	public interface IRadioAck
	{
		bool PacketRecieved { get; }

		bool PowerDet { get; }

		int Rety { get; }

		IEnumerable<byte> Data { get; }
	}
}