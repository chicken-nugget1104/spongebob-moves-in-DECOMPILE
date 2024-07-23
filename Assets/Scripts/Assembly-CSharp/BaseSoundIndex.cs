using System;

// Token: 0x02000449 RID: 1097
public abstract class BaseSoundIndex : ISoundIndex
{
	// Token: 0x060021F8 RID: 8696 RVA: 0x000D15EC File Offset: 0x000CF7EC
	public BaseSoundIndex(string key, int maxInstances)
	{
		this.key = key;
		this.maxInstances = maxInstances;
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x060021F9 RID: 8697 RVA: 0x000D1604 File Offset: 0x000CF804
	public string Key
	{
		get
		{
			return this.key;
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x060021FA RID: 8698 RVA: 0x000D160C File Offset: 0x000CF80C
	public int MaxInstances
	{
		get
		{
			return this.maxInstances;
		}
	}

	// Token: 0x060021FB RID: 8699
	public abstract TFSound GetNextSound(SoundEffectManager sfxMgr);

	// Token: 0x060021FC RID: 8700
	public abstract void Clear();

	// Token: 0x060021FD RID: 8701
	public abstract void Init();

	// Token: 0x04001507 RID: 5383
	private string key;

	// Token: 0x04001508 RID: 5384
	private int maxInstances;
}
