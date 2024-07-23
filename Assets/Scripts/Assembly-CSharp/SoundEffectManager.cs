using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class SoundEffectManager
{
	// Token: 0x0600220A RID: 8714 RVA: 0x000D17C4 File Offset: 0x000CF9C4
	public SoundEffectManager(Dictionary<string, ISoundIndex> sounds)
	{
		this.audioSourcePool = TFPool<GameObject>.CreatePool(6, new Alloc<GameObject>(SoundEffectManager.CreateAudioSource));
		this.cleanupList = new List<GameObject>(6);
		this.characterSet = new HashSet<string>();
		this.soundInstances = new Dictionary<string, int>();
		this.sounds = sounds;
		this.enabled = false;
		TFUtils.Assert(this.sounds.ContainsKey("Silence"), "You should include a sound named Silence in this file to use as the default sound!");
		this.defaultSound = this.sounds["Silence"];
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x000D1884 File Offset: 0x000CFA84
	public static GameObject CreateAudioSource()
	{
		GameObject gameObject = new GameObject("AudioSource_" + SoundEffectManager.sAudioSourceID++);
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		return gameObject;
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x0600220D RID: 8717 RVA: 0x000D18C4 File Offset: 0x000CFAC4
	// (set) Token: 0x0600220E RID: 8718 RVA: 0x000D18CC File Offset: 0x000CFACC
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
			PlayerPrefs.SetInt(SoundEffectManager.SOUND_ENABLED, value2);
			PlayerPrefs.Save();
		}
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x000D1904 File Offset: 0x000CFB04
	public static SoundEffectManager CreateSoundEffectManager()
	{
		if (SoundEffectManager.soundEffectManager == null)
		{
			SoundEffectManager.soundEffectManager = SoundEffectManager.CreateSoundEffectManagerFromSpread();
			if (SoundEffectManager.soundEffectManager != null)
			{
				return SoundEffectManager.soundEffectManager;
			}
			Debug.Log("Loading " + SoundEffectManager.SOUND_FILE + "...");
			string text = TFUtils.ReadAllText(SoundEffectManager.SOUND_FILE);
			TFUtils.Assert(!string.IsNullOrEmpty(text), "Empty file for sound data - SoundEffects.json.");
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(text);
			TFUtils.Assert(dictionary != null, "Invalid json data for SoundEffects.json.");
			Dictionary<string, ISoundIndex> dictionary2 = new Dictionary<string, ISoundIndex>();
			foreach (object obj in (dictionary["sound_effects"] as List<object>))
			{
				Dictionary<string, object> data = (Dictionary<string, object>)obj;
				ISoundIndex soundIndex = SoundIndexFactory.FromDict(data);
				if (dictionary2.ContainsKey(soundIndex.Key))
				{
					TFUtils.Assert(false, "Found duplicate entry for sound entry '" + soundIndex.Key + "'!");
				}
				else
				{
					dictionary2.Add(soundIndex.Key, soundIndex);
				}
			}
			SoundEffectManager.soundEffectManager = new SoundEffectManager(dictionary2);
		}
		return SoundEffectManager.soundEffectManager;
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000D1A4C File Offset: 0x000CFC4C
	private static SoundEffectManager CreateSoundEffectManagerFromSpread()
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
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, ISoundIndex> dictionary2 = new Dictionary<string, ISoundIndex>();
		string b = "n/a";
		int num2 = -1;
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
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "set sounds");
				}
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "type");
				if (!(stringCell != "sound_effects"))
				{
					dictionary.Clear();
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "file");
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						dictionary.Add("file", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "character");
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						dictionary.Add("character", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "set sound 1");
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						dictionary.Add("set", new List<string>
						{
							stringCell
						});
						for (int j = 2; j <= num2; j++)
						{
							stringCell = instance.GetStringCell(sheetIndex, rowIndex, "set sound " + j.ToString());
							if (string.IsNullOrEmpty(stringCell) || stringCell == b)
							{
								break;
							}
							((List<string>)dictionary["set"]).Add(stringCell);
						}
					}
					ISoundIndex soundIndex = SoundIndexFactory.FromDict(dictionary);
					if (dictionary2.ContainsKey(soundIndex.Key))
					{
						TFUtils.Assert(false, "Found duplicate entry for sound entry '" + soundIndex.Key + "'!");
					}
					else
					{
						dictionary2.Add(soundIndex.Key, soundIndex);
					}
				}
			}
		}
		return new SoundEffectManager(dictionary2);
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000D1CF4 File Offset: 0x000CFEF4
	public ISoundIndex GetSoundIndex(string key)
	{
		ISoundIndex result = null;
		if (!this.sounds.TryGetValue(key, out result))
		{
			TFUtils.ErrorLog("Could not find sound effect with key " + key);
			return this.defaultSound;
		}
		return result;
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000D1D30 File Offset: 0x000CFF30
	public void Clear()
	{
		foreach (ISoundIndex soundIndex in this.sounds.Values)
		{
			soundIndex.Clear();
		}
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000D1D9C File Offset: 0x000CFF9C
	public void InitAudio()
	{
		foreach (ISoundIndex soundIndex in this.sounds.Values)
		{
			soundIndex.Init();
		}
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000D1E08 File Offset: 0x000D0008
	private void CleanUpFinishedAudio()
	{
		this.cleanupList.Clear();
		foreach (GameObject gameObject in this.audioSourcePool.ActiveSet)
		{
			if (!gameObject.audio.isPlaying)
			{
				this.cleanupList.Add(gameObject);
			}
		}
		foreach (GameObject gameObject2 in this.cleanupList)
		{
			if (gameObject2.audio.name != null)
			{
				this.characterSet.Remove(gameObject2.audio.name);
			}
			if (this.soundInstances.ContainsKey(gameObject2.name))
			{
				Dictionary<string, int> dictionary2;
				Dictionary<string, int> dictionary = dictionary2 = this.soundInstances;
				string name;
				string key = name = gameObject2.name;
				int num = dictionary2[name];
				dictionary[key] = num - 1;
			}
			this.audioSourcePool.Release(gameObject2);
		}
		this.soundInstances.Clear();
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000D1F5C File Offset: 0x000D015C
	public void StartSoundEffectsManager()
	{
		if (PlayerPrefs.HasKey(SoundEffectManager.SOUND_ENABLED))
		{
			bool flag = PlayerPrefs.GetInt(SoundEffectManager.SOUND_ENABLED) == 1;
			this.enabled = flag;
		}
		else
		{
			this.Enabled = true;
		}
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000D1FA4 File Offset: 0x000D01A4
	public AudioSource PlaySound(string soundId)
	{
		return this.PlaySound(soundId, 0f);
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x000D1FB4 File Offset: 0x000D01B4
	public AudioSource PlaySound(string soundId, float delaySeconds)
	{
		if (!this.Enabled || soundId == null)
		{
			return null;
		}
		if (!this.sounds.ContainsKey(soundId))
		{
			TFUtils.ErrorLog("Cannot find sound effect: " + soundId);
			return null;
		}
		this.CleanUpFinishedAudio();
		BaseSoundIndex baseSoundIndex = this.sounds[soundId] as BaseSoundIndex;
		if (baseSoundIndex != null)
		{
			int num = 0;
			if (this.soundInstances.TryGetValue(soundId, out num) && num >= baseSoundIndex.MaxInstances)
			{
				return null;
			}
		}
		GameObject gameObject = this.audioSourcePool.Create(new Alloc<GameObject>(SoundEffectManager.CreateAudioSource));
		gameObject.name = soundId;
		TFSound nextSound = this.sounds[soundId].GetNextSound(this);
		if (nextSound.clip == null)
		{
			nextSound.Init();
			if (nextSound.clip == null)
			{
				TFUtils.ErrorLog("Sound clip " + soundId + " did not initialize properly (Clip should not be null).\nFilename=" + nextSound.fileName);
				this.audioSourcePool.Release(gameObject);
				return null;
			}
		}
		if (nextSound.characterName != null)
		{
			if (this.characterSet.Contains(nextSound.characterName))
			{
				TFUtils.DebugLog("Character already has VO: " + soundId);
				this.audioSourcePool.Release(gameObject);
				return null;
			}
			this.characterSet.Add(nextSound.characterName);
		}
		gameObject.audio.name = nextSound.characterName;
		gameObject.audio.clip = nextSound.clip;
		gameObject.audio.volume = 1f;
		int frequency = gameObject.audio.clip.frequency;
		ulong num2 = (ulong)(delaySeconds * (float)frequency);
		if (num2 != 0UL)
		{
			gameObject.audio.PlayDelayed((ulong)(delaySeconds * (float)frequency));
		}
		else
		{
			gameObject.audio.Play();
		}
		if (this.soundInstances.ContainsKey(soundId))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = dictionary2 = this.soundInstances;
			int num3 = dictionary2[soundId];
			dictionary[soundId] = num3 + 1;
		}
		else
		{
			this.soundInstances.Add(soundId, 1);
		}
		return gameObject.audio;
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000D21D0 File Offset: 0x000D03D0
	public void ToggleOnOff()
	{
		this.Enabled = !this.Enabled;
	}

	// Token: 0x0400150B RID: 5387
	public const int START_POOL_SIZE = 6;

	// Token: 0x0400150C RID: 5388
	public static int sAudioSourceID = 0;

	// Token: 0x0400150D RID: 5389
	public static string SOUND_ENABLED = "sound_enabled";

	// Token: 0x0400150E RID: 5390
	public static SoundEffectManager soundEffectManager;

	// Token: 0x0400150F RID: 5391
	private static string SOUND_FILE = TFUtils.GetStreamingAssetsFileInDirectory("Sound", "SoundEffects.json");

	// Token: 0x04001510 RID: 5392
	private TFPool<GameObject> audioSourcePool;

	// Token: 0x04001511 RID: 5393
	private List<GameObject> cleanupList;

	// Token: 0x04001512 RID: 5394
	private Dictionary<string, ISoundIndex> sounds;

	// Token: 0x04001513 RID: 5395
	private Dictionary<string, int> soundInstances;

	// Token: 0x04001514 RID: 5396
	private HashSet<string> characterSet;

	// Token: 0x04001515 RID: 5397
	private bool enabled;

	// Token: 0x04001516 RID: 5398
	private ISoundIndex defaultSound;
}
