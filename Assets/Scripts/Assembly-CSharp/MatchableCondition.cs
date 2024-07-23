using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x0200013D RID: 317
public abstract class MatchableCondition : LoadableCondition
{
	// Token: 0x06000B2C RID: 2860 RVA: 0x000445AC File Offset: 0x000427AC
	protected MatchableCondition()
	{
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x000445B4 File Offset: 0x000427B4
	protected MatchableCondition(uint id, uint count, string loadToken, IList<string> relevantTypes, IList<IMatcher> matchers, IList<uint> prerequisiteConditions, int SimulatedExistsID = -1)
	{
		base.Initialize(id, count, loadToken, relevantTypes, prerequisiteConditions);
		this.Initialize(matchers, SimulatedExistsID);
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x000445E0 File Offset: 0x000427E0
	public IList<IMatcher> Matchers
	{
		get
		{
			return this.matchers;
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x000445E8 File Offset: 0x000427E8
	protected void Parse(Dictionary<string, object> loadedData, string loadToken, IList<string> relevantTypes, IList<IMatcher> matchers, int SimulatedExistsID = -1)
	{
		base.Parse(loadedData, loadToken, relevantTypes);
		this.Initialize(matchers, SimulatedExistsID);
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x00044600 File Offset: 0x00042800
	private void Initialize(IList<IMatcher> matchers, int SimulatedExistsID = -1)
	{
		this.matchers = matchers;
		this.simulatedExistsID = SimulatedExistsID;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00044610 File Offset: 0x00042810
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		foreach (IMatcher matcher in this.matchers)
		{
			foreach (string text in matcher.Keys)
			{
				if (matcher.IsRequired(text))
				{
					dictionary[text] = matcher.GetTarget(text);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x000446E0 File Offset: 0x000428E0
	[Conditional("DEBUG")]
	private void VerifyMatchable()
	{
		int num = 0;
		foreach (IMatcher matcher in this.matchers)
		{
			foreach (string property in matcher.Keys)
			{
				if (matcher.IsRequired(property))
				{
					num++;
				}
			}
		}
		TFUtils.Assert(num > 0, "No conditions found to match for Condition(" + this.ToString() + ")");
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x000447BC File Offset: 0x000429BC
	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		if (!base.IsTypeApplicable(trigger))
		{
			if (state.SelfExam == ConditionResult.UNINITIALIZED)
			{
				state.SelfExam = ConditionResult.UNDECIDED;
			}
			return;
		}
		uint num = 1U;
		foreach (IMatcher matcher in this.matchers)
		{
			num *= matcher.MatchAmount(game, trigger.Data);
		}
		state.Count += num;
		uint num2 = this.Count;
		int num3 = 0;
		if (this.simulatedExistsID > 0)
		{
			num2 += 1U;
			if (game.simulation.FindSimulated(new int?(this.simulatedExistsID)) != null)
			{
				num3++;
			}
		}
		if ((ulong)state.Count + (ulong)((long)num3) >= (ulong)num2)
		{
			state.SelfExam = ConditionResult.PASS;
		}
		else
		{
			state.SelfExam = ConditionResult.UNDECIDED;
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x000448B8 File Offset: 0x00042AB8
	public override string ToString()
	{
		string text = string.Empty;
		foreach (IMatcher arg in this.matchers)
		{
			text = text + arg + ",";
		}
		return string.Concat(new string[]
		{
			"MatchableCondition:(, matchers=[",
			text,
			"], ",
			base.ToString(),
			")"
		});
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x00044958 File Offset: 0x00042B58
	public override string Description(Game game)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0400077F RID: 1919
	private IList<IMatcher> matchers;

	// Token: 0x04000780 RID: 1920
	private int simulatedExistsID;
}
