using System;

// Token: 0x020002A2 RID: 674
public class RushTaskCommand
{
	// Token: 0x060014AA RID: 5290 RVA: 0x0008C010 File Offset: 0x0008A210
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RUSH_TASK, sender, receiver);
	}
}
