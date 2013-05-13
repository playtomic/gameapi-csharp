using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Playtomic;

namespace PlaytomicTest
{
	internal class PTestLeaderboards : PTest 
	{	
		public static int rnd;
		
		public static void FirstScore(Action done) 
		{		
			var score = new PlayerScore {
				table = "scores" + rnd,
				name = "person1",
				points = 10000,
				highest =  true,
				fields = new Hashtable { 
					{"rnd", rnd}
				}
			};
			
			Leaderboards.Save (score, r => {
				const string section = "TestLeaderboards.FirstScore";
				AssertTrue(section + "#1", "Request succeeded", r.success);
				AssertEquals(section + "#1", "No errorcode", r.errorcode, 0);

				// duplicate score gets rejected
				score.points = 9000;
				Thread.Sleep (1000);

				Leaderboards.Save (score, r2 => {
					AssertTrue(section + "#2", "Request succeeded", r2.success);
					AssertEquals(section + "#2", "Rejected duplicate score", r2.errorcode, 209);

					// better score gets accepted
					score.points = 11000;

					Leaderboards.Save (score, r3 => {
						AssertTrue(section + "#3", "Request succeeded", r3.success);
						AssertEquals(section + "#3", "No errorcode", r3.errorcode, 0);

						// score gets accepted
						score.points = 9000;
						score.allowduplicates = true;

						Leaderboards.Save (score, r4 => {
							AssertTrue(section + "#4", "Request succeeded", r4.success);
							AssertEquals(section + "#4", "No errorcode", r4.errorcode, 0);
							done();
						});
					});
				});
			});
		}

		public static void SecondScore(Action done) 
		{		
			var score = new PlayerScore {
				table = "scores" + rnd,
				name = "person2",
				points = 20000,
				allowduplicates = true,
				highest =  true,
				fields = new Hashtable { 
					{"rnd", rnd}
				}
			};
			
			Thread.Sleep (1000);
			Leaderboards.Save (score, r => {
				const string section = "TestLeaderboards.SecondScore";
				AssertTrue(section + "#3", "Request succeeded", r.success);
				AssertEquals(section + "#3", "No errorcode", r.errorcode, 0);
				done();
			});
		}
		
		public static void HighScores(Action done)
		{
			var options = new Hashtable
			{
				{"table", "scores" + rnd},
				{"highest", true},
				{"filters", new Hashtable
					{
						{"rnd", rnd}
					}
				}
			};
			
			Leaderboards.List (options, (scores, numscores, r) => {
				const string section = "TestLeaderboards.Highscores";
				scores = scores ?? new List<PlayerScore>();

				AssertTrue(section, "Request succeeded", r.success);
				AssertEquals(section, "No errorcode", r.errorcode, 0);
				AssertTrue(section, "Received scores", scores.Count > 0);
				AssertTrue(section, "Received numscores", numscores > 0);

				if(scores.Count > 1) {
					AssertTrue(section, "First score is greater than second", scores[0].points > scores[1].points);
				} else {
					AssertTrue(section, "First score is greater than second forced failure", false);
				}

				done();
			});
		}
		
		public static void LowScores(Action done)
		{
			var options = new Hashtable
			{
				{"table", "scores" + rnd},
				{"lowest", true},
				{"perpage", 2},
				{"filters", new Hashtable
					{
						{"rnd", rnd}
					}
				}
			};
			
			Leaderboards.List (options, (scores, numscores, r) => {
				const string section = "TestLeaderboards.LowScores";
				scores = scores ?? new List<PlayerScore>();

				AssertTrue(section, "Request succeeded", r.success);
				AssertEquals(section, "No errorcode", r.errorcode, 0);
				AssertTrue(section, "Received scores", scores.Count == 2);
				AssertTrue(section, "Received numscores", numscores > 0);

				if(scores.Count > 1) {
					AssertTrue(section, "First score is less than second", scores[0].points < scores[1].points);
				} else {
					AssertTrue(section, "First score is less than second forced failure", false);
				}

				done();
			});
		}
		
		public static void AllScores(Action done)
		{
			var options = new Hashtable
			{
				{"table", "scores" + rnd},
				{"mode", "newest"},
				{"perpage", 2}
			};
			
			Leaderboards.List (options, (scores, numscores, r) => {
				const string section = "TestLeaderboards.AllScores";
				scores = scores ?? new List<PlayerScore>();

				AssertTrue(section, "Request succeeded", r.success);
				AssertEquals(section, "No errorcode", r.errorcode, 0);
				AssertTrue(section, "Received scores", scores.Count > 0);
				AssertTrue(section, "Received numscores", numscores > 0);

				if(scores.Count > 1) {
					AssertTrue(section, "First score is newer or equal to second", scores[0].date >= scores[1].date);
				} else {
					AssertTrue(section, "First score is newer or equal to second forced failure", false);
				}

				done();
			});
		}
	}
}