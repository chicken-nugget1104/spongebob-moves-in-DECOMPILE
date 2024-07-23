using System;

// Token: 0x0200029E RID: 670
public class TaskCommand
{
	// Token: 0x060014A2 RID: 5282 RVA: 0x0008BFA0 File Offset: 0x0008A1A0
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.TASK, sender, receiver);
	}
}
