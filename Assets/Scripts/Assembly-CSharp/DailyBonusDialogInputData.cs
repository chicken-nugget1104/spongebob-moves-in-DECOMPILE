using System;
using System.Collections.Generic;

// Token: 0x0200015E RID: 350
public class DailyBonusDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C02 RID: 3074 RVA: 0x00048788 File Offset: 0x00046988
	public DailyBonusDialogInputData() : base(uint.MaxValue, "daily_bonus", null, null)
	{
		this.dailyBonusData = SBMISoaring.GetCachedDailyBonus(ref this.currentDay, ref this.alreadyCollected);
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000C03 RID: 3075 RVA: 0x000487C4 File Offset: 0x000469C4
	public SoaringArray<SBMISoaring.SBMIDailyBonusDay> DailyBonusData
	{
		get
		{
			return this.dailyBonusData;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000C04 RID: 3076 RVA: 0x000487CC File Offset: 0x000469CC
	public int CurrentDay
	{
		get
		{
			return this.currentDay;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000C05 RID: 3077 RVA: 0x000487D4 File Offset: 0x000469D4
	public bool AlreadyCollected
	{
		get
		{
			return this.alreadyCollected;
		}
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x000487DC File Offset: 0x000469DC
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref dictionary, "daily_bonus");
		dictionary["current_day"] = this.currentDay;
		dictionary["already_collected"] = this.alreadyCollected;
		dictionary["dailyBonus_data"] = this.dailyBonusData;
		return dictionary;
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x0004883C File Offset: 0x00046A3C
	public new static DailyBonusDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		return new DailyBonusDialogInputData();
	}

	// Token: 0x0400080C RID: 2060
	public const string DIALOG_TYPE = "daily_bonus";

	// Token: 0x0400080D RID: 2061
	private int currentDay = -1;

	// Token: 0x0400080E RID: 2062
	private bool alreadyCollected;

	// Token: 0x0400080F RID: 2063
	private SoaringArray<SBMISoaring.SBMIDailyBonusDay> dailyBonusData;
}
