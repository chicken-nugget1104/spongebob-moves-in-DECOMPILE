using System;
using UnityEngine;

// Token: 0x02000242 RID: 578
public class ScreenMaskSpawn : SessionActionSpawn
{
	// Token: 0x060012A6 RID: 4774 RVA: 0x0008096C File Offset: 0x0007EB6C
	private ScreenMaskSpawn(ScreenMaskSpawn.ScreenMaskType maskType, Game game, SessionActionTracker parentAction)
	{
		this.maskType = maskType;
		this.simulation = game.simulation;
		base.RegisterNewInstance(game, parentAction);
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x000809A8 File Offset: 0x0007EBA8
	public static void Spawn(ScreenMaskSpawn.ScreenMaskType maskType, Game game, SessionActionTracker parentAction, SBGUIElement parentElement, SBGUIScreen containingScreen, Simulated parentSimulated, TerrainSlot slot, float radius, string texture, Vector3 offset, bool useSecondCam = false)
	{
		ScreenMaskSpawn screenMaskSpawn = new ScreenMaskSpawn(maskType, game, parentAction);
		if (maskType == ScreenMaskSpawn.ScreenMaskType.ELEMENT)
		{
			screenMaskSpawn.RegisterNewInstanceForElement(game, parentAction, parentElement, containingScreen, radius, texture, offset, useSecondCam);
		}
		else if (maskType == ScreenMaskSpawn.ScreenMaskType.EXPANSION)
		{
			screenMaskSpawn.RegisterNewInstanceForExpansion(game, parentAction, slot, radius, texture, offset, useSecondCam);
		}
		else
		{
			screenMaskSpawn.RegisterNewInstanceForSimulated(game, parentAction, parentSimulated, radius, texture, offset, useSecondCam);
		}
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x00080A0C File Offset: 0x0007EC0C
	protected void RegisterNewInstanceForElement(Game game, SessionActionTracker parentAction, SBGUIElement uiElement, SBGUIScreen containingScreen, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
		this.uiElement = uiElement;
		this.uiMixin = new UiSpawnMixin();
		this.uiMixin.OnRegisterNewInstance(parentAction, containingScreen);
		if (!parentAction.ManualSuccess)
		{
			foreach (SBGUIButton sbguibutton in this.uiElement.GetComponentsInChildren<SBGUIButton>())
			{
				sbguibutton.ClickEvent += delegate()
				{
					parentAction.MarkSucceeded(false);
				};
			}
		}
		if (this.maskType == ScreenMaskSpawn.ScreenMaskType.ELEMENT)
		{
			this.camera2 = this.GetSecondUICamera();
			if (this.camera2)
			{
				if (useSecondCam)
				{
					this.CreateScreenMaskMesh(radius, null, offset, null, true);
					this.CreateScreenMaskMesh(radius, texture, offset, this.camera2, false);
					this.fullScreen = true;
				}
				else
				{
					this.CreateScreenMaskMesh(radius, texture, offset, null, false);
					this.CreateScreenMaskMesh(radius, null, offset, this.camera2, true);
					this.fullScreen2 = true;
				}
			}
		}
		if (this.camera2 == null)
		{
			this.CreateScreenMaskMesh(radius, texture, offset, null, false);
		}
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x00080B34 File Offset: 0x0007ED34
	private Camera GetSecondUICamera()
	{
		Camera camera = GUIMainView.GetInstance().camera;
		GameObject gameObject = camera.gameObject;
		Camera[] componentsInChildren = gameObject.GetComponentsInChildren<Camera>();
		foreach (Camera camera2 in componentsInChildren)
		{
			if (camera2 != camera)
			{
				return camera2;
			}
		}
		return null;
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x00080B8C File Offset: 0x0007ED8C
	protected void RegisterNewInstanceForSimulated(Game game, SessionActionTracker parentAction, Simulated parentSimulated, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
		this.simulated = parentSimulated;
		if (this.simulated != null)
		{
			this.simHandler = delegate()
			{
				if (parentAction.Status == SessionActionTracker.StatusCode.STARTED)
				{
					parentAction.MarkSucceeded();
				}
			};
			this.simulated.AddClickListener(this.simHandler);
		}
		this.CreateScreenMaskMesh(radius, texture, offset, null, false);
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x00080BEC File Offset: 0x0007EDEC
	protected void RegisterNewInstanceForExpansion(Game game, SessionActionTracker parentAction, TerrainSlot slot, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
		this.slot = slot;
		if (this.slot != null)
		{
			this.slotHandler = delegate()
			{
				if (parentAction.Status == SessionActionTracker.StatusCode.STARTED)
				{
					parentAction.MarkSucceeded();
				}
			};
			this.slot.AddClickListener(this.slotHandler);
		}
		this.CreateScreenMaskMesh(radius, texture, offset, null, false);
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x00080C4C File Offset: 0x0007EE4C
	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		SessionActionManager.SpawnReturnCode spawnReturnCode;
		if (this.maskType == ScreenMaskSpawn.ScreenMaskType.ELEMENT)
		{
			spawnReturnCode = base.OnUpdate(game);
			if (spawnReturnCode != SessionActionManager.SpawnReturnCode.KILL)
			{
				if (this.screenMaskGO != null && !this.fullScreen)
				{
					float x;
					float y;
					this.UpdateDynamicElement(GUIMainView.GetInstance().camera, out x, out y);
					this.screenMaskGO.transform.localPosition = new Vector3(x, y, this.screenZ) + this.offset;
				}
				if (this.screenMaskGO2 != null && this.camera2 != null && !this.fullScreen2)
				{
					float x2;
					float y2;
					this.UpdateDynamicElement(this.camera2, out x2, out y2);
					this.screenMaskGO2.transform.localPosition = new Vector3(x2, y2, this.screenZ) + this.offset;
				}
				if (this.uiElement == null || this.uiElement.gameObject == null || !this.uiElement.IsActive())
				{
					if (base.ParentAction.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS && base.ParentAction.Status != SessionActionTracker.StatusCode.OBLITERATED)
					{
						base.ParentAction.MarkFailed();
					}
					this.Destroy();
					spawnReturnCode = SessionActionManager.SpawnReturnCode.KILL;
				}
			}
		}
		else
		{
			if (this.maskType == ScreenMaskSpawn.ScreenMaskType.SIMULATED && (this.simulated == null || !this.simulated.Visible))
			{
				base.ParentAction.MarkFailed();
			}
			float x3;
			float y3;
			this.UpdateDynamicElement(GUIMainView.GetInstance().camera, out x3, out y3);
			this.screenMaskGO.transform.localPosition = new Vector3(x3, y3, this.screenZ);
			spawnReturnCode = base.OnUpdate(game);
		}
		return spawnReturnCode;
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x00080E14 File Offset: 0x0007F014
	public override void Destroy()
	{
		if (this.screenMaskGO != null)
		{
			UnityEngine.Object.Destroy(this.screenMaskGO);
			this.screenMaskGO = null;
		}
		if (this.screenMaskGO2 != null)
		{
			UnityEngine.Object.Destroy(this.screenMaskGO2);
			this.screenMaskGO2 = null;
			this.camera2 = null;
		}
		if (this.uiMixin != null)
		{
			this.uiMixin.Destroy();
			this.uiMixin = null;
		}
		if (this.simulated != null)
		{
			this.simulated.RemoveClickListener(this.simHandler);
		}
		if (this.slot != null)
		{
			this.slot.RemoveClickListener(this.slotHandler);
		}
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x00080EC8 File Offset: 0x0007F0C8
	private void CreateScreenMaskMesh(float radius, string texture, Vector3 offset, Camera secondCam, bool coverFullScreen = false)
	{
		Camera camera = GUIMainView.GetInstance().camera;
		Camera camera2;
		if (secondCam != null)
		{
			camera2 = secondCam;
		}
		else
		{
			camera2 = camera;
		}
		GameObject gameObject = UnityGameResources.Create("Prefabs/ScreenMask");
		gameObject.name = "ScreenMaskSpawn";
		gameObject.layer = LayerMask.NameToLayer("__GUI__");
		if (secondCam == null)
		{
			this.screenMaskGO = gameObject;
		}
		else
		{
			this.screenMaskGO2 = gameObject;
		}
		if (texture != null && texture.Length > 0)
		{
			Texture2D texture2D = Resources.Load("Textures/GUI/" + texture) as Texture2D;
			if (texture2D != null)
			{
				gameObject.renderer.material.mainTexture = texture2D;
			}
		}
		this.offset = offset;
		if (this.maskType == ScreenMaskSpawn.ScreenMaskType.ELEMENT)
		{
			this.offsetAbsMax = Math.Abs(Math.Max(offset.x, offset.y));
		}
		else
		{
			this.offsetAbsMax = 0f;
		}
		float num = Math.Max(this.offsetAbsMax, 1f);
		this.borderStepY = camera2.orthographicSize * 2f + num;
		if (!double.IsInfinity((double)camera2.aspect) && !double.IsNaN((double)camera2.aspect))
		{
			this.borderStepX = this.borderStepY * camera2.aspect;
		}
		else
		{
			this.borderStepX = this.borderStepY * 2f;
		}
		this.centerStepY = radius * 2f;
		this.centerStepX = this.centerStepY;
		float num2 = this.borderStepX + this.centerStepX * 0.5f;
		float num3 = this.borderStepY + this.centerStepY * 0.5f;
		gameObject.transform.parent = camera2.transform;
		float x = 0f;
		float y = 0f;
		if (!coverFullScreen)
		{
			this.UpdateDynamicElement(camera2, out x, out y);
		}
		else
		{
			x = (this.borderStepX + this.centerStepX) * 0.5f;
			y = (this.borderStepY + this.centerStepY) * 0.5f;
		}
		Vector3 vector = new Vector3(x, y, this.screenZ);
		if (this.maskType == ScreenMaskSpawn.ScreenMaskType.ELEMENT && !coverFullScreen)
		{
			vector += offset;
		}
		gameObject.transform.localPosition = vector;
		gameObject.transform.localRotation = Quaternion.identity;
		Mesh mesh = new Mesh();
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		Vector3[] array = new Vector3[16];
		Vector2[] array2 = new Vector2[16];
		int[] array3 = new int[54];
		int num4 = 0;
		float num5 = -num3;
		for (int i = 0; i < 4; i++)
		{
			float num6 = -num2;
			for (int j = 0; j < 4; j++)
			{
				array[num4].x = num6;
				array[num4].y = num5;
				array[num4].z = 0f;
				if (j != 1)
				{
					num6 += this.borderStepX;
				}
				else
				{
					num6 += this.centerStepX;
				}
				if (j <= 1)
				{
					array2[num4].x = 0f;
				}
				else
				{
					array2[num4].x = 1f;
				}
				if (i <= 1)
				{
					array2[num4].y = 0f;
				}
				else
				{
					array2[num4].y = 1f;
				}
				num4++;
			}
			if (i != 1)
			{
				num5 += this.borderStepY;
			}
			else
			{
				num5 += this.centerStepY;
			}
		}
		int num7 = 0;
		for (int k = 0; k < 3; k++)
		{
			int num8 = 4 * k;
			for (int l = 0; l < 3; l++)
			{
				array3[num7++] = num8;
				array3[num7++] = num8 + 4;
				array3[num7++] = num8 + 1;
				array3[num7++] = num8 + 1;
				array3[num7++] = num8 + 4;
				array3[num7++] = num8 + 4 + 1;
				num8++;
			}
		}
		TFUtils.Assert(num7 == 54, "Error in screen mesh");
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array3;
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x0008132C File Offset: 0x0007F52C
	private void UpdateDynamicElement(Camera cam, out float offsetX, out float offsetY)
	{
		float num;
		float num2;
		if (this.maskType == ScreenMaskSpawn.ScreenMaskType.ELEMENT)
		{
			Vector2 vector = cam.WorldToViewportPoint(this.uiElement.tform.position);
			num = vector.x * cam.pixelWidth;
			num2 = vector.y * cam.pixelHeight;
		}
		else if (this.maskType == ScreenMaskSpawn.ScreenMaskType.SIMULATED)
		{
			Vector3 vector2 = this.simulation.TheCamera.WorldToScreenPoint(this.simulated.DisplayController.Position + this.offset);
			num = vector2.x;
			num2 = vector2.y;
		}
		else if (this.maskType == ScreenMaskSpawn.ScreenMaskType.EXPANSION)
		{
			Vector3 vector3 = this.simulation.TheCamera.WorldToScreenPoint(this.slot.Position + this.offset);
			num = vector3.x;
			num2 = vector3.y;
		}
		else
		{
			Vector3 vector4 = this.simulation.TheCamera.WorldToScreenPoint(this.offset);
			num = vector4.x;
			num2 = vector4.y;
		}
		float m = cam.projectionMatrix.m00;
		float m2 = cam.projectionMatrix.m11;
		offsetX = 2f * (num / cam.pixelWidth - 0.5f) / m;
		offsetY = 2f * (num2 / cam.pixelHeight - 0.5f) / m2;
		this.ClampOffset(cam, ref offsetX, ref offsetY);
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x000814A0 File Offset: 0x0007F6A0
	private void ClampOffset(Camera cam, ref float offsetX, ref float offsetY)
	{
		float num = cam.orthographicSize * 2f;
		float num2 = cam.orthographicSize * 2f * cam.aspect;
		float num3 = 0.5f * (num2 + this.centerStepX) + this.offsetAbsMax;
		float num4 = 0.5f * (num + this.centerStepY) + this.offsetAbsMax;
		if (offsetX < -num3)
		{
			offsetX = -num3;
		}
		else if (offsetX > num3)
		{
			offsetX = num3;
		}
		if (offsetY < -num4)
		{
			offsetY = -num4;
		}
		else if (offsetY > num4)
		{
			offsetY = num4;
		}
	}

	// Token: 0x04000CDD RID: 3293
	private ScreenMaskSpawn.ScreenMaskType maskType;

	// Token: 0x04000CDE RID: 3294
	private SBGUIElement uiElement;

	// Token: 0x04000CDF RID: 3295
	private Simulated simulated;

	// Token: 0x04000CE0 RID: 3296
	private TerrainSlot slot;

	// Token: 0x04000CE1 RID: 3297
	private Action simHandler;

	// Token: 0x04000CE2 RID: 3298
	private Action slotHandler;

	// Token: 0x04000CE3 RID: 3299
	private GameObject screenMaskGO;

	// Token: 0x04000CE4 RID: 3300
	private GameObject screenMaskGO2;

	// Token: 0x04000CE5 RID: 3301
	private Camera camera2;

	// Token: 0x04000CE6 RID: 3302
	private bool fullScreen;

	// Token: 0x04000CE7 RID: 3303
	private bool fullScreen2;

	// Token: 0x04000CE8 RID: 3304
	private float screenZ = 0.2f;

	// Token: 0x04000CE9 RID: 3305
	private Vector3 offset;

	// Token: 0x04000CEA RID: 3306
	private float offsetAbsMax;

	// Token: 0x04000CEB RID: 3307
	private float borderStepX;

	// Token: 0x04000CEC RID: 3308
	private float borderStepY;

	// Token: 0x04000CED RID: 3309
	private float centerStepX;

	// Token: 0x04000CEE RID: 3310
	private float centerStepY;

	// Token: 0x04000CEF RID: 3311
	private UiSpawnMixin uiMixin;

	// Token: 0x04000CF0 RID: 3312
	protected Simulation simulation;

	// Token: 0x02000243 RID: 579
	[Flags]
	public enum ScreenMaskType
	{
		// Token: 0x04000CF2 RID: 3314
		ELEMENT = 0,
		// Token: 0x04000CF3 RID: 3315
		SIMULATED = 1,
		// Token: 0x04000CF4 RID: 3316
		SIMULATION = 2,
		// Token: 0x04000CF5 RID: 3317
		EXPANSION = 3
	}
}
