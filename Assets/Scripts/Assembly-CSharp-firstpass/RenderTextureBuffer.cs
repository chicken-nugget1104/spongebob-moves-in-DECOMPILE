using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class RenderTextureBuffer
{
	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0001D544 File Offset: 0x0001B744
	public Texture Texture
	{
		get
		{
			return this.mRenderBuffer;
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0001D54C File Offset: 0x0001B74C
	private RenderTextureBuffer.PendingTextures CreatePending()
	{
		if (this.mPendingWarehouse.Count == 0)
		{
			return new RenderTextureBuffer.PendingTextures();
		}
		RenderTextureBuffer.PendingTextures result = this.mPendingWarehouse[this.mPendingWarehouse.Count - 1];
		this.mPendingWarehouse.RemoveAt(this.mPendingWarehouse.Count - 1);
		return result;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0001D5A4 File Offset: 0x0001B7A4
	private void ReturnPending(RenderTextureBuffer.PendingTextures p)
	{
		p.Clear();
		if (this.mPendingWarehouse.Count != this.mPendingWarehouse.Capacity)
		{
			this.mPendingWarehouse.Add(p);
		}
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x0001D5E0 File Offset: 0x0001B7E0
	public bool FindBestSupportedFormatsWithAlpha(RenderTextureBuffer.QualityMode q)
	{
		if (q > RenderTextureBuffer.QualityMode.VeryHigh)
		{
			return false;
		}
		RenderTextureFormat[] tests;
		if (q == RenderTextureBuffer.QualityMode.VeryLow || q == RenderTextureBuffer.QualityMode.Low)
		{
			tests = new RenderTextureFormat[]
			{
				RenderTextureFormat.ARGB1555,
				RenderTextureFormat.ARGB4444
			};
		}
		else if (q == RenderTextureBuffer.QualityMode.Medium)
		{
			tests = new RenderTextureFormat[]
			{
				RenderTextureFormat.ARGB4444,
				RenderTextureFormat.ARGB32,
				RenderTextureFormat.ARGBHalf
			};
		}
		else
		{
			tests = new RenderTextureFormat[]
			{
				RenderTextureFormat.ARGB32,
				RenderTextureFormat.ARGBFloat
			};
		}
		this.mTextureFormat = this.FindFirstSupported(tests);
		if (this.mTextureFormat == RenderTextureFormat.Depth)
		{
			Debug.LogError("No Valid Render Format Found For Quality: " + q.ToString());
			RenderTextureBuffer.QualityMode q2 = q + 1;
			return this.FindBestSupportedFormatsWithAlpha(q2);
		}
		return true;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x0001D680 File Offset: 0x0001B880
	private RenderTextureFormat FindFirstSupported(RenderTextureFormat[] tests)
	{
		RenderTextureFormat result = RenderTextureFormat.Depth;
		for (int i = 0; i < tests.Length; i++)
		{
			if (SystemInfo.SupportsRenderTextureFormat(tests[i]))
			{
				result = tests[i];
				break;
			}
		}
		return result;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0001D6BC File Offset: 0x0001B8BC
	private bool CheckValidFormatFound()
	{
		return this.mTextureFormat != RenderTextureFormat.Depth;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0001D6CC File Offset: 0x0001B8CC
	public RenderTextureBuffer.QualityModeSettings SettingsForMode(RenderTextureBuffer.QualityMode mode)
	{
		RenderTextureBuffer.QualityModeSettings qualityModeSettings = new RenderTextureBuffer.QualityModeSettings();
		if (mode == RenderTextureBuffer.QualityMode.VeryLow)
		{
			qualityModeSettings.Height = (qualityModeSettings.Width = 1024);
			qualityModeSettings.Depth = 0;
		}
		else if (mode == RenderTextureBuffer.QualityMode.Low)
		{
			qualityModeSettings.Height = (qualityModeSettings.Width = 1024);
			qualityModeSettings.Depth = 0;
		}
		else if (mode == RenderTextureBuffer.QualityMode.Medium)
		{
			qualityModeSettings.Height = (qualityModeSettings.Width = 2048);
			qualityModeSettings.Depth = 0;
		}
		else if (mode == RenderTextureBuffer.QualityMode.High)
		{
			qualityModeSettings.Height = (qualityModeSettings.Width = 4096);
			qualityModeSettings.Depth = 0;
		}
		else if (mode == RenderTextureBuffer.QualityMode.VeryHigh)
		{
			qualityModeSettings.Height = (qualityModeSettings.Width = 4096);
			qualityModeSettings.Depth = 0;
		}
		return qualityModeSettings;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0001D7A0 File Offset: 0x0001B9A0
	public bool Create(RenderTextureBuffer.QualityMode mode, bool clearBuffer = false)
	{
		this.FindBestSupportedFormatsWithAlpha(mode);
		RenderTextureBuffer.QualityModeSettings qualityModeSettings = this.SettingsForMode(mode);
		this.mRenderBuffer = new RenderTexture(qualityModeSettings.Width, qualityModeSettings.Height, qualityModeSettings.Depth, this.mTextureFormat, RenderTextureReadWrite.Linear);
		this.mRenderBuffer.useMipMap = false;
		if (clearBuffer)
		{
			RenderTexture.active = this.mRenderBuffer;
			GL.Clear(true, true, Color.white);
			RenderTexture.active = null;
		}
		if (!this.mRenderBuffer.Create())
		{
			Debug.LogError("RenderTexture failed to be created");
			return false;
		}
		this.mMapTree = new UVMapTree(new Vector2((float)qualityModeSettings.Width, (float)qualityModeSettings.Height), -1);
		return true;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0001D84C File Offset: 0x0001BA4C
	public bool AddTexture(Texture tx, bool destroyAfterLoad = false, bool processImmidiatly = true)
	{
		if (tx == null)
		{
			return false;
		}
		if (tx.width == 0)
		{
			return false;
		}
		if (this.mTexturesToAdd == null)
		{
			this.mTexturesToAdd = new List<RenderTextureBuffer.PendingTextures>(2);
		}
		RenderTextureBuffer.PendingTextures pendingTextures = this.CreatePending();
		pendingTextures.processImmidiatly = processImmidiatly;
		pendingTextures.texture = tx;
		pendingTextures.destroyOnLoad = destroyAfterLoad;
		bool result;
		if (processImmidiatly)
		{
			pendingTextures.uvs.x = (float)pendingTextures.texture.width;
			pendingTextures.uvs.y = (float)pendingTextures.texture.height;
			result = this.mMapTree.AddTexture(pendingTextures.uvs, ref pendingTextures.uvs);
		}
		else
		{
			result = true;
		}
		this.mTexturesToAdd.Add(pendingTextures);
		return result;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0001D90C File Offset: 0x0001BB0C
	public void UpdateRenderTexture()
	{
		if (this.mTexturesToAdd == null)
		{
			return;
		}
		if (this.mTexturesToAdd.Count == 0)
		{
			return;
		}
		Rect screenRect = default(Rect);
		RenderTexture.active = this.mRenderBuffer;
		GL.PushMatrix();
		GL.LoadPixelMatrix(0f, (float)this.mRenderBuffer.width, (float)this.mRenderBuffer.height, 0f);
		Vector2 vector = new Vector2(0f, 0f);
		for (int i = 0; i < this.mTexturesToAdd.Count; i++)
		{
			RenderTextureBuffer.PendingTextures pendingTextures = this.mTexturesToAdd[i];
			Texture texture = pendingTextures.texture;
			vector.x = (float)texture.width;
			vector.y = (float)texture.height;
			if (!pendingTextures.processImmidiatly)
			{
				this.mMapTree.AddTexture(vector, ref pendingTextures.uvs);
			}
			screenRect.position = pendingTextures.uvs;
			screenRect.size = vector;
			Debug.Log(screenRect.ToString());
			Graphics.DrawTexture(screenRect, texture);
			if (pendingTextures.destroyOnLoad)
			{
				Resources.UnloadAsset(texture);
			}
		}
		GL.PopMatrix();
		RenderTexture.active = null;
		this.mTexturesToAdd.Clear();
	}

	// Token: 0x040004BF RID: 1215
	private const RenderTextureFormat INVALID_FORMAT = RenderTextureFormat.Depth;

	// Token: 0x040004C0 RID: 1216
	private RenderTextureFormat mTextureFormat = RenderTextureFormat.Depth;

	// Token: 0x040004C1 RID: 1217
	private RenderTexture mRenderBuffer;

	// Token: 0x040004C2 RID: 1218
	private UVMapTree mMapTree;

	// Token: 0x040004C3 RID: 1219
	private List<RenderTextureBuffer.PendingTextures> mPendingWarehouse = new List<RenderTextureBuffer.PendingTextures>(32);

	// Token: 0x040004C4 RID: 1220
	private List<RenderTextureBuffer.PendingTextures> mTexturesToAdd;

	// Token: 0x020000C4 RID: 196
	public enum QualityMode
	{
		// Token: 0x040004C6 RID: 1222
		VeryLow,
		// Token: 0x040004C7 RID: 1223
		Low,
		// Token: 0x040004C8 RID: 1224
		Medium,
		// Token: 0x040004C9 RID: 1225
		High,
		// Token: 0x040004CA RID: 1226
		VeryHigh
	}

	// Token: 0x020000C5 RID: 197
	public class QualityModeSettings
	{
		// Token: 0x040004CB RID: 1227
		public int Width;

		// Token: 0x040004CC RID: 1228
		public int Height;

		// Token: 0x040004CD RID: 1229
		public int Depth;
	}

	// Token: 0x020000C6 RID: 198
	private class PendingTextures
	{
		// Token: 0x060007C4 RID: 1988 RVA: 0x0001DA4C File Offset: 0x0001BC4C
		public PendingTextures()
		{
			this.uvs = default(Vector2);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001DA70 File Offset: 0x0001BC70
		public void Clear()
		{
			this.texture = null;
			this.uvs.x = (this.uvs.y = 0f);
		}

		// Token: 0x040004CE RID: 1230
		public Texture texture;

		// Token: 0x040004CF RID: 1231
		public Vector2 uvs;

		// Token: 0x040004D0 RID: 1232
		public bool destroyOnLoad;

		// Token: 0x040004D1 RID: 1233
		public bool processImmidiatly;
	}
}
