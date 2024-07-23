using System;

// Token: 0x02000141 RID: 321
public static class OperatorFactory
{
	// Token: 0x06000B3D RID: 2877 RVA: 0x00044B48 File Offset: 0x00042D48
	public static IOperator StringToOperator(string opString)
	{
		switch (opString)
		{
		case "OR":
			return new OrOperator();
		case "AND":
			return new AndOperator();
		case "NOT":
			return new NotOperator();
		}
		TFUtils.Assert(false, "This ConditionTree uses an unknown operator: " + opString);
		return null;
	}
}
