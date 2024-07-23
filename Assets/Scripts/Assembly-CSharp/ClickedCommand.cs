using System;

// Token: 0x02000287 RID: 647
public class ClickedCommand
{
	// Token: 0x06001474 RID: 5236 RVA: 0x0008BD3C File Offset: 0x00089F3C
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.CLICKED, sender, receiver);
	}
}
