using System;
using System.Collections.Generic;

// Token: 0x0200019A RID: 410
public class TaskMatcher : Matcher
{
	// Token: 0x06000DA4 RID: 3492 RVA: 0x000533F8 File Offset: 0x000515F8
	public static TaskMatcher FromDict(Dictionary<string, object> dict)
	{
		TaskMatcher taskMatcher = new TaskMatcher();
		taskMatcher.RegisterProperty("task_id", dict);
		return taskMatcher;
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x0005341C File Offset: 0x0005161C
	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + this.GetTarget("task_id");
		}
		uint nDID = uint.Parse(this.GetTarget("task_id"));
		return game.taskManager.GetTaskData((int)nDID, false).m_sName;
	}

	// Token: 0x04000915 RID: 2325
	public const string DEFINITION_ID = "task_id";
}
