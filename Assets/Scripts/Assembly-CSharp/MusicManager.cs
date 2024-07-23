using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x02000423 RID: 1059
public class MusicManager
{
	// Token: 0x060020B6 RID: 8374 RVA: 0x000CA4C0 File Offset: 0x000C86C0
	public MusicManager(Dictionary<string, string> tracks)
	{
		this.tracks = tracks;
		if (PlayerPrefs.HasKey(MusicManager.MUSIC_ENABLED))
		{
			bool flag = PlayerPrefs.GetInt(MusicManager.MUSIC_ENABLED) == 1;
			this.enabled = flag;
		}
		else
		{
			this.Enabled = true;
		}
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x060020B8 RID: 8376 RVA: 0x000CA534 File Offset: 0x000C8734
	// (set) Token: 0x060020B9 RID: 8377 RVA: 0x000CA53C File Offset: 0x000C873C
	public bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			this.enabled = value;
			int value2 = (!this.enabled) ? 0 : 1;
			PlayerPrefs.SetInt(MusicManager.MUSIC_ENABLED, value2);
			PlayerPrefs.Save();
		}
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x000CA574 File Offset: 0x000C8774
	public static MusicManager CreateMusicManager()
	{
		MusicManager musicManager = MusicManager.CreateMusicManagerFromSpread();
		if (musicManager != null)
		{
			return musicManager;
		}
		string text = TFUtils.ReadAllText(MusicManager.MUSIC_FILE);
		TFUtils.Assert(!string.IsNullOrEmpty(text), "Empty file for music data - Music.json.");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(text);
		TFUtils.Assert(dictionary != null, "Invalid json data for Music.json.");
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		foreach (object obj in (dictionary["music"] as List<object>))
		{
			Dictionary<string, object> dictionary3 = (Dictionary<string, object>)obj;
			string key = (string)dictionary3["name"];
			string value = (string)dictionary3["file"];
			dictionary2.Add(key, value);
		}
		return new MusicManager(dictionary2);
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x000CA66C File Offset: 0x000C886C
	public void PlayTrack(string trackName)
	{
		if (!this.Enabled)
		{
			return;
		}
		if (this.currentTrackName == trackName)
		{
			return;
		}
		string text = this.tracks[trackName];
		TFUtils.DebugLog(string.Concat(new string[]
		{
			"MusicManager: Playing track(",
			trackName,
			"), file(",
			text,
			")"
		}));
		if (this.currentMusicGo != null)
		{
			UnityEngine.Object.Destroy(this.currentMusicGo);
		}
		this.currentMusicGo = new GameObject("CurrentMusicGo");
		this.currentMusicGo.AddComponent(typeof(AudioSource));
		AudioClip audioClip = (AudioClip)Resources.Load(text);
		TFUtils.Assert(audioClip != null, "Could not find file " + text);
		this.currentMusicGo.audio.loop = true;
		this.currentMusicGo.audio.clip = audioClip;
		this.currentMusicGo.audio.Play();
		this.currentTrackName = trackName;
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x000CA774 File Offset: 0x000C8974
	private void PlayCurrentTrack()
	{
		if (this.currentMusicGo != null)
		{
			this.currentMusicGo.audio.Play();
		}
		else
		{
			this.PlayTrack("InGame");
		}
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x000CA7A8 File Offset: 0x000C89A8
	public void StopTrack()
	{
		if (this.currentMusicGo != null)
		{
			this.currentMusicGo.audio.Stop();
		}
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x000CA7CC File Offset: 0x000C89CC
	public void ToggleOnOff()
	{
		if (this.Enabled)
		{
			this.StopTrack();
			this.Enabled = !this.Enabled;
		}
		else
		{
			this.Enabled = !this.Enabled;
			this.PlayCurrentTrack();
		}
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x000CA814 File Offset: 0x000C8A14
	private static MusicManager CreateMusicManagerFromSpread()
	{
		string text = "Sound";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "type");
				if (!(stringCell != "music"))
				{
					dictionary.Add(instance.GetStringCell(sheetIndex, rowIndex, "name"), instance.GetStringCell(sheetIndex, rowIndex, "file"));
				}
			}
		}
		return new MusicManager(dictionary);
	}

	// Token: 0x040013F5 RID: 5109
	public static string MUSIC_ENABLED = "music_enabled";

	// Token: 0x040013F6 RID: 5110
	private static string MUSIC_FILE = TFUtils.GetStreamingAssetsFileInDirectory("Sound", "Music.json");

	// Token: 0x040013F7 RID: 5111
	private Dictionary<string, string> tracks;

	// Token: 0x040013F8 RID: 5112
	private GameObject currentMusicGo;

	// Token: 0x040013F9 RID: 5113
	private AudioClip currentTrack;

	// Token: 0x040013FA RID: 5114
	private bool enabled;

	// Token: 0x040013FB RID: 5115
	private string currentTrackName;
}
