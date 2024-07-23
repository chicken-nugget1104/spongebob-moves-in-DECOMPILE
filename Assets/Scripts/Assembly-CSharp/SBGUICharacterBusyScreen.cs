using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class SBGUICharacterBusyScreen : SBGUIScreen
{
	// Token: 0x060003E4 RID: 996 RVA: 0x0001582C File Offset: 0x00013A2C
	public void SetupDialogInfo(Simulated pSimulated, Task pTask, Action pFeedWishAction, Action pRushWishAction, Action pRushTaskAction)
	{
		this.m_pSimulated = pSimulated;
		this.m_pTask = pTask;
		this.m_pRushTaskAction = pRushTaskAction;
		this.m_pEntity = this.m_pSimulated.GetEntity<ResidentEntity>();
		this.m_pWishWidget.SetData(this.session, pSimulated, pFeedWishAction, pRushWishAction);
		TaskData pTaskData = pTask.m_pTaskData;
		this.m_pTaskNameLabel.SetText(Language.Get(pTaskData.m_sName));
		foreach (KeyValuePair<int, int> keyValuePair in pTaskData.m_pReward.ResourceAmounts)
		{
			if (keyValuePair.Key != ResourceManager.XP)
			{
				this.m_pTaskSoftRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[keyValuePair.Key].ToString());
				this.m_pCurrencyImage.SetTextureFromAtlas(this.session.TheGame.resourceManager.Resources[keyValuePair.Key].GetResourceTexture());
				break;
			}
		}
		this.m_pTaskXPRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[ResourceManager.XP].ToString());
		this.m_ulTaskTimeLeft = this.m_pTask.GetTimeLeft();
		this.m_pTaskInProgressParent.SetActive(true);
		if (this.m_pTask.m_bMovingToTarget)
		{
			this.m_pTaskProgressLabel.SetText("Waiting...");
			this.m_pTaskProgressMeter.Progress = 0f;
		}
		else
		{
			this.m_pTaskProgressLabel.SetText(TFUtils.DurationToString(this.m_ulTaskTimeLeft));
			this.m_pTaskProgressMeter.Progress = this.m_pTask.GetTimeLeftPercentage();
		}
		Cost cost = this.m_pTask.RushCostNow();
		this.m_pTaskRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
		this.m_pRushTaskButton.ClearClickEvents();
		base.AttachActionToButton(this.m_pRushTaskButton, pRushTaskAction);
		ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
		int num = (entity.CostumeDID == null) ? -1 : entity.CostumeDID.Value;
		if (num < 0)
		{
			num = ((entity.DefaultCostumeDID == null) ? -1 : entity.DefaultCostumeDID.Value);
		}
		if (num >= 0)
		{
			CostumeManager.Costume costume = this.session.TheGame.costumeManager.GetCostume(num);
			this.m_pCharacterPortrait.SetSizeNoRebuild(this.m_pCharacterPortraitSize);
			this.m_pCharacterPortrait.SetTextureFromAtlas(costume.m_sPortrait, true, false, true, false, false, 0);
		}
		else if (!string.IsNullOrEmpty(entity.DialogPortrait))
		{
			this.m_pCharacterPortrait.SetSizeNoRebuild(this.m_pCharacterPortraitSize);
			this.m_pCharacterPortrait.SetTextureFromAtlas(entity.DialogPortrait, true, false, true, false, false, 0);
		}
		else
		{
			this.m_pCharacterPortrait.SetTextureFromAtlas("_blank_.png");
		}
		this.m_pCharacterNameLabel.SetText(Language.Get(entity.Name));
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x00015B80 File Offset: 0x00013D80
	public Vector2 GetWishWidgetRushButtonPosition()
	{
		return this.m_pWishWidget.GetRushWishButtonPosition();
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00015B90 File Offset: 0x00013D90
	public Vector2 GetTaskRushButtonPosition()
	{
		return this.m_pRushTaskButton.GetScreenPosition();
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x00015BA0 File Offset: 0x00013DA0
	protected override void Awake()
	{
		this.m_pWishWidget = (SBGUICharacterWishWidget)base.FindChild("wish_widget");
		this.m_pCharacterPortrait = (SBGUIAtlasImage)base.FindChild("character_portrait");
		this.m_pCurrencyImage = (SBGUIAtlasImage)base.FindChild("soft_currency_icon");
		this.m_pTaskNameLabel = (SBGUILabel)base.FindChild("task_name_label");
		this.m_pTaskSoftRewardLabel = (SBGUILabel)base.FindChild("task_soft_reward_label");
		this.m_pTaskXPRewardLabel = (SBGUILabel)base.FindChild("task_xp_reward_label");
		this.m_pTaskProgressLabel = (SBGUILabel)base.FindChild("task_progress_label");
		this.m_pTaskRushCostLabel = (SBGUILabel)base.FindChild("task_rush_cost_label");
		this.m_pCharacterNameLabel = (SBGUILabel)base.FindChild("character_name_label");
		this.m_pTaskProgressMeter = (SBGUIProgressMeter)base.FindChild("task_progress_meter");
		this.m_pRushTaskButton = (SBGUIButton)base.FindChild("rush_task_button");
		this.m_pTaskInProgressParent = this.m_pTaskProgressMeter.transform.parent.parent.gameObject;
		this.m_pCharacterPortraitSize = this.m_pCharacterPortrait.Size;
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("reward_title_label");
		sbguilabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		base.Awake();
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00015D08 File Offset: 0x00013F08
	private new void Update()
	{
		if (this.m_ulTaskTimeLeft > 0UL)
		{
			this.m_ulTaskTimeLeft = this.m_pTask.GetTimeLeft();
			if (this.m_ulTaskTimeLeft == 0UL)
			{
				this.m_pTaskInProgressParent.SetActive(false);
			}
			else
			{
				if (this.m_pTask.m_bMovingToTarget)
				{
					this.m_pTaskProgressLabel.SetText("Waiting...");
					this.m_pTaskProgressMeter.Progress = 0f;
				}
				else
				{
					this.m_pTaskProgressLabel.SetText(TFUtils.DurationToString(this.m_ulTaskTimeLeft));
					this.m_pTaskProgressMeter.Progress = this.m_pTask.GetTimeLeftPercentage();
				}
				Cost cost = this.m_pTask.RushCostNow();
				this.m_pTaskRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
				int.TryParse(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString(), out this.taskRushCost);
			}
		}
	}

	// Token: 0x04000291 RID: 657
	private SBGUICharacterWishWidget m_pWishWidget;

	// Token: 0x04000292 RID: 658
	private SBGUIAtlasImage m_pCharacterPortrait;

	// Token: 0x04000293 RID: 659
	private SBGUIAtlasImage m_pCurrencyImage;

	// Token: 0x04000294 RID: 660
	private SBGUILabel m_pTaskNameLabel;

	// Token: 0x04000295 RID: 661
	private SBGUILabel m_pTaskSoftRewardLabel;

	// Token: 0x04000296 RID: 662
	private SBGUILabel m_pTaskXPRewardLabel;

	// Token: 0x04000297 RID: 663
	private SBGUILabel m_pTaskProgressLabel;

	// Token: 0x04000298 RID: 664
	private SBGUILabel m_pTaskRushCostLabel;

	// Token: 0x04000299 RID: 665
	private SBGUILabel m_pCharacterNameLabel;

	// Token: 0x0400029A RID: 666
	private SBGUIProgressMeter m_pTaskProgressMeter;

	// Token: 0x0400029B RID: 667
	private SBGUIButton m_pRushTaskButton;

	// Token: 0x0400029C RID: 668
	private GameObject m_pTaskInProgressParent;

	// Token: 0x0400029D RID: 669
	private GameObject m_pTaskDoneParent;

	// Token: 0x0400029E RID: 670
	private Vector3 m_pCharacterPortraitSize;

	// Token: 0x0400029F RID: 671
	private Simulated m_pSimulated;

	// Token: 0x040002A0 RID: 672
	private ResidentEntity m_pEntity;

	// Token: 0x040002A1 RID: 673
	private Task m_pTask;

	// Token: 0x040002A2 RID: 674
	private Action m_pRushTaskAction;

	// Token: 0x040002A3 RID: 675
	private ulong m_ulTaskTimeLeft;

	// Token: 0x040002A4 RID: 676
	public int taskRushCost;
}
