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
using System.Data;

#endregion

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public interface IPacket<out TPacketHeader, out TPacketPayload>
		: IPacket<TPacketHeader>
		where TPacketHeader : IPacketHeader
		where TPacketPayload : IPacketPayload
	{
		new TPacketPayload Payload { get; }
	}

	public interface IPacket<out TPacketHeader>
		: IPacket
		where TPacketHeader : IPacketHeader
	{
		new TPacketHeader Header { get; }
	}

	public interface IPacket
	{
		IPacketHeader Header { get; }

		IPacketPayload Payload { get; }

		byte[] GetBytes();
	}
}