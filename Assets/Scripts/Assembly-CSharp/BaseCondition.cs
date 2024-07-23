using System;
using System.Collections.Generic;

// Token: 0x02000121 RID: 289
public abstract class BaseCondition : ICondition
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x00042870 File Offset: 0x00040A70
	public uint Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00042878 File Offset: 0x00040A78
	public uint Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00042880 File Offset: 0x00040A80
	public IList<uint> PrerequisiteConditions
	{
		get
		{
			return this.prerequisiteConditions;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00042888 File Offset: 0x00040A88
	public ICollection<string> RelevantTypes
	{
		get
		{
			return this.relevantTypes;
		}
	}

	// Token: 0x06000AB5 RID: 2741
	public abstract string Description(Game game);

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00042890 File Offset: 0x00040A90
	public virtual bool IsExpensiveToCalculate
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x00042894 File Offset: 0x00040A94
	protected void Initialize(uint id, uint count, ICollection<string> relevantTypes, IList<uint> prerequisiteConditions)
	{
		this.id = id;
		this.count = count;
		this.relevantTypes = relevantTypes;
		this.prerequisiteConditions = prerequisiteConditions;
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x000428B4 File Offset: 0x00040AB4
	public virtual uint FindNextId()
	{
		return this.FindNextId(this.id);
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x000428C4 File Offset: 0x00040AC4
	public virtual uint FindNextId(uint floor)
	{
		return (floor <= this.id) ? (this.id + 1U) : floor;
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x000428E0 File Offset: 0x00040AE0
	public virtual void FillSubstates(ref List<ConditionState> substates)
	{
	}

	// Token: 0x06000ABB RID: 2747
	public abstract void Evaluate(ConditionState state, Game game, ITrigger trigger);

	// Token: 0x06000ABC RID: 2748 RVA: 0x000428E4 File Offset: 0x00040AE4
	protected bool IsTypeApplicable(ITrigger trigger)
	{
		return this.relevantTypes == null || this.relevantTypes.Contains(trigger.Type);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00042908 File Offset: 0x00040B08
	public override string ToString()
	{
		string text = string.Empty;
		if (this.relevantTypes != null)
		{
			text += ", relevantTypes=[";
			foreach (string str in this.relevantTypes)
			{
				text = text + str + ",";
			}
			text += "]";
		}
		return string.Concat(new object[]
		{
			"BaseCondition:(id=",
			this.id,
			", count=",
			this.count,
			text,
			")"
		});
	}

	// Token: 0x04000741 RID: 1857
	private uint id;

	// Token: 0x04000742 RID: 1858
	private uint count;

	// Token: 0x04000743 RID: 1859
	private ICollection<string> relevantTypes;

	// Token: 0x04000744 RID: 1860
	private IList<uint> prerequisiteConditions;
}
