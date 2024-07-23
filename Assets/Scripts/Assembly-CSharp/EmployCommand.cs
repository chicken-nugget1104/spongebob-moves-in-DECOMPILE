using System;

// Token: 0x0200027D RID: 637
public class EmployCommand
{
	// Token: 0x06001460 RID: 5216 RVA: 0x0008BBFC File Offset: 0x00089DFC
	public static Command Create(Identity sender, Identity receiver, Identity employee)
	{
		Command command = new Command(Command.TYPE.EMPLOY, sender, receiver);
		command["employee"] = employee;
		return command;
	}
}
