#region Imports

using System;
using CrazyflieDotNet.Crazyradio.Driver;
using log4net;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	/// <summary>
	/// Crazyflie messenger.
	/// </summary>
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

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="packet">Packet.</param>
		/// <typeparam name="TPacket">The 1st type parameter.</typeparam>
		public IAckPacket SendMessage<TPacket>(TPacket packet) where TPacket : IProvideBytes
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

		/// <summary>
		/// Sends the message and returns an ack with payload.
		/// </summary>
		/// <returns>The message.</returns>
		/// <param name="packet">Packet.</param>
		/// <param name="createAckPayload">Ack payload builder/delegate.</param>
		/// <typeparam name="TPacketPayload">Ack pa.</typeparam>
		public IAckPacket<TAckPacketPayload> SendMessage<TAckPacketPayload>(IPacket packet, CreateAckPayload<TAckPacketPayload> createAckPayload) where TAckPacketPayload : IProvideBytes
		{
			var packetBytes = packet.GetBytes();

			Log.InfoFormat("Sending packet {0} (bytes: {1})", packet, BitConverter.ToString(packetBytes));

			var responseBytes = _crazyradioDriver.SendData(packetBytes);

			if (responseBytes != null)
			{
				var ackResponse = new AckPacket<TAckPacketPayload>(responseBytes, createAckPayload);

				Log.InfoFormat("Sent packet. Got ACK response {0} (bytes: {1})", ackResponse, responseBytes);

				return ackResponse;
			}
			else
			{
				Log.Warn("Sent packet. Got NULL response.");

				return null;
			}
		}
	}
}