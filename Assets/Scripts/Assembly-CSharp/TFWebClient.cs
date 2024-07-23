using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using Ionic.Zlib;

// Token: 0x0200047F RID: 1151
public class TFWebClient : IDisposable
{
	// Token: 0x06002406 RID: 9222 RVA: 0x000DD9E0 File Offset: 0x000DBBE0
	public TFWebClient(CookieContainer cookieContainer)
	{
		this.cookies = cookieContainer;
		this.client = new TFWebClient.TFCustomWebClient(this.cookies);
		this.client.UploadDataCompleted += new UploadDataCompletedEventHandler(this.OnCallComplete);
		this.client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(this.OnCallComplete);
		TFUtils.SetDefaultHeaders(this.Headers);
	}

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06002407 RID: 9223 RVA: 0x000DDA44 File Offset: 0x000DBC44
	// (remove) Token: 0x06002408 RID: 9224 RVA: 0x000DDA60 File Offset: 0x000DBC60
	public event TFWebClient.OnNetworkError NetworkError;

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06002409 RID: 9225 RVA: 0x000DDA7C File Offset: 0x000DBC7C
	public TFWebResponse Response
	{
		get
		{
			return this.response;
		}
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000DDA84 File Offset: 0x000DBC84
	public void Get(Uri address, TFWebClient.GetCallbackHandler response, object userData = null)
	{
		if (address == null)
		{
			TFUtils.ErrorLog("TFWebClient.Get - null address");
			return;
		}
		if (response == null)
		{
			TFUtils.ErrorLog("TFWebClient.Get - null response");
			return;
		}
		this.setURI(address);
		this.UserData = userData;
		TFWebClient.CallbackInfo callbackInfo = new TFWebClient.CallbackInfo();
		callbackInfo.Client = this;
		callbackInfo.Callback = response;
		callbackInfo.URI = address;
		callbackInfo.UserData = userData;
		callbackInfo.Method = "GET";
		this.client.DownloadDataAsync(address, callbackInfo);
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000DDB04 File Offset: 0x000DBD04
	public void Put(Uri address, byte[] saveData, TFWebClient.GetCallbackHandler response, object userData = null)
	{
		this.Upload("PUT", address, saveData, response, userData);
	}

	// Token: 0x0600240C RID: 9228 RVA: 0x000DDB18 File Offset: 0x000DBD18
	public void Post(Uri address, byte[] saveData, TFWebClient.GetCallbackHandler response, object userData = null)
	{
		this.Upload("POST", address, saveData, response, userData);
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x000DDB2C File Offset: 0x000DBD2C
	public void Upload(string method, Uri address, byte[] saveData, TFWebClient.GetCallbackHandler response, object userData = null)
	{
		this.setURI(address);
		this.UserData = userData;
		TFWebClient.CallbackInfo callbackInfo = new TFWebClient.CallbackInfo();
		callbackInfo.Callback = response;
		callbackInfo.URI = address;
		callbackInfo.UserData = userData;
		callbackInfo.Method = method;
		callbackInfo.RequestData = saveData;
		this.client.UploadDataAsync(address, method, saveData, callbackInfo);
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x000DDB84 File Offset: 0x000DBD84
	public void Put(Uri address, string saveData, TFWebClient.GetCallbackHandler response, object userData = null)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(saveData);
		this.Put(address, bytes, response, userData);
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x000DDBA8 File Offset: 0x000DBDA8
	public void UploadLogDump(Uri address, Dictionary<string, object> postParams, TFWebClient.GetCallbackHandler response, object userData = null)
	{
		this.setURI(address);
		this.UserData = postParams;
		TFWebClient.CallbackInfo callbackInfo = new TFWebClient.CallbackInfo();
		callbackInfo.Callback = response;
		callbackInfo.URI = address;
		callbackInfo.UserData = postParams;
		callbackInfo.Method = "POST";
		callbackInfo.RequestData = null;
		HttpWebResponse httpWebResponse = TFFormPost.PostForm(address, "Innertube Explorer v0.1", postParams, this.cookies);
		Stream responseStream = httpWebResponse.GetResponseStream();
		StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
		TFUtils.DebugLog("Log Dump Complete: " + streamReader.ReadToEnd());
		httpWebResponse.Close();
		streamReader.Close();
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x000DDC38 File Offset: 0x000DBE38
	private void setURI(Uri address)
	{
		if (this.client.BaseAddress == null || this.client.BaseAddress.Length == 0)
		{
			this.URI = address;
		}
		else
		{
			this.URI = new Uri(new Uri(this.client.BaseAddress), address);
		}
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000DDC94 File Offset: 0x000DBE94
	private void retryRequest(TFWebClient.CallbackInfo info)
	{
		this.retryCount++;
		this.client.CancelAsync();
		TFUtils.DebugLog(string.Format("Retrying {0} for the {1} attempt", info.URI, this.retryCount));
		if (info.Method == "GET")
		{
			this.Get(info.URI, info.Callback, info.UserData);
		}
		else
		{
			this.Upload(info.Method, info.URI, info.RequestData, info.Callback, info.UserData);
		}
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x000DDD30 File Offset: 0x000DBF30
	private bool IsExpectedStatus(Exception ex)
	{
		if (ex.GetType().IsAssignableFrom(typeof(WebException)))
		{
			WebException ex2 = (WebException)ex;
			if (ex2 != null)
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)ex2.Response;
				TFUtils.DebugLog(ex);
				if (httpWebResponse != null)
				{
					if (httpWebResponse.StatusCode >= HttpStatusCode.InternalServerError)
					{
						return false;
					}
				}
				else if (ex2.Response == null)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000DDDA4 File Offset: 0x000DBFA4
	protected void OnCallComplete(object sender, AsyncCompletedEventArgs e)
	{
		try
		{
			WebClient webClient = (WebClient)sender;
			this.response = new TFWebResponse();
			TFWebClient.CallbackInfo callbackInfo = (TFWebClient.CallbackInfo)e.UserState;
			this.response.Error = e.Error;
			if (e.Error == null && !e.Cancelled)
			{
				this.response.StatusCode = HttpStatusCode.OK;
				if (e is DownloadDataCompletedEventArgs)
				{
					DownloadDataCompletedEventArgs downloadDataCompletedEventArgs = (DownloadDataCompletedEventArgs)e;
					try
					{
						this.response.Data = TFUtils.Unzip(downloadDataCompletedEventArgs.Result);
					}
					catch (ZlibException)
					{
						this.response.Data = Encoding.UTF8.GetString(downloadDataCompletedEventArgs.Result);
					}
				}
				else if (e is UploadDataCompletedEventArgs)
				{
					UploadDataCompletedEventArgs uploadDataCompletedEventArgs = (UploadDataCompletedEventArgs)e;
					this.response.Data = Encoding.UTF8.GetString(uploadDataCompletedEventArgs.Result);
				}
				this.response.Headers = webClient.ResponseHeaders;
				callbackInfo.Callback(this);
			}
			else if (e.Error != null)
			{
				if (this.retryCount < SBSettings.NETWORK_RETRY_COUNT && !this.IsExpectedStatus(e.Error))
				{
					TFUtils.DebugLog("Going to retry ");
					TFUtils.DebugLog(e);
					this.retryRequest(callbackInfo);
				}
				else
				{
					TFUtils.DebugLog("Going to call network error" + this.URI);
					if (e.Error.GetType().Name == "WebException")
					{
						WebException ex = (WebException)e.Error;
						TFUtils.DebugLog(ex);
						HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;
						if (httpWebResponse != null)
						{
							this.PopulateResponse(this.response, httpWebResponse);
						}
						else
						{
							this.response.StatusCode = HttpStatusCode.ServiceUnavailable;
							this.response.NetworkDown = true;
						}
						if (this.NetworkError != null)
						{
							TFUtils.DebugLog("Calling network error");
							this.NetworkError(this, ex);
						}
					}
					else
					{
						TFUtils.DebugLog("Server returned error");
						TFUtils.DebugLog(e.Error);
						this.response.NetworkDown = true;
						this.response.StatusCode = HttpStatusCode.ServiceUnavailable;
					}
					callbackInfo.Callback(this);
				}
			}
		}
		catch (Exception message)
		{
			TFUtils.DebugLog(message);
		}
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000DE028 File Offset: 0x000DC228
	private void PopulateResponse(TFWebResponse response, HttpWebResponse httpRes)
	{
		response.StatusCode = httpRes.StatusCode;
		response.Headers = httpRes.Headers;
		try
		{
			byte[] array = new byte[4096];
			Stream responseStream = httpRes.GetResponseStream();
			MemoryStream memoryStream = new MemoryStream();
			int count;
			while ((count = responseStream.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, count);
			}
			Encoding utf = Encoding.UTF8;
			response.Data = utf.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
		}
		catch (Exception message)
		{
			TFUtils.DebugLog(message);
		}
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000DE0DC File Offset: 0x000DC2DC
	public void Dispose()
	{
		this.client.Dispose();
	}

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06002416 RID: 9238 RVA: 0x000DE0EC File Offset: 0x000DC2EC
	// (set) Token: 0x06002417 RID: 9239 RVA: 0x000DE0FC File Offset: 0x000DC2FC
	public WebHeaderCollection Headers
	{
		get
		{
			return this.client.Headers;
		}
		set
		{
			this.client.Headers = value;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06002418 RID: 9240 RVA: 0x000DE10C File Offset: 0x000DC30C
	public WebHeaderCollection ResponseHeaders
	{
		get
		{
			return this.client.ResponseHeaders;
		}
	}

	// Token: 0x04001634 RID: 5684
	private const int TIMEOUT = 10000;

	// Token: 0x04001635 RID: 5685
	private const string USER_AGENT = "Innertube Explorer v0.1";

	// Token: 0x04001636 RID: 5686
	private CookieContainer cookies;

	// Token: 0x04001637 RID: 5687
	private WebClient client;

	// Token: 0x04001638 RID: 5688
	private TFWebResponse response;

	// Token: 0x04001639 RID: 5689
	private int retryCount;

	// Token: 0x0400163A RID: 5690
	public Uri URI;

	// Token: 0x0400163B RID: 5691
	public object UserData;

	// Token: 0x02000480 RID: 1152
	private class CallbackInfo
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000DE124 File Offset: 0x000DC324
		// (set) Token: 0x0600241B RID: 9243 RVA: 0x000DE12C File Offset: 0x000DC32C
		public TFWebClient.GetCallbackHandler Callback { get; set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000DE138 File Offset: 0x000DC338
		// (set) Token: 0x0600241D RID: 9245 RVA: 0x000DE140 File Offset: 0x000DC340
		public object UserData { get; set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x000DE14C File Offset: 0x000DC34C
		// (set) Token: 0x0600241F RID: 9247 RVA: 0x000DE154 File Offset: 0x000DC354
		public Uri URI { get; set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x000DE160 File Offset: 0x000DC360
		// (set) Token: 0x06002421 RID: 9249 RVA: 0x000DE168 File Offset: 0x000DC368
		public string Method { get; set; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x000DE174 File Offset: 0x000DC374
		// (set) Token: 0x06002423 RID: 9251 RVA: 0x000DE17C File Offset: 0x000DC37C
		public byte[] RequestData { get; set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000DE188 File Offset: 0x000DC388
		// (set) Token: 0x06002425 RID: 9253 RVA: 0x000DE190 File Offset: 0x000DC390
		public TFWebClient Client { get; set; }
	}

	// Token: 0x02000481 RID: 1153
	private class TFCustomWebClient : WebClient
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x000DE19C File Offset: 0x000DC39C
		public TFCustomWebClient(CookieContainer cookies)
		{
			this.cookies = cookies;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000DE1AC File Offset: 0x000DC3AC
		protected override WebRequest GetWebRequest(Uri address)
		{
			HttpWebRequest httpWebRequest = base.GetWebRequest(address) as HttpWebRequest;
			httpWebRequest.CookieContainer = this.cookies;
			httpWebRequest.Timeout = 10000;
			httpWebRequest.UserAgent = "Innertube Explorer v0.1";
			return httpWebRequest;
		}

		// Token: 0x04001643 RID: 5699
		private CookieContainer cookies;
	}

	// Token: 0x020004C1 RID: 1217
	// (Invoke) Token: 0x06002567 RID: 9575
	public delegate void OnNetworkError(TFWebClient client, WebException e);

	// Token: 0x020004C2 RID: 1218
	// (Invoke) Token: 0x0600256B RID: 9579
	public delegate void GetCallbackHandler(TFWebClient client);

	// Token: 0x020004C3 RID: 1219
	// (Invoke) Token: 0x0600256F RID: 9583
	public delegate void PutCallbackHandler(TFWebClient client);
}
