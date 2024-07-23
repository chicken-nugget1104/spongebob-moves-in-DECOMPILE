using System;

// Token: 0x02000399 RID: 921
public class SoaringEvent : SoaringObjectBase
{
	// Token: 0x06001A3F RID: 6719 RVA: 0x000AB96C File Offset: 0x000A9B6C
	public SoaringEvent(SoaringDictionary ev) : base(SoaringObjectBase.IsType.Object)
	{
		this.Name = ev.soaringValue("event_name");
		this.AutoHandled = ev.soaringValue("auto");
		SoaringArray soaringArray = (SoaringArray)ev.objectWithKey("actions");
		SoaringArray soaringArray2 = (SoaringArray)ev.objectWithKey("client_requires");
		if (soaringArray2 != null)
		{
			this.Requires = new SoaringEvent.SoaringEventRequirements[soaringArray2.count()];
			for (int i = 0; i < soaringArray2.count(); i++)
			{
				SoaringEvent.SoaringEventRequirements soaringEventRequirements = new SoaringEvent.SoaringEventRequirements();
				SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray2.objectAtIndex(i);
				soaringEventRequirements.Key = soaringDictionary.soaringValue("key");
				soaringEventRequirements.Value = soaringDictionary.soaringValue("value");
				soaringEventRequirements.Custom = (SoaringDictionary)soaringDictionary.objectWithKey("custom");
				string text = soaringDictionary.soaringValue("test");
				if (text != null)
				{
					if (text == "equals")
					{
						soaringEventRequirements.Sign = SoaringEvent.Equivelency.equal;
					}
					else if (text == "greaterThenEquals")
					{
						soaringEventRequirements.Sign = SoaringEvent.Equivelency.greaterThenEquals;
					}
					else if (text == "lessThenEquals")
					{
						soaringEventRequirements.Sign = SoaringEvent.Equivelency.lessThenEquals;
					}
					else if (text == "greaterThen")
					{
						soaringEventRequirements.Sign = SoaringEvent.Equivelency.greaterThen;
					}
					else if (text == "lessThen")
					{
						soaringEventRequirements.Sign = SoaringEvent.Equivelency.lessThen;
					}
				}
				this.Requires[i] = soaringEventRequirements;
			}
		}
		int num = 0;
		if (soaringArray != null)
		{
			num = soaringArray.count();
		}
		this.Actions = new SoaringEvent.SoaringEventAction[num];
		for (int j = 0; j < num; j++)
		{
			SoaringEvent.SoaringEventAction soaringEventAction = new SoaringEvent.SoaringEventAction();
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringArray.objectAtIndex(j);
			soaringEventAction.Key = soaringDictionary2.soaringValue("key");
			soaringEventAction.Value = soaringDictionary2.soaringValue("value");
			soaringEventAction.Quantity = soaringDictionary2.soaringValue("quantity");
			SoaringObjectBase soaringObjectBase = soaringDictionary2.soaringValue("display");
			if (soaringObjectBase != null)
			{
				if (soaringObjectBase.Type == SoaringObjectBase.IsType.Dictionary)
				{
					SoaringDictionary soaringDictionary3 = (SoaringDictionary)soaringObjectBase;
					soaringEventAction.Display = soaringDictionary3.soaringValue("display");
					soaringEventAction.Priority = soaringDictionary3.soaringValue("priority");
					soaringEventAction.AutoHandle = soaringDictionary3.soaringValue("auto");
				}
				else
				{
					soaringEventAction.Display = (SoaringValue)soaringObjectBase;
				}
			}
			else
			{
				soaringEventAction.Display = false;
			}
			soaringEventAction.Custom = (SoaringDictionary)soaringDictionary2.objectWithKey("custom");
			soaringEventAction.Type = SoaringEvent.SoaringEventActionType.Custom;
			string a = soaringEventAction.Key.ToLower();
			if (a == "display_banner")
			{
				soaringEventAction.Type = SoaringEvent.SoaringEventActionType.DisplayBanner;
			}
			else if (a == "hard_currency")
			{
				soaringEventAction.Type = SoaringEvent.SoaringEventActionType.HardCurrency;
			}
			else if (a == "soft_currency")
			{
				soaringEventAction.Type = SoaringEvent.SoaringEventActionType.SoftCurrency;
			}
			else if (a == "item")
			{
				soaringEventAction.Type = SoaringEvent.SoaringEventActionType.Item;
			}
			this.Actions[j] = soaringEventAction;
		}
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x000ABCDC File Offset: 0x000A9EDC
	public bool HasDisplayBannerEvent()
	{
		SoaringEvent.SoaringEventAction soaringEventAction = null;
		return this.HasDisplayBannerEvent(ref soaringEventAction);
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x000ABCF4 File Offset: 0x000A9EF4
	public bool HasDisplayBannerEvent(ref SoaringEvent.SoaringEventAction action)
	{
		bool result = false;
		if (this.Actions == null)
		{
			return result;
		}
		int num = this.Actions.Length;
		for (int i = 0; i < num; i++)
		{
			if (this.Actions[i].Type == SoaringEvent.SoaringEventActionType.DisplayBanner && this.Actions[i].Display)
			{
				action = this.Actions[i];
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x0400110A RID: 4362
	public string Name;

	// Token: 0x0400110B RID: 4363
	public SoaringEvent.SoaringEventAction[] Actions;

	// Token: 0x0400110C RID: 4364
	public SoaringEvent.SoaringEventRequirements[] Requires;

	// Token: 0x0400110D RID: 4365
	public bool AutoHandled;

	// Token: 0x0200039A RID: 922
	public enum SoaringEventActionType
	{
		// Token: 0x0400110F RID: 4367
		Custom,
		// Token: 0x04001110 RID: 4368
		DisplayBanner,
		// Token: 0x04001111 RID: 4369
		HardCurrency,
		// Token: 0x04001112 RID: 4370
		SoftCurrency,
		// Token: 0x04001113 RID: 4371
		Item
	}

	// Token: 0x0200039B RID: 923
	public class SoaringEventAction
	{
		// Token: 0x04001114 RID: 4372
		public string Key;

		// Token: 0x04001115 RID: 4373
		public string Value;

		// Token: 0x04001116 RID: 4374
		public int Quantity;

		// Token: 0x04001117 RID: 4375
		public bool Display;

		// Token: 0x04001118 RID: 4376
		public bool AutoHandle;

		// Token: 0x04001119 RID: 4377
		public int Priority;

		// Token: 0x0400111A RID: 4378
		public SoaringDictionary Custom;

		// Token: 0x0400111B RID: 4379
		public SoaringEvent.SoaringEventActionType Type;
	}

	// Token: 0x0200039C RID: 924
	public enum Equivelency
	{
		// Token: 0x0400111D RID: 4381
		equal,
		// Token: 0x0400111E RID: 4382
		greaterThen,
		// Token: 0x0400111F RID: 4383
		greaterThenEquals,
		// Token: 0x04001120 RID: 4384
		lessThen,
		// Token: 0x04001121 RID: 4385
		lessThenEquals
	}

	// Token: 0x0200039D RID: 925
	public class SoaringEventRequirements
	{
		// Token: 0x04001122 RID: 4386
		public string Key;

		// Token: 0x04001123 RID: 4387
		public string Value;

		// Token: 0x04001124 RID: 4388
		public SoaringEvent.Equivelency Sign;

		// Token: 0x04001125 RID: 4389
		public SoaringDictionary Custom;
	}
}
