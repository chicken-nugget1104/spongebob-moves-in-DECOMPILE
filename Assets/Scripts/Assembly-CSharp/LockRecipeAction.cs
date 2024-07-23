using System;
using System.Collections.Generic;

// Token: 0x020000DC RID: 220
public class LockRecipeAction : PersistedTriggerableAction
{
	// Token: 0x06000826 RID: 2086 RVA: 0x00034A8C File Offset: 0x00032C8C
	public LockRecipeAction(int did) : base("lr", Identity.Null())
	{
		this.did = did;
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x00034AA8 File Offset: 0x00032CA8
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00034AAC File Offset: 0x00032CAC
	public new static LockRecipeAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "did");
		return new LockRecipeAction(num);
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x00034AD0 File Offset: 0x00032CD0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.did;
		return dictionary;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00034AFC File Offset: 0x00032CFC
	public override void Apply(Game game, ulong utcNow)
	{
		if (this.did < 0 || !game.craftManager.IsRecipeUnlocked(this.did))
		{
			return;
		}
		game.craftManager.LockRecipe(this.did);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00034B3C File Offset: 0x00032D3C
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["recipes"];
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (Convert.ToInt32(list[i]) == this.did)
			{
				list.RemoveAt(i);
				break;
			}
		}
		base.Confirm(gameState);
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00034BAC File Offset: 0x00032DAC
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00034BB0 File Offset: 0x00032DB0
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005EE RID: 1518
	public const string LOCK_RECIPE = "lr";

	// Token: 0x040005EF RID: 1519
	public int did;
}
