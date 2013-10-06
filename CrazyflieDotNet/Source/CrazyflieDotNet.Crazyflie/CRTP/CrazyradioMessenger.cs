#region Imports

using CrazyflieDotNet.Crazyradio;
using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public class CrazyradioMessenger
		: ICrazyradioMessenger
	{
		private readonly ICrazyradioDriver _crazyradioDriver;

		public CrazyradioMessenger(ICrazyradioDriver crazyradioDriver)
		{
			if (crazyradioDriver == null)
			{
				throw new ArgumentNullException("crazyradioDriver");
			}

			_crazyradioDriver = crazyradioDriver;
		}

		public CRTPPacket SendMessage(CRTPPacket packet)
		{
			var responseBytes = _crazyradioDriver.SendData(packet.PacketBytes);

			return new CRTPPacket(responseBytes);
		}
	}
}