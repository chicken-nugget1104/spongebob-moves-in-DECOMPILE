using System;

// Token: 0x02000299 RID: 665
public class ResumeWishingCommand
{
	// Token: 0x06001498 RID: 5272 RVA: 0x0008BF1C File Offset: 0x0008A11C
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RESUME_WISHING, sender, receiver);
	}
}
