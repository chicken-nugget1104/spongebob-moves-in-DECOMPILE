using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class SBGUIChunkQuestDialog : SBGUIScrollableDialog
{
	// Token: 0x0600040E RID: 1038 RVA: 0x000179F0 File Offset: 0x00015BF0
	protected override void Awake()
	{
		if (this.rewardWidgetPrefab == null)
		{
			this.rewardWidgetPrefab = (GameObject)Resources.Load("Prefabs/GUI/Widgets/RewardWidget");
		}
		this.rewardMarker = base.FindChild("reward_marker");
		base.Awake();
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00017A30 File Offset: 0x00015C30
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00017A3C File Offset: 0x00015C3C
	public void CreateScrollRegionUI(SBGUIStandardScreen screen, List<QuestBookendInfo.ChunkConditions> chunks, List<ConditionDescription> steps, Action findButtonHandler, string forcedStepPrefabName = null)
	{
		this.scrollSize = Vector2.zero;
		if (this.region.Marker.transform.GetChildCount() > 0)
		{
			TFUtils.ErrorLog("Child Count: " + this.region.Marker.transform.childCount);
			YGAtlasSprite[] componentsInChildren = this.region.Marker.GetComponentsInChildren<YGAtlasSprite>();
			foreach (YGAtlasSprite ygatlasSprite in componentsInChildren)
			{
				if (!string.IsNullOrEmpty(ygatlasSprite.nonAtlasName))
				{
					base.View.Library.incrementTextureDuplicates(ygatlasSprite.nonAtlasName);
				}
			}
			SBGUIElement[] componentsInChildren2 = this.region.Marker.GetComponentsInChildren<SBGUIElement>();
			foreach (SBGUIElement sbguielement in componentsInChildren2)
			{
				if (!(sbguielement == this.region.Marker))
				{
					UnityEngine.Object.Destroy(sbguielement.gameObject);
				}
			}
		}
		int num = 0;
		foreach (QuestBookendInfo.ChunkConditions chunkConditions in chunks)
		{
			Dictionary<string, object> dictionary = chunkConditions.Condition.ToDict();
			int? num2 = null;
			int? num3 = null;
			Simulated simBuilding = null;
			int nDID = 0;
			if (dictionary.ContainsKey("resource_id"))
			{
				int? num4 = new int?(TFUtils.LoadInt(dictionary, "resource_id"));
				CraftingRecipe recipeByProductId = this.session.TheGame.craftManager.GetRecipeByProductId(num4.Value);
				simBuilding = this.session.TheGame.simulation.FindSimulated(new int?(recipeByProductId.buildingId));
			}
			if (dictionary.ContainsKey("simulated_id"))
			{
				num3 = new int?(TFUtils.LoadInt(dictionary, "simulated_id"));
			}
			if (dictionary.ContainsKey("task_id"))
			{
				nDID = TFUtils.LoadInt(dictionary, "task_id");
			}
			string prefabName;
			if (forcedStepPrefabName == null)
			{
				if (steps[num].IsPassed)
				{
					prefabName = "Prefabs/GUI/Widgets/QuestCompleteChunk_Step";
				}
				else
				{
					prefabName = "Prefabs/GUI/Widgets/QuestStartChunk_Step";
				}
			}
			else
			{
				prefabName = forcedStepPrefabName;
			}
			SBGUIElement sbguielement2 = SBGUI.InstantiatePrefab(prefabName);
			SBGUIImage sbguiimage = (SBGUIImage)sbguielement2.FindChild("window");
			SBGUILabel sbguilabel = (SBGUILabel)sbguielement2.FindChild("description");
			SBGUIShadowedLabel sbguishadowedLabel = (SBGUIShadowedLabel)sbguielement2.FindChild("count");
			SBGUIButton sbguibutton = (SBGUIButton)sbguielement2.FindChild("store_button");
			SBGUIButton sbguibutton2 = (SBGUIButton)sbguielement2.FindChild("skip_button");
			SBGUIShadowedLabel sbguishadowedLabel2 = (SBGUIShadowedLabel)sbguielement2.FindChild("store_label");
			SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)sbguielement2.FindChild("icon");
			SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)sbguielement2.FindChild("description_boundary");
			SBGUIButton sbguibutton3 = (SBGUIButton)sbguielement2.FindChild("find_button");
			this.stepsMarker = base.FindChild("steps_marker");
			this.prefabIconSize = new int?((int)sbguiatlasImage.Size.x);
			sbguielement2.SetParent(this.stepsMarker);
			sbguielement2.tform.localPosition = Vector3.zero;
			sbguielement2.tform.localPosition = new Vector3(0f, -((sbguiimage.Size.y + -10f) * 0.01f) * (float)num, 0f);
			sbguilabel.SetText(Language.Get(chunkConditions.Name));
			sbguiatlasImage.SetTextureFromAtlas(chunkConditions.Icon, true, false, true, false, false, 0);
			if (sbguishadowedLabel)
			{
				if (chunkConditions.Condition.hasCountField)
				{
					uint num5 = steps[num].OccuranceCount;
					uint occurancesRequired = steps[num].OccurancesRequired;
					if (num5 > occurancesRequired)
					{
						num5 = occurancesRequired;
					}
					sbguishadowedLabel.SetText(num5 + " / " + occurancesRequired);
				}
				else
				{
					sbguishadowedLabel.SetActive(false);
					sbguilabel.transform.Translate(-0.45f, 0f, 0f, Space.Self);
					sbguiatlasImage2.transform.Translate(-0.45f, 0f, 0f, Space.Self);
					Vector2 size = sbguiatlasImage2.Size;
					size.x += 45f;
					sbguiatlasImage2.Size = size;
				}
			}
			if (!steps[num].IsPassed)
			{
				if (steps[num].OccuranceCount < steps[num].OccurancesRequired)
				{
					if (steps[num].OccuranceCount < steps[num].OccurancesRequired)
					{
						sbguibutton3.SetActive(false);
						TaskData taskData = this.session.TheGame.taskManager.GetTaskData(nDID, false);
						if (taskData != null)
						{
							int nSourceDID = taskData.m_nSourceDID;
							Simulated pSimulated = this.session.TheGame.simulation.FindSimulated(new int?(nSourceDID));
							bool flag = true;
							if (pSimulated != null && pSimulated.HasEntity<ResidentEntity>())
							{
								sbguibutton3.SetActive(true);
								ResidentEntity pResidentEntity = pSimulated.GetEntity<ResidentEntity>();
								if (pResidentEntity.m_pTask != null && (pResidentEntity.m_pTask.m_bMovingToTarget || pResidentEntity.m_pTask.m_bAtTarget))
								{
									this.residentPosX = (double)pResidentEntity.m_pTaskTargetPosition.x;
									this.residentPosY = (double)pResidentEntity.m_pTaskTargetPosition.y;
								}
								else
								{
									this.residentPosX = -1.0;
								}
								Action action = delegate()
								{
									this.session.TheSoundEffectManager.PlaySound("Accept");
									this.session.TheGame.selected = pSimulated;
									if (pResidentEntity.m_pTask != null && (pResidentEntity.m_pTask.m_bMovingToTarget || pResidentEntity.m_pTask.m_bAtTarget))
									{
										this.session.ChangeState("UnitBusy", true);
									}
								};
								base.AttachActionToButton(sbguibutton3, findButtonHandler);
								base.AttachActionToButton(sbguibutton3, action);
							}
							if (flag)
							{
								sbguibutton3.SetActive(true);
								Action action2 = delegate()
								{
									this.session.TheSoundEffectManager.PlaySound("Accept");
									this.session.TheGame.selected = pSimulated;
									ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
									if (this.residentPosX > 1000010.0 || this.residentPosX == 0.0 || pSimulated.PositionCenter.x > 1000010f || pSimulated.PositionCenter.x == 0f)
									{
										this.session.TheCamera.AutoPanToPosition(new Vector2(1000.9f, 667.5f), 0.75f);
									}
									else if (this.residentPosX > 0.0 && this.residentPosX < 1000010.0)
									{
										this.session.TheCamera.AutoPanToPosition(new Vector2((float)this.residentPosX, (float)this.residentPosY), 0.75f);
									}
									else if (this.residentPosX == -1.0)
									{
										this.session.TheCamera.AutoPanToPosition(pSimulated.PositionCenter, 0.75f);
										this.session.ChangeState("UnitIdle", true);
									}
								};
								base.AttachActionToButton(sbguibutton3, findButtonHandler);
								base.AttachActionToButton(sbguibutton3, action2);
							}
						}
					}
				}
			}
			sbguilabel.AdjustText(sbguiatlasImage2);
			if (sbguibutton)
			{
				if (simBuilding != null)
				{
					Action value = delegate()
					{
						this.session.TheGame.selected = simBuilding;
						simBuilding.InteractionState.SelectedTransition.Apply(this.session);
					};
					sbguishadowedLabel2.SetText(Language.Get("!!PREFAB_STORE"));
					sbguibutton.ClickEvent += value;
				}
				else if (num3 != null)
				{
					Action value = delegate()
					{
						SBGUIButton sbguibutton4 = (SBGUIButton)screen.FindChild("marketplace");
						sbguibutton4.MockClick();
					};
					sbguishadowedLabel2.SetText(Language.Get("!!PREFAB_STORE"));
					sbguibutton.ClickEvent += value;
				}
				else
				{
					UnityEngine.Object.Destroy(sbguibutton.gameObject);
					if (sbguibutton2)
					{
						UnityEngine.Object.Destroy(sbguibutton2.gameObject);
					}
				}
			}
			GUIMainView.GetInstance().Library.bShowingDialog = true;
			this.scrollSize += (sbguiimage.Size + new Vector2(-10f, -10f)) * 0.01f;
			float y = sbguielement2.transform.localPosition.y;
			sbguielement2.SetParent(this.region.Marker);
			Vector3 localPosition = new Vector3(0f, y, 0f);
			sbguielement2.transform.localPosition = localPosition;
			if (sbguibutton)
			{
				sbguibutton.UpdateCollider();
			}
			if (sbguibutton3)
			{
				sbguibutton3.UpdateCollider();
			}
			num++;
		}
		Rect rect = new Rect(0f, 0f, this.scrollSize.x, this.scrollSize.y);
		this.region.ResetScroll(rect);
		this.region.ResetToMinScroll();
		if (this.region.scrollBar.IsActive())
		{
			this.region.scrollBar.Reset();
		}
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00018304 File Offset: 0x00016504
	public void SetupChunkDialogInfo(string dialogHeading, string dialogBody, string portrait, string name, bool isComplete, QuestDefinition pQuestDef)
	{
		SBGUIShadowedLabel sbguishadowedLabel = (SBGUIShadowedLabel)base.FindChild("dialog_heading");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("dialog_body");
		SBGUIShadowedLabel sbguishadowedLabel2 = (SBGUIShadowedLabel)base.FindChild("description");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("portrait");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("portrait_shadow");
		SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)base.FindChild("dialog_body_boundary");
		SBGUIShadowedLabel sbguishadowedLabel3 = (SBGUIShadowedLabel)base.FindChild("reward_label");
		this.questlineRewardIcon = (SBGUIAtlasImage)base.FindChild("questline_reward_item");
		this.questRewardIcon = (SBGUIAtlasImage)base.FindChild("reward_item");
		this.progressMeter = (SBGUIProgressMeter)base.FindChild("progress_meter");
		this.progressbar_group = base.FindChild("progressbar_group");
		this.rewardItemBg = base.FindChild("reward_item_bg");
		this.stepsMarker = base.FindChild("steps_marker");
		this.window = (SBGUIAtlasImage)base.FindChild("window");
		this.okayButton = (SBGUIPulseButton)base.FindChild("okay");
		int? num = this.questLineIconSize;
		if (num == null)
		{
			this.questLineIconSize = new int?((int)this.questlineRewardIcon.Size.x);
		}
		int? num2 = this.questIconSize;
		if (num2 == null)
		{
			this.questIconSize = new int?((int)this.questRewardIcon.Size.x);
		}
		Vector2? vector = this.prefabWindowSize;
		if (vector == null)
		{
			this.prefabWindowSize = new Vector2?(this.window.Size);
		}
		Vector3? vector2 = this.prefabOkayButtonPos;
		if (vector2 == null)
		{
			this.prefabOkayButtonPos = new Vector3?(this.okayButton.tform.localPosition);
		}
		this.rewardItemBg.SetActive(false);
		if (!this.rewardItemBg.IsActive() && isComplete)
		{
			this.rewardMarker.transform.Translate(new Vector3(0f, 0.5f, 0f));
		}
		if (dialogHeading != string.Empty)
		{
			sbguishadowedLabel.SetText(Language.Get(dialogHeading));
		}
		else
		{
			sbguishadowedLabel.SetActive(false);
			sbguilabel.transform.localPosition = sbguiatlasImage3.transform.localPosition;
		}
		sbguishadowedLabel2.SetText(Language.Get(name));
		sbguilabel.SetText(Language.Get(dialogBody));
		sbguilabel.AdjustText(sbguiatlasImage3);
		sbguishadowedLabel3.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		sbguiatlasImage.SetTextureFromAtlas(portrait, true, false, false, false, false, 0);
		sbguiatlasImage2.renderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x000185FC File Offset: 0x000167FC
	public void SetQuestLineInfo(QuestLineInfo questLine, float? start, float? progress, bool skipAnimation)
	{
		if (progress != null)
		{
			if (!this.progressbar_group.IsActive())
			{
				this.progressbar_group.SetActive(true);
			}
			if (this.window.Size != this.prefabWindowSize)
			{
				SBGUIImage sbguiimage = this.window;
				Vector2? vector = this.prefabWindowSize;
				sbguiimage.Size = vector.Value;
			}
			if (this.okayButton.tform.localPosition != this.prefabOkayButtonPos)
			{
				Transform tform = this.okayButton.tform;
				Vector3? vector2 = this.prefabOkayButtonPos;
				tform.localPosition = vector2.Value;
			}
			if (skipAnimation)
			{
				this.progressMeter.Progress = progress.Value;
			}
			else
			{
				float duration = (progress.Value - start.Value) / 0.1f;
				this.progressMeter.ForceAnimatedProgress(start.Value, progress.Value, duration);
				if (this.progressBarParticle != null)
				{
					this.progressBarParticle = (ParticleSystem)UnityEngine.Object.Instantiate(this.progressBarParticle);
					this.progressBarParticle.transform.parent = this.progressMeter.fill.transform;
					this.progressBarParticle.transform.localPosition = Vector3.zero;
					base.StartCoroutine(this.AnimateParticlePosition(duration));
				}
			}
			this.questlineRewardIcon.SetTextureFromAtlas(questLine.Icon);
			SBGUIAtlasImage sbguiatlasImage = this.questlineRewardIcon;
			int? num = this.questLineIconSize;
			sbguiatlasImage.ScaleToMaxSize(num.Value);
		}
		else
		{
			if (this.progressbar_group.IsActive())
			{
				this.progressbar_group.SetActive(false);
			}
			if (this.window.Size == this.prefabWindowSize)
			{
				Vector2? vector3 = this.prefabWindowSize;
				Vector2 value = vector3.Value;
				value.y -= 71f;
				this.window.Size = value;
			}
			if (this.okayButton.tform.localPosition == this.prefabOkayButtonPos)
			{
				Vector3? vector4 = this.prefabOkayButtonPos;
				Vector3 value2 = vector4.Value;
				value2.y += 0.71f;
				this.okayButton.tform.localPosition = value2;
			}
		}
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x000188A4 File Offset: 0x00016AA4
	private IEnumerator AnimateParticlePosition(float duration)
	{
		this.progressBarParticle.Play();
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			Vector3 position = this.progressBarParticle.transform.localPosition;
			position.x = this.progressMeter.meter.Size.x * this.progressMeter.Progress;
			position *= 0.01f;
			position.z = -0.3f;
			this.progressBarParticle.transform.localPosition = position;
			yield return null;
		}
		this.progressBarParticle.Stop();
		yield return null;
		yield break;
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x000188D0 File Offset: 0x00016AD0
	public virtual void AddItem(string texture, int amount, string prefix)
	{
		if (amount == 0)
		{
			TFUtils.WarningLog("rewarding 0 of :" + texture);
			return;
		}
		if (texture == null || string.IsNullOrEmpty(texture.Trim()))
		{
			TFUtils.WarningLog("resource has no texture");
			return;
		}
		SBGUIRewardWidget item = SBGUIRewardWidget.Create(this.rewardWidgetPrefab, this.rewardMarker, this.markerXOffset, texture, amount, prefix);
		this.rewards.Add(item);
		this.markerXOffset = 0f;
		float num = 1f;
		float y = 0f;
		if (this.rewards.Count > 3)
		{
			num = 0.5f;
			y = 0.1f;
		}
		foreach (SBGUIRewardWidget sbguirewardWidget in this.rewards)
		{
			sbguirewardWidget.transform.localScale = new Vector3(num, num, num);
			sbguirewardWidget.transform.localPosition = new Vector3(this.markerXOffset, y, 0f);
			this.markerXOffset += (float)(sbguirewardWidget.Width + 10) * num * 0.01f;
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00018A14 File Offset: 0x00016C14
	private void ClearItems()
	{
		this.markerXOffset = 0f;
		foreach (SBGUIRewardWidget sbguirewardWidget in this.rewards)
		{
			sbguirewardWidget.gameObject.SetActiveRecursively(false);
			UnityEngine.Object.Destroy(sbguirewardWidget.gameObject);
		}
		this.rewards.Clear();
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00018AA0 File Offset: 0x00016CA0
	private void InitializeRewardComponentAmounts(Reward reward, Dictionary<int, int> componentAmounts, Dictionary<int, int> outAmounts)
	{
		outAmounts.Clear();
		foreach (KeyValuePair<int, int> keyValuePair in componentAmounts)
		{
			int key = keyValuePair.Key;
			int value = keyValuePair.Value;
			if (!outAmounts.ContainsKey(key))
			{
				outAmounts[key] = 0;
			}
			int num;
			int key2 = num = key;
			num = outAmounts[num];
			outAmounts[key2] = num + value;
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00018B40 File Offset: 0x00016D40
	public void SetRewardIcons(Session session, List<Reward> rewards, string prefix)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		this.ClearItems();
		foreach (Reward reward in rewards)
		{
			if (reward != null)
			{
				this.InitializeRewardComponentAmounts(reward, reward.ResourceAmounts, dictionary);
				this.InitializeRewardComponentAmounts(reward, reward.BuildingAmounts, dictionary2);
			}
		}
		foreach (KeyValuePair<int, int> keyValuePair in dictionary)
		{
			int key = keyValuePair.Key;
			int value = keyValuePair.Value;
			this.AddItem(session.TheGame.resourceManager.Resources[key].GetResourceTexture(), value, prefix);
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in dictionary2)
		{
			int value2 = keyValuePair2.Value;
			Blueprint blueprint = EntityManager.GetBlueprint("building", keyValuePair2.Key, false);
			this.AddItem((string)blueprint.Invariable["portrait"], value2, prefix);
		}
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00018CDC File Offset: 0x00016EDC
	public void CenterRewards()
	{
		Vector3 position = this.rewardMarker.tform.position;
		Vector3 vector = this.rewardMarker.TotalBounds.center - position;
		Vector3 localPosition = this.rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		this.rewardMarker.tform.localPosition = localPosition;
	}

	// Token: 0x040002E9 RID: 745
	public const int STEP_GAP = -10;

	// Token: 0x040002EA RID: 746
	private const float PROGRESSBAR_HEIGHT = 71f;

	// Token: 0x040002EB RID: 747
	private const float PROGRESSBAR_FILLRATE = 0.1f;

	// Token: 0x040002EC RID: 748
	private const int REWARD_GAP_SIZE = 10;

	// Token: 0x040002ED RID: 749
	public GameObject rewardWidgetPrefab;

	// Token: 0x040002EE RID: 750
	public ParticleSystem progressBarParticle;

	// Token: 0x040002EF RID: 751
	private List<SBGUIRewardWidget> rewards = new List<SBGUIRewardWidget>();

	// Token: 0x040002F0 RID: 752
	private int? prefabIconSize;

	// Token: 0x040002F1 RID: 753
	private int? questLineIconSize;

	// Token: 0x040002F2 RID: 754
	private int? questIconSize;

	// Token: 0x040002F3 RID: 755
	private float markerXOffset;

	// Token: 0x040002F4 RID: 756
	private Vector2 scrollSize;

	// Token: 0x040002F5 RID: 757
	private Vector2? prefabWindowSize;

	// Token: 0x040002F6 RID: 758
	private Vector3? prefabOkayButtonPos;

	// Token: 0x040002F7 RID: 759
	private SBGUIProgressMeter progressMeter;

	// Token: 0x040002F8 RID: 760
	private SBGUIPulseButton okayButton;

	// Token: 0x040002F9 RID: 761
	private SBGUIAtlasImage questlineRewardIcon;

	// Token: 0x040002FA RID: 762
	private SBGUIAtlasImage questRewardIcon;

	// Token: 0x040002FB RID: 763
	private SBGUIAtlasImage window;

	// Token: 0x040002FC RID: 764
	private SBGUIElement rewardItemBg;

	// Token: 0x040002FD RID: 765
	private SBGUIElement progressbar_group;

	// Token: 0x040002FE RID: 766
	private SBGUIElement stepsMarker;

	// Token: 0x040002FF RID: 767
	protected SBGUIElement rewardMarker;

	// Token: 0x04000300 RID: 768
	private double residentPosX;

	// Token: 0x04000301 RID: 769
	private double residentPosY;
}
