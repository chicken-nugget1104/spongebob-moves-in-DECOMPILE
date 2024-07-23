using System;

// Token: 0x0200027C RID: 636
public class RushCommand
{
	// Token: 0x0600145D RID: 5213 RVA: 0x0008BBC0 File Offset: 0x00089DC0
	public static Command Create(Identity target)
	{
		return new Command(Command.TYPE.RUSH, target, target);
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x0008BBCC File Offset: 0x00089DCC
	public static Command Create(Identity target, int slotId)
	{
		Command command = new Command(Command.TYPE.RUSH, target, target);
		command["slot_id"] = slotId;
		return command;
	}
}
