#region Imports

using System.Collections.Generic;

#endregion

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	/// The result type returned once the Crazyradio USB dongle completed scanning channels for transmission to Crazyflies.
	/// The data contained in an object of this class could mean there are more than one available Crazyflies to communicate with on various channels.
	/// </summary>
	public sealed class ScanChannelsResult
	{
		/// <summary>
		/// Initializes a scan channel result object containing the DataRate and RadioChannels available for communication with Crazyflie quadcopters.
		/// </summary>
		/// <param name="dataRate">The DataRate of which to use for transmission.</param>
		/// <param name="channels">Collection of RadioChannels available to use for transmission with Crazyflie quadcopters..</param>
		public ScanChannelsResult(RadioDataRate dataRate, IEnumerable<RadioChannel> channels)
		{
			DataRate = dataRate;
			Channels = channels;
		}

		/// <summary>
		/// The DataRate of a available communication RadioChannels for Crazyflie quadcopters.
		/// </summary>
		public RadioDataRate DataRate { get; private set; }

		/// <summary>
		/// Collections of channels for available communication with Crazyflie quadcopters.
		/// </summary>
		public IEnumerable<RadioChannel> Channels { get; private set; }
	}
}