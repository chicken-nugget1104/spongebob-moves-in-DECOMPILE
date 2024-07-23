using System;
using System.Collections.Generic;

// Token: 0x02000150 RID: 336
public class TaskCompleteCondition : MatchableCondition
{
	// Token: 0x06000B69 RID: 2921 RVA: 0x00045328 File Offset: 0x00043528
	public static TaskCompleteCondition FromDict(Dictionary<string, object> dict)
	{
		TaskCompleteCondition taskCompleteCondition = new TaskCompleteCondition();
		taskCompleteCondition.Parse(dict, "task_complete", new List<string>
		{
			typeof(TaskCompleteAction).ToString()
		}, new List<IMatcher>
		{
			TaskMatcher.FromDict(dict)
		}, -1);
		return taskCompleteCondition;
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00045378 File Offset: 0x00043578
	public override string Description(Game game)
	{
		return "task_complete";
	}

	// Token: 0x0400079F RID: 1951
	public const string LOAD_TOKEN = "task_complete";
}
