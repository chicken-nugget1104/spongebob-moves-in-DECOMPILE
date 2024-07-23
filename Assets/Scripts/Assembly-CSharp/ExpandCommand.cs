using System;

// Token: 0x02000296 RID: 662
public class ExpandCommand
{
	// Token: 0x06001492 RID: 5266 RVA: 0x0008BED4 File Offset: 0x0008A0D4
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.EXPAND, sender, receiver);
	}
}
