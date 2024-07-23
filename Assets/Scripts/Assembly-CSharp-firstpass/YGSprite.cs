using System;
using UnityEngine;
using Yarg;

// Token: 0x0200010A RID: 266
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class YGSprite : MonoBehaviour, ILoadable
{
	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000261C0 File Offset: 0x000243C0
	// (set) Token: 0x060009B9 RID: 2489 RVA: 0x000261C8 File Offset: 0x000243C8
	public SpritePivot Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			this.pivot = value;
			this.View.RefreshEvent += this.AssembleMesh;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060009BA RID: 2490 RVA: 0x000261EC File Offset: 0x000243EC
	protected GUIView View
	{
		get
		{
			if (this._view == null)
			{
				this._view = GUIView.GetParentView(this.tform);
			}
			return this._view;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060009BB RID: 2491 RVA: 0x00026224 File Offset: 0x00024424
	protected Transform tform
	{
		get
		{
			return (!(this._tform != null)) ? (this._tform = base.transform) : this._tform;
		}
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x060009BC RID: 2492 RVA: 0x0002625C File Offset: 0x0002445C
	// (set) Token: 0x060009BD RID: 2493 RVA: 0x0002626C File Offset: 0x0002446C
	public Vector3 WorldPosition
	{
		get
		{
			return this.tform.position;
		}
		set
		{
			if (value == this.tform.position)
			{
				return;
			}
			this.tform.position = value;
			YGSprite.MeshUpdateHierarchy(base.gameObject);
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x000262A8 File Offset: 0x000244A8
	public static void MeshUpdateHierarchy(GameObject root)
	{
		YGSprite[] componentsInChildren = root.GetComponentsInChildren<YGSprite>();
		foreach (YGSprite ygsprite in componentsInChildren)
		{
			ygsprite.MeshUpdateEvent.FireEvent();
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x000262E4 File Offset: 0x000244E4
	public MeshFilter meshFilter
	{
		get
		{
			if (this._meshFilter == null)
			{
				this._meshFilter = base.gameObject.GetComponent<MeshFilter>();
				if (this._meshFilter == null)
				{
					this._meshFilter = base.gameObject.AddComponent<MeshFilter>();
					base.renderer.castShadows = false;
					base.renderer.receiveShadows = false;
				}
				UnityEngine.Object.DestroyImmediate(this._meshFilter.mesh);
				this._meshFilter.mesh = new Mesh();
			}
			return this._meshFilter;
		}
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00026374 File Offset: 0x00024574
	protected virtual void OnEnable()
	{
		if (base.renderer.sharedMaterial != null)
		{
			this.textureSize = this.GetMainTextureSize(true);
		}
		if (!this.loaded)
		{
			this.View.RefreshEvent += this.Load;
		}
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x000263C8 File Offset: 0x000245C8
	private void UnSubscribe()
	{
		GUIView view = this.View;
		view.RefreshEvent -= this.Load;
		view.RefreshEvent -= this.AssembleMesh;
		this.View.RefreshEvent -= this.UpdateMesh;
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0002641C File Offset: 0x0002461C
	protected virtual void OnDisable()
	{
		this.UnSubscribe();
		this._view = null;
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0002642C File Offset: 0x0002462C
	protected virtual void OnDestroy()
	{
		if (base.transform.parent != null)
		{
			this.UnSubscribe();
		}
		UnityEngine.Object.Destroy(this.meshFilter.sharedMesh);
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00026468 File Offset: 0x00024668
	public virtual void SetPosition(int x, int y)
	{
		Vector3 position = this.View.PixelsToWorld(new Vector2((float)x, (float)y));
		this.tform.position = position;
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00026498 File Offset: 0x00024698
	public virtual Vector2 ResetSize()
	{
		if (base.renderer.sharedMaterial == null)
		{
			return Vector2.zero;
		}
		this.textureSize = this.GetMainTextureSize(true);
		this.size.Set(this.textureSize.x, this.textureSize.y);
		this.AssembleMesh();
		return this.size;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x000264FC File Offset: 0x000246FC
	public virtual Vector2 PixelSnap()
	{
		Vector3 position = this.tform.position;
		position.x = (float)Mathf.RoundToInt(position.x / 0.01f) * 0.01f;
		position.y = (float)Mathf.RoundToInt(position.y / 0.01f) * 0.01f;
		position.z = (float)Mathf.RoundToInt(position.z / 0.01f) * 0.01f;
		this.tform.position = position;
		this.size.x = (float)Mathf.RoundToInt(this.size.x);
		this.size.y = (float)Mathf.RoundToInt(this.size.y);
		this.AssembleMesh();
		return this.size;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x000265C8 File Offset: 0x000247C8
	public void SetMaterial(Material mat)
	{
		base.renderer.sharedMaterial = mat;
		this.textureSize = this.GetMainTextureSize(true);
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x000265E4 File Offset: 0x000247E4
	public void RefreshTextureSize()
	{
		this.textureSize = this.GetMainTextureSize(false);
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x000265F4 File Offset: 0x000247F4
	public virtual Bounds GetBounds()
	{
		return base.renderer.bounds;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00026604 File Offset: 0x00024804
	public virtual void SetSize(Vector2 s)
	{
		this.size = s;
		YGSprite.BuildVerts(this.size, this.scale, ref this.verts);
		this.update.verts = this.verts;
		this.UpdateMesh();
		this.View.RefreshEvent += this.UpdateMesh;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00026660 File Offset: 0x00024860
	public virtual void SetColor(Color c)
	{
		this.color = c;
		YGSprite.BuildColors(this.color, ref this.colors);
		this.update.colors = this.colors;
		this.View.RefreshEvent += this.UpdateMesh;
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x000266B0 File Offset: 0x000248B0
	public virtual void SetAlpha(float alpha)
	{
		Color color = this.color;
		if (color.a != alpha)
		{
			color.a = alpha;
			this.SetColor(color);
		}
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x000266E0 File Offset: 0x000248E0
	public static void BuildVerts(Vector2 size, Vector2 scale, ref Vector3[] verts)
	{
		size.x *= scale.x;
		size.y *= scale.y;
		verts[0].Set(0f, 0f, 0f);
		verts[1].Set(size.x, 0f, 0f);
		verts[2].Set(0f, -size.y, 0f);
		verts[3].Set(size.x, -size.y, 0f);
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00026794 File Offset: 0x00024994
	public static Vector3[] BuildNormals(int count)
	{
		Vector3[] array = new Vector3[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = -Vector3.forward;
		}
		return array;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x000267D4 File Offset: 0x000249D4
	public static void BuildColors(Color color, ref Color[] colors)
	{
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = color;
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00026804 File Offset: 0x00024A04
	public static int[] BuildTris()
	{
		return new int[]
		{
			1,
			3,
			2,
			1,
			2,
			0
		};
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00026824 File Offset: 0x00024A24
	public static void BuildUVs(Rect rect, Vector2 size, ref Vector2[] uvs)
	{
		uvs[0].Set(rect.xMin / size.x, 1f - rect.yMin / size.y);
		uvs[1].Set(rect.xMax / size.x, 1f - rect.yMin / size.y);
		uvs[2].Set(rect.xMin / size.x, 1f - rect.yMax / size.y);
		uvs[3].Set(rect.xMax / size.x, 1f - rect.yMax / size.y);
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x000268F8 File Offset: 0x00024AF8
	protected virtual void OffsetVerts(Vector3[] verts)
	{
		for (int i = 0; i < verts.Length; i++)
		{
			verts[i] *= 0.01f;
			switch (this.pivot)
			{
			case SpritePivot.LowerCenter:
			case SpritePivot.MiddleCenter:
			case SpritePivot.UpperCenter:
			{
				int num = i;
				verts[num].x = verts[num].x - this.size.x * 0.01f * 0.5f * this.scale.x;
				break;
			}
			case SpritePivot.LowerRight:
			case SpritePivot.MiddleRight:
			case SpritePivot.UpperRight:
			{
				int num2 = i;
				verts[num2].x = verts[num2].x - this.size.x * 0.01f * this.scale.x;
				break;
			}
			}
			switch (this.pivot)
			{
			case SpritePivot.LowerCenter:
			case SpritePivot.LowerLeft:
			case SpritePivot.LowerRight:
			{
				int num3 = i;
				verts[num3].y = verts[num3].y + this.size.y * 0.01f * this.scale.y;
				break;
			}
			case SpritePivot.MiddleCenter:
			case SpritePivot.MiddleLeft:
			case SpritePivot.MiddleRight:
			{
				int num4 = i;
				verts[num4].y = verts[num4].y + this.size.y * 0.01f * 0.5f * this.scale.y;
				break;
			}
			}
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00026A7C File Offset: 0x00024C7C
	public virtual void Load()
	{
		this.loaded = true;
		this.AssembleMesh();
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00026A8C File Offset: 0x00024C8C
	public virtual void AssembleMesh()
	{
		this.update.Reset();
		YGSprite.BuildUVs(new Rect(0f, 0f, this.textureSize.x, this.textureSize.y), this.textureSize, ref this.uvs);
		YGSprite.BuildVerts(this.size, this.scale, ref this.verts);
		YGSprite.BuildColors(this.color, ref this.colors);
		this.update.verts = this.verts;
		this.update.normals = this.normals;
		this.update.colors = this.colors;
		this.update.tris = this.tris;
		this.update.uvs = this.uvs;
		this.UpdateMesh(this.update);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00026B64 File Offset: 0x00024D64
	protected void UpdateMesh()
	{
		this.UpdateMesh(this.update);
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00026B74 File Offset: 0x00024D74
	protected virtual void UpdateMesh(YGSprite.MeshUpdate update)
	{
		if (this == null)
		{
			return;
		}
		Mesh mesh = null;
		mesh = ((!(this.meshFilter.sharedMesh == null)) ? this.meshFilter.sharedMesh : new Mesh());
		if (update._vertsUpdate)
		{
			this.OffsetVerts(update.verts);
			try
			{
				mesh.vertices = update.verts;
				mesh.RecalculateBounds();
			}
			catch
			{
				Debug.Log(string.Format("{0} : {1}", base.gameObject.name, mesh == null));
				throw;
			}
		}
		if (update._uvsUpdate)
		{
			if (update.uvs.Length != mesh.vertices.Length)
			{
				return;
			}
			mesh.uv = update.uvs;
		}
		if (update._trisUpdate)
		{
			mesh.triangles = update.tris;
		}
		if (update._normalsUpdate)
		{
			if (update.normals.Length != mesh.vertices.Length)
			{
				return;
			}
			mesh.normals = update.normals;
		}
		if (update._colorsUpdate)
		{
			if (update.colors.Length != mesh.vertices.Length)
			{
				return;
			}
			mesh.colors = update.colors;
		}
		this.meshFilter.mesh = mesh;
		update.Reset();
		this.MeshUpdateEvent.FireEvent();
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00026CF0 File Offset: 0x00024EF0
	protected virtual Vector2 GetMainTextureSize(bool fromShared)
	{
		if (fromShared)
		{
			if (base.renderer.sharedMaterial != null && base.renderer.sharedMaterial.mainTexture != null)
			{
				return new Vector2((float)base.renderer.sharedMaterial.mainTexture.width, (float)base.renderer.sharedMaterial.mainTexture.height);
			}
			return Vector2.zero;
		}
		else
		{
			if (base.renderer.material != null && base.renderer.material.mainTexture != null)
			{
				return new Vector2((float)base.renderer.material.mainTexture.width, (float)base.renderer.material.mainTexture.height);
			}
			return Vector2.zero;
		}
	}

	// Token: 0x0400063E RID: 1598
	public Vector2 size = new Vector2(64f, 64f);

	// Token: 0x0400063F RID: 1599
	public bool lockAspect = true;

	// Token: 0x04000640 RID: 1600
	public Vector2 scale = Vector2.one;

	// Token: 0x04000641 RID: 1601
	public SpritePivot pivot = SpritePivot.MiddleCenter;

	// Token: 0x04000642 RID: 1602
	public Color color = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04000643 RID: 1603
	private bool loaded;

	// Token: 0x04000644 RID: 1604
	protected Vector3[] verts = new Vector3[4];

	// Token: 0x04000645 RID: 1605
	protected Color[] colors = new Color[4];

	// Token: 0x04000646 RID: 1606
	protected Vector2[] uvs = new Vector2[4];

	// Token: 0x04000647 RID: 1607
	protected Vector3[] normals = YGSprite.BuildNormals(4);

	// Token: 0x04000648 RID: 1608
	protected int[] tris = YGSprite.BuildTris();

	// Token: 0x04000649 RID: 1609
	private GUIView _view;

	// Token: 0x0400064A RID: 1610
	private Transform _tform;

	// Token: 0x0400064B RID: 1611
	public EventDispatcher MeshUpdateEvent = new EventDispatcher();

	// Token: 0x0400064C RID: 1612
	protected Vector2 textureSize = Vector2.one;

	// Token: 0x0400064D RID: 1613
	[NonSerialized]
	protected MeshFilter _meshFilter;

	// Token: 0x0400064E RID: 1614
	protected bool init;

	// Token: 0x0400064F RID: 1615
	protected YGSprite.MeshUpdate update = new YGSprite.MeshUpdate();

	// Token: 0x0200010B RID: 267
	public class MeshUpdate
	{
		// Token: 0x060009D8 RID: 2520 RVA: 0x00026DD4 File Offset: 0x00024FD4
		public MeshUpdate()
		{
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00026DDC File Offset: 0x00024FDC
		public MeshUpdate(SpriteCoordinates source)
		{
			this.verts = source.verts;
			this.normals = source.normals;
			this.colors = source.color;
			this.tris = source.tris;
			this.uvs = source.uvs;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00026E2C File Offset: 0x0002502C
		public MeshUpdate(Mesh source)
		{
			this.verts = source.vertices;
			this.normals = source.normals;
			this.colors = source.colors;
			this.tris = source.triangles;
			this.uvs = source.uv;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x00026E7C File Offset: 0x0002507C
		// (set) Token: 0x060009DC RID: 2524 RVA: 0x00026E84 File Offset: 0x00025084
		public int vertCount { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x00026E90 File Offset: 0x00025090
		// (set) Token: 0x060009DE RID: 2526 RVA: 0x00026E98 File Offset: 0x00025098
		public Vector3[] verts
		{
			get
			{
				return this._verts;
			}
			set
			{
				if (value == null)
				{
					Debug.LogError("Null verts sent to MeshUpdate");
					return;
				}
				this._verts = value;
				this._vertsUpdate = true;
				this.vertCount = value.Length;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x00026ED0 File Offset: 0x000250D0
		// (set) Token: 0x060009E0 RID: 2528 RVA: 0x00026ED8 File Offset: 0x000250D8
		public Vector3[] normals
		{
			get
			{
				return this._normals;
			}
			set
			{
				this._normals = value;
				this._normalsUpdate = true;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x00026EE8 File Offset: 0x000250E8
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x00026EF0 File Offset: 0x000250F0
		public Color[] colors
		{
			get
			{
				return this._colors;
			}
			set
			{
				this._colors = value;
				this._colorsUpdate = true;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00026F00 File Offset: 0x00025100
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x00026F08 File Offset: 0x00025108
		public int[] tris
		{
			get
			{
				return this._tris;
			}
			set
			{
				this._tris = value;
				this._trisUpdate = true;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x00026F18 File Offset: 0x00025118
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x00026F20 File Offset: 0x00025120
		public Vector2[] uvs
		{
			get
			{
				return this._uvs;
			}
			set
			{
				this._uvs = value;
				this._uvsUpdate = true;
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00026F30 File Offset: 0x00025130
		public void Reset()
		{
			this._vertsUpdate = false;
			this._normalsUpdate = false;
			this._colorsUpdate = false;
			this._trisUpdate = false;
			this._uvsUpdate = false;
		}

		// Token: 0x04000650 RID: 1616
		public bool _vertsUpdate;

		// Token: 0x04000651 RID: 1617
		private Vector3[] _verts;

		// Token: 0x04000652 RID: 1618
		public bool _normalsUpdate;

		// Token: 0x04000653 RID: 1619
		private Vector3[] _normals;

		// Token: 0x04000654 RID: 1620
		public bool _colorsUpdate;

		// Token: 0x04000655 RID: 1621
		private Color[] _colors;

		// Token: 0x04000656 RID: 1622
		public bool _trisUpdate;

		// Token: 0x04000657 RID: 1623
		private int[] _tris;

		// Token: 0x04000658 RID: 1624
		public bool _uvsUpdate;

		// Token: 0x04000659 RID: 1625
		private Vector2[] _uvs;
	}
}
