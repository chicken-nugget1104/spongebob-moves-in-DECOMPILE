using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class SBGUIRewardWidget : SBGUIAtlasImage
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x00024B0C File Offset: 0x00022D0C
	public override int Width
	{
		get
		{
			int num = this.prefixLabel.Width + this.label.Width;
			return Mathf.CeilToInt(base.sprite.size.x + 5f + (float)num);
		}
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00024B50 File Offset: 0x00022D50
	private static SBGUIRewardWidget Alloc()
	{
		SBGUIRewardWidget sbguirewardWidget = (SBGUIRewardWidget)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/RewardWidget");
		sbguirewardWidget.name = "RewardWidget_" + SBGUIRewardWidget.sNumAllocations++;
		sbguirewardWidget.gameObject.SetActiveRecursively(false);
		sbguirewardWidget.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sbguirewardWidget;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x00024BB8 File Offset: 0x00022DB8
	public static void MakeRewardWidgetPool()
	{
		SBGUIRewardWidget.widgetsPool = TFPool<SBGUIRewardWidget>.CreatePool(20, new Alloc<SBGUIRewardWidget>(SBGUIRewardWidget.Alloc));
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00024BD4 File Offset: 0x00022DD4
	protected override void Awake()
	{
		this.label = (SBGUILabel)base.FindChild("reward_label");
		this.prefixLabel = (SBGUILabel)base.FindChild("reward_prefix_label");
		base.Awake();
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00024C14 File Offset: 0x00022E14
	public void DetailedSetup(GameObject prefab, SBGUIElement parent, float xOffset, string texture, int amount, string prefix)
	{
		base.name = string.Format("Reward_{0}_{1}", xOffset, amount);
		this.SetParent(parent);
		base.tform.localPosition = new Vector3(xOffset, 0f, 0f);
		base.SetTextureFromAtlas(texture);
		this.SetPrefixText(prefix, false);
		this.SetText(amount.ToString(), false);
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00024C80 File Offset: 0x00022E80
	public static SBGUIRewardWidget Create(GameObject prefab, SBGUIElement parent, float xOffset, string texture, int amount, string prefix)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
		SBGUIRewardWidget component = gameObject.GetComponent<SBGUIRewardWidget>();
		component.DetailedSetup(prefab, parent, xOffset, texture, amount, prefix);
		return component;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00024CB0 File Offset: 0x00022EB0
	public void BriefSetup(SBGUIElement parent, float xOffset)
	{
		base.name = string.Format("Reward", new object[0]);
		this.SetParent(parent);
		base.tform.localPosition = new Vector3(xOffset, 0f, 0f);
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00024CF8 File Offset: 0x00022EF8
	public static SBGUIRewardWidget Create(SBGUIElement parent, float xOffset)
	{
		SBGUIRewardWidget sbguirewardWidget = (SBGUIRewardWidget)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/RewardWidget");
		sbguirewardWidget.BriefSetup(parent, xOffset);
		return sbguirewardWidget;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00024D20 File Offset: 0x00022F20
	public void SetText(string text, bool dim = false)
	{
		this.label.SetText(text);
		if (dim)
		{
			this.label.SetAlpha(0.2f);
		}
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00024D48 File Offset: 0x00022F48
	public void SetPrefixText(string text, bool dim = false)
	{
		this.prefixLabel.SetText(text);
		if (dim)
		{
			this.prefixLabel.SetAlpha(0.2f);
		}
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00024D70 File Offset: 0x00022F70
	public void SetTextScale(float scale)
	{
		this.prefixLabel.textSprite.scale = new Vector2(scale, scale);
		this.label.textSprite.scale = new Vector2(scale, scale);
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00024DAC File Offset: 0x00022FAC
	public void SetTextColor(Color color)
	{
		color.a = 0.5f;
		this.label.SetColor(color);
		this.prefixLabel.SetColor(color);
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x00024DE0 File Offset: 0x00022FE0
	public void CreateTextStroke(Color color)
	{
		float num = 0.02f;
		List<SBGUILabel> list = new List<SBGUILabel>();
		List<Vector3> list2 = new List<Vector3>();
		list2.Add(new Vector3(num, 0f, 0.01f));
		list2.Add(new Vector3(-num, 0f, 0.01f));
		list2.Add(new Vector3(0f, num, 0.01f));
		list2.Add(new Vector3(0f, -num, 0.01f));
		for (int i = 0; i < 4; i++)
		{
			SBGUILabel sbguilabel = (SBGUILabel)UnityEngine.Object.Instantiate(this.label, this.label.tform.position, this.label.tform.rotation);
			sbguilabel.name = "reward_label_outline" + i;
			list.Add(sbguilabel);
		}
		int num2 = 0;
		foreach (SBGUILabel sbguilabel2 in list)
		{
			sbguilabel2.SetParent(this.label);
			sbguilabel2.SetColor(color);
			sbguilabel2.tform.localPosition = list2[num2];
			num2++;
		}
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x00024F40 File Offset: 0x00023140
	public static void SetupRewardWidget(ResourceManager resMgr, Reward reward, string prefix, int maxCount, SBGUIElement marker, float rewardGapSize, bool dim, Color textColor, bool useCache = false, float scale = 1f)
	{
		float num = 0f;
		int num2 = 0;
		if (reward.ResourceAmounts != null)
		{
			foreach (KeyValuePair<int, int> keyValuePair in reward.ResourceAmounts)
			{
				string resourceTexture = resMgr.Resources[keyValuePair.Key].GetResourceTexture();
				SBGUIRewardWidget.AddRewardWidget(resourceTexture, keyValuePair.Value.ToString(), prefix, marker, ref num, ref num2, rewardGapSize, textColor, dim, useCache, scale);
				if (num2 >= maxCount)
				{
					return;
				}
			}
		}
		if (reward.BuildingAmounts != null)
		{
			foreach (KeyValuePair<int, int> keyValuePair2 in reward.BuildingAmounts)
			{
				Blueprint blueprint = EntityManager.GetBlueprint("building", keyValuePair2.Key, false);
				string texture = (string)blueprint.Invariable["portrait"];
				SBGUIRewardWidget.AddRewardWidget(texture, keyValuePair2.Value.ToString(), prefix, marker, ref num, ref num2, rewardGapSize, textColor, dim, useCache, scale);
				if (num2 >= maxCount)
				{
					break;
				}
			}
		}
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x000250BC File Offset: 0x000232BC
	private static void ResetWidget(SBGUIRewardWidget rewardWidget)
	{
		rewardWidget.muted = false;
		rewardWidget.SetParent(null);
		rewardWidget.SetActive(false);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x000250D4 File Offset: 0x000232D4
	public static void ClearWidgetPool()
	{
		SBGUIRewardWidget.widgetsPool.Clear(new Deactivate<SBGUIRewardWidget>(SBGUIRewardWidget.ResetWidget));
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x000250EC File Offset: 0x000232EC
	public static void ReleaseRewardWidget(SBGUIRewardWidget widget)
	{
		widget.muted = false;
		SBGUIRewardWidget.widgetsPool.Release(widget);
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x00025104 File Offset: 0x00023304
	private static void AddRewardWidget(string texture, string text, string prefix, SBGUIElement marker, ref float markerXOffset, ref int rewardCount, float rewardGapSize, Color textColor, bool dim, bool useCache, float scale)
	{
		SBGUIRewardWidget sbguirewardWidget;
		if (useCache)
		{
			sbguirewardWidget = SBGUIRewardWidget.widgetsPool.Create(new Alloc<SBGUIRewardWidget>(SBGUIRewardWidget.Alloc));
			sbguirewardWidget.gameObject.SetActiveRecursively(true);
			sbguirewardWidget.BriefSetup(marker, markerXOffset);
		}
		else
		{
			sbguirewardWidget = SBGUIRewardWidget.Create(marker, markerXOffset);
		}
		sbguirewardWidget.SetTextureFromAtlas(texture);
		sbguirewardWidget.SetTextScale(scale);
		sbguirewardWidget.SetTextColor(textColor);
		sbguirewardWidget.SetPrefixText(prefix, dim);
		sbguirewardWidget.SetText(text, dim);
		if (dim)
		{
			sbguirewardWidget.renderer.material.color = new Color(1f, 1f, 1f, 0.2f);
		}
		markerXOffset += ((float)sbguirewardWidget.Width + rewardGapSize) * 0.01f;
		rewardCount++;
	}

	// Token: 0x04000465 RID: 1125
	private SBGUILabel prefixLabel;

	// Token: 0x04000466 RID: 1126
	private SBGUILabel label;

	// Token: 0x04000467 RID: 1127
	private static int sNumAllocations;

	// Token: 0x04000468 RID: 1128
	protected static TFPool<SBGUIRewardWidget> widgetsPool;
}
