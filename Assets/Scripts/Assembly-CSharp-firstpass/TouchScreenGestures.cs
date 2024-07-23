using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class TouchScreenGestures : FingerGestures
{
	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x000154C4 File Offset: 0x000136C4
	public override int MaxFingers
	{
		get
		{
			return this.maxFingers;
		}
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x000154CC File Offset: 0x000136CC
	protected override void Start()
	{
		this.finger2touchMap = new int[this.MaxFingers];
		base.Start();
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x000154E8 File Offset: 0x000136E8
	protected override FingerGestures.FingerPhase GetPhase(FingerGestures.Finger finger)
	{
		if (!this.HasValidTouch(finger))
		{
			return FingerGestures.FingerPhase.None;
		}
		switch (this.GetTouch(finger).phase)
		{
		case TouchPhase.Began:
			return FingerGestures.FingerPhase.Began;
		case TouchPhase.Moved:
			return FingerGestures.FingerPhase.Moved;
		case TouchPhase.Stationary:
			return FingerGestures.FingerPhase.Stationary;
		default:
			return FingerGestures.FingerPhase.Ended;
		}
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00015534 File Offset: 0x00013734
	protected override Vector2 GetPosition(FingerGestures.Finger finger)
	{
		return this.GetTouch(finger).position;
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00015550 File Offset: 0x00013750
	private void UpdateFingerTouchMap()
	{
		for (int i = 0; i < this.finger2touchMap.Length; i++)
		{
			this.finger2touchMap[i] = -1;
		}
		for (int j = 0; j < Input.touchCount; j++)
		{
			int fingerId = Input.touches[j].fingerId;
			if (fingerId < this.finger2touchMap.Length)
			{
				this.finger2touchMap[fingerId] = j;
			}
		}
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x000155C0 File Offset: 0x000137C0
	private bool HasValidTouch(FingerGestures.Finger finger)
	{
		return this.finger2touchMap[finger.Index] != -1;
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x000155D8 File Offset: 0x000137D8
	private Touch GetTouch(FingerGestures.Finger finger)
	{
		int num = this.finger2touchMap[finger.Index];
		if (num == -1)
		{
			return this.nullTouch;
		}
		return Input.touches[num];
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00015614 File Offset: 0x00013814
	protected override void Update()
	{
		this.UpdateFingerTouchMap();
		base.Update();
	}

	// Token: 0x04000313 RID: 787
	public int maxFingers = 5;

	// Token: 0x04000314 RID: 788
	private Touch nullTouch = default(Touch);

	// Token: 0x04000315 RID: 789
	private int[] finger2touchMap;
}
