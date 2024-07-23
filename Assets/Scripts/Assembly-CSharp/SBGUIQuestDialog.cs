using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class SBGUIQuestDialog : SBGUIModalDialog
{
	// Token: 0x060005AD RID: 1453 RVA: 0x00024288 File Offset: 0x00022488
	protected override void Awake()
	{
		this.rewardMarker = base.FindChild("reward_marker");
		base.Awake();
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x000242A4 File Offset: 0x000224A4
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x000242B0 File Offset: 0x000224B0
	public void SetupQuestDialogInfo(string name, string icon, List<ConditionDescription> steps, bool hasCount, List<QuestBookendInfo.ChunkConditions> chunks, Action findButtonHandler)
	{
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("description_label");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("icon");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("count");
		SBGUIButton sbguibutton = (SBGUIButton)base.FindChild("find_button");
		SBGUIButton sbguibutton2 = (SBGUIButton)base.FindChild("okay");
		int? num = this.prefabIconSize;
		if (num == null)
		{
			this.prefabIconSize = new int?((int)sbguiatlasImage.Size.x);
		}
		sbguilabel.SetText(name);
		sbguibutton.gameObject.SetActive(false);
		sbguibutton2.gameObject.SetActive(false);
		if (steps != null && hasCount)
		{
			if (sbguilabel2 != null)
			{
				sbguilabel2.SetActive(true);
				sbguilabel2.SetText(steps[0].OccuranceCount + "/" + steps[0].OccurancesRequired);
			}
		}
		else if (sbguilabel2 != null)
		{
			sbguilabel2.SetActive(false);
		}
		if (sbguiatlasImage != null)
		{
			SBGUIAtlasImage sbguiatlasImage2 = sbguiatlasImage;
			bool resize = true;
			bool resizeToTrimmed = false;
			bool resizeToFit = true;
			bool keepSmallSize = false;
			bool explanationDialog = false;
			int? num2 = this.prefabIconSize;
			sbguiatlasImage2.SetTextureFromAtlas(icon, resize, resizeToTrimmed, resizeToFit, keepSmallSize, explanationDialog, num2.Value);
		}
		if (chunks.Count > 0)
		{
			Dictionary<string, object> dictionary = chunks[0].Condition.ToDict();
			int num3 = 0;
			int num4 = 0;
			if (dictionary.ContainsKey("task_id"))
			{
				num3 = TFUtils.LoadInt(dictionary, "task_id");
			}
			if (dictionary.ContainsKey("costume_id"))
			{
				num4 = TFUtils.LoadInt(dictionary, "costume_id");
			}
			if (num3 != 0 || num4 != 0)
			{
				sbguibutton.SetActive(true);
				sbguibutton2.SetActive(false);
			}
			else
			{
				sbguibutton.SetActive(false);
				sbguibutton2.SetActive(true);
			}
			if (num3 != 0 || num4 != 0)
			{
				TaskData taskData = null;
				Simulated costumeSourceDID = null;
				if (num3 != 0)
				{
					taskData = this.session.TheGame.taskManager.GetTaskData(num3, false);
				}
				if (num4 != 0)
				{
					costumeSourceDID = this.session.TheGame.simulation.FindSimulated(new int?(this.session.TheGame.costumeManager.GetCostume(num4).m_nUnitDID));
				}
				if (taskData != null)
				{
					TFUtils.ErrorLog("Task Data != null - line 130 SBGUIQuestDialog.cs");
					int nSourceDID = taskData.m_nSourceDID;
					Simulated pSimulated = this.session.TheGame.simulation.FindSimulated(new int?(nSourceDID));
					bool flag = true;
					if (pSimulated != null && pSimulated.HasEntity<ResidentEntity>())
					{
						TFUtils.ErrorLog("pSimulated != null - line 143 SBGUIQuestDialog.cs");
						ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
						if (entity.m_pTask != null && (entity.m_pTask.m_bMovingToTarget || entity.m_pTask.m_bAtTarget))
						{
							this.residentPosX = (double)entity.m_pTaskTargetPosition.x;
							this.residentPosY = (double)entity.m_pTaskTargetPosition.y;
							TFUtils.ErrorLog("pResidentEntity.m_pTask != null - line 158 SBGUIQuestDialog.cs");
						}
						else
						{
							TFUtils.ErrorLog("pResidentEntity.m_pTask == null - line 162 SBGUIQuestDialog.cs");
							this.residentPosX = -1.0;
						}
					}
					if (flag)
					{
						Action action = delegate()
						{
							this.session.TheSoundEffectManager.PlaySound("Accept");
							this.session.TheGame.selected = pSimulated;
							ResidentEntity entity3 = pSimulated.GetEntity<ResidentEntity>();
							if (this.residentPosX > 1000010.0 || this.residentPosX == 0.0 || pSimulated.PositionCenter.x > 1000010f || pSimulated.PositionCenter.x == 0f)
							{
								this.session.TheCamera.AutoPanToPosition(new Vector2(1000.9f, 667.5f), 0.75f);
								if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
								{
									this.session.ChangeState("UnitBusy", true);
									TFUtils.ErrorLog("ChangeState BUSY - task != null, moving to or at target 224");
									pSimulated.InteractionState.SelectedTransition = new Session.UnitBusyTransition(pSimulated);
								}
								else
								{
									this.session.ChangeState("UnitIdle", true);
									TFUtils.ErrorLog("ChangeState IDLE - line 229 SBGUIQuestDialog.cs");
									pSimulated.InteractionState.SelectedTransition = new Session.UnitIdleTransition(pSimulated);
								}
							}
							else if (this.residentPosX > 0.0 && this.residentPosX < 1000010.0)
							{
								this.session.TheCamera.AutoPanToPosition(new Vector2((float)this.residentPosX, (float)this.residentPosY), 0.75f);
								if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
								{
									this.session.ChangeState("UnitBusy", true);
									pSimulated.InteractionState.SelectedTransition = new Session.UnitBusyTransition(pSimulated);
								}
								else
								{
									this.session.ChangeState("UnitIdle", true);
									pSimulated.InteractionState.SelectedTransition = new Session.UnitIdleTransition(pSimulated);
								}
							}
							else if (this.residentPosX == -1.0)
							{
								this.session.TheCamera.AutoPanToPosition(pSimulated.PositionCenter, 0.75f);
								if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
								{
									this.session.ChangeState("UnitBusy", true);
									TFUtils.ErrorLog("ChangeState BUSY - Task != null, mvoing to or at target");
								}
								else
								{
									this.session.ChangeState("UnitIdle", true);
									TFUtils.ErrorLog("ChangeState IDLE - resident position = -1 ");
								}
							}
						};
						base.AttachActionToButton(sbguibutton, findButtonHandler);
						base.AttachActionToButton(sbguibutton, action);
					}
				}
				else if (costumeSourceDID != null)
				{
					TFUtils.ErrorLog("costumeSourceDID != null ");
					bool flag2 = true;
					if (costumeSourceDID != null && costumeSourceDID.HasEntity<ResidentEntity>())
					{
						ResidentEntity entity2 = costumeSourceDID.GetEntity<ResidentEntity>();
						if (entity2.CostumeDID != null)
						{
							this.residentPosX = (double)costumeSourceDID.Position.x;
							this.residentPosY = (double)costumeSourceDID.Position.y;
						}
						else
						{
							this.residentPosX = -1.0;
						}
					}
					if (flag2)
					{
						Action action2 = delegate()
						{
							this.session.TheSoundEffectManager.PlaySound("Accept");
							this.session.TheGame.selected = costumeSourceDID;
							ResidentEntity entity3 = costumeSourceDID.GetEntity<ResidentEntity>();
							if (this.residentPosX > 1000010.0 || this.residentPosX == 0.0 || costumeSourceDID.PositionCenter.x > 1000010f || costumeSourceDID.PositionCenter.x == 0f)
							{
								this.session.TheCamera.AutoPanToPosition(new Vector2(1000.9f, 667.5f), 0.75f);
								if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
								{
									this.session.ChangeState("UnitBusy", true);
								}
								else
								{
									this.session.ChangeState("UnitIdle", true);
								}
							}
							else if (this.residentPosX > 0.0 && this.residentPosX < 1000010.0)
							{
								this.session.TheCamera.AutoPanToPosition(new Vector2((float)this.residentPosX, (float)this.residentPosY), 0.75f);
								if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
								{
									this.session.ChangeState("UnitBusy", true);
								}
								else
								{
									this.session.ChangeState("UnitIdle", true);
								}
							}
							else if (this.residentPosX == -1.0)
							{
								this.session.TheCamera.AutoPanToPosition(costumeSourceDID.PositionCenter, 0.75f);
								if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
								{
									this.session.ChangeState("UnitBusy", true);
								}
								else
								{
									this.session.ChangeState("UnitIdle", true);
								}
							}
						};
						base.AttachActionToButton(sbguibutton, findButtonHandler);
						base.AttachActionToButton(sbguibutton, action2);
					}
				}
			}
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00024724 File Offset: 0x00022924
	public void SetupQuestDialogInfo(string name, string icon, List<ConditionDescription> steps, bool hasCount)
	{
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("description_label");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("icon");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("count");
		SBGUIButton sbguibutton = (SBGUIButton)base.FindChild("find_button");
		sbguibutton.gameObject.SetActive(false);
		int? num = this.prefabIconSize;
		if (num == null)
		{
			this.prefabIconSize = new int?((int)sbguiatlasImage.Size.x);
		}
		sbguilabel.SetText(name);
		if (steps != null && hasCount)
		{
			if (sbguilabel2 != null)
			{
				sbguilabel2.SetActive(true);
				sbguilabel2.SetText(steps[0].OccuranceCount + "/" + steps[0].OccurancesRequired);
			}
		}
		else if (sbguilabel2 != null)
		{
			sbguilabel2.SetActive(false);
		}
		if (sbguiatlasImage != null)
		{
			SBGUIAtlasImage sbguiatlasImage2 = sbguiatlasImage;
			bool resize = true;
			bool resizeToTrimmed = false;
			bool resizeToFit = true;
			bool keepSmallSize = false;
			bool explanationDialog = false;
			int? num2 = this.prefabIconSize;
			sbguiatlasImage2.SetTextureFromAtlas(icon, resize, resizeToTrimmed, resizeToFit, keepSmallSize, explanationDialog, num2.Value);
		}
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00024858 File Offset: 0x00022A58
	public void SetupQuestDialogInfo(string name, string icon)
	{
		this.SetupQuestDialogInfo(name, icon, null, false);
	}

	// Token: 0x0400045B RID: 1115
	public const int STEP_GAP = 4;

	// Token: 0x0400045C RID: 1116
	private double residentPosX;

	// Token: 0x0400045D RID: 1117
	private double residentPosY;

	// Token: 0x0400045E RID: 1118
	private int? prefabIconSize;
}
