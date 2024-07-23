using System;
using System.Collections.Generic;

// Token: 0x0200042C RID: 1068
public class Path<Position>
{
	// Token: 0x06002132 RID: 8498 RVA: 0x000CD198 File Offset: 0x000CB398
	public void Add(Position position)
	{
		this.head = new Path<Position>.PathNode(position)
		{
			next = this.head
		};
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06002133 RID: 8499 RVA: 0x000CD1C0 File Offset: 0x000CB3C0
	public Position Current
	{
		get
		{
			return this.current.position;
		}
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000CD1D0 File Offset: 0x000CB3D0
	public void Begin()
	{
		this.current = this.head;
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x000CD1E0 File Offset: 0x000CB3E0
	public bool Next()
	{
		this.current = this.current.next;
		return this.Done();
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000CD1FC File Offset: 0x000CB3FC
	public bool Done()
	{
		return null == this.current;
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x000CD208 File Offset: 0x000CB408
	public IEnumerator<Position> GetEnumerator()
	{
		for (Path<Position>.PathNode node = this.head; node != null; node = node.next)
		{
			yield return node.position;
		}
		yield break;
	}

	// Token: 0x04001467 RID: 5223
	private Path<Position>.PathNode head;

	// Token: 0x04001468 RID: 5224
	private Path<Position>.PathNode current;

	// Token: 0x0200042D RID: 1069
	private class PathNode
	{
		// Token: 0x06002138 RID: 8504 RVA: 0x000CD224 File Offset: 0x000CB424
		public PathNode(Position position)
		{
			this.position = position;
		}

		// Token: 0x04001469 RID: 5225
		public Path<Position>.PathNode next;

		// Token: 0x0400146A RID: 5226
		public Position position;
	}
}
