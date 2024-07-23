using System;
using System.Collections.Generic;

// Token: 0x0200013B RID: 315
public interface ICondition
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000B1B RID: 2843
	uint Id { get; }

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000B1C RID: 2844
	uint Count { get; }

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000B1D RID: 2845
	ICollection<string> RelevantTypes { get; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000B1E RID: 2846
	IList<uint> PrerequisiteConditions { get; }

	// Token: 0x06000B1F RID: 2847
	string Description(Game game);

	// Token: 0x06000B20 RID: 2848
	uint FindNextId();

	// Token: 0x06000B21 RID: 2849
	uint FindNextId(uint floor);

	// Token: 0x06000B22 RID: 2850
	void FillSubstates(ref List<ConditionState> substates);

	// Token: 0x06000B23 RID: 2851
	void Evaluate(ConditionState state, Game game, ITrigger trigger);

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000B24 RID: 2852
	bool IsExpensiveToCalculate { get; }

	// Token: 0x06000B25 RID: 2853
	string ToString();
}
