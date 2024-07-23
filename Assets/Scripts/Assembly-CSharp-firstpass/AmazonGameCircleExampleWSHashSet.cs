using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class AmazonGameCircleExampleWSHashSet
{
	// Token: 0x060001DA RID: 474 RVA: 0x00009538 File Offset: 0x00007738
	public AmazonGameCircleExampleWSHashSet(string title, Func<HashSet<string>> refreshFunction)
	{
		this.hashSetTitle = title;
		if (refreshFunction == null)
		{
			return;
		}
		this.refreshHashSetFunction = (Func<HashSet<string>>)Delegate.Combine(this.refreshHashSetFunction, refreshFunction);
		this.Refresh();
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060001DB RID: 475 RVA: 0x0000956C File Offset: 0x0000776C
	// (remove) Token: 0x060001DC RID: 476 RVA: 0x00009588 File Offset: 0x00007788
	private event Func<HashSet<string>> refreshHashSetFunction;

	// Token: 0x060001DD RID: 477 RVA: 0x000095A4 File Offset: 0x000077A4
	public void DrawGUI()
	{
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.foldoutOpen, this.hashSetTitle);
		if (this.foldoutOpen)
		{
			if (GUILayout.Button("Refresh", new GUILayoutOption[0]))
			{
				this.Refresh();
			}
			if (this.hashSet.Count == 0)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel("Key list is empty", new GUILayoutOption[0]);
			}
			else
			{
				foreach (string text in this.hashSet)
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel(text, new GUILayoutOption[0]);
				}
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000968C File Offset: 0x0000788C
	public void Refresh()
	{
		if (this.refreshHashSetFunction == null)
		{
			return;
		}
		this.hashSet = this.refreshHashSetFunction();
	}

	// Token: 0x04000105 RID: 261
	private const string emptyHashSetLabel = "Key list is empty";

	// Token: 0x04000106 RID: 262
	private const string refreshHashSetButtonLabel = "Refresh";

	// Token: 0x04000107 RID: 263
	private string hashSetTitle;

	// Token: 0x04000108 RID: 264
	private HashSet<string> hashSet;

	// Token: 0x04000109 RID: 265
	private bool foldoutOpen;
}
