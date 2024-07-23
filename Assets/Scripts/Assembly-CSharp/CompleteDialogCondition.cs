using System;
using System.Collections.Generic;

// Token: 0x02000127 RID: 295
public class CompleteDialogCondition : MatchableCondition
{
	// Token: 0x06000ACD RID: 2765 RVA: 0x00042D6C File Offset: 0x00040F6C
	public CompleteDialogCondition(uint id, uint targetDialogId) : base(id, 1U, "NOT LOADABLE", new List<string>
	{
		"dialogtrigger_character",
		"dialogtrigger_quest_start",
		"dialogtrigger_quest_complete",
		"dialogtrigger_booty_quest_complete",
		"dialogtrigger_level_up"
	}, new List<IMatcher>
	{
		new DialogMatcher(targetDialogId)
	}, new List<uint>(), -1)
	{
	}
}
