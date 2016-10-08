#region Imports

using System;
using CrazyflieDotNet.Crazyradio.Driver;
using log4net;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public class CrazyflieMessenger
		: ICrazyflieMessenger
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CrazyflieMessenger));
		private readonly ICrazyradioDriver _crazyradioDriver;

		public CrazyflieMessenger(ICrazyradioDriver crazyradioDriver)
		{
			if (crazyradioDriver == null)
			{
				throw new ArgumentNullException(nameof(crazyradioDriver));
			}

			_crazyradioDriver = crazyradioDriver;

			Log.DebugFormat("Initialized with CrazyradioDriver instance {0}.", crazyradioDriver);
		}

		#region ICrazyflieMessenger Members

		public IAckPacket SendMessage(IPacket packet)
		{
			Log.InfoFormat("Sending message... Packet: {0}", packet);

			var packetBytes = packet.GetBytes();
			var responseBytes = _crazyradioDriver.SendData(packetBytes);
			var ackResponse = new AckPacket(responseBytes);

			Log.InfoFormat("Sent message... Response: {0}", ackResponse);

			return ackResponse;
		}

		#endregion
	}
}