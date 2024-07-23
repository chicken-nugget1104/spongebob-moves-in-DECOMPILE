using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class ULSpriteAnimModel : ULSpriteAnimModelInterface
{
	// Token: 0x06002478 RID: 9336 RVA: 0x000DF4C4 File Offset: 0x000DD6C4
	public ULSpriteAnimModel(ULSpriteAnimationSetting[] animationSettings)
	{
		this.animationHashtable = new Hashtable();
		foreach (ULSpriteAnimationSetting ulspriteAnimationSetting in animationSettings)
		{
			this.animationHashtable.Add(ulspriteAnimationSetting.animName, ulspriteAnimationSetting);
		}
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x000DF510 File Offset: 0x000DD710
	public ULSpriteAnimModel(Hashtable hashtable)
	{
		this.animationHashtable = hashtable;
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000DF520 File Offset: 0x000DD720
	public ULSpriteAnimModel()
	{
		this.animationHashtable = new Hashtable();
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000DF534 File Offset: 0x000DD734
	public void AddAnimationSetting(string key, ULSpriteAnimationSetting setting)
	{
		this.animationHashtable.Add(key, setting);
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x000DF544 File Offset: 0x000DD744
	public string GetMaterialName(string animName)
	{
		ULSpriteAnimationSetting ulspriteAnimationSetting = (ULSpriteAnimationSetting)this.animationHashtable[animName];
		if (ulspriteAnimationSetting.texture != null)
		{
			return "Materials/lod/" + ulspriteAnimationSetting.resourceName;
		}
		return ulspriteAnimationSetting.resourceName;
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x000DF588 File Offset: 0x000DD788
	public string GetResourceName(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).resourceName;
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x000DF5A0 File Offset: 0x000DD7A0
	public string GetTextureName(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).texture;
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x000DF5B8 File Offset: 0x000DD7B8
	public bool HasAnimation(string animName)
	{
		return this.animationHashtable.ContainsKey(animName);
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x000DF5D0 File Offset: 0x000DD7D0
	public float CellTop(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).cellTop;
	}

	// Token: 0x06002481 RID: 9345 RVA: 0x000DF5E8 File Offset: 0x000DD7E8
	public float CellLeft(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).cellLeft;
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000DF600 File Offset: 0x000DD800
	public float CellWidth(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).cellWidth;
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x000DF618 File Offset: 0x000DD818
	public float CellHeight(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).cellHeight;
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000DF630 File Offset: 0x000DD830
	public int CellStartColumn(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).cellStartColumn;
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000DF648 File Offset: 0x000DD848
	public int CellColumns(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).cellColumns;
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x000DF660 File Offset: 0x000DD860
	public int CellCount(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).cellCount;
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x000DF678 File Offset: 0x000DD878
	public int FramesPerSecond(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).framesPerSecond;
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x000DF690 File Offset: 0x000DD890
	public float TimingTotal(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).timingTotal;
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x000DF6A8 File Offset: 0x000DD8A8
	public List<float> TimingList(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).timingList;
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x000DF6C0 File Offset: 0x000DD8C0
	public bool Loop(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).loopMode == ULSpriteAnimationSetting.LoopMode.Loop;
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x000DF6DC File Offset: 0x000DD8DC
	public bool FlipH(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).flipH;
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000DF6F4 File Offset: 0x000DD8F4
	public bool FlipV(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).flipV;
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000DF70C File Offset: 0x000DD90C
	public Color32 MainColor(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).mainColor;
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x000DF724 File Offset: 0x000DD924
	public string MaskName(string animName)
	{
		return ((ULSpriteAnimationSetting)this.animationHashtable[animName]).maskName;
	}

	// Token: 0x04001678 RID: 5752
	protected Hashtable animationHashtable;
}
