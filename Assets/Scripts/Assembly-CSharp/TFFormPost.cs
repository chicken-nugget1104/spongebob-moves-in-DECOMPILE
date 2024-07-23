using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

// Token: 0x0200047E RID: 1150
public static class TFFormPost
{
	// Token: 0x06002404 RID: 9220 RVA: 0x000DD758 File Offset: 0x000DB958
	public static HttpWebResponse PostForm(Uri postUri, string userAgent, Dictionary<string, object> postParameters, CookieContainer cookies)
	{
		string text = string.Format("----------{0:N}", Guid.NewGuid());
		string contentType = "multipart/form-data; boundary=" + text;
		byte[] formData = TFFormPost.GetFormData(postParameters, text);
		HttpWebRequest httpWebRequest = WebRequest.Create(postUri) as HttpWebRequest;
		if (httpWebRequest == null)
		{
			throw new NullReferenceException("Request is not a valid http request.");
		}
		httpWebRequest.Method = "POST";
		httpWebRequest.ContentType = contentType;
		httpWebRequest.UserAgent = userAgent;
		httpWebRequest.CookieContainer = cookies;
		httpWebRequest.ContentLength = (long)formData.Length;
		using (Stream requestStream = httpWebRequest.GetRequestStream())
		{
			requestStream.Write(formData, 0, formData.Length);
			requestStream.Close();
		}
		return httpWebRequest.GetResponse() as HttpWebResponse;
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x000DD82C File Offset: 0x000DBA2C
	private static byte[] GetFormData(Dictionary<string, object> postParameters, string boundary)
	{
		Stream stream = new MemoryStream();
		bool flag = false;
		foreach (KeyValuePair<string, object> keyValuePair in postParameters)
		{
			if (flag)
			{
				stream.Write(TFFormPost.encoding.GetBytes("\r\n"), 0, TFFormPost.encoding.GetByteCount("\r\n"));
			}
			flag = true;
			if (keyValuePair.Value.GetType() == typeof(byte[]))
			{
				byte[] array = (byte[])keyValuePair.Value;
				string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n", new object[]
				{
					boundary,
					keyValuePair.Key,
					keyValuePair.Key,
					"application/octet-stream"
				});
				stream.Write(TFFormPost.encoding.GetBytes(s), 0, TFFormPost.encoding.GetByteCount(s));
				stream.Write(array, 0, array.Length);
			}
			else
			{
				string s2 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary, keyValuePair.Key, keyValuePair.Value);
				stream.Write(TFFormPost.encoding.GetBytes(s2), 0, TFFormPost.encoding.GetByteCount(s2));
			}
		}
		string s3 = "\r\n--" + boundary + "--\r\n";
		stream.Write(TFFormPost.encoding.GetBytes(s3), 0, TFFormPost.encoding.GetByteCount(s3));
		stream.Position = 0L;
		byte[] array2 = new byte[stream.Length];
		stream.Read(array2, 0, array2.Length);
		stream.Close();
		return array2;
	}

	// Token: 0x04001633 RID: 5683
	private static readonly Encoding encoding = Encoding.UTF8;
}
