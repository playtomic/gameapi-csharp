using System;
using Playtomic;

namespace PlaytomicTest
{
	internal class PTestGeoIP : PTest 
	{
		public static void Lookup(Action done) 
		{
			Playtomic.GeoIP.Lookup ((geo, r) => {
				geo = geo ?? new PlayerCountry();

				const string section = "PTestGeoIP.Lookup";
				AssertTrue(section, "Request succeeded", r.success);
				AssertEquals(section, "No errorcode", r.errorcode, 0);
				AssertFalse(section, "Has country name", string.IsNullOrEmpty (geo.name));
				AssertFalse(section, "Has country code", string.IsNullOrEmpty (geo.code));
				done();
			});
		}
	}
}
