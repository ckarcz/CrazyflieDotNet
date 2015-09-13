#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using CrazyflieDotNet.Crazyflie.TransferProtocol;
using CrazyflieDotNet.Crazyradio.Driver;
using log4net;
using log4net.Config;

#endregion

namespace CrazyflieDotNet
{
	/// <summary>
	///     Currently, this Program is only a small Test like executable for testing during development.
	/// </summary>
	internal class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

		private static void Main(string[] args)
		{
			SetUpLogging();

			IEnumerable<ICrazyradioDriver> crazyradioDrivers = null;

			try
			{
				Log.Debug("Starting Crazyradio USB dongle tests.");

				crazyradioDrivers = CrazyradioDriver.GetCrazyradios();
			}
			catch (Exception ex)
			{
				Log.Error("Error getting Crazyradios.", ex);
			}

			if (crazyradioDrivers != null && crazyradioDrivers.Any())
			{
				var crazyradioDriver = crazyradioDrivers.First();

				try
				{
					crazyradioDriver.Open();

					var scanResults = crazyradioDriver.ScanChannels(RadioChannel.Channel0, RadioChannel.Channel125);
					if (scanResults.Any())
					{
						var firstScanResult = scanResults.First();

						var dataRateWithCrazyflie = firstScanResult.DataRate;
						var channelWithCrazyflie = firstScanResult.Channels.First();

						crazyradioDriver.DataRate = dataRateWithCrazyflie;
						crazyradioDriver.Channel = channelWithCrazyflie;

						var pingPacket = new PingPacket();
						var pingPacketBytes = pingPacket.GetBytes();

						var crazyRadioMessenger = new CrazyradioMessenger(crazyradioDriver);

						var loop = true;
						while (loop)
						{
							// test 2 (using CRTP lib)
							{
								Log.InfoFormat("Ping Packet Bytes: {0}", BitConverter.ToString(pingPacketBytes));

								var ackPacket = crazyRadioMessenger.SendMessage(new PingPacket());
								var ackPacketBytes = ackPacket.GetBytes();

								Log.InfoFormat("ACK Response Bytes (using CTRP): {0}", BitConverter.ToString(ackPacketBytes));
							}

							if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Spacebar)
							{
								loop = false;
							}
						}
					}
					else
					{
						Log.Warn("No Crazyflie Quadcopters found!");
					}
				}
				catch (Exception ex)
				{
					Log.Error("Error testing Crazyradio.", ex);
				}
				finally
				{
					crazyradioDriver.Close();
				}
			}
			else
			{
				Log.Warn("No Crazyradio USB dongles found!");
			}

			Log.Info("Sleepy time...Hit space to exit.");

			var sleep = true;
			while (sleep)
			{
				if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Spacebar)
				{
					sleep = false;
				}
			}
		}

		private static void SetUpLogging()
		{
			BasicConfigurator.Configure();
		}
	}
}
