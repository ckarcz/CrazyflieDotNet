#region Imports

using System;
using CrazyflieDotNet.Crazyradio.Driver;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class CrazyradioMessenger
		: ICrazyflieMessenger
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

		#region ICrazyflieMessenger Members

		public IAckPacket SendMessage(IPacket packet)
		{
			var packetBytes = packet.GetBytes();
			var responseBytes = _crazyradioDriver.SendData(packetBytes);

			return new AckPacket(responseBytes);
		}

		#endregion
	}
}