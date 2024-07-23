using System;
using UnityEngine;

// Token: 0x020002A6 RID: 678
public class GoodReturnRequest : GoodWidgetTransfer
{
	// Token: 0x060014C4 RID: 5316 RVA: 0x0008C20C File Offset: 0x0008A40C
	public GoodReturnRequest(Vector2 initialScreenPosition, int goodId, string materialName) : base(goodId, materialName, 40f, 20f)
	{
		this.initialScreenPosition = initialScreenPosition;
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x0008C228 File Offset: 0x0008A428
	public override Vector2 GetOriginalScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return this.initialScreenPosition;
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x0008C230 File Offset: 0x0008A430
	public override Vector2 GetTargetScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return hudWidgetPosition;
	}

	// Token: 0x04000E80 RID: 3712
	private Vector2 initialScreenPosition;
}
