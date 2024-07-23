using System;

// Token: 0x02000188 RID: 392
public class DialogMatcher : Matcher
{
	// Token: 0x06000D4B RID: 3403 RVA: 0x00051F0C File Offset: 0x0005010C
	public DialogMatcher(uint dialogSequenceId)
	{
		base.AddRequiredProperty("sequence_id", dialogSequenceId.ToString());
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x00051F28 File Offset: 0x00050128
	public override string DescribeSubject(Game game)
	{
		return "Close dialog " + this.GetTarget("sequence_id");
	}

	// Token: 0x040008E5 RID: 2277
	public const string DIALOGSEQUENCE_ID = "sequence_id";
}
