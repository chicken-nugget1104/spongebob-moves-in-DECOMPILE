using System;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class MoveCommand
{
	// Token: 0x06001459 RID: 5209 RVA: 0x0008BB5C File Offset: 0x00089D5C
	public static Command Create(Identity sender, Identity receiver, Vector2 position, bool flip)
	{
		Command command = new Command(Command.TYPE.MOVE, sender, receiver);
		command["position"] = position;
		command["flip"] = flip;
		return command;
	}
}
