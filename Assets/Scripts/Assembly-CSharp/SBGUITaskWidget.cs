using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class SBGUITaskWidget : SBGUIElement
{
	// Token: 0x060006D0 RID: 1744 RVA: 0x0002AA20 File Offset: 0x00028C20
	public void SetData(Session pSession, Action pDoTaskAction, TaskData pTaskData, int nCostumeDID, int costumeCount)
	{
		this.m_pDoTaskButton.ClearClickEvents();
		base.AttachActionToButton(this.m_pDoTaskButton, pDoTaskAction);
		this.m_pDoTaskButton.UpdateCollider();
		this.m_pDoTaskButton.SessionActionId = "task_do_button_" + pTaskData.m_sName;
		this.m_pTaskNameLabel.SetText(Language.Get(pTaskData.m_sName));
		this.m_pTaskDurationLabel.SetText(TFUtils.DurationToString((ulong)((long)pTaskData.m_nDuration)));
		this.m_pTaskXPRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[ResourceManager.XP].ToString());
		foreach (KeyValuePair<int, int> keyValuePair in pTaskData.m_pReward.ResourceAmounts)
		{
			if (keyValuePair.Key != ResourceManager.XP)
			{
				this.m_pTaskSoftRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[keyValuePair.Key].ToString());
				this.m_pCurrencyImage.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[keyValuePair.Key].GetResourceTexture());
				break;
			}
		}
		if (pTaskData.tasksHasBonus.Contains(pTaskData.m_nDID))
		{
			this.m_pTaskBonusRewardIcon.SetTextureFromAtlas(pTaskData.m_sPaytableRewardIcon);
			this.m_pTaskBonusRewardIcon.SetActive(true);
		}
		else
		{
			this.m_pTaskBonusRewardIcon.gameObject.SetActive(false);
		}
		TaskManager.TaskBlockedStatus taskBlockedStatus = pSession.TheGame.taskManager.GetTaskBlockedStatus(pSession.TheGame, pTaskData, nCostumeDID);
		bool flag = taskBlockedStatus.m_eTaskBlockedType == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone;
		if (costumeCount > 0 && pSession.TheGame.costumeManager.IsCostumeUnlocked(nCostumeDID) && flag)
		{
			this.m_pLockedParent.SetActive(false);
			this.m_pUnlockedParent.SetActive(true);
		}
		else if (costumeCount == 0 && flag)
		{
			this.m_pLockedParent.SetActive(false);
			this.m_pUnlockedParent.SetActive(true);
		}
		else
		{
			this.m_pLockedParent.SetActive(true);
			this.m_pUnlockedParent.SetActive(false);
			this.m_pLockedLevelLabel.SetActive(false);
			this.m_pLockedLevelLabelSmall.SetActive(false);
			this.m_pLockedLevelImage.SetActive(false);
			this.m_pLockedLevelImageSmall.SetActive(false);
			bool flag2 = (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eLevel) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone;
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eTarget) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				this.m_pLockedImage.SetActive(true);
				this.m_pLockedBackingImage.SetActive(true);
				if (flag2)
				{
					this.m_pLockedLevelLabelSmall.SetActive(true);
					this.m_pLockedLevelImageSmall.SetActive(true);
					this.m_pLockedLevelLabelSmall.SetText(pTaskData.m_nMinLevel.ToString());
				}
				Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, pTaskData.m_nTargetDID, false);
				if (blueprint.Invariable.ContainsKey("portrait"))
				{
					this.m_pLockedImage.SetSizeNoRebuild(this.m_pLockedIconSize);
					this.m_pLockedImage.SetTextureFromAtlas((string)blueprint.Invariable["portrait"], true, false, true, false, false, 0);
				}
				else
				{
					this.m_pLockedImage.SetActive(false);
				}
			}
			else if (flag2)
			{
				this.m_pLockedImage.SetActive(false);
				this.m_pLockedBackingImage.SetActive(false);
				this.m_pLockedLevelLabel.SetActive(true);
				this.m_pLockedLevelImage.SetActive(true);
				this.m_pLockedLevelLabel.SetText(pTaskData.m_nMinLevel.ToString());
			}
		}
		this.nTaskDID = pTaskData.m_nDID;
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0002ADF4 File Offset: 0x00028FF4
	protected override void Awake()
	{
		this.m_pCurrencyImage = (SBGUIAtlasImage)base.FindChild("soft_currency_icon");
		this.m_pLockedImage = (SBGUIAtlasImage)base.FindChild("locked_icon");
		this.m_pLockedBackingImage = (SBGUIAtlasImage)base.FindChild("locked_icon_backing");
		this.m_pLockedLevelImage = (SBGUIAtlasImage)base.FindChild("locked_level_icon");
		this.m_pLockedLevelImageSmall = (SBGUIAtlasImage)base.FindChild("locked_level_icon_small");
		this.m_pTaskNameLabel = (SBGUILabel)base.FindChild("task_name_label");
		this.m_pTaskDurationLabel = (SBGUILabel)base.FindChild("task_duration_label");
		this.m_pTaskXPRewardLabel = (SBGUILabel)base.FindChild("task_xp_reward_label");
		this.m_pTaskSoftRewardLabel = (SBGUILabel)base.FindChild("task_soft_reward_label");
		this.m_pLockedLevelLabel = (SBGUILabel)base.FindChild("locked_level_label");
		this.m_pLockedLevelLabelSmall = (SBGUILabel)base.FindChild("locked_level_label_small");
		this.m_pDoTaskButton = (SBGUIButton)base.FindChild("do_task_button");
		this.m_pTaskBonusRewardIcon = (SBGUIAtlasImage)base.FindChild("task_bonus_icon");
		this.m_pLockedIconSize = this.m_pLockedImage.Size;
		this.m_pLockedParent = this.m_pLockedImage.transform.parent.gameObject;
		this.m_pUnlockedParent = this.m_pDoTaskButton.transform.parent.gameObject;
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("reward_title_label");
		sbguilabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		base.Awake();
	}

	// Token: 0x04000512 RID: 1298
	private SBGUIAtlasImage m_pCurrencyImage;

	// Token: 0x04000513 RID: 1299
	private SBGUIAtlasImage m_pLockedImage;

	// Token: 0x04000514 RID: 1300
	private SBGUIAtlasImage m_pLockedBackingImage;

	// Token: 0x04000515 RID: 1301
	private SBGUIAtlasImage m_pLockedLevelImage;

	// Token: 0x04000516 RID: 1302
	private SBGUIAtlasImage m_pLockedLevelImageSmall;

	// Token: 0x04000517 RID: 1303
	private SBGUILabel m_pTaskNameLabel;

	// Token: 0x04000518 RID: 1304
	private SBGUILabel m_pTaskDurationLabel;

	// Token: 0x04000519 RID: 1305
	private SBGUILabel m_pTaskXPRewardLabel;

	// Token: 0x0400051A RID: 1306
	private SBGUILabel m_pTaskSoftRewardLabel;

	// Token: 0x0400051B RID: 1307
	private SBGUILabel m_pLockedLevelLabel;

	// Token: 0x0400051C RID: 1308
	private SBGUILabel m_pLockedLevelLabelSmall;

	// Token: 0x0400051D RID: 1309
	private SBGUIButton m_pDoTaskButton;

	// Token: 0x0400051E RID: 1310
	private Vector2 m_pLockedIconSize;

	// Token: 0x0400051F RID: 1311
	private GameObject m_pLockedParent;

	// Token: 0x04000520 RID: 1312
	private GameObject m_pUnlockedParent;

	// Token: 0x04000521 RID: 1313
	private SBGUIAtlasImage m_pTaskBonusRewardIcon;

	// Token: 0x04000522 RID: 1314
	private int nTaskDID;
}
