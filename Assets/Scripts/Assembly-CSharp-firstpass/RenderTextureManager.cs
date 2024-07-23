using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class RenderTextureManager
{
	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0001DABC File Offset: 0x0001BCBC
	public static RenderTextureManager Active
	{
		get
		{
			return RenderTextureManager.sActive;
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x0001DAC4 File Offset: 0x0001BCC4
	public static void CreateActive(RenderTextureBuffer.QualityMode quality)
	{
		if (RenderTextureManager.sActive != null)
		{
			return;
		}
		RenderTextureManager.sActive = new RenderTextureManager();
		RenderTextureManager.sActive.mQuality = quality;
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0001DAF4 File Offset: 0x0001BCF4
	public bool AddTexture(Texture texture, bool destroyOnLoad, bool processInstantly = false)
	{
		bool flag = false;
		for (int i = 0; i < this.mBufferList.Count; i++)
		{
			if (this.mBufferList[i].AddTexture(texture, destroyOnLoad, true))
			{
				if (processInstantly)
				{
					this.mBufferList[i].UpdateRenderTexture();
				}
				flag = true;
				break;
			}
		}
		if (!flag && this.mBufferList.Count != this.mBufferList.Capacity)
		{
			RenderTextureBuffer renderTextureBuffer = new RenderTextureBuffer();
			this.mBufferList.Add(renderTextureBuffer);
			if (renderTextureBuffer.Create(this.mQuality, false))
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
				gameObject.name = "Buffer_" + this.mBufferList.Count;
				gameObject.renderer.material.mainTexture = renderTextureBuffer.Texture;
				if (renderTextureBuffer.AddTexture(texture, destroyOnLoad, true))
				{
					if (processInstantly)
					{
						renderTextureBuffer.UpdateRenderTexture();
					}
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x0001DBF8 File Offset: 0x0001BDF8
	public void UpdateRenderBuffers()
	{
		for (int i = 0; i < this.mBufferList.Count; i++)
		{
			this.mBufferList[i].UpdateRenderTexture();
		}
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0001DC34 File Offset: 0x0001BE34
	public static void DestroyActive()
	{
		RenderTextureManager.sActive = null;
	}

	// Token: 0x040004D2 RID: 1234
	private const int MaxAtlases = 4;

	// Token: 0x040004D3 RID: 1235
	private List<RenderTextureBuffer> mBufferList = new List<RenderTextureBuffer>(4);

	// Token: 0x040004D4 RID: 1236
	private static RenderTextureManager sActive;

	// Token: 0x040004D5 RID: 1237
	private RenderTextureBuffer.QualityMode mQuality;
}
