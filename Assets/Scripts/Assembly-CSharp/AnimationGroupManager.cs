using System;
using System.Collections.Generic;

// Token: 0x020003EF RID: 1007
public class AnimationGroupManager
{
	// Token: 0x06001EB4 RID: 7860 RVA: 0x000BD0FC File Offset: 0x000BB2FC
	public AnimationGroupManager()
	{
		this.animationGroups = new Dictionary<string, AnimationGroupManager.AnimGroup>();
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x000BD110 File Offset: 0x000BB310
	public AnimationGroupManager.AnimGroup FindAnimGroup(string state)
	{
		AnimationGroupManager.AnimGroup result = null;
		foreach (string key in this.animationGroups.Keys)
		{
			AnimationGroupManager.AnimGroup animGroup = this.animationGroups[key];
			if (animGroup.animModel.HasAnimation(state))
			{
				result = animGroup;
				break;
			}
		}
		return result;
	}

	// Token: 0x06001EB6 RID: 7862 RVA: 0x000BD19C File Offset: 0x000BB39C
	public void ApplyToGroups(AnimationGroupManager.ApplyDelegate apply)
	{
		foreach (string key in this.animationGroups.Keys)
		{
			apply(this.animationGroups[key]);
		}
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x000BD214 File Offset: 0x000BB414
	public void AddDisplayStateWithBlueprint(Dictionary<string, object> dict)
	{
		if (!dict.ContainsKey("animation_resource") || !dict.ContainsKey("name"))
		{
			TFUtils.Assert(false, "Paperdoll.AddDisplayState(): dictionary does not contain required fields 'animation_resource' or 'name'");
			return;
		}
		string skeletonName = (string)dict["skeleton"];
		string key = (string)dict["group"];
		SkeletonAnimationModel animModel;
		if (!this.animationGroups.ContainsKey(key))
		{
			AnimationGroupManager.AnimGroup animGroup = new AnimationGroupManager.AnimGroup();
			animGroup.skeletonName = skeletonName;
			animGroup.animModel = new SkeletonAnimationModel();
			animModel = animGroup.animModel;
			this.animationGroups.Add(key, animGroup);
		}
		else
		{
			AnimationGroupManager.AnimGroup animGroup = this.animationGroups[key];
			animModel = animGroup.animModel;
		}
		animModel.AddAnimationDataWithBlueprint(dict);
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x000BD2D0 File Offset: 0x000BB4D0
	public void CleanseAnimations(SkeletonCollection skeletons)
	{
		foreach (AnimationGroupManager.AnimGroup animGroup in this.animationGroups.Values)
		{
			skeletons.Cleanse(animGroup);
		}
	}

	// Token: 0x0400130E RID: 4878
	private Dictionary<string, AnimationGroupManager.AnimGroup> animationGroups;

	// Token: 0x020003F0 RID: 1008
	public class AnimGroup
	{
		// Token: 0x0400130F RID: 4879
		public string skeletonName;

		// Token: 0x04001310 RID: 4880
		public SkeletonAnimationModel animModel;
	}

	// Token: 0x020004BE RID: 1214
	// (Invoke) Token: 0x0600255B RID: 9563
	public delegate void ApplyDelegate(AnimationGroupManager.AnimGroup animGroup);
}
