using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class ItemDropDefinition
{
	// Token: 0x06000D05 RID: 3333 RVA: 0x00050718 File Offset: 0x0004E918
	public ItemDropDefinition(int did, IDisplayController displayController, Vector2 cleanupScreenDestination, bool forceTapToCollect)
	{
		this.did = did;
		this.displayController = displayController;
		this.cleanupScreenDestination = cleanupScreenDestination;
		this.forceTapToCollect = forceTapToCollect;
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00050740 File Offset: 0x0004E940
	public IDisplayController DisplayController
	{
		get
		{
			return this.displayController;
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00050754 File Offset: 0x0004E954
	// (set) Token: 0x06000D07 RID: 3335 RVA: 0x00050748 File Offset: 0x0004E948
	public Vector2 CleanupScreenDestination
	{
		get
		{
			return this.cleanupScreenDestination;
		}
		set
		{
			this.cleanupScreenDestination = value;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0005075C File Offset: 0x0004E95C
	public int Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00050764 File Offset: 0x0004E964
	public bool ForceTapToCollect
	{
		get
		{
			return this.forceTapToCollect;
		}
	}

	// Token: 0x040008CB RID: 2251
	private int did;

	// Token: 0x040008CC RID: 2252
	private IDisplayController displayController;

	// Token: 0x040008CD RID: 2253
	private Vector2 cleanupScreenDestination;

	// Token: 0x040008CE RID: 2254
	private bool forceTapToCollect;
}
