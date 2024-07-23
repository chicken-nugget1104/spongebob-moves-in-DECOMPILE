using System;

// Token: 0x02000359 RID: 857
public class SoaringCheckRewardModule : SoaringModule
{
	// Token: 0x06001892 RID: 6290 RVA: 0x000A28FC File Offset: 0x000A0AFC
	public override string ModuleName()
	{
		return "retrieveVirtualGoodCoupons";
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000A2904 File Offset: 0x000A0B04
	public override int ModuleChannel()
	{
		return 1;
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x000A2908 File Offset: 0x000A0B08
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		if (moduleData.data != null)
		{
			SoaringObjectBase soaringObjectBase = moduleData.data.objectWithKey("coupons");
			if (soaringObjectBase != null)
			{
				if (soaringObjectBase.Type == SoaringObjectBase.IsType.Array)
				{
					soaringArray = (SoaringArray)soaringObjectBase;
				}
				else
				{
					soaringArray = new SoaringArray(1);
					soaringArray.addObject(soaringObjectBase);
				}
				int num = soaringArray.count();
				SoaringArray soaringArray2 = new SoaringArray(num);
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(i);
					string text = soaringDictionary.soaringValue("coupon");
					string text2 = soaringDictionary.soaringValue("receipt");
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						SoaringCoupon obj = new SoaringCoupon(text, text2);
						soaringArray2.addObject(obj);
					}
				}
				soaringArray = soaringArray2;
			}
		}
		SoaringInternal.Delegate.OnCheckUserRewards(moduleData.state, moduleData.error, soaringArray);
	}
}
