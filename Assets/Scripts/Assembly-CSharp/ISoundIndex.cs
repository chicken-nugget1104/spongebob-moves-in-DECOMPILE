using System;

// Token: 0x0200044A RID: 1098
public interface ISoundIndex
{
	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x060021FE RID: 8702
	string Key { get; }

	// Token: 0x060021FF RID: 8703
	TFSound GetNextSound(SoundEffectManager sfxMgr);

	// Token: 0x06002200 RID: 8704
	void Clear();

	// Token: 0x06002201 RID: 8705
	void Init();
}
