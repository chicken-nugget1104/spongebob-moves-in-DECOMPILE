using System;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000012 RID: 18
	public class AmazonLogger
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00003CEC File Offset: 0x00001EEC
		public AmazonLogger(string tag)
		{
			this.tag = tag;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003CFC File Offset: 0x00001EFC
		public void Debug(string msg)
		{
			AmazonLogging.Log(AmazonLogging.AmazonLoggingLevel.Verbose, this.tag, msg);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003D0C File Offset: 0x00001F0C
		public string getTag()
		{
			return this.tag;
		}

		// Token: 0x04000031 RID: 49
		private readonly string tag;
	}
}
