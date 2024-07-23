using System;

// Token: 0x0200029B RID: 667
public class StoreResidentCommand
{
	// Token: 0x0600149C RID: 5276 RVA: 0x0008BF64 File Offset: 0x0008A164
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.STORE_RESIDENT, sender, receiver);
	}
}
