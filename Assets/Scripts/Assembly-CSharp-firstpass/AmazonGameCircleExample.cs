using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class AmazonGameCircleExample : MonoBehaviour
{
	// Token: 0x06000186 RID: 390 RVA: 0x000078A8 File Offset: 0x00005AA8
	private void Start()
	{
		this.Initialize();
	}

	// Token: 0x06000187 RID: 391 RVA: 0x000078B0 File Offset: 0x00005AB0
	private void OnGUI()
	{
		this.InitializeUI();
		this.ApplyLocalUISkin();
		AmazonGameCircleExampleGUIHelpers.BeginMenuLayout();
		this.scroll = GUILayout.BeginScrollView(this.scroll, new GUILayoutOption[0]);
		if (this.initializationMenu.InitializationStatus != AmazonGameCircleExampleInitialization.EInitializationStatus.Ready)
		{
			this.initializationMenu.DrawMenu();
		}
		else
		{
			foreach (AmazonGameCircleExampleBase amazonGameCircleExampleBase in this.gameCircleExampleMenus)
			{
				GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
				amazonGameCircleExampleBase.foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(amazonGameCircleExampleBase.foldoutOpen, amazonGameCircleExampleBase.MenuTitle());
				if (amazonGameCircleExampleBase.foldoutOpen)
				{
					amazonGameCircleExampleBase.DrawMenu();
				}
				GUILayout.EndVertical();
			}
		}
		GUILayout.EndScrollView();
		AmazonGameCircleExampleGUIHelpers.EndMenuLayout();
		this.RevertLocalUISkin();
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000079AC File Offset: 0x00005BAC
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.gameCircleExampleMenus.Add(new AmazonGameCircleExampleProfiles());
		this.gameCircleExampleMenus.Add(new AmazonGameCircleExampleAchievements());
		this.gameCircleExampleMenus.Add(new AmazonGameCircleExampleLeaderboards());
		this.gameCircleExampleMenus.Add(new AmazonGameCircleExampleWhispersync());
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00007A0C File Offset: 0x00005C0C
	private void InitializeUI()
	{
		if (this.uiInitialized)
		{
			return;
		}
		this.uiInitialized = true;
		this.localGuiSkin = GUI.skin;
		this.originalGuiSkin = GUI.skin;
		AmazonGameCircleExampleGUIHelpers.SetGUISkinTouchFriendly(this.localGuiSkin);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00007A50 File Offset: 0x00005C50
	private void ApplyLocalUISkin()
	{
		GUI.skin = this.localGuiSkin;
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00007A60 File Offset: 0x00005C60
	private void RevertLocalUISkin()
	{
		GUI.skin = this.originalGuiSkin;
	}

	// Token: 0x0400007B RID: 123
	private AmazonGameCircleExampleInitialization initializationMenu = new AmazonGameCircleExampleInitialization();

	// Token: 0x0400007C RID: 124
	private List<AmazonGameCircleExampleBase> gameCircleExampleMenus = new List<AmazonGameCircleExampleBase>();

	// Token: 0x0400007D RID: 125
	private bool initialized;

	// Token: 0x0400007E RID: 126
	private Vector2 scroll = Vector2.zero;

	// Token: 0x0400007F RID: 127
	private bool uiInitialized;

	// Token: 0x04000080 RID: 128
	private GUISkin localGuiSkin;

	// Token: 0x04000081 RID: 129
	private GUISkin originalGuiSkin;
}
