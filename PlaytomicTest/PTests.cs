using System;
using System.Collections.Generic;
using Playtomic;

namespace PlaytomicTest
{
	public class PTests 
	{
		private List<Action<Action>> _tests;
		
		public void Start() 
		{
			Initialize.SetCredentials("testpublickey", "testprivatekey", "http://127.0.0.1:3000");
			PTest.Setup ();	
			PTestLeaderboards.rnd = PTestPlayerLevels.rnd = RND();
			
			_tests = new List<Action<Action>>
			    {
			        PTestGameVars.All,
			        PTestGameVars.Single,
			        PTestGeoIP.Lookup,
			        PTestLeaderboards.FirstScore,
			        PTestLeaderboards.SecondScore,
			        PTestLeaderboards.HighScores,
			        PTestLeaderboards.LowScores,
			        PTestLeaderboards.AllScores,
					PTestLeaderboards.FriendsScores,
					PTestLeaderboards.OwnScores,
			        PTestPlayerLevels.Create,
			        PTestPlayerLevels.List,
			        PTestPlayerLevels.Load,
			        PTestPlayerLevels.Rate
			    };
		    Next ();
		}
		
		void Next()
		{
			if(_tests.Count == 0) {
				PTest.Render ();
				return;
			}
			
			var action = _tests[0];
			_tests.RemoveAt(0);
			action(Next);
		}
		
		private static int RND()
		{
			var random = new Random();
			return random.Next (int.MaxValue);
		}
	}
}