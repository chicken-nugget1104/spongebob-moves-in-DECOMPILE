using System;
using System.Collections.Generic;

// Token: 0x0200012C RID: 300
public class ConditionState
{
	// Token: 0x06000AD2 RID: 2770 RVA: 0x00043304 File Offset: 0x00041504
	public ConditionState(ICondition definition)
	{
		this.definition = definition;
		this.substates = new List<ConditionState>(2);
		this.definition.FillSubstates(ref this.substates);
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0004333C File Offset: 0x0004153C
	public T GetDefinition<T>()
	{
		return (T)((object)this.definition);
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0004334C File Offset: 0x0004154C
	public List<ConditionState> SubStates
	{
		get
		{
			return this.substates;
		}
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x00043354 File Offset: 0x00041554
	public void Hydrate(ConditionalProgress progress, Game game, List<uint> metIDs = null)
	{
		if (metIDs == null)
		{
			metIDs = new List<uint>();
			this.GetMetIds(ref metIDs);
		}
		foreach (ConditionState conditionState in this.substates)
		{
			conditionState.Hydrate(progress, game, metIDs);
		}
		this.Count = (uint)progress.MetIds.FindAll((uint id) => id == this.definition.Id).Count;
		if (this.Count >= this.definition.Count)
		{
			this.SelfExam = ConditionResult.PASS;
		}
		if (this.SelfExam == ConditionResult.UNINITIALIZED)
		{
			int count = this.definition.PrerequisiteConditions.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					if (!metIDs.Contains(this.definition.PrerequisiteConditions[i]))
					{
						this.SelfExam = ConditionResult.UNDECIDED;
						return;
					}
				}
			}
			this.definition.Evaluate(this, game, Trigger.Null);
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0004347C File Offset: 0x0004167C
	public ConditionalProgress Dehydrate()
	{
		List<uint> metIds = new List<uint>();
		this.GetMetIds(ref metIds);
		return new ConditionalProgress(metIds);
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x000434A0 File Offset: 0x000416A0
	public static ConditionalProgress DehydrateChunks(List<ConditionState> list)
	{
		List<uint> metIds = new List<uint>();
		foreach (ConditionState conditionState in list)
		{
			conditionState.GetMetIds(ref metIds);
		}
		return new ConditionalProgress(metIds);
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x00043510 File Offset: 0x00041710
	protected virtual void GetMetIds(ref List<uint> completedIds)
	{
		foreach (ConditionState conditionState in this.substates)
		{
			conditionState.GetMetIds(ref completedIds);
		}
		if (this.SelfExam == ConditionResult.PASS || this.SelfExam == ConditionResult.UNDECIDED)
		{
			int num = 0;
			while ((long)num < (long)((ulong)this.Count))
			{
				completedIds.Add(this.definition.Id);
				num++;
			}
		}
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x000435BC File Offset: 0x000417BC
	public virtual ConditionResult Examine()
	{
		return this.SelfExam;
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x000435C4 File Offset: 0x000417C4
	public bool Recalculate(Game game, ITrigger trigger, List<uint> metIDs = null)
	{
		if (metIDs == null)
		{
			metIDs = new List<uint>();
			this.GetMetIds(ref metIDs);
		}
		int count = this.definition.PrerequisiteConditions.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				if (!metIDs.Contains(this.definition.PrerequisiteConditions[i]))
				{
					return false;
				}
			}
		}
		TFUtils.Assert(trigger != null, "Given trigger cannot be null. Use Trigger.Null instead.");
		if (this.SelfExam != ConditionResult.UNINITIALIZED && this.SelfExam != ConditionResult.UNDECIDED)
		{
			return false;
		}
		bool flag = false;
		foreach (ConditionState conditionState in this.substates)
		{
			flag = (flag || conditionState.Recalculate(game, trigger, metIDs));
		}
		bool flag2 = this.SelfExam == ConditionResult.UNINITIALIZED;
		uint num = this.Count;
		this.definition.Evaluate(this, game, trigger);
		TFUtils.Assert(this.SelfExam != ConditionResult.UNINITIALIZED, "A ConditionState's Definition returned UNINITIALIZED as a result of the Evaulate call. Fix that definition so it doesn't do that.");
		num = this.Count - num;
		return this.SelfExam != ConditionResult.UNDECIDED || flag2 || flag || num != 0U;
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00043728 File Offset: 0x00041928
	public List<ConditionDescription> Describe(Game game)
	{
		return new List<ConditionDescription>
		{
			this.DescribeMe(game)
		};
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0004374C File Offset: 0x0004194C
	protected virtual ConditionDescription DescribeMe(Game game)
	{
		return new ConditionDescription
		{
			Id = this.definition.Id,
			OccuranceCount = this.Count,
			OccurancesRequired = this.definition.Count,
			IsPassed = (this.Examine() == ConditionResult.PASS),
			Description = this.definition.Description(game)
		};
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x000437B8 File Offset: 0x000419B8
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"ConditionState:(SelfExam=",
			this.SelfExam,
			", Definition=",
			this.definition,
			", Count=",
			this.Count,
			", Substates=",
			this.substates,
			")"
		});
	}

	// Token: 0x0400075D RID: 1885
	public ConditionResult SelfExam;

	// Token: 0x0400075E RID: 1886
	public uint Count;

	// Token: 0x0400075F RID: 1887
	private ICondition definition;

	// Token: 0x04000760 RID: 1888
	private List<ConditionState> substates;
}
