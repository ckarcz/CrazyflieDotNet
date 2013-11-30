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

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public sealed class CRTPPingPacket
		: CRTPDataPacket
	{
		public CRTPPingPacket(CRTPChannel channel = CRTPChannel.Channel0)
			: base(new CRTPOutPacketHeader(CRTPPort.All, channel))
		{
		}

		protected override byte[] GetPacketPayloadBytes()
		{
			return new byte[0];
		}
	}
}