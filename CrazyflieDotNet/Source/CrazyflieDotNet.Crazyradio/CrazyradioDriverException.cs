/* 
 *						 _ _  _     
 *		       ____ ___  ___  __________(_|_)(_)____
 *		      / __ `__ \/ _ \/ ___/ ___/ / _ \/ ___/
 *		     / / / / / /  __(__  |__  ) /  __/ /    
 *		    /_/ /_/ /_/\___/____/____/_/\___/_/  
 *
 *	     Copyright 2013 - Messi?r/Chris Karcz - ckarcz@gmail.com
 *
 *	This Source Code Form is subject to the terms of the Mozilla Public
 *	License, v. 2.0. If a copy of the MPL was not distributed with this
 *	file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#region Imports

using System;

#endregion Imports

namespace CrazyflieDotNet.Crazyradio
{
	/// <summary>
	///   Exception occuring within the CrazyradioDriver.
	/// </summary>
	public class CrazyradioDriverException
		: Exception
	{
		/// <summary>
		///   Initializes a new instance of CrazyradioDriverException.
		/// </summary>
		/// <param name="message"> The exception message. </param>
		/// <param name="innerException"> The inner exception. </param>
		public CrazyradioDriverException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		///   Initializes a new instance of CrazyradioDriverException.
		/// </summary>
		/// <param name="message"> The exception message. </param>
		public CrazyradioDriverException(string message)
			: base(message)
		{
		}
	}
}