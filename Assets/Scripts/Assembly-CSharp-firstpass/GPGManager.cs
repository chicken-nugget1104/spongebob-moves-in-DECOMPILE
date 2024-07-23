using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x020000BB RID: 187
public class GPGManager : AbstractManager
{
	// Token: 0x0600070F RID: 1807 RVA: 0x0001B250 File Offset: 0x00019450
	static GPGManager()
	{
		AbstractManager.initialize(typeof(GPGManager));
	}

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06000710 RID: 1808 RVA: 0x0001B264 File Offset: 0x00019464
	// (remove) Token: 0x06000711 RID: 1809 RVA: 0x0001B27C File Offset: 0x0001947C
	public static event Action<string> authenticationSucceededEvent;

	// Token: 0x140000AA RID: 170
	// (add) Token: 0x06000712 RID: 1810 RVA: 0x0001B294 File Offset: 0x00019494
	// (remove) Token: 0x06000713 RID: 1811 RVA: 0x0001B2AC File Offset: 0x000194AC
	public static event Action<string> authenticationFailedEvent;

	// Token: 0x140000AB RID: 171
	// (add) Token: 0x06000714 RID: 1812 RVA: 0x0001B2C4 File Offset: 0x000194C4
	// (remove) Token: 0x06000715 RID: 1813 RVA: 0x0001B2DC File Offset: 0x000194DC
	public static event Action userSignedOutEvent;

	// Token: 0x140000AC RID: 172
	// (add) Token: 0x06000716 RID: 1814 RVA: 0x0001B2F4 File Offset: 0x000194F4
	// (remove) Token: 0x06000717 RID: 1815 RVA: 0x0001B30C File Offset: 0x0001950C
	public static event Action<string> reloadDataForKeyFailedEvent;

	// Token: 0x140000AD RID: 173
	// (add) Token: 0x06000718 RID: 1816 RVA: 0x0001B324 File Offset: 0x00019524
	// (remove) Token: 0x06000719 RID: 1817 RVA: 0x0001B33C File Offset: 0x0001953C
	public static event Action<string> reloadDataForKeySucceededEvent;

	// Token: 0x140000AE RID: 174
	// (add) Token: 0x0600071A RID: 1818 RVA: 0x0001B354 File Offset: 0x00019554
	// (remove) Token: 0x0600071B RID: 1819 RVA: 0x0001B36C File Offset: 0x0001956C
	public static event Action licenseCheckFailedEvent;

	// Token: 0x140000AF RID: 175
	// (add) Token: 0x0600071C RID: 1820 RVA: 0x0001B384 File Offset: 0x00019584
	// (remove) Token: 0x0600071D RID: 1821 RVA: 0x0001B39C File Offset: 0x0001959C
	public static event Action<string> profileImageLoadedAtPathEvent;

	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x0600071E RID: 1822 RVA: 0x0001B3B4 File Offset: 0x000195B4
	// (remove) Token: 0x0600071F RID: 1823 RVA: 0x0001B3CC File Offset: 0x000195CC
	public static event Action<string> loadCloudDataForKeyFailedEvent;

	// Token: 0x140000B1 RID: 177
	// (add) Token: 0x06000720 RID: 1824 RVA: 0x0001B3E4 File Offset: 0x000195E4
	// (remove) Token: 0x06000721 RID: 1825 RVA: 0x0001B3FC File Offset: 0x000195FC
	public static event Action<int, string> loadCloudDataForKeySucceededEvent;

