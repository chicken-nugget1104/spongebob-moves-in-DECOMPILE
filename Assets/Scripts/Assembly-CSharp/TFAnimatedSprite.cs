using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class TFAnimatedSprite : BasicSprite
{
	// Token: 0x06002256 RID: 8790 RVA: 0x000D33D4 File Offset: 0x000D15D4
	public TFAnimatedSprite(Vector2 center, float width, float height, SpriteAnimationModel animModel) : base(null, null, center, width, height)
	{
		this.spriteAnimationModel = animModel;
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000D33EC File Offset: 0x000D15EC
	public TFAnimatedSprite(TFAnimatedSprite prototype) : base(prototype)
	{
		this.spriteAnimationModel = prototype.SpriteAnimationModel;
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06002258 RID: 8792 RVA: 0x000D3404 File Offset: 0x000D1604
	// (set) Token: 0x06002259 RID: 8793 RVA: 0x000D340C File Offset: 0x000D160C
	public override bool Visible
	{
		get
		{
			return this.shouldBeVisible;
		}
		set
		{
			this.shouldBeVisible = value;
			this.UpdateVisibility();
		}
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x0600225A RID: 8794 RVA: 0x000D341C File Offset: 0x000D161C
	// (set) Token: 0x0600225B RID: 8795 RVA: 0x000D3424 File Offset: 0x000D1624
	public SpriteAnimationModel SpriteAnimationModel
	{
		get
		{
			return this.spriteAnimationModel;
		}
		set
		{
			this.spriteAnimationModel = value;
			this.spriteAnimationController.animationModel = this.spriteAnimationModel;
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x0600225C RID: 8796 RVA: 0x000D3440 File Offset: 0x000D1640
	public ULAnimControllerInterface AnimController
	{
		get
		{
			return this.spriteAnimationController;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x0600225D RID: 8797 RVA: 0x000D3448 File Offset: 0x000D1648
	public override string MaterialName
	{
		get
		{
			string text = this.spriteAnimationModel.GetTextureName(this.DefaultDisplayState);
			if (text == null)
			{
				text = this.spriteAnimationModel.GetResourceName(this.DefaultDisplayState);
			}
			return text;
		}
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000D3480 File Offset: 0x000D1680
	public override string GetDisplayState()
	{
		return this.currentDisplayState;
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000D3488 File Offset: 0x000D1688
	public override void ChangeMesh(string state, string hitMeshName)
	{
		this.HitMeshName = hitMeshName;
		if (!string.IsNullOrEmpty(this.HitMeshName))
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("RemappedMesh/" + fileNameWithoutExtension + "_asset");
			if (mesh != null)
			{
				this.HitMeshName = fileNameWithoutExtension + "_asset.fbx";
			}
			else
			{
				mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
			}
			base.GameObject.GetComponent<MeshFilter>().mesh = mesh;
		}
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000D3514 File Offset: 0x000D1714
	public override void DisplayState(string state)
	{
		if (state == null)
		{
			if (this.currentDisplayState != null)
			{
				this.Flags |= DisplayControllerFlags.SWITCHED_STATE;
			}
			this.validCurrentDisplayState = false;
			this.currentDisplayState = null;
		}
		else
		{
			if (this.currentDisplayState != null && this.currentDisplayState.Equals(state))
			{
				return;
			}
			if (!this.spriteAnimationModel.HasAnimation(state))
			{
				state = this.DefaultDisplayState;
			}
			TFUtils.Assert(this.spriteAnimationModel.HasAnimation(state), string.Format("TFAnimatedSprite '{0}' display state should exist but doesn't.", this.DefaultDisplayState));
			string materialName = this.spriteAnimationModel.GetMaterialName(state);
			if (materialName == null)
			{
				this.validCurrentDisplayState = false;
			}
			else
			{
				base.GameObject.renderer.material = TextureLibrarian.LookUp(materialName);
				this.AnimController.EnableAnimation(true);
				this.AnimController.PlayAnimation(state);
				this.validCurrentDisplayState = true;
				this.currentDisplayState = state;
				string animName = state;
				if (!this.spriteAnimationModel.HasQuadData(animName))
				{
					animName = this.DefaultDisplayState;
				}
				if (this.spriteAnimationModel.HasQuadData(animName))
				{
					int num = this.spriteAnimationModel.Width(animName);
					int num2 = this.spriteAnimationModel.Height(animName);
					if (this.Width != (float)num || this.Height != (float)num2)
					{
						this.Resize(new Vector2(0f, -0.5f * (float)num2), (float)num, (float)num2);
					}
				}
				base.Scale = this.spriteAnimationModel.Scale(animName);
				this.Flags |= DisplayControllerFlags.SWITCHED_STATE;
				if (this.spriteAnimationModel.CellCount(state) > 1 && this.spriteAnimationModel.FramesPerSecond(state) > 0)
				{
					this.Flags |= DisplayControllerFlags.NEED_UPDATE;
				}
			}
		}
		this.UpdateVisibility();
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x000D36D8 File Offset: 0x000D18D8
	public override void AddDisplayState(Dictionary<string, object> dict)
	{
		this.spriteAnimationModel.AddAnimationDataWithBlueprint(dict);
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x000D36E8 File Offset: 0x000D18E8
	public static double CalcWorldSize(double textureValue, double scaleFactor)
	{
		return textureValue * 0.1302 * scaleFactor;
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000D36F8 File Offset: 0x000D18F8
	public override IDisplayController Clone(DisplayControllerManager dcm)
	{
		TFAnimatedSprite tfanimatedSprite = new TFAnimatedSprite(this);
		tfanimatedSprite.Initialize();
		return tfanimatedSprite;
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x000D3714 File Offset: 0x000D1914
	public override IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		TFAnimatedSprite tfanimatedSprite = new TFAnimatedSprite(this);
		tfanimatedSprite.HitMeshName = hitMeshName;
		tfanimatedSprite.SeparateTap = separateTap;
		tfanimatedSprite.Initialize();
		return tfanimatedSprite;
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000D3740 File Offset: 0x000D1940
	public override IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		IDisplayController displayController = this.Clone(dcm);
		displayController.DisplayState(this.DefaultDisplayState);
		displayController.Visible = true;
		return displayController;
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000D376C File Offset: 0x000D196C
	private void UpdateVisibility()
	{
		if (this.validCurrentDisplayState && this.shouldBeVisible)
		{
			base.GameObject.renderer.enabled = true;
			this.flags |= DisplayControllerFlags.VISIBLE_AND_VALID_STATE;
		}
		else
		{
			base.GameObject.renderer.enabled = false;
			this.flags &= ~DisplayControllerFlags.VISIBLE_AND_VALID_STATE;
		}
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000D37D4 File Offset: 0x000D19D4
	public override void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		base.OnUpdate(sceneCamera, psm);
		this.spriteAnimationController.OnUpdate();
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000D37EC File Offset: 0x000D19EC
	protected override void Initialize()
	{
		if (!string.IsNullOrEmpty(this.HitMeshName))
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("RemappedMesh/" + fileNameWithoutExtension + "_asset");
			GameObject gameObject;
			if (mesh != null)
			{
				this.HitMeshName = fileNameWithoutExtension + "_asset.fbx";
				gameObject = base.CreateQuadGameObject("TFAnimatedSprite", null, null, mesh);
				if (this.SeparateTap)
				{
					mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
				}
			}
			else
			{
				mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
				gameObject = base.CreateQuadGameObject("TFAnimatedSprite", null, null, null);
			}
			this.spriteAnimationController = new ULSpriteAnimController();
			ULSpriteAnimController ulspriteAnimController = this.spriteAnimationController;
			int[] array = new int[4];
			array[0] = 3;
			array[1] = 1;
			array[2] = 2;
			ulspriteAnimController.uvToVertMap = array;
			this.spriteAnimationController.animationModel = this.spriteAnimationModel;
			this.spriteAnimationController.quad = gameObject.GetComponent<MeshFilter>();
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;
		}
		else
		{
			GameObject gameObject = base.CreateQuadGameObject("TFAnimatedSprite", null, null, null);
			this.spriteAnimationController = new ULSpriteAnimController();
			ULSpriteAnimController ulspriteAnimController2 = this.spriteAnimationController;
			int[] array2 = new int[4];
			array2[0] = 3;
			array2[1] = 1;
			array2[2] = 2;
			ulspriteAnimController2.uvToVertMap = array2;
			this.spriteAnimationController.animationModel = this.spriteAnimationModel;
			this.spriteAnimationController.quad = gameObject.GetComponent<MeshFilter>();
		}
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x000D396C File Offset: 0x000D1B6C
	public override void PublicInitialize()
	{
		this.Initialize();
	}

	// Token: 0x0400153E RID: 5438
	private bool shouldBeVisible;

	// Token: 0x0400153F RID: 5439
	private bool validCurrentDisplayState;

	// Token: 0x04001540 RID: 5440
	private string currentDisplayState;

	// Token: 0x04001541 RID: 5441
	private SpriteAnimationModel spriteAnimationModel;

	// Token: 0x04001542 RID: 5442
	private ULSpriteAnimController spriteAnimationController;
}
