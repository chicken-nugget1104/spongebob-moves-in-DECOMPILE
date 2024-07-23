using System;

namespace Helpshift
{
	// Token: 0x0200002C RID: 44
	public class HelpshiftLog
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x00009348 File Offset: 0x00007548
		public static int v(string tag, string log)
		{
			return HelpshiftAndroidLog.v(tag, log);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009354 File Offset: 0x00007554
		public static int d(string tag, string log)
		{
			return HelpshiftAndroidLog.d(tag, log);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009360 File Offset: 0x00007560
		public static int i(string tag, string log)
		{
			return HelpshiftAndroidLog.i(tag, log);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000936C File Offset: 0x0000756C
		public static int w(string tag, string log)
		{
			return HelpshiftAndroidLog.w(tag, log);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00009378 File Offset: 0x00007578
		public static int e(string tag, string log)
		{
			return HelpshiftAndroidLog.e(tag, log);
		}
	}
}
