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
	public interface IOutputPacket<out TDataPacketHeader, out TDataPacketPayload>
		: IPacket<TDataPacketHeader, TDataPacketPayload>
		where TDataPacketHeader : IOutputPacketHeader
		where TDataPacketPayload : IOutputPacketPayload
	{
	}

	public interface IOutputPacket<out TDataPacketHeader>
		: IOutputPacket, IPacket<TDataPacketHeader>
		where TDataPacketHeader : IOutputPacketHeader
	{
	}

	public interface IOutputPacket
		: IPacket
	{
	}
}