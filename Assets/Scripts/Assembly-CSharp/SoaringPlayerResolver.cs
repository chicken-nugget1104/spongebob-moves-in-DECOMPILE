using System;
using MTools;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class SoaringPlayerResolver : SoaringDelegate
{
	// Token: 0x06001CC4 RID: 7364 RVA: 0x000B5F70 File Offset: 0x000B4170
	public SoaringPlayerResolver()
	{
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x000B5F78 File Offset: 0x000B4178
	public SoaringPlayerResolver(bool retrieveID)
	{
		this.RetrieveID = retrieveID;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x000B5F88 File Offset: 0x000B4188
	public SoaringPlayerResolver(SoaringPlayerResolver.SoaringPlayerData platform_user, SoaringPlayerResolver.SoaringPlayerData player_last, SoaringPlayerResolver.SoaringPlayerData device_player)
	{
		this.ResolvePlatformData = platform_user;
		this.ResolveLastUserData = player_last;
		this.ResolveDeviceData = device_player;
		this.RetrieveID = false;
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x000B5FBC File Offset: 0x000B41BC
	public static SoaringArray UsersArray
	{
		get
		{
			return SoaringPlayerResolver.sUserArray;
		}
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x000B5FC4 File Offset: 0x000B41C4
	public static bool Load(SoaringPlayer player, string loadPlayer)
	{
		bool result;
		if (PlayerPrefs.HasKey("SCFWS_CaC"))
		{
			result = SoaringPlayerResolver.LoadV1(player);
		}
		else
		{
			result = SoaringPlayerResolver.LoadV2(player, loadPlayer);
		}
		return result;
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x000B5FF8 File Offset: 0x000B41F8
	private static bool LoadV1(SoaringPlayer player)
	{
		SoaringPlayer.ValidCredentials = (PlayerPrefs.GetInt("SCFWS_CaC") != 0);
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		string @string = PlayerPrefs.GetString("SCFWS_AuthTo", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			soaringDictionary.addValue(@string, "authToken");
		}
		@string = PlayerPrefs.GetString("SCFWS_Tag", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			soaringDictionary.addValue(@string, "tag");
		}
		@string = PlayerPrefs.GetString("SCFWS_UserID", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			soaringDictionary.addValue(@string, "userId");
		}
		@string = PlayerPrefs.GetString("SCFWS_Password", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			soaringDictionary.addValue(@string, "password");
		}
		@string = PlayerPrefs.GetString("SCFWS_Invite", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			soaringDictionary.addValue(@string, "invitationCode");
		}
		player.SetUserData(soaringDictionary);
		PlayerPrefs.DeleteAll();
		return true;
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x000B610C File Offset: 0x000B430C
	private static string LoadSoaringPlayers()
	{
		SoaringDictionary soaringDictionary = null;
		string text = null;
		string text2 = null;
		int num = 0;
		try
		{
			MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringUsers", "Soaring", "dat", 1);
			if (fileStream != null)
			{
				if (fileStream.IsOpen())
				{
					text = fileStream.ReadString();
					text2 = fileStream.ReadString();
					if (!string.IsNullOrEmpty(text))
					{
						string text3 = fileStream.ReadString();
						byte[] array = Convert.FromBase64String(text3);
						text3 = string.Empty;
						if (array == null)
						{
							text3 = "{}";
						}
						else
						{
							for (int i = 0; i < array.Length; i++)
							{
								text3 += (char)array[i];
							}
						}
						SoaringDebug.Log(string.Concat(new string[]
						{
							text,
							"\n",
							text2,
							"\n ",
							text3
						}), LogType.Warning);
						soaringDictionary = new SoaringDictionary(text3);
						num = soaringDictionary.count();
					}
				}
				else
				{
					SoaringDebug.Log("Failed To Open Users Data", LogType.Warning);
				}
			}
			else
			{
				SoaringDebug.Log("Failed To Create Users Data Reader", LogType.Warning);
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringPlayerResolver: " + ex.Message + "\n" + ex.StackTrace, LogType.Error);
			soaringDictionary = null;
			num = 0;
		}
		if (num == 0)
		{
		}
		if (num > 0)
		{
			SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectAtIndex(0);
			int num2 = soaringArray.count();
			SoaringPlayerResolver.sUserArray = new SoaringArray(num2);
			for (int j = 0; j < num2; j++)
			{
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringArray.objectAtIndex(j);
				if (soaringDictionary2 != null)
				{
					SoaringPlayerResolver.SoaringPlayerData obj = SoaringPlayerResolver.ExtractPlayerData(soaringDictionary2);
					SoaringPlayerResolver.sUserArray.addObject(obj);
				}
			}
		}
		else
		{
			SoaringPlayerResolver.sUserArray = new SoaringArray();
		}
		SoaringPlayerResolver.sProperties = text;
		return text2;
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x000B62F4 File Offset: 0x000B44F4
	private static bool TestLoadPlatformUserID(string last_user, SoaringContext context, bool retrieveID)
	{
		if (context == null)
		{
			context = new SoaringContext();
		}
		context.Responder = new SoaringPlayerResolver(retrieveID);
		if (!string.IsNullOrEmpty(last_user))
		{
			context.addValue(last_user, "last_user");
		}
		context.addValue(SoaringPlayerResolver.sProperties, "properties");
		return SoaringPlatform.AuthenticatedPlatformUser(context);
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x000B6354 File Offset: 0x000B4554
	public static void FindLoginID(SoaringContext context)
	{
		string text = SoaringPlayerResolver.LoadSoaringPlayers();
		if (!string.IsNullOrEmpty(text))
		{
			context.addValue(text, "last_user");
		}
		bool flag = true;
		if (SoaringPlatform.PlatformLoginAvailable)
		{
			if (SoaringPlatform.PlatformLoginAuthenticated)
			{
				context.addValue(SoaringPlatform.PlatformUserID, "platform_id");
			}
			else
			{
				flag = !SoaringPlayerResolver.TestLoadPlatformUserID(text, context, true);
			}
		}
		if (flag)
		{
			SoaringPlayerResolver.FindLoginIDReturn(context);
		}
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x000B63CC File Offset: 0x000B45CC
	private static void FindLoginIDReturn(SoaringContext context)
	{
		SoaringValue b = context.soaringValue("platform_id");
		SoaringLoginType b2 = Soaring.PreferedDeviceLogin;
		string b3;
		if (!string.IsNullOrEmpty(b))
		{
			b3 = b;
		}
		else
		{
			b2 = SoaringLoginType.Device;
			b3 = SoaringPlatform.DeviceID;
		}
		context.removeObjectWithKey("platform_id");
		context.removeObjectWithKey("last_user");
		context.addValue(b3, "id");
		context.addValue((int)b2, "type");
		context.Responder = null;
		if (context.ContextResponder != null)
		{
			context.ContextResponder(context);
		}
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x000B6468 File Offset: 0x000B4668
	private static bool LoadV2(SoaringPlayer player, string loadPlayer)
	{
		string text = SoaringPlayerResolver.LoadSoaringPlayers();
		if (!string.IsNullOrEmpty(loadPlayer))
		{
			text = loadPlayer;
		}
		if (SoaringPlatform.PlatformLoginAvailable)
		{
			if (SoaringPlatform.PlatformLoginAuthenticated)
			{
				return SoaringPlayerResolver.LoadPart2(SoaringPlatform.PlatformUserID, SoaringPlatform.PlatformUserAlias, text);
			}
			if (SoaringPlayerResolver.TestLoadPlatformUserID(text, null, false))
			{
				return false;
			}
		}
		return SoaringPlayerResolver.LoadPart2(null, null, text);
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x000B64C4 File Offset: 0x000B46C4
	private static void SetContextData(SoaringContext context, SoaringPlayerResolver.SoaringPlayerData playerData)
	{
		if (context == null)
		{
			return;
		}
		context.addValue(playerData, "user_data");
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x000B64DC File Offset: 0x000B46DC
	private static SoaringPlayerResolver.SoaringPlayerData NullPlayerDataResolver(SoaringPlayerResolver.SoaringPlayerData data, string userID, SoaringLoginType loginType)
	{
		if (data != null)
		{
			return data;
		}
		data = new SoaringPlayerResolver.SoaringPlayerData();
		data.soaringTag = userID;
		data.platformID = userID;
		data.loginType = loginType;
		return data;
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x000B6504 File Offset: 0x000B4704
	private static bool CanCallLogin(SoaringPlayerResolver.SoaringPlayerData playerData)
	{
		return playerData != null && !string.IsNullOrEmpty(playerData.password) && !string.IsNullOrEmpty(playerData.soaringTag);
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x000B6530 File Offset: 0x000B4730
	private static bool LoadPart2(string platformUserID, string platformUserAlias, string lastUser)
	{
		Debug.LogWarning("LoadPart2: " + lastUser);
		if (string.IsNullOrEmpty(platformUserAlias))
		{
			platformUserAlias = platformUserID;
		}
		bool flag = false;
		if (!string.IsNullOrEmpty(platformUserID))
		{
			flag = true;
		}
		if (SoaringInternalProperties.ForceOfflineModeUser)
		{
			if (string.IsNullOrEmpty(platformUserID))
			{
				platformUserID = "Player";
				platformUserAlias = platformUserID;
			}
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = SoaringPlayerResolver.GetUserData(platformUserID, true);
			if (soaringPlayerData == null)
			{
				soaringPlayerData = new SoaringPlayerResolver.SoaringPlayerData();
				soaringPlayerData.soaringTag = (soaringPlayerData.platformID = platformUserID);
				soaringPlayerData.loginType = SoaringLoginType.Device;
			}
			SoaringDictionary soaringDictionary = new SoaringDictionary(4);
			soaringDictionary.addValue(soaringPlayerData.soaringTag, "tag");
			soaringDictionary.addValue("Offline", "userId");
			soaringDictionary.addValue(string.Empty, "authToken");
			SoaringInternal.instance.UpdatePlayerData(soaringDictionary);
			Soaring.Player.LoginType = soaringPlayerData.loginType;
			Soaring.Player.IsLocalAuthorized = true;
			SoaringContext soaringContext = new SoaringContext();
			soaringContext.Responder = new SoaringPlayerResolver();
			soaringContext.addValue(soaringPlayerData, "user_data");
			soaringContext.Responder.OnAuthorize(true, null, Soaring.Player, soaringContext);
			return false;
		}
		Soaring.Player.IsLocalAuthorized = false;
		SoaringContext soaringContext2 = new SoaringContext();
		if (flag)
		{
			Debug.LogError("LoadPart2: Platform: " + platformUserID);
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData2 = null;
			soaringContext2.Responder = new SoaringPlayerResolver();
			if (string.IsNullOrEmpty(lastUser))
			{
				Debug.LogError("No Last User");
				soaringPlayerData2 = SoaringPlayerResolver.GetUserData(platformUserID, true);
				SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(soaringPlayerData2, platformUserID, Soaring.PreferedDeviceLogin));
				if (SoaringPlayerResolver.CanCallLogin(soaringPlayerData2))
				{
					Soaring.Login(soaringPlayerData2.soaringTag, soaringPlayerData2.password, soaringContext2);
				}
				else
				{
					SoaringPlayerResolver.LookupUser(platformUserID, Soaring.PreferedDeviceLogin, soaringContext2);
				}
			}
			else if (SoaringPlayerResolver.IsSamePlayer(platformUserID, lastUser, ref soaringPlayerData2))
			{
				Debug.LogError("IsSamePlayer");
				SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(soaringPlayerData2, platformUserID, Soaring.PreferedDeviceLogin));
				if (SoaringPlayerResolver.CanCallLogin(soaringPlayerData2))
				{
					Soaring.Login(soaringPlayerData2.soaringTag, soaringPlayerData2.password, soaringContext2);
				}
				else
				{
					SoaringPlayerResolver.LookupUser(platformUserID, Soaring.PreferedDeviceLogin, soaringContext2);
				}
			}
			else
			{
				if (!SoaringInternalProperties.AutoChooseUserPlayer)
				{
					SoaringPlayerResolver.SoaringPlayerData soaringPlayerData3;
					if (soaringPlayerData2 != null)
					{
						soaringPlayerData3 = soaringPlayerData2;
					}
					else
					{
						soaringPlayerData3 = new SoaringPlayerResolver.SoaringPlayerData();
						soaringPlayerData3.soaringTag = (soaringPlayerData3.platformID = platformUserID);
						soaringPlayerData3.loginType = Soaring.PreferedDeviceLogin;
					}
					SoaringPlayerResolver.SoaringPlayerData soaringPlayerData4 = SoaringPlayerResolver.GetUserData(lastUser, true);
					if (soaringPlayerData4 == null)
					{
						soaringPlayerData4 = SoaringPlayerResolver.GetUserData(lastUser, false);
					}
					SoaringPlayerResolver.SoaringPlayerData soaringPlayerData5 = SoaringPlayerResolver.CreateDevicePlayerData();
					if (soaringPlayerData4 == null)
					{
						soaringPlayerData4 = soaringPlayerData5;
					}
					Debug.LogError(soaringPlayerData3.ToJsonString());
					Debug.LogError(soaringPlayerData4.ToJsonString());
					Debug.LogError(soaringPlayerData5.ToJsonString());
					Soaring.Delegate.OnPlayerConflict(new SoaringPlayerResolver(soaringPlayerData3, soaringPlayerData4, soaringPlayerData5), soaringPlayerData3, soaringPlayerData4, soaringPlayerData5, null);
					return false;
				}
				SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(null, platformUserID, Soaring.PreferedDeviceLogin));
				SoaringPlayerResolver.LookupUser(platformUserID, Soaring.PreferedDeviceLogin, soaringContext2);
			}
		}
		else
		{
			soaringContext2.Responder = new SoaringPlayerResolver();
			if (string.IsNullOrEmpty(lastUser))
			{
				SoaringDebug.Log("SoaringPlayerResolve: Warning: No Valid Last User", LogType.Warning);
				SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(null, SoaringPlatform.DeviceID, SoaringLoginType.Device));
				SoaringPlayerResolver.LookupUser(SoaringPlatform.DeviceID, SoaringLoginType.Device, soaringContext2);
			}
			else
			{
				bool flag2 = false;
				if (SoaringInternalProperties.AutoChooseUserPlayer)
				{
					SoaringPlayerResolver.SoaringPlayerData userData = SoaringPlayerResolver.GetUserData(lastUser, false);
					SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(userData, SoaringPlatform.DeviceID, SoaringLoginType.Device));
					if (SoaringPlayerResolver.CanCallLogin(userData))
					{
						Soaring.Login(userData.soaringTag, userData.password, soaringContext2);
					}
					else
					{
						flag2 = true;
					}
				}
				else
				{
					SoaringPlayerResolver.SoaringPlayerData userData2 = SoaringPlayerResolver.GetUserData(lastUser, false);
					if (userData2 == null)
					{
						flag2 = true;
						SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(userData2, SoaringPlatform.DeviceID, SoaringLoginType.Device));
					}
					else
					{
						Debug.LogError(userData2.ToJsonString());
						if (userData2.platformID == SoaringPlatform.DeviceID)
						{
							SoaringDebug.Log("lastUser == SoaringPlatform.DeviceID", LogType.Error);
							SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(userData2, SoaringPlatform.DeviceID, SoaringLoginType.Device));
							if (SoaringPlayerResolver.CanCallLogin(userData2))
							{
								Soaring.Login(userData2.soaringTag, userData2.password, soaringContext2);
							}
							else
							{
								flag2 = true;
							}
						}
						else if (userData2.loginType == SoaringLoginType.Device || userData2.loginType == SoaringLoginType.Soaring)
						{
							SoaringDebug.Log("lastUser == SoaringLoginType." + userData2.loginType.ToString(), LogType.Error);
							SoaringPlayerResolver.SetContextData(soaringContext2, SoaringPlayerResolver.NullPlayerDataResolver(userData2, SoaringPlatform.DeviceID, SoaringLoginType.Device));
							if (SoaringPlayerResolver.CanCallLogin(userData2))
							{
								Soaring.Login(userData2.soaringTag, userData2.password, soaringContext2);
							}
							else
							{
								SoaringPlayerResolver.LookupUserWithTag(userData2.soaringTag, userData2.userID, soaringContext2);
							}
						}
						else
						{
							SoaringDebug.Log("lastUser != SoaringPlatform.DeviceID: " + userData2.loginType.ToString(), LogType.Error);
							SoaringPlayerResolver.SoaringPlayerData soaringPlayerData6 = SoaringPlayerResolver.GetUserData(SoaringPlatform.DeviceID, true);
							if (soaringPlayerData6 == null)
							{
								soaringPlayerData6 = SoaringPlayerResolver.CreateDevicePlayerData();
							}
							Soaring.Delegate.OnPlayerConflict(new SoaringPlayerResolver(null, userData2, soaringPlayerData6), null, userData2, soaringPlayerData6, null);
						}
					}
				}
				if (flag2)
				{
					if (lastUser == null)
					{
						lastUser = "Unknown Last user";
					}
					SoaringDebug.Log("SoaringPlayerResolve: Error: No Valid Last User: " + lastUser, LogType.Error);
					SoaringPlayerResolver.LookupUser(SoaringPlatform.DeviceID, SoaringLoginType.Device, soaringContext2);
				}
			}
		}
		return true;
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x000B6A88 File Offset: 0x000B4C88
	public static void Save(string lastUser = null)
	{
		if (SoaringPlayerResolver.sUserArray == null)
		{
			return;
		}
		try
		{
			string writePath = ResourceUtils.GetWritePath("SoaringUsers.dat", "Soaring", 1);
			MBinaryWriter mbinaryWriter = new MBinaryWriter();
			if (!mbinaryWriter.Open(writePath, true))
			{
				SoaringDebug.Log("Failed To Save Users Data: LU: " + lastUser, LogType.Error);
			}
			else
			{
				string val = "0";
				string text = lastUser;
				if (string.IsNullOrEmpty(text))
				{
					text = Soaring.Player.UserTag;
				}
				if (string.IsNullOrEmpty(text))
				{
					text = string.Empty;
				}
				mbinaryWriter.Write(val);
				mbinaryWriter.Write(text);
				SoaringDictionary soaringDictionary = new SoaringDictionary(1);
				soaringDictionary.addValue(SoaringPlayerResolver.sUserArray, "0");
				string text2 = soaringDictionary.ToJsonString();
				byte[] array = new byte[text2.Length];
				for (int i = 0; i < text2.Length; i++)
				{
					array[i] = (byte)text2[i];
				}
				text2 = Convert.ToBase64String(array);
				mbinaryWriter.Write(text2);
				mbinaryWriter.Close();
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Failed To Save Users Data: " + ex.Message + "\n" + ex.StackTrace, LogType.Error);
		}
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x000B6BD8 File Offset: 0x000B4DD8
	private static bool IsSamePlayer(string platformUserID, string lastUserTag, ref SoaringPlayerResolver.SoaringPlayerData userData)
	{
		userData = null;
		if (SoaringPlayerResolver.sUserArray == null)
		{
			return false;
		}
		int num = SoaringPlayerResolver.sUserArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)SoaringPlayerResolver.sUserArray.objectAtIndex(i);
			if (soaringPlayerData != null)
			{
				Debug.Log(string.Concat(new string[]
				{
					soaringPlayerData.ToJsonString(),
					" vs ",
					platformUserID,
					" : ",
					lastUserTag
				}));
				if (!(lastUserTag != soaringPlayerData.soaringTag) || !(lastUserTag != soaringPlayerData.platformID))
				{
					if (soaringPlayerData.platformID == platformUserID)
					{
						userData = soaringPlayerData;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x000B6C9C File Offset: 0x000B4E9C
	private static SoaringPlayerResolver.SoaringPlayerData ExtractPlayerData(SoaringDictionary userData)
	{
		if (userData == null)
		{
			return null;
		}
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = new SoaringPlayerResolver.SoaringPlayerData();
		soaringPlayerData.soaringTag = userData.soaringValue("0");
		soaringPlayerData.platformID = userData.soaringValue("1");
		soaringPlayerData.password = userData.soaringValue("2");
		soaringPlayerData.userID = userData.soaringValue("4");
		string userID = userData.soaringValue("3");
		soaringPlayerData.loginType = SoaringInternal.PlatformKeyAbriviationWithTag(userID);
		return soaringPlayerData;
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x000B6D30 File Offset: 0x000B4F30
	public static SoaringPlayerResolver.SoaringPlayerData CreateDevicePlayerData()
	{
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = SoaringPlayerResolver.GetUserData(SoaringPlatform.DeviceID, false);
		if (soaringPlayerData == null)
		{
			soaringPlayerData = SoaringPlayerResolver.GetUserData(SoaringPlatform.DeviceID, true);
		}
		if (soaringPlayerData == null)
		{
			soaringPlayerData = new SoaringPlayerResolver.SoaringPlayerData();
			soaringPlayerData.loginType = SoaringLoginType.Device;
			soaringPlayerData.soaringTag = (soaringPlayerData.platformID = SoaringPlatform.DeviceID);
		}
		return soaringPlayerData;
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x000B6D84 File Offset: 0x000B4F84
	private static SoaringPlayerResolver.SoaringPlayerData GetUserData(string userID, bool checkPlatformID)
	{
		if (SoaringPlayerResolver.sUserArray == null || string.IsNullOrEmpty(userID))
		{
			return null;
		}
		int num = SoaringPlayerResolver.sUserArray.count();
		SoaringPlayerResolver.SoaringPlayerData result = null;
		for (int i = 0; i < num; i++)
		{
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)SoaringPlayerResolver.sUserArray.objectAtIndex(i);
			if (soaringPlayerData != null)
			{
				if (checkPlatformID)
				{
					if (userID != soaringPlayerData.platformID)
					{
						goto IL_85;
					}
				}
				else if (userID != soaringPlayerData.soaringTag)
				{
					goto IL_85;
				}
				result = soaringPlayerData;
				break;
			}
			IL_85:;
		}
		return result;
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x000B6E24 File Offset: 0x000B5024
	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		Debug.LogWarning(base.GetType().Name + ": OnComponentFinished: \n" + data.ToJsonString());
		if (string.IsNullOrEmpty(module))
		{
			return;
		}
		if (module.Equals("login"))
		{
			if (this.RetrieveID)
			{
				if (data != null)
				{
					string text = data.soaringValue("id");
					string userAlias = data.soaringValue("name");
					SoaringPlatform.SetPlatformUserData(text, userAlias);
					context.addValue(text, "platform_id");
				}
				SoaringPlayerResolver.FindLoginIDReturn(context);
			}
			else
			{
				string lastUser = context.soaringValue("last_user");
				if (data == null)
				{
					SoaringPlayerResolver.LoadPart2(null, null, lastUser);
				}
				else
				{
					string text2 = data.soaringValue("id");
					string text3 = data.soaringValue("name");
					SoaringPlatform.SetPlatformUserData(text2, text3);
					SoaringPlayerResolver.LoadPart2(text2, text3, lastUser);
				}
			}
		}
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x000B6F28 File Offset: 0x000B5128
	public bool BadConnection(SoaringError error)
	{
		return error != null && (error.ErrorCode == -6 || error.ErrorCode == -10 || error.ErrorCode == -8 || error.ErrorCode == -9 || error.ErrorCode == -7);
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x000B6F80 File Offset: 0x000B5180
	public static void RemovePlayer(SoaringPlayerResolver.SoaringPlayerData data)
	{
		if (data == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(data.soaringTag))
		{
			return;
		}
		for (int i = 0; i < SoaringPlayerResolver.sUserArray.count(); i++)
		{
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)SoaringPlayerResolver.sUserArray.objectAtIndex(i);
			if (soaringPlayerData.soaringTag == data.soaringTag)
			{
				SoaringDebug.Log("REMOVING PLAYER ID: " + soaringPlayerData.ToJsonString() + " With: " + data.ToJsonString(), LogType.Error);
				SoaringPlayerResolver.sUserArray.removeObjectAtIndex(i);
				i--;
			}
		}
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x000B7018 File Offset: 0x000B5218
	private static void UpdateSaveData(SoaringPlayerResolver.SoaringPlayerData data)
	{
		if (data == null)
		{
			return;
		}
		data.soaringTag = Soaring.Player.UserTag;
		data.userID = Soaring.Player.UserID;
		if (string.IsNullOrEmpty(data.soaringTag) || string.IsNullOrEmpty(data.userID))
		{
			return;
		}
		data.password = Soaring.Player.Password;
		for (int i = 0; i < SoaringPlayerResolver.sUserArray.count(); i++)
		{
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)SoaringPlayerResolver.sUserArray.objectAtIndex(i);
			if (soaringPlayerData.userID == data.userID)
			{
				if (soaringPlayerData.soaringTag != data.soaringTag || soaringPlayerData.platformID != data.platformID || soaringPlayerData.password != data.password)
				{
					SoaringDebug.Log("UPDATING PLAYER ID: " + soaringPlayerData.ToJsonString() + " TO: " + data.ToJsonString(), LogType.Error);
					SoaringPlayerResolver.sUserArray.setObjectAtIndex(data, i);
					SoaringPlayerResolver.Save(null);
				}
				return;
			}
		}
		SoaringPlayerResolver.sUserArray.addObject(data);
		SoaringPlayerResolver.Save(null);
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x000B7148 File Offset: 0x000B5348
	public override void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
		if (error != null)
		{
			success = false;
		}
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = null;
		if (context != null)
		{
			soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)context.objectWithKey("user_data");
		}
		if (!success)
		{
			if (this.BadConnection(error))
			{
				SoaringInternal.instance.TriggerOfflineMode(true);
				this.OnAuthorize(success, error, Soaring.Player, context);
			}
			else if (SoaringPlayerResolver.CanCallLogin(soaringPlayerData))
			{
				Soaring.Login(soaringPlayerData.soaringTag, soaringPlayerData.password, context);
			}
			else
			{
				Soaring.GenerateUniqueNewUserName(context);
			}
		}
		Debug.LogError(soaringPlayerData.ToJsonString());
		Soaring.Delegate.OnLookupUser(success, error, null);
		if (success)
		{
			this.OnAuthorize(success, error, Soaring.Player, context);
		}
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x000B71FC File Offset: 0x000B53FC
	public override void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
		if (error != null || string.IsNullOrEmpty(nextTag))
		{
			success = false;
		}
		Soaring.Delegate.OnGenerateUserName(success, error, nextTag, null);
		if (!success)
		{
			this.OnAuthorize(success, error, Soaring.Player, context);
		}
		else
		{
			SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = null;
			if (context != null)
			{
				soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)context.objectWithKey("user_data");
			}
			Soaring.RegisterLiteUser(nextTag, soaringPlayerData.platformID, soaringPlayerData.loginType, context);
		}
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x000B7274 File Offset: 0x000B5474
	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (error != null)
		{
			success = false;
		}
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = null;
		if (context != null)
		{
			soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)context.objectWithKey("user_data");
		}
		player.LoginType = soaringPlayerData.loginType;
		if (!success)
		{
			Debug.LogError(error.ToJsonString());
			SoaringInternal.instance.TriggerOfflineMode(true);
			if (!string.IsNullOrEmpty(soaringPlayerData.userID) && !string.IsNullOrEmpty(soaringPlayerData.platformID) && !string.IsNullOrEmpty(soaringPlayerData.soaringTag))
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(soaringPlayerData.userID, "userId");
				soaringDictionary.addValue(soaringPlayerData.soaringTag, "tag");
				Soaring.Player.SetUserData(soaringDictionary);
				Debug.LogError(soaringDictionary.ToJsonString());
			}
		}
		else
		{
			SoaringPlayerResolver.UpdateSaveData(soaringPlayerData);
		}
		Soaring.Delegate.OnAuthorize(success, error, player, null);
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x000B7360 File Offset: 0x000B5560
	public override void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (!success)
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
		}
		Soaring.Delegate.OnRegisterUser(success, error, player, null);
		this.OnAuthorize(success, error, player, context);
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x000B7398 File Offset: 0x000B5598
	public void HandleLoginConflict(SoaringPlayerResolver.SoaringPlayerData playerData, SoaringContext context = null)
	{
		if (playerData == null)
		{
			SoaringDebug.Log("HandleLogingConflict: Error: Null Player Data Selected");
			playerData = SoaringPlayerResolver.CreateDevicePlayerData();
		}
		if (context == null)
		{
			context = new SoaringContext();
			context.Responder = new SoaringPlayerResolver();
		}
		SoaringPlayerResolver.SetContextData(context, SoaringPlayerResolver.NullPlayerDataResolver(playerData, playerData.platformID, playerData.loginType));
		if (SoaringPlayerResolver.CanCallLogin(playerData))
		{
			Soaring.Login(playerData.soaringTag, playerData.password, context);
		}
		else
		{
			SoaringPlayerResolver.LookupUser(playerData.platformID, playerData.loginType, context);
		}
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x000B7420 File Offset: 0x000B5620
	private static void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context)
	{
		SoaringDebug.Log(platformID + " : " + loginType.ToString());
		Soaring.LookupUser(platformID, loginType, context);
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x000B7448 File Offset: 0x000B5648
	private static void LookupUserWithTag(string userTag, string userID, SoaringContext context)
	{
		SoaringDebug.Log(userTag + " : Tag");
		SoaringArray soaringArray = new SoaringArray();
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringArray.addObject(soaringDictionary);
		soaringDictionary.addValue(userTag, "tag");
		Soaring.LookupUser(soaringArray, context);
	}

	// Token: 0x04001269 RID: 4713
	public const string Soaring_LastUser_Key = "last_user";

	// Token: 0x0400126A RID: 4714
	public const string Soaring_PlatformUser_Key = "platform_id";

	// Token: 0x0400126B RID: 4715
	public const string Soaring_ExternalLogin_Key = "login";

	// Token: 0x0400126C RID: 4716
	private const string SoaringSoaringTagKey = "0";

	// Token: 0x0400126D RID: 4717
	private const string SoaringUserPlatformKey = "1";

	// Token: 0x0400126E RID: 4718
	private const string SoaringUserPasswordKey = "2";

	// Token: 0x0400126F RID: 4719
	private const string SoaringLoginTypeKey = "3";

	// Token: 0x04001270 RID: 4720
	private const string SoaringUserIDKey = "4";

	// Token: 0x04001271 RID: 4721
	private static SoaringArray sUserArray;

	// Token: 0x04001272 RID: 4722
	private static string sProperties;

	// Token: 0x04001273 RID: 4723
	public SoaringPlayerResolver.SoaringPlayerData ResolvePlatformData;

	// Token: 0x04001274 RID: 4724
	public SoaringPlayerResolver.SoaringPlayerData ResolveLastUserData;

	// Token: 0x04001275 RID: 4725
	public SoaringPlayerResolver.SoaringPlayerData ResolveDeviceData;

	// Token: 0x04001276 RID: 4726
	public bool RetrieveID;

	// Token: 0x020003C7 RID: 967
	public class SoaringPlayerData : SoaringObjectBase
	{
		// Token: 0x06001CE4 RID: 7396 RVA: 0x000B7490 File Offset: 0x000B5690
		public SoaringPlayerData() : base(SoaringObjectBase.IsType.Object)
		{
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000B749C File Offset: 0x000B569C
		public string playerAlias
		{
			get
			{
				if (this.loginType != SoaringLoginType.Soaring && this.loginType != SoaringLoginType.Device)
				{
					return SoaringPlatform.PlatformUserAlias;
				}
				if (this.soaringTag == null)
				{
					return SoaringPlatform.PlatformUserID;
				}
				return this.soaringTag;
			}
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000B74E0 File Offset: 0x000B56E0
		public override string ToJsonString()
		{
			return string.Concat(new string[]
			{
				"{\"0\":\"",
				this.soaringTag,
				"\",\"1\":\"",
				(this.platformID != null) ? this.platformID : string.Empty,
				"\",\"2\":\"",
				this.password,
				"\",\"4\":\"",
				this.userID,
				"\",\"3\":\"",
				SoaringInternal.PlatformKeyAbriviationWithLoginType(this.loginType, true),
				"\"}"
			});
		}

		// Token: 0x04001277 RID: 4727
		public string soaringTag;

		// Token: 0x04001278 RID: 4728
		public string platformID;

		// Token: 0x04001279 RID: 4729
		public string password;

		// Token: 0x0400127A RID: 4730
		public string userID;

		// Token: 0x0400127B RID: 4731
		public SoaringLoginType loginType;
	}
}
