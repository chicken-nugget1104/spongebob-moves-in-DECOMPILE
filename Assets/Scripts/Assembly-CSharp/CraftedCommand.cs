using System;

// Token: 0x02000292 RID: 658
public class CraftedCommand
{
	// Token: 0x0600148A RID: 5258 RVA: 0x0008BE64 File Offset: 0x0008A064
	public static Command Create(Identity sender, Identity receiver, int slotId)
	{
		Command command = new Command(Command.TYPE.CRAFTED, sender, receiver);
		command["slot_id"] = slotId;
		return command;
	}
}
