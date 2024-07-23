using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000072 RID: 114
public sealed class GraphTexture
{
	// Token: 0x060003E4 RID: 996 RVA: 0x00011000 File Offset: 0x0000F200
	public GraphTexture(Vector2 size, Color bgColor, Color graphColor)
	{
		this.blankTexture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGBA32, false);
		this.texture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGBA32, false);
		Color[] array = new Color[(int)(size.x * size.y)];
		this.i = 0;
		while (this.i < array.Length)
		{
			array[this.i] = bgColor;
			this.i++;
		}
		this.penColor = graphColor;
		this.blankTexture.SetPixels(array);
		this.blankTexture.Apply();
		this.texture.SetPixels(array);
		this.texture.Apply();
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060003E5 RID: 997 RVA: 0x00011114 File Offset: 0x0000F314
	// (set) Token: 0x060003E6 RID: 998 RVA: 0x0001111C File Offset: 0x0000F31C
	public Texture2D texture { get; private set; }

	// Token: 0x060003E7 RID: 999 RVA: 0x00011128 File Offset: 0x0000F328
	public void Draw(List<float> data)
	{
		this.texture.SetPixels(this.blankTexture.GetPixels());
		this.plotScale = new Vector2((float)this.texture.width / this.limits.width, (float)this.texture.height / this.limits.height);
		this.i = 0;
		while (this.i < data.Count)
		{
			this.PlotPoint(new Vector2((float)this.i, data[this.i]), this.texture);
			this.i++;
		}
		this.texture.Apply();
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x000111E0 File Offset: 0x0000F3E0
	private void PlotPoint(Vector2 point, Texture2D tex)
	{
		this.offScale = false;
		point += this.offset;
		point.x *= this.plotScale.x;
		point.y *= this.plotScale.y;
		if (point.x > (float)tex.width || point.x < 0f)
		{
			return;
		}
		if (point.y > (float)tex.height)
		{
			this.offScale = true;
			point.y = (float)(tex.height - 1);
		}
		else if (point.y < 0f)
		{
			return;
		}
		tex.SetPixel(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), (!this.offScale) ? this.penColor : Color.red);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x000112D4 File Offset: 0x0000F4D4
	private void Circle(Texture2D tex, int cx, int cy, int r, Color col)
	{
		int num = r;
		int num2 = 0 - r;
		float num3 = Mathf.Ceil((float)r / Mathf.Sqrt(2f));
		int num4 = 0;
		while ((float)num4 <= num3)
		{
			tex.SetPixel(cx + num4, cy + num, col);
			tex.SetPixel(cx + num4, cy - num, col);
			tex.SetPixel(cx - num4, cy + num, col);
			tex.SetPixel(cx - num4, cy - num, col);
			tex.SetPixel(cx + num, cy + num4, col);
			tex.SetPixel(cx - num, cy + num4, col);
			tex.SetPixel(cx + num, cy - num4, col);
			tex.SetPixel(cx - num, cy - num4, col);
			num2 += 2 * num4 + 1;
			if (num2 > 0)
			{
				num2 += 2 - 2 * num--;
			}
			num4++;
		}
	}

	// Token: 0x0400020E RID: 526
	private Texture2D blankTexture;

	// Token: 0x0400020F RID: 527
	public Rect limits = new Rect(0f, 0f, 100f, 100f);

	// Token: 0x04000210 RID: 528
	public Vector2 offset = Vector2.zero;

	// Token: 0x04000211 RID: 529
	private Vector2 plotScale = Vector2.one;

	// Token: 0x04000212 RID: 530
	private Color penColor = Color.green;

	// Token: 0x04000213 RID: 531
	private int i;

	// Token: 0x04000214 RID: 532
	private bool offScale;
}
