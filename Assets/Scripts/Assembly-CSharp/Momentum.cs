using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class Momentum
{
	// Token: 0x060002CB RID: 715 RVA: 0x0000DF70 File Offset: 0x0000C170
	public Momentum() : this(4)
	{
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0000DF7C File Offset: 0x0000C17C
	public Momentum(int smoothingIterations)
	{
		this.smoothingIterations = smoothingIterations;
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060002CD RID: 717 RVA: 0x0000DFA4 File Offset: 0x0000C1A4
	public Vector3 Velocity
	{
		get
		{
			return this.velocity;
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x0000DFAC File Offset: 0x0000C1AC
	public void Reset()
	{
		this.velocity = Vector3.zero;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000DFBC File Offset: 0x0000C1BC
	public void ClearTrackPositions()
	{
		this.lastPositions.Clear();
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x0000DFCC File Offset: 0x0000C1CC
	public void TrackForSmoothing(Vector3 position)
	{
		this.lastPositions.Add(position);
		if (this.lastPositions.Count > this.smoothingIterations)
		{
			this.lastPositions.RemoveAt(0);
		}
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0000E008 File Offset: 0x0000C208
	public void CalculateSmoothVelocity()
	{
		this.velocity = Vector3.zero;
		int num = this.lastPositions.Count - 1;
		Vector3 a = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			Vector3 a2 = this.lastPositions[i + 1] - this.lastPositions[i];
			a = a * 0.1f + a2 * 0.9f;
		}
		this.velocity = a;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0000E08C File Offset: 0x0000C28C
	public void ApplyFriction(float amount)
	{
		this.velocity.Scale(new Vector3(amount, amount, amount));
	}

	// Token: 0x04000182 RID: 386
	private int smoothingIterations;

	// Token: 0x04000183 RID: 387
	private List<Vector3> lastPositions = new List<Vector3>();

	// Token: 0x04000184 RID: 388
	private Vector3 velocity = Vector3.zero;
}
