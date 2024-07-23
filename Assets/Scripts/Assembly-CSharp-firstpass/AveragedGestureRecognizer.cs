using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
public abstract class AveragedGestureRecognizer : GestureRecognizer
{
	// Token: 0x060003EB RID: 1003 RVA: 0x000113C8 File Offset: 0x0000F5C8
	protected override int GetRequiredFingerCount()
	{
		return this.RequiredFingerCount;
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060003EC RID: 1004 RVA: 0x000113D0 File Offset: 0x0000F5D0
	// (set) Token: 0x060003ED RID: 1005 RVA: 0x000113D8 File Offset: 0x0000F5D8
	public Vector2 StartPosition
	{
		get
		{
			return this.startPos;
		}
		protected set
		{
			this.startPos = value;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060003EE RID: 1006 RVA: 0x000113E4 File Offset: 0x0000F5E4
	// (set) Token: 0x060003EF RID: 1007 RVA: 0x000113EC File Offset: 0x0000F5EC
	public Vector2 Position
	{
		get
		{
			return this.pos;
		}
		protected set
		{
			this.pos = value;
		}
	}

	// Token: 0x04000216 RID: 534
	public int RequiredFingerCount = 1;

	// Token: 0x04000217 RID: 535
	private Vector2 startPos = Vector2.zero;

	// Token: 0x04000218 RID: 536
	private Vector2 pos = Vector2.zero;
}
