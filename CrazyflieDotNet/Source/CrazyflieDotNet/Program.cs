/* 
 *						 _ _  _     
 *		       ____ ___  ___  __________(_|_)(_)____
 *		      / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *		     / / / / / /  __(__  |__  ) /  __/ /    
 *		    /_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *	     Copyright 2013 - Messier/Chris Karcz - ckarcz@gmail.com
 *
 *	This Source Code Form is subject to the terms of the Mozilla Public
 *	License, v. 2.0. If a copy of the MPL was not distributed with this
 *	file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#region Imports

using CrazyflieDotNet.Crazyflie.CRTP;
using CrazyflieDotNet.Crazyradio;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
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

			IEnumerable<ICrazyradioDriver> crazyradios = null;

			try
			{
				Log.Debug("Starting Crazyradio USB dongle tests.");

				crazyradios = CrazyradioDriver.GetCrazyradios();
			}
			catch (Exception ex)
			{
				Log.Error("Error getting Crazyradios.", ex);
			}

			if (crazyradios != null && crazyradios.Any())
			{
				var crazyradio = crazyradios.First();

				try
				{
					crazyradio.Open();

					var scanResults = crazyradio.ScanChannels();
					if (scanResults.Any())
					{
						var firstScanResult = scanResults.First();

						var dataRateWithCrazyflie = firstScanResult.DataRate;
						var channelWithCrazyflie = firstScanResult.Channels.First();

						crazyradio.DataRate = dataRateWithCrazyflie;
						crazyradio.Channel = channelWithCrazyflie;

						var loop = true;
						while (loop)
						{
							var pingPacket = new CRTPPingPacket();
							var pingPacketBytes = pingPacket.PacketBytes;

							Log.InfoFormat("Ping Packet Bytes: {0}", BitConverter.ToString(pingPacketBytes));

							var ackResponse = crazyradio.SendData(pingPacketBytes);

							Log.InfoFormat("ACK Response Bytes: {0}", BitConverter.ToString(ackResponse));

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
				catch (Exception ex)
				{
					Log.Error("Error testing Crazyradio.", ex);
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

			Log.Info("Sleepy time...");

			Thread.Sleep(Timeout.Infinite);
		}

		private static void SetUpLogging()
		{
			BasicConfigurator.Configure();
		}
	}
}