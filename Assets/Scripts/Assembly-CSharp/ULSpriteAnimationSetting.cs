using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200048C RID: 1164
[Serializable]
public class ULSpriteAnimationSetting
{
	// Token: 0x04001663 RID: 5731
	public string animName;

	// Token: 0x04001664 RID: 5732
	public string resourceName;

	// Token: 0x04001665 RID: 5733
	public string texture;

	// Token: 0x04001666 RID: 5734
	public float cellTop;

	// Token: 0x04001667 RID: 5735
	public float cellLeft;

	// Token: 0x04001668 RID: 5736
	public float cellWidth;

	// Token: 0x04001669 RID: 5737
	public float cellHeight;

	// Token: 0x0400166A RID: 5738
	public int cellStartColumn;

	// Token: 0x0400166B RID: 5739
	public int cellColumns;

	// Token: 0x0400166C RID: 5740
	public int cellCount;

	// Token: 0x0400166D RID: 5741
	public int framesPerSecond;

	// Token: 0x0400166E RID: 5742
	public float timingTotal;

	// Token: 0x0400166F RID: 5743
	public List<float> timingList;

	// Token: 0x04001670 RID: 5744
	public ULSpriteAnimationSetting.LoopMode loopMode;

	// Token: 0x04001671 RID: 5745
	public bool flipH;

	// Token: 0x04001672 RID: 5746
	public bool flipV;

	// Token: 0x04001673 RID: 5747
	public Color32 mainColor;

	// Token: 0x04001674 RID: 5748
	public string maskName;

	// Token: 0x0200048D RID: 1165
	public enum LoopMode
	{
		// Token: 0x04001676 RID: 5750
		None,
		// Token: 0x04001677 RID: 5751
		Loop
	}
}
