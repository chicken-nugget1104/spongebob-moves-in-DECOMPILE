using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class SBStatisticsTracker : MonoBehaviour
{
	// Token: 0x0600033B RID: 827 RVA: 0x00010500 File Offset: 0x0000E700
	public SBStatisticsTracker()
	{
		this.initBuckets();
	}

	// Token: 0x17000064 RID: 100
	// (set) Token: 0x0600033C RID: 828 RVA: 0x00010548 File Offset: 0x0000E748
	public Session TheSession
	{
		set
		{
			this.session = value;
		}
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00010554 File Offset: 0x0000E754
	private void Start()
	{
		TFUtils.DebugLog("Tracking Framerate statistics");
		base.StartCoroutine(this.SendStatistics());
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00010570 File Offset: 0x0000E770
	public void OnApplicationPause(bool paused)
	{
		this.Paused = paused;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0001057C File Offset: 0x0000E77C
	private int getBucket(int lastFrameRenderMS)
	{
		for (int i = 0; i < 7; i++)
		{
			if (lastFrameRenderMS >= this.lowRanges[i] && lastFrameRenderMS < this.highRanges[i])
			{
				return i;
			}
		}
		TFUtils.ErrorLog("Error. Last Render MS " + lastFrameRenderMS + " does not fit in a bucket");
		return 7;
	}

	// Token: 0x06000340 RID: 832 RVA: 0x000105D4 File Offset: 0x0000E7D4
	private string getBucketName(int bucket)
	{
		return string.Format("{0}to{1}", this.lowRanges[bucket], this.highRanges[bucket]);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00010608 File Offset: 0x0000E808
	private void initBuckets()
	{
		this.frameRateBuckets = new int[7];
		for (int i = 0; i < 7; i++)
		{
			this.frameRateBuckets[i] = 0;
		}
	}

	// Token: 0x06000342 RID: 834 RVA: 0x0001063C File Offset: 0x0000E83C
	private void Update()
	{
		this.renderTime += 1f;
		this.totalFrames++;
		int lastFrameRenderMS = (int)(Time.deltaTime * 1000f);
		int bucket = this.getBucket(lastFrameRenderMS);
		this.frameRateBuckets[bucket]++;
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00010690 File Offset: 0x0000E890
	private IEnumerator SendStatistics()
	{
		for (;;)
		{
			if (!this.Paused)
			{
				for (int i = 0; i < 7; i++)
				{
					this.session.Analytics.LogFrameRenderRates(this.getBucketName(i), this.frameRateBuckets[i]);
				}
				this.initBuckets();
			}
			yield return new WaitForSeconds(SBSettings.StatisticsTrackingInterval);
		}
		yield break;
	}

	// Token: 0x04000228 RID: 552
	private const int BUCKET_COUNT = 7;

	// Token: 0x04000229 RID: 553
	private int acceptableFrameCount;

	// Token: 0x0400022A RID: 554
	private int totalFrames;

	// Token: 0x0400022B RID: 555
	private long lastTick;

	// Token: 0x0400022C RID: 556
	private float renderTime;

	// Token: 0x0400022D RID: 557
	private int[] frameRateBuckets;

	// Token: 0x0400022E RID: 558
	private Session session;

	// Token: 0x0400022F RID: 559
	private int[] lowRanges = new int[]
	{
		0,
		30,
		50,
		75,
		100,
		150,
		200
	};

	// Token: 0x04000230 RID: 560
	private int[] highRanges = new int[]
	{
		30,
		50,
		75,
		100,
		150,
		200,
		99999999
	};

	// Token: 0x04000231 RID: 561
	public bool Paused;
}
