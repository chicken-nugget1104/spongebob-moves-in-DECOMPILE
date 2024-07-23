using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class PaperdollRenderTexture : BasicSprite
{
	// Token: 0x06002106 RID: 8454 RVA: 0x000CC148 File Offset: 0x000CA348
	public PaperdollRenderTexture(Vector2 center, float width, float height) : base(null, null, center, width, height)
	{
		this.animationGroupManager = new AnimationGroupManager();
		this.cameraOffset = new Vector3(0f, 0f, 0f);
		this.cameraLookAtOffset = new Vector3(0f, 0f, 0f);
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x000CC1A0 File Offset: 0x000CA3A0
	public PaperdollRenderTexture(PaperdollRenderTexture prototype, DisplayControllerManager dcm) : base(prototype)
	{
		this.displayControllerManager = dcm;
		this.cameraOffset = prototype.cameraOffset;
		this.cameraLookAtOffset = prototype.cameraLookAtOffset;
		this.animationGroupManager = prototype.animationGroupManager;
		this.animationGroupManager.ApplyToGroups(new AnimationGroupManager.ApplyDelegate(this.ApplyAnimationGroupToSkeleton));
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000CC204 File Offset: 0x000CA404
	private void ApplyAnimationGroupToSkeleton(AnimationGroupManager.AnimGroup ag)
	{
		bool flag;
		GameObject skeleton = this.displayControllerManager.Skeletons.GetSkeleton(ag.skeletonName, true, out flag);
		ULRenderTextureCameraRig.SetRenderLayer(skeleton, 22);
		if (flag)
		{
			Animation component = skeleton.GetComponent<Animation>();
			ag.animModel.ApplyAnimationSettings(component);
		}
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000CC24C File Offset: 0x000CA44C
	protected override void Initialize()
	{
		this.gameObjectPaperdoll = UnityGameResources.CreateEmpty("paperdollRenderTexture");
		this.animationController = new ULAnimController();
		this.gameObjectPaperdoll.transform.localPosition = new Vector3(0f, 0f, 0f);
		global::RenderTextureManager renderTextureManager = this.displayControllerManager.RenderTextureManager;
		this.renderTextureRig = renderTextureManager.AddGameObject(this.gameObjectPaperdoll, new ULRenderTextureCameraRig.RelativeCamDelegate(this.CamSetup), PaperdollRenderTexture.RENDERTEXTURE_SHADER);
		base.CreateQuadGameObject("PaperdollSprite", this.renderTextureRig.target.RMaterial, null, null);
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x0600210B RID: 8459 RVA: 0x000CC2F0 File Offset: 0x000CA4F0
	// (set) Token: 0x0600210C RID: 8460 RVA: 0x000CC2F8 File Offset: 0x000CA4F8
	public Vector3 CameraOffset
	{
		get
		{
			return this.cameraOffset;
		}
		set
		{
			this.cameraOffset = value;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x0600210D RID: 8461 RVA: 0x000CC304 File Offset: 0x000CA504
	// (set) Token: 0x0600210E RID: 8462 RVA: 0x000CC30C File Offset: 0x000CA50C
	public Vector3 CameraLookAtOffset
	{
		get
		{
			return this.cameraLookAtOffset;
		}
		set
		{
			this.cameraLookAtOffset = value;
		}
	}

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x0600210F RID: 8463 RVA: 0x000CC318 File Offset: 0x000CA518
	public override string MaterialName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000CC320 File Offset: 0x000CA520
	protected void CamSetup(GameObject subject, Camera cam)
	{
		cam.transform.position = subject.transform.position + this.cameraOffset;
		cam.transform.LookAt(subject.transform.position + this.cameraLookAtOffset);
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000CC370 File Offset: 0x000CA570
	public override void AddDisplayState(Dictionary<string, object> dict)
	{
		this.animationGroupManager.AddDisplayStateWithBlueprint(dict);
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000CC380 File Offset: 0x000CA580
	public override IDisplayController Clone(DisplayControllerManager dcm)
	{
		PaperdollRenderTexture paperdollRenderTexture = new PaperdollRenderTexture(this, dcm);
		paperdollRenderTexture.Initialize();
		return paperdollRenderTexture;
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000CC39C File Offset: 0x000CA59C
	public override void DisplayState(string state)
	{
		if (state == null)
		{
			this.Visible = false;
		}
		else
		{
			if (this.currentAnimationState != null && this.currentAnimationState.Equals(state))
			{
				return;
			}
			if (this.animationGroupManager.FindAnimGroup(state) == null)
			{
				state = this.DefaultDisplayState;
			}
			AnimationGroupManager.AnimGroup animGroup = this.animationGroupManager.FindAnimGroup(state);
			TFUtils.Assert(animGroup != null, string.Format("Paperdoll '{0}' display state should exist but doesn't.", this.DefaultDisplayState));
			bool flag;
			GameObject skeleton = this.displayControllerManager.Skeletons.GetSkeleton(animGroup.skeletonName, false, out flag);
			Animation component = skeleton.GetComponent<Animation>();
			if (this.currentAnimGroup != animGroup)
			{
				this.animationController.UnityAnimation = component;
				this.animationController.AnimationModel = animGroup.animModel;
				this.currentAnimGroup = animGroup;
			}
			this.currentAnimationState = state;
			this.animationTime = 0f;
			this.Visible = true;
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (set) Token: 0x06002114 RID: 8468 RVA: 0x000CC488 File Offset: 0x000CA688
	public override Vector3 Position
	{
		set
		{
			Paperdoll.HorizontalFlipWithDirectionAndCamera(this, value - base.Position, Camera.main);
			base.Position = value;
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06002115 RID: 8469 RVA: 0x000CC4A8 File Offset: 0x000CA6A8
	// (set) Token: 0x06002116 RID: 8470 RVA: 0x000CC4B0 File Offset: 0x000CA6B0
	public DisplayControllerManager DisplayControllerManager
	{
		get
		{
			return this.displayControllerManager;
		}
		set
		{
			this.displayControllerManager = value;
		}
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000CC4BC File Offset: 0x000CA6BC
	private void ParentCurrentSkeleton(Transform parent)
	{
		string skeletonName = this.currentAnimGroup.skeletonName;
		bool flag;
		GameObject skeleton = this.displayControllerManager.Skeletons.GetSkeleton(skeletonName, false, out flag);
		skeleton.transform.parent = parent;
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06002118 RID: 8472 RVA: 0x000CC4F8 File Offset: 0x000CA6F8
	public override int NumberOfLevelsOfDetail
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06002119 RID: 8473 RVA: 0x000CC4FC File Offset: 0x000CA6FC
	public override int MaxLevelOfDetail
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x0600211A RID: 8474 RVA: 0x000CC500 File Offset: 0x000CA700
	public void UpdateLOD(Camera sceneCamera)
	{
		if (this.LevelOfDetail != 1 && sceneCamera.orthographicSize >= 200f)
		{
			this.LevelOfDetail = 1;
		}
		else if (this.LevelOfDetail != 0 && sceneCamera.orthographicSize < 200f)
		{
			this.LevelOfDetail = 0;
		}
	}

	// Token: 0x0600211B RID: 8475 RVA: 0x000CC558 File Offset: 0x000CA758
	public override void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		base.OnUpdate(sceneCamera, psm);
		this.UpdateLOD(sceneCamera);
		this.animationTime += Time.deltaTime;
		if (this.LevelOfDetail == 0)
		{
			this.ParentCurrentSkeleton(this.gameObjectPaperdoll.transform);
			this.animationController.Sample(this.currentAnimationState, this.animationTime);
			this.displayControllerManager.RenderTextureManager.RenderEntry(this.renderTextureRig);
			this.ParentCurrentSkeleton(null);
		}
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x000CC5D8 File Offset: 0x000CA7D8
	public override void Destroy()
	{
		base.Destroy();
		UnityGameResources.Destroy(this.gameObjectPaperdoll);
	}

	// Token: 0x04001429 RID: 5161
	public const int PAPERDOLL_RENDERTEXTURE_NUM_LODS = 2;

	// Token: 0x0400142A RID: 5162
	public const int PAPERDOLL_RENDERTEXTURE_MAX_LOD = 1;

	// Token: 0x0400142B RID: 5163
	public const int LOD_1_ORTHOGRAPHIC_SIZE = 200;

	// Token: 0x0400142C RID: 5164
	private static string RENDERTEXTURE_SHADER = "Unlit/TransparentTint";

	// Token: 0x0400142D RID: 5165
	private GameObject gameObjectPaperdoll;

	// Token: 0x0400142E RID: 5166
	private Vector3 cameraOffset;

	// Token: 0x0400142F RID: 5167
	private Vector3 cameraLookAtOffset;

	// Token: 0x04001430 RID: 5168
	private ULAnimController animationController;

	// Token: 0x04001431 RID: 5169
	private AnimationGroupManager animationGroupManager;

	// Token: 0x04001432 RID: 5170
	private AnimationGroupManager.AnimGroup currentAnimGroup;

	// Token: 0x04001433 RID: 5171
	private float animationTime;

	// Token: 0x04001434 RID: 5172
	private string currentAnimationState;

	// Token: 0x04001435 RID: 5173
	private DisplayControllerManager displayControllerManager;

	// Token: 0x04001436 RID: 5174
	private ULRenderTextureBatchEntry renderTextureRig;
}
