using System;
using UnityEngine;

// Token: 0x0200023F RID: 575
public class RestrictInteraction
{
	// Token: 0x06001289 RID: 4745 RVA: 0x000801D4 File Offset: 0x0007E3D4
	public static void AddWhitelistElement(SBGUIElement element)
	{
		SBGUI.GetInstance().WhitelistElement(element);
		SBGUI.GetInstance().RestoreWhiteList();
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x000801EC File Offset: 0x0007E3EC
	public static void RemoveWhitelistElement(SBGUIElement element)
	{
		SBGUI.GetInstance().UnWhitelistElement(element);
		SBGUI.GetInstance().ResetWhiteList();
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x00080204 File Offset: 0x0007E404
	public static bool ContainsWhitelistElement(SBGUIElement element)
	{
		return SBGUI.GetInstance().CheckWhitelisted(element);
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x00080214 File Offset: 0x0007E414
	public static void AddWhitelistSimulated(Simulation simulation, Identity id)
	{
		simulation.WhitelistSimulated(id);
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x00080220 File Offset: 0x0007E420
	public static void AddWhitelistSimulated(Simulation simulation, int did)
	{
		simulation.WhitelistSimulated(did);
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x0008022C File Offset: 0x0007E42C
	public static void RemoveWhitelistSimulated(Simulation simulation, Identity id)
	{
		simulation.UnWhitelistSimulated(id);
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x00080238 File Offset: 0x0007E438
	public static void RemoveWhitelistSimulated(Simulation simulation, int did)
	{
		simulation.UnWhitelistSimulated(did);
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00080244 File Offset: 0x0007E444
	public static void AddWhitelistExpansion(Simulation simulation, int did)
	{
		simulation.WhitelistExpansion(did);
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x00080250 File Offset: 0x0007E450
	public static void RemoveWhitelistExpansion(Simulation simulation, int did)
	{
		simulation.UnWhitelistExpansion(did);
	}

	// Token: 0x04000CBB RID: 3259
	public const string RESTRICT_INTERACTION = "restrict_clicks";

	// Token: 0x04000CBC RID: 3260
	public const int RESTRICT_SIM_ID = -2147483648;

	// Token: 0x04000CBD RID: 3261
	public const int RESTRICT_EXPANSION_ID = -2147483648;

	// Token: 0x04000CBE RID: 3262
	public static readonly SBGUIElement RESTRICT_ALL_UI_ELEMENT = new GameObject("dummy").AddComponent<SBGUIDummyElement>();
}
