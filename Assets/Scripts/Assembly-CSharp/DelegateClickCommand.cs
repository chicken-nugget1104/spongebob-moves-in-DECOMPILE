using System;

// Token: 0x02000288 RID: 648
public class DelegateClickCommand
{
	// Token: 0x06001476 RID: 5238 RVA: 0x0008BD50 File Offset: 0x00089F50
	public static Command Create(Identity sender, Identity receiver)
	{
		return new Command(Command.TYPE.DELEGATE_CLICK, sender, receiver);
	}
}
