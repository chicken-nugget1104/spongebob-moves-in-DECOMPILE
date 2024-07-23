using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class ULRenderTexture
{
	// Token: 0x06002456 RID: 9302 RVA: 0x000DE6C4 File Offset: 0x000DC8C4
	public ULRenderTexture(int squareSize, string materialName, string shaderIdentifier)
	{
		this.texture = new RenderTexture(squareSize, squareSize, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		this.texture.wrapMode = TextureWrapMode.Clamp;
		this.texture.anisoLevel = 0;
		this.texture.filterMode = FilterMode.Bilinear;
		Shader shader = Shader.Find(shaderIdentifier);
		this.material = new Material(shader);
		this.material.name = materialName;
		this.material.mainTexture = this.texture;
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x06002457 RID: 9303 RVA: 0x000DE73C File Offset: 0x000DC93C
	public Material RMaterial
	{
		get
		{
			return this.material;
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x06002458 RID: 9304 RVA: 0x000DE744 File Offset: 0x000DC944
	public RenderTexture RTexture
	{
		get
		{
			return this.texture;
		}
	}

	// Token: 0x0400164D RID: 5709
	private RenderTexture texture;

	// Token: 0x0400164E RID: 5710
	private Material material;
}
