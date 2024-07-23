using System;

// Token: 0x02000282 RID: 642
public class AdvanceCommand
{
	// Token: 0x0600146A RID: 5226 RVA: 0x0008BCD8 File Offset: 0x00089ED8
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.ADVANCE, sender, receiver);
	}
}
