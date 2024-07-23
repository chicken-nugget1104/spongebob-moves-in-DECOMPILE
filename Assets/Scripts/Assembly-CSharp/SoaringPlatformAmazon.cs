using System;

// Token: 0x0200037C RID: 892
public class SoaringPlatformAmazon : SoaringPlatformAndroid
{
	// Token: 0x0600194B RID: 6475 RVA: 0x000A6FC0 File Offset: 0x000A51C0
	public override SoaringLoginType PreferedLoginType()
	{
		return SoaringLoginType.Amazon;
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x000A6FC4 File Offset: 0x000A51C4
	public override string PlatformName()
	{
		return "amazon";
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x000A6FCC File Offset: 0x000A51CC
	public override void SetPlatformUserData(string userID, string userAlias)
	{
		this.mProfileID = userID;
		this.mProfileAlias = userAlias;
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000A6FDC File Offset: 0x000A51DC
	public override void Init()
	{
		base.Init();
		GameCircleManager.getInstance();
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000A6FEC File Offset: 0x000A51EC
	public override bool PlatformLoginAvailable()
	{
		return true;
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x000A6FF0 File Offset: 0x000A51F0
	public override bool PlatformAuthenticated()
	{
		return AGSClient.IsServiceReady();
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x000A6FF8 File Offset: 0x000A51F8
	public override bool PlatformAuthenticate(SoaringContext context)
	{
		bool result = false;
		if (context == null)
		{
			context = new SoaringContext();
		}
		context.Name = "login";
		this.mProfileID = string.Empty;
		this.mProfileAlias = string.Empty;
		try
		{
			SoaringInternal.instance.PushContextEvent(context);
			this.RegisterServiceEvent();
			AGSClient.Init(false, true, false);
			result = true;
		}
		catch
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x000A707C File Offset: 0x000A527C
	public override string PlatformID()
	{
		return this.mProfileID;
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x000A7084 File Offset: 0x000A5284
	public override string PlatformAlias()
	{
		return this.mProfileAlias;
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000A708C File Offset: 0x000A528C
	private void RegisterServiceEvent()
	{
		AGSClient.ServiceReadyEvent += this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += this.ServiceNotReadyHandler;
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x000A70BC File Offset: 0x000A52BC
	private void UnServiceEvent()
	{
		AGSClient.ServiceReadyEvent -= this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= this.ServiceNotReadyHandler;
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x000A70EC File Offset: 0x000A52EC
	private void ServiceReadyHandler()
	{
		this.UnServiceEvent();
		this.SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000A7100 File Offset: 0x000A5300
	private void ServiceNotReadyHandler(string error)
	{
		this.UnServiceEvent();
		this.callback_comlete_failed(error);
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000A7110 File Offset: 0x000A5310
	private void SubscribeToProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent += this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent += this.PlayerAliasFailed;
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000A7140 File Offset: 0x000A5340
	private void UnsubscribeFromProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent -= this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent -= this.PlayerAliasFailed;
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000A7170 File Offset: 0x000A5370
	private void PlayerAliasReceived(AGSProfile profile)
	{
		this.UnsubscribeFromProfileEvents();
		this.SetPlatformUserData(profile.playerId, profile.alias);
		this.callback_comlete_success();
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000A7190 File Offset: 0x000A5390
	private void PlayerAliasFailed(string errorMessage)
	{
		this.UnsubscribeFromProfileEvents();
		this.callback_comlete_failed(errorMessage);
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x000A71A0 File Offset: 0x000A53A0
	private void callback_comlete_failed(string error)
	{
		SoaringDebug.Log("SoaringPlatformAmazon: error: " + error);
		SoaringInternal.instance.mWebQueue.onExternalMessage("{\"call\":\"login\",\"status\":\"false\"}");
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x000A71D4 File Offset: 0x000A53D4
	private void callback_comlete_success()
	{
		string message = string.Concat(new string[]
		{
			"{\"call\":\"login\",\"status\":\"true\",\"id\":\"",
			this.mProfileID,
			"\",\"name\":\"",
			this.mProfileAlias,
			"\"}"
		});
		SoaringInternal.instance.mWebQueue.onExternalMessage(message);
	}

	// Token: 0x0400107F RID: 4223
	private string mProfileID = string.Empty;

	// Token: 0x04001080 RID: 4224
	private string mProfileAlias = string.Empty;
}
