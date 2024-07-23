using System;
using MTools;
using UnityEngine;

// Token: 0x0200036F RID: 879
public class SoaringRetrieveProductModule : SoaringModule
{
	// Token: 0x06001908 RID: 6408 RVA: 0x000A531C File Offset: 0x000A351C
	public override string ModuleName()
	{
		return "retrieveIapProducts";
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x000A5324 File Offset: 0x000A3524
	public override int ModuleChannel()
	{
		return 1;
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x000A5328 File Offset: 0x000A3528
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

	// Token: 0x0600190B RID: 6411 RVA: 0x000A53D0 File Offset: 0x000A35D0
	public static SoaringPurchasable[] LoadCachedProductData()
	{
		string empty = string.Empty;
		SoaringDictionary soaringDictionary = null;
		SoaringInternal.instance.Purchasables.clear();
		MBinaryReader mbinaryReader = null;
		try
		{
			mbinaryReader = ResourceUtils.GetFileStream("Products", empty + "Soaring", "dat", 9);
			if (mbinaryReader != null)
			{
				string json_data = mbinaryReader.ReadString();
				soaringDictionary = new SoaringDictionary(json_data);
				mbinaryReader.Close();
				mbinaryReader = null;
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message + "\n" + ex.StackTrace);
			try
			{
				mbinaryReader.Close();
				mbinaryReader = null;
			}
			catch
			{
			}
		}
		SoaringPurchasable[] array = null;
		if (soaringDictionary == null)
		{
			return array;
		}
		SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("products");
		if (soaringDictionary != null)
		{
			int num = soaringArray.count();
			array = new SoaringPurchasable[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new SoaringPurchasable((SoaringDictionary)soaringArray.objectAtIndex(i));
				SoaringInternal.instance.Purchasables.addValue_unsafe(array[i], array[i].ProductID);
			}
		}
		return array;
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x000A5524 File Offset: 0x000A3724
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData data)
	{
		SoaringPurchasable[] array = null;
		SoaringInternal.instance.Purchasables.clear();
		if (!data.state || data.error != null || data.data == null)
		{
			data.state = false;
			string empty = string.Empty;
			try
			{
				MBinaryReader fileStream = ResourceUtils.GetFileStream("Products", empty + "Soaring", "dat", 9);
				if (fileStream != null)
				{
					string json_data = fileStream.ReadString();
					data.data = new SoaringDictionary(json_data);
					fileStream.Close();
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message + "\n" + ex.StackTrace);
				data.data = null;
			}
		}
		if (data.data != null)
		{
			SoaringArray soaringArray = (SoaringArray)data.data.objectWithKey("products");
			if (soaringArray != null)
			{
				int num = soaringArray.count();
				array = new SoaringPurchasable[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = new SoaringPurchasable((SoaringDictionary)soaringArray.objectAtIndex(i));
					SoaringInternal.instance.Purchasables.addValue_unsafe(array[i], array[i].ProductID);
				}
			}
			string empty2 = string.Empty;
			try
			{
				string writePath = ResourceUtils.GetWritePath("Products.dat", empty2 + "Soaring", 9);
				MBinaryWriter mbinaryWriter = new MBinaryWriter();
				if (!mbinaryWriter.Open(writePath, true))
				{
					throw new Exception();
				}
				mbinaryWriter.Write(data.data.ToJsonString());
				mbinaryWriter.Close();
			}
			catch (Exception ex2)
			{
				Debug.Log(ex2.Message + "\n" + ex2.StackTrace);
			}
		}
		Soaring.Delegate.OnRetrieveProducts(data.state, data.error, array, data.context);
	}
}
