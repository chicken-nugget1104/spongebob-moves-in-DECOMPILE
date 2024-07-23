using System;

// Token: 0x02000291 RID: 657
public class CraftCommand
{
	// Token: 0x06001488 RID: 5256 RVA: 0x0008BE50 File Offset: 0x0008A050
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CRAFT, sender, receiver);
	}
}
