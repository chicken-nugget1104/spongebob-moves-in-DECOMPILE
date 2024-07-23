using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000425 RID: 1061
internal class Paperdoll : IDisplayController
{
	// Token: 0x060020C1 RID: 8385 RVA: 0x000CA944 File Offset: 0x000C8B44
	public Paperdoll(Vector2 center, float width, float height, Vector3 displayScale, bool flippable, Paperdoll.PaperdollType dolltype)
	{
		this.quadHitObject = new QuadHitObject(center, width, height);
		this.animationGroupManager = new AnimationGroupManager();
		this.animationEventManager = new AnimationEventManager();
		TFUtils.Assert(displayScale.x > 0f && displayScale.y > 0f && displayScale.z > 0f, "Invalid display scale: contains a zero scale factor.");
		this.displayScale = displayScale;
		this.flippable = flippable;
		this.paperDollType = dolltype;
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x000CAA94 File Offset: 0x000C8C94
	public Paperdoll(Paperdoll prototype, DisplayControllerManager dcm)
	{
		this.quadHitObject = prototype.quadHitObject;
		this.animationGroupManager = prototype.animationGroupManager;
		this.animationEventManager = prototype.animationEventManager;
		this.displayScale = prototype.displayScale;
		this.flippable = prototype.flippable;
		this.isPerspectiveInArt = prototype.isPerspectiveInArt;
		this.paperDollType = prototype.paperDollType;
		this.skeletons = new SkeletonCollection();
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000CAC2C File Offset: 0x000C8E2C
	private void ApplyAnimationGroupToSkeleton(AnimationGroupManager.AnimGroup ag)
	{
		bool flag;
		GameObject skeleton = this.skeletons.GetSkeleton(this.GetSkeletonName(ag.skeletonName), true, out flag);
		if (flag)
		{
			skeleton.transform.parent = this.tform;
			Animation component = skeleton.GetComponent<Animation>();
			ag.animModel.ApplyAnimationSettings(component);
			component.cullingType = AnimationCullingType.BasedOnRenderers;
			ULRenderTextureCameraRig.SetRenderLayer(skeleton, 22);
		}
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x000CAC90 File Offset: 0x000C8E90
	public string GetSkeletonName(string name)
	{
		return this.GetSkeletonName(name, null);
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x000CAC9C File Offset: 0x000C8E9C
	public string GetSkeletonName(string name, PaperdollSkin skin)
	{
		string text = name;
		if (skin == null)
		{
			skin = this.dollSkin;
		}
		if (this.dollSkin != null)
		{
			text = this.dollSkin.skeletonReplacement;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = name;
		}
		return text;
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x000CACE0 File Offset: 0x000C8EE0
	public void ApplyCostumeWithLOD(CostumeManager.Costume costume, int did)
	{
		if (costume == null)
		{
			this.dollSkin = null;
			return;
		}
		if (this.paperDollType == Paperdoll.PaperdollType.Other)
		{
			this.dollSkin = null;
			return;
		}
		TFUtils.WarningLog("LoadingCostume: " + costume.m_sMaterial + " : " + costume.m_sSkeleton);
		Blueprint blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, did, false);
		if (blueprint == null)
		{
			return;
		}
		Dictionary<string, PaperdollSkin> dictionary = (Dictionary<string, PaperdollSkin>)blueprint.Invariable["costumes"];
		if (dictionary != null)
		{
			PaperdollSkin paperdollSkin = this.dollSkin;
			if (!string.IsNullOrEmpty(costume.m_sSkeleton))
			{
				dictionary.TryGetValue(costume.m_sSkeleton, out paperdollSkin);
			}
			if (paperdollSkin == this.dollSkin)
			{
				return;
			}
			if (paperdollSkin != null)
			{
				Debug.LogWarning("ApplyCostumeWithLOD Skeleton: " + paperdollSkin.skeletonReplacement);
			}
			bool flag;
			if (this.currentAnimGroup != null)
			{
				GameObject skeleton = this.skeletons.GetSkeleton(this.GetSkeletonName(this.currentAnimGroup.skeletonName), false, out flag);
				if (skeleton != null)
				{
					this.animationController.StopAnimations();
					ULRenderTextureCameraRig.SetRenderLayer(skeleton, 22);
					this.skeletons.Cleanse(this.GetSkeletonName(this.currentAnimGroup.skeletonName));
				}
			}
			this.dollSkin = paperdollSkin;
			this.animationGroupManager.ApplyToGroups(new AnimationGroupManager.ApplyDelegate(this.ApplyAnimationGroupToSkeleton));
			GameObject skeleton2 = this.skeletons.GetSkeleton(this.GetSkeletonName(this.currentAnimGroup.skeletonName), true, out flag);
			if (skeleton2 == null)
			{
				Debug.LogError("ApplyCostumeWithLOD: Failed to Find Costume: " + costume.m_sName);
				return;
			}
			skeleton2.transform.localPosition = new Vector3(0f, 0f, 0f);
			ULRenderTextureCameraRig.SetRenderLayer(skeleton2, LayerMask.NameToLayer("Default"));
			Animation component = skeleton2.GetComponent<Animation>();
			this.animationController.UnityAnimation = component;
			this.animationController.AnimationModel = this.currentAnimGroup.animModel;
			this.animationController.EnableAnimation(true);
			this.animationController.PlayAnimation(this.currentAnimationState);
			this.Visible = true;
			this.SetupAnimationEvents(component);
		}
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x000CAF00 File Offset: 0x000C9100
	public bool Unload(string file)
	{
		return true;
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x000CAF04 File Offset: 0x000C9104
	private void ApplyMaterialLOD()
	{
		if (this.currentMaterialName != null)
		{
			Debug.LogWarning("M: " + this.currentMaterialName);
		}
		if (this.currentMaterialName != "default" && !string.IsNullOrEmpty(this.currentMaterialName))
		{
			return;
		}
		this.currentMaterialName = "default";
		CommonUtils.LevelOfDetail levelOfDetail = CommonUtils.TextureLod();
		if (levelOfDetail == CommonUtils.LevelOfDetail.Low)
		{
			return;
		}
		SkinnedMeshRenderer[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		string text = Paperdoll.PaperdollTypes[(int)this.paperDollType];
		if (componentsInChildren.Length != 0)
		{
			this.currentMaterialName = componentsInChildren[0].name;
		}
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
			if (!(skinnedMeshRenderer == null))
			{
				Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
				int num2 = sharedMaterials.Length;
				Material[] array = new Material[num2];
				for (int j = 0; j <= num2 - 1; j++)
				{
					array[j] = sharedMaterials[j];
					string text2;
					if (levelOfDetail == CommonUtils.LevelOfDetail.Standard)
					{
						text2 = text + array[j].name;
						text2 = text2.TrimEnd(new char[]
						{
							'2'
						});
					}
					else
					{
						text2 = text + array[j].name;
						int num3 = text2.IndexOf("_lr2");
						text2 = ((num3 >= 0) ? text2.Remove(num3, "_lr2".Length) : text2);
					}
					if (!(text2 == text + array[j].name))
					{
						Material material = Resources.Load(text2, typeof(Material)) as Material;
						if (material != null)
						{
							if (this.paperDollType == Paperdoll.PaperdollType.Building)
							{
								material.shader = Shader.Find(material.shader.name);
							}
							Resources.UnloadAsset(array[j]);
							array[j] = material;
						}
						else
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"Did not find material: ",
								text2,
								" for the material ",
								array[j].name,
								" at ",
								text,
								". Consider making one to use higher res textures on higher end devices."
							}));
						}
					}
				}
				skinnedMeshRenderer.sharedMaterials = array;
			}
		}
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000CB148 File Offset: 0x000C9348
	private void ApplyPropLOD(GameObject prop)
	{
		if (prop == null)
		{
			return;
		}
		CommonUtils.LevelOfDetail levelOfDetail = CommonUtils.TextureLod();
		if (levelOfDetail == CommonUtils.LevelOfDetail.Low)
		{
			return;
		}
		SkinnedMeshRenderer[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		string str = Paperdoll.PaperdollTypes[(int)this.paperDollType];
		if (componentsInChildren.Length != 0)
		{
			this.currentMaterialName = componentsInChildren[0].name;
		}
		int num = componentsInChildren.Length;
		for (int i = 0; i < num; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i];
			if (!(skinnedMeshRenderer == null))
			{
				Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
				for (int j = 0; j <= sharedMaterials.Length - 1; j++)
				{
					string text;
					if (levelOfDetail == CommonUtils.LevelOfDetail.Standard)
					{
						text = str + sharedMaterials[j].name;
						text = text.TrimEnd(new char[]
						{
							'2'
						});
					}
					else
					{
						text = str + sharedMaterials[j].name;
						int num2 = text.IndexOf("_lr2");
						text = ((num2 >= 0) ? text.Remove(num2, "_lr2".Length) : text);
					}
					if (!(text == str + sharedMaterials[j].name))
					{
						Material material = Resources.Load(text, typeof(Material)) as Material;
						if (material != null)
						{
							if (this.paperDollType == Paperdoll.PaperdollType.Building)
							{
								material.shader = Shader.Find(material.shader.name);
							}
							Resources.UnloadAsset(sharedMaterials[j]);
							sharedMaterials[j] = material;
						}
					}
				}
				skinnedMeshRenderer.sharedMaterials = sharedMaterials;
			}
		}
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000CB2E8 File Offset: 0x000C94E8
	public IDisplayController Clone(DisplayControllerManager dcm)
	{
		Paperdoll paperdoll = new Paperdoll(this, dcm);
		paperdoll.Initialize();
		paperdoll.LevelOfDetail = 0;
		return paperdoll;
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000CB30C File Offset: 0x000C950C
	public IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		Paperdoll paperdoll = new Paperdoll(this, dcm);
		paperdoll.HitMeshName = hitMeshName;
		paperdoll.SeparateTap = separateTap;
		paperdoll.Initialize();
		paperdoll.LevelOfDetail = 0;
		return paperdoll;
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x000CB340 File Offset: 0x000C9540
	public IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		IDisplayController displayController = this.Clone(dcm);
		displayController.Visible = true;
		return displayController;
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x000CB360 File Offset: 0x000C9560
	private void Initialize()
	{
		this.rootGameObject = UnityGameResources.CreateEmpty("paperdoll");
		this.tform = this.rootGameObject.transform;
		this.animationController = new ULAnimController();
		this.animationGroupManager.ApplyToGroups(new AnimationGroupManager.ApplyDelegate(this.ApplyAnimationGroupToSkeleton));
		this.tform.localScale = this.displayScale;
		this.Color = new Color(1f, 1f, 1f, 1f);
		this.ApplyMaterialLOD();
		if (!string.IsNullOrEmpty(this.HitMeshName))
		{
			MeshCollider meshCollider = this.rootGameObject.AddComponent<MeshCollider>();
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
			meshCollider.mesh = mesh;
		}
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x000CB428 File Offset: 0x000C9628
	public void AddDisplayState(Dictionary<string, object> dict)
	{
		this.animationGroupManager.AddDisplayStateWithBlueprint(dict);
		this.animationEventManager.AddAnimationEventsWithBlueprint(dict);
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x000CB444 File Offset: 0x000C9644
	public void Billboard(BillboardDelegate billboard)
	{
		billboard(this.tform, this);
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x060020D1 RID: 8401 RVA: 0x000CB454 File Offset: 0x000C9654
	// (set) Token: 0x060020D2 RID: 8402 RVA: 0x000CB45C File Offset: 0x000C965C
	public bool Visible
	{
		get
		{
			return this.displayVisible;
		}
		set
		{
			this.displayVisible = value;
			this.Flags |= DisplayControllerFlags.VISIBLE_AND_VALID_STATE;
			Renderer[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<Renderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				componentsInChildren[i].enabled = value;
			}
		}
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x060020D3 RID: 8403 RVA: 0x000CB4AC File Offset: 0x000C96AC
	public bool isVisible
	{
		get
		{
			MeshRenderer[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<MeshRenderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				if (componentsInChildren[i].isVisible)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x060020D4 RID: 8404 RVA: 0x000CB4EC File Offset: 0x000C96EC
	public bool IsDestroyed
	{
		get
		{
			return this.rootGameObject == null;
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x060020D5 RID: 8405 RVA: 0x000CB4FC File Offset: 0x000C96FC
	// (set) Token: 0x060020D6 RID: 8406 RVA: 0x000CB504 File Offset: 0x000C9704
	public float Alpha
	{
		get
		{
			return this.displayAlpha;
		}
		set
		{
			this.displayAlpha = value;
			MeshRenderer[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<MeshRenderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				Color color = componentsInChildren[i].material.color;
				componentsInChildren[i].material.color = new Color(color.r, color.g, color.b, value);
			}
		}
	}

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x060020D7 RID: 8407 RVA: 0x000CB570 File Offset: 0x000C9770
	// (set) Token: 0x060020D8 RID: 8408 RVA: 0x000CB578 File Offset: 0x000C9778
	public Color Color
	{
		get
		{
			return this.displayColor;
		}
		set
		{
			this.displayColor = value;
			MeshRenderer[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<MeshRenderer>();
			int num = componentsInChildren.Length;
			for (int i = 0; i < num; i++)
			{
				componentsInChildren[i].material.color = value;
			}
		}
	}

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x060020D9 RID: 8409 RVA: 0x000CB5BC File Offset: 0x000C97BC
	public string MaterialName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x060020DA RID: 8410 RVA: 0x000CB5C4 File Offset: 0x000C97C4
	public QuadHitObject HitObject
	{
		get
		{
			return this.quadHitObject;
		}
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000CB5CC File Offset: 0x000C97CC
	public bool Intersects(Ray ray)
	{
		MeshCollider component = this.rootGameObject.GetComponent<MeshCollider>();
		if (component != null)
		{
			RaycastHit raycastHit;
			return component.Raycast(ray, out raycastHit, 5000f);
		}
		return this.quadHitObject.Intersects(this.tform, ray, -this.quadHitObject.Center);
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000CB62C File Offset: 0x000C982C
	public string GetDisplayState()
	{
		return this.currentAnimationState;
	}

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x060020DD RID: 8413 RVA: 0x000CB634 File Offset: 0x000C9834
	// (set) Token: 0x060020DE RID: 8414 RVA: 0x000CB63C File Offset: 0x000C983C
	public string DefaultDisplayState
	{
		get
		{
			return this.defaultDisplayState;
		}
		set
		{
			this.defaultDisplayState = value;
		}
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000CB648 File Offset: 0x000C9848
	public void ChangeMesh(string state, string meshName)
	{
		this.HitMeshName = meshName;
		if (!string.IsNullOrEmpty(this.HitMeshName))
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.HitMeshName);
			Mesh mesh = Resources.Load<Mesh>("Meshes/" + fileNameWithoutExtension);
			this.rootGameObject.GetComponent<MeshCollider>().mesh = mesh;
		}
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000CB69C File Offset: 0x000C989C
	public void DisplayState(string state)
	{
		if (state == null)
		{
			this.Visible = false;
			this.currentAnimationState = null;
			this.animationController.StopAnimations();
		}
		else
		{
			if (this.currentAnimationState != null && this.currentAnimationState.Equals(state))
			{
				return;
			}
			AnimationGroupManager.AnimGroup animGroup = this.animationGroupManager.FindAnimGroup(state);
			if (animGroup == null)
			{
				state = this.DefaultDisplayState;
				if (this.currentAnimationState == state)
				{
					return;
				}
				animGroup = this.animationGroupManager.FindAnimGroup(state);
			}
			TFUtils.Assert(animGroup != null, string.Format("Paperdoll '{0}' display state should exist but doesn't.", this.DefaultDisplayState));
			bool flag;
			GameObject skeleton = this.skeletons.GetSkeleton(this.GetSkeletonName(animGroup.skeletonName), false, out flag);
			if (this.currentAnimGroup != null)
			{
				GameObject skeleton2 = this.skeletons.GetSkeleton(this.GetSkeletonName(this.currentAnimGroup.skeletonName), false, out flag);
				if (this.propResource)
				{
					SkeletonAnimationSetting skeletonAnimationSetting = this.currentAnimGroup.animModel.SkeletonSettings(this.currentAnimationState);
					SkeletonAnimationSetting skeletonAnimationSetting2 = animGroup.animModel.SkeletonSettings(state);
					if (skeletonAnimationSetting2.itemResource != skeletonAnimationSetting.itemResource)
					{
						this.RemoveProp(skeletonAnimationSetting.itemBone, skeletonAnimationSetting.itemResource, skeleton2);
					}
					if (skeletonAnimationSetting2.objectResource != skeletonAnimationSetting.objectResource)
					{
						this.RemoveProp(skeletonAnimationSetting.objectBone, skeletonAnimationSetting.objectResource, skeleton2);
					}
				}
				if (skeleton2 != skeleton)
				{
					this.animationController.StopAnimations();
					ULRenderTextureCameraRig.SetRenderLayer(skeleton2, 22);
				}
			}
			ULRenderTextureCameraRig.SetRenderLayer(skeleton, LayerMask.NameToLayer("Default"));
			Animation component = skeleton.GetComponent<Animation>();
			if (this.currentAnimGroup != animGroup)
			{
				this.animationController.UnityAnimation = component;
				this.animationController.AnimationModel = animGroup.animModel;
				this.currentAnimGroup = animGroup;
			}
			this.animationController.EnableAnimation(true);
			this.animationController.PlayAnimation(state);
			this.currentAnimationState = state;
			if (!this.propResource)
			{
				SkeletonAnimationSetting skeletonAnimationSetting3 = this.currentAnimGroup.animModel.SkeletonSettings(this.currentAnimationState);
				if (skeletonAnimationSetting3.itemResource != null)
				{
					this.AttachPropToBoneAndOrient(skeletonAnimationSetting3.itemResource, skeletonAnimationSetting3.itemBone, skeleton, skeletonAnimationSetting3.itemScale);
				}
				if (skeletonAnimationSetting3.objectResource != null)
				{
					this.AttachPropToBoneAndOrient(skeletonAnimationSetting3.objectResource, skeletonAnimationSetting3.objectBone, skeleton, skeletonAnimationSetting3.objectScale);
				}
			}
			this.Visible = true;
			this.SetupAnimationEvents(component);
		}
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000CB920 File Offset: 0x000C9B20
	private void AttachPropToBoneAndOrient(string propPath, string boneName, GameObject def_base_object, Vector3 scale)
	{
		Transform transform = this.GetBone(boneName);
		if (transform == null && boneName == null)
		{
			transform = def_base_object.transform;
		}
		GameObject gameObject = UnityGameResources.Create(propPath);
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty).Trim();
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = scale;
		gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		this.ApplyPropLOD(gameObject);
		this.propResource = true;
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000CB9E8 File Offset: 0x000C9BE8
	private void RemoveProp(string boneName, string propName, GameObject def_base_object)
	{
		Transform transform = null;
		if (boneName != null)
		{
			transform = this.GetBone(boneName);
		}
		if (transform == null)
		{
			if (def_base_object == null)
			{
				return;
			}
			transform = def_base_object.transform;
		}
		if (!string.IsNullOrEmpty(propName))
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform transform2 = transform.GetChild(i);
				if (propName.EndsWith(transform2.name))
				{
					transform2 = transform.FindChild(transform2.name);
					UnityEngine.Object.Destroy(transform2.gameObject);
					break;
				}
			}
		}
		this.propResource = false;
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000CBA84 File Offset: 0x000C9C84
	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
		element.SetTransformParent(this.GetBone(target));
		element.transform.localPosition = Vector3.zero;
		element.tform.localPosition = Vector3.zero;
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000CBAC0 File Offset: 0x000C9CC0
	public Transform GetBoneRecursive(Transform trans, string boneName)
	{
		if (trans.name == boneName)
		{
			return trans;
		}
		int childCount = trans.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = trans.GetChild(i);
			Transform boneRecursive = this.GetBoneRecursive(child, boneName);
			if (boneRecursive != null && boneRecursive.name == boneName)
			{
				return boneRecursive;
			}
		}
		return null;
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000CBB2C File Offset: 0x000C9D2C
	public Transform GetBone(string boneName)
	{
		bool flag;
		GameObject skeleton = this.skeletons.GetSkeleton(this.GetSkeletonName(this.currentAnimGroup.skeletonName), false, out flag);
		return this.GetBoneRecursive(skeleton.transform, boneName);
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000CBB68 File Offset: 0x000C9D68
	public virtual void UpdateMaterialOrTexture(string material)
	{
		TFUtils.Assert(false, "UpdateMaterial(string) is not implemented and should not be called.");
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x000CBB78 File Offset: 0x000C9D78
	public virtual void SetMaskPercentage(float pct)
	{
		pct = TFMath.ClampF(pct, 0f, 1f);
		SkinnedMeshRenderer[] componentsInChildren = this.rootGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			for (int j = 0; j <= skinnedMeshRenderer.materials.Length - 1; j++)
			{
				pct = TFMath.ClampF(pct, 0f, 1f);
				if (this.assignedShader == null)
				{
					Shader shader = skinnedMeshRenderer.materials[j].shader;
					Shader shader2 = Paperdoll.maskShader;
					this.assignedShader = shader;
					if (CommonUtils.CheckReloadShader() && shader == Paperdoll.altShader)
					{
						shader2 = Paperdoll.altMaskShader;
					}
					skinnedMeshRenderer.materials[j].shader = shader2;
				}
				else if (pct == 0f)
				{
					if (this.assignedShader != null)
					{
						skinnedMeshRenderer.materials[j].shader = this.assignedShader;
						this.assignedShader = null;
					}
					return;
				}
				Vector2[] uv = skinnedMeshRenderer.sharedMesh.uv;
				float value = pct;
				skinnedMeshRenderer.materials[j].SetFloat("_Mask", value);
			}
		}
	}

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x060020E8 RID: 8424 RVA: 0x000CBCB4 File Offset: 0x000C9EB4
	// (set) Token: 0x060020E9 RID: 8425 RVA: 0x000CBCBC File Offset: 0x000C9EBC
	public virtual string HitMeshName { get; set; }

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x060020EA RID: 8426 RVA: 0x000CBCC8 File Offset: 0x000C9EC8
	// (set) Token: 0x060020EB RID: 8427 RVA: 0x000CBCD0 File Offset: 0x000C9ED0
	public virtual bool SeparateTap { get; set; }

	// Token: 0x060020EC RID: 8428 RVA: 0x000CBCDC File Offset: 0x000C9EDC
	public static void HorizontalFlipWithDirectionAndCamera(IDisplayController dc, Vector3 direction, Camera camera)
	{
		if (camera == null)
		{
			return;
		}
		Vector3 scale = dc.Scale;
		float num = Vector3.Dot(direction, camera.transform.right);
		float x = scale.x;
		float y = scale.y;
		float z = scale.z;
		if (x * -num < 0f)
		{
			dc.Scale = new Vector3(-x, y, z);
		}
	}

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x060020ED RID: 8429 RVA: 0x000CBD48 File Offset: 0x000C9F48
	public Transform Transform
	{
		get
		{
			return this.tform;
		}
	}

	// Token: 0x060020EE RID: 8430 RVA: 0x000CBD50 File Offset: 0x000C9F50
	public bool ShouldFlip(Vector3 direction, Camera camera)
	{
		if (camera == null)
		{
			return false;
		}
		Vector3 scale = this.Scale;
		float num = Vector3.Dot(direction, camera.transform.right);
		return this.flippable && scale.x * -num < 0f;
	}

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x060020EF RID: 8431 RVA: 0x000CBDA8 File Offset: 0x000C9FA8
	// (set) Token: 0x060020F0 RID: 8432 RVA: 0x000CBDDC File Offset: 0x000C9FDC
	public Vector3 Position
	{
		get
		{
			return (!(this.tform == null)) ? this.tform.position : Vector3.zero;
		}
		set
		{
			if (this.flippable && this.skeletons != null && this.currentAnimGroup != null)
			{
				bool flag;
				GameObject skeleton = this.skeletons.GetSkeleton(this.GetSkeletonName(this.currentAnimGroup.skeletonName), false, out flag);
				if (this.ShouldFlip(value - this.Position, Camera.main))
				{
					skeleton.transform.localScale = this.inverseScale;
					skeleton.transform.localRotation = this.inverseRotation;
				}
				else
				{
					skeleton.transform.localScale = this.normalScale;
					skeleton.transform.localRotation = this.normalRotation;
				}
			}
			this.tform.position = value;
		}
	}

	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000CBE9C File Offset: 0x000CA09C
	// (set) Token: 0x060020F2 RID: 8434 RVA: 0x000CBF04 File Offset: 0x000CA104
	public Vector3 Scale
	{
		get
		{
			Vector3 vector = this.displayScale;
			return (!(this.tform == null)) ? Vector3.Scale(this.tform.localScale, new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z)) : vector;
		}
		set
		{
			this.tform.localScale = Vector3.Scale(value, this.displayScale);
		}
	}

	// Token: 0x170004C6 RID: 1222
	// (get) Token: 0x060020F3 RID: 8435 RVA: 0x000CBF20 File Offset: 0x000CA120
	// (set) Token: 0x060020F4 RID: 8436 RVA: 0x000CBF28 File Offset: 0x000CA128
	public Vector3 BillboardScaling
	{
		get
		{
			return this.Scale;
		}
		set
		{
			this.Scale = value;
		}
	}

	// Token: 0x170004C7 RID: 1223
	// (get) Token: 0x060020F5 RID: 8437 RVA: 0x000CBF34 File Offset: 0x000CA134
	public Vector3 Forward
	{
		get
		{
			return this.tform.forward;
		}
	}

	// Token: 0x170004C8 RID: 1224
	// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000CBF44 File Offset: 0x000CA144
	public Vector3 Up
	{
		get
		{
			return this.tform.up;
		}
	}

	// Token: 0x170004C9 RID: 1225
	// (get) Token: 0x060020F7 RID: 8439 RVA: 0x000CBF54 File Offset: 0x000CA154
	public float Width
	{
		get
		{
			return this.quadHitObject.Width;
		}
	}

	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000CBF64 File Offset: 0x000CA164
	public float Height
	{
		get
		{
			return this.quadHitObject.Height;
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x060020F9 RID: 8441 RVA: 0x000CBF74 File Offset: 0x000CA174
	// (set) Token: 0x060020FA RID: 8442 RVA: 0x000CBF7C File Offset: 0x000CA17C
	public bool isPerspectiveInArt { get; set; }

	// Token: 0x060020FB RID: 8443 RVA: 0x000CBF88 File Offset: 0x000CA188
	private void ApplyLevelOfDetail()
	{
		int num = this.levelOfDetail;
		if (num != 0)
		{
			if (num != 1)
			{
				TFUtils.Assert(false, string.Format("Paperdoll.ApplyLevelOfDetail(), unsupported value for level of detail = {0}", this.levelOfDetail));
			}
			else
			{
				this.animationController.StopAnimations();
			}
		}
		else
		{
			this.animationController.PlayAnimation(this.currentAnimationState);
		}
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x060020FC RID: 8444 RVA: 0x000CBFF4 File Offset: 0x000CA1F4
	// (set) Token: 0x060020FD RID: 8445 RVA: 0x000CBFFC File Offset: 0x000CA1FC
	public int LevelOfDetail
	{
		get
		{
			return this.levelOfDetail;
		}
		set
		{
			value = ((value >= 2) ? 1 : value);
			if (this.levelOfDetail != value)
			{
				this.levelOfDetail = value;
				this.ApplyLevelOfDetail();
			}
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x060020FE RID: 8446 RVA: 0x000CC028 File Offset: 0x000CA228
	public int NumberOfLevelsOfDetail
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x060020FF RID: 8447 RVA: 0x000CC02C File Offset: 0x000CA22C
	public int MaxLevelOfDetail
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000CC030 File Offset: 0x000CA230
	public void UpdateLOD(Camera sceneCamera)
	{
		if (this.LevelOfDetail != 1 && sceneCamera.orthographicSize >= 230f)
		{
			this.LevelOfDetail = 1;
		}
		else if (this.LevelOfDetail != 0 && sceneCamera.orthographicSize < 230f)
		{
			this.LevelOfDetail = 0;
		}
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000CC088 File Offset: 0x000CA288
	protected void SetupAnimationEvents(Animation unityAnimation)
	{
		string text = this.currentAnimGroup.animModel.AnimationEventsKey(this.currentAnimationState);
		if (text != null)
		{
			this.animationEventManager.Clear();
			AnimationEventData animationEventData = this.animationEventManager.FindAnimationEventData(text);
			animationEventData.SetupAnimationEvents(this.rootGameObject, unityAnimation, unityAnimation[this.currentAnimationState].clip, this.animationEventManager);
		}
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x000CC0F0 File Offset: 0x000CA2F0
	public void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		if (sceneCamera != null)
		{
			this.UpdateLOD(sceneCamera);
		}
		this.animationEventManager.UpdateWithParticleSystemManager(psm);
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x000CC114 File Offset: 0x000CA314
	public void Destroy()
	{
		UnityGameResources.Destroy(this.rootGameObject);
		this.animationGroupManager.CleanseAnimations(this.skeletons);
	}

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06002104 RID: 8452 RVA: 0x000CC134 File Offset: 0x000CA334
	// (set) Token: 0x06002105 RID: 8453 RVA: 0x000CC13C File Offset: 0x000CA33C
	public DisplayControllerFlags Flags
	{
		get
		{
			return this.flags;
		}
		set
		{
			this.flags = value;
		}
	}

	// Token: 0x04001401 RID: 5121
	public const int NUM_LODS = 2;

	// Token: 0x04001402 RID: 5122
	public const int MAX_LOD = 1;

	// Token: 0x04001403 RID: 5123
	public const int LOD_1_ORTHOGRAPHIC_SIZE = 230;

	// Token: 0x04001404 RID: 5124
	private static string[] PaperdollTypes = new string[]
	{
		"Materials/lod/character/",
		"Art/FWS_ModelAtlasCreator_Generated/Buildings/",
		"Materials/lod/character/"
	};

	// Token: 0x04001405 RID: 5125
	private Paperdoll.PaperdollType paperDollType = Paperdoll.PaperdollType.Other;

	// Token: 0x04001406 RID: 5126
	public string currentMaterialName;

	// Token: 0x04001407 RID: 5127
	private static Shader maskShader = Shader.Find("Unlit/TransparentMask");

	// Token: 0x04001408 RID: 5128
	private static Shader altMaskShader = Shader.Find("Custom/RGBAlphaOverlay_Mask");

	// Token: 0x04001409 RID: 5129
	private static Shader altShader = Shader.Find("Custom/RGBAlphaOverlay");

	// Token: 0x0400140A RID: 5130
	public PaperdollSkin dollSkin;

	// Token: 0x0400140B RID: 5131
	protected Transform tform;

	// Token: 0x0400140C RID: 5132
	private ULAnimController animationController;

	// Token: 0x0400140D RID: 5133
	private AnimationGroupManager animationGroupManager;

	// Token: 0x0400140E RID: 5134
	private AnimationEventManager animationEventManager;

	// Token: 0x0400140F RID: 5135
	private SkeletonCollection skeletons;

	// Token: 0x04001410 RID: 5136
	private string currentAnimationState;

	// Token: 0x04001411 RID: 5137
	private AnimationGroupManager.AnimGroup currentAnimGroup;

	// Token: 0x04001412 RID: 5138
	private GameObject rootGameObject;

	// Token: 0x04001413 RID: 5139
	private bool propResource;

	// Token: 0x04001414 RID: 5140
	private QuadHitObject quadHitObject;

	// Token: 0x04001415 RID: 5141
	private string defaultDisplayState = "default";

	// Token: 0x04001416 RID: 5142
	private Vector3 displayScale = Vector3.one;

	// Token: 0x04001417 RID: 5143
	private bool flippable = true;

	// Token: 0x04001418 RID: 5144
	private bool displayVisible = true;

	// Token: 0x04001419 RID: 5145
	private Color displayColor = new Color(1f, 1f, 1f, 1f);

	// Token: 0x0400141A RID: 5146
	private float displayAlpha = 1f;

	// Token: 0x0400141B RID: 5147
	private int levelOfDetail;

	// Token: 0x0400141C RID: 5148
	private Shader assignedShader;

	// Token: 0x0400141D RID: 5149
	private readonly Vector3 inverseScale = new Vector3(-1f, -1f, -1f);

	// Token: 0x0400141E RID: 5150
	private readonly Quaternion inverseRotation = Quaternion.Euler(0f, 180f, 180f);

	// Token: 0x0400141F RID: 5151
	private readonly Vector3 normalScale = new Vector3(1f, 1f, 1f);

	// Token: 0x04001420 RID: 5152
	private readonly Quaternion normalRotation = Quaternion.Euler(0f, 0f, 0f);

	// Token: 0x04001421 RID: 5153
	protected DisplayControllerFlags flags = DisplayControllerFlags.NEED_UPDATE;

	// Token: 0x02000426 RID: 1062
	public enum PaperdollType
	{
		// Token: 0x04001426 RID: 5158
		Character,
		// Token: 0x04001427 RID: 5159
		Building,
		// Token: 0x04001428 RID: 5160
		Other
	}
}
