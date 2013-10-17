/* 
 *						 _ _  _     
 *		       ____ ___  ___  __________(_|_)(_)____
 *		      / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *		     / / / / / /  __(__  |__  ) /  __/ /    
 *		    /_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *		    Copyright 2013 - http://www.messier.com
 *
 *	This Source Code Form is subject to the terms of the Mozilla Public
 *	License, v. 2.0. If a copy of the MPL was not distributed with this
 *	file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	/// Represents the firmware version of a Crazyradio USB dongle.
	/// </summary>
	public sealed class FirmwareVersion
		: IEquatable<FirmwareVersion>
	{
		/// <summary>
		/// Initializes an instance of CrazyradioFirmwareVersion given the USB bcdDevice/bcdVersion number.
		/// </summary>
		/// <param name="bcdVersionNumber">The usb bcdDevice/bcdVersion number.</param>
		public FirmwareVersion(short bcdVersionNumber)
			: this((0xFF00 & bcdVersionNumber) >> 8, (0xF0 & bcdVersionNumber) >> 4, 0x0F & bcdVersionNumber)
		{
		}

		/// <summary>
		/// Initializes an instance of CrazyradioFirmwareVersion given the three version parameters.
		/// </summary>
		/// <param name="majorVersion">The major version part.</param>
		/// <param name="minorVersion">The minor version part.</param>
		/// <param name="patchVersion">The patch version part.</param>
		public FirmwareVersion(int majorVersion, int minorVersion, int patchVersion)
		{
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			PatchVersion = patchVersion;
		}

		public static FirmwareVersion ParseString(string tripleVersionString)
		{
			if (string.IsNullOrEmpty(tripleVersionString))
			{
				return new FirmwareVersion(0, 0, 0);
			}
			else
			{
				var majorVerison = 0;
				var minorVersion = 0;
				var patchVersion = 0;

				var versionTokens = tripleVersionString.Split('.');
				var versionTokenCount = versionTokens.Length;

				if (versionTokenCount > 0)
				{
					majorVerison = Convert.ToInt32(versionTokens[0]);
				}
				if (versionTokenCount > 1)
				{
					minorVersion = Convert.ToInt32(versionTokens[1]);
				}
				if (versionTokenCount > 2)
				{
					patchVersion = Convert.ToInt32(versionTokens[2]);
				}

				return new FirmwareVersion(majorVerison, minorVersion, patchVersion);
			}
		}

		/// <summary>
		/// The major version part of the firmware version.
		/// </summary>
		public int MajorVersion { get; private set; }

		/// <summary>
		/// The minor version part of the firmware version.
		/// </summary>
		public int MinorVersion { get; private set; }

		/// <summary>
		/// The patch version part of the firmware version.
		/// </summary>
		public int PatchVersion { get; private set; }

		/// <summary>
		/// Checks for equality between this firmware version and another object.
		/// </summary>
		/// <param name="obj">The object to compare equality with.</param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as FirmwareVersion);
		}

		/// <summary>
		/// Checks for equality between two ICrazyradioFirmwareVersion's.
		/// </summary>
		/// <param name="other">The other firmware version to compare equality with.</param>
		/// <returns></returns>
		public bool Equals(FirmwareVersion other)
		{
			if (other == null)
				return false;

			return MajorVersion == other.MajorVersion && MinorVersion == other.MinorVersion && PatchVersion == other.PatchVersion;
		}

		/// <summary>
		/// The hash code for this instance of ICrazyradioFirmwareVersion.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = MajorVersion;
				hashCode = (hashCode * 397) ^ MinorVersion;
				hashCode = (hashCode * 397) ^ PatchVersion;
				return hashCode;
			}
		}

		/// <summary>
		/// Compares ICrazyradioFirmwareVersions.
		/// </summary>
		/// <param name="other">The other ICrazyradioFirmwareVersion to compare to.</param>
		/// <returns>Positive number if this is a higher version. Negative if other is higher version. 0 if same version.</returns>
		public int CompareTo(FirmwareVersion other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			var thisVersion = (100 * MajorVersion) + (10 * MinorVersion) + PatchVersion;
			var otherVersion = (100 * other.MajorVersion) + (10 * other.MinorVersion) + other.PatchVersion;

			return thisVersion - otherVersion;
		}

		/// <summary>
		/// String representation of this ICrazyradioFirmwareVersion.
		/// </summary>
		/// <returns>Full firmware in "Major.Minor.Patch" format.</returns>
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, PatchVersion);
		}
	}
}