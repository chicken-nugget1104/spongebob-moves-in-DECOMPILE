using System;

// Token: 0x02000298 RID: 664
public class ResumeFullCommand
{
	// Token: 0x06001496 RID: 5270 RVA: 0x0008BF08 File Offset: 0x0008A108
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.RESUME_FULL, sender, receiver);
	}
}
