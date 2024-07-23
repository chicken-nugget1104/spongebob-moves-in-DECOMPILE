using System;
using System.Collections.Generic;

// Token: 0x02000145 RID: 325
public class PaveCondition : MatchableCondition
{
	// Token: 0x06000B48 RID: 2888 RVA: 0x00044D00 File Offset: 0x00042F00
	public static PaveCondition FromDict(Dictionary<string, object> dict)
	{
		PaveCondition paveCondition = new PaveCondition();
		paveCondition.Parse(dict, "pave", new List<string>
		{
			typeof(PaveAction).ToString()
		}, new List<IMatcher>
		{
			PaveMatcher.FromDict(dict)
		}, -1);
		return paveCondition;
	}

	// Token: 0x06000B49 RID: 2889 RVA: 0x00044D50 File Offset: 0x00042F50
	public override string Description(Game game)
	{
		return Language.Get("!!COND_PLACE_PATH");
	}

	// Token: 0x04000789 RID: 1929
	public const string LOAD_TOKEN = "pave";
}
