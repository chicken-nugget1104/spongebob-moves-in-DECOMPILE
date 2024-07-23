using System;

// Token: 0x02000285 RID: 645
public class BonusCommand
{
	// Token: 0x06001470 RID: 5232 RVA: 0x0008BD14 File Offset: 0x00089F14
	public static Command Create(Identity senderReceiver)
	{
		return new Command(Command.TYPE.BONUS, senderReceiver, senderReceiver);
	}
}
