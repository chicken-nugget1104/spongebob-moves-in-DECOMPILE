using System;

// Token: 0x02000284 RID: 644
public class CompleteCommand
{
	// Token: 0x0600146E RID: 5230 RVA: 0x0008BD00 File Offset: 0x00089F00
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.COMPLETE, sender, receiver);
	}
}
