using System;

// Token: 0x0200029D RID: 669
public class CheerCommand
{
	// Token: 0x060014A0 RID: 5280 RVA: 0x0008BF8C File Offset: 0x0008A18C
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CHEER, sender, receiver);
	}
}
