using System;

// Token: 0x0200028A RID: 650
public class WanderCommand
{
	// Token: 0x0600147A RID: 5242 RVA: 0x0008BD78 File Offset: 0x00089F78
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.WANDER, sender, receiver);
	}
}
