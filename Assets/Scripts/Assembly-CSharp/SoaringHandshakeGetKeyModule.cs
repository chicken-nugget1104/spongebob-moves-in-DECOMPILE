using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

// Token: 0x0200034F RID: 847
internal class SoaringHandshakeGetKeyModule : SoaringModule
{
	// Token: 0x0600185E RID: 6238 RVA: 0x000A15AC File Offset: 0x0009F7AC
	public override string ModuleName()
	{
		return "handshake_pt1";
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x000A15B4 File Offset: 0x0009F7B4
	public override int ModuleChannel()
	{
		return 0;
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x000A15B8 File Offset: 0x0009F7B8
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x000A15BC File Offset: 0x0009F7BC
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		string b = UnityEngine.Random.Range(100000000, int.MaxValue).ToString();
		context.addValue(b, "ra");
		soaringDictionary.addValue(b, "ra");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "handshake", data.soaringValue("gameId"), null) + ",\n";
		text = text + "\"data\" : " + soaringDictionary.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		soaringDictionary2.addValue(text, "data");
		context.addValue(soaringDictionary.ToJsonString(), "request");
		base.PushCorePostDataToQueue(soaringDictionary2, 0, context, false);
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x000A1688 File Offset: 0x0009F888
	public byte[] GetRSAData(string certData, byte[] encryptedData)
	{
		byte[] result = null;
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(certData);
			X509Certificate2 x509Certificate = new X509Certificate2(bytes);
			RSACryptoServiceProvider rsacryptoServiceProvider = (RSACryptoServiceProvider)x509Certificate.PublicKey.Key;
			result = rsacryptoServiceProvider.Encrypt(encryptedData, false);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
			result = null;
		}
		return result;
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x000A1710 File Offset: 0x0009F910
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		bool flag = false;
		if (moduleData.state)
		{
			string text = moduleData.data.soaringValue("cert");
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = moduleData.data.soaringValue("digest");
				if ("sha256" == text2)
				{
					string str = moduleData.data.soaringValue("rb");
					string str2 = moduleData.context.soaringValue("ra");
					SoaringEncryption soaringEncryption = new SoaringEncryption(moduleData.data.soaringValue("cipher"), text2);
					soaringEncryption.SetSID(moduleData.data.soaringValue("sid"));
					moduleData.context.addValue(soaringEncryption, "encryption");
					byte[] bytes = Encoding.ASCII.GetBytes(str2 + "-" + str);
					System.Random random = new System.Random();
					byte[] array = new byte[32];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = (byte)random.Next();
					}
					byte[] rsadata = this.GetRSAData(text, array);
					string b = Convert.ToBase64String(rsadata);
					HMACSHA256 hmacsha = new HMACSHA256(array);
					byte[] array2 = hmacsha.ComputeHash(bytes);
					SoaringDebug.Log(Convert.ToBase64String(array2));
					soaringEncryption.SetEncryptionKey(array2);
					SoaringDictionary soaringDictionary = new SoaringDictionary();
					soaringDictionary.addValue(b, "pk");
					soaringDictionary.addValue(SoaringEncryption.SID, "sid");
					flag = SoaringInternal.instance.CallModule("handshake_pt2", soaringDictionary, moduleData.context);
				}
			}
		}
		if (!flag)
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
			if (!SoaringInternal.instance.IsInitialized())
			{
				SoaringInternal.instance.HandleFinalGameInitialization(false);
			}
			else
			{
				SoaringInternal.instance.HandleStashedCalls();
			}
		}
	}
}
