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
using System.Linq;
using log4net;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio.Driver
{
	/// <summary>
	/// The radio address of the Crazyradio.
	/// </summary>
	public sealed class RadioAddress
		: IEquatable<RadioAddress>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RadioAddress));

		/// <summary>
		/// Initializes a CrazyradioAddress instance.
		/// </summary>
		/// <param name="addressBytes">The address bytes as single byte parameters. Address must be exactly 5 bytes long.</param>
		public RadioAddress(params byte[] addressBytes)
		{
			if (addressBytes == null)
			{
				Log.Error("Address byte array is null.");
				throw new ArgumentNullException("addressBytes");
			}

			if (addressBytes.Length != 5)
			{
				var message = string.Format("Address must be exactly 5 bytes. Given {0} bytes.", addressBytes.Length);
				Log.Error(message);
				throw new ArgumentException(message);
			}

			Bytes = addressBytes;
		}

		/// <summary>
		/// The address bytes.
		/// </summary>
		public byte[] Bytes { get; private set; }

		/// <summary>
		/// Checks for Crazyradio address equality.
		/// </summary>
		/// <param name="obj">Object comparing equality to.</param>
		/// <returns>True if this address is equal to the other object.</returns>
		public override bool Equals(object obj)
		{
			return Equals((RadioAddress)obj);
		}

		/// <summary>
		/// Checks for Crazyradio address equality.
		/// </summary>
		/// <param name="other">Other CrazyradioAddress comparing equality to.</param>
		/// <returns>True if this address is equal to the other CrazyradioAddress.</returns>
		public bool Equals(RadioAddress other)
		{
			if (other == null)
			{
				return false;
			}

			return Bytes.SequenceEqual(other.Bytes);
		}

		/// <summary>
		/// Hashcode for this CrazyradioAddress.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (Bytes != null ? Bytes.GetHashCode() : 0);
		}

		/// <summary>
		/// String representation of this CrazyradioAddress.
		/// </summary>
		/// <returns>The 5 byte Crazyradio address in hexadecimal form.</returns>
		public override string ToString()
		{
			return BitConverter.ToString(Bytes.ToArray());
		}
	}
}