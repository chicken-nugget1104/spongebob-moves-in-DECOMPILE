using System;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class GridPosition : IEquatable<GridPosition>
{
	// Token: 0x060023B8 RID: 9144 RVA: 0x000DCAC0 File Offset: 0x000DACC0
	public GridPosition(int row, int col)
	{
		this.row = row;
		this.col = col;
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x060023B9 RID: 9145 RVA: 0x000DCAD8 File Offset: 0x000DACD8
	// (set) Token: 0x060023BA RID: 9146 RVA: 0x000DCAE0 File Offset: 0x000DACE0
	public int X
	{
		get
		{
			return this.col;
		}
		set
		{
			this.col = value;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x060023BB RID: 9147 RVA: 0x000DCAEC File Offset: 0x000DACEC
	// (set) Token: 0x060023BC RID: 9148 RVA: 0x000DCAF4 File Offset: 0x000DACF4
	public int Y
	{
		get
		{
			return this.row;
		}
		set
		{
			this.row = value;
		}
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000DCB00 File Offset: 0x000DAD00
	public bool Within(GridPosition min, GridPosition max)
	{
		return this.row >= min.row && this.row <= max.row && this.col >= min.col && this.col <= max.col;
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000DCB54 File Offset: 0x000DAD54
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"(",
			this.row.ToString(),
			", ",
			this.col.ToString(),
			")"
		});
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000DCBA0 File Offset: 0x000DADA0
	public override int GetHashCode()
	{
		int num = 17;
		num = num * 31 + this.row;
		return num * 31 + this.col;
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000DCBCC File Offset: 0x000DADCC
	public override bool Equals(object other)
	{
		return other is GridPosition && this.Equals((GridPosition)other);
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x000DCBEC File Offset: 0x000DADEC
	public bool Equals(GridPosition other)
	{
		return this.row == other.row && this.col == other.col;
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000DCC1C File Offset: 0x000DAE1C
	public void MakeValid(int maxRow, int maxCol)
	{
		this.row = Mathf.Clamp(this.row, 0, maxRow);
		this.col = Mathf.Clamp(this.col, 0, maxCol);
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000DCC50 File Offset: 0x000DAE50
	public Vector2 ToVector2()
	{
		return new Vector2((float)this.X, (float)this.Y);
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x000DCC68 File Offset: 0x000DAE68
	public static GridPosition operator +(GridPosition lhs, GridPosition rhs)
	{
		return new GridPosition(lhs.row + rhs.row, lhs.col + rhs.col);
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000DCC8C File Offset: 0x000DAE8C
	public static GridPosition operator -(GridPosition lhs, GridPosition rhs)
	{
		return new GridPosition(lhs.row - rhs.row, lhs.col - rhs.col);
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000DCCB0 File Offset: 0x000DAEB0
	public static bool operator ==(GridPosition a, GridPosition b)
	{
		return object.ReferenceEquals(a, b) || (a != null && b != null && a.row == b.row && a.col == b.col);
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000DCCFC File Offset: 0x000DAEFC
	public static bool operator !=(GridPosition a, GridPosition b)
	{
		return !(a == b);
	}

	// Token: 0x04001617 RID: 5655
	public int row;

	// Token: 0x04001618 RID: 5656
	public int col;
}
