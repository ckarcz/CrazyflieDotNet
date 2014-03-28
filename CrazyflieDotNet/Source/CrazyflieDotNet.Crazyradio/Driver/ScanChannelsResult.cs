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

using System.Collections.Generic;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio.Driver
{
	public sealed class ScanChannelsResult
	{
		public ScanChannelsResult(RadioDataRate dataRate, IEnumerable<RadioChannel> channels)
		{
			DataRate = dataRate;
			Channels = channels;
		}

		public RadioDataRate DataRate { get; private set; }

		public IEnumerable<RadioChannel> Channels { get; private set; }
	}
}