	// Token: 0x140000B2 RID: 178
	// (add) Token: 0x06000722 RID: 1826 RVA: 0x0001B414 File Offset: 0x00019614
	// (remove) Token: 0x06000723 RID: 1827 RVA: 0x0001B42C File Offset: 0x0001962C
	public static event Action<string> updateCloudDataForKeyFailedEvent;

	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x06000724 RID: 1828 RVA: 0x0001B444 File Offset: 0x00019644
	// (remove) Token: 0x06000725 RID: 1829 RVA: 0x0001B45C File Offset: 0x0001965C
	public static event Action<int, string> updateCloudDataForKeySucceededEvent;

	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06000726 RID: 1830 RVA: 0x0001B474 File Offset: 0x00019674
	// (remove) Token: 0x06000727 RID: 1831 RVA: 0x0001B48C File Offset: 0x0001968C
	public static event Action<string> clearCloudDataForKeyFailedEvent;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06000728 RID: 1832 RVA: 0x0001B4A4 File Offset: 0x000196A4
	// (remove) Token: 0x06000729 RID: 1833 RVA: 0x0001B4BC File Offset: 0x000196BC
	public static event Action<string> clearCloudDataForKeySucceededEvent;

	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x0600072A RID: 1834 RVA: 0x0001B4D4 File Offset: 0x000196D4
	// (remove) Token: 0x0600072B RID: 1835 RVA: 0x0001B4EC File Offset: 0x000196EC
	public static event Action<string> deleteCloudDataForKeyFailedEvent;

	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x0600072C RID: 1836 RVA: 0x0001B504 File Offset: 0x00019704
	// (remove) Token: 0x0600072D RID: 1837 RVA: 0x0001B51C File Offset: 0x0001971C
	public static event Action<string> deleteCloudDataForKeySucceededEvent;

	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x0600072E RID: 1838 RVA: 0x0001B534 File Offset: 0x00019734
	// (remove) Token: 0x0600072F RID: 1839 RVA: 0x0001B54C File Offset: 0x0001974C
	public static event Action<string, string> unlockAchievementFailedEvent;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x06000730 RID: 1840 RVA: 0x0001B564 File Offset: 0x00019764
	// (remove) Token: 0x06000731 RID: 1841 RVA: 0x0001B57C File Offset: 0x0001977C
	public static event Action<string, bool> unlockAchievementSucceededEvent;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x06000732 RID: 1842 RVA: 0x0001B594 File Offset: 0x00019794
	// (remove) Token: 0x06000733 RID: 1843 RVA: 0x0001B5AC File Offset: 0x000197AC
	public static event Action<string, string> incrementAchievementFailedEvent;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x06000734 RID: 1844 RVA: 0x0001B5C4 File Offset: 0x000197C4
	// (remove) Token: 0x06000735 RID: 1845 RVA: 0x0001B5DC File Offset: 0x000197DC
	public static event Action<string, bool> incrementAchievementSucceededEvent;

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x06000736 RID: 1846 RVA: 0x0001B5F4 File Offset: 0x000197F4
	// (remove) Token: 0x06000737 RID: 1847 RVA: 0x0001B60C File Offset: 0x0001980C
	public static event Action<string, string> revealAchievementFailedEvent;

	// Token: 0x140000BD RID: 189
	// (add) Token: 0x06000738 RID: 1848 RVA: 0x0001B624 File Offset: 0x00019824
	// (remove) Token: 0x06000739 RID: 1849 RVA: 0x0001B63C File Offset: 0x0001983C
	public static event Action<string> revealAchievementSucceededEvent;

	// Token: 0x140000BE RID: 190
	// (add) Token: 0x0600073A RID: 1850 RVA: 0x0001B654 File Offset: 0x00019854
	// (remove) Token: 0x0600073B RID: 1851 RVA: 0x0001B66C File Offset: 0x0001986C
	public static event Action<string, string> submitScoreFailedEvent;

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x0600073C RID: 1852 RVA: 0x0001B684 File Offset: 0x00019884
	// (remove) Token: 0x0600073D RID: 1853 RVA: 0x0001B69C File Offset: 0x0001989C
	public static event Action<string, Dictionary<string, object>> submitScoreSucceededEvent;

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x0600073E RID: 1854 RVA: 0x0001B6B4 File Offset: 0x000198B4
	// (remove) Token: 0x0600073F RID: 1855 RVA: 0x0001B6CC File Offset: 0x000198CC
	public static event Action<string, string> loadScoresFailedEvent;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x06000740 RID: 1856 RVA: 0x0001B6E4 File Offset: 0x000198E4
	// (remove) Token: 0x06000741 RID: 1857 RVA: 0x0001B6FC File Offset: 0x000198FC
	public static event Action<List<GPGScore>> loadScoresSucceededEvent;

