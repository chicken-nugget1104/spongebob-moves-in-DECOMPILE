using System;

// Token: 0x02000290 RID: 656
public class PerformTaskCommand
{
	// Token: 0x06001486 RID: 5254 RVA: 0x0008BE3C File Offset: 0x0008A03C
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.PERFORM_TASK, sender, receiver);
	}
}
