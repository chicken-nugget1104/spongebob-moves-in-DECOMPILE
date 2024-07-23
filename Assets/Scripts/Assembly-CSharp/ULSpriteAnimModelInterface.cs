using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public interface ULSpriteAnimModelInterface
{
	// Token: 0x0600248F RID: 9359
	string GetMaterialName(string animName);

	// Token: 0x06002490 RID: 9360
	string GetResourceName(string animName);

	// Token: 0x06002491 RID: 9361
	string GetTextureName(string animName);

	// Token: 0x06002492 RID: 9362
	bool HasAnimation(string animName);

	// Token: 0x06002493 RID: 9363
	float CellTop(string animName);

	// Token: 0x06002494 RID: 9364
	float CellLeft(string animName);

	// Token: 0x06002495 RID: 9365
	float CellWidth(string animName);

	// Token: 0x06002496 RID: 9366
	float CellHeight(string animName);

	// Token: 0x06002497 RID: 9367
	int CellStartColumn(string animName);

	// Token: 0x06002498 RID: 9368
	int CellColumns(string animName);

	// Token: 0x06002499 RID: 9369
	int CellCount(string animName);

	// Token: 0x0600249A RID: 9370
	int FramesPerSecond(string animName);

	// Token: 0x0600249B RID: 9371
	float TimingTotal(string animName);

	// Token: 0x0600249C RID: 9372
	List<float> TimingList(string animName);

	// Token: 0x0600249D RID: 9373
	bool Loop(string animName);

	// Token: 0x0600249E RID: 9374
	bool FlipH(string animName);

	// Token: 0x0600249F RID: 9375
	bool FlipV(string animName);

	// Token: 0x060024A0 RID: 9376
	Color32 MainColor(string animName);

	// Token: 0x060024A1 RID: 9377
	string MaskName(string animName);
}
