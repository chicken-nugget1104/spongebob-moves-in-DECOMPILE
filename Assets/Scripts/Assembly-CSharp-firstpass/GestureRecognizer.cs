using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
public abstract class GestureRecognizer : FGComponent
{
	// Token: 0x1400002A RID: 42
	// (add) Token: 0x060003F8 RID: 1016 RVA: 0x00011450 File Offset: 0x0000F650
	// (remove) Token: 0x060003F9 RID: 1017 RVA: 0x0001146C File Offset: 0x0000F66C
	public event FGComponent.EventDelegate<GestureRecognizer> OnStateChanged;

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x00011488 File Offset: 0x0000F688
	public GestureRecognizer.GestureState PreviousState
	{
		get
		{
			return this.prevState;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x00011490 File Offset: 0x0000F690
	// (set) Token: 0x060003FC RID: 1020 RVA: 0x00011498 File Offset: 0x0000F698
	public GestureRecognizer.GestureState State
	{
		get
		{
			return this.state;
		}
		protected set
		{
			if (this.state != value)
			{
				this.prevState = this.state;
				this.state = value;
				if (this.OnStateChanged != null)
				{
					this.OnStateChanged(this);
				}
			}
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x000114DC File Offset: 0x0000F6DC
	public bool IsActive
	{
		get
		{
			return this.State == GestureRecognizer.GestureState.InProgress;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060003FE RID: 1022 RVA: 0x000114E8 File Offset: 0x0000F6E8
	// (set) Token: 0x060003FF RID: 1023 RVA: 0x000114F0 File Offset: 0x0000F6F0
	public float StartTime
	{
		get
		{
			return this.startTime;
		}
		protected set
		{
			this.startTime = value;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000400 RID: 1024 RVA: 0x000114FC File Offset: 0x0000F6FC
	public float ElapsedTime
	{
		get
		{
			return Time.time - this.startTime;
		}
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001150C File Offset: 0x0000F70C
	protected virtual void Reset()
	{
		this.State = GestureRecognizer.GestureState.Ready;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00011518 File Offset: 0x0000F718
	protected override void Start()
	{
		base.Start();
		this.Reset();
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00011528 File Offset: 0x0000F728
	protected virtual void OnTouchSequenceStarted()
	{
		if (this.ResetMode == GestureRecognizer.GestureResetMode.StartOfTouchSequence && (this.State == GestureRecognizer.GestureState.Recognized || this.State == GestureRecognizer.GestureState.Failed))
		{
			this.Reset();
		}
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00011560 File Offset: 0x0000F760
	protected virtual void OnTouchSequenceEnded()
	{
		if (this.ResetMode == GestureRecognizer.GestureResetMode.EndOfTouchSequence && (this.State == GestureRecognizer.GestureState.Recognized || this.State == GestureRecognizer.GestureState.Failed))
		{
			this.Reset();
		}
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x00011598 File Offset: 0x0000F798
	protected override void OnUpdate(FingerGestures.IFingerList touches)
	{
		if (this.touchFilter != null)
		{
			touches = this.touchFilter.Apply(touches);
		}
		if (touches.Count > 0 && this.lastTouchesCount == 0)
		{
			this.OnTouchSequenceStarted();
		}
		switch (this.State)
		{
		case GestureRecognizer.GestureState.Ready:
			this.State = this.OnReady(touches);
			break;
		case GestureRecognizer.GestureState.InProgress:
			this.State = this.OnActive(touches);
			break;
		case GestureRecognizer.GestureState.Failed:
		case GestureRecognizer.GestureState.Recognized:
			if (this.ResetMode == GestureRecognizer.GestureResetMode.NextFrame)
			{
				this.Reset();
			}
			break;
		default:
			Debug.LogError(string.Concat(new object[]
			{
				this,
				" - Unhandled state: ",
				this.State,
				". Failing recognizer."
			}));
			this.State = GestureRecognizer.GestureState.Failed;
			break;
		}
		if (touches.Count == 0 && this.lastTouchesCount > 0)
		{
			this.OnTouchSequenceEnded();
		}
		this.lastTouchesCount = touches.Count;
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x000116A0 File Offset: 0x0000F8A0
	protected virtual GestureRecognizer.GestureState OnReady(FingerGestures.IFingerList touches)
	{
		if (this.ShouldFailFromReady(touches))
		{
			return GestureRecognizer.GestureState.Failed;
		}
		if (this.CanBegin(touches))
		{
			this.StartTime = Time.time;
			this.OnBegin(touches);
			return GestureRecognizer.GestureState.InProgress;
		}
		return GestureRecognizer.GestureState.Ready;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x000116DC File Offset: 0x0000F8DC
	protected virtual bool ShouldFailFromReady(FingerGestures.IFingerList touches)
	{
		return touches.Count != this.GetRequiredFingerCount() && touches.Count > 0 && !this.Young(touches);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x00011718 File Offset: 0x0000F918
	protected virtual bool CanBegin(FingerGestures.IFingerList touches)
	{
		return touches.Count == this.GetRequiredFingerCount() && this.CheckCanBeginDelegate(touches);
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00011748 File Offset: 0x0000F948
	public virtual bool CheckCanBeginDelegate(FingerGestures.IFingerList touches)
	{
		return this.canBeginDelegate == null || this.canBeginDelegate(this, touches);
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00011778 File Offset: 0x0000F978
	public void SetCanBeginDelegate(GestureRecognizer.CanBeginDelegate f)
	{
		this.canBeginDelegate = f;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00011784 File Offset: 0x0000F984
	public GestureRecognizer.CanBeginDelegate GetCanBeginDelegate()
	{
		return this.canBeginDelegate;
	}

	// Token: 0x0600040C RID: 1036
	protected abstract int GetRequiredFingerCount();

	// Token: 0x0600040D RID: 1037
	protected abstract void OnBegin(FingerGestures.IFingerList touches);

	// Token: 0x0600040E RID: 1038
	protected abstract GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches);

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001178C File Offset: 0x0000F98C
	// (set) Token: 0x06000410 RID: 1040 RVA: 0x00011794 File Offset: 0x0000F994
	public FingerGestures.ITouchFilter TouchFilter
	{
		get
		{
			return this.touchFilter;
		}
		set
		{
			this.touchFilter = value;
		}
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x000117A0 File Offset: 0x0000F9A0
	protected bool Young(FingerGestures.IFingerList touches)
	{
		FingerGestures.Finger oldest = touches.GetOldest();
		if (oldest == null)
		{
			return false;
		}
		float num = Time.time - oldest.StarTime;
		return num < 0.25f;
	}

	// Token: 0x04000219 RID: 537
	private GestureRecognizer.GestureState prevState;

	// Token: 0x0400021A RID: 538
	private GestureRecognizer.GestureState state;

	// Token: 0x0400021B RID: 539
	private float startTime;

	// Token: 0x0400021C RID: 540
	public GestureRecognizer.GestureResetMode ResetMode = GestureRecognizer.GestureResetMode.StartOfTouchSequence;

	// Token: 0x0400021D RID: 541
	private int lastTouchesCount;

	// Token: 0x0400021E RID: 542
	private GestureRecognizer.CanBeginDelegate canBeginDelegate;

	// Token: 0x0400021F RID: 543
	private FingerGestures.ITouchFilter touchFilter;

	// Token: 0x02000076 RID: 118
	public enum GestureState
	{
		// Token: 0x04000222 RID: 546
		Ready,
		// Token: 0x04000223 RID: 547
		InProgress,
		// Token: 0x04000224 RID: 548
		Failed,
		// Token: 0x04000225 RID: 549
		Recognized
	}

	// Token: 0x02000077 RID: 119
	public enum GestureResetMode
	{
		// Token: 0x04000227 RID: 551
		NextFrame,
		// Token: 0x04000228 RID: 552
		EndOfTouchSequence,
		// Token: 0x04000229 RID: 553
		StartOfTouchSequence
	}

	// Token: 0x02000115 RID: 277
	// (Invoke) Token: 0x06000A1A RID: 2586
	public delegate bool CanBeginDelegate(GestureRecognizer gr, FingerGestures.IFingerList touches);
}
