using System;

// Token: 0x02000286 RID: 646
public class ExpireCommand
{
	// Token: 0x06001472 RID: 5234 RVA: 0x0008BD28 File Offset: 0x00089F28
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.EXPIRE, sender, receiver);
	}
}
