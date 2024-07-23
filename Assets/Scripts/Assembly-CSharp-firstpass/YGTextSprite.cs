using System;
using UnityEngine;
using Yarg;

// Token: 0x0200010D RID: 269
public class YGTextSprite : YGAtlasSprite
{
	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x00027100 File Offset: 0x00025300
	// (set) Token: 0x060009ED RID: 2541 RVA: 0x00027108 File Offset: 0x00025308
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (this.text == value)
			{
				return;
			}
			this.text = value;
			this.GenerateChars();
			this.dirty = true;
			this.textChanged = true;
			base.View.RefreshEvent += this.AssembleMesh;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x0002715C File Offset: 0x0002535C
	// (set) Token: 0x060009EF RID: 2543 RVA: 0x00027164 File Offset: 0x00025364
	public string LocalizationKey
	{
		get
		{
			return this.localizationKey;
		}
		set
		{
			if (value == string.Empty || this.localizationKey == value)
			{
				return;
			}
			this.localizationKey = value;
			this.Text = Language.Get(this.localizationKey);
		}
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x000271AC File Offset: 0x000253AC
	public string StripScalarDataFromString(string text, bool storeScale = true)
	{
		if (text.Contains("~"))
		{
			int num = text.IndexOf("~");
			string result = null;
			try
			{
				if (storeScale)
				{
					this.textScale = float.Parse(text.Substring(num + 1));
				}
				result = text.Substring(0, num);
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
				result = text;
			}
			return result;
		}
		return text;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00027234 File Offset: 0x00025434
	protected override void OnEnable()
	{
		YGTextureLibrary library = base.View.Library;
		if (base.renderer.sharedMaterial == null && library.fontAtlases[this.fontIndex].material != null)
		{
			base.SetMaterial(library.fontAtlases[this.fontIndex].material);
		}
		if (this.sprite == null)
		{
			Debug.LogWarning("TextSprite is null");
			this.sprite = new SpriteCoordinates();
		}
		if (string.IsNullOrEmpty(this.sprite.name))
		{
			string filename = library.fontAtlases[this.fontIndex].filename;
			this.sprite.name = filename;
		}
		if (this.localizationKey != string.Empty)
		{
			this.Text = Language.Get(this.localizationKey);
		}
		else
		{
			this.Text = this.text;
		}
		this.dirty = true;
		this.textChanged = true;
		base.View.RefreshEvent += this.AssembleMesh;
		base.OnEnable();
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00027350 File Offset: 0x00025550
	private void GenerateChars()
	{
		this.text = this.StripScalarDataFromString(this.text, true);
		char[] array = this.text.ToCharArray();
		FontAtlas fontAtlas = base.View.Library.fontAtlases[this.fontIndex];
		FontAtlas.CharData charOffset = this.GetCharOffset(' ', fontAtlas);
		this.characters = new YGTextSprite.CharSprite[array.Length];
		Vector2 zero = Vector2.zero;
		this.lineHeight = fontAtlas.info.lineHeight;
		int first = -1;
		for (int i = 0; i < array.Length; i++)
		{
			char c = array[i];
			if (c == '|')
			{
				c = '\n';
			}
			FontAtlas.CharData charOffset2 = this.GetCharOffset(c, fontAtlas);
			charOffset2.offset.Set(charOffset2.offset.x * this.scale.x, charOffset2.offset.y * this.scale.y);
			if (this.useKerning)
			{
				int num = fontAtlas.Kerning(first, (int)c);
				if (num != 0)
				{
					zero.x += (float)num * this.scale.x;
				}
			}
			if (!char.IsWhiteSpace(c))
			{
				YGTextSprite.CharSprite charSprite = new YGTextSprite.CharSprite(c, zero - charOffset2.offset, charOffset2, this.textureSize, this.scale, this.color);
				this.characters[i] = charSprite;
			}
			else
			{
				this.characters[i] = new YGTextSprite.CharSprite(c);
			}
			char c2 = c;
			if (c2 != '\t')
			{
				if (c2 != '\n')
				{
					zero.x += (float)charOffset2.xadvance * this.scale.x;
				}
				else
				{
					zero.y -= (float)this.lineHeight;
					zero.x = 0f;
				}
			}
			else
			{
				zero.x += (float)charOffset.xadvance * this.scale.x * 4f;
			}
			first = (int)c;
		}
		zero.y = -zero.y + (float)this.lineHeight;
		this.textSize = zero;
		this.dirty = false;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0002758C File Offset: 0x0002578C
	private bool ValidateCharacters()
	{
		if (this.dirty || this.text == null || this.characters == null)
		{
			return false;
		}
		if (this.text.Length != this.characters.Length)
		{
			return false;
		}
		char[] array = this.text.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (this.characters[i] == null)
			{
				return false;
			}
			if (array[i] != this.characters[i].chr)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00027620 File Offset: 0x00025820
	private void BuildTextSprite()
	{
		YGTextSprite.CharSprite[] array = Array.FindAll<YGTextSprite.CharSprite>(this.characters, (YGTextSprite.CharSprite x) => !char.IsWhiteSpace(x.chr));
		int num = array.Length;
		this.tris = new int[num * 6];
		this.uvs = new Vector2[num * 4];
		this.verts = new Vector3[num * 4];
		this.colors = new Color[num * 4];
		this.normals = YGSprite.BuildNormals(this.verts.Length);
		int[] array2 = YGSprite.BuildTris();
		for (int i = 0; i < num; i++)
		{
			YGTextSprite.CharSprite charSprite = array[i];
			int num2 = i * 4;
			for (int j = 0; j < 4; j++)
			{
				this.verts[num2 + j] = charSprite.verts[j];
				this.uvs[num2 + j] = charSprite.uvs[j];
				this.colors[num2 + j] = charSprite.colors[j];
			}
			num2 = i * 6;
			for (int k = 0; k < 6; k++)
			{
				this.tris[num2 + k] = array2[k];
				array2[k] += 4;
			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = this.verts;
		mesh.uv = this.uvs;
		mesh.colors = this.colors;
		mesh.triangles = this.tris;
		mesh.normals = this.normals;
		mesh.RecalculateBounds();
		this.sprite.SetMesh(mesh);
		this.bounds = mesh.bounds;
		Vector3 min = this.bounds.min;
		Vector3 max = this.bounds.max;
		this.size = new Vector2(max.x - min.x, max.y - min.y);
		this.center = this.bounds.center;
		UnityEngine.Object.DestroyImmediate(mesh);
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00027858 File Offset: 0x00025A58
	public override void SetSize(Vector2 s)
	{
		if (this.size == s)
		{
			return;
		}
		this.size = s;
		this.dirty = true;
		base.View.RefreshEvent += this.AssembleMesh;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000278A0 File Offset: 0x00025AA0
	public virtual void SetScale(Vector2 s)
	{
		if (this.scale == s)
		{
			return;
		}
		this.scale = s;
		this.dirty = true;
		base.View.RefreshEvent += this.AssembleMesh;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x000278E8 File Offset: 0x00025AE8
	public override void SetColor(Color c)
	{
		if (this.dirty)
		{
			this.color = c;
			return;
		}
		base.SetColor(c);
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00027904 File Offset: 0x00025B04
	public override SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex)
	{
		this.LoadSprite();
		return this.sprite;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00027914 File Offset: 0x00025B14
	protected override void LoadSprite()
	{
		if (!this.ValidateCharacters())
		{
			this.GenerateChars();
		}
		this.BuildTextSprite();
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x00027930 File Offset: 0x00025B30
	protected override void UpdateMesh(YGSprite.MeshUpdate update)
	{
		if (base.meshFilter.sharedMesh != null && (this.textChanged || !Application.isPlaying))
		{
			base.meshFilter.sharedMesh.Clear();
			this.textChanged = false;
		}
		base.UpdateMesh(update);
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00027988 File Offset: 0x00025B88
	protected override void OffsetVerts(Vector3[] verts)
	{
		Vector3 b = this.center;
		b.x -= this.size.x * this.scale.x / 2f;
		b.y += this.size.y * this.scale.y / 2f;
		for (int i = 0; i < verts.Length; i++)
		{
			verts[i] -= b;
		}
		base.OffsetVerts(verts);
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00027A2C File Offset: 0x00025C2C
	public virtual FontAtlas.CharData GetCharOffset(char chr, FontAtlas atlas)
	{
		return atlas[chr];
	}

	// Token: 0x0400065B RID: 1627
	private const string SCALAR_SYMBOL = "~";

	// Token: 0x0400065C RID: 1628
	[HideInInspector]
	public int fontIndex;

	// Token: 0x0400065D RID: 1629
	[HideInInspector]
	public int lineHeight;

	// Token: 0x0400065E RID: 1630
	[HideInInspector]
	public YGTextSprite.CharSprite[] characters;

	// Token: 0x0400065F RID: 1631
	public Bounds bounds;

	// Token: 0x04000660 RID: 1632
	public Vector2 textSize;

	// Token: 0x04000661 RID: 1633
	private bool dirty;

	// Token: 0x04000662 RID: 1634
	private bool textChanged;

	// Token: 0x04000663 RID: 1635
	public bool useKerning;

	// Token: 0x04000664 RID: 1636
	public Vector2 center;

	// Token: 0x04000665 RID: 1637
	public string text = "Text";

	// Token: 0x04000666 RID: 1638
	public string localizationKey = string.Empty;

	// Token: 0x04000667 RID: 1639
	public float textScale;

	// Token: 0x0200010E RID: 270
	[Serializable]
	public class CharSprite
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x00027A48 File Offset: 0x00025C48
		public CharSprite(char _chr)
		{
			this.chr = _chr;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00027A58 File Offset: 0x00025C58
		public CharSprite(char _chr, Vector2 _pos, FontAtlas.CharData data, Vector2 textureSize, Vector2 scale, Color color)
		{
			this.chr = _chr;
			this.pos = _pos;
			this.coords = data.size;
			Vector2 size = new Vector2(this.coords.width, this.coords.height);
			this.uvs = new Vector2[4];
			this.verts = new Vector3[4];
			this.colors = new Color[4];
			YGSprite.BuildVerts(size, scale, ref this.verts);
			YGSprite.BuildColors(color, ref this.colors);
			YGSprite.BuildUVs(this.coords, textureSize, ref this.uvs);
			for (int i = 0; i < this.verts.Length; i++)
			{
				Vector3[] array = this.verts;
				int num = i;
				array[num].x = array[num].x + _pos.x;
				Vector3[] array2 = this.verts;
				int num2 = i;
				array2[num2].y = array2[num2].y + _pos.y;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00027B4C File Offset: 0x00025D4C
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x00027B58 File Offset: 0x00025D58
		public char chr
		{
			get
			{
				return (char)this.character;
			}
			set
			{
				this.character = (int)value;
			}
		}

		// Token: 0x04000669 RID: 1641
		public Vector2 pos;

		// Token: 0x0400066A RID: 1642
		public int character;

		// Token: 0x0400066B RID: 1643
		public Rect coords;

		// Token: 0x0400066C RID: 1644
		public Vector2[] uvs;

		// Token: 0x0400066D RID: 1645
		public Vector3[] verts;

		// Token: 0x0400066E RID: 1646
		public Color[] colors;
	}
}
