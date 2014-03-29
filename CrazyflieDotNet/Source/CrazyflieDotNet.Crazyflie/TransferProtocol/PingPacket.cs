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

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public sealed class PingPacket
		: OutputPacket<IPingPacketHeader>, IPingPacket
	{
		public PingPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		public PingPacket(IPingPacketHeader header)
			: base(header)
		{
		}

		public PingPacket(Channel channel = Channel.Channel0)
			: this(new PingPacketHeader(channel))
		{
		}

		protected override IPingPacketHeader ParseHeader(byte[] packetBytes)
		{
			if (packetBytes != null && packetBytes.Length != 0)
			{
				var packetHeader = new PingPacketHeader(packetBytes[0]);
				return packetHeader;
			}

			return null;
		}
	}
}