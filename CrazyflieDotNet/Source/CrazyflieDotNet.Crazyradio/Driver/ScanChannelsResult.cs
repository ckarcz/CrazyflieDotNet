#region Imports

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	/// The result type returned once the Crazyradio USB dongle completed scanning channels for transmission to Crazyflies.
	/// The data contained in an object of this class could mean there are more than one available Crazyflies to communicate with on various channels.
	/// </summary>
	public sealed class ScanChannelsResult : IEquatable<ScanChannelsResult>
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
		public RadioDataRate DataRate { get; }

		/// <summary>
		/// Collections of channels for available communication with Crazyflie quadcopters.
		/// </summary>
		public IEnumerable<RadioChannel> Channels { get; }

		public bool Equals(ScanChannelsResult other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return DataRate == other.DataRate && Channels.SequenceEqual(other.Channels);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			return obj is ScanChannelsResult && Equals((ScanChannelsResult) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int) DataRate * 397) ^ (Channels != null ? Channels.GetHashCode() : 0);
			}
		}

		public static bool operator ==(ScanChannelsResult left, ScanChannelsResult right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ScanChannelsResult left, ScanChannelsResult right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			return $"[DataRate: {DataRate}, Channels: {Channels}]";
		}
	}
}