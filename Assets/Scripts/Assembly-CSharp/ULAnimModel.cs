using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class ULAnimModel : ULAnimModelInterface
{
	// Token: 0x06002443 RID: 9283 RVA: 0x000DE45C File Offset: 0x000DC65C
	public ULAnimModel(Hashtable hashtable)
	{
		this.animationHashtable = hashtable;
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x000DE46C File Offset: 0x000DC66C
	public ULAnimModel()
	{
		this.animationHashtable = new Hashtable();
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x000DE480 File Offset: 0x000DC680
	public void AddAnimationSetting(string key, ULAnimationSetting setting)
	{
		this.animationHashtable.Add(key, setting);
	}

	// Token: 0x06002446 RID: 9286 RVA: 0x000DE490 File Offset: 0x000DC690
	public bool HasAnimation(string animName)
	{
		return this.animationHashtable.ContainsKey(animName);
	}

	// Token: 0x06002447 RID: 9287 RVA: 0x000DE4A0 File Offset: 0x000DC6A0
	public AnimationClip AnimClip(string animName)
	{
		TFUtils.Assert(false, "We should not be loading anim clips from ULAnimModels");
		return null;
	}

	// Token: 0x06002448 RID: 9288 RVA: 0x000DE4B0 File Offset: 0x000DC6B0
	public AnimationBlendMode AnimBlendMode(string animName)
	{
		return ((ULAnimationSetting)this.animationHashtable[animName]).blendMode;
	}

	// Token: 0x06002449 RID: 9289 RVA: 0x000DE4C8 File Offset: 0x000DC6C8
	public WrapMode AnimWrapMode(string animName)
	{
		return ((ULAnimationSetting)this.animationHashtable[animName]).wrapMode;
	}

	// Token: 0x0600244A RID: 9290 RVA: 0x000DE4E0 File Offset: 0x000DC6E0
	public PlayMode AnimPlayMode(string animName)
	{
		return ((ULAnimationSetting)this.animationHashtable[animName]).playMode;
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x000DE4F8 File Offset: 0x000DC6F8
	public int AnimLayer(string animName)
	{
		return ((ULAnimationSetting)this.animationHashtable[animName]).layer;
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x000DE510 File Offset: 0x000DC710
	public void ApplyAnimationSettings(Animation targetAnimation)
	{
		foreach (object obj in this.animationHashtable.Keys)
		{
			string text = (string)obj;
			ULAnimationSetting ulanimationSetting = (ULAnimationSetting)this.animationHashtable[text];
			UnityEngine.Object @object = FileSystemCoordinator.LoadAsset(Path.GetFileName(ulanimationSetting.resource));
			if (@object == null)
			{
				@object = Resources.Load(ulanimationSetting.resource);
			}
			if (@object == null)
			{
				TFUtils.ErrorLog("Something went wrong trying to load " + ulanimationSetting.resource);
			}
			else
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(@object) as GameObject;
				targetAnimation.AddClip(gameObject.animation.clip, text);
				UnityEngine.Object.DestroyImmediate(gameObject);
				targetAnimation[text].blendMode = ulanimationSetting.blendMode;
				targetAnimation[text].wrapMode = ulanimationSetting.wrapMode;
				targetAnimation[text].layer = ulanimationSetting.layer;
			}
		}
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x000DE640 File Offset: 0x000DC840
	public void UnapplyAnimationSettings(Animation targetAnimation)
	{
		foreach (object obj in this.animationHashtable.Keys)
		{
			string text = (string)obj;
			AnimationClip clip = targetAnimation.GetClip(text);
			targetAnimation.RemoveClip(text);
			UnityEngine.Object.DestroyImmediate(clip);
		}
	}

	// Token: 0x0400164C RID: 5708
	protected Hashtable animationHashtable;
}
