using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044F RID: 1103
public class SoundSet : BaseSoundIndex
{
	// Token: 0x0600221A RID: 8730 RVA: 0x000D22E8 File Offset: 0x000D04E8
	public SoundSet(string thisKey, int maxInstances, List<string> thoseKeys) : base(thisKey, maxInstances)
	{
		this.keys = new List<string>();
		foreach (string item in thoseKeys)
		{
			this.keys.Add(item);
		}
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x000D2364 File Offset: 0x000D0564
	public override TFSound GetNextSound(SoundEffectManager sfxMgr)
	{
		string key = this.keys[UnityEngine.Random.Range(0, this.keys.Count)];
		ISoundIndex soundIndex = sfxMgr.GetSoundIndex(key);
		return soundIndex.GetNextSound(sfxMgr);
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x000D23A0 File Offset: 0x000D05A0
	public override void Clear()
	{
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000D23A4 File Offset: 0x000D05A4
	public override void Init()
	{
	}

	// Token: 0x04001517 RID: 5399
	private List<string> keys;
}
