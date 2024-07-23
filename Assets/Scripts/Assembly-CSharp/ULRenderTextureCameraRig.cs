using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class ULRenderTextureCameraRig
{
	// Token: 0x06002460 RID: 9312 RVA: 0x000DE8A0 File Offset: 0x000DCAA0
	public ULRenderTextureCameraRig()
	{
		this.rig = new GameObject("ULRenderTextureCameraRig");
		this.rig.AddComponent<Camera>();
		this.camera = this.rig.GetComponent<Camera>();
		this.camera.enabled = false;
		this.camera.targetTexture = null;
		this.camera.cullingMask = 0;
		this.camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		this.camera.clearFlags = CameraClearFlags.Color;
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x000DE934 File Offset: 0x000DCB34
	public ULRenderTextureCameraRig(int layer) : this()
	{
		this.Layer = layer;
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06002462 RID: 9314 RVA: 0x000DE944 File Offset: 0x000DCB44
	// (set) Token: 0x06002463 RID: 9315 RVA: 0x000DE94C File Offset: 0x000DCB4C
	public int Layer
	{
		get
		{
			return this.layer;
		}
		set
		{
			Camera.main.cullingMask |= 1 << this.layer;
			this.camera.cullingMask &= ~(1 << this.layer);
			this.layer = value;
			Camera.main.cullingMask &= ~(1 << this.layer);
			this.camera.cullingMask |= 1 << this.layer;
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x06002464 RID: 9316 RVA: 0x000DE9E0 File Offset: 0x000DCBE0
	public Camera RigCamera
	{
		get
		{
			return this.camera;
		}
	}

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x06002465 RID: 9317 RVA: 0x000DE9E8 File Offset: 0x000DCBE8
	public GameObject RigGameObject
	{
		get
		{
			return this.rig;
		}
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x000DE9F0 File Offset: 0x000DCBF0
	public static void SetRenderLayer(GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		int childCount = gameObject.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			ULRenderTextureCameraRig.SetRenderLayer(gameObject.transform.GetChild(i).gameObject, layer);
		}
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x000DEA3C File Offset: 0x000DCC3C
	public void RenderSubjectToTexture(GameObject subject, ULRenderTexture renderTexture, ULRenderTextureCameraRig.RelativeCamDelegate camDelegate)
	{
		int num = subject.layer;
		ULRenderTextureCameraRig.SetRenderLayer(subject, this.layer);
		this.camera.targetTexture = renderTexture.RTexture;
		if (camDelegate != null)
		{
			camDelegate(subject, this.camera);
		}
		this.camera.Render();
		ULRenderTextureCameraRig.SetRenderLayer(subject, num);
	}

	// Token: 0x04001654 RID: 5716
	private GameObject rig;

	// Token: 0x04001655 RID: 5717
	private Camera camera;

	// Token: 0x04001656 RID: 5718
	private int layer;

	// Token: 0x020004C4 RID: 1220
	// (Invoke) Token: 0x06002573 RID: 9587
	public delegate void RelativeCamDelegate(GameObject subject, Camera cam);
}
