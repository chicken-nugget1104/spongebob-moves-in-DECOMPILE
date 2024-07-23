using System;

// Token: 0x020002A1 RID: 673
public class BonusRewardCommand
{
	// Token: 0x060014A8 RID: 5288 RVA: 0x0008BFDC File Offset: 0x0008A1DC
	public static Command Create(Identity sender, Identity receiver, int sourceID)
	{
		Command command = new Command(Command.TYPE.BONUS_REWARD, sender, receiver);
		command["source_id"] = sourceID;
		return command;
	}
}
