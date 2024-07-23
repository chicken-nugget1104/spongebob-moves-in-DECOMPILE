using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class SBGUIQuestTrackerSlot : SBGUIAtlasButton
{
	// Token: 0x060005B8 RID: 1464 RVA: 0x00024A3C File Offset: 0x00022C3C
	public SBGUIQuestTrackerSlot.QuestTrackerState OnUpdate(float upperBound, float lowerBound)
	{
		Vector2 screenPosition = base.GetScreenPosition();
		if (screenPosition.y < upperBound)
		{
			base.transform.FindChild("questbackground").renderer.enabled = false;
			base.renderer.enabled = false;
			return SBGUIQuestTrackerSlot.QuestTrackerState.AboveBounds;
		}
		if (screenPosition.y > lowerBound)
		{
			base.renderer.enabled = false;
			base.transform.FindChild("questbackground").renderer.enabled = false;
			return SBGUIQuestTrackerSlot.QuestTrackerState.BelowBounds;
		}
		if (base.transform.parent.gameObject.active)
		{
			base.renderer.enabled = true;
			base.transform.FindChild("questbackground").renderer.enabled = true;
		}
		return SBGUIQuestTrackerSlot.QuestTrackerState.InBounds;
	}

	// Token: 0x0200009C RID: 156
	public enum QuestTrackerState
	{
		// Token: 0x04000462 RID: 1122
		InBounds,
		// Token: 0x04000463 RID: 1123
		AboveBounds,
		// Token: 0x04000464 RID: 1124
		BelowBounds
	}
}
