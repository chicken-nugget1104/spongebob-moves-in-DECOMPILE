using System;

// Token: 0x02000293 RID: 659
public class ClearCommand
{
	// Token: 0x0600148C RID: 5260 RVA: 0x0008BE98 File Offset: 0x0008A098
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CLEAR, sender, receiver);
	}
}
