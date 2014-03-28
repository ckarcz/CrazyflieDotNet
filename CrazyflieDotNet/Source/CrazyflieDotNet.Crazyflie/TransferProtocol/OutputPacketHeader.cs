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

namespace CrazyflieDotNet.Crazyflie.TransferProtocol
{
	public abstract class OutputPacketHeader
		: IOutputPacketHeader
	{
		protected OutputPacketHeader(byte headerByte)
		{
			Port = GetPort(headerByte);
			Channel = GetChannel(headerByte);
		}

		protected OutputPacketHeader(Port port, Channel channel = DefaultChannel)
		{
			Port = port;
			Channel = channel;
		}

		public Port Port { get; private set; }

		public Channel Channel { get; private set; }

		public const Channel DefaultChannel = Channel.Channel0;

		public byte[] GetBytes()
		{
			// Header Format (1 byte):
			//  7  6  5  4  3  2  1  0
			// [   Port   ][Res. ][Ch.]
			// Res. = reserved for transfer layer. not much info on this...

			try
			{
				var portByte = (byte)Port;
				var portByteAnd15 = (byte)(portByte & 0x0F);
				var portByteAnd15LeftShifted4 = (byte)(portByteAnd15 << 4);
				var reservedLeftShifted2 = (byte)(0x03 << 2);
				var channelByte = (byte)Channel;
				var channelByteAnd3 = (byte)(channelByte & 0x03);

				return new[] { (byte)(portByteAnd15LeftShifted4 | reservedLeftShifted2 | channelByteAnd3) };
			}
			catch (Exception ex)
			{
				throw new Exception("Error converting output header to bytes.", ex);
			}
		}

		private Port GetPort(byte headerBytes)
		{
			throw new NotImplementedException();
		}

		private Channel GetChannel(byte headerBytes)
		{
			throw new NotImplementedException();
		}
	}
}