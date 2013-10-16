/* 
 *										 _ _  _     
 *			   ____ ___  ___  __________(_|_)(_)____
 *			  / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *			 / / / / / /  __(__  |__  ) /  __/ /    
 *			/_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *				Copyright 2013 - http://www.messier.com
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#region Imports

using CrazyflieDotNet.Crazyflie.CRTP;
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
		private static readonly ILog Log = LogManager.GetLogger(typeof(CrazyradioDriver));

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

							var loop = true;

							while (loop)
							{
								var packet = CRTPDataPacket.PingPacket.PacketBytes;

								Log.InfoFormat("Packet Result: {0}", BitConverter.ToString(packet));

								var results = crazyradio.SendData(packet);

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