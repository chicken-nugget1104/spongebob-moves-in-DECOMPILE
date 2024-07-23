using System;
using System.Collections.Generic;

// Token: 0x0200012D RID: 301
public class ConditionTree : LoadableCondition
{
	// Token: 0x06000ADF RID: 2783 RVA: 0x00043838 File Offset: 0x00041A38
	private ConditionTree()
	{
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x00043840 File Offset: 0x00041A40
	public ConditionTree(uint id, ICondition left, IOperator op, ICondition right)
	{
		this.Initialize(id, "tree", left, op, right);
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00043864 File Offset: 0x00041A64
	public IOperator Operator
	{
		get
		{
			return this.op;
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x0004386C File Offset: 0x00041A6C
	public ICondition Left
	{
		get
		{
			return this.left;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00043874 File Offset: 0x00041A74
	public ICondition Right
	{
		get
		{
			return this.right;
		}
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x0004387C File Offset: 0x00041A7C
	public override Dictionary<string, object> ToDict()
	{
		LoadableCondition loadableCondition = this.left as LoadableCondition;
		LoadableCondition loadableCondition2 = this.right as LoadableCondition;
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["operator"] = this.op.ToString();
		dictionary["loperand"] = ((loadableCondition == null) ? null : loadableCondition.ToDict());
		dictionary["roperand"] = ((loadableCondition2 == null) ? null : loadableCondition2.ToDict());
		return dictionary;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x000438FC File Offset: 0x00041AFC
	public static ConditionTree FromDict(Dictionary<string, object> data)
	{
		TFUtils.Assert(data.Count != 0, "This ConditionTree has no conditions to evaluate.");
		string opString = (string)data["operator"];
		IOperator @operator = OperatorFactory.StringToOperator(opString);
		ICondition condition = null;
		ICondition condition2 = null;
		if (data.ContainsKey("loperand"))
		{
			condition = ConditionFactory.FromDict((Dictionary<string, object>)data["loperand"]);
		}
		if (data.ContainsKey("roperand"))
		{
			condition2 = ConditionFactory.FromDict((Dictionary<string, object>)data["roperand"]);
		}
		ConditionTree conditionTree = new ConditionTree();
		conditionTree.Parse(data, "tree", condition, @operator, condition2);
		return conditionTree;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x000439A0 File Offset: 0x00041BA0
	public static ConditionTree Create(ICondition first, IOperator op, ICondition last)
	{
		return new ConditionTree(first.FindNextId(last.FindNextId()), first, op, last);
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x000439C4 File Offset: 0x00041BC4
	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICondition left, IOperator op, ICondition right)
	{
		base.Parse(loadedData, loadToken, ConditionTree.BubbleTypes(left, right));
		this.InitializePartial(left, op, right);
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x000439E4 File Offset: 0x00041BE4
	private void Initialize(uint id, string loadToken, ICondition left, IOperator op, ICondition right)
	{
		base.Initialize(id, 1U, loadToken, ConditionTree.BubbleTypes(left, right), new List<uint>());
		this.InitializePartial(left, op, right);
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00043A08 File Offset: 0x00041C08
	private void InitializePartial(ICondition left, IOperator op, ICondition right)
	{
		this.left = left;
		this.right = right;
		this.op = op;
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00043A20 File Offset: 0x00041C20
	public override string Description(Game game)
	{
		return string.Concat(new string[]
		{
			this.left.Description(game),
			" ",
			this.op.ToString().ToLower(),
			Environment.NewLine,
			this.right.Description(game)
		});
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x00043A7C File Offset: 0x00041C7C
	public override void FillSubstates(ref List<ConditionState> substates)
	{
		substates.Insert(0, new ConditionState(this.left));
		substates.Insert(1, new ConditionState(this.right));
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x00043AB0 File Offset: 0x00041CB0
	public override uint FindNextId(uint floor)
	{
		return this.right.FindNextId(this.left.FindNextId(base.FindNextId(floor)));
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00043AD0 File Offset: 0x00041CD0
	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		ConditionState conditionState = state.SubStates[0];
		ConditionState conditionState2 = state.SubStates[1];
		state.SelfExam = this.op.Operate(conditionState.Examine(), conditionState2.Examine());
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00043B14 File Offset: 0x00041D14
	private static List<string> BubbleTypes(ICondition left, ICondition right)
	{
		if (left.RelevantTypes == null && right.RelevantTypes == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		if (left.RelevantTypes != null)
		{
			list.AddRange(left.RelevantTypes);
		}
		if (right.RelevantTypes != null)
		{
			list.AddRange(right.RelevantTypes);
		}
		return list;
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x00043B70 File Offset: 0x00041D70
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			base.ToString(),
			"ConditionTree:( Operator=",
			this.op,
			", Left=",
			this.left.ToString(),
			", Right=",
			this.right.ToString(),
			")"
		});
	}

	// Token: 0x04000761 RID: 1889
	public const string LOAD_TOKEN = "tree";

	// Token: 0x04000762 RID: 1890
	private const string DESCRIPTION = "ConditionTree";

	// Token: 0x04000763 RID: 1891
	private const int LEFT = 0;

	// Token: 0x04000764 RID: 1892
	private const int RIGHT = 1;

	// Token: 0x04000765 RID: 1893
	private IOperator op;

	// Token: 0x04000766 RID: 1894
	private ICondition left;

	// Token: 0x04000767 RID: 1895
	private ICondition right;
}
