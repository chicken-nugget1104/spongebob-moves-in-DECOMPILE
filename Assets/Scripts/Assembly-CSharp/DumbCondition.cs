using System;
using System.Collections.Generic;

// Token: 0x02000136 RID: 310
public class DumbCondition : BaseCondition
{
	// Token: 0x06000B0B RID: 2827 RVA: 0x00044088 File Offset: 0x00042288
	public DumbCondition(uint id)
	{
		base.Initialize(id, 1U, new List<string>
		{
			"DumbCondition"
		}, new List<uint>());
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00044120 File Offset: 0x00042320
	public override string Description(Game game)
	{
		return "Dumb condition";
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00044128 File Offset: 0x00042328
	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		if (base.IsTypeApplicable(trigger))
		{
			if (!trigger.Data.ContainsKey("status"))
			{
				TFUtils.Assert(trigger.Data.ContainsKey("status"), "Found a DumbCondition Trigger, but the Data was not formatted as expected (could not find the 'status' field). Data=" + TFUtils.DebugDictToString(trigger.Data));
			}
			state.SelfExam = (ConditionResult)((int)trigger.Data["status"]);
		}
		else
		{
			state.SelfExam = ConditionResult.UNDECIDED;
		}
	}

	// Token: 0x04000771 RID: 1905
	public const string TRIGGER_TYPE = "DumbCondition";

	// Token: 0x04000772 RID: 1906
	private const string STATUS = "status";

	// Token: 0x04000773 RID: 1907
	public static readonly Trigger PASS_TRIGGER = new Trigger("DumbCondition", new Dictionary<string, object>
	{
		{
			"status",
			ConditionResult.PASS
		}
	}, 0UL, null, null);

	// Token: 0x04000774 RID: 1908
	public static readonly Trigger FAIL_TRIGGER = new Trigger("DumbCondition", new Dictionary<string, object>
	{
		{
			"status",
			ConditionResult.FAIL
		}
	}, 0UL, null, null);
}
