using System;

// Token: 0x0200028F RID: 655
public class OfferFoodCommand
{
	// Token: 0x06001484 RID: 5252 RVA: 0x0008BE08 File Offset: 0x0008A008
	public static Command Create(Identity sender, Identity receiver, int productId)
	{
		Command command = new Command(Command.TYPE.FEED, sender, receiver);
		command["product_id"] = productId;
		return command;
	}
}
