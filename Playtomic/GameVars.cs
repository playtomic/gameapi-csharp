using System;
using System.Collections;

namespace Playtomic
{
	public class GameVars
	{
		private const string SECTION = "gamevars";
		private const string LOAD = "load";
		private const string LOADSINGLE = "single";
			
		/**
		 * Loads all GameVars
		 * @param	callback	Action<Hashtable, PResponse>	Your callback method
		 */
		public static void Load(Action<Hashtable, PResponse> callback)
		{
			PRequest.GetResponse (SECTION, LOAD, null, response => {
				var data = response.success ? response.json : null;
				callback(data, response);
			});
		}

		/**
		 * Loads a single GameVar
		 * @param	name	string	The variable name to load
		 * @param	callback	Action<Hashtable, PResponse>	Your callback method
		 */
		public static void LoadSingle(string name, Action<Hashtable, PResponse> callback)
		{
			var postdata = new Hashtable
			{
				{"name", name}
			};

			PRequest.GetResponse (SECTION, LOADSINGLE, postdata, response => {
				var data = response.success ? response.json : null;
				callback(data, response);
			});
		}
	}
}