using System;

// Token: 0x02000295 RID: 661
public class PurchaseCommand
{
	// Token: 0x06001490 RID: 5264 RVA: 0x0008BEC0 File Offset: 0x0008A0C0
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.PURCHASE, sender, receiver);
	}
}
