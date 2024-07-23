using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class SkeletonAnimationModel : ULAnimModel
{
	// Token: 0x060021ED RID: 8685 RVA: 0x000D11DC File Offset: 0x000CF3DC
	public SkeletonAnimationSetting SkeletonSettings(string animName)
	{
		return (SkeletonAnimationSetting)this.animationHashtable[animName];
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000D11F0 File Offset: 0x000CF3F0
	public string AnimationEventsKey(string animName)
	{
		return ((SkeletonAnimationSetting)this.animationHashtable[animName]).animationEventsKey;
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000D1208 File Offset: 0x000CF408
	public string ItemResource(string animName)
	{
		return ((SkeletonAnimationSetting)this.animationHashtable[animName]).itemResource;
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000D1220 File Offset: 0x000CF420
	public string ObjectResource(string animName)
	{
		return ((SkeletonAnimationSetting)this.animationHashtable[animName]).objectResource;
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000D1238 File Offset: 0x000CF438
	public Vector3 ItemScale(string animName)
	{
		return ((SkeletonAnimationSetting)this.animationHashtable[animName]).itemScale;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000D1250 File Offset: 0x000CF450
	public Vector3 ObjectScale(string animName)
	{
		return ((SkeletonAnimationSetting)this.animationHashtable[animName]).objectScale;
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000D1268 File Offset: 0x000CF468
	public void AddAnimationDataWithBlueprint(Dictionary<string, object> dict)
	{
		string key = TFUtils.LoadString(dict, "name");
		string text = TFUtils.LoadString(dict, "animation_resource");
		Vector3 one = Vector3.one;
		Vector3 one2 = Vector3.one;
		SkeletonAnimationSetting skeletonAnimationSetting = new SkeletonAnimationSetting();
		object obj = null;
		if (dict.TryGetValue("item_prop", out obj))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			skeletonAnimationSetting.itemResource = TFUtils.LoadString(dictionary, "item_resource");
			skeletonAnimationSetting.itemBone = TFUtils.LoadString(dictionary, "item_bone", "BN_ITEM");
			if (skeletonAnimationSetting.itemBone == "base")
			{
				skeletonAnimationSetting.itemBone = null;
			}
			TFUtils.LoadVector3(out one, (Dictionary<string, object>)dictionary["item_scale"]);
		}
		if (dict.TryGetValue("object_prop", out obj))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			skeletonAnimationSetting.objectResource = TFUtils.LoadString(dictionary2, "object_resource");
			skeletonAnimationSetting.objectBone = TFUtils.LoadString(dictionary2, "object_bone", "BN_OBJECT");
			if (skeletonAnimationSetting.objectBone == "base")
			{
				skeletonAnimationSetting.objectBone = null;
			}
			TFUtils.LoadVector3(out one2, (Dictionary<string, object>)dictionary2["object_scale"]);
		}
		TFUtils.DebugLog("Loading resource " + text, TFUtils.LogFilter.Assets);
		skeletonAnimationSetting.resource = text;
		skeletonAnimationSetting.blendMode = AnimationBlendMode.Blend;
		skeletonAnimationSetting.wrapMode = WrapMode.Default;
		skeletonAnimationSetting.playMode = PlayMode.StopSameLayer;
		skeletonAnimationSetting.layer = 0;
		skeletonAnimationSetting.animationEventsKey = null;
		skeletonAnimationSetting.itemScale = one;
		skeletonAnimationSetting.objectScale = one2;
		if (dict.TryGetValue("blend_mode", out obj))
		{
			skeletonAnimationSetting.blendMode = SkeletonAnimationModel.blendModeDictionary[(string)obj];
		}
		if (dict.TryGetValue("wrap_mode", out obj))
		{
			skeletonAnimationSetting.wrapMode = SkeletonAnimationModel.wrapModeDictionary[(string)obj];
		}
		if (dict.TryGetValue("play_mode", out obj))
		{
			skeletonAnimationSetting.playMode = SkeletonAnimationModel.playModeDictionary[(string)obj];
		}
		if (dict.ContainsKey("layer"))
		{
			skeletonAnimationSetting.layer = TFUtils.LoadInt(dict, "layer");
		}
		if (dict.TryGetValue("animation_events", out obj))
		{
			skeletonAnimationSetting.animationEventsKey = (string)obj;
		}
		skeletonAnimationSetting.unloadable = TFUtils.LoadBool(dict, "unloadable", false);
		base.AddAnimationSetting(key, skeletonAnimationSetting);
	}

	// Token: 0x04001503 RID: 5379
	public static Dictionary<string, WrapMode> wrapModeDictionary = new Dictionary<string, WrapMode>
	{
		{
			"clamp",
			WrapMode.Once
		},
		{
			"clamp_forever",
			WrapMode.ClampForever
		},
		{
			"default",
			WrapMode.Default
		},
		{
			"loop",
			WrapMode.Loop
		},
		{
			"once",
			WrapMode.Once
		},
		{
			"pingpong",
			WrapMode.PingPong
		}
	};

	// Token: 0x04001504 RID: 5380
	public static Dictionary<string, PlayMode> playModeDictionary = new Dictionary<string, PlayMode>
	{
		{
			"stop_all",
			PlayMode.StopAll
		},
		{
			"stop_same_layer",
			PlayMode.StopSameLayer
		}
	};

	// Token: 0x04001505 RID: 5381
	public static Dictionary<string, AnimationBlendMode> blendModeDictionary = new Dictionary<string, AnimationBlendMode>
	{
		{
			"additive",
			AnimationBlendMode.Additive
		},
		{
			"blend",
			AnimationBlendMode.Blend
		}
	};
}
