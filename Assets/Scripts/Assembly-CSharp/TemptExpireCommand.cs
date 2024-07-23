using System;

// Token: 0x0200028E RID: 654
public class TemptExpireCommand
{
	// Token: 0x06001482 RID: 5250 RVA: 0x0008BDE8 File Offset: 0x00089FE8
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.EXPIRE, sender, receiver);
	}
}
