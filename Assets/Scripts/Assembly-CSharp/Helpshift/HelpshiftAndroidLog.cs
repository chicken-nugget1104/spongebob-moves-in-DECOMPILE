using System;
using UnityEngine;

namespace Helpshift
{
	// Token: 0x02000026 RID: 38
	public class HelpshiftAndroidLog
	{
		// Token: 0x06000180 RID: 384 RVA: 0x00008508 File Offset: 0x00006708
		private HelpshiftAndroidLog()
		{
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008514 File Offset: 0x00006714
		private static void initLogger()
		{
			if (HelpshiftAndroidLog.logger == null)
			{
				HelpshiftAndroidLog.logger = new AndroidJavaClass("com.helpshift.Log");
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008530 File Offset: 0x00006730
		public static int v(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			return HelpshiftAndroidLog.logger.CallStatic<int>("v", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008560 File Offset: 0x00006760
		public static int d(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			return HelpshiftAndroidLog.logger.CallStatic<int>("d", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008590 File Offset: 0x00006790
		public static int i(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			return HelpshiftAndroidLog.logger.CallStatic<int>("i", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000085C0 File Offset: 0x000067C0
		public static int w(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			return HelpshiftAndroidLog.logger.CallStatic<int>("w", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000085F0 File Offset: 0x000067F0
		public static int e(string tag, string log)
		{
			HelpshiftAndroidLog.initLogger();
			return HelpshiftAndroidLog.logger.CallStatic<int>("e", new object[]
			{
				tag,
				log
			});
		}

		// Token: 0x040000D7 RID: 215
		private static AndroidJavaClass logger;
	}
}
