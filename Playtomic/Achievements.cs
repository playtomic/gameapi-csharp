using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Playtomic
{
	public class Achievements
	{
		private static string SECTION = "achievements";
		private static string LIST = "list";
		private static string STREAM = "stream";
		private static string SAVE = "save";

		/**
		 * Lists all achievements
		 * @param	options		The list options
		 * @param	callback	Your callback Action<List<Achievement>, PResponse>
		 */
		public static void List(Hashtable options, Action<List<PlayerAchievement>, PResponse> callback) {
			PRequest.GetResponse (SECTION, LIST, options, response => {
				var data = response.success ? response.json : null;
				var achievements = new List<PlayerAchievement>();
				if (response.success) 
				{
					var acharray = (ArrayList) data["achievements"];
					achievements.AddRange(from object t in acharray select new PlayerAchievement((Hashtable) t));
				}

				callback(achievements, response);
			});
		}

		/**
		 * Shows a chronological stream of achievements 
		 * @param	options		The stream options
		 * @param	callback	Your callback Action<List<Achievement>, int, PResponse>
		 */ 
		public static void Stream(Hashtable options, Action<List<PlayerAward>, int, PResponse> callback) {
			PRequest.GetResponse (SECTION, STREAM, options, response => {
				var data = response.success ? response.json : null;
				int numachievements = response.success ? (int)(double)data["numachievements"] : 0;
				var achievements = new List<PlayerAward>();

				if (response.success) 
				{
					var acharray = (ArrayList) data["achievements"];
					achievements.AddRange(from object t in acharray select new PlayerAward((Hashtable) t));
				}

				callback(achievements, numachievements, response);
			});
		}

		/**
		 * Award an achievement to a player
		 * @param	achievement	The achievement
		 * @param	callback	Your callback Action<PResponse>
		 */
		public static void Save(Hashtable achievement, Action<PResponse> callback) {
			PRequest.GetResponse (SECTION, SAVE, achievement, response => {
				callback(response);
			});
		}
	}
}