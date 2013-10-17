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

namespace CrazyflieDotNet.Crazyflie.CRTP
{
	public class CRTPException
		: Exception
	{
		public CRTPException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public CRTPException(string message)
			: base(message)
		{
		}
	}
}