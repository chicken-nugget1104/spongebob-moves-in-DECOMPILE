using System;
using System.IO;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class SBWebFileServer
{
	// Token: 0x06000345 RID: 837 RVA: 0x000106B4 File Offset: 0x0000E8B4
	public void SetPlayerInfo(Player player)
	{
		this.eTagFile = player.CacheFile("lastETag");
	}

	// Token: 0x06000346 RID: 838 RVA: 0x000106C8 File Offset: 0x0000E8C8
	public void GetGameData(string gameID, long timestamp, SoaringContext context)
	{
		Soaring.RequestSessionData("game", timestamp, context);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x000106D8 File Offset: 0x0000E8D8
	public void DeleteGameData(Session session = null)
	{
		if (session == null)
		{
			return;
		}
		SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(new SoaringContextDelegate(this.HandleGameReset));
		soaringContext.addValue(new SoaringObject(session), "session");
		session.TheGame.CanSave = false;
		TextAsset textAsset = (TextAsset)Resources.Load("Default/DefaultPark");
		if (textAsset == null)
		{
			return;
		}
		SoaringDictionary gameData = new SoaringDictionary(textAsset.text);
		Resources.UnloadAsset(textAsset);
		this.SaveGameData(gameData, soaringContext);
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00010754 File Offset: 0x0000E954
	private void HandleGameReset(SoaringContext context)
	{
		if (context == null)
		{
			return;
		}
		SoaringObject soaringObject = (SoaringObject)context.objectWithKey("session");
		if (soaringObject == null)
		{
			return;
		}
		Session session = (Session)soaringObject.Object;
		session.ThePlayer.DeleteTimestamp();
		session.reinitializeSession = true;
		session.ChangeState("Sync", true);
	}

	// Token: 0x06000349 RID: 841 RVA: 0x000107AC File Offset: 0x0000E9AC
	public void SaveGameData(string gameData, SoaringContext context)
	{
		Soaring.SendSessionData("game", SoaringSession.SessionType.PersistantOneWay, new SoaringDictionary(gameData), context);
	}

	// Token: 0x0600034A RID: 842 RVA: 0x000107C0 File Offset: 0x0000E9C0
	public void SaveGameData(SoaringDictionary gameData, SoaringContext context)
	{
		Soaring.SendSessionData("game", SoaringSession.SessionType.PersistantOneWay, gameData, context);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x000107D0 File Offset: 0x0000E9D0
	public string ReadETag()
	{
		string obj = this.eTagFile;
		string result;
		lock (obj)
		{
			if (TFUtils.FileIsExists(this.eTagFile))
			{
				result = TFUtils.ReadAllText(this.eTagFile);
			}
			else
			{
				result = null;
			}
		}
		return result;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0001083C File Offset: 0x0000EA3C
	public void DeleteETagFile()
	{
		string obj = this.eTagFile;
		lock (obj)
		{
			TFUtils.WarningLog("DeleteETagFile");
			if (TFUtils.FileIsExists(this.eTagFile))
			{
				File.Delete(this.eTagFile);
			}
		}
	}

	// Token: 0x04000232 RID: 562
	private string eTagFile;

	// Token: 0x04000233 RID: 563
	public static DateTime LastSuccessfulSave;
}
