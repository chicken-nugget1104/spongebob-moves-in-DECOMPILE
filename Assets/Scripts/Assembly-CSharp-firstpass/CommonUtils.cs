using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005C RID: 92
public static class CommonUtils
{
	// Token: 0x060002EA RID: 746 RVA: 0x0000D424 File Offset: 0x0000B624
	public static void SetMemoryLevel(int ml)
	{
		CommonUtils.MemoryLevel = ml;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0000D42C File Offset: 0x0000B62C
	public static void Init(Dictionary<string, object> data)
	{
		CommonUtils.QualityMemoryRanges = new int[]
		{
			0,
			512,
			1024,
			1024
		};
		CommonUtils.TextureOverrides = null;
		CommonUtils.CommonProperties = null;
		if (data == null)
		{
			return;
		}
		object obj = null;
		if (data.TryGetValue("version", out obj) && Convert.ToInt32(obj) != 0)
		{
			return;
		}
		try
		{
			if (data.TryGetValue("memory_range", out obj))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
				Array values = Enum.GetValues(typeof(CommonUtils.LevelOfDetail));
				foreach (object obj2 in values)
				{
					CommonUtils.LevelOfDetail levelOfDetail = (CommonUtils.LevelOfDetail)((int)obj2);
					if (dictionary.TryGetValue(levelOfDetail.ToString(), out obj))
					{
						CommonUtils.QualityMemoryRanges[(int)levelOfDetail] = Convert.ToInt32(obj);
					}
				}
			}
			obj = null;
			if (data.TryGetValue("properties", out obj))
			{
				CommonUtils.CommonProperties = (Dictionary<string, object>)obj;
			}
			obj = null;
			if (data.TryGetValue("devices", out obj))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
				string text = SystemInfo.deviceModel;
				if (!string.IsNullOrEmpty(text))
				{
					text = text.ToLower();
					if (dictionary.TryGetValue(text, out obj))
					{
						CommonUtils.TextureOverrides = (Dictionary<string, object>)obj;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
	public static string TextureForDeviceOverride(string textureName)
	{
		Debug.Log("TextureName: Loading: " + textureName);
		if (CommonUtils.TextureOverrides != null && !string.IsNullOrEmpty(textureName))
		{
			object obj = null;
			if (CommonUtils.TextureOverrides.TryGetValue(textureName, out obj))
			{
				textureName = (string)obj;
			}
		}
		return textureName;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0000D630 File Offset: 0x0000B830
	public static string PropertyForDeviceOverride(string propertyName)
	{
		if (CommonUtils.CommonProperties != null && !string.IsNullOrEmpty(propertyName))
		{
			object obj = null;
			if (CommonUtils.CommonProperties.TryGetValue(propertyName, out obj))
			{
				propertyName = (string)obj;
			}
		}
		return propertyName;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0000D670 File Offset: 0x0000B870
	public static CommonUtils.LevelOfDetail TextureLod()
	{
		if (CommonUtils.sTextureOfDetail != CommonUtils.LevelOfDetail.None)
		{
			return CommonUtils.sTextureOfDetail;
		}
		CommonUtils.sTextureOfDetail = CommonUtils.LevelOfDetail.Standard;
		int num = SystemInfo.systemMemorySize;
		if (CommonUtils.MemoryLevel > 0 && num < CommonUtils.MemoryLevel)
		{
			num = CommonUtils.MemoryLevel;
		}
		if (num <= CommonUtils.QualityMemoryRanges[1])
		{
			CommonUtils.sTextureOfDetail = CommonUtils.LevelOfDetail.Low;
		}
		else if (num > CommonUtils.QualityMemoryRanges[1] && num <= CommonUtils.QualityMemoryRanges[3])
		{
			CommonUtils.sTextureOfDetail = CommonUtils.LevelOfDetail.Standard;
		}
		else
		{
			CommonUtils.sTextureOfDetail = CommonUtils.LevelOfDetail.High;
		}
		return CommonUtils.sTextureOfDetail;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000D700 File Offset: 0x0000B900
	public static bool CheckReloadShader()
	{
		bool result = false;
		if (CommonUtils.TextureLod() != CommonUtils.LevelOfDetail.High)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x040001C2 RID: 450
	private static int MemoryLevel = -1;

	// Token: 0x040001C3 RID: 451
	private static Dictionary<string, object> TextureOverrides = null;

	// Token: 0x040001C4 RID: 452
	private static Dictionary<string, object> CommonProperties = null;

	// Token: 0x040001C5 RID: 453
	private static int[] QualityMemoryRanges = new int[]
	{
		0,
		512,
		1024,
		1024
	};

	// Token: 0x040001C6 RID: 454
	private static CommonUtils.LevelOfDetail sTextureOfDetail = CommonUtils.LevelOfDetail.None;

	// Token: 0x0200005D RID: 93
	public enum LevelOfDetail
	{
		// Token: 0x040001C8 RID: 456
		None,
		// Token: 0x040001C9 RID: 457
		Low,
		// Token: 0x040001CA RID: 458
		Standard,
		// Token: 0x040001CB RID: 459
		High,
		// Token: 0x040001CC RID: 460
		_Total
	}
}
