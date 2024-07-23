using System;

// Token: 0x02000144 RID: 324
public class NotOperator : IOperator
{
	// Token: 0x06000B45 RID: 2885 RVA: 0x00044CB0 File Offset: 0x00042EB0
	public override string ToString()
	{
		return "NOT";
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x00044CB8 File Offset: 0x00042EB8
	public ConditionResult Operate(ConditionResult left, ConditionResult right)
	{
		TFUtils.Assert(left != ConditionResult.UNINITIALIZED, "Can't operate on an unitialized left operand!");
		TFUtils.Assert(right == ConditionResult.UNINITIALIZED, "Right operand should not have a value!");
		if (left == ConditionResult.UNDECIDED)
		{
			return ConditionResult.UNDECIDED;
		}
		if (left == ConditionResult.PASS)
		{
			return ConditionResult.FAIL;
		}
		return ConditionResult.PASS;
	}

	// Token: 0x04000788 RID: 1928
	public const string DESCRIPTION = "NOT";
}
