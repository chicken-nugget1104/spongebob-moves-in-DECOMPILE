using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public abstract class MultiFingerGestureRecognizer : GestureRecognizer
{
	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000413 RID: 1043 RVA: 0x000117DC File Offset: 0x0000F9DC
	// (set) Token: 0x06000414 RID: 1044 RVA: 0x000117E4 File Offset: 0x0000F9E4
	protected Vector2[] StartPosition
	{
		get
		{
			return this.startPos;
		}
		set
		{
			this.startPos = value;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000415 RID: 1045 RVA: 0x000117F0 File Offset: 0x0000F9F0
	// (set) Token: 0x06000416 RID: 1046 RVA: 0x000117F8 File Offset: 0x0000F9F8
	protected Vector2[] Position
	{
		get
		{
			return this.pos;
		}
		set
		{
			this.pos = value;
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00011804 File Offset: 0x0000FA04
	protected override void Start()
	{
		base.Start();
		this.OnFingerCountChanged(this.GetRequiredFingerCount());
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00011818 File Offset: 0x0000FA18
	protected void OnFingerCountChanged(int fingerCount)
	{
		this.StartPosition = new Vector2[fingerCount];
		this.Position = new Vector2[fingerCount];
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000419 RID: 1049 RVA: 0x00011834 File Offset: 0x0000FA34
	public int RequiredFingerCount
	{
		get
		{
			return this.GetRequiredFingerCount();
		}
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001183C File Offset: 0x0000FA3C
	public Vector2 GetPosition(int index)
	{
		return this.pos[index];
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00011850 File Offset: 0x0000FA50
	public Vector2 GetStartPosition(int index)
	{
		return this.startPos[index];
	}

	// Token: 0x0400022A RID: 554
	private Vector2[] pos;

	// Token: 0x0400022B RID: 555
	private Vector2[] startPos;
}
