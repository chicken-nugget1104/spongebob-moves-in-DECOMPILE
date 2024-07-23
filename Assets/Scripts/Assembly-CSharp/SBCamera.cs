using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Yarg;

// Token: 0x0200003A RID: 58
public class SBCamera
{
	// Token: 0x06000284 RID: 644 RVA: 0x0000C72C File Offset: 0x0000A92C
	public SBCamera()
	{
		this.camera.transform.position = new Vector3(820f, 520f, 150f);
		this.camera.orthographicSize = 150f;
		this.targetPosition = this.camera.transform.position;
		this.targetZoom = this.camera.orthographicSize;
		this.momentum = new Momentum();
		this.camera.transform.LookAt(this.camera.transform.position + SBCamera.CameraDirectionDefinition(), new Vector3(1f, 1f, 1f));
		this.PauseStateMachine();
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
	public void AutoPanToPosition(Vector2 worldTarget, float screenSafeZonePercentageHeight)
	{
		this.autoPanTargetLookAt = worldTarget;
		this.safeZonePixels = (float)Screen.height * (1f - screenSafeZonePercentageHeight);
		this.ChangeState(SBCamera.State.AutoPanning);
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000287 RID: 647 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
	public Camera UnityCamera
	{
		get
		{
			return this.camera;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000288 RID: 648 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
	public Vector2 ScreenCenter
	{
		get
		{
			return this.camera.pixelRect.center;
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x0000C908 File Offset: 0x0000AB08
	public void SetEnableUserInput(bool isEnabled, bool isDraggingBuilding = false, [Optional] Vector3 interactionStripPosition3D)
	{
		this.allowUserInput = isEnabled;
		this.isDraggingBuilding = isDraggingBuilding;
		if (interactionStripPosition3D != default(Vector3))
		{
			this.interactionStripPosition3D = interactionStripPosition3D;
		}
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000C940 File Offset: 0x0000AB40
	public static Vector3 CameraDirectionDefinition()
	{
		float num = Mathf.Sqrt(2f) * Mathf.Tan(0.5235988f);
		return new Vector3(1f, 1f, -num);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000C974 File Offset: 0x0000AB74
	public static void BillboardDefinition(Transform t, IDisplayController idc)
	{
		t.LookAt(t.position - SBCamera.CameraDirectionDefinition(), SBCamera.up);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000C994 File Offset: 0x0000AB94
	public static Vector3 CameraUp()
	{
		return SBCamera.up;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000C99C File Offset: 0x0000AB9C
	public Vector2 WorldPointToScreenPoint(Vector3 worldPosition)
	{
		Vector3 vector = this.camera.WorldToScreenPoint(worldPosition);
		return new Vector2(vector.x, SBGUI.GetScreenHeight() - vector.y);
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
	public Vector3 ScreenPointToWorldPoint(Terrain terrain, Vector2 screenPoint)
	{
		if (terrain == null)
		{
			return Vector3.zero;
		}
		Ray ray = this.camera.ScreenPointToRay(TFUtils.TruncateVector(screenPoint));
		Vector3 result;
		bool condition = terrain.ComputeIntersection(ray, out result);
		TFUtils.Assert(condition, "Could not intersect against the ground!");
		return result;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0000CA1C File Offset: 0x0000AC1C
	public Ray ScreenPointToRay(Vector2 position)
	{
		return this.camera.ScreenPointToRay(position);
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000CA30 File Offset: 0x0000AC30
	public Vector3 ScreenSpaceToTerrainSpace(Vector2 cameraVector, Terrain terrain)
	{
		float magnitude = cameraVector.magnitude;
		cameraVector.Normalize();
		Ray ray = this.ScreenPointToRay(Vector2.zero);
		Ray ray2 = this.ScreenPointToRay(cameraVector);
		Vector3 b;
		bool flag = terrain.ComputeIntersection(ray, out b);
		Vector3 a;
		bool flag2 = terrain.ComputeIntersection(ray2, out a);
		TFUtils.Assert(flag && flag2, "Fail on display offset computation, please check on camera or terrain data.");
		return (a - b) * magnitude;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000CA9C File Offset: 0x0000AC9C
	private void UpdateTransform(Session session)
	{
		this.camera.transform.position = this.targetPosition;
		this.camera.orthographicSize = this.targetZoom;
		this.camera.nearClipPlane = -this.camera.orthographicSize * 1.5f;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000CAF0 File Offset: 0x0000ACF0
	public void ResetCameraPosition()
	{
		Debug.Log("ResetCameraPosition");
		this.camera.transform.position = new Vector3(820f, 520f, 150f);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000CB2C File Offset: 0x0000AD2C
	public void StartCamera()
	{
		Debug.Log("StartCamera");
		this.ActivateStateMachine();
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0000CB40 File Offset: 0x0000AD40
	public void StopCamera()
	{
		Debug.Log("StopCamera");
		this.PauseStateMachine();
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000295 RID: 661 RVA: 0x0000CB54 File Offset: 0x0000AD54
	public bool ScreenBufferOn
	{
		get
		{
			Debug.Log("ScreenBufferOn: " + this.camera.enabled);
			return !this.camera.enabled;
		}
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000CB90 File Offset: 0x0000AD90
	public void TurnOnScreenBuffer(float zDist)
	{
		this.PrepareSwitchToReducedBuffer(zDist);
		this.camera.enabled = false;
		this.camera.Render();
		SBCamera.fullScreenQuadGO.active = true;
		Debug.Log("TurnOnScreenBuffer: " + zDist);
		this.StopCamera();
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
	public void TurnOnScreenBuffer()
	{
		this.TurnOnScreenBuffer(18f);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
	public void TurnOffScreenBuffer()
	{
		SBCamera.offScreenRenderTexture.DiscardContents();
		SBCamera.offScreenRenderTexture.Release();
		this.camera.enabled = true;
		this.camera.targetTexture = null;
		SBCamera.fullScreenQuadGO.active = false;
		this.StartCamera();
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000CC40 File Offset: 0x0000AE40
	private void PrepareSwitchToReducedBuffer(float zDist)
	{
		if (SBCamera.offScreenRenderTexture == null)
		{
			SBCamera.CreateScreenRenderTexture();
		}
		if (SBCamera.fullScreenQuadGO == null)
		{
			SBCamera.CreateFullScreenQuad();
		}
		SBCamera.fullScreenQuadGO.transform.localPosition = new Vector3(0f, 0f, zDist);
		this.camera.targetTexture = SBCamera.offScreenRenderTexture;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000CCA8 File Offset: 0x0000AEA8
	private static void CreateScreenRenderTexture()
	{
		int num = 1;
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			num = 2;
		}
		int width = Screen.width / num;
		int height = Screen.height / num;
		int depth = 24;
		SBCamera.offScreenRenderTexture = new RenderTexture(width, height, depth, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
		SBCamera.offScreenRenderTexture.wrapMode = TextureWrapMode.Clamp;
		SBCamera.offScreenRenderTexture.anisoLevel = 0;
		SBCamera.offScreenRenderTexture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000CD08 File Offset: 0x0000AF08
	private static void CreateFullScreenQuad()
	{
		SBCamera.fullScreenQuadGO = new GameObject("FullScreenQuad");
		SBCamera.fullScreenQuadGO.layer = LayerMask.NameToLayer("__GUI__");
		Camera camera = GUIMainView.GetInstance().camera;
		SBCamera.fullScreenQuadGO.transform.parent = camera.transform;
		SBCamera.fullScreenQuadGO.transform.localPosition = Vector3.zero;
		SBCamera.fullScreenQuadGO.transform.localRotation = Quaternion.identity;
		SBCamera.fullScreenQuadGO.AddComponent<MeshFilter>();
		MeshFilter component = SBCamera.fullScreenQuadGO.GetComponent<MeshFilter>();
		SBCamera.fullScreenQuadGO.AddComponent<MeshRenderer>();
		MeshRenderer component2 = SBCamera.fullScreenQuadGO.GetComponent<MeshRenderer>();
		component2.castShadows = false;
		component2.receiveShadows = false;
		Shader shader = Shader.Find("Mobile/Unlit (Supports Lightmap)");
		component2.material = new Material(shader)
		{
			name = SBCamera.fullScreenQuadGO.name + "_Mat",
			mainTexture = SBCamera.offScreenRenderTexture
		};
		Mesh mesh = new Mesh();
		component.mesh = mesh;
		float orthographicSize = camera.orthographicSize;
		float num = orthographicSize * camera.aspect;
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-num, -orthographicSize, 0f),
			new Vector3(-num, orthographicSize, 0f),
			new Vector3(num, -orthographicSize, 0f),
			new Vector3(num, orthographicSize, 0f)
		};
		mesh.vertices = vertices;
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000CF2C File Offset: 0x0000B12C
	public void OnUpdate(float dT, Session session)
	{
		TFUtils.Assert(dT > 0f, "We have regressed in time.");
		if (this == null)
		{
			return;
		}
		this.state.OnUpdate(dT, session, this);
		this.momentum.TrackForSmoothing(this.camera.transform.position);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0000CF7C File Offset: 0x0000B17C
	public void HandleGUIEvent(SBGUIEvent evt)
	{
		if (!this.allowUserInput)
		{
			return;
		}
		this.state.OnGuiEvent(evt, this);
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0000CF98 File Offset: 0x0000B198
	public void ProcessExtraGuiEvent(SBGUIEvent evt)
	{
		this.HandleGUIEvent(evt);
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0000CFA4 File Offset: 0x0000B1A4
	public void ResetCurrentState()
	{
		this.state.OnResetState(this);
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
	protected void PauseStateMachine()
	{
		this.state = this.states[SBCamera.State.Paused];
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000CFC8 File Offset: 0x0000B1C8
	protected void ActivateStateMachine()
	{
		this.state = this.states[SBCamera.State.AtRest];
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000CFDC File Offset: 0x0000B1DC
	public void ChangeState(SBCamera.State state)
	{
		if (this.state == this.states[state])
		{
			return;
		}
		this.state.OnLeave(this);
		this.state = this.states[state];
		this.state.OnEnter(this);
		if (this.deferredGuiEvent != null && !this.deferredGuiEvent.used)
		{
			this.deferredGuiEvent.used = true;
			this.state.OnGuiEvent(this.deferredGuiEvent, this);
		}
		this.deferredGuiEvent = null;
	}

	// Token: 0x04000141 RID: 321
	public const bool DEBUG_LOG = false;

	// Token: 0x04000142 RID: 322
	public const float TAP_NUDGE_TOLERANCE = 400f;

	// Token: 0x04000143 RID: 323
	public const double PIXEL_TO_WORLD = 0.1302;

	// Token: 0x04000144 RID: 324
	public const double WORLD_TO_PIXEL = 7.680491551459292;

	// Token: 0x04000145 RID: 325
	private const float INIT_ORTHO_SIZE = 150f;

	// Token: 0x04000146 RID: 326
	public const float INIT_CAMERA_X = 820f;

	// Token: 0x04000147 RID: 327
	public const float INIT_CAMERA_Y = 520f;

	// Token: 0x04000148 RID: 328
	public const float MAX_CAMERA_DRAG_Y = 815f;

	// Token: 0x04000149 RID: 329
	private const float NEAR_CLIP_PLANE_ZOOM_COEF = 1.5f;

	// Token: 0x0400014A RID: 330
	private const float PINCH_SCALE = 0.2f;

	// Token: 0x0400014B RID: 331
	private Vector2 autoPanTargetLookAt;

	// Token: 0x0400014C RID: 332
	private Vector3? autoPanTargetCameraPosition;

	// Token: 0x0400014D RID: 333
	private float safeZonePixels;

	// Token: 0x0400014E RID: 334
	private float safeDistanceWorldSqrd;

	// Token: 0x0400014F RID: 335
	private Vector3 previousDragPosition;

	// Token: 0x04000150 RID: 336
	private Vector2 touchDragVectorScreen;

	// Token: 0x04000151 RID: 337
	private Vector2? previousTouchDragCenter;

	// Token: 0x04000152 RID: 338
	private bool dragNeedsUpdate;

	// Token: 0x04000153 RID: 339
	private int xMoveScreenNumber = (int)((float)Screen.width / 8f);

	// Token: 0x04000154 RID: 340
	private int yMoveScreenNumber = (int)((float)Screen.height / 4f);

	// Token: 0x04000155 RID: 341
	private Vector2 moveCamLeft;

	// Token: 0x04000156 RID: 342
	private Vector2 moveCamRight;

	// Token: 0x04000157 RID: 343
	private Vector2 moveCamUp;

	// Token: 0x04000158 RID: 344
	private Vector2 moveCamDown;

	// Token: 0x04000159 RID: 345
	public static bool EnableFullSCreenQuad = false;

	// Token: 0x0400015A RID: 346
	private static float EXPECTED_UPDATE_PERIOD = 0.016666668f;

	// Token: 0x0400015B RID: 347
	private static readonly Vector3 up = new Vector3(0.4f, 0.4f, 0.9f);

	// Token: 0x0400015C RID: 348
	private Camera camera = Camera.main;

	// Token: 0x0400015D RID: 349
	private Vector3 targetPosition;

	// Token: 0x0400015E RID: 350
	private Momentum momentum;

	// Token: 0x0400015F RID: 351
	private float targetZoom;

	// Token: 0x04000160 RID: 352
	private bool allowUserInput = true;

	// Token: 0x04000161 RID: 353
	private bool isDraggingBuilding;

	// Token: 0x04000162 RID: 354
	private Vector3 interactionStripPosition3D;

	// Token: 0x04000163 RID: 355
	public bool freeCameraMode;

	// Token: 0x04000164 RID: 356
	private static RenderTexture offScreenRenderTexture = null;

	// Token: 0x04000165 RID: 357
	private static GameObject fullScreenQuadGO = null;

	// Token: 0x04000166 RID: 358
	private SBCamera.StateBehavior state;

	// Token: 0x04000167 RID: 359
	private Dictionary<SBCamera.State, SBCamera.StateBehavior> states = new Dictionary<SBCamera.State, SBCamera.StateBehavior>
	{
		{
			SBCamera.State.Paused,
			new SBCamera.Paused()
		},
		{
			SBCamera.State.AtRest,
			new SBCamera.AtRest()
		},
		{
			SBCamera.State.Stopping,
			new SBCamera.Stopping()
		},
		{
			SBCamera.State.Dragging,
			new SBCamera.Dragging()
		},
		{
			SBCamera.State.ZoomDragging,
			new SBCamera.ZoomDragging()
		},
		{
			SBCamera.State.AutoPanning,
			new SBCamera.AutoPanning()
		}
	};

	// Token: 0x04000168 RID: 360
	private SBGUIEvent deferredGuiEvent;

	// Token: 0x04000169 RID: 361
	private float initialOrthoSize;

	// Token: 0x0400016A RID: 362
	private float pinchDiff;

	// Token: 0x0400016B RID: 363
	private float? initialPinchMagnitude;

	// Token: 0x0200003B RID: 59
	public class AtRest : SBCamera.StateBehavior
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0000D074 File Offset: 0x0000B274
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
			YGEvent.TYPE type = evt.type;
			if (type == YGEvent.TYPE.TOUCH_BEGIN || type == YGEvent.TYPE.PINCH)
			{
				camera.deferredGuiEvent = evt;
				camera.ChangeState(SBCamera.State.Dragging);
			}
		}
	}

	// Token: 0x0200003C RID: 60
	public class AutoPanning : SBCamera.StateBehavior
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		public override void OnEnter(SBCamera camera)
		{
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000D0BC File Offset: 0x0000B2BC
		public override void OnLeave(SBCamera camera)
		{
			camera.autoPanTargetCameraPosition = null;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D0DC File Offset: 0x0000B2DC
		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
			if (camera.autoPanTargetCameraPosition == null)
			{
				Ray ray = camera.ScreenPointToRay(camera.ScreenCenter);
				Vector3 b;
				session.TheGame.terrain.ComputeIntersection(ray, out b);
				Vector3 b2 = new Vector3(camera.autoPanTargetLookAt.x, camera.autoPanTargetLookAt.y, 0f) - b;
				camera.autoPanTargetCameraPosition = new Vector3?(camera.targetPosition + b2);
				camera.safeDistanceWorldSqrd = camera.ScreenSpaceToTerrainSpace(new Vector2(camera.safeZonePixels, 0f), session.TheGame.terrain).sqrMagnitude;
			}
			Vector3 vector;
			if (!this.IsCloseEnough(camera, out vector))
			{
				vector.Normalize();
				vector *= 5f;
				camera.targetPosition += vector;
				camera.UpdateTransform(session);
			}
			else
			{
				camera.ChangeState(SBCamera.State.AtRest);
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		private bool IsCloseEnough(SBCamera camera, out Vector3 delta)
		{
			delta = camera.autoPanTargetCameraPosition.Value - camera.targetPosition;
			float sqrMagnitude = delta.sqrMagnitude;
			return sqrMagnitude <= camera.safeDistanceWorldSqrd;
		}

		// Token: 0x0400016C RID: 364
		private const float SPEED = 5f;
	}

	// Token: 0x0200003D RID: 61
	public class Dragging : SBCamera.StateBehavior
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000D230 File Offset: 0x0000B430
		public override void OnEnter(SBCamera camera)
		{
			this.InitializeDragParams(camera);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000D23C File Offset: 0x0000B43C
		public override void OnLeave(SBCamera camera)
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000D240 File Offset: 0x0000B440
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
			Vector2 position = evt.position;
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_END:
				camera.ChangeState(SBCamera.State.Stopping);
				break;
			case YGEvent.TYPE.TOUCH_CANCEL:
				camera.ChangeState(SBCamera.State.Stopping);
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				if (camera.previousTouchDragCenter == null)
				{
					camera.previousTouchDragCenter = new Vector2?(position);
					camera.touchDragVectorScreen = Vector2.zero;
				}
				else if (!camera.dragNeedsUpdate)
				{
					camera.touchDragVectorScreen = camera.previousTouchDragCenter.Value - position;
					camera.previousTouchDragCenter = new Vector2?(position);
					camera.dragNeedsUpdate = true;
				}
				break;
			case YGEvent.TYPE.PINCH:
				camera.deferredGuiEvent = evt;
				camera.ChangeState(SBCamera.State.ZoomDragging);
				break;
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000D328 File Offset: 0x0000B528
		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
			if (this == null)
			{
				return;
			}
			if (!camera.allowUserInput)
			{
				camera.ChangeState(SBCamera.State.Stopping);
			}
			else if (camera.isDraggingBuilding)
			{
				bool flag = false;
				Vector3 vector = Vector3.zero;
				if (Input.mousePosition.x < (float)camera.xMoveScreenNumber)
				{
					vector += camera.ScreenSpaceToTerrainSpace(camera.moveCamLeft, session.TheGame.terrain);
					flag = true;
				}
				else if (Input.mousePosition.x > (float)(Screen.width - camera.xMoveScreenNumber))
				{
					vector += camera.ScreenSpaceToTerrainSpace(camera.moveCamRight, session.TheGame.terrain);
					flag = true;
				}
				if (Input.mousePosition.y > (float)(Screen.height - camera.yMoveScreenNumber))
				{
					vector += camera.ScreenSpaceToTerrainSpace(camera.moveCamDown, session.TheGame.terrain);
					flag = true;
				}
				else if (Input.mousePosition.y < (float)camera.yMoveScreenNumber)
				{
					vector += camera.ScreenSpaceToTerrainSpace(camera.moveCamUp, session.TheGame.terrain);
					flag = true;
				}
				if (!flag)
				{
					return;
				}
				camera.targetPosition = camera.previousDragPosition + vector;
				camera.previousDragPosition = camera.targetPosition;
				camera.touchDragVectorScreen = Vector2.zero;
				camera.UpdateTransform(session);
				if (this.panConstraints.HardKeepInBounds(session.TheGame.terrain, camera, camera.ScreenPointToWorldPoint(session.TheGame.terrain, camera.ScreenCenter)))
				{
					camera.UpdateTransform(session);
				}
				camera.dragNeedsUpdate = false;
			}
			else if (camera.dragNeedsUpdate)
			{
				Vector3 b = camera.ScreenSpaceToTerrainSpace(camera.touchDragVectorScreen, session.TheGame.terrain);
				camera.targetPosition = camera.previousDragPosition + b;
				camera.previousDragPosition = camera.targetPosition;
				camera.touchDragVectorScreen = Vector2.zero;
				camera.UpdateTransform(session);
				if (this.panConstraints.HardKeepInBounds(session.TheGame.terrain, camera, camera.ScreenPointToWorldPoint(session.TheGame.terrain, camera.ScreenCenter)))
				{
					camera.UpdateTransform(session);
				}
				camera.dragNeedsUpdate = false;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000D574 File Offset: 0x0000B774
		public override void OnResetState(SBCamera camera)
		{
			this.InitializeDragParams(camera);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D580 File Offset: 0x0000B780
		protected void InitializeDragParams(SBCamera camera)
		{
			camera.previousDragPosition = camera.UnityCamera.transform.position;
			camera.previousTouchDragCenter = null;
			camera.touchDragVectorScreen = Vector2.zero;
			camera.dragNeedsUpdate = false;
			camera.momentum.Reset();
			camera.momentum.ClearTrackPositions();
			camera.moveCamLeft = new Vector2(-880f / camera.targetZoom, 0f);
			camera.moveCamRight = new Vector2(880f / camera.targetZoom, 0f);
			camera.moveCamUp = new Vector2(0f, -600f / camera.targetZoom);
			camera.moveCamDown = new Vector2(0f, 600f / camera.targetZoom);
		}

		// Token: 0x0400016D RID: 365
		protected SBCamera.ZoomConstrainedMixin zoomConstraints = new SBCamera.ZoomConstrainedMixin();

		// Token: 0x0400016E RID: 366
		private SBCamera.PanConstrainedMixin panConstraints = new SBCamera.PanConstrainedMixin();
	}

	// Token: 0x0200003E RID: 62
	public class FrictionMixin
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000D658 File Offset: 0x0000B858
		public bool Apply(float dT, SBCamera camera)
		{
			float p = dT / SBCamera.EXPECTED_UPDATE_PERIOD;
			float amount = Mathf.Pow(0.85f, p);
			camera.momentum.ApplyFriction(amount);
			if (camera.momentum.Velocity.sqrMagnitude < 0.01f)
			{
				camera.momentum.Reset();
				return false;
			}
			return true;
		}

		// Token: 0x0400016F RID: 367
		private const float GLIDE_TOLERANCE_SQARED = 0.01f;

		// Token: 0x04000170 RID: 368
		private const float FRICTION_FACTOR = 0.85f;
	}

	// Token: 0x0200003F RID: 63
	public class PanConstrainedMixin
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
		public bool HardKeepInBounds(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			if (terrain == null || terrain.CameraExtents == null || camera.freeCameraMode)
			{
				return false;
			}
			Vector3 targetPosition = camera.targetPosition;
			float num = targetPosition.x - terrainCameraFocus.x;
			float num2 = targetPosition.y - terrainCameraFocus.y;
			AlignedBox alignedBox = terrain.CameraExtents;
			alignedBox = new AlignedBox(alignedBox.xmin - 50f + num, alignedBox.xmax + 50f + num, alignedBox.ymin - 50f + num2, alignedBox.ymax + 50f + num2);
			camera.targetPosition = new Vector3(Mathf.Clamp(camera.targetPosition.x, alignedBox.xmin, alignedBox.xmax), Mathf.Clamp(camera.targetPosition.y, alignedBox.ymin, alignedBox.ymax), camera.targetPosition.z);
			return camera.targetPosition != targetPosition;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		public bool SmoothKeepInRestBounds(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			if (terrain == null || terrain.CameraExtents == null || terrain.CameraExtents.Contains(terrainCameraFocus.x, terrainCameraFocus.y) || camera.freeCameraMode)
			{
				return false;
			}
			Vector3 zero = Vector3.zero;
			if (terrainCameraFocus.x > terrain.CameraExtents.xmax)
			{
				zero.x = (terrain.CameraExtents.xmax - terrainCameraFocus.x) * 0.1f;
			}
			else if (terrainCameraFocus.x < terrain.CameraExtents.xmin)
			{
				zero.x = (terrain.CameraExtents.xmin - terrainCameraFocus.x) * 0.1f;
			}
			if (terrainCameraFocus.y > terrain.CameraExtents.ymax)
			{
				zero.y = (terrain.CameraExtents.ymax - terrainCameraFocus.y) * 0.1f;
			}
			else if (terrainCameraFocus.y < terrain.CameraExtents.ymin)
			{
				zero.y = (terrain.CameraExtents.ymin - terrainCameraFocus.y) * 0.1f;
			}
			zero.x = Mathf.Round(zero.x * 10f) / 10f;
			zero.y = Mathf.Round(zero.y * 10f) / 10f;
			zero.z = Mathf.Round(zero.z * 10f) / 10f;
			if (zero == Vector3.zero)
			{
				return false;
			}
			camera.targetPosition += zero;
			return true;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000D96C File Offset: 0x0000BB6C
		public bool SmoothKeepInteractionStrip(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			if (terrain != null && terrain.CameraExtents != null && !terrain.CameraExtents.Contains(terrainCameraFocus.x, terrainCameraFocus.y) && !camera.freeCameraMode)
			{
				if (terrainCameraFocus.x > terrain.CameraExtents.xmax)
				{
					return false;
				}
				if (terrainCameraFocus.x < terrain.CameraExtents.xmin)
				{
					return false;
				}
				if (terrainCameraFocus.y > terrain.CameraExtents.ymax)
				{
					return false;
				}
				if (terrainCameraFocus.y < terrain.CameraExtents.ymin)
				{
					return false;
				}
			}
			Vector3 zero = Vector3.zero;
			bool result = false;
			Vector2 vector = camera.WorldPointToScreenPoint(camera.interactionStripPosition3D);
			if (vector.x < (float)(Screen.width / 5))
			{
				zero = new Vector3(2f, -2f);
				camera.targetPosition += zero;
				result = true;
			}
			else if (vector.x > (float)(Screen.width - Screen.width / 5))
			{
				zero = new Vector3(-2f, 2f);
				camera.targetPosition += zero;
				result = true;
			}
			if (vector.y > (float)(Screen.height / 8 * 5))
			{
				zero = new Vector3(-4f, -4f);
				camera.targetPosition += zero;
				return true;
			}
			return result;
		}
	}

	// Token: 0x02000040 RID: 64
	public class Paused : SBCamera.StateBehavior
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000DAF0 File Offset: 0x0000BCF0
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}
	}

	// Token: 0x02000041 RID: 65
	public abstract class StateBehavior
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		public virtual void OnEnter(SBCamera camera)
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000DB00 File Offset: 0x0000BD00
		public virtual void OnLeave(SBCamera camera)
		{
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000DB04 File Offset: 0x0000BD04
		public virtual void OnUpdate(float dT, Session session, SBCamera camera)
		{
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000DB08 File Offset: 0x0000BD08
		public virtual void OnResetState(SBCamera camera)
		{
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000DB0C File Offset: 0x0000BD0C
		public virtual void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}
	}

	// Token: 0x02000042 RID: 66
	public enum State
	{
		// Token: 0x04000172 RID: 370
		Paused,
		// Token: 0x04000173 RID: 371
		AtRest,
		// Token: 0x04000174 RID: 372
		Stopping,
		// Token: 0x04000175 RID: 373
		Dragging,
		// Token: 0x04000176 RID: 374
		ZoomDragging,
		// Token: 0x04000177 RID: 375
		AutoPanning
	}

	// Token: 0x02000043 RID: 67
	public class Stopping : SBCamera.AtRest
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x0000DB3C File Offset: 0x0000BD3C
		public override void OnEnter(SBCamera camera)
		{
			camera.momentum.CalculateSmoothVelocity();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
			if (session.TheGame == null)
			{
				return;
			}
			camera.targetPosition += camera.momentum.Velocity * dT / SBCamera.EXPECTED_UPDATE_PERIOD;
			camera.UpdateTransform(session);
			Terrain terrain = session.TheGame.terrain;
			Vector3 terrainCameraFocus = camera.ScreenPointToWorldPoint(terrain, camera.ScreenCenter);
			bool flag = camera.momentum.Velocity.sqrMagnitude > 0f;
			flag |= this.friction.Apply(dT, camera);
			flag |= this.zoomConstraints.SmoothKeepInRestBounds(camera);
			bool flag2 = this.panConstraints.HardKeepInBounds(terrain, camera, terrainCameraFocus);
			flag = (flag || flag2);
			flag |= this.panConstraints.SmoothKeepInRestBounds(terrain, camera, terrainCameraFocus);
			if (camera.isDraggingBuilding && !flag)
			{
				flag |= this.panConstraints.SmoothKeepInteractionStrip(terrain, camera, terrainCameraFocus);
			}
			if (flag2)
			{
				camera.momentum.Reset();
			}
			if (!flag)
			{
				camera.momentum.Reset();
				camera.ChangeState(SBCamera.State.AtRest);
			}
			else
			{
				camera.UpdateTransform(session);
			}
			base.OnUpdate(dT, session, camera);
		}

		// Token: 0x04000178 RID: 376
		private const float SMOOTH_FACTOR = 9f;

		// Token: 0x04000179 RID: 377
		private SBCamera.FrictionMixin friction = new SBCamera.FrictionMixin();

		// Token: 0x0400017A RID: 378
		private SBCamera.ZoomConstrainedMixin zoomConstraints = new SBCamera.ZoomConstrainedMixin();

		// Token: 0x0400017B RID: 379
		private SBCamera.PanConstrainedMixin panConstraints = new SBCamera.PanConstrainedMixin();
	}

	// Token: 0x02000044 RID: 68
	public class ZoomConstrainedMixin
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000DC74 File Offset: 0x0000BE74
		public ZoomConstrainedMixin()
		{
			SBCamera.ZoomConstrainedMixin.REST_MIN_ORTHO_SIZE = 60f;
			SBCamera.ZoomConstrainedMixin.REST_MAX_ORTHO_SIZE = 200f;
			SBCamera.ZoomConstrainedMixin.HARD_MIN_ORTHO_SIZE = SBCamera.ZoomConstrainedMixin.REST_MIN_ORTHO_SIZE - 10f;
			SBCamera.ZoomConstrainedMixin.HARD_MAX_ORTHO_SIZE = SBCamera.ZoomConstrainedMixin.REST_MAX_ORTHO_SIZE + 25f;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		public bool HardKeepInBounds(SBCamera camera)
		{
			if ((camera.targetZoom > SBCamera.ZoomConstrainedMixin.HARD_MAX_ORTHO_SIZE || camera.targetZoom < SBCamera.ZoomConstrainedMixin.HARD_MIN_ORTHO_SIZE) && !camera.freeCameraMode)
			{
				camera.targetZoom = Mathf.Clamp(camera.targetZoom, SBCamera.ZoomConstrainedMixin.HARD_MIN_ORTHO_SIZE, SBCamera.ZoomConstrainedMixin.HARD_MAX_ORTHO_SIZE);
				return true;
			}
			return false;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000DD14 File Offset: 0x0000BF14
		public bool SmoothKeepInRestBounds(SBCamera camera)
		{
			if (camera.freeCameraMode)
			{
				return false;
			}
			float num = 0f;
			if (camera.targetZoom > SBCamera.ZoomConstrainedMixin.REST_MAX_ORTHO_SIZE)
			{
				num = SBCamera.ZoomConstrainedMixin.REST_MAX_ORTHO_SIZE - camera.targetZoom;
			}
			else if (camera.targetZoom < SBCamera.ZoomConstrainedMixin.REST_MIN_ORTHO_SIZE)
			{
				num = SBCamera.ZoomConstrainedMixin.REST_MIN_ORTHO_SIZE - camera.targetZoom;
			}
			num *= 0.25f;
			if (Mathf.Abs(num) < 0.01f)
			{
				camera.targetZoom = Mathf.Clamp(camera.targetZoom, SBCamera.ZoomConstrainedMixin.REST_MIN_ORTHO_SIZE, SBCamera.ZoomConstrainedMixin.REST_MAX_ORTHO_SIZE);
				return false;
			}
			camera.targetZoom += num;
			return true;
		}

		// Token: 0x0400017C RID: 380
		private const float ZOOM_FRICTION_FACTOR = 0.25f;

		// Token: 0x0400017D RID: 381
		private const float ZOOM_TOLERANCE = 0.01f;

		// Token: 0x0400017E RID: 382
		private static float REST_MIN_ORTHO_SIZE;

		// Token: 0x0400017F RID: 383
		private static float REST_MAX_ORTHO_SIZE;

		// Token: 0x04000180 RID: 384
		private static float HARD_MIN_ORTHO_SIZE;

		// Token: 0x04000181 RID: 385
		private static float HARD_MAX_ORTHO_SIZE;
	}

	// Token: 0x02000045 RID: 69
	public class ZoomDragging : SBCamera.Dragging
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0000DDC8 File Offset: 0x0000BFC8
		public override void OnEnter(SBCamera camera)
		{
			base.InitializeDragParams(camera);
			camera.initialOrthoSize = camera.camera.orthographicSize;
			camera.pinchDiff = 0f;
			camera.initialPinchMagnitude = null;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000DE08 File Offset: 0x0000C008
		public override void OnLeave(SBCamera camera)
		{
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000DE0C File Offset: 0x0000C00C
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
			YGEvent.TYPE type = evt.type;
			switch (type)
			{
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
				break;
			default:
				if (type == YGEvent.TYPE.PINCH)
				{
					Vector2 vector = (evt.position + evt.startPosition) * 0.5f;
					if (camera.initialPinchMagnitude == null)
					{
						camera.initialPinchMagnitude = new float?(evt.deltaPosition.magnitude);
					}
					camera.pinchDiff = evt.deltaPosition.magnitude - camera.initialPinchMagnitude.Value;
					if (camera.previousTouchDragCenter == null)
					{
						camera.previousTouchDragCenter = new Vector2?(vector);
						camera.touchDragVectorScreen = Vector2.zero;
					}
					else
					{
						camera.touchDragVectorScreen = camera.previousTouchDragCenter.Value - vector;
						camera.previousTouchDragCenter = new Vector2?(vector);
						camera.dragNeedsUpdate = true;
					}
					return;
				}
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				camera.deferredGuiEvent = evt;
				camera.ChangeState(SBCamera.State.Dragging);
				return;
			}
			camera.ChangeState(SBCamera.State.Stopping);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000DF20 File Offset: 0x0000C120
		public override void OnUpdate(float dT, Session session, SBCamera sbCamera)
		{
			if (sbCamera.initialPinchMagnitude != null)
			{
				float num = -0.2f * sbCamera.pinchDiff;
				sbCamera.targetZoom = sbCamera.initialOrthoSize + num;
			}
			this.zoomConstraints.HardKeepInBounds(sbCamera);
			base.OnUpdate(dT, session, sbCamera);
		}
	}
}
