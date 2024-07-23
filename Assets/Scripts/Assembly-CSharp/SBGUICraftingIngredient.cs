using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class SBGUICraftingIngredient : SBGUIElement
{
	// Token: 0x0600043C RID: 1084 RVA: 0x0001A580 File Offset: 0x00018780
	public static SBGUICraftingIngredient Create(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
		SBGUICraftingIngredient sbguicraftingIngredient = (SBGUICraftingIngredient)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftingIngredient");
		sbguicraftingIngredient.resourceIcon = (SBGUIAtlasImage)sbguicraftingIngredient.FindChild("icon");
		sbguicraftingIngredient.resourceCost = (SBGUILabel)sbguicraftingIngredient.FindChild("cost_label");
		sbguicraftingIngredient.resourceOwned = (SBGUILabel)sbguicraftingIngredient.FindChild("owned_label");
		sbguicraftingIngredient.startingUIPosition = sbguicraftingIngredient.resourceIcon.tform.localPosition;
		sbguicraftingIngredient.Setup(resMgr, parent, resourceId, price, offset);
		return sbguicraftingIngredient;
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x0001A604 File Offset: 0x00018804
	public void Setup(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
		this.SetParent(parent);
		base.transform.localPosition = offset;
		this.cost = price;
		int num = resMgr.Query(resourceId);
		this.resourceManager = resMgr;
		this.resourceId = resourceId;
		this.resourceIcon.SetTextureFromAtlas(resMgr.Resources[resourceId].GetResourceTexture());
		if (resourceId == ResourceManager.SOFT_CURRENCY || resourceId == ResourceManager.HARD_CURRENCY)
		{
			this.update = false;
			this.resourceOwned.SetText(Language.Get("!!PREFAB_COSTS"));
			this.resourceOwned.SetColor(new Color(0.384f, 0.133f, 0.09f, 0.5f));
			this.resourceCost.SetText(this.cost.ToString());
			this.resourceOwned.tform.localPosition = this.startingUIPosition;
			Vector3 localPosition = this.resourceOwned.tform.localPosition;
			localPosition.x += (float)(this.resourceOwned.Width + 4) * 0.01f;
			this.resourceCost.tform.localPosition = localPosition;
			localPosition = this.resourceCost.tform.localPosition;
			localPosition.x += (float)(this.resourceCost.Width + 2) * 0.01f;
			this.resourceIcon.tform.localPosition = localPosition;
		}
		else
		{
			this.update = true;
			if (num > this.cost)
			{
				num = this.cost;
			}
			this.resourceOwned.SetText(num.ToString());
			this.resourceCost.SetText("/" + this.cost.ToString());
			this.resourceIcon.tform.localPosition = this.startingUIPosition;
			Vector3 localPosition2 = this.resourceIcon.tform.localPosition;
			localPosition2.x += (float)(this.resourceIcon.Width + 2) * 0.01f;
			this.resourceOwned.tform.localPosition = localPosition2;
			localPosition2 = this.resourceOwned.tform.localPosition;
			localPosition2.x += (float)(this.resourceOwned.Width + 2) * 0.01f;
			this.resourceCost.tform.localPosition = localPosition2;
			if (num < this.cost)
			{
				this.resourceOwned.SetColor(SBGUICraftingIngredient.insufficientColor);
			}
			else
			{
				this.resourceOwned.SetColor(SBGUICraftingIngredient.sufficientColor);
			}
		}
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x0001A894 File Offset: 0x00018A94
	public void Update()
	{
		if (this != null && this.update && this.resourceOwned != null && this.resourceManager != null)
		{
			int num = this.resourceManager.Query(this.resourceId);
			if (num > this.cost)
			{
				num = this.cost;
			}
			Vector3 localPosition = this.resourceOwned.tform.localPosition;
			localPosition.x += (float)(this.resourceOwned.Width + 2) * 0.01f;
			this.resourceCost.tform.localPosition = localPosition;
			this.resourceOwned.SetText(num.ToString());
			if (num < this.cost)
			{
				this.resourceOwned.SetColor(SBGUICraftingIngredient.insufficientColor);
			}
			else
			{
				this.resourceOwned.SetColor(SBGUICraftingIngredient.sufficientColor);
			}
		}
	}

	// Token: 0x0400033B RID: 827
	public const int GAP_SIZE = 2;

	// Token: 0x0400033C RID: 828
	public const int TEXT_SPACING = 2;

	// Token: 0x0400033D RID: 829
	public Vector3 startingUIPosition;

	// Token: 0x0400033E RID: 830
	private SBGUIAtlasImage resourceIcon;

	// Token: 0x0400033F RID: 831
	private SBGUILabel resourceCost;

	// Token: 0x04000340 RID: 832
	private SBGUILabel resourceOwned;

	// Token: 0x04000341 RID: 833
	private int cost;

	// Token: 0x04000342 RID: 834
	private int resourceId;

	// Token: 0x04000343 RID: 835
	private ResourceManager resourceManager;

	// Token: 0x04000344 RID: 836
	private bool update;

	// Token: 0x04000345 RID: 837
	private static readonly Color sufficientColor = new Color(0.384f, 0.133f, 0.09f, 0.5f);

	// Token: 0x04000346 RID: 838
	private static readonly Color insufficientColor = new Color(1f, 0.486f, 0.412f, 0.5f);
}
