using System;
using System.IO;
using UnityEngine;
using Yarg;

// Token: 0x0200048B RID: 1163
public class ULSpriteAnimController : ULAnimControllerInterface
{
	// Token: 0x06002469 RID: 9321 RVA: 0x000DEB10 File Offset: 0x000DCD10
	protected void StartAnim()
	{
		this.elapsed = 0f;
		this.SetupSprite();
		this.UpdateSprite();
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000DEB2C File Offset: 0x000DCD2C
	public void OnUpdate()
	{
		this.elapsed += Time.deltaTime;
		if (this.animate && this.currentAnimationSetting.cellCount > 1)
		{
			this.UpdateSprite();
		}
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000DEB70 File Offset: 0x000DCD70
	private void SetupSprite()
	{
		ULSpriteAnimationSetting ulspriteAnimationSetting = this.currentAnimationSetting;
		string materialName = this.animationModel.GetMaterialName(ulspriteAnimationSetting.animName);
		if (materialName != null)
		{
			this.material = TextureLibrarian.LookUp(materialName);
			if (this.material && this.quad)
			{
				MeshRenderer meshRenderer = (MeshRenderer)this.quad.GetComponent("MeshRenderer");
				if (meshRenderer)
				{
					if (ulspriteAnimationSetting.texture != null && !string.IsNullOrEmpty(ulspriteAnimationSetting.maskName))
					{
						AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(ulspriteAnimationSetting.texture);
						this.CreateMaterial(meshRenderer, ulspriteAnimationSetting);
						atlasCoords.atlasCoords.frame.x = 0f;
						atlasCoords.atlasCoords.frame.y = 0f;
						atlasCoords.atlasCoords.frame.width = atlasCoords.atlas.meta.size.width;
						atlasCoords.atlasCoords.frame.height = atlasCoords.atlas.meta.size.height;
					}
					else
					{
						meshRenderer.material = this.material;
					}
				}
			}
		}
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000DECA0 File Offset: 0x000DCEA0
	private void CreateMaterial(MeshRenderer mr, ULSpriteAnimationSetting cs)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(cs.texture);
		string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(cs.maskName);
		string path = "Masks/" + fileNameWithoutExtension;
		string path2 = "Masks/" + fileNameWithoutExtension2;
		Texture texture = (Texture)Resources.Load(path, typeof(Texture));
		Texture texture2 = (Texture)Resources.Load(path2, typeof(Texture));
		Material material = new Material(Shader.Find("Custom/TwoImageColorOverlay"));
		mr.material = material;
		Color color = mr.material.GetColor("_Color");
		mr.material.SetTexture("_MainTex", texture);
		mr.material.SetTexture("_AlphaTex", texture2);
		mr.material.SetColor("_Color", cs.mainColor);
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x000DED78 File Offset: 0x000DCF78
	private void UpdateSprite()
	{
		ULSpriteAnimationSetting ulspriteAnimationSetting = this.currentAnimationSetting;
		this.seconds_per_frame = ((ulspriteAnimationSetting.framesPerSecond != 0) ? (1f / (float)ulspriteAnimationSetting.framesPerSecond) : 0f);
		if (ulspriteAnimationSetting.timingTotal == 0f)
		{
			ULSpriteAnimationSetting.LoopMode loopMode = ulspriteAnimationSetting.loopMode;
			if (loopMode != ULSpriteAnimationSetting.LoopMode.None)
			{
				if (loopMode == ULSpriteAnimationSetting.LoopMode.Loop)
				{
					this.elapsed = ((this.seconds_per_frame <= 0f) ? 0f : Mathf.Repeat(this.elapsed, (float)ulspriteAnimationSetting.cellCount * this.seconds_per_frame));
					this.frame = ((this.seconds_per_frame <= 0f) ? 0f : (this.elapsed / this.seconds_per_frame));
				}
			}
			else
			{
				if (this.elapsed > (float)ulspriteAnimationSetting.cellCount * this.seconds_per_frame)
				{
					this.elapsed = (float)ulspriteAnimationSetting.cellCount * this.seconds_per_frame;
				}
				this.frame = this.elapsed / this.seconds_per_frame;
			}
		}
		else
		{
			ULSpriteAnimationSetting.LoopMode loopMode = ulspriteAnimationSetting.loopMode;
			if (loopMode != ULSpriteAnimationSetting.LoopMode.None)
			{
				if (loopMode == ULSpriteAnimationSetting.LoopMode.Loop)
				{
					this.elapsed = Mathf.Repeat(this.elapsed, ulspriteAnimationSetting.timingTotal);
				}
			}
			else if (this.elapsed > ulspriteAnimationSetting.timingTotal)
			{
				this.elapsed = ulspriteAnimationSetting.timingTotal;
			}
			this.frame = 0f;
			foreach (float num in ulspriteAnimationSetting.timingList)
			{
				float num2 = num;
				if (this.elapsed <= num2)
				{
					break;
				}
				this.frame += 1f;
			}
		}
		if (this.frame < 0f)
		{
			this.frame = 0f;
		}
		if (this.frame > (float)(ulspriteAnimationSetting.cellCount - 1))
		{
			this.frame = (float)(ulspriteAnimationSetting.cellCount - 1);
		}
		int num3 = (int)((this.frame + (float)ulspriteAnimationSetting.cellStartColumn) % (float)ulspriteAnimationSetting.cellColumns);
		int num4 = (int)((this.frame + (float)ulspriteAnimationSetting.cellStartColumn) / (float)ulspriteAnimationSetting.cellColumns);
		float num5 = ulspriteAnimationSetting.cellWidth * (float)num3 + ulspriteAnimationSetting.cellLeft;
		float num6 = 1f - (ulspriteAnimationSetting.cellHeight * (float)num4 + ulspriteAnimationSetting.cellTop);
		float new_x = ulspriteAnimationSetting.flipH ? (num5 + ulspriteAnimationSetting.cellWidth) : num5;
		float new_x2 = ulspriteAnimationSetting.flipH ? num5 : (num5 + ulspriteAnimationSetting.cellWidth);
		float new_y = ulspriteAnimationSetting.flipV ? (num6 - ulspriteAnimationSetting.cellHeight) : num6;
		float new_y2 = ulspriteAnimationSetting.flipV ? num6 : (num6 - ulspriteAnimationSetting.cellHeight);
		if (ulspriteAnimationSetting.texture != null)
		{
			AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(ulspriteAnimationSetting.texture);
			atlasCoords.atlas.AdjustUVsToFrame(atlasCoords.atlasCoords, ref new_x, ref new_x2, ref new_y, ref new_y2);
		}
		this.uvOrder[0].Set(new_x, new_y);
		this.uvOrder[1].Set(new_x2, new_y);
		this.uvOrder[2].Set(new_x, new_y2);
		this.uvOrder[3].Set(new_x2, new_y2);
		if (this.uvToVertMap != null && this.uvToVertMap.Length == 4)
		{
			this.uvs[0] = this.uvOrder[this.uvToVertMap[0]];
			this.uvs[1] = this.uvOrder[this.uvToVertMap[1]];
			this.uvs[2] = this.uvOrder[this.uvToVertMap[2]];
			this.uvs[3] = this.uvOrder[this.uvToVertMap[3]];
		}
		else
		{
			this.uvs[0] = this.uvOrder[0];
			this.uvs[1] = this.uvOrder[1];
			this.uvs[2] = this.uvOrder[2];
			this.uvs[3] = this.uvOrder[3];
		}
		if (this.quad)
		{
			Mesh mesh = this.quad.mesh;
			if (mesh != null && mesh.uv.Length == this.uvs.Length)
			{
				mesh.uv = this.uvs;
			}
		}
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x000DF29C File Offset: 0x000DD49C
	public bool HasAnimation(string animationName)
	{
		return this.animationModel.HasAnimation(animationName);
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000DF2AC File Offset: 0x000DD4AC
	public bool AnimationEnabled()
	{
		return this.animate;
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000DF2B4 File Offset: 0x000DD4B4
	public void EnableAnimation(bool enabled)
	{
		this.animate = enabled;
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000DF2C0 File Offset: 0x000DD4C0
	private void ApplyAnimation(string animationName)
	{
		ULSpriteAnimationSetting ulspriteAnimationSetting = this.currentAnimationSetting;
		ULSpriteAnimModelInterface ulspriteAnimModelInterface = this.animationModel;
		ulspriteAnimationSetting.animName = animationName;
		ulspriteAnimationSetting.resourceName = ulspriteAnimModelInterface.GetResourceName(animationName);
		ulspriteAnimationSetting.texture = ulspriteAnimModelInterface.GetTextureName(animationName);
		ulspriteAnimationSetting.cellTop = ulspriteAnimModelInterface.CellTop(animationName);
		ulspriteAnimationSetting.cellLeft = ulspriteAnimModelInterface.CellLeft(animationName);
		ulspriteAnimationSetting.cellWidth = ulspriteAnimModelInterface.CellWidth(animationName);
		ulspriteAnimationSetting.cellHeight = ulspriteAnimModelInterface.CellHeight(animationName);
		ulspriteAnimationSetting.cellStartColumn = ulspriteAnimModelInterface.CellStartColumn(animationName);
		ulspriteAnimationSetting.cellColumns = ulspriteAnimModelInterface.CellColumns(animationName);
		ulspriteAnimationSetting.cellCount = ulspriteAnimModelInterface.CellCount(animationName);
		ulspriteAnimationSetting.framesPerSecond = ulspriteAnimModelInterface.FramesPerSecond(animationName);
		ulspriteAnimationSetting.timingTotal = ulspriteAnimModelInterface.TimingTotal(animationName);
		ulspriteAnimationSetting.timingList = ulspriteAnimModelInterface.TimingList(animationName);
		ulspriteAnimationSetting.loopMode = ((!ulspriteAnimModelInterface.Loop(animationName)) ? ULSpriteAnimationSetting.LoopMode.None : ULSpriteAnimationSetting.LoopMode.Loop);
		ulspriteAnimationSetting.flipH = ulspriteAnimModelInterface.FlipH(animationName);
		ulspriteAnimationSetting.flipV = ulspriteAnimModelInterface.FlipV(animationName);
		ulspriteAnimationSetting.mainColor = ulspriteAnimModelInterface.MainColor(animationName);
		ulspriteAnimationSetting.maskName = ulspriteAnimModelInterface.MaskName(animationName);
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000DF3CC File Offset: 0x000DD5CC
	public void PlayAnimation(string animationName)
	{
		if (this.HasAnimation(animationName))
		{
			this.ApplyAnimation(animationName);
			this.EnableAnimation(true);
			this.StartAnim();
		}
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x000DF3FC File Offset: 0x000DD5FC
	public void StopAnimation(string animationName)
	{
		if (animationName.Equals(this.currentAnimationSetting.animName))
		{
			this.EnableAnimation(false);
		}
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000DF41C File Offset: 0x000DD61C
	public void StopAnimations()
	{
		this.EnableAnimation(false);
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x000DF428 File Offset: 0x000DD628
	public void Sample(string animationName, float position)
	{
		this.ApplyAnimation(animationName);
		this.SetupSprite();
		ULSpriteAnimModelInterface ulspriteAnimModelInterface = this.animationModel;
		this.elapsed = position * ((float)ulspriteAnimModelInterface.CellCount(animationName) * 1f / (float)ulspriteAnimModelInterface.FramesPerSecond(animationName));
		this.UpdateSprite();
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000DF470 File Offset: 0x000DD670
	public float NormalizedTimePerFrame(string animationName)
	{
		ULSpriteAnimModelInterface ulspriteAnimModelInterface = this.animationModel;
		float num = (float)ulspriteAnimModelInterface.FramesPerSecond(animationName);
		float num2 = num / ((Application.targetFrameRate >= 0) ? ((float)Application.targetFrameRate) : 60f);
		return 1f / (float)ulspriteAnimModelInterface.CellCount(animationName) * num2;
	}

	// Token: 0x04001657 RID: 5719
	public bool animate;

	// Token: 0x04001658 RID: 5720
	public int[] uvToVertMap;

	// Token: 0x04001659 RID: 5721
	public ULSpriteAnimModelInterface animationModel;

	// Token: 0x0400165A RID: 5722
	public ULSpriteAnimationSetting currentAnimationSetting = new ULSpriteAnimationSetting();

	// Token: 0x0400165B RID: 5723
	public MeshFilter quad;

	// Token: 0x0400165C RID: 5724
	public Material spriteMaterial;

	// Token: 0x0400165D RID: 5725
	private Vector2[] uvs = new Vector2[4];

	// Token: 0x0400165E RID: 5726
	private float frame;

	// Token: 0x0400165F RID: 5727
	private float elapsed;

	// Token: 0x04001660 RID: 5728
	private float seconds_per_frame;

	// Token: 0x04001661 RID: 5729
	private Material material;

	// Token: 0x04001662 RID: 5730
	private Vector2[] uvOrder = new Vector2[]
	{
		Vector2.zero,
		Vector2.zero,
		Vector2.zero,
		Vector2.zero
	};
}
