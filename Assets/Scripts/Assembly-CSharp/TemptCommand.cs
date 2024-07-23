using System;

// Token: 0x0200028D RID: 653
public class TemptCommand
{
	// Token: 0x06001480 RID: 5248 RVA: 0x0008BDB4 File Offset: 0x00089FB4
	public static Command Create(Identity sender, Identity receiver, int? productId)
	{
		Command command = new Command(Command.TYPE.TEMPT, sender, receiver);
		command["product_id"] = productId;
		return command;
	}
}
