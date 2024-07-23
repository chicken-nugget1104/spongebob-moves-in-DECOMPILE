using System;

// Token: 0x02000279 RID: 633
public class SpawnCommand
{
	// Token: 0x06001457 RID: 5207 RVA: 0x0008BB30 File Offset: 0x00089D30
	public static Command Create(Identity sender, Identity receiver, string blueprint)
	{
		Command command = new Command(Command.TYPE.SPAWN, sender, receiver);
		command["blueprint"] = blueprint;
		return command;
	}
}
