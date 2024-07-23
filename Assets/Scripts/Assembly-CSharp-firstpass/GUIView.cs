using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using UnityEngine;
using Yarg;

// Token: 0x020000EA RID: 234
[ExecuteInEditMode]
public class GUIView : MonoBehaviour
{
	// Token: 0x140000D6 RID: 214
	// (add) Token: 0x060008BD RID: 2237 RVA: 0x0002144C File Offset: 0x0001F64C
	// (remove) Token: 0x060008BE RID: 2238 RVA: 0x00021468 File Offset: 0x0001F668
	private event Action refreshEvent;

	// Token: 0x140000D7 RID: 215
	// (add) Token: 0x060008BF RID: 2239 RVA: 0x00021484 File Offset: 0x0001F684
	// (remove) Token: 0x060008C0 RID: 2240 RVA: 0x000214E4 File Offset: 0x0001F6E4
	public event Action RefreshEvent
	{
		add
		{
			if (this.refreshEvent == null)
			{
				this.refreshEvent = (Action)Delegate.Combine(this.refreshEvent, value);
				return;
			}
			this.refreshEvent = (Action)Delegate.Remove(this.refreshEvent, value);
			this.refreshEvent = (Action)Delegate.Combine(this.refreshEvent, value);
		}
		remove
		{
			if (this.refreshEvent == null)
			{
				return;
			}
			this.refreshEvent = (Action)Delegate.Remove(this.refreshEvent, value);
		}
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0002150C File Offset: 0x0001F70C
	public static bool IsIPhone6s(ref bool plusSize)
	{
		bool result = false;
		plusSize = false;
		return result;
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00021520 File Offset: 0x0001F720
	private static int IPHONE_SCREEN_HEIGHT
	{
		get
		{
			if (GUIView.IPHONE_SCREEN_HEIGHT_LOCAL < 0)
			{
				GUIView.IPHONE_SCREEN_HEIGHT_LOCAL = 640;
			}
			return GUIView.IPHONE_SCREEN_HEIGHT_LOCAL;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0002153C File Offset: 0x0001F73C
	public static bool RetinaDisplay
	{
		get
		{
			return Screen.dpi > 250f;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0002154C File Offset: 0x0001F74C
	public YGTextureLibrary Library
	{
		get
		{
			if (this.library == null)
			{
				this.library = base.GetComponent<YGTextureLibrary>();
				if (this.library == null)
				{
					GUIView parentView = GUIView.GetParentView(base.transform.parent);
					this.library = ((!(parentView == null)) ? parentView.Library : null);
				}
			}
			return this.library;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060008C5 RID: 2245 RVA: 0x000215BC File Offset: 0x0001F7BC
	public YG2DWorld _2DWorld
	{
		get
		{
			if (this._world == null)
			{
				this._world = base.gameObject.GetComponent<YG2DWorld>();
				if (this._world == null)
				{
					Debug.LogError("GUIView does not have a 2DWorld associated.");
				}
			}
			return this._world;
		}
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0002160C File Offset: 0x0001F80C
	public void RefreshWorld()
	{
		this.updateWorld = true;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00021618 File Offset: 0x0001F818
	private void UpdateWorld()
	{
		if (this.updateWorld)
		{
			this.updateWorld = false;
			this._2DWorld.World.Step(0f);
		}
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00021648 File Offset: 0x0001F848
	public float GetPixelScale()
	{
		return this.pixelScale;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00021650 File Offset: 0x0001F850
	public Bounds GetTotalBounds()
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}
		Bounds bounds = componentsInChildren[0].bounds;
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			bounds.Encapsulate(componentsInChildren[i].bounds);
		}
		return bounds;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x000216B4 File Offset: 0x0001F8B4
	public void ReloadSprites()
	{
		this.SendRefreshEvent();
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x000216BC File Offset: 0x0001F8BC
	protected void ValidateTargets(List<ITouchable> prev, List<ITouchable> current)
	{
		if (prev.Count == 0)
		{
			return;
		}
		YGEvent ygevent = new YGEvent();
		ygevent.type = YGEvent.TYPE.RESET;
		for (int i = 0; i < prev.Count; i++)
		{
			if (current == null)
			{
				prev[i].TouchEvent(ygevent);
			}
			else if (!current.Contains(prev[i]))
			{
				prev[i].TouchEvent(ygevent);
			}
		}
		if (current == null)
		{
			prev.Clear();
		}
		else
		{
			prev = current;
		}
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00021748 File Offset: 0x0001F948
	protected YGEvent UpdateAndSendEvent(YGEvent evt, List<ITouchable> targets)
	{
		YGEvent ygevent = null;
		if (this.eventHistory.TryGetValue(evt.fingerId, out ygevent) && ygevent != null)
		{
			evt = ygevent.Update(evt);
		}
		else
		{
			this.eventHistory[evt.fingerId] = evt;
		}
		if (evt.type == YGEvent.TYPE.TOUCH_END || evt.type == YGEvent.TYPE.TOUCH_CANCEL)
		{
			this.eventHistory.Remove(evt.fingerId);
		}
		else if (evt.type == YGEvent.TYPE.TOUCH_STAY && evt.Hold)
		{
			this.eventHistory[evt.fingerId] = evt;
		}
		targets.Sort(delegate(ITouchable a, ITouchable b)
		{
			float z = a.tform.position.z;
			float z2 = b.tform.position.z;
			return z.CompareTo(z2);
		});
		for (int i = 0; i < targets.Count; i++)
		{
			if (!evt.used || (evt.type != YGEvent.TYPE.TAP && evt.type != YGEvent.TYPE.TOUCH_BEGIN && evt.type != YGEvent.TYPE.TOUCH_END))
			{
				if (targets[i].TouchEvent(evt))
				{
					evt.used = true;
				}
			}
		}
		return evt;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00021878 File Offset: 0x0001FA78
	public static float ResolutionScaleFactor()
	{
		if (GUIView.RESOLUTION_FACTOR < 0f)
		{
			GUIView.RESOLUTION_FACTOR = (float)Screen.height / 380f;
		}
		return GUIView.RESOLUTION_FACTOR;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x000218A0 File Offset: 0x0001FAA0
	protected virtual void ResizePortal()
	{
		this.Cam.orthographicSize = this.pixelScale / GUIView.ResolutionScaleFactor();
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x000218BC File Offset: 0x0001FABC
	public static GUIView GetParentView(Transform tf)
	{
		GUIView guiview = null;
		while (tf != null)
		{
			guiview = tf.GetComponent<GUIView>();
			if (guiview != null)
			{
				return guiview;
			}
			tf = tf.parent;
		}
		if (guiview == null)
		{
			return GUIMainView.GetInstance();
		}
		return guiview;
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00021910 File Offset: 0x0001FB10
	public Vector3 PixelsToWorld(Vector2 pixels)
	{
		Vector3 position = new Vector3(pixels.x, (float)Screen.height - pixels.y, -this.Cam.transform.position.z);
		Vector3 result = this.Cam.ScreenToWorldPoint(position);
		result.z = 0f;
		return result;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0002196C File Offset: 0x0001FB6C
	public Vector3 ScreenToWorld(Vector2 screenPos)
	{
		Vector3 result = this.Cam.ScreenToWorldPoint(screenPos);
		result.z = 0f;
		return result;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00021998 File Offset: 0x0001FB98
	public Vector3 WorldToScreen(Vector3 worldPos)
	{
		return this.Cam.WorldToScreenPoint(worldPos);
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060008D3 RID: 2259 RVA: 0x000219A8 File Offset: 0x0001FBA8
	public Camera Cam
	{
		get
		{
			if (this._cam == null)
			{
				this._cam = base.GetComponent<Camera>();
				if (this._cam == null)
				{
					this._cam = base.gameObject.AddComponent<Camera>();
					this._cam.isOrthoGraphic = true;
					this._cam.clearFlags = CameraClearFlags.Depth;
					this._cam.cullingMask = 1 << LayerMask.NameToLayer("__GUI__");
				}
			}
			return this._cam;
		}
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x00021A2C File Offset: 0x0001FC2C
	public virtual void RegisterTouchable(int t, ITouchable touchable)
	{
		this.touchables[t] = touchable;
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00021A3C File Offset: 0x0001FC3C
	public void UnregisterTouchable(int t)
	{
		this.touchables.Remove(t);
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00021A4C File Offset: 0x0001FC4C
	private void PixelSnapTransform(Transform transf)
	{
		Vector3 position = transf.position;
		position.x = (float)Mathf.RoundToInt(position.x / 0.01f) * 0.01f;
		position.y = (float)Mathf.RoundToInt(position.y / 0.01f) * 0.01f;
		position.z = (float)Mathf.RoundToInt(position.z / 0.01f) * 0.01f;
		transf.position = position;
		foreach (object obj in transf)
		{
			Transform transf2 = (Transform)obj;
			this.PixelSnapTransform(transf2);
		}
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00021B24 File Offset: 0x0001FD24
	public void PixelSnapSprites()
	{
		this.PixelSnapTransform(base.transform);
		YGAtlasSprite[] componentsInChildren = this.Cam.GetComponentsInChildren<YGAtlasSprite>(true);
		foreach (YGAtlasSprite ygatlasSprite in componentsInChildren)
		{
			ygatlasSprite.PixelSnap();
		}
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00021B6C File Offset: 0x0001FD6C
	protected virtual void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer("__GUI__");
		this.guiMask = 1 << LayerMask.NameToLayer("__GUI__");
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00021BA4 File Offset: 0x0001FDA4
	protected virtual void OnEnable()
	{
		this.targets = new List<ITouchable>(10);
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00021BB4 File Offset: 0x0001FDB4
	protected virtual void Start()
	{
		if (this.Cam.orthographic)
		{
			this.ResizePortal();
		}
		this.ReadyEvent.FireEvent();
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00021BD8 File Offset: 0x0001FDD8
	protected virtual void OnDisable()
	{
		this.library = null;
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00021BE4 File Offset: 0x0001FDE4
	public void SnapAnchors()
	{
		ScreenAnchor[] componentsInChildren = base.GetComponentsInChildren<ScreenAnchor>(true);
		foreach (ScreenAnchor screenAnchor in componentsInChildren)
		{
			screenAnchor.SnapAnchor(this.Cam);
		}
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00021C20 File Offset: 0x0001FE20
	protected void SendRefreshEvent()
	{
		if (this.refreshEvent == null)
		{
			return;
		}
		Delegate[] invocationList = this.refreshEvent.GetInvocationList();
		this.refreshEvent = null;
		for (int i = 0; i < invocationList.Length; i++)
		{
			((Action)invocationList[i])();
		}
		this.UpdateWorld();
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00021C74 File Offset: 0x0001FE74
	protected virtual void LateUpdate()
	{
		this.SendRefreshEvent();
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00021C7C File Offset: 0x0001FE7C
	protected virtual List<ITouchable> RayHit(Vector2 pos)
	{
		YG2DWorld 2DWorld = this._2DWorld;
		this.targets.Clear();
		if (!Application.isPlaying || 2DWorld == null)
		{
			return this.targets;
		}
		List<Fixture> hitFixtures = 2DWorld.GetHitFixtures(pos);
		if (hitFixtures.Count == 0)
		{
			return this.targets;
		}
		foreach (Fixture fixture in hitFixtures)
		{
			YG2DBody yg2DBody = fixture.Body.UserData as YG2DBody;
			if (!(yg2DBody == null))
			{
				int instanceID = yg2DBody.transform.GetInstanceID();
				if (this.touchables.ContainsKey(instanceID))
				{
					this.targets.Add(this.touchables[instanceID]);
				}
			}
		}
		return this.targets;
	}

	// Token: 0x0400057D RID: 1405
	public const string guiLayer = "__GUI__";

	// Token: 0x0400057E RID: 1406
	public const float UNITS_PER_PIXEL = 0.01f;

	// Token: 0x0400057F RID: 1407
	private static float RESOLUTION_FACTOR = -1f;

	// Token: 0x04000580 RID: 1408
	private static int IPHONE_SCREEN_HEIGHT_LOCAL = -1;

	// Token: 0x04000581 RID: 1409
	[HideInInspector]
	public int guiMask;

	// Token: 0x04000582 RID: 1410
	protected Dictionary<int, ITouchable> touchables = new Dictionary<int, ITouchable>();

	// Token: 0x04000583 RID: 1411
	protected List<ITouchable> targets = new List<ITouchable>(5);

	// Token: 0x04000584 RID: 1412
	protected List<ITouchable> activeTargetSet = new List<ITouchable>(5);

	// Token: 0x04000585 RID: 1413
	private RaycastHit[] hits;

	// Token: 0x04000586 RID: 1414
	private YGTextureLibrary library;

	// Token: 0x04000587 RID: 1415
	private volatile bool updateWorld;

	// Token: 0x04000588 RID: 1416
	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	// Token: 0x04000589 RID: 1417
	protected YG2DWorld _world;

	// Token: 0x0400058A RID: 1418
	protected float pixelScale;

	// Token: 0x0400058B RID: 1419
	protected Dictionary<int, YGEvent> eventHistory = new Dictionary<int, YGEvent>();

	// Token: 0x0400058C RID: 1420
	protected Camera _cam;
}
