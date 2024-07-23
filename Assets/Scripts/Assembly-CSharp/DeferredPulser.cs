using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class DeferredPulser
{
	// Token: 0x06000357 RID: 855 RVA: 0x00010FA4 File Offset: 0x0000F1A4
	public DeferredPulser(Vector2 restingSize, float amplitude, float period, Action onUpdateCallback, Action onCompleteCallback)
	{
		this.restingSize = restingSize;
		this.amplitude = amplitude;
		this.period = period;
		this.onUpdateCallback = onUpdateCallback;
		this.onCompleteCallback = onCompleteCallback;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00010FD4 File Offset: 0x0000F1D4
	public void PulseOneShot()
	{
		this.PulseOneShot(1);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00010FE0 File Offset: 0x0000F1E0
	public void PulseOneShot(int count)
	{
		if (this.mixin == null)
		{
			this.mixin = this.Create();
		}
		this.mixin.PulseOneShot(count);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0001100C File Offset: 0x0000F20C
	public void PulseStartLoop()
	{
		if (this.mixin == null)
		{
			this.mixin = this.Create();
		}
		this.mixin.PulseStartLoop();
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00011044 File Offset: 0x0000F244
	public void PulseStopLoop()
	{
		if (this.mixin != null)
		{
			this.mixin.PulseStopLoop();
		}
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00011064 File Offset: 0x0000F264
	public void Destroy()
	{
		if (this.mixin != null)
		{
			this.mixin.Release();
			this.mixin = null;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600035D RID: 861 RVA: 0x0001108C File Offset: 0x0000F28C
	public Vector2 Size
	{
		get
		{
			return (!(this.mixin == null)) ? this.mixin.Size : Vector2.one;
		}
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000110C0 File Offset: 0x0000F2C0
	private PulserMixin Create()
	{
		PulserMixin pulserMixin = PulserMixin.Create();
		pulserMixin.Initialize(this.restingSize, this.amplitude, this.period, this.onUpdateCallback, this.onCompleteCallback);
		return pulserMixin;
	}

	// Token: 0x04000237 RID: 567
	private Vector2 restingSize;

	// Token: 0x04000238 RID: 568
	private float amplitude;

	// Token: 0x04000239 RID: 569
	private float period;

	// Token: 0x0400023A RID: 570
	private Action onUpdateCallback;

	// Token: 0x0400023B RID: 571
	private Action onCompleteCallback;

	// Token: 0x0400023C RID: 572
	private PulserMixin mixin;
}
