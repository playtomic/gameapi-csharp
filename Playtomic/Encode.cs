using System;
using System.Text;

namespace Playtomic
{
	internal class Encode
	{
		public static string Md5(string input)
		{
	        var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
	        var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

	        var sb = new StringBuilder();
			
	        for (var i = 0; i < data.Length; i++)
	            sb.Append(data[i].ToString("x2"));
			
	        return sb.ToString();
	    }
		
		public static string Base64(string data)
		{
	        return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
		}
	}
}