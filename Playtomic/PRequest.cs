using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Playtomic
{
	internal class PRequest
	{
		private static string APIURL;
		private static string PUBLICKEY;
		private static string PRIVATEKEY;
		
		public static void Initialise(string publickey, string privatekey, string apiurl)
		{
			if(!apiurl.EndsWith ("/")) 
			{
				apiurl += "/";
			}
			
			APIURL = apiurl + "v1?publickey=" + publickey;
			PRIVATEKEY = privatekey;
			PUBLICKEY = publickey;
		}
		
		public static void GetResponse(string section, string action, Hashtable postdata, Action<PResponse> callback)
		{
			if(postdata == null)
			{
				postdata = new Hashtable();
			}
			else 
			{
				postdata.Remove ("publickey");
				postdata.Remove ("section");
				postdata.Remove ("action");
			}

			postdata.Add ("publickey", PUBLICKEY);
			postdata.Add ("section", section);
			postdata.Add ("action", action);

			var json = JSON.JsonEncode(postdata);
			var buffer = Encoding.UTF8.GetBytes ("data=" + Encode.Base64 (json) + "&hash=" + Encode.Md5 (json + PRIVATEKEY));
			var task = MakeAsyncRequest(buffer);

			if(string.IsNullOrEmpty (task.Result) || task.Exception != null || task.IsCanceled || task.IsFaulted)
			{
				callback(PResponse.GeneralError(1));
				return;
			}

			var results = (Hashtable)JSON.JsonDecode(task.Result);

			if(!results.ContainsKey("success") || !results.ContainsKey("errorcode"))
			{
				callback(PResponse.GeneralError(1));
				return;
			}

		    callback(new PResponse
			    {
			        success = (bool) results["success"],
			        errorcode = (int) (double) results["errorcode"],
			        json = results
			    });
		}

		// Courtesy of http://stackoverflow.com/questions/10565090/getting-the-response-of-a-asynchronous-httpwebrequest
		private static Task<string> MakeAsyncRequest(byte[] buffer)
		{
			var request = (HttpWebRequest)WebRequest.Create(APIURL);
			request.Method = "POST";
			request.ContentType ="application/x-www-form-urlencoded";
			request.ContentLength = buffer.Length;

			var post = request.GetRequestStream();
			post.Write(buffer, 0, buffer.Length);
			post.Close();

			var task = Task.Factory.FromAsync(
				request.BeginGetResponse,
				asyncResult => request.EndGetResponse(asyncResult),
				null);

			return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
		}

		private static string ReadStreamFromResponse(WebResponse response)
		{
		    using (var stream = response.GetResponseStream())
		    {
		        if (stream != null)
		        {
		            using (var sr = new StreamReader(stream))
		            {
		                return sr.ReadToEnd();
		            }
		        }
		    }

		    return null;
		}
	}
}