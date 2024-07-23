using System;
using System.Collections.Generic;

// Token: 0x0200018C RID: 396
public class MatchableProperty
{
	// Token: 0x06000D6B RID: 3435 RVA: 0x000523CC File Offset: 0x000505CC
	public MatchableProperty(bool isRequired, string key, object target, MatchableProperty.MatchFn matchFn)
	{
		this.isRequired = isRequired;
		this.key = key;
		this.target = target;
		this.matchFn = matchFn;
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000D6C RID: 3436 RVA: 0x000523F4 File Offset: 0x000505F4
	public bool IsRequired
	{
		get
		{
			return this.isRequired;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000D6D RID: 3437 RVA: 0x000523FC File Offset: 0x000505FC
	public string Key
	{
		get
		{
			return this.key;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00052404 File Offset: 0x00050604
	public object Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x0005240C File Offset: 0x0005060C
	public uint Evaluate(Dictionary<string, object> triggerData, Game game)
	{
		return this.matchFn(this, triggerData, game);
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x0005241C File Offset: 0x0005061C
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"{",
			this.key,
			" (isRequired=",
			this.isRequired,
			", target=",
			this.Target,
			")}"
		});
	}

	// Token: 0x040008EC RID: 2284
	private bool isRequired;

	// Token: 0x040008ED RID: 2285
	private string key;

	// Token: 0x040008EE RID: 2286
	private object target;

	// Token: 0x040008EF RID: 2287
	private MatchableProperty.MatchFn matchFn;

	// Token: 0x020004A5 RID: 1189
	// (Invoke) Token: 0x060024F7 RID: 9463
	public delegate uint MatchFn(MatchableProperty property, Dictionary<string, object> triggerData, Game game);
}
