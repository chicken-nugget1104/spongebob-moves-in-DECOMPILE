using System;
using System.Collections.Generic;

// Token: 0x02000151 RID: 337
public class TaskStartCondition : MatchableCondition
{
	// Token: 0x06000B6C RID: 2924 RVA: 0x00045388 File Offset: 0x00043588
	public static TaskStartCondition FromDict(Dictionary<string, object> dict)
	{
		TaskStartCondition taskStartCondition = new TaskStartCondition();
		taskStartCondition.Parse(dict, "task_start", new List<string>
		{
			typeof(TaskStartAction).ToString()
		}, new List<IMatcher>
		{
			TaskMatcher.FromDict(dict)
		}, -1);
		return taskStartCondition;
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x000453D8 File Offset: 0x000435D8
	public override string Description(Game game)
	{
		return "task_start";
	}

	// Token: 0x040007A0 RID: 1952
	public const string LOAD_TOKEN = "task_start";
}
