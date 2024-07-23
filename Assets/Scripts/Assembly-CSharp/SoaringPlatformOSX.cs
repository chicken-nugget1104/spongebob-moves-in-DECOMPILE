using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

// Token: 0x0200037F RID: 895
public class SoaringPlatformOSX : SoaringPlatform.SoaringPlatformDelegate
{
	// Token: 0x0600198D RID: 6541 RVA: 0x000A7CE4 File Offset: 0x000A5EE4
	public override void Init()
	{
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x000A7CE8 File Offset: 0x000A5EE8
	public override SoaringLoginType PreferedLoginType()
	{
		return SoaringLoginType.Soaring;
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x000A7CEC File Offset: 0x000A5EEC
	public override string PlatformName()
	{
		return "MacOSX";
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000A7CF4 File Offset: 0x000A5EF4
	public override string DeviceID()
	{
		return Environment.MachineName;
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x000A7CFC File Offset: 0x000A5EFC
	public override SoaringDictionary GenerateDeviceDictionary()
	{
		return new SoaringDictionary();
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x000A7D04 File Offset: 0x000A5F04
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

	// Token: 0x06001993 RID: 6547 RVA: 0x000A7D24 File Offset: 0x000A5F24
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

	// Token: 0x06001994 RID: 6548 RVA: 0x000A7DAC File Offset: 0x000A5FAC
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
