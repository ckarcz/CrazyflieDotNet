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
			var packetBytes = packet.GetBytes();

			Log.InfoFormat("Sending packet {0} (bytes: {1})", packet, BitConverter.ToString(packetBytes));

			var responseBytes = _crazyradioDriver.SendData(packetBytes);

			if (responseBytes != null)
			{
				var ackResponse = new AckPacket(responseBytes);

				Log.InfoFormat("Sent packet. Got ACK response {0} (bytes: {1})", ackResponse, responseBytes);

				return ackResponse;
			}
			else
			{
				Log.Warn("Sent packet. Got NULL response.");

				return null;
			}
		}

		#endregion
	}
}