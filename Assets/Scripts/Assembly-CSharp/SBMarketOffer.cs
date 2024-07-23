using System;
using System.Collections.Generic;

// Token: 0x0200008B RID: 139
public class SBMarketOffer
{
	// Token: 0x06000549 RID: 1353 RVA: 0x000219F0 File Offset: 0x0001FBF0
	public SBMarketOffer(Dictionary<string, object> offer)
	{
		this.identity = Convert.ToInt32(offer["identity"]);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)offer["cost"];
		this.cost = new Dictionary<int, int>();
		foreach (KeyValuePair<string, object> keyValuePair in dictionary)
		{
			this.cost[Convert.ToInt32(keyValuePair.Key)] = Convert.ToInt32(keyValuePair.Value);
		}
		if (offer.ContainsKey("type"))
		{
			this.type = TFUtils.LoadString(offer, "type");
		}
		if (offer.ContainsKey("name"))
		{
			this.itemName = Language.Get(TFUtils.LoadString(offer, "name"));
		}
		if (offer.ContainsKey("description"))
		{
			this.description = Language.Get(TFUtils.LoadString(offer, "description"));
		}
		if (offer.ContainsKey("code"))
		{
			this.innerOffer = TFUtils.LoadString(offer, "code");
		}
		if (offer.ContainsKey("result_type"))
		{
			this.resultType = TFUtils.LoadString(offer, "result_type");
		}
		if (offer.ContainsKey("data"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)offer["data"];
			this.data = new Dictionary<int, int>();
			foreach (KeyValuePair<string, object> keyValuePair2 in dictionary2)
			{
				this.data[Convert.ToInt32(keyValuePair2.Key)] = Convert.ToInt32(keyValuePair2.Value);
			}
		}
		if (offer.ContainsKey("button_texture"))
		{
			this.buttonTexture = TFUtils.LoadString(offer, "button_texture");
		}
		if (offer.ContainsKey("micro_event_did"))
		{
			this.microEventDID = TFUtils.LoadInt(offer, "micro_event_did");
		}
		else
		{
			this.microEventDID = -1;
		}
		if (offer.ContainsKey("event_only"))
		{
			this.microEventOnly = TFUtils.LoadBool(offer, "event_only");
		}
		else
		{
			this.microEventOnly = false;
		}
		if (offer.ContainsKey("sale_banner"))
		{
			this.isSaleItem = TFUtils.LoadBool(offer, "sale_banner");
		}
		else
		{
			this.isSaleItem = false;
		}
		if (offer.ContainsKey("sale_percent"))
		{
			this.salePercent = TFUtils.LoadFloat(offer, "sale_percent");
		}
		if (offer.ContainsKey("new_banner"))
		{
			this.isNewItem = TFUtils.LoadBool(offer, "new_banner");
		}
		else
		{
			this.isNewItem = false;
		}
		if (offer.ContainsKey("limited_banner"))
		{
			this.isLimitedItem = TFUtils.LoadBool(offer, "limited_banner");
		}
		else
		{
			this.isLimitedItem = false;
		}
		if (!offer.ContainsKey("display"))
		{
			return;
		}
		dictionary = (Dictionary<string, object>)offer["display"];
		this.width = TFUtils.LoadInt(dictionary, "width");
		this.height = TFUtils.LoadInt(dictionary, "height");
		dictionary = (Dictionary<string, object>)offer["display.default"];
		if (dictionary.ContainsKey("texture"))
		{
			this.texture = (string)dictionary["texture"];
		}
		if (dictionary.ContainsKey("material"))
		{
			this.material = (string)dictionary["material"];
		}
	}

	// Token: 0x04000400 RID: 1024
	public string type;

	// Token: 0x04000401 RID: 1025
	public int identity;

	// Token: 0x04000402 RID: 1026
	public bool itemLocked;

	// Token: 0x04000403 RID: 1027
	public string itemName;

	// Token: 0x04000404 RID: 1028
	public string description;

	// Token: 0x04000405 RID: 1029
	public string innerOffer;

	// Token: 0x04000406 RID: 1030
	public string material;

	// Token: 0x04000407 RID: 1031
	public string texture;

	// Token: 0x04000408 RID: 1032
	public string buttonTexture;

	// Token: 0x04000409 RID: 1033
	public Dictionary<int, int> cost;

	// Token: 0x0400040A RID: 1034
	public Dictionary<int, int> data;

	// Token: 0x0400040B RID: 1035
	public int width;

	// Token: 0x0400040C RID: 1036
	public int height;

	// Token: 0x0400040D RID: 1037
	public string resultType;

	// Token: 0x0400040E RID: 1038
	public int microEventDID;

	// Token: 0x0400040F RID: 1039
	public bool microEventOnly;

	// Token: 0x04000410 RID: 1040
	public bool isSaleItem;

	// Token: 0x04000411 RID: 1041
	public bool isNewItem;

	// Token: 0x04000412 RID: 1042
	public bool isLimitedItem;

	// Token: 0x04000413 RID: 1043
	public float salePercent;
}
