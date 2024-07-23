using System;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public class AlignedBox
{
	// Token: 0x060023AA RID: 9130 RVA: 0x000DC398 File Offset: 0x000DA598
	public AlignedBox()
	{
		this.xmin = 0f;
		this.xmax = 0f;
		this.ymin = 0f;
		this.ymax = 0f;
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000DC3D8 File Offset: 0x000DA5D8
	public AlignedBox(float xmin, float xmax, float ymin, float ymax)
	{
		TFUtils.Assert(xmin <= xmax, "AlignedBox cannot have xmin > xmax");
		TFUtils.Assert(ymin <= ymax, "AlignedBox cannot have ymin > ymax");
		this.xmin = xmin;
		this.xmax = xmax;
		this.ymin = ymin;
		this.ymax = ymax;
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x000DC488 File Offset: 0x000DA688
	public string Describe()
	{
		return string.Format("[{0}, {1}, {2}, {3}]", new object[]
		{
			this.xmin,
			this.xmax,
			this.ymin,
			this.ymax
		});
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000DC4E0 File Offset: 0x000DA6E0
	public override string ToString()
	{
		return "AlignedBox: " + this.Describe();
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000DC4F4 File Offset: 0x000DA6F4
	public static bool Intersects(AlignedBox lhs, Segment rhs)
	{
		AlignedBox.point[0].Set(lhs.xmin, lhs.ymin);
		AlignedBox.point[1].Set(lhs.xmin, lhs.ymax);
		AlignedBox.point[2].Set(lhs.xmax, lhs.ymax);
		AlignedBox.point[3].Set(lhs.xmax, lhs.ymin);
		Vector2 r = rhs.second - rhs.first;
		if (AlignedBox.Left(r, AlignedBox.point[0] - rhs.first) && AlignedBox.Left(r, AlignedBox.point[1] - rhs.first) && AlignedBox.Left(r, AlignedBox.point[2] - rhs.first) && AlignedBox.Left(r, AlignedBox.point[3] - rhs.first))
		{
			return false;
		}
		r = rhs.first - rhs.second;
		if (AlignedBox.Left(r, AlignedBox.point[0] - rhs.second) && AlignedBox.Left(r, AlignedBox.point[1] - rhs.second) && AlignedBox.Left(r, AlignedBox.point[2] - rhs.second) && AlignedBox.Left(r, AlignedBox.point[3] - rhs.second))
		{
			return false;
		}
		r = AlignedBox.point[1] - AlignedBox.point[0];
		if (AlignedBox.Left(r, rhs.first - AlignedBox.point[0]) && AlignedBox.Left(r, rhs.second - AlignedBox.point[0]))
		{
			return false;
		}
		r = AlignedBox.point[2] - AlignedBox.point[1];
		if (AlignedBox.Left(r, rhs.first - AlignedBox.point[1]) && AlignedBox.Left(r, rhs.second - AlignedBox.point[1]))
		{
			return false;
		}
		r = AlignedBox.point[3] - AlignedBox.point[2];
		if (AlignedBox.Left(r, rhs.first - AlignedBox.point[2]) && AlignedBox.Left(r, rhs.second - AlignedBox.point[2]))
		{
			return false;
		}
		r = AlignedBox.point[0] - AlignedBox.point[3];
		return !AlignedBox.Left(r, rhs.first - AlignedBox.point[3]) || !AlignedBox.Left(r, rhs.second - AlignedBox.point[3]);
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000DC8B0 File Offset: 0x000DAAB0
	public static bool Intersects(AlignedBox lhs, AlignedBox rhs)
	{
		return rhs.xmax > lhs.xmin && lhs.xmax > rhs.xmin && rhs.ymax > lhs.ymin && lhs.ymax > rhs.ymin;
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000DC908 File Offset: 0x000DAB08
	public static bool Contains(AlignedBox lhs, AlignedBox rhs)
	{
		return rhs.xmin > lhs.xmin && lhs.xmax > rhs.xmax && rhs.ymin > lhs.ymin && lhs.ymax > rhs.ymax;
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x000DC960 File Offset: 0x000DAB60
	public bool Contains(float x, float y)
	{
		return x >= this.xmin && x <= this.xmax && y >= this.ymin && y <= this.ymax;
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x060023B3 RID: 9139 RVA: 0x000DC998 File Offset: 0x000DAB98
	public float Width
	{
		get
		{
			return this.xmax - this.xmin;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x060023B4 RID: 9140 RVA: 0x000DC9A8 File Offset: 0x000DABA8
	public float Height
	{
		get
		{
			return this.ymax - this.ymin;
		}
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x000DC9B8 File Offset: 0x000DABB8
	public static AlignedBox Union(AlignedBox lhs, AlignedBox rhs)
	{
		return new AlignedBox((lhs.xmin >= rhs.xmin) ? rhs.xmin : lhs.xmin, (lhs.xmax <= rhs.xmax) ? rhs.xmax : lhs.xmax, (lhs.ymin >= rhs.ymin) ? rhs.ymin : lhs.ymin, (lhs.ymax <= rhs.ymax) ? rhs.ymax : lhs.ymax);
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x000DCA54 File Offset: 0x000DAC54
	private static bool Left(Vector2 r, Vector2 q)
	{
		return r.x * q.y - q.x * r.y >= 0f;
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000DCA80 File Offset: 0x000DAC80
	public AlignedBox OffsetByVector(Vector2 offset)
	{
		return new AlignedBox(this.xmin + offset.x, this.xmax + offset.x, this.ymin + offset.y, this.ymax + offset.y);
	}

	// Token: 0x04001612 RID: 5650
	public float xmin;

	// Token: 0x04001613 RID: 5651
	public float xmax;

	// Token: 0x04001614 RID: 5652
	public float ymin;

	// Token: 0x04001615 RID: 5653
	public float ymax;

	// Token: 0x04001616 RID: 5654
	private static Vector2[] point = new Vector2[]
	{
		Vector2.zero,
		Vector2.zero,
		Vector2.zero,
		Vector2.zero
	};
}
