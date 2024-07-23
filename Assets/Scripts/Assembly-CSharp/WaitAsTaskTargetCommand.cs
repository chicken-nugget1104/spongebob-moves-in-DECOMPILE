using System;

// Token: 0x02000294 RID: 660
public class WaitAsTaskTargetCommand
{
	// Token: 0x0600148E RID: 5262 RVA: 0x0008BEAC File Offset: 0x0008A0AC
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.WAIT_AS_TASK_TARGET, sender, receiver);
	}
}
