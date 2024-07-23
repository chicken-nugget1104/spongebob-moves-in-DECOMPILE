using System;
using System.Collections.Generic;

// Token: 0x020001AC RID: 428
public class AutoQuestData
{
	// Token: 0x06000E47 RID: 3655 RVA: 0x00057104 File Offset: 0x00055304
	public AutoQuestData(Dictionary<string, object> pData)
	{
		this.m_nDID = TFUtils.LoadInt(pData, "did");
		this.m_nMinItems = TFUtils.LoadInt(pData, "min_items");
		this.m_nMaxItems = TFUtils.LoadInt(pData, "max_items");
		this.m_fExpMultiplier = TFUtils.LoadFloat(pData, "exp_multiplier");
		this.m_fGoldMultiplier = TFUtils.LoadFloat(pData, "gold_multiplier");
		this.m_sName = TFUtils.LoadString(pData, "name");
		this.m_sDescription = TFUtils.LoadString(pData, "description");
		this.m_pCharacters = TFUtils.LoadList<int>(pData, "characters").ToArray();
		this.m_pItemCategories = TFUtils.LoadList<string>(pData, "item_categories").ToArray();
		this.m_pPickOneCategories = TFUtils.LoadList<bool>(pData, "pick_one_categories").ToArray();
		this.m_eDistribution = ((!(TFUtils.LoadString(pData, "distribution") == "equal")) ? AutoQuestData.eDistributionType.eRandom : AutoQuestData.eDistributionType.eEqual);
		Dictionary<string, object> dictionary = TFUtils.TryLoadDict(pData, "intro_dialog_data");
		Dictionary<string, object> dictionary2 = TFUtils.TryLoadDict(pData, "outro_dialog_data");
		this.m_pDialogData = new Dictionary<int, AutoQuestData.DialogData>();
		if (dictionary != null && dictionary2 != null)
		{
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				if (dictionary2.ContainsKey(keyValuePair.Key))
				{
					this.m_pDialogData.Add(int.Parse(keyValuePair.Key), new AutoQuestData.DialogData((string)keyValuePair.Value, (string)dictionary2[keyValuePair.Key]));
				}
			}
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000E48 RID: 3656 RVA: 0x000572C4 File Offset: 0x000554C4
	// (set) Token: 0x06000E49 RID: 3657 RVA: 0x000572CC File Offset: 0x000554CC
	public int m_nDID { get; private set; }

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000E4A RID: 3658 RVA: 0x000572D8 File Offset: 0x000554D8
	// (set) Token: 0x06000E4B RID: 3659 RVA: 0x000572E0 File Offset: 0x000554E0
	public int m_nMinItems { get; private set; }

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000E4C RID: 3660 RVA: 0x000572EC File Offset: 0x000554EC
	// (set) Token: 0x06000E4D RID: 3661 RVA: 0x000572F4 File Offset: 0x000554F4
	public int m_nMaxItems { get; private set; }

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000E4E RID: 3662 RVA: 0x00057300 File Offset: 0x00055500
	// (set) Token: 0x06000E4F RID: 3663 RVA: 0x00057308 File Offset: 0x00055508
	public float m_fExpMultiplier { get; private set; }

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00057314 File Offset: 0x00055514
	// (set) Token: 0x06000E51 RID: 3665 RVA: 0x0005731C File Offset: 0x0005551C
	public float m_fGoldMultiplier { get; private set; }

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00057328 File Offset: 0x00055528
	// (set) Token: 0x06000E53 RID: 3667 RVA: 0x00057330 File Offset: 0x00055530
	public string m_sName { get; private set; }

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0005733C File Offset: 0x0005553C
	// (set) Token: 0x06000E55 RID: 3669 RVA: 0x00057344 File Offset: 0x00055544
	public string m_sDescription { get; private set; }

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000E56 RID: 3670 RVA: 0x00057350 File Offset: 0x00055550
	// (set) Token: 0x06000E57 RID: 3671 RVA: 0x00057358 File Offset: 0x00055558
	public AutoQuestData.eDistributionType m_eDistribution { get; private set; }

	// Token: 0x06000E58 RID: 3672 RVA: 0x00057364 File Offset: 0x00055564
	public int[] GetCharacters()
	{
		return (int[])this.m_pCharacters.Clone();
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x00057378 File Offset: 0x00055578
	public string[] GetItemCategories()
	{
		return (string[])this.m_pItemCategories.Clone();
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x0005738C File Offset: 0x0005558C
	public bool[] GetPickOneCategories()
	{
		return (bool[])this.m_pPickOneCategories.Clone();
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x000573A0 File Offset: 0x000555A0
	public AutoQuestData.DialogData GetDialogData(int nDID)
	{
		if (this.m_pDialogData.ContainsKey(nDID))
		{
			return this.m_pDialogData[nDID];
		}
		return null;
	}

	// Token: 0x0400096C RID: 2412
	private readonly int[] m_pCharacters;

	// Token: 0x0400096D RID: 2413
	private readonly string[] m_pItemCategories;

	// Token: 0x0400096E RID: 2414
	private readonly bool[] m_pPickOneCategories;

	// Token: 0x0400096F RID: 2415
	private readonly Dictionary<int, AutoQuestData.DialogData> m_pDialogData;

	// Token: 0x020001AD RID: 429
	public class DialogData
	{
		// Token: 0x06000E5C RID: 3676 RVA: 0x000573C4 File Offset: 0x000555C4
		public DialogData(string sIntroDialog, string sOutroDialog)
		{
			this.m_sIntroDialog = sIntroDialog;
			this.m_sOutroDialog = sOutroDialog;
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x000573DC File Offset: 0x000555DC
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x000573E4 File Offset: 0x000555E4
		public string m_sIntroDialog { get; private set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x000573F0 File Offset: 0x000555F0
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x000573F8 File Offset: 0x000555F8
		public string m_sOutroDialog { get; private set; }
	}

	// Token: 0x020001AE RID: 430
	public enum eDistributionType
	{
		// Token: 0x0400097B RID: 2427
		eEqual,
		// Token: 0x0400097C RID: 2428
		eRandom,
		// Token: 0x0400097D RID: 2429
		eNumTypes
	}
}
