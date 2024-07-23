using System;

// Token: 0x02000370 RID: 880
public class SoaringRetrievePurchasesModule : SoaringModule
{
	// Token: 0x0600190F RID: 6415 RVA: 0x000A5744 File Offset: 0x000A3944
	public override string ModuleName()
	{
		return "retrieveIapPurchases";
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x000A574C File Offset: 0x000A394C
	public override int ModuleChannel()
	{
		return 1;
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x000A5750 File Offset: 0x000A3950
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + this.ModuleName() + "\",\n";
		text = text + "\"authToken\":\"" + data.soaringValue("authToken") + "\"\n},";
		text += "\n\"data\" : ";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, this.ModuleChannel(), context, false);
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x000A57F8 File Offset: 0x000A39F8
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData data)
	{
		SoaringRetrievePurchasesModule.totalMoneySpent = 0;
		SoaringPurchase[] array = null;
		SoaringDictionary purchasables = SoaringInternal.instance.Purchasables;
		if (data.data != null)
		{
			SoaringArray soaringArray = (SoaringArray)data.data.objectWithKey("purchases");
			if (soaringArray != null)
			{
				int num = soaringArray.count();
				array = new SoaringPurchase[num];
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(i);
					string key = soaringDictionary.soaringValue("productId");
					SoaringPurchasable soaringPurchasable = (SoaringPurchasable)purchasables.objectWithKey(key);
					array[i] = new SoaringPurchase(soaringDictionary, soaringPurchasable);
					SoaringRetrievePurchasesModule.totalMoneySpent += soaringPurchasable.USDPrice;
				}
			}
		}
		Soaring.Delegate.OnRetrievePurchases(data.state, data.error, array, data.context);
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06001913 RID: 6419 RVA: 0x000A58D0 File Offset: 0x000A3AD0
	public static int TotalMoneySpent
	{
		get
		{
			return SoaringRetrievePurchasesModule.totalMoneySpent;
		}
	}

	// Token: 0x04001073 RID: 4211
	private static int totalMoneySpent;
}
