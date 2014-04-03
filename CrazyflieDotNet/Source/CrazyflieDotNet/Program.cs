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
	internal class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CrazyradioDriver));

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

					var scanResults = crazyradioDriver.ScanChannels();
					if (scanResults.Any())
					{
						var firstScanResult = scanResults.First();

						var dataRateWithCrazyflie = firstScanResult.DataRate;
						var channelWithCrazyflie = firstScanResult.Channels.First();

						crazyradioDriver.DataRate = dataRateWithCrazyflie;
						crazyradioDriver.Channel = channelWithCrazyflie;

						var crazyRadioMessenger = new CrazyradioMessenger(crazyradioDriver);

						var loop = true;
						while (loop)
						{
							var pingPacket = new PingPacket();
							var pingPacketBytes = pingPacket.GetBytes();

							Log.InfoFormat("Ping Packet Bytes: {0}", BitConverter.ToString(pingPacketBytes));

							var ackResponse = crazyradioDriver.SendData(pingPacketBytes);

							Log.InfoFormat("ACK Response Bytes: {0}", BitConverter.ToString(ackResponse));

							crazyRadioMessenger.SendMessage(new PingPacket());

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