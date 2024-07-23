using System;

// Token: 0x020003DD RID: 989
public class SoaringDelegate : SoaringObjectBase
{
	// Token: 0x06001E0B RID: 7691 RVA: 0x000BA8FC File Offset: 0x000B8AFC
	public SoaringDelegate() : base(SoaringObjectBase.IsType.Object)
	{
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x000BA908 File Offset: 0x000B8B08
	public virtual void InternetStateChange(bool state)
	{
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x000BA90C File Offset: 0x000B8B0C
	public virtual void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x000BA910 File Offset: 0x000B8B10
	public virtual void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x000BA914 File Offset: 0x000B8B14
	public virtual void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x000BA918 File Offset: 0x000B8B18
	public virtual void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x000BA91C File Offset: 0x000B8B1C
	public virtual void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x000BA920 File Offset: 0x000B8B20
	public virtual void OnRetrieveUserProfile(bool succes, SoaringError error, SoaringUser user, SoaringContext context)
	{
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x000BA924 File Offset: 0x000B8B24
	public virtual void OnUpdatingUserProfile(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x000BA928 File Offset: 0x000B8B28
	public virtual void OnRetrieveInvitationCode(bool success, SoaringError error, string invite_code)
	{
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x000BA92C File Offset: 0x000B8B2C
	public virtual void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x000BA930 File Offset: 0x000B8B30
	public virtual void OnFindUser(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x000BA934 File Offset: 0x000B8B34
	public virtual void OnRequestFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x000BA938 File Offset: 0x000B8B38
	public virtual void OnRemoveFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000BA93C File Offset: 0x000B8B3C
	public virtual void OnApplyInviteCode(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000BA940 File Offset: 0x000B8B40
	public virtual void OnUpdateFriendList(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000BA944 File Offset: 0x000B8B44
	public virtual void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x000BA948 File Offset: 0x000B8B48
	public virtual void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
	{
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000BA94C File Offset: 0x000B8B4C
	public virtual void OnCheckUserRewards(bool success, SoaringError error, SoaringArray rewards)
	{
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x000BA950 File Offset: 0x000B8B50
	public virtual void OnRedeemUserReward(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000BA954 File Offset: 0x000B8B54
	public virtual void OnServerTimeUpdated(bool success, SoaringError error, long timestamp, SoaringContext context)
	{
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x000BA958 File Offset: 0x000B8B58
	public virtual void OnCheckMessages(bool success, SoaringError error, SoaringArray messages)
	{
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x000BA95C File Offset: 0x000B8B5C
	public virtual void OnSendMessage(bool success, SoaringError error, SoaringMessage message)
	{
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x000BA960 File Offset: 0x000B8B60
	public virtual void OnMessageStateChanged(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x000BA964 File Offset: 0x000B8B64
	public virtual void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000BA968 File Offset: 0x000B8B68
	public virtual void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
	{
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000BA96C File Offset: 0x000B8B6C
	public virtual void OnBlockGameSession(bool forceBlock, float version, float minvVer, float maxVer, string message)
	{
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000BA970 File Offset: 0x000B8B70
	public virtual void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000BA974 File Offset: 0x000B8B74
	public virtual void OnPasswordReset(bool success, SoaringError error)
	{
	}

	// Token: 0x06001E28 RID: 7720 RVA: 0x000BA978 File Offset: 0x000B8B78
	public virtual void OnPasswordResetConfirmed(bool success, SoaringError error)
	{
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x000BA97C File Offset: 0x000B8B7C
	public virtual void OnPasswordChanged(bool success, SoaringError error, SoaringContext context)
	{
	}

	// Token: 0x06001E2A RID: 7722 RVA: 0x000BA980 File Offset: 0x000B8B80
	public virtual void OnDeviceRegistered(bool success, SoaringError error, SoaringContext context)
	{
	}

	// Token: 0x06001E2B RID: 7723 RVA: 0x000BA984 File Offset: 0x000B8B84
	public virtual void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
	{
	}

	// Token: 0x06001E2C RID: 7724 RVA: 0x000BA988 File Offset: 0x000B8B88
	public virtual void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
	{
	}

	// Token: 0x06001E2D RID: 7725 RVA: 0x000BA98C File Offset: 0x000B8B8C
	public virtual void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
	{
	}

	// Token: 0x06001E2E RID: 7726 RVA: 0x000BA990 File Offset: 0x000B8B90
	public virtual void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
	{
	}

	// Token: 0x06001E2F RID: 7727 RVA: 0x000BA994 File Offset: 0x000B8B94
	public virtual void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray campaigns, SoaringContext context)
	{
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000BA998 File Offset: 0x000B8B98
	public virtual void OnPlayerConflict(SoaringPlayerResolver player, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x000BA99C File Offset: 0x000B8B9C
	public virtual void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
	}
}
