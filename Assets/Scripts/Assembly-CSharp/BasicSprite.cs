using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x020003F4 RID: 1012
public class BasicSprite : IDisplayController
{
	// Token: 0x06001EC0 RID: 7872 RVA: 0x000BD954 File Offset: 0x000BBB54
	public BasicSprite(string material, string texture, Vector2 center, float width, float height) : this(material, texture, center, width, height, new QuadHitObject(center, width, height))
	{
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x000BD978 File Offset: 0x000BBB78
	public BasicSprite(string material, string texture, Vector2 center, float width, float height, QuadHitObject hitObject)
	{
		this.ymin = 1f;
		this.overallScale = Vector3.one;
		this.defaultDisplayState = "default";
		base..ctor();
		this.texture = texture;
		if (this.texture != null)
		{
			this.material = YGTextureLibrary.GetAtlasCoords(texture).atlas.name;
		}
		else
		{
			this.material = material;
		}
		this.center = center;
		this.width = width;
		this.height = height;
		this.quadHitObject = hitObject;
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000BDA00 File Offset: 0x000BBC00
	public BasicSprite(BasicSprite prototype)
	{
		this.ymin = 1f;
		this.overallScale = Vector3.one;
		this.defaultDisplayState = "default";
		base..ctor();
		this.material = prototype.material;
		this.texture = prototype.texture;
		this.center = prototype.center;
		this.width = prototype.width;
		this.height = prototype.height;
		this.quadHitObject = prototype.quadHitObject;
		this.isPerspectiveInArt = prototype.isPerspectiveInArt;
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x000BDAD4 File Offset: 0x000BBCD4
	public void Billboard(BillboardDelegate billboard)
	{
		billboard(this.tform, this);
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x000BDAE4 File Offset: 0x000BBCE4
	public Transform Transform
	{
		get
		{
			return this.tform;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000BDAEC File Offset: 0x000BBCEC
	// (set) Token: 0x06001EC7 RID: 7879 RVA: 0x000BDB20 File Offset: 0x000BBD20
	public virtual Vector3 Position
	{
		get
		{
			return (!(this.tform == null)) ? this.tform.position : Vector3.zero;
		}
		set
		{
			this.tform.position = value;
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x000BDB30 File Offset: 0x000BBD30
	public virtual Vector3 Forward
	{
		get
		{
			return this.tform.forward;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x000BDB40 File Offset: 0x000BBD40
	public virtual Vector3 Up
	{
		get
		{
			return this.tform.up;
		}
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x000BDB50 File Offset: 0x000BBD50
	public virtual void Face(Vector3 direction, Vector3 worldUp)
	{
		this.tform.LookAt(this.tform.position + direction, worldUp);
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06001ECB RID: 7883 RVA: 0x000BDB70 File Offset: 0x000BBD70
	// (set) Token: 0x06001ECC RID: 7884 RVA: 0x000BDBA4 File Offset: 0x000BBDA4
	public virtual Vector3 Scale
	{
		get
		{
			return (!(this.tform == null)) ? this.tform.localScale : Vector3.one;
		}
		set
		{
			this.tform.localScale = value;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06001ECD RID: 7885 RVA: 0x000BDBB4 File Offset: 0x000BBDB4
	// (set) Token: 0x06001ECE RID: 7886 RVA: 0x000BDBBC File Offset: 0x000BBDBC
	public virtual Vector3 BillboardScaling
	{
		get
		{
			return this.overallScale;
		}
		set
		{
			this.overallScale = value;
			this.Resize(this.center, this.width, this.height);
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06001ECF RID: 7887 RVA: 0x000BDBE0 File Offset: 0x000BBDE0
	// (set) Token: 0x06001ED0 RID: 7888 RVA: 0x000BDBF4 File Offset: 0x000BBDF4
	public virtual bool Visible
	{
		get
		{
			return this.gameObject.renderer.enabled;
		}
		set
		{
			this.gameObject.renderer.enabled = value;
			this.gameObject.SetActiveRecursively(value);
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000BDC14 File Offset: 0x000BBE14
	public virtual bool isVisible
	{
		get
		{
			return this.gameObject.renderer.isVisible;
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x000BDC28 File Offset: 0x000BBE28
	// (set) Token: 0x06001ED3 RID: 7891 RVA: 0x000BDC30 File Offset: 0x000BBE30
	public virtual int LevelOfDetail
	{
		get
		{
			return this.levelOfDetail;
		}
		set
		{
			this.levelOfDetail = ((value >= this.NumberOfLevelsOfDetail) ? this.MaxLevelOfDetail : value);
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x000BDC50 File Offset: 0x000BBE50
	public virtual int NumberOfLevelsOfDetail
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x000BDC54 File Offset: 0x000BBE54
	public virtual int MaxLevelOfDetail
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x000BDC58 File Offset: 0x000BBE58
	public virtual string MaterialName
	{
		get
		{
			return (this.texture == null) ? this.material : this.texture;
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x000BDC78 File Offset: 0x000BBE78
	// (set) Token: 0x06001ED8 RID: 7896 RVA: 0x000BDC80 File Offset: 0x000BBE80
	public virtual string HitMeshName { get; set; }

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000BDC8C File Offset: 0x000BBE8C
	// (set) Token: 0x06001EDA RID: 7898 RVA: 0x000BDC94 File Offset: 0x000BBE94
	public virtual bool SeparateTap { get; set; }

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06001EDB RID: 7899 RVA: 0x000BDCA0 File Offset: 0x000BBEA0
	// (set) Token: 0x06001EDC RID: 7900 RVA: 0x000BDD14 File Offset: 0x000BBF14
	public virtual float Alpha
	{
		get
		{
			if (this.gameObject != null && this.gameObject.renderer != null && this.gameObject.renderer.material != null)
			{
				return this.gameObject.renderer.material.color.a;
			}
			return 1f;
		}
		set
		{
			Color color = this.gameObject.renderer.material.color;
			this.gameObject.renderer.material.color = new Color(color.r, color.g, color.b, value);
		}
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06001EDD RID: 7901 RVA: 0x000BDD68 File Offset: 0x000BBF68
	// (set) Token: 0x06001EDE RID: 7902 RVA: 0x000BDDD4 File Offset: 0x000BBFD4
	public virtual Color Color
	{
		get
		{
			if (this.gameObject != null && this.gameObject.renderer != null && this.gameObject.renderer.material != null)
			{
				return this.gameObject.renderer.material.color;
			}
			return Color.white;
		}
		set
		{
			this.gameObject.renderer.material.color = value;
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000BDDEC File Offset: 0x000BBFEC
	public QuadHitObject HitObject
	{
		get
		{
			return this.quadHitObject;
		}
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x000BDDF4 File Offset: 0x000BBFF4
	public virtual bool Intersects(Ray ray)
	{
		MeshCollider component = this.gameObject.GetComponent<MeshCollider>();
		if (component != null)
		{
			RaycastHit raycastHit;
			return component.Raycast(ray, out raycastHit, 5000f);
		}
		return this.quadHitObject.Intersects(this.tform, ray, Vector2.zero);
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x000BDE48 File Offset: 0x000BC048
	public virtual void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x000BDE4C File Offset: 0x000BC04C
	public virtual void AddDisplayState(Dictionary<string, object> dict)
	{
		TFUtils.Assert(false, "BasicSprite.AddDisplayState(Dictionary) is not implemented and should not be called.");
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x000BDE5C File Offset: 0x000BC05C
	public virtual string GetDisplayState()
	{
		throw new InvalidOperationException("Cannot call GetDisplayState() in BasicSprite");
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x000BDE68 File Offset: 0x000BC068
	public virtual IDisplayController Clone(DisplayControllerManager dcm)
	{
		BasicSprite basicSprite = new BasicSprite(this);
		basicSprite.Initialize();
		return basicSprite;
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x000BDE84 File Offset: 0x000BC084
	public virtual IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		this.HitMeshName = hitMeshName;
		this.SeparateTap = separateTap;
		BasicSprite basicSprite = new BasicSprite(this);
		basicSprite.Initialize();
		return basicSprite;
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000BDEB0 File Offset: 0x000BC0B0
	public virtual IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		IDisplayController displayController = this.Clone(dcm);
		displayController.Visible = true;
		return displayController;
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000BDED0 File Offset: 0x000BC0D0
	// (set) Token: 0x06001EE8 RID: 7912 RVA: 0x000BDED8 File Offset: 0x000BC0D8
	public virtual string DefaultDisplayState
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

	// Token: 0x06001EE9 RID: 7913 RVA: 0x000BDEE4 File Offset: 0x000BC0E4
	public virtual void ChangeMesh(string state, string HitMeshName)
	{
		TFUtils.Assert(false, "BasicSprite.ChangeMesh(string, string) is not implemented and should not be called.");
	}

	// Token: 0x06001EEA RID: 7914 RVA: 0x000BDEF4 File Offset: 0x000BC0F4
	public virtual void DisplayState(string state)
	{
		TFUtils.Assert(false, "BasicSprite.DisplayState(string) is not implemented and should not be called.");
	}

	// Token: 0x06001EEB RID: 7915 RVA: 0x000BDF04 File Offset: 0x000BC104
	public virtual void UpdateMaterialOrTexture(string material)
	{
		if (material == null)
		{
			TFUtils.Assert(false, "Cannot update BasicSprite to use a null material!");
			return;
		}
		int num = material.LastIndexOf('/');
		Material x;
		Vector2[] uv;
		if (num >= 0)
		{
			x = TextureLibrarian.LookUp(material);
			TFUtils.Assert(x != null, "Could not find the material " + material);
			uv = new Vector2[]
			{
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 0f),
				new Vector2(0f, 1f)
			};
			this.texture = null;
			this.material = material;
		}
		else
		{
			AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(material);
			x = TextureLibrarian.LookUp("Materials/lod/" + atlasCoords.atlas.name);
			Rect rect = default(Rect);
			atlasCoords.atlas.GetUVs(atlasCoords.atlasCoords, ref rect);
			uv = new Vector2[]
			{
				new Vector2(rect.xMax, rect.yMin),
				new Vector2(rect.xMax, rect.yMax),
				new Vector2(rect.xMin, rect.yMin),
				new Vector2(rect.xMin, rect.yMax)
			};
			this.texture = material;
			this.material = null;
		}
		this.gameObject.renderer.material = x;
		this.gameObject.GetComponent<MeshFilter>().mesh.uv = uv;
	}

	// Token: 0x06001EEC RID: 7916 RVA: 0x000BE0CC File Offset: 0x000BC2CC
	public virtual void SetMaskPercentage(float pct)
	{
		pct = TFMath.ClampF(pct, 0f, 1f);
		if (this.assignedShader == null)
		{
			Shader shader = this.gameObject.renderer.material.shader;
			Shader shader2;
			if (shader.name.Contains("TwoImageColorOverlay"))
			{
				shader2 = BasicSprite.twoImageMaskShader;
			}
			else
			{
				shader2 = BasicSprite.maskShader;
			}
			this.assignedShader = shader;
			if (CommonUtils.CheckReloadShader() && shader == BasicSprite.altShader)
			{
				shader2 = BasicSprite.altMaskShader;
			}
			this.gameObject.renderer.material.shader = shader2;
		}
		else if (pct == 0f)
		{
			this.gameObject.renderer.material.shader = this.assignedShader;
			this.assignedShader = null;
			return;
		}
		if (this.ymin == 1f)
		{
			Vector2[] uv = this.gameObject.GetComponent<MeshFilter>().mesh.uv;
			this.ymin = uv[0].y;
			this.ymax = uv[1].y;
		}
		float value = (this.ymax - this.ymin) * pct + this.ymin;
		this.gameObject.renderer.material.SetFloat("_Mask", value);
	}

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x06001EED RID: 7917 RVA: 0x000BE224 File Offset: 0x000BC424
	protected GameObject GameObject
	{
		get
		{
			return this.gameObject;
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06001EEE RID: 7918 RVA: 0x000BE22C File Offset: 0x000BC42C
	// (set) Token: 0x06001EEF RID: 7919 RVA: 0x000BE23C File Offset: 0x000BC43C
	public string Name
	{
		get
		{
			return this.gameObject.name;
		}
		set
		{
			this.gameObject.name = value;
		}
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x000BE24C File Offset: 0x000BC44C
	public Material GetMaterial
	{
		get
		{
			return this.gameObject.renderer.material;
		}
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x000BE260 File Offset: 0x000BC460
	public virtual void Destroy()
	{
		UnityGameResources.Destroy(this.gameObject);
		this.gameObject = null;
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x000BE274 File Offset: 0x000BC474
	// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x000BE288 File Offset: 0x000BC488
	protected bool LayerRendering
	{
		get
		{
			return this.gameObject.layer != 22;
		}
		set
		{
			int layer = (!value) ? 22 : LayerMask.NameToLayer("Default");
			ULRenderTextureCameraRig.SetRenderLayer(this.gameObject, layer);
		}
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x000BE2BC File Offset: 0x000BC4BC
	protected GameObject CreateQuadGameObject(string name, Material material, Rect? uvs = null, Mesh hitMesh = null)
	{
		TFUtils.Assert(this.gameObject == null, "Recreating a Basic Sprite - this will cause an untracked game object!");
		this.gameObject = UnityGameResources.CreateEmpty(name);
		this.tform = this.gameObject.transform;
		TFQuad.SetupQuad(this.gameObject, material, this.width, this.height, this.center, uvs, hitMesh);
		this.LayerRendering = true;
		if (hitMesh != null)
		{
			Vector2[] uv = this.gameObject.GetComponent<MeshFilter>().mesh.uv;
			this.ymax = 0f;
			this.ymin = 1f;
			for (int i = 0; i < uv.Length; i++)
			{
				if (uv[i].y > this.ymax)
				{
					this.ymax = uv[i].y;
				}
				if (uv[i].y < this.ymin)
				{
					this.ymin = uv[i].y;
				}
			}
		}
		return this.gameObject;
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x000BE3CC File Offset: 0x000BC5CC
	public virtual void PublicInitialize()
	{
		this.Initialize();
		this.gameObject.renderer.enabled = true;
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x000BE3E8 File Offset: 0x000BC5E8
	protected virtual void Initialize()
	{
		Rect? uvs = null;
		Material material;
		if (this.texture == null)
		{
			material = TextureLibrarian.LookUp(this.material);
		}
		else
		{
			AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(this.texture);
			material = TextureLibrarian.LookUp("Materials/lod/" + atlasCoords.atlas.name);
			Rect value = default(Rect);
			atlasCoords.atlas.GetUVs(atlasCoords.atlasCoords, ref value);
			uvs = new Rect?(value);
		}
		this.CreateQuadGameObject("BasicSprite", material, uvs, null);
		this.gameObject.renderer.enabled = false;
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x000BE488 File Offset: 0x000BC688
	public virtual void Resize(Vector2 center, float width, float height)
	{
		if (string.IsNullOrEmpty(this.HitMeshName) || !(this.HitMeshName.Substring(this.HitMeshName.Length - 10, 10) == "_asset.fbx"))
		{
			TFQuad.SetupQuadMesh(this.gameObject.GetComponent<MeshFilter>().mesh, width * this.overallScale.x, height * this.overallScale.y, center, true, null);
		}
		this.width = width;
		this.height = height;
		this.center = center;
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06001EF8 RID: 7928 RVA: 0x000BE524 File Offset: 0x000BC724
	// (set) Token: 0x06001EF9 RID: 7929 RVA: 0x000BE52C File Offset: 0x000BC72C
	public float Width
	{
		get
		{
			return this.width;
		}
		set
		{
			this.width = value;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06001EFA RID: 7930 RVA: 0x000BE538 File Offset: 0x000BC738
	// (set) Token: 0x06001EFB RID: 7931 RVA: 0x000BE540 File Offset: 0x000BC740
	public float Height
	{
		get
		{
			return this.height;
		}
		set
		{
			this.height = value;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06001EFC RID: 7932 RVA: 0x000BE54C File Offset: 0x000BC74C
	// (set) Token: 0x06001EFD RID: 7933 RVA: 0x000BE554 File Offset: 0x000BC754
	public Vector2 Center
	{
		get
		{
			return this.center;
		}
		set
		{
			this.center = value;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06001EFE RID: 7934 RVA: 0x000BE560 File Offset: 0x000BC760
	public Quaternion Rotation
	{
		get
		{
			return this.tform.rotation;
		}
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x000BE570 File Offset: 0x000BC770
	public void Translate(Vector3 v)
	{
		this.tform.Translate(v);
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x000BE580 File Offset: 0x000BC780
	public void RotateAround(Vector3 point, Vector3 axis, float angle)
	{
		this.tform.RotateAround(point, axis, angle);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x000BE590 File Offset: 0x000BC790
	public void Rotate(Vector3 v)
	{
		this.tform.Rotate(v);
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x000BE5A0 File Offset: 0x000BC7A0
	public void ResetRotation()
	{
		this.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06001F03 RID: 7939 RVA: 0x000BE5B4 File Offset: 0x000BC7B4
	// (set) Token: 0x06001F04 RID: 7940 RVA: 0x000BE5BC File Offset: 0x000BC7BC
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

	// Token: 0x06001F05 RID: 7941 RVA: 0x000BE5C8 File Offset: 0x000BC7C8
	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
		element.SetTransformParent(this.tform);
		element.transform.localPosition = Vector3.zero;
		element.tform.localPosition = Vector3.zero;
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06001F06 RID: 7942 RVA: 0x000BE604 File Offset: 0x000BC804
	public bool IsDestroyed
	{
		get
		{
			return this.gameObject == null;
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06001F07 RID: 7943 RVA: 0x000BE614 File Offset: 0x000BC814
	// (set) Token: 0x06001F08 RID: 7944 RVA: 0x000BE61C File Offset: 0x000BC81C
	public bool isPerspectiveInArt { get; set; }

	// Token: 0x04001319 RID: 4889
	public const int NUM_LODS = 1;

	// Token: 0x0400131A RID: 4890
	public const int MAX_LOD = 0;

	// Token: 0x0400131B RID: 4891
	private static Shader maskShader = Shader.Find("Unlit/TransparentMask");

	// Token: 0x0400131C RID: 4892
	private static Shader twoImageMaskShader = Shader.Find("Custom/TwoImageWithMask");

	// Token: 0x0400131D RID: 4893
	private static Shader altMaskShader = Shader.Find("Custom/RGBAlphaOverlay_Mask");

	// Token: 0x0400131E RID: 4894
	private static Shader altShader = Shader.Find("Custom/RGBAlphaOverlay");

	// Token: 0x0400131F RID: 4895
	private float ymax;

	// Token: 0x04001320 RID: 4896
	private float ymin;

	// Token: 0x04001321 RID: 4897
	protected Vector3 overallScale;

	// Token: 0x04001322 RID: 4898
	protected Transform tform;

	// Token: 0x04001323 RID: 4899
	protected DisplayControllerFlags flags;

	// Token: 0x04001324 RID: 4900
	private string material;

	// Token: 0x04001325 RID: 4901
	private string texture;

	// Token: 0x04001326 RID: 4902
	private Vector2 center;

	// Token: 0x04001327 RID: 4903
	private float width;

	// Token: 0x04001328 RID: 4904
	private float height;

	// Token: 0x04001329 RID: 4905
	private GameObject gameObject;

	// Token: 0x0400132A RID: 4906
	private QuadHitObject quadHitObject;

	// Token: 0x0400132B RID: 4907
	private int levelOfDetail;

	// Token: 0x0400132C RID: 4908
	private string defaultDisplayState;

	// Token: 0x0400132D RID: 4909
	private Shader assignedShader;
}
