using System;
using System.Collections.Generic;

// Token: 0x020000D7 RID: 215
public class FeatureUnlocksAction : PersistedTriggerableAction
{
	// Token: 0x06000805 RID: 2053 RVA: 0x000341E0 File Offset: 0x000323E0
	public FeatureUnlocksAction(List<string> features) : base("uf", Identity.Null())
	{
		this.features = features;
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000806 RID: 2054 RVA: 0x000341FC File Offset: 0x000323FC
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00034200 File Offset: 0x00032400
	public new static FeatureUnlocksAction FromDict(Dictionary<string, object> data)
	{
		List<string> list = TFUtils.LoadList<string>(data, "features");
		return new FeatureUnlocksAction(list);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00034224 File Offset: 0x00032424
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["features"] = TFUtils.CloneAndCastList<string, object>(this.features);
		return dictionary;
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00034250 File Offset: 0x00032450
	public override void Apply(Game game, ulong utcNow)
	{
		foreach (string feature in this.features)
		{
			game.featureManager.UnlockFeature(feature);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x000342C4 File Offset: 0x000324C4
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("features"))
		{
			dictionary["features"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["features"];
		foreach (string item in this.features)
		{
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
		base.Confirm(gameState);
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00034380 File Offset: 0x00032580
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00034384 File Offset: 0x00032584
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005DB RID: 1499
	public const string UNLOCK_FEATURE = "uf";

	// Token: 0x040005DC RID: 1500
	public List<string> features;
}
