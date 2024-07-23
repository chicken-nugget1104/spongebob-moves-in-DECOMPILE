using System;

// Token: 0x0200028C RID: 652
public class WishCommand
{
	// Token: 0x0600147E RID: 5246 RVA: 0x0008BDA0 File Offset: 0x00089FA0
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.WISH, sender, receiver);
	}
}
