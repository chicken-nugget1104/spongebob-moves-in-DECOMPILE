using System;

// Token: 0x020003DC RID: 988
public static class Soaring
{
	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x000BA354 File Offset: 0x000B8554
	public static SoaringDelegate Delegate
	{
		get
		{
			return SoaringInternal.Delegate;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x000BA35C File Offset: 0x000B855C
	public static SoaringLoginType PreferedDeviceLogin
	{
		get
		{
			return SoaringPlatform.PreferedLoginType;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06001DBA RID: 7610 RVA: 0x000BA364 File Offset: 0x000B8564
	public static string ServerUrl
	{
		get
		{
			return SoaringInternal.instance.CurrentServer;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06001DBB RID: 7611 RVA: 0x000BA370 File Offset: 0x000B8570
	public static string ServerContentUrl
	{
		get
		{
			return SoaringInternal.instance.CurrentContentURL;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06001DBC RID: 7612 RVA: 0x000BA37C File Offset: 0x000B857C
	public static SoaringPlayer Player
	{
		get
		{
			return SoaringInternal.instance.Player;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06001DBD RID: 7613 RVA: 0x000BA388 File Offset: 0x000B8588
	public static SoaringCommunityEventManager CommunityEventManager
	{
		get
		{
			return SoaringInternal.instance.CommunityEventManager;
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06001DBE RID: 7614 RVA: 0x000BA394 File Offset: 0x000B8594
	public static bool IsOnline
	{
		get
		{
			return SoaringInternal.IsOnline;
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06001DBF RID: 7615 RVA: 0x000BA39C File Offset: 0x000B859C
	public static bool IsInitialized
	{
		get
		{
			return SoaringInternal.instance.IsInitialized();
		}
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x000BA3A8 File Offset: 0x000B85A8
	public static void StartSoaring(string gameID, SoaringDelegate del, SoaringMode mode, SoaringPlatformType platform = SoaringPlatformType.System)
	{
		SoaringInternal.instance.Initialize(gameID, del, mode, platform);
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x000BA3BC File Offset: 0x000B85BC
	public static void StopSoaring()
	{
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x000BA3C0 File Offset: 0x000B85C0
	public static void SetGameVersion(Version version)
	{
		SoaringInternal.SetGameVersion(version);
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000BA3C8 File Offset: 0x000B85C8
	public static void AddDelegate(SoaringDelegate del)
	{
		SoaringInternal.instance.RegisterDelegate(del);
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x000BA3D8 File Offset: 0x000B85D8
	public static void RemoveDelegate(SoaringDelegate del)
	{
		SoaringInternal.instance.UnregisterDelegate(del);
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000BA3E8 File Offset: 0x000B85E8
	public static void RemoveDelegate(Type type)
	{
		SoaringInternal.instance.UnregisterDelegate(type);
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x000BA3F8 File Offset: 0x000B85F8
	public static bool IsAuthorized
	{
		get
		{
			return SoaringInternal.instance.IsAuthorized();
		}
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x000BA404 File Offset: 0x000B8604
	public static bool HasAuthorizedCredentials
	{
		get
		{
			return SoaringInternal.instance.HasAuthorizedCredentials();
		}
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000BA410 File Offset: 0x000B8610
	public static void GenerateUniqueNewUserName(SoaringContext context = null)
	{
		SoaringInternal.instance.GenerateUniqueNewUserName(false, context);
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000BA420 File Offset: 0x000B8620
	public static void GenerateInviteCode()
	{
		SoaringInternal.instance.GenerateInviteCode();
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000BA42C File Offset: 0x000B862C
	public static void Login(SoaringContext context = null)
	{
		SoaringInternal.instance.Login(context);
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000BA43C File Offset: 0x000B863C
	public static void Login(string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.Login(null, null, platformID, loginType, false, context);
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x000BA45C File Offset: 0x000B865C
	public static void Login(string userName, string password, SoaringContext context = null)
	{
		SoaringInternal.instance.Login(userName, password, null, SoaringLoginType.Soaring, false, context);
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x000BA47C File Offset: 0x000B867C
	public static void Login(string userName, string password, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.Login(userName, password, null, loginType, false, context);
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000BA49C File Offset: 0x000B869C
	public static void LookupUser(string platformID, SoaringContext context = null)
	{
		Soaring.LookupUser(platformID, Soaring.PreferedDeviceLogin, context);
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x000BA4AC File Offset: 0x000B86AC
	public static void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.LookupUser(platformID, loginType, context);
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x000BA4BC File Offset: 0x000B86BC
	public static void LookupUser(SoaringArray identifiers, SoaringContext context = null)
	{
		SoaringInternal.instance.LookupUser(identifiers, context);
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x000BA4CC File Offset: 0x000B86CC
	public static void RetreiveUserProfile(SoaringContext context = null)
	{
		SoaringInternal.instance.RetrieveUserProfile(null, context);
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x000BA4DC File Offset: 0x000B86DC
	public static void RetreiveUserProfile(string userID, SoaringContext context = null)
	{
		SoaringInternal.instance.RetrieveUserProfile(userID, context);
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x000BA4EC File Offset: 0x000B86EC
	public static void RegisterLiteUser(string userName, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, null, null, true, SoaringLoginType.Soaring, false, context);
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x000BA50C File Offset: 0x000B870C
	public static void RegisterLiteUser(string userName, string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, null, platformID, true, loginType, false, context);
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x000BA52C File Offset: 0x000B872C
	public static void RegisterUser(string userName, string password, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, password, null, false, SoaringLoginType.Soaring, false, context);
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x000BA54C File Offset: 0x000B874C
	public static void RegisterUser(string userName, string password, string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, password, platformID, false, loginType, false, context);
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x000BA56C File Offset: 0x000B876C
	public static void RegisterUser(string userName, string password, bool userCreated, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, password, null, !userCreated, SoaringLoginType.Soaring, false, context);
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x000BA590 File Offset: 0x000B8790
	public static void RequestinviteCode()
	{
		SoaringInternal.instance.GenerateInviteCode();
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000BA59C File Offset: 0x000B879C
	public static void RequestFriendship(string tag, string email, string userid, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestFriendship(tag, email, userid, context);
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000BA5AC File Offset: 0x000B87AC
	public static void RequestFriendships(SoaringArray userIds, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestFriendships(userIds, context);
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x000BA5BC File Offset: 0x000B87BC
	public static void RequestFriendshipWithCode(string code, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestFriendshipWithCode(code, context);
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x000BA5CC File Offset: 0x000B87CC
	public static void RemoveFriendship(string tag, string email, string userid, SoaringContext context = null)
	{
		SoaringInternal.instance.RemoveFriendship(tag, email, userid, context);
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x000BA5DC File Offset: 0x000B87DC
	public static void UpdateFriendsListWithLastSettings(SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsListWithLastSettings(-1, -1, context);
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x000BA5EC File Offset: 0x000B87EC
	public static void UpdateFriendsListWithLastSettings(int start, int end, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsListWithLastSettings(start, end, context);
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x000BA5FC File Offset: 0x000B87FC
	public static void UpdateFriendList(string order = null, string mode = null, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsList(-1, -1, order, mode, context);
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000BA610 File Offset: 0x000B8810
	public static void UpdateFriendList(int start, int end, string order = null, string mode = null, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsList(start, end, order, mode, context);
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000BA624 File Offset: 0x000B8824
	public static void UpdateUserProfile(SoaringDictionary custom, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerProfile(custom, context);
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000BA634 File Offset: 0x000B8834
	public static void UpdateUserProfile(SoaringDictionary userData, SoaringDictionary custom, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerProfile(userData, custom, context);
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x000BA644 File Offset: 0x000B8844
	public static void UpdateUserProfile(string tag, string status, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerProfile(tag, status, context);
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000BA654 File Offset: 0x000B8854
	public static void UpdateUserFacebookInfo(string userId, string icon, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerFacebookID(userId, icon, context);
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000BA664 File Offset: 0x000B8864
	public static void FindUser(string tag, string email, string userId, string facebookId, SoaringContext context = null)
	{
		SoaringInternal.instance.FindUser(tag, email, userId, facebookId, context);
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000BA678 File Offset: 0x000B8878
	public static void FindUsers(SoaringArray tag, SoaringArray email, SoaringArray userIds, SoaringArray facebookIds, SoaringContext context = null)
	{
		SoaringInternal.instance.FindUsers(tag, email, userIds, facebookIds, context);
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000BA68C File Offset: 0x000B888C
	public static void SendSessionData(SoaringDictionary data, SoaringContext context = null)
	{
		SoaringInternal.instance.SendSessionData(data, context);
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x000BA69C File Offset: 0x000B889C
	public static void SendSessionData(string tag, SoaringSession.SessionType sessionType, SoaringDictionary data, SoaringContext context = null)
	{
		SoaringInternal.instance.SendSessionData(tag, sessionType, data, context);
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x000BA6AC File Offset: 0x000B88AC
	public static void SendSessionData(SoaringSession.SessionType sessionType, string sessionID, SoaringDictionary data, SoaringContext context = null)
	{
		SoaringInternal.instance.SendSessionData(sessionType, sessionID, data, context);
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x000BA6BC File Offset: 0x000B88BC
	public static void ApplyInviteCode(string invite_code)
	{
		SoaringInternal.instance.ApplyInviteCode(invite_code, null);
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x000BA6CC File Offset: 0x000B88CC
	public static void RequestSessionData(SoaringContext context = null)
	{
		SoaringInternal.instance.RequestSessionData(Soaring.Player.UserID, 0L, context);
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x000BA6F0 File Offset: 0x000B88F0
	public static void RequestSessionData(string session, long timeStamp = 0L, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestSessionData(session, timeStamp, context);
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x000BA700 File Offset: 0x000B8900
	public static void RequestSessionData(SoaringArray identifiers, SoaringDictionary sort, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestSessionData(identifiers, sort, context);
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x000BA710 File Offset: 0x000B8910
	public static void UpdateServerTime(SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateServerTime(context);
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x000BA720 File Offset: 0x000B8920
	public static void CheckUserRewards()
	{
		SoaringInternal.instance.CheckUserRewards();
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x000BA72C File Offset: 0x000B892C
	public static void RedeemUserReward(SoaringArray arr)
	{
		SoaringInternal.instance.RedeemRewardCoupons(arr);
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000BA73C File Offset: 0x000B893C
	public static void RedeemUserReward(SoaringCoupon coupon)
	{
		SoaringInternal.instance.RedeemRewardCoupons(coupon);
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000BA74C File Offset: 0x000B894C
	public static void CheckUnreadMessages()
	{
		SoaringInternal.instance.CheckUnreadMessages();
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x000BA758 File Offset: 0x000B8958
	public static void SendMessage(SoaringMessage message)
	{
		SoaringInternal.instance.SendMessage(message);
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x000BA768 File Offset: 0x000B8968
	public static void MarkMessageAsRead(SoaringMessage message)
	{
		SoaringInternal.instance.MarkMessageAsRead(message);
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x000BA778 File Offset: 0x000B8978
	public static void MarkMessageAsRead(SoaringArray messages)
	{
		SoaringInternal.instance.MarkMessageAsRead(messages);
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x000BA788 File Offset: 0x000B8988
	public static string SoaringAddress(string addresKey)
	{
		return SoaringInternal.instance.GetSoaringAddress(addresKey);
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x000BA798 File Offset: 0x000B8998
	public static void CheckFilesForUpdates(bool updateFiles)
	{
		SoaringInternal.instance.CheckFilesForUpdates(updateFiles);
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x000BA7A8 File Offset: 0x000B89A8
	public static void SetVersionedFileRepo(string versioning, string contentRepo = null, string fileRepo = null, string versionName = null)
	{
		SoaringInternal.instance.Versions.SetVersionServer(versioning, contentRepo, fileRepo, versionName);
	}

	// Token: 0x06001DF9 RID: 7673 RVA: 0x000BA7C8 File Offset: 0x000B89C8
	public static void RequestSoaringAdvert(string adverName = null, bool displayOnComplete = false, SoaringContext context = null)
	{
		SoaringInternal.instance.AdServer.RequestAd(adverName, displayOnComplete, context);
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x000BA7DC File Offset: 0x000B89DC
	public static bool SoaringAdvertAvailable(string adverName = null)
	{
		return SoaringInternal.instance.AdServer.AdAvailable(adverName);
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x000BA7F0 File Offset: 0x000B89F0
	public static bool SoaringDisplayAdvert(string adverName = null)
	{
		return SoaringInternal.instance.AdServer.DisplayAd(adverName);
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x000BA804 File Offset: 0x000B8A04
	public static void RequestCampaign(SoaringContext context = null)
	{
		SoaringInternal.instance.RequestCampaign(context);
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x000BA814 File Offset: 0x000B8A14
	public static void SetAdServerURL(string url)
	{
		SoaringInternal.instance.AdServer.SetAdServerURL(url);
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x000BA828 File Offset: 0x000B8A28
	public static void ResetPassword(string verifyUsername, string verifyEmail)
	{
		SoaringInternal.instance.ResetPassword(verifyUsername, verifyEmail);
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x000BA838 File Offset: 0x000B8A38
	public static void ConfirmResetPassword(string verifyUserName, string confirmCode, string newPassword)
	{
		SoaringInternal.instance.ResetPasswordConfirm(verifyUserName, confirmCode, newPassword);
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x000BA848 File Offset: 0x000B8A48
	public static void ChangePassword(string oldPassword, string newPassword, SoaringContext context = null)
	{
		SoaringInternal.instance.ChangePassword(oldPassword, newPassword, context);
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x000BA858 File Offset: 0x000B8A58
	public static void RegisterDevicePushToken(string tokenID)
	{
		SoaringInternal.instance.RegisterDevice(tokenID, null);
	}

	// Token: 0x06001E02 RID: 7682 RVA: 0x000BA868 File Offset: 0x000B8A68
	public static void SaveStat(string key, SoaringObjectBase value)
	{
		SoaringInternal.instance.SaveStat(key, value);
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x000BA878 File Offset: 0x000B8A78
	public static void SaveStat(SoaringArray entries)
	{
		SoaringInternal.instance.SaveStat(entries);
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x000BA888 File Offset: 0x000B8A88
	public static void SaveAnonymousStat(SoaringArray entries)
	{
		SoaringInternal.instance.SaveAnonymousStat(entries);
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x000BA898 File Offset: 0x000B8A98
	public static void SaveAnonymousStat(string keys, SoaringDictionary entries)
	{
		SoaringInternal.instance.SaveAnonymousStat(keys, entries);
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x000BA8A8 File Offset: 0x000B8AA8
	public static void FireEvent(string eventName, SoaringDictionary custom)
	{
		SoaringInternal.instance.FireEvent(eventName, custom, null);
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x000BA8B8 File Offset: 0x000B8AB8
	public static void RequestProducts(string store, string language, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestPurchasables(store, language, context);
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000BA8C8 File Offset: 0x000B8AC8
	public static void RequestPurchases(string store = null, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestPurchases(store, context);
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x000BA8D8 File Offset: 0x000B8AD8
	public static void ValidatePurchasableReciept(string reciept, SoaringPurchasable purchasable, string storeName = null, bool isProduction = true, string userID = null, SoaringContext context = null)
	{
		SoaringInternal.instance.ValidatePurchaseReciept(reciept, purchasable, storeName, userID, isProduction, context);
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x000BA8F8 File Offset: 0x000B8AF8
	public static void LogOut()
	{
	}
}
