using System;

// Token: 0x02000297 RID: 663
public class IdlePauseCommand
{
	// Token: 0x06001494 RID: 5268 RVA: 0x0008BEE8 File Offset: 0x0008A0E8
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.IDLE_PAUSE, sender, receiver);
	}
}
