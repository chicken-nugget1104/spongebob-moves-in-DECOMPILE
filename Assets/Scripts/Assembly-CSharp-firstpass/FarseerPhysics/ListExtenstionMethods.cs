using System;
using System.Collections.Generic;

namespace FarseerPhysics
{
	// Token: 0x02000070 RID: 112
	public static class ListExtenstionMethods
	{
		// Token: 0x060003CE RID: 974 RVA: 0x00010620 File Offset: 0x0000E820
		public static float Min(this List<float> list)
		{
			float num = float.MaxValue;
			for (int i = 0; i < list.Count; i++)
			{
				num = ((num.CompareTo(list[i]) != 1) ? num : list[i]);
			}
			return num;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00010670 File Offset: 0x0000E870
		public static float Max(this List<float> list)
		{
			float num = float.MinValue;
			for (int i = 0; i < list.Count; i++)
			{
				num = ((num.CompareTo(list[i]) != -1) ? num : list[i]);
			}
			return num;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000106C0 File Offset: 0x0000E8C0
		public static float Average(this List<float> list)
		{
			float num = 0f;
			int count = list.Count;
			if (count == 0)
			{
				return 0f;
			}
			for (int i = 0; i < count; i++)
			{
				num += list[i];
			}
			return num / (float)count;
		}
	}
}
