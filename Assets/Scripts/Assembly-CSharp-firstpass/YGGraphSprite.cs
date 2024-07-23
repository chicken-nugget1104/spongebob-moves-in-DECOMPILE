using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class YGGraphSprite : YGSprite
{
	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060009AA RID: 2474 RVA: 0x00025B00 File Offset: 0x00023D00
	// (set) Token: 0x060009A9 RID: 2473 RVA: 0x00025AF4 File Offset: 0x00023CF4
	public Color PenColor
	{
		get
		{
			return this.color;
		}
		set
		{
			this.color = value;
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00025B08 File Offset: 0x00023D08
	protected override void OnEnable()
	{
		this.dataPoints = Mathf.CeilToInt(this.size.x / this.pointSpacing);
		this.points = new Vector2[this.dataPoints];
		this.data = new List<float>(new float[this.dataPoints]);
		this.buffer = new List<float>(this.samplesPerPoint);
		this.tris = new int[this.dataPoints * 6];
		this.uvs = new Vector2[this.dataPoints * 4];
		this.verts = new Vector3[this.dataPoints * 4];
		this.graphMesh = new Mesh();
		base.OnEnable();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00025BB4 File Offset: 0x00023DB4
	public override void AssembleMesh()
	{
		Vector2 vector = new Vector2(0f, this.lineWidth / 2f);
		if (this.data.Count > this.dataPoints)
		{
			this.data.RemoveRange(0, this.data.Count - this.dataPoints);
		}
		float num = this.size.x / (float)this.dataPoints;
		float num2 = (this.yMax - this.yMin) / 2f * this.size.y;
		Vector2 a = Vector2.zero;
		bool flag = true;
		this.triSet = new int[]
		{
			1,
			3,
			2,
			1,
			2,
			0
		};
		float new_y = this.lineWidth / 2f;
		for (int i = 0; i < this.data.Count; i++)
		{
			Vector2 vector2 = this.points[i];
			vector2.Set((float)i * num, this.data[i] * num2);
			if (flag)
			{
				a = (this.points[i] = vector2);
				flag = false;
			}
			else
			{
				if (this.deskew)
				{
					vector.Set(0f, new_y);
					Quaternion rotation = Quaternion.LookRotation(a - vector2);
					vector = rotation * vector;
				}
				int num3 = i * 4;
				this.verts[num3] = (a - vector) * 0.01f;
				this.verts[num3 + 1] = (a + vector) * 0.01f;
				this.verts[num3 + 2] = (vector2 - vector) * 0.01f;
				this.verts[num3 + 3] = (vector2 + vector) * 0.01f;
				this.uvs[num3] = YGGraphSprite.uvSet[0];
				this.uvs[num3 + 1] = YGGraphSprite.uvSet[1];
				this.uvs[num3 + 2] = YGGraphSprite.uvSet[2];
				this.uvs[num3 + 3] = YGGraphSprite.uvSet[3];
				num3 = i * 6;
				this.tris[num3] = this.triSet[0];
				this.tris[num3 + 1] = this.triSet[1];
				this.tris[num3 + 2] = this.triSet[2];
				this.tris[num3 + 3] = this.triSet[3];
				this.tris[num3 + 4] = this.triSet[4];
				this.tris[num3 + 5] = this.triSet[5];
				for (int j = 0; j < 6; j++)
				{
					this.triSet[j] += 4;
				}
				a = (this.points[i] = vector2);
			}
		}
		this.graphMesh.vertices = this.verts;
		this.graphMesh.uv = this.uvs;
		this.graphMesh.triangles = this.tris;
		Color[] colors = new Color[this.verts.Length];
		YGSprite.BuildColors(this.color, ref colors);
		this.graphMesh.normals = YGSprite.BuildNormals(this.verts.Length);
		this.graphMesh.colors = colors;
		this.UpdateMesh(this.graphMesh);
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00025F9C File Offset: 0x0002419C
	protected virtual void UpdateMesh(Mesh source)
	{
		base.meshFilter.mesh = source;
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x00025FAC File Offset: 0x000241AC
	protected override void OnDisable()
	{
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00025FB0 File Offset: 0x000241B0
	public void SubmitData()
	{
		while (this.buffer.Count > this.samplesPerPoint)
		{
			float num = 0f;
			for (int i = 0; i < this.samplesPerPoint; i++)
			{
				num += this.buffer[i];
			}
			num /= (float)this.samplesPerPoint;
			this.data.Add(num);
			this.buffer.RemoveRange(0, this.samplesPerPoint);
			this.dirty = true;
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x00026034 File Offset: 0x00024234
	public void Add(float val)
	{
		this.buffer.Add(val);
		if (this.buffer.Count >= this.samplesPerPoint)
		{
			this.SubmitData();
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0002606C File Offset: 0x0002426C
	public void Add(IList<float> vals)
	{
		this.buffer.AddRange(vals);
		if (this.buffer.Count >= this.samplesPerPoint)
		{
			this.SubmitData();
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x000260A4 File Offset: 0x000242A4
	public void Clear()
	{
		this.buffer = new List<float>(new float[this.samplesPerPoint]);
		this.Draw();
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x000260C4 File Offset: 0x000242C4
	public void Draw(float val)
	{
		this.Add(val);
		this.Draw();
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x000260D4 File Offset: 0x000242D4
	public void Draw(IList<float> vals)
	{
		this.Add(vals);
		this.Draw();
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x000260E4 File Offset: 0x000242E4
	public void Draw()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		this.AssembleMesh();
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00026100 File Offset: 0x00024300
	private void PlotPoint(float x, float y, ref Vector2 point)
	{
	}

	// Token: 0x04000626 RID: 1574
	public float yMin;

	// Token: 0x04000627 RID: 1575
	public float yMax = 1f;

	// Token: 0x04000628 RID: 1576
	public bool deskew;

	// Token: 0x04000629 RID: 1577
	public int samplesPerPoint = 4;

	// Token: 0x0400062A RID: 1578
	private Vector2[] points;

	// Token: 0x0400062B RID: 1579
	private List<float> data;

	// Token: 0x0400062C RID: 1580
	public float lineWidth = 4f;

	// Token: 0x0400062D RID: 1581
	public float pointSpacing = 4f;

	// Token: 0x0400062E RID: 1582
	public bool dirty = true;

	// Token: 0x0400062F RID: 1583
	private static Vector2[] uvSet = new Vector2[]
	{
		new Vector2(0f, 0f),
		new Vector2(0f, -1f),
		new Vector2(1f, 0f),
		new Vector2(1f, -1f)
	};

	// Token: 0x04000630 RID: 1584
	private Mesh graphMesh;

	// Token: 0x04000631 RID: 1585
	private int dataPoints;

	// Token: 0x04000632 RID: 1586
	private List<float> buffer;

	// Token: 0x04000633 RID: 1587
	private int[] triSet;
}
