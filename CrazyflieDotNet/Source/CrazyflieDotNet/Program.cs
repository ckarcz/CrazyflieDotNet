#region Imports

using CrazyflieDotNet.Crazyradio;
using log4net;
using log4net.Config;
using System;
using System.Linq;
using System.Threading;

#endregion Imports

namespace CrazyflieDotNet
{
	internal class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CrazyradioDriver));

		private static void Main(string[] args)
		{
			SetUpLogging();

			try
			{
				Log.Debug("Starting Crazyradio USB dongle tests.");

				var crazyradios = CrazyradioDriver.GetCrazyradios();

				if (crazyradios.Any())
				{
					var crazyradio = crazyradios.First();

					try
					{
						crazyradio.Open();

						var scanResults = crazyradio.ScanChannels();

						if (scanResults.Any())
						{
							var dataRateWithCrazyflie = scanResults.First().DataRate;
							var channelWithCrazyflie = scanResults.First().Channels.First();

							crazyradio.DataRate = dataRateWithCrazyflie;
							crazyradio.Channel = channelWithCrazyflie;

							var packet = new byte[] {0xFF};
							var loop = true;

							while (loop)
							{
								var results = crazyradio.SendPacket(packet);
								Log.InfoFormat("Packet Result: {0}", BitConverter.ToString(results));

								if (Console.ReadKey().Key == ConsoleKey.Spacebar)
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
					finally
					{
						crazyradio.Close();
					}
				}
				else
				{
					Log.Warn("No Crazyradio USB dongles found!");
				}
			}
			catch (Exception ex)
			{
				Log.Error("Unhandled exception occured!", ex);
			}

			Thread.Sleep(Timeout.Infinite);
		}

		private static void SetUpLogging()
		{
			BasicConfigurator.Configure();
		}
	}
}