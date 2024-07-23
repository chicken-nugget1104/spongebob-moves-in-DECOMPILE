using System;

// Token: 0x0200029F RID: 671
public class EnterCommand
{
	// Token: 0x060014A4 RID: 5284 RVA: 0x0008BFB4 File Offset: 0x0008A1B4
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ENTER, sender, receiver);
	}
}
