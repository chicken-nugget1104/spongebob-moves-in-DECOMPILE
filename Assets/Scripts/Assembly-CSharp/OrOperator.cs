using System;

// Token: 0x02000143 RID: 323
public class OrOperator : IOperator
{
	// Token: 0x06000B42 RID: 2882 RVA: 0x00044C50 File Offset: 0x00042E50
	public override string ToString()
	{
		return "OR";
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00044C58 File Offset: 0x00042E58
	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		TFUtils.Assert(left != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized left operand!");
		TFUtils.Assert(right != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized right operand!");
		if (left == ConditionResult.PASS || right == ConditionResult.PASS)
		{
			return ConditionResult.PASS;
		}
		if (left == ConditionResult.FAIL && right == ConditionResult.FAIL)
		{
			return ConditionResult.FAIL;
		}
		return ConditionResult.UNDECIDED;
	}

	// Token: 0x04000787 RID: 1927
	public const string DESCRIPTION = "OR";
}
