using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public class SoundArray : BaseSoundIndex
{
	// Token: 0x06002206 RID: 8710 RVA: 0x000D1654 File Offset: 0x000CF854
	public SoundArray(string key, int maxInstances, List<string> filenames, string character) : base(key, maxInstances)
	{
		this.sounds = new List<TFSound>();
		foreach (string file in filenames)
		{
			this.sounds.Add(new TFSound(file, character));
		}
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x000D16D4 File Offset: 0x000CF8D4
	public override TFSound GetNextSound(SoundEffectManager sfxMgr)
	{
		return this.sounds[UnityEngine.Random.Range(0, this.sounds.Count)];
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x000D16F4 File Offset: 0x000CF8F4
	public override void Clear()
	{
		foreach (TFSound tfsound in this.sounds)
		{
			tfsound.Clear();
		}
	}

	// Token: 0x06002209 RID: 8713 RVA: 0x000D175C File Offset: 0x000CF95C
	public override void Init()
	{
		foreach (TFSound tfsound in this.sounds)
		{
			tfsound.Init();
		}
	}

	// Token: 0x0400150A RID: 5386
	private List<TFSound> sounds;
}
