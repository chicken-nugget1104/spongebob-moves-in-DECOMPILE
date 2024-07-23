using System;

// Token: 0x0200027E RID: 638
public class HUBCraftCommand
{
	// Token: 0x06001462 RID: 5218 RVA: 0x0008BC28 File Offset: 0x00089E28
	public static Command Create(Identity sender, Identity receiver, bool start)
	{
		Command command = new Command(Command.TYPE.HUBCRAFT, sender, receiver);
		command["start"] = start;
		return command;
	}
}
