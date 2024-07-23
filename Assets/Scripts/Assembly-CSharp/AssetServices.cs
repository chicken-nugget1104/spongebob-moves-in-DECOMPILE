using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class AssetServices : MonoBehaviour
{
	// Token: 0x06000A32 RID: 2610 RVA: 0x0003F700 File Offset: 0x0003D900
	private static AssetServices CreateService()
	{
		if (AssetServices.mServiceObject == null)
		{
			AssetServices.mServiceObject = new GameObject("AssetServices");
		}
		AssetServices assetServices = AssetServices.mServiceObject.AddComponent<AssetServices>();
		if (assetServices != null)
		{
			AssetServices.mServiceCounter++;
		}
		return assetServices;
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0003F754 File Offset: 0x0003D954
	public static AssetServices.AssetServicesMonitor CreateUnloadUnusedAssetService(Action callback)
	{
		AssetServices assetServices = AssetServices.CreateService();
		AssetServices.AssetServicesMonitor assetServicesMonitor = new AssetServices.AssetServicesMonitor();
		if (assetServices == null)
		{
			if (callback != null)
			{
				callback();
			}
			assetServicesMonitor.IsCompleted = true;
			return assetServicesMonitor;
		}
		assetServices.StartCoroutine(assetServices.UnloadUnusedAssets_Coroutine(callback, assetServicesMonitor));
		return assetServicesMonitor;
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0003F7A0 File Offset: 0x0003D9A0
	private IEnumerator UnloadUnusedAssets_Coroutine(Action callback, AssetServices.AssetServicesMonitor monitor)
	{
		yield return Resources.UnloadUnusedAssets();
		if (callback != null)
		{
			callback();
		}
		this.CleanupService(monitor);
		yield break;
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0003F7D8 File Offset: 0x0003D9D8
	public void CleanupService(AssetServices.AssetServicesMonitor monitor)
	{
		monitor.IsCompleted = true;
		AssetServices.mServiceCounter--;
		if (AssetServices.mServiceCounter <= 0)
		{
			UnityEngine.Object.Destroy(AssetServices.mServiceObject);
			AssetServices.mServiceObject = null;
		}
	}

	// Token: 0x040006F9 RID: 1785
	public static GameObject mServiceObject;

	// Token: 0x040006FA RID: 1786
	public static volatile int mServiceCounter;

	// Token: 0x02000116 RID: 278
	public class AssetServicesMonitor
	{
		// Token: 0x040006FB RID: 1787
		public volatile bool IsCompleted;

		// Token: 0x040006FC RID: 1788
		public object Data;

		// Token: 0x040006FD RID: 1789
		public object ServiceData;
	}
}
