using System;

// Token: 0x02000289 RID: 649
public class AbortCommand
{
	// Token: 0x06001478 RID: 5240 RVA: 0x0008BD64 File Offset: 0x00089F64
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ABORT, sender, receiver);
	}
}
