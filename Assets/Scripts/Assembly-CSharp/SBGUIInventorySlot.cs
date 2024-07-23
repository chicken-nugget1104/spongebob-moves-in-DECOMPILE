using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class SBGUIInventorySlot : SBGUIScrollListElement
{
	// Token: 0x06000507 RID: 1287 RVA: 0x0001FCBC File Offset: 0x0001DEBC
	public static string CalculateSlotName(SBInventoryItem invItem)
	{
		if (invItem.entity != null)
		{
			return string.Format("InventorySlot_{0}", invItem.entity.DefinitionId);
		}
		return string.Format("InventorySlot_{0}", invItem.displayName);
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0001FD00 File Offset: 0x0001DF00
	public static SBGUIInventorySlot MakeInventorySlot()
	{
		SBGUIInventorySlot sbguiinventorySlot = (SBGUIInventorySlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/InventorySlot");
		sbguiinventorySlot.name = "InventorySlot_" + SBGUIInventorySlot.slotCount++;
		sbguiinventorySlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sbguiinventorySlot;
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x0001FD60 File Offset: 0x0001DF60
	public void Setup(Session session, SBGUIElement anchor, SBInventoryItem invItem, EventDispatcher<SBInventoryItem> itemClickedEvent, Vector3 offset)
	{
		this.SetParent(anchor);
		base.tform.localPosition = offset;
		this.SetActive(true);
		base.name = SBGUIInventorySlot.CalculateSlotName(invItem);
		this.iconImage = (SBGUIAtlasImage)base.FindChild("icon");
		this.iconImage.SetSizeNoRebuild(new Vector2(150f, 150f));
		this.iconImage.SetTextureFromAtlas(invItem.iconFilename, true, false, true, false, false, 0);
		this.titleLabel = (SBGUILabel)base.FindChild("name_label");
		this.titleLabel.SetText(Language.Get(invItem.displayName));
		this.descriptionLabel = (SBGUILabel)base.FindChild("description_label");
		this.productionInfo = base.FindChild("makes_info");
		this.productionTimeLabel = (SBGUILabel)base.FindChild("makes_per_hour_label");
		this.rewardMarker = base.FindChild("makes_icon");
		this.ownedInfo = base.FindChild("owned_info");
		this.numberOwnedLabel = (SBGUILabel)base.FindChild("owned_num_label");
		this.numberOwnedLabel.SetActive(false);
		this.buttonLabel = (SBGUILabel)base.FindChild("place_label");
		base.ClearButtonActions("button");
		base.AttachActionToButton("button", delegate()
		{
			itemClickedEvent.FireEvent(invItem);
		});
		string itemType = invItem.itemType;
		if (itemType != null)
		{
			if (SBGUIInventorySlot.<>f__switch$map3 == null)
			{
				SBGUIInventorySlot.<>f__switch$map3 = new Dictionary<string, int>(1)
				{
					{
						"movie",
						0
					}
				};
			}
			int num;
			if (SBGUIInventorySlot.<>f__switch$map3.TryGetValue(itemType, out num))
			{
				if (num == 0)
				{
					this.RemoveProductionInfo();
					string description = invItem.description;
					if (description != null)
					{
						this.descriptionLabel.SetText(Language.Get(description));
					}
					else
					{
						this.descriptionLabel.SetText(string.Empty);
					}
					this.RemoveOwnedInfo();
					this.buttonLabel.SetText(Language.Get("!!PREFAB_PLAY_MOVIE"));
					goto IL_45D;
				}
			}
		}
		if (invItem.entity.Invariable.ContainsKey("product") && invItem.entity.Invariable["product"] != null)
		{
			this.RemoveDescriptionInfo();
			RewardDefinition rewardDefinition = (RewardDefinition)invItem.entity.Invariable["product"];
			SBGUIRewardWidget.SetupRewardWidget(session.TheGame.resourceManager, rewardDefinition.Summary, string.Empty, 2, this.rewardMarker, 10f, false, Color.white, false, 1f);
			ulong duration = (ulong)invItem.entity.Invariable["time.production"];
			this.productionTimeLabel.SetText(TFUtils.DurationToString(duration));
		}
		else
		{
			this.RemoveProductionInfo();
			string description = session.TheGame.catalog.GetDescription(invItem.entity.DefinitionId);
			if (description != null)
			{
				this.descriptionLabel.SetText(Language.Get(description));
			}
			else
			{
				this.descriptionLabel.SetText(string.Empty);
			}
		}
		int level = session.TheGame.resourceManager.Query(ResourceManager.LEVEL);
		Blueprint blueprint = EntityManager.GetBlueprint(invItem.entity.AllTypes, invItem.entity.DefinitionId, false);
		if (blueprint.GetInstanceLimitByLevel(level) != null)
		{
			this.numberOwnedLabel.SetText(string.Format("{0}/{1}", session.TheGame.entities.GetEntityCount(invItem.entity.AllTypes, invItem.entity.DefinitionId), blueprint.GetInstanceLimitByLevel(level)));
		}
		else
		{
			this.numberOwnedLabel.SetText(session.TheGame.entities.GetEntityCount(blueprint.PrimaryType, invItem.entity.DefinitionId).ToString());
		}
		IL_45D:
		this.SessionActionId = SBGUIInventorySlot.CalculateSlotName(invItem);
		base.View.RefreshEvent += base.ReregisterColliders;
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x000201F4 File Offset: 0x0001E3F4
	private void RemoveProductionInfo()
	{
		this.productionInfo.SetActive(false);
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00020204 File Offset: 0x0001E404
	private void RemoveOwnedInfo()
	{
		this.ownedInfo.SetActive(false);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00020214 File Offset: 0x0001E414
	private void RemoveDescriptionInfo()
	{
		this.descriptionLabel.SetActive(false);
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00020224 File Offset: 0x0001E424
	public override void Deactivate()
	{
		base.ClearButtonActions("button");
		SBGUIRewardWidget[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIRewardWidget>(true);
		foreach (SBGUIRewardWidget sbguirewardWidget in componentsInChildren)
		{
			sbguirewardWidget.SetParent(null);
			sbguirewardWidget.gameObject.SetActiveRecursively(false);
			SBGUIRewardWidget.ReleaseRewardWidget(sbguirewardWidget);
		}
		base.Deactivate();
	}

	// Token: 0x040003CA RID: 970
	public const int GAP_SIZE = 6;

	// Token: 0x040003CB RID: 971
	private const int MAX_SLOT_ICON_SIZE = 150;

	// Token: 0x040003CC RID: 972
	private const int MAX_REWARDS = 2;

	// Token: 0x040003CD RID: 973
	private const int REWARD_GAP_SIZE = 10;

	// Token: 0x040003CE RID: 974
	public bool needsToBeDeleted;

	// Token: 0x040003CF RID: 975
	private SBGUIAtlasImage iconImage;

	// Token: 0x040003D0 RID: 976
	private SBGUILabel titleLabel;

	// Token: 0x040003D1 RID: 977
	private SBGUILabel descriptionLabel;

	// Token: 0x040003D2 RID: 978
	private SBGUIElement productionInfo;

	// Token: 0x040003D3 RID: 979
	private SBGUILabel productionTimeLabel;

	// Token: 0x040003D4 RID: 980
	private SBGUIElement rewardMarker;

	// Token: 0x040003D5 RID: 981
	private SBGUILabel buttonLabel;

	// Token: 0x040003D6 RID: 982
	private SBGUIElement ownedInfo;

	// Token: 0x040003D7 RID: 983
	protected SBGUILabel numberOwnedLabel;

	// Token: 0x040003D8 RID: 984
	private static int slotCount;
}
