using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Playtomic
{
	public class Leaderboards
	{
		private const string SECTION = "leaderboards";
		private const string SAVEANDLIST = "saveandlist";
		private const string SAVE = "save";
		private const string LIST = "list";
		
		/**
		 * Saves a player's score
		 * @param	score	PlayerScore	The PlayerScore object 
		 * @param	callback	Action<PResponse> Your callback method
		 */
		public static void Save(PlayerScore score, Action<PResponse> callback)
		{
			PRequest.GetResponse (SECTION, SAVE, score, callback);
		}

		/**
		 * Saves a player's score and then returns the page of scores
		 * it is on
		 * @param	score	PlayerScore	The PlayerScore object 
		 * @param	callback	Action<List<PlayerScore>, int, PResponse> Your callback method
		 */
		public static void SaveAndList(PlayerScore score, Action<List<PlayerScore>, int, PResponse> callback)
		{
			SendListRequest(SECTION, SAVEANDLIST, score, callback);
		}

		/**
		 * Lists scores
		 * @param	options	Hashtable	The listing options
		 * @param	callback	Action<List<PlayerScore>, int, PResponse>	Your callback function
		 */
		public static void List(Hashtable options, Action<List<PlayerScore>, int, PResponse> callback)
		{	
			SendListRequest(SECTION, LIST, options, callback);
		}

		private static void SendListRequest(string section, string action, Hashtable postdata, Action<List<PlayerScore>, int, PResponse> callback)
		{ 
			PRequest.GetResponse (section, action, postdata, response => {
				var data = response.json;
				List<PlayerScore> scores;
				int numscores;
				ProcessScores (response, data, out scores, out numscores);
				callback (scores, numscores, response);
			});
		}
		
		private static void ProcessScores(PResponse response, IDictionary data, out List<PlayerScore> scores, out int numitems)
		{
			scores = new List<PlayerScore>();
			numitems = 0;

		    if (!response.success) 
                return;

		    numitems = (int)(double)data["numscores"];
		    var scorearr = (ArrayList) data["scores"];

		    scores.AddRange(from object t in scorearr select new PlayerScore((Hashtable) t));
		}
	}
}