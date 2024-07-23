using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x0200010C RID: 268
public class Resource
{
	// Token: 0x06000977 RID: 2423 RVA: 0x0003B670 File Offset: 0x00039870
	public Resource(string name, string name_plural, string tag, float width, float height, int maxAmount, string texture, string collectedSound, string tapSound, string eatenSound, RewardDefinition reward, float jellyConversion, int fullnessTime, bool forceTapToCollect, bool forceWishMatch, bool ignoreWishDurationTimer, bool forceNoWishPayout, int did, int currencyDisplayQuestTrigger, bool consumable)
	{
		this.name = name;
		this.name_plural = name_plural;
		this.tag = tag;
		this.width = width;
		this.height = height;
		this.maxAmount = maxAmount;
		this.texture = texture;
		this.collectedSound = collectedSound;
		this.tapSound = tapSound;
		this.eatenSound = eatenSound;
		this.reward = reward;
		this.jellyConversion = jellyConversion;
		this.fullnessTime = fullnessTime;
		this.forceTapToCollect = forceTapToCollect;
		this.forceWishMatch = forceWishMatch;
		this.ignoreWishDurationTimer = ignoreWishDurationTimer;
		this.forceNoWishPayout = forceNoWishPayout;
		this.did = did;
		this.currencyDisplayQuestTrigger = currencyDisplayQuestTrigger;
		this.consumable = consumable;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0003B720 File Offset: 0x00039920
	public Resource(Resource other)
	{
		this.name = other.name;
		this.name_plural = other.name_plural;
		this.tag = other.tag;
		this.width = other.width;
		this.height = other.height;
		this.maxAmount = other.maxAmount;
		this.texture = other.texture;
		this.collectedSound = other.collectedSound;
		this.tapSound = other.tapSound;
		this.eatenSound = other.eatenSound;
		this.reward = other.reward;
		this.jellyConversion = other.jellyConversion;
		this.fullnessTime = other.fullnessTime;
		this.forceTapToCollect = other.forceTapToCollect;
		this.forceWishMatch = other.forceWishMatch;
		this.ignoreWishDurationTimer = other.ignoreWishDurationTimer;
		this.forceNoWishPayout = other.forceNoWishPayout;
		this.did = other.did;
		this.amountEarned = other.amountEarned;
		this.amountPurchased = other.amountPurchased;
		this.amountSpent = other.amountSpent;
		this.currencyDisplayQuestTrigger = other.currencyDisplayQuestTrigger;
		this.consumable = other.consumable;
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000979 RID: 2425 RVA: 0x0003B848 File Offset: 0x00039A48
	public bool Consumable
	{
		get
		{
			return this.consumable;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x0003B850 File Offset: 0x00039A50
	public RewardDefinition Reward
	{
		get
		{
			return this.reward;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x0003B858 File Offset: 0x00039A58
	public int Amount
	{
		get
		{
			return this.amountEarned + this.amountPurchased - this.amountSpent;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x0600097C RID: 2428 RVA: 0x0003B870 File Offset: 0x00039A70
	public int AmountPurchased
	{
		get
		{
			return this.amountPurchased;
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x0600097D RID: 2429 RVA: 0x0003B878 File Offset: 0x00039A78
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600097E RID: 2430 RVA: 0x0003B880 File Offset: 0x00039A80
	public string Name_Plural
	{
		get
		{
			return this.name_plural;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600097F RID: 2431 RVA: 0x0003B888 File Offset: 0x00039A88
	public string Tag
	{
		get
		{
			return this.tag;
		}
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0003B890 File Offset: 0x00039A90
	public string GetResourceTexture()
	{
		return this.texture;
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x0003B898 File Offset: 0x00039A98
	public int Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000982 RID: 2434 RVA: 0x0003B8A0 File Offset: 0x00039AA0
	public int CurrencyDisplayQuestTrigger
	{
		get
		{
			return this.currencyDisplayQuestTrigger;
		}
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0003B8A8 File Offset: 0x00039AA8
	public string GetResourceTexture(int amount)
	{
		string text = null;
		if (this.did == ResourceManager.SOFT_CURRENCY)
		{
			if (amount <= 7)
			{
				text = "IconMoney_1.png";
			}
			else if (amount > 7 && amount <= 20)
			{
				text = "IconMoney_2.png";
			}
			else if (amount > 20 && amount <= 54)
			{
				text = "IconMoney_3.png";
			}
			else if (amount > 54 && amount <= 143)
			{
				text = "IconMoney_4.png";
			}
			else if (amount > 143 && amount <= 376)
			{
				text = "IconMoney_5.png";
			}
			else if (amount > 376 && amount <= 986)
			{
				text = "IconMoney_6.png";
			}
			else if (amount > 986)
			{
				text = "IconMoney_6.png";
			}
		}
		else if (this.did == ResourceManager.XP)
		{
			if (amount <= 2)
			{
				text = "IconXP_1.png";
			}
			else if (amount > 2 && amount <= 7)
			{
				text = "IconXP_2.png";
			}
			else if (amount > 7 && amount <= 20)
			{
				text = "IconXP_3.png";
			}
			else if (amount > 20 && amount <= 54)
			{
				text = "IconXP_4.png";
			}
			else if (amount > 54 && amount <= 143)
			{
				text = "IconXP_5.png";
			}
			else if (amount > 143 && amount <= 376)
			{
				text = "IconXP_6.png";
			}
			else if (amount > 376 && amount <= 986)
			{
				text = "IconXP_6.png";
			}
			else if (amount > 986)
			{
				text = "IconXP_6.png";
			}
		}
		else if (this.did == ResourceManager.DEFAULT_JJ)
		{
			if (amount == 1)
			{
				text = "IconJellyfishJelly_1.png";
			}
			else if (amount == 25)
			{
				text = "IconJellyfishJelly_2.png";
			}
			else if (amount == 50)
			{
				text = "IconJellyfishJelly_2.png";
			}
		}
		if (text != null)
		{
			AtlasCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(text).atlasCoords;
			this.width = (float)TFAnimatedSprite.CalcWorldSize((double)atlasCoords.frame.width, 0.8);
			this.height = (float)TFAnimatedSprite.CalcWorldSize((double)atlasCoords.frame.height, 0.8);
		}
		return text;
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x0003BB04 File Offset: 0x00039D04
	public string CollectedSound
	{
		get
		{
			return this.collectedSound;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000985 RID: 2437 RVA: 0x0003BB0C File Offset: 0x00039D0C
	public string TapSound
	{
		get
		{
			return this.tapSound;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000986 RID: 2438 RVA: 0x0003BB14 File Offset: 0x00039D14
	public string EatenSound
	{
		get
		{
			return this.eatenSound;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000987 RID: 2439 RVA: 0x0003BB1C File Offset: 0x00039D1C
	public float Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000988 RID: 2440 RVA: 0x0003BB24 File Offset: 0x00039D24
	public float Height
	{
		get
		{
			return this.height;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x0003BB2C File Offset: 0x00039D2C
	public float HardCurrencyConversion
	{
		get
		{
			return this.jellyConversion;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x0600098A RID: 2442 RVA: 0x0003BB34 File Offset: 0x00039D34
	public int FullnessTime
	{
		get
		{
			return this.fullnessTime;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x0003BB3C File Offset: 0x00039D3C
	public bool ForceTapToCollect
	{
		get
		{
			return this.forceTapToCollect;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x0600098C RID: 2444 RVA: 0x0003BB44 File Offset: 0x00039D44
	public bool ForceWishMatch
	{
		get
		{
			return this.forceWishMatch;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x0003BB4C File Offset: 0x00039D4C
	public bool ForceNoWishPayout
	{
		get
		{
			return this.forceNoWishPayout;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600098E RID: 2446 RVA: 0x0003BB54 File Offset: 0x00039D54
	public bool IgnoreWishDurationTimer
	{
		get
		{
			return this.ignoreWishDurationTimer;
		}
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x0003BB5C File Offset: 0x00039D5C
	public void AddAmount(int amountToAdd)
	{
		this.SetAmountEarned(this.amountEarned + amountToAdd);
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0003BB6C File Offset: 0x00039D6C
	public void SubtractAmount(int amountToSubtract)
	{
		this.amountSpent += amountToSubtract;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0003BB7C File Offset: 0x00039D7C
	public void SetAmountEarned(int newAmount)
	{
		this.amountEarned = this.amountSpent + Mathf.Min(newAmount - this.amountSpent, this.maxAmount);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0003BBAC File Offset: 0x00039DAC
	public void SetAmounts(int amountEarned, int amountSpent)
	{
		this.amountSpent = amountSpent;
		this.SetAmountEarned(amountEarned);
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0003BBBC File Offset: 0x00039DBC
	public void SetAmountPurchased(int amountPurchased)
	{
		this.amountPurchased = amountPurchased;
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0003BBC8 File Offset: 0x00039DC8
	public static int Prorate(int amount, float percentLeft)
	{
		if (percentLeft < 0f)
		{
			percentLeft = 0f;
		}
		else if (percentLeft > 1f)
		{
			percentLeft = 1f;
		}
		return Mathf.CeilToInt(percentLeft * (float)amount);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0003BC08 File Offset: 0x00039E08
	public static void AddToTriggerData(ref Dictionary<string, object> data, int did)
	{
		Resource.AddToTriggerData(ref data, did, 1);
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0003BC14 File Offset: 0x00039E14
	public static void AddToTriggerData(ref Dictionary<string, object> data, int did, int amount)
	{
		if (!data.ContainsKey("resource_amounts"))
		{
			data["resource_amounts"] = new Dictionary<string, object>();
		}
		((Dictionary<string, object>)data["resource_amounts"])[did.ToString()] = amount;
	}

	// Token: 0x04000692 RID: 1682
	public const string RESOURCE_AMOUNTS = "resource_amounts";

	// Token: 0x04000693 RID: 1683
	private int did;

	// Token: 0x04000694 RID: 1684
	private int amountSpent;

	// Token: 0x04000695 RID: 1685
	private int amountEarned;

	// Token: 0x04000696 RID: 1686
	private int amountPurchased;

	// Token: 0x04000697 RID: 1687
	private int maxAmount;

	// Token: 0x04000698 RID: 1688
	private int currencyDisplayQuestTrigger;

	// Token: 0x04000699 RID: 1689
	private string name;

	// Token: 0x0400069A RID: 1690
	private string name_plural;

	// Token: 0x0400069B RID: 1691
	private string tag;

	// Token: 0x0400069C RID: 1692
	private string texture;

	// Token: 0x0400069D RID: 1693
	private string collectedSound;

	// Token: 0x0400069E RID: 1694
	private string tapSound;

	// Token: 0x0400069F RID: 1695
	private string eatenSound;

	// Token: 0x040006A0 RID: 1696
	private float width;

	// Token: 0x040006A1 RID: 1697
	private float height;

	// Token: 0x040006A2 RID: 1698
	private float jellyConversion;

	// Token: 0x040006A3 RID: 1699
	private RewardDefinition reward;

	// Token: 0x040006A4 RID: 1700
	private int fullnessTime;

	// Token: 0x040006A5 RID: 1701
	private bool forceTapToCollect;

	// Token: 0x040006A6 RID: 1702
	private bool forceWishMatch;

	// Token: 0x040006A7 RID: 1703
	private bool forceNoWishPayout;

	// Token: 0x040006A8 RID: 1704
	private bool ignoreWishDurationTimer;

	// Token: 0x040006A9 RID: 1705
	private bool consumable;
}
