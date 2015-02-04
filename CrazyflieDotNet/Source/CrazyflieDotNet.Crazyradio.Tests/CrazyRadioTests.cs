#region Imports

using System;
using System.Collections.Generic;
using System.Linq;
using CrazyflieDotNet.Crazyflie.TransferProtocol;
using CrazyflieDotNet.Crazyradio.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace CrazyflieDotNet.Crazyradio.Tests
{
	/// <summary>
	///     Test class with main purpose to show how to use the driver abstraction for the crazyradio.
	/// </summary>
	[TestClass]
	public class CrazyRadioTests
	{
		private ICrazyradioDriver crazyradioDriver = null;
		private IEnumerable<ICrazyradioDriver> crazyradioDrivers = null;

		/// <summary>
		///     This method initializes the GetCrazyRadio scan collection for uses in later tests.
		/// </summary>
		[TestInitialize]
		public void Initailize()
		{
			crazyradioDrivers = null;

			try
			{
				// Scan and return a collection of Crazyradio USB dongles connected to the computer.
				crazyradioDrivers = CrazyradioDriver.GetCrazyradios();

				if (crazyradioDrivers == null || !crazyradioDrivers.Any())
				{
					throw new Exception("No Crazyradio USB dongle devices found using GetCrazyradio method call.");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error getting Crazyradios.", ex);
			}
		}

		/// <summary>
		///     This test method tests selecting a crazyflie quadcopter to communication to by using the first communication tuple
		///     (dataRate + radioChannel), opens the CrazyradioDriver for communication, and attempts to ping the copter 10 times.
		/// </summary>
		[TestMethod]
		public void TestPingCopter()
		{
			if (crazyradioDrivers != null && crazyradioDrivers.Any())
			{
				// Use the first available Crazyradio USB dongle returned from the scan in the initialize test methods call.
				var crazyradioDriver = crazyradioDrivers.First();

				try
				{
					// Open the Crazyradio USB dongle device for communication and configuration.
					crazyradioDriver.Open();

					// Get Crazyradio USB dongle to scann for communication channels of Crazyflie quadcopters.
					var scanResults = crazyradioDriver.ScanChannels();
					if (scanResults.Any())
					{
						// Pick the first result...
						var firstScanResult = scanResults.First();

						// Results are grouped by DataRate...
						var dataRateWithCrazyflie = firstScanResult.DataRate;
						// Pick the first communication RadioChannel...
						var channelWithCrazyflie = firstScanResult.Channels.First();

						// Set the CrazyradioDriver to use the above communication RadioChannel and DataRate to communicate with the Crazyflie quadcopter using that DataRate and RadioChannel...
						crazyradioDriver.DataRate = dataRateWithCrazyflie;
						crazyradioDriver.Channel = channelWithCrazyflie;

						// Create a ping packet to set to the Crazyflie we're going to ping 10 times.
						// We can reuse this same ping packet object. No point in creating a new one for each ping.
						var pingPacket = new PingPacket();

						// Get ping packet bytes just to print out.
						var pingPacketBytes = pingPacket.GetBytes();

						int i = 0;
						while (i < 10)
						{
							// Print out the ping packet bytes.
							Console.WriteLine("Ping Packet Bytes: {0}", BitConverter.ToString(pingPacketBytes));

							// Send the ping packet bytes via the CrazyradioDriver and get ACK (acknowledgement) response bytes.
							var ackResponse = crazyradioDriver.SendData(pingPacketBytes);

							// Print out the ACK response bytes.
							Console.WriteLine("ACK Response Bytes (using driver): {0}", BitConverter.ToString(ackResponse));

							// one more time!
							i++;
						}
					}
					else
					{
						throw new Exception("No Crazyflie Quadcopters found!");
					}
				}
				catch (Exception ex)
				{
					throw new Exception("Error testing Crazyradio.", ex);
				}
				finally
				{
					// Close the Crazyradio USB dongle for communication.
					crazyradioDriver.Close();
				}
			}
			else
			{
				throw new Exception("No Crazyradio USB dongles found!");
			}

			Assert.IsTrue(true);
		}
	}
}