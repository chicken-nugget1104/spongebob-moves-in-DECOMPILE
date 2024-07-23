using System;
using UnityEngine;

// Token: 0x020002A8 RID: 680
public abstract class GoodWidgetTransfer
{
	// Token: 0x060014CA RID: 5322 RVA: 0x0008C280 File Offset: 0x0008A480
	public GoodWidgetTransfer(int goodId, string materialName, float speed, float dRad)
	{
		this.speed = speed;
		this.dRad = dRad;
		this.goodId = goodId;
		this.materialName = materialName;
	}

	// Token: 0x060014CB RID: 5323
	public abstract Vector2 GetOriginalScreenPosition(Session session, Vector2 hudWidgetPosition);

	// Token: 0x060014CC RID: 5324
	public abstract Vector2 GetTargetScreenPosition(Session session, Vector2 hudWidgetPosition);

	// Token: 0x04000E82 RID: 3714
	public SBGUIPulseImage icon;

	// Token: 0x04000E83 RID: 3715
	public float speed;

	// Token: 0x04000E84 RID: 3716
	public float dRad;

	// Token: 0x04000E85 RID: 3717
	public int goodId;

	// Token: 0x04000E86 RID: 3718
	public string materialName;
}
