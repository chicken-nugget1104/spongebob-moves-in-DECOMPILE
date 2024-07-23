using System;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class GoHomeCommand
{
	// Token: 0x0600149A RID: 5274 RVA: 0x0008BF30 File Offset: 0x0008A130
	public static Command Create(Identity sender, Identity receiver, Vector2 position)
	{
		Command command = new Command(Command.TYPE.GO_HOME, sender, receiver);
		command["home_position"] = position;
		return command;
	}
}
