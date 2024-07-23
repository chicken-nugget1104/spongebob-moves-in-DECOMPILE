using System;

// Token: 0x020002A0 RID: 672
public class StandCommand
{
	// Token: 0x060014A6 RID: 5286 RVA: 0x0008BFC8 File Offset: 0x0008A1C8
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.STAND, sender, receiver);
	}
}
