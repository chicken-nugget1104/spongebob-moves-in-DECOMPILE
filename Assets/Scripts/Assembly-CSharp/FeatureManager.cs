using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000170 RID: 368
public class FeatureManager
{
	// Token: 0x06000C61 RID: 3169 RVA: 0x0004A764 File Offset: 0x00048964
	public FeatureManager()
	{
		this.unlockedFeatures = new HashSet<string>();
		this.featureLocks = new Dictionary<string, FeatureLock>();
		this.LoadFeatures();
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x0004A7A0 File Offset: 0x000489A0
	private string[] GetFilesToLoad()
	{
		return Config.FEATURE_DATA_PATH;
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x0004A7A8 File Offset: 0x000489A8
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000C65 RID: 3173 RVA: 0x0004A7AC File Offset: 0x000489AC
	public HashSet<string> ActiveFeatures
	{
		get
		{
			return this.unlockedFeatures;
		}
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x0004A7B4 File Offset: 0x000489B4
	private void LoadFeatures()
	{
		string[] filesToLoad = this.GetFilesToLoad();
		foreach (string filePath in filesToLoad)
		{
			string filePathFromString = this.GetFilePathFromString(filePath);
			TFUtils.DebugLog("Loading features: " + filePathFromString, TFUtils.LogFilter.Features);
			string json = TFUtils.ReadAllText(filePathFromString);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
			string text = (string)dictionary["type"];
			if (text != null)
			{
				if (FeatureManager.<>f__switch$mapC == null)
				{
					FeatureManager.<>f__switch$mapC = new Dictionary<string, int>(1)
					{
						{
							"unlock",
							0
						}
					};
				}
				int num;
				if (FeatureManager.<>f__switch$mapC.TryGetValue(text, out num))
				{
					if (num == 0)
					{
						FeatureLock featureLock = new FeatureLock(dictionary);
						this.featureLocks.Add(featureLock.Feature, featureLock);
					}
				}
			}
		}
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x0004A898 File Offset: 0x00048A98
	public bool CheckFeature(string feature)
	{
		return !this.featureLocks.ContainsKey(feature) || this.unlockedFeatures.Contains(feature);
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x0004A8BC File Offset: 0x00048ABC
	public void UnlockFeature(string feature)
	{
		if (this.featureLocks.ContainsKey(feature) && !this.unlockedFeatures.Contains(feature))
		{
			this.unlockedFeatures.Add(feature);
		}
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x0004A8F0 File Offset: 0x00048AF0
	public void UnlockAllFeatures()
	{
		foreach (KeyValuePair<string, FeatureLock> keyValuePair in this.featureLocks)
		{
			string key = keyValuePair.Key;
			if (!this.unlockedFeatures.Contains(key))
			{
				this.unlockedFeatures.Add(key);
			}
		}
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x0004A978 File Offset: 0x00048B78
	public void UnlockAllFeaturesToGamestate(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("features"))
		{
			dictionary["features"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["features"];
		foreach (KeyValuePair<string, FeatureLock> keyValuePair in this.featureLocks)
		{
			string key = keyValuePair.Key;
			if (!list.Contains(key))
			{
				list.Add(key);
			}
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x0004AA38 File Offset: 0x00048C38
	public void ActivateFeatureActions(Game game, string feature)
	{
		this.featureLocks[feature].Activate(game);
	}

	// Token: 0x0400084D RID: 2125
	private static readonly string FEATURE_DATA_PATH = "Features";

	// Token: 0x0400084E RID: 2126
	private HashSet<string> unlockedFeatures;

	// Token: 0x0400084F RID: 2127
	private Dictionary<string, FeatureLock> featureLocks;
}
