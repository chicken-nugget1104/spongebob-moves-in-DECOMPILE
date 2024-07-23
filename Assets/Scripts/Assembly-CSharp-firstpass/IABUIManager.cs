using System;
using Prime31;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public class IABUIManager : MonoBehaviourGUI
{
	// Token: 0x060006B2 RID: 1714 RVA: 0x00017ED4 File Offset: 0x000160D4
	private void OnGUI()
	{
		base.beginColumn();
		if (GUILayout.Button("Initialize IAB", new GUILayoutOption[0]))
		{
			string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwqo+nBgOz9uvW/EeHYhTpctG0OdutjmzVbHlQBJWYVgqaDFcliO76afXQ2qnf5EW7H7jlQcF42zizzs1O1vC7cmYN6mbTnWnmOEDHuSyG02tKIVU2pGWBM/VsnIqL4lpmNaM4JKJYNcZ7a9pnBhfiUAqR9pejVAqYqVc5dG091TPIwzE/MjchSac0EFXa49iMuiYfdYjzXye/981t23PDk7IS+weXcthqjSfjwtnxobxktnJ9eSUK7jaBpFPbRIFHXuE/+ZQSYRtMu48myp/CnJd/0A/srnmDetb2ai990A4hQgvP9/HySyw14CHhq87tRCRAzXso5Q09iaU8e538QIDAQAB";
			GoogleIAB.init(publicKey);
		}
		if (GUILayout.Button("Query Inventory", new GUILayoutOption[0]))
		{
			string[] skus = new string[]
			{
				"com.mtvn.sbmi.jellybundle1",
				"com.mtvn.sbmi.jellybundle2"
			};
			GoogleIAB.queryInventory(skus);
		}
		if (GUILayout.Button("Are subscriptions supported?", new GUILayoutOption[0]))
		{
			Debug.Log("subscriptions supported: " + GoogleIAB.areSubscriptionsSupported());
		}
		if (GUILayout.Button("Purchase Test Product", new GUILayoutOption[0]))
		{
			GoogleIAB.purchaseProduct("com.mtvn.sbmi.jellybundle1");
		}
		if (GUILayout.Button("Purchase Test Product2", new GUILayoutOption[0]))
		{
			GoogleIAB.purchaseProduct("com.mtvn.sbmi.jellybundle2");
		}
		if (GUILayout.Button("Consume Test Purchase", new GUILayoutOption[0]))
		{
			GoogleIAB.consumeProduct("android.test.purchased");
		}
		if (GUILayout.Button("Test Unavailable Item", new GUILayoutOption[0]))
		{
			GoogleIAB.purchaseProduct("android.test.item_unavailable");
		}
		base.endColumn(true);
		if (GUILayout.Button("Purchase Real Product", new GUILayoutOption[0]))
		{
			GoogleIAB.purchaseProduct("com.prime31.testproduct", "payload that gets stored and returned");
		}
		if (GUILayout.Button("Purchase Real Subscription", new GUILayoutOption[0]))
		{
			GoogleIAB.purchaseProduct("com.prime31.testsubscription", "subscription payload");
		}
		if (GUILayout.Button("Consume Real Purchase", new GUILayoutOption[0]))
		{
			GoogleIAB.consumeProduct("com.prime31.testproduct");
		}
		if (GUILayout.Button("Enable High Details Logs", new GUILayoutOption[0]))
		{
			GoogleIAB.enableLogging(true);
		}
		if (GUILayout.Button("Consume Multiple Purchases", new GUILayoutOption[0]))
		{
			string[] skus2 = new string[]
			{
				"com.prime31.testproduct",
				"android.test.purchased"
			};
			GoogleIAB.consumeProducts(skus2);
		}
		if (GUILayout.Button("loading spongeBob", new GUILayoutOption[0]))
		{
			Application.LoadLevel("Scene0");
		}
		base.endColumn();
	}
}
