using System;
using System.Collections.Generic;

// Token: 0x02000225 RID: 549
public class FirePlayHavenPlacement : SessionActionDefinition
{
	// Token: 0x06001201 RID: 4609 RVA: 0x0007DCB8 File Offset: 0x0007BEB8
	public static FirePlayHavenPlacement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		FirePlayHavenPlacement firePlayHavenPlacement = new FirePlayHavenPlacement();
		firePlayHavenPlacement.Parse(data, id, startConditions, originatedFromQuest);
		return firePlayHavenPlacement;
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x0007DCD8 File Offset: 0x0007BED8
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new ConstantCondition(0U, true), originatedFromQuest);
		if (data.ContainsKey("placement"))
		{
			this.placement = (string)data["placement"];
		}
		else
		{
			TFUtils.ErrorLog("Error defining playhaven placement. No placement defined");
		}
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x0007DD2C File Offset: 0x0007BF2C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["placement"] = this.placement;
		return dictionary;
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x0007DD54 File Offset: 0x0007BF54
	public override void PreActivate(Game game, SessionActionTracker tracker)
	{
		if (this.placement != null)
		{
			game.playHavenController.RequestContent(this.placement);
		}
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x0007DD74 File Offset: 0x0007BF74
	public override string ToString()
	{
		return base.ToString() + "FirePlayHavenPlacement:(placement=" + this.placement + ")";
	}

	// Token: 0x04000C4C RID: 3148
	public const string TYPE = "call_playhaven";

	// Token: 0x04000C4D RID: 3149
	private const string PLACEMENT_FIELD = "placement";

	// Token: 0x04000C4E RID: 3150
	private string placement;
}
