using System;
using System.Collections.Generic;
using System.IO;
using Prime31;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class GameCenterPlayer
{
	// Token: 0x0600064C RID: 1612 RVA: 0x00016CCC File Offset: 0x00014ECC
	public GameCenterPlayer(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("playerId"))
		{
			this.playerId = (dict["playerId"] as string);
		}
		if (dict.ContainsKey("alias"))
		{
			this.alias = (dict["alias"] as string);
		}
		if (dict.ContainsKey("displayName"))
		{
			this.displayName = (dict["displayName"] as string);
		}
		if (dict.ContainsKey("isFriend"))
		{
			this.isFriend = (bool)dict["isFriend"];
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00016D78 File Offset: 0x00014F78
	public bool hasProfilePhoto
	{
		get
		{
			return File.Exists(this.profilePhotoPath);
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x00016D88 File Offset: 0x00014F88
	public Texture2D profilePhoto
	{
		get
		{
			if (!this.hasProfilePhoto)
			{
				return null;
			}
			byte[] data = File.ReadAllBytes(this.profilePhotoPath);
			Texture2D texture2D = new Texture2D(0, 0);
			if (!texture2D.LoadImage(data))
			{
				return null;
			}
			return texture2D;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x00016DC8 File Offset: 0x00014FC8
	public string profilePhotoPath
	{
		get
		{
			if (this._profilePhotoPath == null)
			{
				this._profilePhotoPath = Path.Combine(Application.persistentDataPath, string.Format("{0}.png", this.playerId).Replace(":", string.Empty));
			}
			return this._profilePhotoPath;
		}
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00016E18 File Offset: 0x00015018
	public static List<GameCenterPlayer> fromJSON(string json)
	{
		List<GameCenterPlayer> list = new List<GameCenterPlayer>();
		List<object> list2 = json.listFromJson();
		foreach (object obj in list2)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			list.Add(new GameCenterPlayer(dict));
		}
		return list;
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00016E94 File Offset: 0x00015094
	public override string ToString()
	{
		return string.Format("<Player> playerId: {0}, alias: {1}, displayName: {2}, isFriend: {3}", new object[]
		{
			this.playerId,
			this.alias,
			this.displayName,
			this.isFriend
		});
	}

	// Token: 0x04000373 RID: 883
	public string playerId;

	// Token: 0x04000374 RID: 884
	public string alias;

	// Token: 0x04000375 RID: 885
	public string displayName;

	// Token: 0x04000376 RID: 886
	public bool isFriend;

	// Token: 0x04000377 RID: 887
	private string _profilePhotoPath;
}
