using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class PulserMixin : MonoBehaviour
{
	// Token: 0x06000364 RID: 868 RVA: 0x00011190 File Offset: 0x0000F390
	private PulserMixin()
	{
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000366 RID: 870 RVA: 0x000111B8 File Offset: 0x0000F3B8
	public Vector2 Size
	{
		get
		{
			return this.currentSize;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000367 RID: 871 RVA: 0x000111C0 File Offset: 0x0000F3C0
	public Vector2 RestingSize
	{
		get
		{
			return this.restingSize;
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x000111C8 File Offset: 0x0000F3C8
	public static PulserMixin Create()
	{
		return PulserMixin.pool.Create(delegate
		{
			GameObject gameObject = new GameObject("PulserMixin_" + PulserMixin.nextId++);
			PulserMixin pulserMixin = gameObject.AddComponent<PulserMixin>();
			pulserMixin.currentSize = pulserMixin.restingSize;
			return gameObject;
		}).GetComponent<PulserMixin>();
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00011204 File Offset: 0x0000F404
	public void Initialize(Vector2 restingSize, float amplitude, float period)
	{
		this.Initialize(restingSize, amplitude, period, null, null);
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00011214 File Offset: 0x0000F414
	public void Initialize(Vector2 restingSize, float amplitude, float period, Action updateCallback, Action completeCallback)
	{
		TFUtils.Assert(restingSize.x >= 0f && restingSize.y >= 0f, "Should only pulse to non-negative scales");
		this.restingSize = restingSize;
		this.amplitude = amplitude;
		this.period = period;
		this.updateCallback = updateCallback;
		this.completeCallback = completeCallback;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00011278 File Offset: 0x0000F478
	public void Destroy()
	{
		if (this == null)
		{
			return;
		}
		base.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00011298 File Offset: 0x0000F498
	public void Release()
	{
		if (this == null)
		{
			return;
		}
		base.StopAllCoroutines();
		this.count = 0;
		this.isRunning = false;
		this.currentSize = this.restingSize;
		PulserMixin.pool.Release(base.gameObject);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x000112E4 File Offset: 0x0000F4E4
	public void PulseOneShot()
	{
		this.PulseOneShot(1);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x000112F0 File Offset: 0x0000F4F0
	public void PulseOneShot(int count)
	{
		object obj = this.controlLock;
		lock (obj)
		{
			this.count += count;
			this.isLooped = false;
			this.StartPulseMachine();
		}
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00011350 File Offset: 0x0000F550
	public void PulseStartLoop()
	{
		object obj = this.controlLock;
		lock (obj)
		{
			this.count = 0;
			this.isLooped = true;
			this.StartPulseMachine();
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x000113A8 File Offset: 0x0000F5A8
	public void PulseStopLoop()
	{
		this.PulseStopLoop(false);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x000113B4 File Offset: 0x0000F5B4
	public void PulseStopLoop(bool hardStop)
	{
		object obj = this.controlLock;
		lock (obj)
		{
			this.isLooped = false;
			this.count = 0;
			if (hardStop)
			{
				this.isRunning = false;
			}
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00011414 File Offset: 0x0000F614
	private void StartPulseMachine()
	{
		if (!this.isRunning)
		{
			this.isRunning = true;
			base.StartCoroutine(this.PulseMachineRun());
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00011438 File Offset: 0x0000F638
	private IEnumerator PulseMachineRun()
	{
		PeriodicPattern pattern = new Sinusoid(1f, this.amplitude, this.period, 0f);
		while (this.isRunning)
		{
			float t = 0f;
			while (this.isRunning && t <= this.period)
			{
				float v = pattern.ValueAtTime(t);
				t += Time.deltaTime;
				this.currentSize = this.restingSize * v;
				if (this.updateCallback != null)
				{
					this.updateCallback();
				}
				yield return null;
			}
			object obj = this.controlLock;
			lock (obj)
			{
				this.count--;
				if (!this.isLooped && this.count <= 0)
				{
					this.isRunning = false;
					break;
				}
			}
		}
		this.currentSize = this.restingSize;
		if (this.updateCallback != null)
		{
			this.updateCallback();
		}
		if (this.completeCallback != null)
		{
			this.completeCallback();
		}
		yield break;
	}

	// Token: 0x04000244 RID: 580
	private static int nextId = 0;

	// Token: 0x04000245 RID: 581
	private Action updateCallback;

	// Token: 0x04000246 RID: 582
	private Action completeCallback;

	// Token: 0x04000247 RID: 583
	private Vector2 restingSize;

	// Token: 0x04000248 RID: 584
	private Vector2 currentSize;

	// Token: 0x04000249 RID: 585
	private float amplitude;

	// Token: 0x0400024A RID: 586
	private float period;

	// Token: 0x0400024B RID: 587
	private readonly object controlLock = new object();

	// Token: 0x0400024C RID: 588
	private int count;

	// Token: 0x0400024D RID: 589
	private bool isLooped;

	// Token: 0x0400024E RID: 590
	private bool isRunning;

	// Token: 0x0400024F RID: 591
	private static TFPool<GameObject> pool = new TFPool<GameObject>();
}
