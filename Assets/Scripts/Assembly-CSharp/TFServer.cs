using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using MiniJSON;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public class TFServer
{
	// Token: 0x06001E78 RID: 7800 RVA: 0x000BBE74 File Offset: 0x000BA074
	public TFServer()
	{
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x000BBEAC File Offset: 0x000BA0AC
	public TFServer(CookieContainer cookies)
	{
		this.cookies = cookies;
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x000BBF1C File Offset: 0x000BA11C
	public static bool IsNetworkError(Dictionary<string, object> response)
	{
		return response.ContainsKey("error") && "Network error".Equals(response["error"]);
	}

	// Token: 0x170003F6 RID: 1014
	// (set) Token: 0x06001E7C RID: 7804 RVA: 0x000BBF54 File Offset: 0x000BA154
	public CookieContainer Cookies
	{
		set
		{
			this.cookies = value;
		}
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06001E7D RID: 7805 RVA: 0x000BBF60 File Offset: 0x000BA160
	public bool Connected
	{
		get
		{
			return !this.unreachable;
		}
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x000BBF6C File Offset: 0x000BA16C
	public void SetConnected(bool val)
	{
		this.loggingIn = val;
		this.unreachable = !val;
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x000BBF80 File Offset: 0x000BA180
	public void PostToJSON(string url, Dictionary<string, object> postDict, TFServer.JsonResponseHandler callback, bool checkConnection = false, object userData = null)
	{
		if (checkConnection)
		{
			this.CheckConnectivity();
		}
		string s = this.encodePostData(postDict);
		using (TFWebClient tfwebClient = this.RegisterCallback(callback))
		{
			if (this.ShortCircuitRequest())
			{
				TFUtils.DebugLog("shortcircuiting a post to " + url);
				this.GetCallback(tfwebClient)(TFServer.NETWORK_ERROR_JSON, userData);
			}
			else
			{
				TFUtils.DebugLog("Sending a post to " + url);
				tfwebClient.Post(new Uri(url), Encoding.UTF8.GetBytes(s), new TFWebClient.GetCallbackHandler(this.OnCallComplete), userData);
			}
		}
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x000BC040 File Offset: 0x000BA240
	public void GetToJSON(string url, TFServer.JsonResponseHandler callback, bool checkConnection = false, object userData = null)
	{
		if (checkConnection)
		{
			this.CheckConnectivity();
		}
		using (TFWebClient tfwebClient = this.RegisterCallback(callback))
		{
			if (this.ShortCircuitRequest())
			{
				TFUtils.DebugLog("Shortcircuiting a request to " + url);
				this.GetCallback(tfwebClient)(TFServer.NETWORK_ERROR_JSON, userData);
			}
			else
			{
				TFUtils.DebugLog("Sending a request to " + url);
				tfwebClient.Get(new Uri(url), new TFWebClient.GetCallbackHandler(this.OnCallComplete), userData);
			}
		}
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x000BC0EC File Offset: 0x000BA2EC
	public Cookie GetCookie(Uri uri, string key)
	{
		return this.cookies.GetCookies(uri)[key];
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x000BC100 File Offset: 0x000BA300
	private TFWebClient RegisterCallback(TFServer.JsonStringHandler callback)
	{
		TFWebClient tfwebClient = new TFWebClient(this.cookies);
		WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
		TFUtils.SetDefaultHeaders(webHeaderCollection);
		tfwebClient.Headers = webHeaderCollection;
		this.reqs[tfwebClient] = callback;
		return tfwebClient;
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x000BC13C File Offset: 0x000BA33C
	private TFWebClient RegisterCallback(TFServer.JsonResponseHandler callback)
	{
		TFWebClient tfwebClient = new TFWebClient(this.cookies);
		WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
		TFUtils.SetDefaultHeaders(webHeaderCollection);
		tfwebClient.Headers = webHeaderCollection;
		this.reqs[tfwebClient] = this.JsCallback(callback);
		return tfwebClient;
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x000BC17C File Offset: 0x000BA37C
	private string encodePostData(Dictionary<string, object> d)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, object> keyValuePair in d)
		{
			string s = keyValuePair.Value as string;
			list.Add(keyValuePair.Key + "=" + WWW.EscapeURL(s));
		}
		return string.Join("&", list.ToArray());
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x000BC218 File Offset: 0x000BA418
	private TFServer.JsonStringHandler JsCallback(TFServer.JsonResponseHandler cb)
	{
		return delegate(string jsonResponse, object userData)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(jsonResponse);
			if (dictionary == null)
			{
				TFUtils.ErrorLog("TFServer.JsCallback - Failed to parse jsonResponse: " + jsonResponse);
			}
			else if (!(bool)dictionary["success"])
			{
				cb(dictionary, userData);
			}
			else
			{
				Dictionary<string, object> dict = dictionary["data"] as Dictionary<string, object>;
				cb(dict, userData);
			}
		};
	}

	// Token: 0x06001E86 RID: 7814 RVA: 0x000BC240 File Offset: 0x000BA440
	private void OnNetworkError(TFWebClient client, TFServer.JsonStringHandler callback)
	{
		this.strikes++;
		if (this.strikes > 3)
		{
			this.deactivatedTime = TFUtils.EpochTime();
			this.activeConnection = false;
			this.strikes = 0;
		}
		WebException ex = client.Response.Error as WebException;
		if (ex != null && ex.Response != null)
		{
			this.LogResponse(ex.Response as HttpWebResponse);
		}
		if (callback != null)
		{
			callback(TFServer.NETWORK_ERROR_JSON, client.UserData);
		}
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x000BC2CC File Offset: 0x000BA4CC
	private void OnCallComplete(TFWebClient client)
	{
		TFServer.JsonStringHandler callback = this.GetCallback(client);
		if (callback != null)
		{
			if (client.Response.Error == null)
			{
				TFUtils.DebugLog("Got response data: " + client.Response.Data);
				callback(client.Response.Data, client.UserData);
			}
			else
			{
				TFUtils.DebugLog("Got response error: " + client.Response.Error);
				this.OnNetworkError(client, callback);
			}
		}
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x000BC350 File Offset: 0x000BA550
	private TFServer.JsonStringHandler GetCallback(TFWebClient sender)
	{
		if (this.reqs.ContainsKey(sender))
		{
			TFServer.JsonStringHandler result = this.reqs[sender];
			this.reqs.Remove(sender);
			return result;
		}
		return null;
	}

	// Token: 0x06001E89 RID: 7817 RVA: 0x000BC38C File Offset: 0x000BA58C
	private void LogResponse(HttpWebResponse response)
	{
		Stream responseStream = response.GetResponseStream();
		using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
		{
			string text = streamReader.ReadToEnd();
			if (!string.IsNullOrEmpty(text))
			{
				TFUtils.DebugLog("Writing out error: " + text);
				File.WriteAllText(string.Format("{0}{1}.html", TFServer.LOG_LOCATION, ++TFServer.errorCount), text);
			}
		}
		responseStream.Dispose();
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x000BC42C File Offset: 0x000BA62C
	private void CheckConnectivity()
	{
		bool flag = Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
		if (!flag || !this.loggingIn)
		{
			this.unreachable = true;
			this.activeConnection = false;
		}
		else
		{
			this.unreachable = false;
			if (TFUtils.EpochTime() > this.deactivatedTime + 60UL)
			{
				this.activeConnection = true;
			}
		}
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x000BC498 File Offset: 0x000BA698
	private bool ShortCircuitRequest()
	{
		return !this.activeConnection || this.unreachable;
	}

	// Token: 0x040012E5 RID: 4837
	public const string ERROR_KEY = "error";

	// Token: 0x040012E6 RID: 4838
	public const string NETWORK_ERROR = "Network error";

	// Token: 0x040012E7 RID: 4839
	private const bool LOG_FAILED_REQUESTS = true;

	// Token: 0x040012E8 RID: 4840
	private const ulong DEACTIVATION_PERIOD = 60UL;

	// Token: 0x040012E9 RID: 4841
	private const int STRIKE_OUT = 3;

	// Token: 0x040012EA RID: 4842
	private static string NETWORK_ERROR_JSON = "{\"success\": false, \"error\": \"Network error\"}";

	// Token: 0x040012EB RID: 4843
	private static string LOG_LOCATION = Application.persistentDataPath + Path.DirectorySeparatorChar + "error";

	// Token: 0x040012EC RID: 4844
	private static int errorCount = 0;

	// Token: 0x040012ED RID: 4845
	private bool loggingIn = true;

	// Token: 0x040012EE RID: 4846
	private int strikes;

	// Token: 0x040012EF RID: 4847
	private bool activeConnection = true;

	// Token: 0x040012F0 RID: 4848
	private ulong deactivatedTime;

	// Token: 0x040012F1 RID: 4849
	private CookieContainer cookies = new CookieContainer();

	// Token: 0x040012F2 RID: 4850
	private Dictionary<TFWebClient, TFServer.JsonStringHandler> reqs = new Dictionary<TFWebClient, TFServer.JsonStringHandler>();

	// Token: 0x040012F3 RID: 4851
	private bool unreachable;

	// Token: 0x020004BB RID: 1211
	// (Invoke) Token: 0x0600254F RID: 9551
	public delegate void JsonStringHandler(string jsonResponse, object userData);

	// Token: 0x020004BC RID: 1212
	// (Invoke) Token: 0x06002553 RID: 9555
	public delegate void JsonResponseHandler(Dictionary<string, object> dict, object userData);
}
