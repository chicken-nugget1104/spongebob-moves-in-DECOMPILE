using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class MouseGestures : FingerGestures
{
	// Token: 0x06000583 RID: 1411 RVA: 0x00015414 File Offset: 0x00013614
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x0001541C File Offset: 0x0001361C
	public override int MaxFingers
	{
		get
		{
			return this.maxMouseButtons;
		}
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00015424 File Offset: 0x00013624
	protected override FingerGestures.FingerPhase GetPhase(FingerGestures.Finger finger)
	{
		int index = finger.Index;
		if (Input.GetMouseButtonDown(index))
		{
			return FingerGestures.FingerPhase.Began;
		}
		if (Input.GetMouseButton(index))
		{
			if ((this.GetPosition(finger) - finger.Position).sqrMagnitude < 1f)
			{
				return FingerGestures.FingerPhase.Stationary;
			}
			return FingerGestures.FingerPhase.Moved;
		}
		else
		{
			if (Input.GetMouseButtonUp(index))
			{
				return FingerGestures.FingerPhase.Ended;
			}
			return FingerGestures.FingerPhase.None;
		}
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0001548C File Offset: 0x0001368C
	protected override Vector2 GetPosition(FingerGestures.Finger finger)
	{
		return Input.mousePosition;
	}

	// Token: 0x04000312 RID: 786
	public int maxMouseButtons = 3;
}
