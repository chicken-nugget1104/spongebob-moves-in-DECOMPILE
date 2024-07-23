using System;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class GoodToSimulatedDeliveryRequest : GoodWidgetTransfer
{
	// Token: 0x060014C7 RID: 5319 RVA: 0x0008C234 File Offset: 0x0008A434
	public GoodToSimulatedDeliveryRequest(Simulated targetSimulated, int goodId, string materialName) : base(goodId, materialName, 30f, 20f)
	{
		this.targetSimulated = targetSimulated;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0008C250 File Offset: 0x0008A450
	public override Vector2 GetOriginalScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		return hudWidgetPosition;
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x0008C254 File Offset: 0x0008A454
	public override Vector2 GetTargetScreenPosition(Session session, Vector2 hudWidgetPosition)
	{
		Vector3 position = this.targetSimulated.ThoughtDisplayController.Position;
		return session.TheCamera.WorldPointToScreenPoint(position);
	}

	// Token: 0x04000E81 RID: 3713
	private Simulated targetSimulated;
}
