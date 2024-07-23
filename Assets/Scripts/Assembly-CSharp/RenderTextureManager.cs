using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public class RenderTextureManager
{
	// Token: 0x0600219B RID: 8603 RVA: 0x000CF144 File Offset: 0x000CD344
	public RenderTextureManager()
	{
		Camera.main.cullingMask &= -6291457;
		this.renderTextureBatch = new ULRenderTextureBatch(21);
		Camera rigCamera = this.renderTextureBatch.CameraRig.RigCamera;
		rigCamera.transform.position = global::RenderTextureManager.RENDERTEXTURE_RIGCAM_POSITION;
		rigCamera.transform.LookAt(global::RenderTextureManager.SUBJECT_POSITION);
	}

	// Token: 0x0600219D RID: 8605 RVA: 0x000CF1EC File Offset: 0x000CD3EC
	public ULRenderTextureBatchEntry AddGameObject(GameObject gameObject, ULRenderTextureCameraRig.RelativeCamDelegate theCamDelegate, string shaderIdentifier)
	{
		ULRenderTextureCameraRig.SetRenderLayer(gameObject, 22);
		ULRenderTexture target = new ULRenderTexture(256, string.Format("RenderTexture{0}", this.entryCount++), shaderIdentifier);
		return this.renderTextureBatch.AddEntry(gameObject, target, theCamDelegate);
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x000CF23C File Offset: 0x000CD43C
	public void RenderEntry(ULRenderTextureBatchEntry entry)
	{
		this.renderTextureBatch.CameraRig.RenderSubjectToTexture(entry.subject, entry.target, entry.camDelegate);
	}

	// Token: 0x040014B3 RID: 5299
	public const int RENDERTEXTURE_CAMERA_LAYER = 21;

	// Token: 0x040014B4 RID: 5300
	public const int RENDERTEXTURE_GAMEOBJECT_STAGING_LAYER = 22;

	// Token: 0x040014B5 RID: 5301
	public const float CAM_DISTANCE_TO_SUBJECT = 7f;

	// Token: 0x040014B6 RID: 5302
	public const int RENDERTEXTURE_SQUARE_SIZE = 256;

	// Token: 0x040014B7 RID: 5303
	public static Vector3 SUBJECT_POSITION = new Vector3(0f, 0f, 0f);

	// Token: 0x040014B8 RID: 5304
	public static Vector3 RENDERTEXTURE_RIGCAM_POSITION = new Vector3(0f, 0f, 7f);

	// Token: 0x040014B9 RID: 5305
	private ULRenderTextureBatch renderTextureBatch;

	// Token: 0x040014BA RID: 5306
	private int entryCount;
}
