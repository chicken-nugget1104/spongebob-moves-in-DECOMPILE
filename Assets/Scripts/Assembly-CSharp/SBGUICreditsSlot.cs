using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class SBGUICreditsSlot : SBGUIScrollListElement
{
	// Token: 0x06000473 RID: 1139 RVA: 0x0001C03C File Offset: 0x0001A23C
	public static SBGUICreditsSlot MakeCreditsSlot()
	{
		SBGUICreditsSlot sbguicreditsSlot = (SBGUICreditsSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CreditsSlot");
		sbguicreditsSlot.name = "CreditsSlot";
		sbguicreditsSlot.gameObject.SetActiveRecursively(false);
		sbguicreditsSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sbguicreditsSlot;
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001C090 File Offset: 0x0001A290
	public static SBGUICreditsSlot MakeCreditsSlot1()
	{
		SBGUICreditsSlot sbguicreditsSlot = (SBGUICreditsSlot)SBGUI.InstantiatePrefab("Prefabs/CreditsSlot1");
		sbguicreditsSlot.name = "CreditsSlot1";
		sbguicreditsSlot.gameObject.SetActiveRecursively(false);
		sbguicreditsSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sbguicreditsSlot;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0001C0E4 File Offset: 0x0001A2E4
	public static SBGUICreditsSlot MakeCreditsSlot2()
	{
		SBGUICreditsSlot sbguicreditsSlot = (SBGUICreditsSlot)SBGUI.InstantiatePrefab("Prefabs/CreditsSlot2");
		sbguicreditsSlot.name = "CreditsSlot2";
		sbguicreditsSlot.gameObject.SetActiveRecursively(false);
		sbguicreditsSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sbguicreditsSlot;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001C138 File Offset: 0x0001A338
	public void Setup(Session session, SBGUIScreen screen, SBGUIElement parent, Vector3 offset)
	{
		this.SetParent(parent);
		base.transform.localPosition = offset;
	}
}
