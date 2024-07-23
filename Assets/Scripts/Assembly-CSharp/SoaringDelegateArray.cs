using System;

// Token: 0x020003DE RID: 990
internal class SoaringDelegateArray : SoaringDelegate
{
	// Token: 0x06001E32 RID: 7730 RVA: 0x000BA9A0 File Offset: 0x000B8BA0
	public SoaringDelegateArray()
	{
		this.mDelegateArray = new SoaringArray<SoaringDelegate>(2);
	}

	// Token: 0x06001E33 RID: 7731 RVA: 0x000BA9B4 File Offset: 0x000B8BB4
	public SoaringArray<SoaringDelegate> Modules()
	{
		SoaringArray<SoaringDelegate> soaringArray = new SoaringArray<SoaringDelegate>(this.mDelegateArray.count());
		for (int i = 0; i < this.mDelegateArray.count(); i++)
		{
			soaringArray.addObject(this.mDelegateArray.objectAtIndex(i));
		}
		return soaringArray;
	}

	// Token: 0x06001E34 RID: 7732 RVA: 0x000BAA04 File Offset: 0x000B8C04
	public void RegisterDelegate(SoaringDelegate del)
	{
		if (del == null)
		{
			return;
		}
		Type type = del.GetType();
		int num = -1;
		int num2 = this.mDelegateArray.count();
		for (int i = 0; i < num2; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate == null)
			{
				num = i;
			}
			else
			{
				if (del == soaringDelegate)
				{
					return;
				}
				if (type == soaringDelegate.GetType())
				{
					return;
				}
			}
		}
		if (num == -1)
		{
			this.mDelegateArray.addObject(del);
		}
		else
		{
			this.mDelegateArray.setObjectAtIndex(del, num);
		}
	}

	// Token: 0x06001E35 RID: 7733 RVA: 0x000BAA98 File Offset: 0x000B8C98
	public void UnregisterDelegate(SoaringDelegate del)
	{
		if (del == null)
		{
			return;
		}
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				if (del == soaringDelegate)
				{
					this.mDelegateArray.removeObjectAtIndex(i);
					return;
				}
			}
		}
	}

	// Token: 0x06001E36 RID: 7734 RVA: 0x000BAAF8 File Offset: 0x000B8CF8
	public void UnregisterDelegate(Type type)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				if (type == soaringDelegate.GetType())
				{
					this.mDelegateArray.removeObjectAtIndex(i);
					return;
				}
			}
		}
	}

	// Token: 0x06001E37 RID: 7735 RVA: 0x000BAB54 File Offset: 0x000B8D54
	public bool UseMainResponder(SoaringContext context)
	{
		return context != null && context.Responder != null;
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x000BAB6C File Offset: 0x000B8D6C
	public override void InternetStateChange(bool state)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.InternetStateChange(state);
			}
		}
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x000BABB8 File Offset: 0x000B8DB8
	public override void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnInitializing(success, error, data);
			}
		}
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x000BAC04 File Offset: 0x000B8E04
	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnAuthorize(success, error, player, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnAuthorize(success, error, player, context);
				}
			}
		}
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x000BAC78 File Offset: 0x000B8E78
	public override void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnLookupUser(success, error, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnLookupUser(success, error, context);
				}
			}
		}
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000BACE4 File Offset: 0x000B8EE4
	public override void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnGenerateUserName(success, error, nextTag, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnGenerateUserName(success, error, nextTag, context);
				}
			}
		}
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x000BAD58 File Offset: 0x000B8F58
	public override void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRegisterUser(success, error, player, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRegisterUser(success, error, player, context);
				}
			}
		}
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x000BADCC File Offset: 0x000B8FCC
	public override void OnRetrieveUserProfile(bool succes, SoaringError error, SoaringUser user, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRetrieveUserProfile(succes, error, user, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRetrieveUserProfile(succes, error, user, context);
				}
			}
		}
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x000BAE40 File Offset: 0x000B9040
	public override void OnUpdatingUserProfile(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnUpdatingUserProfile(success, error, data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnUpdatingUserProfile(success, error, data, context);
				}
			}
		}
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x000BAEB4 File Offset: 0x000B90B4
	public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnSavingSessionData(success, error, data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnSavingSessionData(success, error, data, context);
				}
			}
		}
	}

	// Token: 0x06001E41 RID: 7745 RVA: 0x000BAF28 File Offset: 0x000B9128
	public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray session_data, SoaringDictionary raw_data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRequestingSessionData(success, error, session_data, raw_data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRequestingSessionData(success, error, session_data, raw_data, context);
				}
			}
		}
	}

	// Token: 0x06001E42 RID: 7746 RVA: 0x000BAFA0 File Offset: 0x000B91A0
	public override void OnRetrieveInvitationCode(bool success, SoaringError error, string invite_code)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRetrieveInvitationCode(success, error, invite_code);
			}
		}
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x000BAFEC File Offset: 0x000B91EC
	public override void OnFindUser(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnFindUser(success, error, users, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnFindUser(success, error, users, context);
				}
			}
		}
	}

	// Token: 0x06001E44 RID: 7748 RVA: 0x000BB060 File Offset: 0x000B9260
	public override void OnRequestFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRequestFriend(success, error, data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRequestFriend(success, error, data, context);
				}
			}
		}
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x000BB0D4 File Offset: 0x000B92D4
	public override void OnRemoveFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRemoveFriend(success, error, data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRemoveFriend(success, error, data, context);
				}
			}
		}
	}

	// Token: 0x06001E46 RID: 7750 RVA: 0x000BB148 File Offset: 0x000B9348
	public override void OnApplyInviteCode(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnApplyInviteCode(success, error, data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnApplyInviteCode(success, error, data, context);
				}
			}
		}
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x000BB1BC File Offset: 0x000B93BC
	public override void OnUpdateFriendList(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnUpdateFriendList(success, error, users, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnUpdateFriendList(success, error, users, context);
				}
			}
		}
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x000BB230 File Offset: 0x000B9430
	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnComponentFinished(success, module, error, data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnComponentFinished(success, module, error, data, context);
				}
			}
		}
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x000BB2A8 File Offset: 0x000B94A8
	public override void OnCheckUserRewards(bool success, SoaringError error, SoaringArray rewards)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnCheckUserRewards(success, error, rewards);
			}
		}
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x000BB2F4 File Offset: 0x000B94F4
	public override void OnRedeemUserReward(bool success, SoaringError error, SoaringDictionary data)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRedeemUserReward(success, error, data);
			}
		}
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x000BB340 File Offset: 0x000B9540
	public override void OnServerTimeUpdated(bool success, SoaringError error, long timestamp, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnServerTimeUpdated(success, error, timestamp, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnServerTimeUpdated(success, error, timestamp, context);
				}
			}
		}
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x000BB3B4 File Offset: 0x000B95B4
	public override void OnCheckMessages(bool success, SoaringError error, SoaringArray messages)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnCheckMessages(success, error, messages);
			}
		}
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x000BB400 File Offset: 0x000B9600
	public override void OnSendMessage(bool success, SoaringError error, SoaringMessage message)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnSendMessage(success, error, message);
			}
		}
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x000BB44C File Offset: 0x000B964C
	public override void OnMessageStateChanged(bool success, SoaringError error, SoaringDictionary data)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnMessageStateChanged(success, error, data);
			}
		}
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x000BB498 File Offset: 0x000B9698
	public override void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnFileDownloadUpdate(state, error, data, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnFileDownloadUpdate(state, error, data, context);
				}
			}
		}
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x000BB50C File Offset: 0x000B970C
	public override void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnFileVersionsUpdated(state, error, data);
			}
		}
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x000BB558 File Offset: 0x000B9758
	public override void OnBlockGameSession(bool forceBlock, float version, float minvVer, float maxVer, string message)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnBlockGameSession(forceBlock, version, minvVer, maxVer, message);
			}
		}
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x000BB5A8 File Offset: 0x000B97A8
	public override void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnAdServed(success, adData, state, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnAdServed(success, adData, state, context);
				}
			}
		}
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x000BB61C File Offset: 0x000B981C
	public override void OnPasswordReset(bool success, SoaringError error)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnPasswordReset(success, error);
			}
		}
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x000BB668 File Offset: 0x000B9868
	public override void OnPasswordResetConfirmed(bool success, SoaringError error)
	{
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnPasswordResetConfirmed(success, error);
			}
		}
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x000BB6B4 File Offset: 0x000B98B4
	public override void OnPasswordChanged(bool success, SoaringError error, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnPasswordChanged(success, error, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnPasswordChanged(success, error, context);
				}
			}
		}
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x000BB720 File Offset: 0x000B9920
	public override void OnDeviceRegistered(bool success, SoaringError error, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnDeviceRegistered(success, error, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnDeviceRegistered(success, error, context);
				}
			}
		}
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x000BB78C File Offset: 0x000B998C
	public override void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRecieptValidated(success, error, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRecieptValidated(success, error, context);
				}
			}
		}
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x000BB7F8 File Offset: 0x000B99F8
	public override void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnSaveStat(success, anonymous, error, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnSaveStat(success, anonymous, error, context);
				}
			}
		}
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x000BB86C File Offset: 0x000B9A6C
	public override void OnPlayerConflict(SoaringPlayerResolver player, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnPlayerConflict(player, platform_player, last_player, device_player, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnPlayerConflict(player, platform_player, last_player, device_player, context);
				}
			}
		}
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x000BB8E4 File Offset: 0x000B9AE4
	public override void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRetrievePurchases(success, error, purchases, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRetrievePurchases(success, error, purchases, context);
				}
			}
		}
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x000BB958 File Offset: 0x000B9B58
	public override void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRetrieveProducts(success, error, purchasables, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRetrieveProducts(success, error, purchasables, context);
				}
			}
		}
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x000BB9CC File Offset: 0x000B9BCC
	public override void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray campaigns, SoaringContext context)
	{
		if (this.UseMainResponder(context))
		{
			context.Responder.OnRetrieveCampaign(success, error, campaigns, context);
		}
		else
		{
			int num = this.mDelegateArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDelegate soaringDelegate = this.mDelegateArray[i];
				if (soaringDelegate != null)
				{
					soaringDelegate.OnRetrieveCampaign(success, error, campaigns, context);
				}
			}
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x000BBA40 File Offset: 0x000B9C40
	public override void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
		if (soaringEv == null)
		{
			return;
		}
		int num = this.mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = this.mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRecievedEvent(manager, soaringEv);
			}
		}
	}

	// Token: 0x040012C7 RID: 4807
	private SoaringArray<SoaringDelegate> mDelegateArray;
}
