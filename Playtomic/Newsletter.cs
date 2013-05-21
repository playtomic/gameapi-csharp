using System;
using System.Collections;

namespace Playtomic
{
	public class Newsletter
	{
		private const string SECTION = "newsletter";
		private const string SUBSCRIBE = "subscribe";

		/**
		 * Subscribes a person to your newsletter 
		 * @param	options	Hashtable	The email and other information
		 * @param	callback	Action<PResponse>	Your callback function
		 */
		public static void Subscribe(Hashtable options, Action<PResponse> callback)
		{
			PRequest.GetResponse (SECTION, SUBSCRIBE, options, response => {
				callback(response);
			});
		}
	}
}