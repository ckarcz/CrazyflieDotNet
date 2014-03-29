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

#endregion Imports

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class OutputPacket<TOutputPacketHeader>
		   : Packet<TOutputPacketHeader>,
		   IOutputPacket<TOutputPacketHeader>
		where TOutputPacketHeader : IOutputPacketHeader
	{
		#region Constructors

		protected OutputPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		protected OutputPacket(TOutputPacketHeader header)
			: base(header)
		{
		}

		#endregion Constructors
	}

	public abstract class OutputPacket<TOutputPacketHeader, TOutputPacketPayload>
		: Packet<TOutputPacketHeader, TOutputPacketPayload>,
		IOutputPacket<TOutputPacketHeader, TOutputPacketPayload>
		where TOutputPacketHeader : IOutputPacketHeader
		where TOutputPacketPayload : IOutputPacketPayload
	{
		#region Constructors

		protected OutputPacket(byte[] packetBytes)
			: base(packetBytes)
		{
		}

		protected OutputPacket(TOutputPacketHeader header, TOutputPacketPayload payload)
			: base(header, payload)
		{
		}

		#endregion Constructors
	}
}