	// Token: 0x06000742 RID: 1858 RVA: 0x0001B714 File Offset: 0x00019914
	private void fireEventWithIdentifierAndError(Action<string, string> theEvent, string json)
	{
		if (theEvent == null)
		{
			return;
		}
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		if (dictionary != null && dictionary.ContainsKey("identifier") && dictionary.ContainsKey("error"))
		{
			theEvent(dictionary["identifier"].ToString(), dictionary["error"].ToString());
		}
		else
		{
			Debug.LogError("json could not be deserialized to an identifier and an error: " + json);
		}
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0001B790 File Offset: 0x00019990
	private void fireEventWithIdentifierAndBool(Action<string, bool> theEvent, string param)
	{
		if (theEvent == null)
		{
			return;
		}
		string[] array = param.Split(new char[]
		{
			','
		});
		if (array.Length == 2)
		{
			theEvent(array[0], array[1] == "1");
		}
		else
		{
			Debug.LogError("param could not be deserialized to an identifier and an error: " + param);
		}
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0001B7EC File Offset: 0x000199EC
	public void userSignedOut(string empty)
	{
		GPGManager.userSignedOutEvent.fire();
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x0001B7F8 File Offset: 0x000199F8
	public void reloadDataForKeyFailed(string error)
	{
		GPGManager.reloadDataForKeyFailedEvent.fire(error);
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0001B808 File Offset: 0x00019A08
	public void reloadDataForKeySucceeded(string param)
	{
		GPGManager.reloadDataForKeySucceededEvent.fire(param);
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0001B818 File Offset: 0x00019A18
	public void licenseCheckFailed(string param)
	{
		GPGManager.licenseCheckFailedEvent.fire();
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0001B824 File Offset: 0x00019A24
	public void profileImageLoadedAtPath(string path)
	{
		GPGManager.profileImageLoadedAtPathEvent.fire(path);
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x0001B834 File Offset: 0x00019A34
	public void loadCloudDataForKeyFailed(string error)
	{
		GPGManager.loadCloudDataForKeyFailedEvent.fire(error);
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0001B844 File Offset: 0x00019A44
	public void loadCloudDataForKeySucceeded(string json)
	{
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		GPGManager.loadCloudDataForKeySucceededEvent.fire(int.Parse(dictionary["key"].ToString()), dictionary["data"].ToString());
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0001B888 File Offset: 0x00019A88
	public void updateCloudDataForKeyFailed(string error)
	{
		GPGManager.updateCloudDataForKeyFailedEvent.fire(error);
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0001B898 File Offset: 0x00019A98
	public void updateCloudDataForKeySucceeded(string json)
	{
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		GPGManager.updateCloudDataForKeySucceededEvent.fire(int.Parse(dictionary["key"].ToString()), dictionary["data"].ToString());
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0001B8DC File Offset: 0x00019ADC
	public void clearCloudDataForKeyFailed(string error)
	{
		GPGManager.clearCloudDataForKeyFailedEvent.fire(error);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0001B8EC File Offset: 0x00019AEC
	public void clearCloudDataForKeySucceeded(string param)
	{
		GPGManager.clearCloudDataForKeySucceededEvent.fire(param);
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0001B8FC File Offset: 0x00019AFC
	public void deleteCloudDataForKeyFailed(string error)
	{
		GPGManager.deleteCloudDataForKeyFailedEvent.fire(error);
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0001B90C File Offset: 0x00019B0C
	public void deleteCloudDataForKeySucceeded(string param)
	{
		GPGManager.deleteCloudDataForKeySucceededEvent.fire(param);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0001B91C File Offset: 0x00019B1C
	public void unlockAchievementFailed(string json)
	{
		this.fireEventWithIdentifierAndError(GPGManager.unlockAchievementFailedEvent, json);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0001B92C File Offset: 0x00019B2C
	public void unlockAchievementSucceeded(string param)
	{
		this.fireEventWithIdentifierAndBool(GPGManager.unlockAchievementSucceededEvent, param);
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0001B93C File Offset: 0x00019B3C
	public void incrementAchievementFailed(string json)
	{
		this.fireEventWithIdentifierAndError(GPGManager.incrementAchievementFailedEvent, json);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0001B94C File Offset: 0x00019B4C
	public void incrementAchievementSucceeded(string param)
	{
		string[] array = param.Split(new char[]
		{
			','
		});
		if (array.Length == 2)
		{
			GPGManager.incrementAchievementSucceededEvent.fire(array[0], array[1] == "1");
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0001B990 File Offset: 0x00019B90
	public void revealAchievementFailed(string json)
	{
		this.fireEventWithIdentifierAndError(GPGManager.revealAchievementFailedEvent, json);
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0001B9A0 File Offset: 0x00019BA0
	public void revealAchievementSucceeded(string achievementId)
	{
		GPGManager.revealAchievementSucceededEvent.fire(achievementId);
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0001B9B0 File Offset: 0x00019BB0
	public void submitScoreFailed(string json)
	{
		this.fireEventWithIdentifierAndError(GPGManager.submitScoreFailedEvent, json);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0001B9C0 File Offset: 0x00019BC0
	public void submitScoreSucceeded(string json)
	{
		if (GPGManager.submitScoreSucceededEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			string arg = "Unknown";
			if (dictionary.ContainsKey("leaderboardId"))
			{
				arg = dictionary["leaderboardId"].ToString();
			}
			GPGManager.submitScoreSucceededEvent(arg, dictionary);
		}
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0001BA14 File Offset: 0x00019C14
	public void loadScoresFailed(string json)
	{
		this.fireEventWithIdentifierAndError(GPGManager.loadScoresFailedEvent, json);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0001BA24 File Offset: 0x00019C24
	public void loadScoresSucceeded(string json)
	{
		GPGManager.loadScoresSucceededEvent.fire(DTOBase.listFromJson<GPGScore>(json));
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0001BA38 File Offset: 0x00019C38
	public void authenticationSucceeded(string param)
	{
		GPGManager.authenticationSucceededEvent.fire(param);
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x0001BA48 File Offset: 0x00019C48
	public void authenticationFailed(string error)
	{
		GPGManager.authenticationFailedEvent.fire(error);
	}
}
