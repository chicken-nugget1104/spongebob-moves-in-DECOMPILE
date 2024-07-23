using System;
using UnityEngine;

// Token: 0x0200038A RID: 906
internal static class SCQueueTools
{
	// Token: 0x060019CF RID: 6607 RVA: 0x000A9E64 File Offset: 0x000A8064
	public static SoaringError CheckAndHandleError(string data, ref SoaringDictionary parsed_data)
	{
		if (string.IsNullOrEmpty(data))
		{
			return null;
		}
		parsed_data = SCQueueTools.ParseMessage(data);
		if (parsed_data == null)
		{
			return new SoaringError(data, -1);
		}
		string text = parsed_data.soaringValue("error_message");
		if (text == null)
		{
			text = string.Empty;
		}
		SoaringObjectBase soaringObjectBase = parsed_data.soaringValue("error_code");
		if (soaringObjectBase == null && string.IsNullOrEmpty(text))
		{
			return null;
		}
		int code = -1;
		if (soaringObjectBase != null)
		{
			code = (SoaringValue)soaringObjectBase;
		}
		return new SoaringError(text, code);
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000A9EF0 File Offset: 0x000A80F0
	public static SoaringDictionary CreateMessage(string action, string gameID, SoaringDictionary parameters)
	{
		if (parameters == null && string.IsNullOrEmpty(action) && string.IsNullOrEmpty(gameID))
		{
			return null;
		}
		if (parameters == null)
		{
			parameters = new SoaringDictionary(2);
		}
		if (action != null)
		{
			parameters.addValue(action, "name");
		}
		if (gameID != null)
		{
			parameters.addValue(gameID, "gameId");
		}
		string b = "{\n\"action\" : " + parameters.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(b, "data");
		return soaringDictionary;
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000A9F88 File Offset: 0x000A8188
	public static SoaringDictionary CreateJsonDictionary(string header, string action, string gameID, SoaringDictionary parameters)
	{
		string text = null;
		return SCQueueTools.CreateJsonDictionary(header, action, gameID, parameters, ref text);
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000A9FA4 File Offset: 0x000A81A4
	public static SoaringDictionary CreateJsonDictionary(string header, string action, string gameID, SoaringDictionary parameters, ref string message)
	{
		message = SCQueueTools.CreateJsonMessage(header, action, gameID, parameters);
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(message, "data");
		return soaringDictionary;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x000A9FD8 File Offset: 0x000A81D8
	public static string CreateJsonMessage(string header, string action, string gameID, SoaringDictionary parameters)
	{
		if (string.IsNullOrEmpty(header))
		{
			return null;
		}
		if (parameters == null)
		{
			parameters = new SoaringDictionary(2);
		}
		string str = "\"" + header + "\" : ";
		if (action != null)
		{
			parameters.addValue(action, "name");
		}
		if (gameID != null)
		{
			parameters.addValue(gameID, "gameId");
		}
		return str + parameters.ToJsonString();
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x000AA050 File Offset: 0x000A8250
	public static SoaringDictionary ParseMessage(string message)
	{
		SoaringDictionary soaringDictionary = null;
		try
		{
			soaringDictionary = new SoaringDictionary(message);
			if (soaringDictionary.count() == 0)
			{
				soaringDictionary = null;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SCQueueTools: " + ex.Message, LogType.Warning);
			soaringDictionary = null;
		}
		return soaringDictionary;
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x000AA0B4 File Offset: 0x000A82B4
	public static SoaringArray<SoaringUser> ParseUsers(SoaringArray data, bool mixedUsers)
	{
		if (data == null)
		{
			return new SoaringArray<SoaringUser>(0);
		}
		SoaringArray<SoaringUser> soaringArray = new SoaringArray<SoaringUser>(data.count());
		int num = data.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectAtIndex(i);
			if (mixedUsers)
			{
				string text = soaringDictionary.soaringValue("isFriend");
				bool flag = false;
				if (!string.IsNullOrEmpty(text))
				{
					text = text.ToLower();
					if (text == "true")
					{
						flag = true;
					}
				}
				SoaringUser soaringUser;
				if (flag)
				{
					soaringUser = new SoaringFriend();
				}
				else
				{
					soaringUser = new SoaringUser();
				}
				soaringUser.SetUserData(soaringDictionary);
				soaringArray.addObject(soaringUser);
			}
			else
			{
				SoaringUser soaringUser2 = new SoaringFriend();
				soaringUser2.SetUserData(soaringDictionary);
				soaringArray.addObject(soaringUser2);
			}
		}
		return soaringArray;
	}
}
