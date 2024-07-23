using System;

// Token: 0x0200028B RID: 651
public class HungerCommand
{
	// Token: 0x0600147C RID: 5244 RVA: 0x0008BD8C File Offset: 0x00089F8C
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.HUNGER, sender, receiver);
	}
}
