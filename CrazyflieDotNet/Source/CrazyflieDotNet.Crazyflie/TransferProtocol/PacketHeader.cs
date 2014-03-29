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

using System;
using System.Data;

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class PacketHeader
		: IPacketHeader
	{
		public byte? GetByte()
		{
			try
			{
				var packetHeaderByte = GetPacketHeaderByte();
				return packetHeaderByte;
			}
			catch (Exception ex)
			{
				throw new DataException("Error obtaining packet header byte.", ex);
			}
		}

		protected abstract byte? GetPacketHeaderByte();
	}
}