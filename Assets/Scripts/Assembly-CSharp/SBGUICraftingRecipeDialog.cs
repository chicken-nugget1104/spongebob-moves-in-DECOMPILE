using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000070 RID: 112
public class SBGUICraftingRecipeDialog : SBGUIElement
{
	// Token: 0x06000441 RID: 1089 RVA: 0x0001A9C4 File Offset: 0x00018BC4
	public void Init()
	{
		this.nameLabel = (SBGUILabel)base.FindChild("name_label");
		this.cookTimeLabel = (SBGUILabel)base.FindChild("time_label");
		this.cookTimeIcon = (SBGUIAtlasImage)base.FindChild("timer_icon");
		this.topSecretTreatment = (SBGUIAtlasImage)base.FindChild("top_secret_treatment");
		this.topSecretTreatment.SetVisible(false);
		this.rewardMarker = base.FindChild("reward_marker");
		this.ingredientAreaImage = (SBGUIAtlasImage)base.FindChild("ingredient_area");
		this.ingredientAreaDimensions = new Vector2(this.ingredientAreaImage.Size.x, this.ingredientAreaImage.Size.y);
		YGAtlasSprite component = this.craftingIngredientPrefab.GetComponent<YGAtlasSprite>();
		this.ingredientWidgetWidth = component.size.x;
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0001AAAC File Offset: 0x00018CAC
	private void CreateCraftingIngredient(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
		SBGUICraftingIngredient sbguicraftingIngredient;
		if (this.emptyIngredientPool.Count > 0)
		{
			sbguicraftingIngredient = this.emptyIngredientPool.Pop();
			sbguicraftingIngredient.MuteButtons(false);
			sbguicraftingIngredient.Setup(resMgr, parent, resourceId, price, offset);
			sbguicraftingIngredient.SetActive(true);
		}
		else
		{
			sbguicraftingIngredient = SBGUICraftingIngredient.Create(resMgr, parent, resourceId, price, offset);
		}
		this.activeIngredientPool.Push(sbguicraftingIngredient);
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0001AB10 File Offset: 0x00018D10
	public void Setup(CraftingRecipe recipe, ResourceManager resourceManager)
	{
		TFUtils.Assert(this.craftingIngredientPrefab != null, "The Crafting Ingredient Prefab has not been set in the CraftingRecipeDialog prefab.");
		this.nameLabel.SetText(Language.Get(recipe.recipeName));
		this.cookTimeLabel.SetText(TFUtils.DurationToString(recipe.craftTime));
		this.cookTimeIcon.SetActive(true);
		this.ResetIngredientArea();
		int count = recipe.cost.ResourceAmounts.Count;
		float num = 0.55f;
		if (resourceManager.Resources[recipe.productId].Reward != null)
		{
			SBGUIRewardWidget.SetupRewardWidget(resourceManager, resourceManager.Resources[recipe.productId].Reward.Summary, Language.Get("!!PREFAB_EARNS"), 1, this.rewardMarker, 10f, false, SBGUICraftingRecipeDialog.rewardColor, false, 0.85f);
		}
		switch (count)
		{
		case 1:
		{
			float num2 = (this.ingredientAreaDimensions.x - (float)this.cookTimeIcon.Width / 2f) / 2f * 0.01f;
			float num3 = -this.ingredientAreaDimensions.y / 2f * 0.01f;
			if (recipe.productId == 90)
			{
				this.topSecretTreatment.SetVisible(true);
				num3 -= 0.5f;
			}
			foreach (KeyValuePair<int, int> keyValuePair in recipe.cost.ResourceAmounts)
			{
				this.CreateCraftingIngredient(resourceManager, this.ingredientAreaImage, keyValuePair.Key, keyValuePair.Value, new Vector3(num2, num3, -0.1f));
			}
			break;
		}
		case 2:
		{
			float num2 = num;
			float num3 = -this.ingredientAreaDimensions.y / 2f * 0.01f;
			foreach (KeyValuePair<int, int> keyValuePair2 in recipe.cost.ResourceAmounts)
			{
				this.CreateCraftingIngredient(resourceManager, this.ingredientAreaImage, keyValuePair2.Key, keyValuePair2.Value, new Vector3(num2, num3, -0.1f));
				num2 += (this.ingredientWidgetWidth + 2f) * 0.01f;
			}
			break;
		}
		case 3:
		case 4:
		{
			int num4 = 1;
			int num5 = 0;
			float num2 = num;
			foreach (KeyValuePair<int, int> keyValuePair3 in recipe.cost.ResourceAmounts)
			{
				float num3 = -(this.ingredientAreaDimensions.y - (float)this.cookTimeIcon.Height) / 2f * 0.01f * (float)num4;
				this.CreateCraftingIngredient(resourceManager, this.ingredientAreaImage, keyValuePair3.Key, keyValuePair3.Value, new Vector3(num2, num3, -0.1f));
				num2 += (this.ingredientWidgetWidth + 2f) * 0.01f;
				num5++;
				if (num5 % 2 == 0)
				{
					num4++;
					num2 = num;
				}
			}
			break;
		}
		case 5:
		case 6:
		{
			int num4 = 1;
			float num2 = num;
			int num5 = 0;
			foreach (KeyValuePair<int, int> keyValuePair4 in recipe.cost.ResourceAmounts)
			{
				float num3 = -(this.ingredientAreaDimensions.y - (float)this.cookTimeIcon.Height / 2f) / 3f * 0.01f * (float)num4;
				this.CreateCraftingIngredient(resourceManager, this.ingredientAreaImage, keyValuePair4.Key, keyValuePair4.Value, new Vector3(num2, num3, -0.1f));
				num2 += (this.ingredientWidgetWidth + 2f) * 0.01f;
				num5++;
				if (num5 % 2 == 0)
				{
					num4++;
					num2 = num;
				}
			}
			break;
		}
		default:
			TFUtils.Assert(false, "SBGUICraftingRecipeDialog doesn't support recipes with over 6 ingredients.");
			break;
		}
		this.SetActive(true);
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0001AF9C File Offset: 0x0001919C
	private void ResetIngredientArea()
	{
		this.topSecretTreatment.SetVisible(false);
		SBGUIRewardWidget[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIRewardWidget>(true);
		foreach (SBGUIRewardWidget sbguirewardWidget in componentsInChildren)
		{
			sbguirewardWidget.SetParent(null);
			sbguirewardWidget.gameObject.SetActiveRecursively(false);
			SBGUIRewardWidget.ReleaseRewardWidget(sbguirewardWidget);
		}
		while (this.activeIngredientPool.Count > 0)
		{
			SBGUICraftingIngredient sbguicraftingIngredient = this.activeIngredientPool.Pop();
			sbguicraftingIngredient.SetActive(false);
			sbguicraftingIngredient.SetParent(null);
			this.emptyIngredientPool.Push(sbguicraftingIngredient);
		}
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0001B038 File Offset: 0x00019238
	public void Deactivate()
	{
		this.ResetIngredientArea();
		this.SetActive(false);
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0001B048 File Offset: 0x00019248
	public void Deselect()
	{
		this.ResetIngredientArea();
		this.nameLabel.SetText(string.Empty);
		this.cookTimeLabel.SetText(string.Empty);
		this.cookTimeIcon.SetActive(false);
	}

	// Token: 0x04000347 RID: 839
	private const int MAX_REWARDS = 1;

	// Token: 0x04000348 RID: 840
	private const int REWARD_GAP_SIZE = 10;

	// Token: 0x04000349 RID: 841
	private static readonly Color rewardColor = new Color(0.384f, 0.133f, 0.09f, 0.5f);

	// Token: 0x0400034A RID: 842
	public GameObject craftingIngredientPrefab;

	// Token: 0x0400034B RID: 843
	private SBGUILabel nameLabel;

	// Token: 0x0400034C RID: 844
	private SBGUILabel cookTimeLabel;

	// Token: 0x0400034D RID: 845
	private SBGUIAtlasImage cookTimeIcon;

	// Token: 0x0400034E RID: 846
	private SBGUIAtlasImage topSecretTreatment;

	// Token: 0x0400034F RID: 847
	protected SBGUIElement rewardMarker;

	// Token: 0x04000350 RID: 848
	private SBGUIAtlasImage ingredientAreaImage;

	// Token: 0x04000351 RID: 849
	private float ingredientWidgetWidth;

	// Token: 0x04000352 RID: 850
	private Vector2 ingredientAreaDimensions;

	// Token: 0x04000353 RID: 851
	private Stack<SBGUICraftingIngredient> emptyIngredientPool = new Stack<SBGUICraftingIngredient>();

	// Token: 0x04000354 RID: 852
	private Stack<SBGUICraftingIngredient> activeIngredientPool = new Stack<SBGUICraftingIngredient>();
}
