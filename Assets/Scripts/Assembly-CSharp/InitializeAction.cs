using System;
using System.Collections.Generic;

// Token: 0x020000DA RID: 218
public class InitializeAction : PersistedActionBuffer.PersistedAction
{
	// Token: 0x0600081C RID: 2076 RVA: 0x00034898 File Offset: 0x00032A98
	public InitializeAction() : base("i", Identity.Null())
	{
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x000348AC File Offset: 0x00032AAC
	public new static InitializeAction FromDict(Dictionary<string, object> data)
	{
		return new InitializeAction();
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x000348C0 File Offset: 0x00032AC0
	public override void Process(Game game)
	{
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x000348C4 File Offset: 0x00032AC4
	public override void Apply(Game game, ulong utcNow)
	{
	}

	// Token: 0x040005E6 RID: 1510
	public const string INITIALIZE = "i";
}
