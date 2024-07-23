using System;

// Token: 0x0200044B RID: 1099
public class SingleSound : BaseSoundIndex
{
	// Token: 0x06002202 RID: 8706 RVA: 0x000D1614 File Offset: 0x000CF814
	public SingleSound(string key, int maxInstances, string filename, string characterName) : base(key, maxInstances)
	{
		this.sound = new TFSound(filename, characterName);
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x000D162C File Offset: 0x000CF82C
	public override TFSound GetNextSound(SoundEffectManager sfxMgr)
	{
		return this.sound;
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x000D1634 File Offset: 0x000CF834
	public override void Clear()
	{
		this.sound.Clear();
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x000D1644 File Offset: 0x000CF844
	public override void Init()
	{
		this.sound.Init();
	}

	// Token: 0x04001509 RID: 5385
	private TFSound sound;
}
