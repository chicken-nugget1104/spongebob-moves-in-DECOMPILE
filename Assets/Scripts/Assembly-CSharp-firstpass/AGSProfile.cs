using System;
using System.Collections;

// Token: 0x02000049 RID: 73
public class AGSProfile
{
	// Token: 0x0600026A RID: 618 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
	private AGSProfile()
	{
		this.alias = null;
		this.playerId = null;
		AGSClient.LogGameCircleError("AGSProfile was instantiated without valid playerId and alias information.");
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000C0E8 File Offset: 0x0000A2E8
	private AGSProfile(string alias, string playerId)
	{
		this.alias = alias;
		this.playerId = playerId;
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000C100 File Offset: 0x0000A300
	public static AGSProfile fromHashtable(Hashtable profileDataAsHashtable)
	{
		if (profileDataAsHashtable == null)
		{
			return null;
		}
		return new AGSProfile(AGSProfile.getStringValue(profileDataAsHashtable, "alias"), AGSProfile.getStringValue(profileDataAsHashtable, "playerId"));
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000C128 File Offset: 0x0000A328
	private static string getStringValue(Hashtable hashtable, string key)
	{
		if (hashtable == null)
		{
			return null;
		}
		if (hashtable.Contains(key))
		{
			return hashtable[key].ToString();
		}
		return null;
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000C158 File Offset: 0x0000A358
	public override string ToString()
	{
		return string.Format("alias: {0}, playerId: {1}", this.alias, this.playerId);
	}

	// Token: 0x0400019F RID: 415
	public readonly string alias;

	// Token: 0x040001A0 RID: 416
	public readonly string playerId;
}
