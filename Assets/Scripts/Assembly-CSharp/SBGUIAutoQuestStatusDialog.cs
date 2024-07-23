using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class SBGUIAutoQuestStatusDialog : SBGUIScrollableDialog
{
	// Token: 0x060003C9 RID: 969 RVA: 0x0001407C File Offset: 0x0001227C
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00014088 File Offset: 0x00012288
	public void CreateScrollRegionUI(SBGUIStandardScreen screen, List<QuestBookendInfo.ChunkConditions> chunks, List<ConditionDescription> steps, Action makeButtonHandler, string forcedStepPrefabName = null)
	{
		this.scrollSize = Vector2.zero;
		if (this.region.Marker.transform.GetChildCount() > 0)
		{
			SBGUIElement[] componentsInChildren = this.region.Marker.GetComponentsInChildren<SBGUIElement>();
			YGAtlasSprite[] componentsInChildren2 = this.region.Marker.GetComponentsInChildren<YGAtlasSprite>();
			foreach (YGAtlasSprite ygatlasSprite in componentsInChildren2)
			{
				if (!string.IsNullOrEmpty(ygatlasSprite.nonAtlasName))
				{
					base.View.Library.incrementTextureDuplicates(ygatlasSprite.nonAtlasName);
				}
			}
			foreach (SBGUIElement sbguielement in componentsInChildren)
			{
				if (!(sbguielement == this.region.Marker))
				{
					UnityEngine.Object.Destroy(sbguielement.gameObject);
				}
			}
		}
		int count = chunks.Count;
		List<QuestBookendInfo.ChunkConditions> list = new List<QuestBookendInfo.ChunkConditions>(count);
		List<ConditionDescription> list2 = new List<ConditionDescription>(count);
		int k;
		for (k = 0; k < count; k++)
		{
			if (!steps[k].IsPassed && k != count - 1)
			{
				list.Insert(0, chunks[k]);
				list2.Insert(0, steps[k]);
			}
			else
			{
				list.Add(chunks[k]);
				list2.Add(steps[k]);
			}
		}
		k = 0;
		this.numChunksLeft = count - 1;
		foreach (QuestBookendInfo.ChunkConditions chunkConditions in list)
		{
			if (k >= count - 1)
			{
				break;
			}
			Dictionary<string, object> dictionary = chunkConditions.Condition.ToDict();
			int? num = null;
			int? num2 = null;
			string prefabName = "Prefabs/GUI/Widgets/AutoQuest_Step";
			if (dictionary.ContainsKey("resource_id"))
			{
				int? num3 = new int?(TFUtils.LoadInt(dictionary, "resource_id"));
				CraftingRecipe recipeByProductId = this.session.TheGame.craftManager.GetRecipeByProductId(num3.Value);
				Simulated simulated = this.session.TheGame.simulation.FindSimulated(new int?(recipeByProductId.buildingId));
			}
			if (dictionary.ContainsKey("simulated_id"))
			{
				int? num4 = new int?(TFUtils.LoadInt(dictionary, "simulated_id"));
			}
			SBGUIElement sbguielement2 = SBGUI.InstantiatePrefab(prefabName);
			SBGUIImage sbguiimage = (SBGUIImage)sbguielement2.FindChild("window");
			SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)sbguielement2.FindChild("item_icon");
			SBGUILabel sbguilabel = (SBGUILabel)sbguielement2.FindChild("item_name");
			SBGUIAtlasImage boundary = (SBGUIAtlasImage)sbguielement2.FindChild("item_name_boundary");
			SBGUILabel stepYouHave = (SBGUILabel)sbguielement2.FindChild("you_have_amount");
			SBGUILabel sbguilabel2 = (SBGUILabel)sbguielement2.FindChild("you_need_amount");
			SBGUILabel stepYouHaveTitle = (SBGUILabel)sbguielement2.FindChild("you_have_title");
			SBGUILabel stepYouNeedTitle = (SBGUILabel)sbguielement2.FindChild("you_need_title");
			SBGUIButton sbguibutton = (SBGUIButton)sbguielement2.FindChild("make_button");
			SBGUIButton collectButton = (SBGUIButton)sbguielement2.FindChild("collect_button");
			GameObject doneGO = sbguielement2.FindChild("done_icon").gameObject.transform.parent.gameObject;
			this.stepsMarker = base.FindChild("steps_marker");
			sbguielement2.SetParent(this.stepsMarker);
			sbguielement2.tform.localPosition = Vector3.zero;
			sbguielement2.tform.localPosition = new Vector3(0f, -(sbguiimage.Size.y * 0.01f) * (float)k, 0f);
			Dictionary<string, object> pCondition = chunkConditions.Condition.ToDict();
			int nYouNeed = 0;
			if (pCondition.ContainsKey("count"))
			{
				nYouNeed = TFUtils.LoadInt(pCondition, "count");
				sbguilabel2.SetText(nYouNeed.ToString());
			}
			int num5 = 0;
			Resource resource = null;
			if (pCondition.ContainsKey("resource_id"))
			{
				int key = TFUtils.LoadInt(pCondition, "resource_id");
				resource = this.session.TheGame.resourceManager.Resources[key];
				sbguiatlasImage.SetTextureFromAtlas(resource.GetResourceTexture(), false, false, false, false, false, 0);
				sbguiatlasImage.ScaleToMaxSize((int)sbguiatlasImage.Size.x);
				sbguilabel.SetText(Language.Get(resource.Name));
				sbguilabel.AdjustText(boundary);
				num5 = resource.Amount;
				stepYouHave.SetText(num5.ToString());
			}
			doneGO.SetActive(list2[k].IsPassed);
			if (list2[k].IsPassed)
			{
				this.numChunksLeft--;
				sbguibutton.SetActive(false);
				collectButton.SetActive(false);
				stepYouHave.SetActive(false);
				stepYouHaveTitle.SetActive(false);
				stepYouNeedTitle.SetActive(false);
			}
			else if (num5 >= nYouNeed)
			{
				sbguibutton.SetActive(false);
				collectButton.SetActive(true);
				stepYouHave.SetColor(Color.blue);
				Action action = delegate()
				{
					this.session.TheSoundEffectManager.PlaySound("Accept");
					if (pCondition.ContainsKey("resource_id"))
					{
						int nDID = TFUtils.LoadInt(pCondition, "resource_id");
						this.session.TheGame.simulation.ModifyGameStateSimulated(this.session.TheGame.simulation.FindSimulated(new int?(1018)), new AutoQuestCraftCollectAction(nDID, nYouNeed));
						stepYouHave.SetActive(false);
						stepYouHaveTitle.SetActive(false);
						stepYouNeedTitle.SetActive(false);
						collectButton.SetActive(false);
						doneGO.SetActive(true);
						this.numChunksLeft--;
						if (this.numChunksLeft <= 0)
						{
							this.okayButton.SetActive(false);
							this.allDoneButton.SetActive(true);
						}
					}
				};
				base.AttachActionToButton(collectButton, action);
			}
			else
			{
				sbguibutton.SetActive(false);
				collectButton.SetActive(false);
				stepYouHave.SetColor(Color.red);
				if (resource != null)
				{
					CraftingRecipe recipeByProductId2 = this.session.TheGame.craftManager.GetRecipeByProductId(resource.Did);
					if (recipeByProductId2 != null)
					{
						Simulated pSimulated = this.session.TheGame.simulation.FindSimulated(new int?(recipeByProductId2.buildingId));
						if (pSimulated != null && pSimulated.HasEntity<BuildingEntity>())
						{
							bool flag = true;
							BuildingEntity entity = pSimulated.GetEntity<BuildingEntity>();
							if (entity.CanCraft && this.session.TheGame.craftManager.GetCookbookById(entity.CraftMenu) != null && entity.HasSlots)
							{
								sbguibutton.SetActive(true);
								Action action2 = delegate()
								{
									this.session.TheSoundEffectManager.PlaySound("Accept");
									this.session.TheGame.selected = pSimulated;
									this.session.ChangeState("BrowsingRecipes", true);
								};
								base.AttachActionToButton(sbguibutton, makeButtonHandler);
								base.AttachActionToButton(sbguibutton, action2);
								flag = false;
							}
							else if (entity.CanVend)
							{
								VendingDecorator entity2 = pSimulated.GetEntity<VendingDecorator>();
								if (entity2 != null)
								{
									VendorDefinition vendorDefinition = this.session.TheGame.vendingManager.GetVendorDefinition(entity2.VendorId);
									if (vendorDefinition != null)
									{
										sbguibutton.SetActive(true);
										Action action3 = delegate()
										{
											this.session.TheSoundEffectManager.PlaySound("Accept");
											this.session.TheGame.selected = pSimulated;
											this.session.ChangeState("vending", true);
										};
										base.AttachActionToButton(sbguibutton, makeButtonHandler);
										base.AttachActionToButton(sbguibutton, action3);
										flag = false;
									}
								}
							}
							if (flag)
							{
								sbguibutton.SetActive(true);
								Action action4 = delegate()
								{
									this.session.TheSoundEffectManager.PlaySound("Accept");
									this.session.TheGame.selected = pSimulated;
									this.session.TheCamera.AutoPanToPosition(pSimulated.PositionCenter, 0.75f);
									this.session.ChangeState("SelectedPlaying", true);
								};
								base.AttachActionToButton(sbguibutton, makeButtonHandler);
								base.AttachActionToButton(sbguibutton, action4);
							}
						}
					}
				}
			}
			this.scrollSize += (sbguiimage.Size + new Vector2(0f, 0f)) * 0.01f;
			float y = sbguielement2.transform.localPosition.y;
			sbguielement2.SetParent(this.region.Marker);
			Vector3 localPosition = new Vector3(0f, y, 0f);
			sbguielement2.transform.localPosition = localPosition;
			sbguibutton.UpdateCollider();
			collectButton.UpdateCollider();
			k++;
		}
		if (this.numChunksLeft <= 0)
		{
			this.okayButton.SetActive(false);
			this.allDoneButton.SetActive(true);
		}
		else
		{
			this.okayButton.SetActive(true);
			this.allDoneButton.SetActive(false);
		}
		Rect rect = new Rect(0f, 0f, this.scrollSize.x, this.scrollSize.y);
		this.region.ResetScroll(rect);
		this.region.ResetToMinScroll();
		if (this.region.scrollBar.IsActive())
		{
			this.region.scrollBar.Reset();
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x000149A0 File Offset: 0x00012BA0
	public void SetupDialogInfo(string sDialogHeading, string sDialogBody, string sPortrait, List<Reward> pRewards, List<ConditionDescription> steps, QuestDefinition pQuestDef)
	{
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("dialog_heading");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("dialog_body");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("portrait");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("portrait_shadow");
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)base.FindChild("dialog_body_boundary");
		SBGUIShadowedLabel sbguishadowedLabel = (SBGUIShadowedLabel)base.FindChild("reward_label");
		SBGUILabel sbguilabel3 = (SBGUILabel)base.FindChild("reward_gold_label");
		SBGUILabel sbguilabel4 = (SBGUILabel)base.FindChild("reward_xp_label");
		this.stepsMarker = base.FindChild("steps_marker");
		this.window = (SBGUIAtlasImage)base.FindChild("window");
		this.okayButton = (SBGUIPulseButton)base.FindChild("okay");
		this.allDoneButton = (SBGUIPulseButton)base.FindChild("done");
		int num = 0;
		int num2 = 0;
		int count = pRewards.Count;
		for (int i = 0; i < count; i++)
		{
			Reward reward = pRewards[i];
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				num += reward.ResourceAmounts[ResourceManager.SOFT_CURRENCY];
			}
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.XP))
			{
				num2 += reward.ResourceAmounts[ResourceManager.XP];
			}
		}
		sbguilabel3.SetText(num.ToString());
		sbguilabel4.SetText(num2.ToString());
		this.numChunksLeft = steps.Count - 1;
		for (int j = 0; j < steps.Count - 1; j++)
		{
			if (steps[j].IsPassed)
			{
				this.numChunksLeft--;
			}
		}
		if (this.numChunksLeft <= 0)
		{
			this.okayButton.SetActive(false);
			this.allDoneButton.SetActive(true);
		}
		else
		{
			this.okayButton.SetActive(true);
			this.allDoneButton.SetActive(false);
		}
		string text = Language.Get(sDialogBody);
		if (text.Contains("{0}") && pQuestDef.AutoQuestCharacterID >= 0)
		{
			Simulated simulated = this.session.TheGame.simulation.FindSimulated(new int?(pQuestDef.AutoQuestCharacterID));
			if (simulated != null && simulated.HasEntity<ResidentEntity>())
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				text = string.Format(text, Language.Get(entity.Name));
			}
		}
		sbguilabel.SetText(Language.Get(sDialogHeading));
		sbguilabel2.SetText(text);
		sbguilabel2.AdjustText(boundary);
		sbguishadowedLabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		sbguiatlasImage.SetTextureFromAtlas(sPortrait, true, false, false, false, false, 0);
		sbguiatlasImage2.renderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
	}

	// Token: 0x0400027B RID: 635
	public const int STEP_GAP = 0;

	// Token: 0x0400027C RID: 636
	private int? questIconSize;

	// Token: 0x0400027D RID: 637
	private float markerXOffset;

	// Token: 0x0400027E RID: 638
	private Vector2 scrollSize;

	// Token: 0x0400027F RID: 639
	private SBGUIPulseButton okayButton;

	// Token: 0x04000280 RID: 640
	private SBGUIPulseButton allDoneButton;

	// Token: 0x04000281 RID: 641
	private SBGUIAtlasImage window;

	// Token: 0x04000282 RID: 642
	private SBGUIElement stepsMarker;

	// Token: 0x04000283 RID: 643
	private int numChunksLeft = -1;
}
