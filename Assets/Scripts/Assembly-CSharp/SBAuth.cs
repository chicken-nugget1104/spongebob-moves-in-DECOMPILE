using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class SBAuth : SoaringDelegate
{
	// Token: 0x060009CF RID: 2511 RVA: 0x0003D5D8 File Offset: 0x0003B7D8
	public SBAuth(RuntimePlatform platform)
	{
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0003D5E4 File Offset: 0x0003B7E4
	public SoaringPlayerResolver AccountResolver()
	{
		return this.soaringPlayerResolver;
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0003D5EC File Offset: 0x0003B7EC
	public bool AccountResolveRequired()
	{
		return this.soaringPlayerResolver != null;
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0003D5FC File Offset: 0x0003B7FC
	public void AccountResolved()
	{
		this.soaringPlayerResolver = null;
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x0003D608 File Offset: 0x0003B808
	public void ResetAuth()
	{
		SBAuth.campaigns = null;
		Soaring.LogOut();
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x0003D618 File Offset: 0x0003B818
	public void FindAndMigrateLoginID()
	{
		SoaringPlayerResolver.FindLoginID(new SoaringContext
		{
			ContextResponder = new SoaringContextDelegate(this.OnFindLoginID)
		});
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0003D644 File Offset: 0x0003B844
	private void MigrateLocalData(string kffPlayerID, string soaringUserID, SoaringLoginType loginType)
	{
		bool flag = false;
		if (!string.IsNullOrEmpty(soaringUserID))
		{
			flag = !Player.CheckSoaringPathExists(soaringUserID);
		}
		if (flag)
		{
			string text;
			if (!string.IsNullOrEmpty(kffPlayerID))
			{
				text = kffPlayerID;
			}
			else if (loginType == SoaringLoginType.Soaring || loginType == SoaringLoginType.Device)
			{
				text = SoaringPlatform.DeviceID;
			}
			else
			{
				text = SoaringPlatform.PlatformUserID;
			}
			if (SoaringInternal.instance.Versions != null)
			{
				SoaringInternal.instance.Versions.ClearAllContent();
				TFUtils.RefreshSAFiles();
			}
			SoaringDebug.Log("Migration ID: " + text + " : " + loginType.ToString());
			Player.MigratePlayerData(soaringUserID, text);
		}
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0003D6EC File Offset: 0x0003B8EC
	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (error != null)
		{
			success = false;
		}
		if (success)
		{
			this.MigrateLocalData(null, player.UserTag, player.LoginType);
			if (player.Name != null)
			{
				if (player.LoginType == SoaringLoginType.Soaring || player.LoginType == SoaringLoginType.Device)
				{
					if (player.Name != SoaringPlatform.DeviceID)
					{
					}
				}
				else if (player.Name != SoaringPlatform.PlatformUserAlias)
				{
				}
			}
			Soaring.RetreiveUserProfile(null);
			Soaring.RequestCampaign(new SoaringContext
			{
				Responder = this
			});
			if (player.LoginType != SoaringLoginType.Soaring && player.LoginType != SoaringLoginType.Device)
			{
				GameObject gameObject = GameObject.Find("SBGameCenterManager");
				SBGameCenterManager component = gameObject.GetComponent<SBGameCenterManager>();
				component.PlayerAuthenticated();
			}
		}
		else
		{
			SoaringDebug.Log("Authorization Failed: " + error, LogType.Error);
			this.SoaringAuthorizing = false;
		}
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x0003D7E4 File Offset: 0x0003B9E4
	public override void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
		if (!success)
		{
			string str = string.Empty;
			if (error != null)
			{
				str = error;
			}
			SoaringDebug.Log("Failed to initialize soaring: " + str, LogType.Error);
		}
		SBMISoaring.OnInitializeSoaring();
		this.FindAndMigrateLoginID();
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x0003D828 File Offset: 0x0003BA28
	public void OnFindLoginID(SoaringContext context)
	{
		SoaringDebug.Log("OnFindLoginID: " + context.ToJsonString(), LogType.Error);
		string playerID = context.soaringValue("id");
		SoaringLoginType type = (SoaringLoginType)context.soaringValue("type");
		context.Responder = this;
		context.ContextResponder = null;
		SBAuth.campaigns = null;
		SBMISoaring.FinalizeMigration(playerID, type, context);
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0003D88C File Offset: 0x0003BA8C
	public override void OnPlayerConflict(SoaringPlayerResolver resolver, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
		this.soaringPlayerResolver = resolver;
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = platform_player;
		if (soaringPlayerData == null)
		{
			soaringPlayerData = device_player;
		}
		resolver.HandleLoginConflict(soaringPlayerData, null);
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (data == null)
		{
			Debug.LogWarning(base.GetType().Name + ": OnComponentFinished: " + module);
		}
		else
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				base.GetType().Name,
				": OnComponentFinished: ",
				module,
				"\n",
				data.ToJsonString()
			}));
		}
		if (module == "finalizeMigration")
		{
			if (!success || error != null || data == null)
			{
				SoaringInternal.instance.TriggerOfflineMode(true);
			}
			else
			{
				bool flag = data.soaringValue("found");
				SoaringLoginType loginType = (SoaringLoginType)context.soaringValue("type");
				if (flag)
				{
					string kffPlayerID = data.soaringValue("kffUserId");
					string soaringUserID = data.soaringValue("tag");
					this.MigrateLocalData(kffPlayerID, soaringUserID, loginType);
				}
			}
			Soaring.Player.Load(null);
		}
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
	public override void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray cpns, SoaringContext context)
	{
		this.SoaringAuthorizing = false;
		if (cpns == null)
		{
			SBAuth.campaigns = null;
			return;
		}
		int num = cpns.count();
		if (num == 0)
		{
			SBAuth.campaigns = null;
			return;
		}
		SBAuth.campaigns = new SoaringDictionary(num);
		for (int i = 0; i < num; i++)
		{
			SoaringCampaign soaringCampaign = (SoaringCampaign)cpns.objectAtIndex(i);
			string text = soaringCampaign.Group;
			if (string.IsNullOrEmpty(text))
			{
				text = "none";
			}
			SBAuth.campaigns.addValue(text, soaringCampaign.Description);
		}
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0003DA50 File Offset: 0x0003BC50
	public override void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
		if (soaringEv == null)
		{
			return;
		}
		Session session_ref = SessionDriver.session_ref;
		if (session_ref == null)
		{
			return;
		}
		SoaringDebug.Log(soaringEv.Name + ": Adding Event");
		session_ref.soaringEvents.addObject(soaringEv);
	}

	// Token: 0x040006C3 RID: 1731
	public bool SoaringAuthorizing;

	// Token: 0x040006C4 RID: 1732
	private SoaringPlayerResolver soaringPlayerResolver;

	// Token: 0x040006C5 RID: 1733
	public static SoaringDictionary campaigns;
}
