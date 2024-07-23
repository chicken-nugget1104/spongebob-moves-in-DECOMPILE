using System;

// Token: 0x020003C8 RID: 968
public class SoaringSession
{
	// Token: 0x06001CE8 RID: 7400 RVA: 0x000B757C File Offset: 0x000B577C
	public static string GetSoaringSessionTypeString(SoaringSession.SessionType type)
	{
		return SoaringSession.SoaringSessionStringList.GetSoaringSessionString(type);
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x000B7584 File Offset: 0x000B5784
	public static string GetSoaringSessionQueryTypeString(SoaringSession.QueryType type)
	{
		return SoaringSession.SoaringSessionStringList.GetSoaringSessionQueryTypeString(type);
	}

	// Token: 0x020003C9 RID: 969
	public enum SessionType
	{
		// Token: 0x0400127D RID: 4733
		OneWay,
		// Token: 0x0400127E RID: 4734
		PersistantOneWay
	}

	// Token: 0x020003CA RID: 970
	public enum QueryType
	{
		// Token: 0x04001280 RID: 4736
		Random,
		// Token: 0x04001281 RID: 4737
		List,
		// Token: 0x04001282 RID: 4738
		Range,
		// Token: 0x04001283 RID: 4739
		List2
	}

	// Token: 0x020003CB RID: 971
	public static class SoaringSessionStringList
	{
		// Token: 0x06001CEA RID: 7402 RVA: 0x000B758C File Offset: 0x000B578C
		static SoaringSessionStringList()
		{
			SoaringSession.SoaringSessionStringList.kSoaringSessionType[0] = "one-way";
			SoaringSession.SoaringSessionStringList.kSoaringSessionType[1] = "persistent-one-way";
			SoaringSession.SoaringSessionStringList.kSoaringQueryType = new string[4];
			SoaringSession.SoaringSessionStringList.kSoaringQueryType[0] = "random";
			SoaringSession.SoaringSessionStringList.kSoaringQueryType[2] = "range";
			SoaringSession.SoaringSessionStringList.kSoaringQueryType[1] = "list";
			SoaringSession.SoaringSessionStringList.kSoaringQueryType[3] = "list2";
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x000B75F8 File Offset: 0x000B57F8
		public static string GetSoaringSessionString(SoaringSession.SessionType type)
		{
			return SoaringSession.SoaringSessionStringList.kSoaringSessionType[(int)type];
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x000B7604 File Offset: 0x000B5804
		public static string GetSoaringSessionQueryTypeString(SoaringSession.QueryType type)
		{
			return SoaringSession.SoaringSessionStringList.kSoaringQueryType[(int)type];
		}

		// Token: 0x04001284 RID: 4740
		private static string[] kSoaringSessionType = new string[2];

		// Token: 0x04001285 RID: 4741
		private static string[] kSoaringQueryType;
	}
}
