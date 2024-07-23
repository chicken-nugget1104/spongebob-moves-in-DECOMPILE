using System;

// Token: 0x0200027B RID: 635
public class ReturnCommand
{
	// Token: 0x0600145B RID: 5211 RVA: 0x0008BBA0 File Offset: 0x00089DA0
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RETURN, sender, receiver);
	}
}
