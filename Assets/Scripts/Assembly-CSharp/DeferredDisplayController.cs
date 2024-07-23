using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class DeferredDisplayController : IDisplayController
{
	// Token: 0x06001F34 RID: 7988 RVA: 0x000C006C File Offset: 0x000BE26C
	public DeferredDisplayController(IDisplayController source, DisplayControllerManager dcm)
	{
		this.dcm = dcm;
		this.source = source;
		this.scale = source.Scale;
		this.position = source.Position;
		this.alpha = source.Alpha;
		this.color = source.Color;
		this.flags = source.Flags;
		this.levelOfDetail = source.LevelOfDetail;
		this.billboardScale = source.BillboardScaling;
		this.defaultDisplayState = source.DefaultDisplayState;
		this.perspectiveInArt = source.isPerspectiveInArt;
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x000C00FC File Offset: 0x000BE2FC
	public IDisplayController Clone(DisplayControllerManager dcm)
	{
		throw new NotImplementedException("You should not be cloning a deferred display controller!");
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x000C0108 File Offset: 0x000BE308
	public IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		throw new NotImplementedException("You should not be cloning a deferred display controller!");
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x000C0114 File Offset: 0x000BE314
	public IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		throw new NotImplementedException("You should not be cloning a deferred display controller!");
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06001F38 RID: 7992 RVA: 0x000C0120 File Offset: 0x000BE320
	// (set) Token: 0x06001F39 RID: 7993 RVA: 0x000C0144 File Offset: 0x000BE344
	public int LevelOfDetail
	{
		get
		{
			return (this.idc != null) ? this.idc.LevelOfDetail : this.levelOfDetail;
		}
		set
		{
			this.levelOfDetail = value;
			if (this.idc != null)
			{
				this.idc.LevelOfDetail = value;
			}
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06001F3A RID: 7994 RVA: 0x000C0164 File Offset: 0x000BE364
	// (set) Token: 0x06001F3B RID: 7995 RVA: 0x000C0184 File Offset: 0x000BE384
	public bool Visible
	{
		get
		{
			return this.idc != null && this.idc.Visible;
		}
		set
		{
			if (value)
			{
				this.InternalDisplayController.Visible = value;
			}
			else if (this.idc != null)
			{
				this.Destroy();
			}
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06001F3C RID: 7996 RVA: 0x000C01BC File Offset: 0x000BE3BC
	public bool IsDestroyed
	{
		get
		{
			return this.idc == null || this.idc.IsDestroyed;
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06001F3D RID: 7997 RVA: 0x000C01D8 File Offset: 0x000BE3D8
	// (set) Token: 0x06001F3E RID: 7998 RVA: 0x000C01FC File Offset: 0x000BE3FC
	public float Alpha
	{
		get
		{
			return (this.idc != null) ? this.idc.Alpha : this.alpha;
		}
		set
		{
			this.alpha = value;
			if (this.idc != null)
			{
				this.idc.Alpha = value;
			}
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06001F3F RID: 7999 RVA: 0x000C021C File Offset: 0x000BE41C
	// (set) Token: 0x06001F40 RID: 8000 RVA: 0x000C0240 File Offset: 0x000BE440
	public Color Color
	{
		get
		{
			return (this.idc != null) ? this.idc.Color : this.color;
		}
		set
		{
			this.color = value;
			if (this.idc != null)
			{
				this.idc.Color = value;
			}
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06001F41 RID: 8001 RVA: 0x000C0260 File Offset: 0x000BE460
	// (set) Token: 0x06001F42 RID: 8002 RVA: 0x000C0284 File Offset: 0x000BE484
	public string DefaultDisplayState
	{
		get
		{
			return (this.idc != null) ? this.idc.DefaultDisplayState : this.defaultDisplayState;
		}
		set
		{
			this.defaultDisplayState = value;
			if (this.idc != null)
			{
				this.idc.DefaultDisplayState = value;
			}
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06001F43 RID: 8003 RVA: 0x000C02A4 File Offset: 0x000BE4A4
	// (set) Token: 0x06001F44 RID: 8004 RVA: 0x000C02C8 File Offset: 0x000BE4C8
	public Vector3 Position
	{
		get
		{
			return (this.idc != null) ? this.idc.Position : this.position;
		}
		set
		{
			this.position = value;
			if (this.idc != null)
			{
				this.idc.Position = value;
			}
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06001F45 RID: 8005 RVA: 0x000C02E8 File Offset: 0x000BE4E8
	// (set) Token: 0x06001F46 RID: 8006 RVA: 0x000C030C File Offset: 0x000BE50C
	public Vector3 BillboardScaling
	{
		get
		{
			return (this.idc != null) ? this.idc.BillboardScaling : this.billboardScale;
		}
		set
		{
			this.billboardScale = value;
			if (this.idc != null)
			{
				this.idc.BillboardScaling = value;
			}
		}
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06001F47 RID: 8007 RVA: 0x000C032C File Offset: 0x000BE52C
	// (set) Token: 0x06001F48 RID: 8008 RVA: 0x000C0350 File Offset: 0x000BE550
	public Vector3 Scale
	{
		get
		{
			return (this.idc != null) ? this.idc.Scale : this.scale;
		}
		set
		{
			this.scale = value;
			if (this.idc != null)
			{
				this.idc.Scale = value;
			}
		}
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06001F49 RID: 8009 RVA: 0x000C0370 File Offset: 0x000BE570
	// (set) Token: 0x06001F4A RID: 8010 RVA: 0x000C0394 File Offset: 0x000BE594
	public DisplayControllerFlags Flags
	{
		get
		{
			return (this.idc != null) ? this.idc.Flags : this.flags;
		}
		set
		{
			this.flags = value;
			if (this.idc != null)
			{
				this.idc.Flags = value;
			}
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06001F4B RID: 8011 RVA: 0x000C03B4 File Offset: 0x000BE5B4
	// (set) Token: 0x06001F4C RID: 8012 RVA: 0x000C03D8 File Offset: 0x000BE5D8
	public bool isPerspectiveInArt
	{
		get
		{
			return (this.idc != null) ? this.idc.isPerspectiveInArt : this.perspectiveInArt;
		}
		set
		{
			this.perspectiveInArt = value;
			if (this.idc != null)
			{
				this.idc.isPerspectiveInArt = value;
			}
		}
	}

	// Token: 0x1700043A RID: 1082
	// (get) Token: 0x06001F4D RID: 8013 RVA: 0x000C03F8 File Offset: 0x000BE5F8
	public Vector3 Forward
	{
		get
		{
			return this.InternalDisplayController.Forward;
		}
	}

	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x06001F4E RID: 8014 RVA: 0x000C0408 File Offset: 0x000BE608
	public Vector3 Up
	{
		get
		{
			return this.InternalDisplayController.Up;
		}
	}

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x06001F4F RID: 8015 RVA: 0x000C0418 File Offset: 0x000BE618
	public float Width
	{
		get
		{
			return this.InternalDisplayController.Width;
		}
	}

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000C0428 File Offset: 0x000BE628
	public float Height
	{
		get
		{
			return this.InternalDisplayController.Height;
		}
	}

	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06001F51 RID: 8017 RVA: 0x000C0438 File Offset: 0x000BE638
	public Transform Transform
	{
		get
		{
			return this.InternalDisplayController.Transform;
		}
	}

	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06001F52 RID: 8018 RVA: 0x000C0448 File Offset: 0x000BE648
	public int NumberOfLevelsOfDetail
	{
		get
		{
			return this.InternalDisplayController.NumberOfLevelsOfDetail;
		}
	}

	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000C0458 File Offset: 0x000BE658
	public int MaxLevelOfDetail
	{
		get
		{
			return this.InternalDisplayController.MaxLevelOfDetail;
		}
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000C0468 File Offset: 0x000BE668
	public bool isVisible
	{
		get
		{
			return this.idc != null && this.idc.isVisible;
		}
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x000C0488 File Offset: 0x000BE688
	public string GetDisplayState()
	{
		return (this.idc != null) ? this.idc.GetDisplayState() : null;
	}

	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06001F56 RID: 8022 RVA: 0x000C04A8 File Offset: 0x000BE6A8
	public string MaterialName
	{
		get
		{
			return (this.idc != null) ? this.idc.MaterialName : null;
		}
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x000C04C8 File Offset: 0x000BE6C8
	public void AddDisplayState(Dictionary<string, object> dict)
	{
		this.InternalDisplayController.AddDisplayState(dict);
	}

	// Token: 0x06001F58 RID: 8024 RVA: 0x000C04D8 File Offset: 0x000BE6D8
	public bool Intersects(Ray ray)
	{
		return this.InternalDisplayController.Intersects(ray);
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06001F59 RID: 8025 RVA: 0x000C04E8 File Offset: 0x000BE6E8
	public QuadHitObject HitObject
	{
		get
		{
			return this.InternalDisplayController.HitObject;
		}
	}

	// Token: 0x06001F5A RID: 8026 RVA: 0x000C04F8 File Offset: 0x000BE6F8
	public void ChangeMesh(string state, string meshName)
	{
		if (state == null)
		{
			this.Destroy();
		}
		else
		{
			this.InternalDisplayController.ChangeMesh(state, meshName);
		}
	}

	// Token: 0x06001F5B RID: 8027 RVA: 0x000C0518 File Offset: 0x000BE718
	public void DisplayState(string state)
	{
		if (state == null)
		{
			this.Destroy();
		}
		else
		{
			this.InternalDisplayController.DisplayState(state);
		}
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x000C0538 File Offset: 0x000BE738
	public void UpdateMaterialOrTexture(string material)
	{
		if (material == null)
		{
			this.Destroy();
		}
		else
		{
			this.InternalDisplayController.UpdateMaterialOrTexture(material);
		}
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x000C0558 File Offset: 0x000BE758
	public void SetMaskPercentage(float pct)
	{
		this.InternalDisplayController.SetMaskPercentage(pct);
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x000C0568 File Offset: 0x000BE768
	public void Billboard(BillboardDelegate billboard)
	{
		this.billboardDelegate = billboard;
		if (this.idc != null)
		{
			this.idc.Billboard(this.billboardDelegate);
		}
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x000C0590 File Offset: 0x000BE790
	public void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		if (this.idc != null)
		{
			this.idc.OnUpdate(sceneCamera, psm);
		}
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x000C05AC File Offset: 0x000BE7AC
	public void Destroy()
	{
		if (this.idc != null)
		{
			this.idc.Destroy();
			this.idc = null;
		}
	}

	// Token: 0x06001F61 RID: 8033 RVA: 0x000C05CC File Offset: 0x000BE7CC
	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
		this.InternalDisplayController.AttachGUIElementToTarget(element, target);
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06001F62 RID: 8034 RVA: 0x000C05DC File Offset: 0x000BE7DC
	private IDisplayController InternalDisplayController
	{
		get
		{
			if (this.idc != null)
			{
				return this.idc;
			}
			this.idc = this.source.CloneAndSetVisible(this.dcm);
			if (this.billboardDelegate != null)
			{
				this.idc.Billboard(this.billboardDelegate);
			}
			this.idc.Position = this.position;
			if (this.color != this.source.Color)
			{
				this.idc.Color = this.color;
			}
			if (this.alpha != this.source.Alpha)
			{
				this.idc.Alpha = this.alpha;
			}
			if (this.scale != this.source.Scale)
			{
				this.idc.Scale = this.scale;
			}
			if (this.billboardScale != this.source.BillboardScaling)
			{
				this.idc.BillboardScaling = this.BillboardScaling;
			}
			if (this.levelOfDetail != this.source.LevelOfDetail)
			{
				this.idc.LevelOfDetail = this.levelOfDetail;
			}
			if (this.flags != this.source.Flags)
			{
				this.idc.Flags = this.flags;
			}
			if (this.defaultDisplayState != this.source.DefaultDisplayState)
			{
				this.idc.DefaultDisplayState = this.defaultDisplayState;
			}
			if (this.perspectiveInArt != this.source.isPerspectiveInArt)
			{
				this.idc.isPerspectiveInArt = this.perspectiveInArt;
			}
			return this.idc;
		}
	}

	// Token: 0x04001368 RID: 4968
	private float alpha;

	// Token: 0x04001369 RID: 4969
	private Color color;

	// Token: 0x0400136A RID: 4970
	private Vector3 position;

	// Token: 0x0400136B RID: 4971
	private Vector3 scale;

	// Token: 0x0400136C RID: 4972
	private Vector3 billboardScale;

	// Token: 0x0400136D RID: 4973
	private int levelOfDetail;

	// Token: 0x0400136E RID: 4974
	private string defaultDisplayState;

	// Token: 0x0400136F RID: 4975
	private DisplayControllerFlags flags;

	// Token: 0x04001370 RID: 4976
	private bool perspectiveInArt;

	// Token: 0x04001371 RID: 4977
	private BillboardDelegate billboardDelegate;

	// Token: 0x04001372 RID: 4978
	private IDisplayController idc;

	// Token: 0x04001373 RID: 4979
	private IDisplayController source;

	// Token: 0x04001374 RID: 4980
	private DisplayControllerManager dcm;
}
