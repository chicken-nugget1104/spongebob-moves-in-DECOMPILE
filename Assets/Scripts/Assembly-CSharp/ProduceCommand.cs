using System;

// Token: 0x02000281 RID: 641
public class ProduceCommand
{
	// Token: 0x06001468 RID: 5224 RVA: 0x0008BCC4 File Offset: 0x00089EC4
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.PRODUCE, sender, receiver);
	}
}
