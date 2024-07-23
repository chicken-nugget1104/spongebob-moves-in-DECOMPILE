using System;

// Token: 0x02000283 RID: 643
public class ActivateCommand
{
	// Token: 0x0600146C RID: 5228 RVA: 0x0008BCEC File Offset: 0x00089EEC
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ACTIVATE, sender, receiver);
	}
}
