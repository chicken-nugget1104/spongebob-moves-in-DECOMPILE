using System;

// Token: 0x0200029C RID: 668
public class FleeCommand
{
	// Token: 0x0600149E RID: 5278 RVA: 0x0008BF78 File Offset: 0x0008A178
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.FLEE, sender, receiver);
	}
}
