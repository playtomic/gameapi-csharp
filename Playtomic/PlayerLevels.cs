using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Playtomic
{
	public class PlayerLevels
	{	
		private const string SECTION = "playerlevels";
		private const string SAVE = "save";
		private const string LIST = "list";
		private const string LOAD = "load";
		private const string RATE = "rate";
		
		/**
		 * Saves a PlayerLevel
		 * @param	level	PlayerLevel	The level
		 * @param	callback	Action<PlayerLevel, PResponse>	Callback function
		 */
		public static void Save(PlayerLevel level, Action<PlayerLevel, PResponse> callback)
		{
			SendSaveLoadRequest(SECTION, SAVE, level, callback);
		}

		/**
		 * Loads a level
		 * @param	levelid	string 	The level id
		 * @param 	callback	Action<PlayerLevel, PResponse>	Callback function
		 */
		public static void Load(string levelid, Action<PlayerLevel, PResponse> callback)
		{
			var postdata = new Hashtable
			{
				{"levelid", levelid }
			};

			SendSaveLoadRequest(SECTION, LOAD, postdata, callback);
		}
		
		private static void SendSaveLoadRequest(string section, string action, Hashtable postdata, Action<PlayerLevel, PResponse> callback)
		{ 
			PRequest.GetResponse (section, action, postdata, response => {
				var level = response.success 
						? new PlayerLevel((Hashtable) response.json["level"])
						: null;
				callback(level, response);
			});
		}
		
		/**
		 * Lists levels
		 * @param	options	Hashtable	The listing options
		 * @param 	callback	Action<List<PlayerLevel>, int, PResponse>	Callback function
		 */
		public static void List(Hashtable options, Action<List<PlayerLevel>, int, PResponse> callback)
		{
			SendListRequest(SECTION, LIST, options, callback);
		}

		private static void SendListRequest(string section, string action, Hashtable postdata, Action<List<PlayerLevel>, int, PResponse> callback) 
		{
			PRequest.GetResponse (section, action, postdata, response => {
				var data = response.json;
				List<PlayerLevel> levels;
				int numlevels;
				ProcessLevels (response, data, out levels, out numlevels);
				callback (levels, numlevels, response);
			});
		}

		private static void ProcessLevels(PResponse response, Hashtable data, out List<PlayerLevel> levels, out int numlevels)
		{
			levels = new List<PlayerLevel> ();
			numlevels = 0;

		    if (!response.success) 
                return;

		    numlevels = (int)(double)data["numlevels"];
		    var levelarr = (ArrayList)data["levels"];
		    levels.AddRange(from object t in levelarr select new PlayerLevel((Hashtable) t));
		}
		
		/**
		 * Rates a level
		 * @param	levelid	String	The level id
		 * @param	rating	Int		Rating from 1 - 10
		 * @param	callback	Action<PResponse> Your callback function
		 */
		public static void Rate(string levelid, int rating, Action<PResponse> callback)
		{
			if(rating < 1 || rating > 10)
			{
				callback(PResponse.Error(401));
				return;
			}

			var postdata = new Hashtable
			{
				{"levelid", levelid},
				{"rating", rating}
			};

			PRequest.GetResponse (SECTION, RATE, postdata, callback);
		}
	}
}