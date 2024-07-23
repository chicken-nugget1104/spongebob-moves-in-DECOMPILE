using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class FingerMotionDetector : FGComponent
{
	// Token: 0x1400002F RID: 47
	// (add) Token: 0x0600042F RID: 1071 RVA: 0x00011B1C File Offset: 0x0000FD1C
	// (remove) Token: 0x06000430 RID: 1072 RVA: 0x00011B38 File Offset: 0x0000FD38
	public event FGComponent.EventDelegate<FingerMotionDetector> OnMoveBegin;

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06000431 RID: 1073 RVA: 0x00011B54 File Offset: 0x0000FD54
	// (remove) Token: 0x06000432 RID: 1074 RVA: 0x00011B70 File Offset: 0x0000FD70
	public event FGComponent.EventDelegate<FingerMotionDetector> OnMove;

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06000433 RID: 1075 RVA: 0x00011B8C File Offset: 0x0000FD8C
	// (remove) Token: 0x06000434 RID: 1076 RVA: 0x00011BA8 File Offset: 0x0000FDA8
	public event FGComponent.EventDelegate<FingerMotionDetector> OnMoveEnd;

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06000435 RID: 1077 RVA: 0x00011BC4 File Offset: 0x0000FDC4
	// (remove) Token: 0x06000436 RID: 1078 RVA: 0x00011BE0 File Offset: 0x0000FDE0
	public event FGComponent.EventDelegate<FingerMotionDetector> OnStationaryBegin;

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06000437 RID: 1079 RVA: 0x00011BFC File Offset: 0x0000FDFC
	// (remove) Token: 0x06000438 RID: 1080 RVA: 0x00011C18 File Offset: 0x0000FE18
	public event FGComponent.EventDelegate<FingerMotionDetector> OnStationary;

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06000439 RID: 1081 RVA: 0x00011C34 File Offset: 0x0000FE34
	// (remove) Token: 0x0600043A RID: 1082 RVA: 0x00011C50 File Offset: 0x0000FE50
	public event FGComponent.EventDelegate<FingerMotionDetector> OnStationaryEnd;

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600043B RID: 1083 RVA: 0x00011C6C File Offset: 0x0000FE6C
	// (set) Token: 0x0600043C RID: 1084 RVA: 0x00011C74 File Offset: 0x0000FE74
	public virtual FingerGestures.Finger Finger
	{
		get
		{
			return this.finger;
		}
		set
		{
			this.finger = value;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600043D RID: 1085 RVA: 0x00011C80 File Offset: 0x0000FE80
	// (set) Token: 0x0600043E RID: 1086 RVA: 0x00011C88 File Offset: 0x0000FE88
	private protected FingerMotionDetector.MotionState State
	{
		protected get
		{
			return this.state;
		}
		private set
		{
			this.state = value;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x0600043F RID: 1087 RVA: 0x00011C94 File Offset: 0x0000FE94
	// (set) Token: 0x06000440 RID: 1088 RVA: 0x00011C9C File Offset: 0x0000FE9C
	private protected FingerMotionDetector.MotionState PreviousState
	{
		protected get
		{
			return this.prevState;
		}
		private set
		{
			this.prevState = value;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000441 RID: 1089 RVA: 0x00011CA8 File Offset: 0x0000FEA8
	// (set) Token: 0x06000442 RID: 1090 RVA: 0x00011CB0 File Offset: 0x0000FEB0
	public int Moves
	{
		get
		{
			return this.moves;
		}
		private set
		{
			this.moves = value;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000443 RID: 1091 RVA: 0x00011CBC File Offset: 0x0000FEBC
	public bool Moved
	{
		get
		{
			return this.Moves > 0;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x00011CC8 File Offset: 0x0000FEC8
	public bool WasMoving
	{
		get
		{
			return this.PreviousState == FingerMotionDetector.MotionState.Moving;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000445 RID: 1093 RVA: 0x00011CD4 File Offset: 0x0000FED4
	public bool Moving
	{
		get
		{
			return this.State == FingerMotionDetector.MotionState.Moving;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x00011CE0 File Offset: 0x0000FEE0
	public float ElapsedStationaryTime
	{
		get
		{
			return Time.time - this.stationaryStartTime;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000447 RID: 1095 RVA: 0x00011CF0 File Offset: 0x0000FEF0
	// (set) Token: 0x06000448 RID: 1096 RVA: 0x00011CF8 File Offset: 0x0000FEF8
	public Vector2 AnchorPos
	{
		get
		{
			return this.anchorPos;
		}
		private set
		{
			this.anchorPos = value;
		}
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00011D04 File Offset: 0x0000FF04
	protected override void OnUpdate(FingerGestures.IFingerList touches)
	{
		if (this.Finger.IsDown)
		{
			if (!this.wasDown)
			{
				this.Moves = 0;
				this.AnchorPos = this.Finger.Position;
				this.State = FingerMotionDetector.MotionState.Stationary;
			}
			if (this.Finger.Phase == FingerGestures.FingerPhase.Moved)
			{
				if (this.State != FingerMotionDetector.MotionState.Moving)
				{
					if ((this.Finger.Position - this.AnchorPos).sqrMagnitude >= this.MoveThreshold * this.MoveThreshold)
					{
						this.State = FingerMotionDetector.MotionState.Moving;
					}
					else
					{
						this.State = FingerMotionDetector.MotionState.Stationary;
					}
				}
			}
			else
			{
				this.State = FingerMotionDetector.MotionState.Stationary;
			}
		}
		else
		{
			this.State = FingerMotionDetector.MotionState.None;
		}
		this.RaiseEvents();
		this.PreviousState = this.State;
		this.wasDown = this.Finger.IsDown;
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x00011DE8 File Offset: 0x0000FFE8
	private void RaiseEvents()
	{
		if (this.State != this.PreviousState)
		{
			if (this.PreviousState == FingerMotionDetector.MotionState.Moving)
			{
				this.RaiseOnMoveEnd();
				this.AnchorPos = this.Finger.Position;
			}
			else if (this.PreviousState == FingerMotionDetector.MotionState.Stationary)
			{
				this.RaiseOnStationaryEnd();
			}
			if (this.State == FingerMotionDetector.MotionState.Moving)
			{
				this.RaiseOnMoveBegin();
				this.Moves++;
			}
			else if (this.State == FingerMotionDetector.MotionState.Stationary)
			{
				this.stationaryStartTime = Time.time;
				this.RaiseOnStationaryBegin();
			}
		}
		if (this.State == FingerMotionDetector.MotionState.Stationary)
		{
			this.RaiseOnStationary();
		}
		else if (this.State == FingerMotionDetector.MotionState.Moving)
		{
			this.RaiseOnMove();
		}
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00011EAC File Offset: 0x000100AC
	protected void RaiseOnMoveBegin()
	{
		if (this.OnMoveBegin != null)
		{
			this.OnMoveBegin(this);
		}
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00011EC8 File Offset: 0x000100C8
	protected void RaiseOnMove()
	{
		if (this.OnMove != null)
		{
			this.OnMove(this);
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00011EE4 File Offset: 0x000100E4
	protected void RaiseOnMoveEnd()
	{
		if (this.OnMoveEnd != null)
		{
			this.OnMoveEnd(this);
		}
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00011F00 File Offset: 0x00010100
	protected void RaiseOnStationaryBegin()
	{
		if (this.OnStationaryBegin != null)
		{
			this.OnStationaryBegin(this);
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x00011F1C File Offset: 0x0001011C
	protected void RaiseOnStationary()
	{
		if (this.OnStationary != null)
		{
			this.OnStationary(this);
		}
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00011F38 File Offset: 0x00010138
	protected void RaiseOnStationaryEnd()
	{
		if (this.OnStationaryEnd != null)
		{
			this.OnStationaryEnd(this);
		}
	}

	// Token: 0x04000233 RID: 563
	public float MoveThreshold = 5f;

	// Token: 0x04000234 RID: 564
	private FingerGestures.Finger finger;

	// Token: 0x04000235 RID: 565
	private FingerMotionDetector.MotionState state;

	// Token: 0x04000236 RID: 566
	private FingerMotionDetector.MotionState prevState;

	// Token: 0x04000237 RID: 567
	private int moves;

	// Token: 0x04000238 RID: 568
	private float stationaryStartTime;

	// Token: 0x04000239 RID: 569
	private Vector2 anchorPos = Vector2.zero;

	// Token: 0x0400023A RID: 570
	private bool wasDown;

	// Token: 0x0200007B RID: 123
	public enum MotionState
	{
		// Token: 0x04000242 RID: 578
		None,
		// Token: 0x04000243 RID: 579
		Stationary,
		// Token: 0x04000244 RID: 580
		Moving
	}
}
