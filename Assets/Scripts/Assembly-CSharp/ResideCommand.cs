using System;

// Token: 0x02000280 RID: 640
public class ResideCommand
{
	// Token: 0x06001466 RID: 5222 RVA: 0x0008BC98 File Offset: 0x00089E98
	public static Command Create(Identity sender, Identity receiver, Identity residence)
	{
		Command command = new Command(Command.TYPE.RESIDE, sender, receiver);
		command["residence"] = residence;
		return command;
	}
}
