using System;

namespace PlaytomicTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Playtomic Xamarin / Mono.NET / C# tests");
			var tests = new PTests ();
			tests.Start ();
		}
	}
}
