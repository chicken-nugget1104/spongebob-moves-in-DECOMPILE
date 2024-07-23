using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class LoadingFunnel : MonoBehaviour
{
	// Token: 0x06001E5F RID: 7775 RVA: 0x000BBAA8 File Offset: 0x000B9CA8
	public void Initialize(ref Dictionary<string, object> commonData)
	{
		this.lastTime = Time.time;
		this.LogStep("UnityInitialized", ref commonData);
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x000BBACC File Offset: 0x000B9CCC
	public void LogStep(string stepName, ref Dictionary<string, object> eventData)
	{
		this.logRequests.Enqueue(new LoadingFunnel.LogInfo(stepName, eventData));
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x000BBAE4 File Offset: 0x000B9CE4
	public void Update()
	{
		while (this.logRequests.Count > 0)
		{
			float num = Time.time - this.lastTime;
			this.lastTime = Time.time;
			LoadingFunnel.LogInfo logInfo = this.logRequests.Dequeue();
			logInfo.eventData["value"] = (int)num;
			logInfo.eventData["subtype1"] = "LoadFunnel";
			TFAnalytics.LogEvent(logInfo.stepName, logInfo.eventData);
		}
	}

	// Token: 0x040012C8 RID: 4808
	private float lastTime;

	// Token: 0x040012C9 RID: 4809
	private Queue<LoadingFunnel.LogInfo> logRequests = new Queue<LoadingFunnel.LogInfo>();

	// Token: 0x020003E0 RID: 992
	private struct LogInfo
	{
		// Token: 0x06001E62 RID: 7778 RVA: 0x000BBB6C File Offset: 0x000B9D6C
		public LogInfo(string stepName, Dictionary<string, object> eventData)
		{
			this.stepName = stepName;
			this.eventData = eventData;
		}

		// Token: 0x040012CA RID: 4810
		public string stepName;

		// Token: 0x040012CB RID: 4811
		public Dictionary<string, object> eventData;
	}
}
