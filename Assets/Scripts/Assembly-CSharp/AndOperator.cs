using System;

// Token: 0x02000142 RID: 322
public class AndOperator : IOperator
{
	// Token: 0x06000B3F RID: 2879 RVA: 0x00044BF0 File Offset: 0x00042DF0
	public override string ToString()
	{
		return "AND";
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x00044BF8 File Offset: 0x00042DF8
	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		TFUtils.Assert(left != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized left operand!");
		TFUtils.Assert(right != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized right operand!");
		if (left == ConditionResult.PASS && right == ConditionResult.PASS)
		{
			return ConditionResult.PASS;
		}
		if (left == ConditionResult.FAIL || right == ConditionResult.FAIL)
		{
			return ConditionResult.FAIL;
		}
		return ConditionResult.UNDECIDED;
	}

	// Token: 0x04000786 RID: 1926
	public const string DESCRIPTION = "AND";
}
