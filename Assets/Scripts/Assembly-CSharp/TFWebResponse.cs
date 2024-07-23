using System;
using System.Collections.Generic;
using System.Net;
using MiniJSON;

// Token: 0x020003E4 RID: 996
public class TFWebResponse
{
	// Token: 0x06001E8D RID: 7821 RVA: 0x000BC4B8 File Offset: 0x000BA6B8
	public Dictionary<string, object> GetAsJSONDict()
	{
		if (this.Data != null && this.StatusCode == HttpStatusCode.OK)
		{
			object obj = Json.Deserialize(this.Data);
			if (obj.GetType() == typeof(Dictionary<string, object>))
			{
				return (Dictionary<string, object>)Json.Deserialize(this.Data);
			}
		}
		return null;
	}

	// Token: 0x040012F4 RID: 4852
	public HttpStatusCode StatusCode;

	// Token: 0x040012F5 RID: 4853
	public string Data;

	// Token: 0x040012F6 RID: 4854
	public WebHeaderCollection Headers;

	// Token: 0x040012F7 RID: 4855
	public bool NetworkDown;

	// Token: 0x040012F8 RID: 4856
	public Exception Error;
}
