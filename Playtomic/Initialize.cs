namespace Playtomic
{
	public class Initialize
	{
		public static void SetCredentials(string publickey, string privatekey, string apiurl)
		{
			PRequest.Initialise(publickey, privatekey, apiurl);
		}
	}
}