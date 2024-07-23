using System;

// Token: 0x0200027F RID: 639
public class ErectCommand
{
	// Token: 0x06001464 RID: 5220 RVA: 0x0008BC5C File Offset: 0x00089E5C
	public static Command Create(Identity sender, Identity receiver, Identity building, ulong timeBuild)
	{
		Command command = new Command(Command.TYPE.ERECT, sender, receiver);
		command["building"] = building;
		command["time.build"] = timeBuild;
		return command;
	}
}
