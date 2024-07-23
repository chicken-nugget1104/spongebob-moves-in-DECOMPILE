using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x02000380 RID: 896
public class SoaringPlatformWindows : SoaringPlatform.SoaringPlatformDelegate
{
	// Token: 0x06001996 RID: 6550 RVA: 0x000A7E18 File Offset: 0x000A6018
	public override void Init()
	{
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x000A7E1C File Offset: 0x000A601C
	public override SoaringLoginType PreferedLoginType()
	{
		return SoaringLoginType.Soaring;
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x000A7E20 File Offset: 0x000A6020
	public override string PlatformName()
	{
		return "Windows";
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x000A7E28 File Offset: 0x000A6028
	public override string DeviceID()
	{
		return Environment.MachineName;
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x000A7E30 File Offset: 0x000A6030
	public override SoaringDictionary GenerateDeviceDictionary()
	{
		return new SoaringDictionary();
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000A7E38 File Offset: 0x000A6038
	public override bool OpenURL(string url)
	{
		bool result = false;
		if (url == null)
		{
			return result;
		}
		Application.OpenURL(url);
		return true;
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000A7E58 File Offset: 0x000A6058
	public override bool SendEmail(string subject, string body, string email)
	{
		bool result = false;
		if (subject == null || body == null || email == null)
		{
			return result;
		}
		subject = WWW.EscapeURL(subject).Replace("+", "%20");
		body = WWW.EscapeURL(body).Replace("+", "%20");
		Application.OpenURL(string.Concat(new string[]
		{
			"mailto:",
			email,
			"?subject=",
			subject,
			"&body=",
			body
		}));
		return true;
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x000A7EE0 File Offset: 0x000A60E0
	public override bool OpenPath(string path)
	{
		bool result = false;
		if (path == null)
		{
			return result;
		}
		try
		{
			if (File.Exists(path))
			{
				Process.Start("open", path);
			}
			result = true;
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
		return result;
	}
}
