using System;

namespace Playtomic
{
	public class GeoIP
	{
		private const string SECTION = "geoip";
		private const string LOOKUP = "lookup";

		/**
		 * Performs a GeoIP lookup
		 * @param	callback	Action<PlayerCountry, PResponse>	Your callback method
		 */
		public static void Lookup(Action<PlayerCountry, PResponse> callback)
		{
			PRequest.GetResponse (SECTION, LOOKUP, null, response => {
				var data = response.success ? response.json : null;
				callback(new PlayerCountry(data), response);
			});
		}
	}
}