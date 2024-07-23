using System;
using System.Collections;

// Token: 0x02000042 RID: 66
public class AGSAchievement
{
	// Token: 0x06000229 RID: 553 RVA: 0x0000B1DC File Offset: 0x000093DC
	public static AGSAchievement fromHashtable(Hashtable ht)
	{
		AGSAchievement agsachievement = new AGSAchievement();
		agsachievement.title = AGSAchievement.getStringValue(ht, "title");
		agsachievement.id = AGSAchievement.getStringValue(ht, "id");
		agsachievement.decription = AGSAchievement.getStringValue(ht, "description");
		try
		{
			string stringValue = AGSAchievement.getStringValue(ht, "pointValue");
			agsachievement.pointValue = int.Parse(stringValue);
		}
		catch (FormatException ex)
		{
			AGSClient.Log("Unable to parse pointValue from achievement " + ex.Message);
		}
		catch (ArgumentNullException ex2)
		{
			AGSClient.Log("pointValue not found  " + ex2.Message);
		}
		try
		{
			string stringValue2 = AGSAchievement.getStringValue(ht, "position");
			agsachievement.position = int.Parse(stringValue2);
		}
		catch (FormatException ex3)
		{
			AGSClient.Log("Unable to parse position from achievement " + ex3.Message);
		}
		catch (ArgumentNullException ex4)
		{
			AGSClient.Log("position not found " + ex4.Message);
		}
		try
		{
			string stringValue3 = AGSAchievement.getStringValue(ht, "progress");
			agsachievement.progress = float.Parse(stringValue3);
		}
		catch (FormatException ex5)
		{
			AGSClient.Log("Unable to parse progress from achievement " + ex5.Message);
		}
		catch (ArgumentNullException ex6)
		{
			AGSClient.Log("progress not found " + ex6.Message);
		}
		try
		{
			string stringValue4 = AGSAchievement.getStringValue(ht, "hidden");
			agsachievement.isHidden = bool.Parse(stringValue4);
		}
		catch (FormatException ex7)
		{
			AGSClient.Log("Unable to parse isHidden from achievement " + ex7.Message);
		}
		catch (ArgumentNullException ex8)
		{
			AGSClient.Log("isHidden not found " + ex8.Message);
		}
		try
		{
			string stringValue5 = AGSAchievement.getStringValue(ht, "unlocked");
			agsachievement.isUnlocked = bool.Parse(stringValue5);
		}
		catch (FormatException ex9)
		{
			AGSClient.Log("Unable to parse isUnlocked from achievement " + ex9.Message);
		}
		catch (ArgumentNullException ex10)
		{
			AGSClient.Log("isUnlocked not found " + ex10.Message);
		}
		try
		{
			string stringValue6 = AGSAchievement.getStringValue(ht, "dateUnlocked");
			long num = long.Parse(stringValue6);
			agsachievement.dateUnlocked = AGSAchievement.getTimefromEpochTime((double)num);
		}
		catch (FormatException ex11)
		{
			AGSClient.Log("Unable to parse dateUnlocked from achievement " + ex11.Message);
		}
		catch (ArgumentNullException ex12)
		{
			AGSClient.Log("dateUnlocked not found " + ex12.Message);
		}
		return agsachievement;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000B564 File Offset: 0x00009764
	private static DateTime getTimefromEpochTime(double javaTimeStamp)
	{
		DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		result = result.AddSeconds(Math.Round(javaTimeStamp / 1000.0)).ToLocalTime();
		return result;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000B5A4 File Offset: 0x000097A4
	private static string getStringValue(Hashtable ht, string key)
	{
		if (ht.Contains(key))
		{
			return ht[key].ToString();
		}
		return null;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000B5C0 File Offset: 0x000097C0
	public override string ToString()
	{
		return string.Format("title: {0}, id: {1}, pointValue: {2}, hidden: {3}, unlocked: {4}, progress: {5}, position: {6}, date: {7} ", new object[]
		{
			this.title,
			this.id,
			this.pointValue,
			this.isHidden,
			this.isUnlocked,
			this.progress,
			this.position,
			this.dateUnlocked
		});
	}

	// Token: 0x0400017B RID: 379
	public string title;

	// Token: 0x0400017C RID: 380
	public string id;

	// Token: 0x0400017D RID: 381
	public int pointValue;

	// Token: 0x0400017E RID: 382
	public bool isHidden;

	// Token: 0x0400017F RID: 383
	public bool isUnlocked;

	// Token: 0x04000180 RID: 384
	public float progress;

	// Token: 0x04000181 RID: 385
	public int position;

	// Token: 0x04000182 RID: 386
	public string decription;

	// Token: 0x04000183 RID: 387
	public DateTime dateUnlocked;
}
