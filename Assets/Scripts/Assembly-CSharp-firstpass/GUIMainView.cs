using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x020000E8 RID: 232
[RequireComponent(typeof(YG2DWorld))]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(YGTextureLibrary))]
public class GUIMainView : GUIView
{
	// Token: 0x06000893 RID: 2195 RVA: 0x000207E0 File Offset: 0x0001E9E0
	public void ClearFinalEventListener()
	{
		this.FinalEventListener.ClearListeners();
		this.pauseFinalEventListener = false;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x000207F4 File Offset: 0x0001E9F4
	public void PauseFinalEventListener(bool pause)
	{
		this.pauseFinalEventListener = pause;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00020800 File Offset: 0x0001EA00
	public static GUIMainView GetInstance()
	{
		if (GUIMainView.instance == null && Application.isPlaying)
		{
			GUIMainView.SetInstance((GUIMainView)UnityEngine.Object.FindObjectOfType(typeof(GUIMainView)));
			if (GUIMainView.instance == null)
			{
				Debug.LogWarning("No GUIMainView in scene, creating one");
				GameObject gameObject = new GameObject("__GUIMainView__");
				GUIMainView.SetInstance(gameObject.AddComponent<GUIMainView>());
			}
		}
		return GUIMainView.instance;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00020878 File Offset: 0x0001EA78
	private static bool SetInstance(GUIMainView inst)
	{
		if (GUIMainView.instance != null && GUIMainView.instance != inst)
		{
			return false;
		}
		GUIMainView.instance = inst;
		return true;
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x000208A4 File Offset: 0x0001EAA4
	protected override void OnEnable()
	{
		if (!GUIMainView.SetInstance(this))
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		base.useGUILayout = false;
		this.RegisterFingers(true);
		base.OnEnable();
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000208D8 File Offset: 0x0001EAD8
	protected override void OnDisable()
	{
		this.RegisterFingers(false);
		base.OnDisable();
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000899 RID: 2201 RVA: 0x000208E8 File Offset: 0x0001EAE8
	public static float EffectiveDPI
	{
		get
		{
			float num = (Screen.dpi != 0f) ? Screen.dpi : 110f;
			if (Screen.height < 800 || Screen.width < 800)
			{
				num *= 0.5f;
			}
			return num;
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0002093C File Offset: 0x0001EB3C
	protected override void Start()
	{
		GUIMainView.FINGER_DRAG_RADIUS_SQR = Mathf.Pow(0.5f * GUIMainView.EffectiveDPI, 2f);
		base.Start();
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0002096C File Offset: 0x0001EB6C
	protected override void ResizePortal()
	{
		this.pixelScale = (float)Screen.height * 0.01f;
		base.ResizePortal();
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x00020988 File Offset: 0x0001EB88
	public Bounds ViewBounds()
	{
		Camera cam = base.Cam;
		Vector3 min = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane));
		Vector3 max = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.farClipPlane));
		Bounds result = new Bounds(Vector3.zero, Vector3.zero);
		result.SetMinMax(min, max);
		return result;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x000209F0 File Offset: 0x0001EBF0
	public GUISubView CreateSubView()
	{
		GUISubView guisubView = GUISubView.Create(base.transform);
		Vector3 position = base.transform.position;
		position.z += base.Cam.farClipPlane + 1f;
		float num = base.Cam.depth + 1f;
		foreach (GUISubView guisubView2 in this.subViews)
		{
			position.z += guisubView2.Cam.farClipPlane + 1f;
			num += 1f;
		}
		guisubView.Cam.depth = num;
		guisubView.transform.position = position;
		this.AddSubView(guisubView);
		return guisubView;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00020AE4 File Offset: 0x0001ECE4
	public bool AddSubView(GUISubView sub)
	{
		if (this.subViews.Contains(sub))
		{
			return false;
		}
		this.subViews.Add(sub);
		return true;
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00020B14 File Offset: 0x0001ED14
	public bool RemoveSubView(GUISubView sub)
	{
		this.activeTargetSet.Remove(sub);
		this.targets.Remove(sub);
		return this.subViews.Remove(sub);
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00020B48 File Offset: 0x0001ED48
	protected override List<ITouchable> RayHit(Vector2 pos)
	{
		this.targets = base.RayHit(pos);
		if (this.subViews != null)
		{
			foreach (GUISubView guisubView in this.subViews)
			{
				if (guisubView.ContainsPoint(base.ScreenToWorld(pos)))
				{
					this.targets.Add(guisubView);
				}
			}
		}
		return this.targets;
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00020BE8 File Offset: 0x0001EDE8
	private void RegisterFingers(bool active)
	{
		if (active)
		{
			FingerGestures.OnTap += this.Tap;
			FingerGestures.OnFingerDown += this.FingerDown;
			FingerGestures.OnFingerUp += this.FingerUp;
			FingerGestures.OnPinchBegin += this.PinchBegin;
			FingerGestures.OnPinchMove += this.PinchMove;
			FingerGestures.OnPinchEnd += this.PinchEnd;
			FingerGestures.OnLongPress += this.LongPress;
			FingerGestures.OnDragBegin += this.DragBegin;
			FingerGestures.OnDragEnd += this.DragEnd;
			FingerGestures.OnDragMove += this.DragMove;
			FingerGestures.OnDragStationary += this.DragStationary;
		}
		else
		{
			FingerGestures.OnTap -= this.Tap;
			FingerGestures.OnFingerDown -= this.FingerDown;
			FingerGestures.OnFingerUp -= this.FingerUp;
			FingerGestures.OnPinchBegin -= this.PinchBegin;
			FingerGestures.OnPinchMove -= this.PinchMove;
			FingerGestures.OnPinchEnd -= this.PinchEnd;
			FingerGestures.OnLongPress -= this.LongPress;
			FingerGestures.OnDragBegin -= this.DragBegin;
			FingerGestures.OnDragEnd -= this.DragEnd;
			FingerGestures.OnDragMove -= this.DragMove;
			FingerGestures.OnDragStationary -= this.DragStationary;
		}
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x00020D78 File Offset: 0x0001EF78
	private void FingerUp(int fingerIndex, Vector2 pos, float time)
	{
		if (this.currentFinger != null && this.currentFinger.Value == fingerIndex)
		{
			this.BroadcastEvent(pos, YGEvent.TYPE.TOUCH_END, null);
			this.currentFinger = null;
		}
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x00020DC8 File Offset: 0x0001EFC8
	private void FingerDown(int fingerIndex, Vector2 pos)
	{
		int? num = this.currentFinger;
		if (num == null)
		{
			this.currentFinger = new int?(fingerIndex);
			this.BroadcastEvent(pos, YGEvent.TYPE.TOUCH_BEGIN, null);
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00020E08 File Offset: 0x0001F008
	private void Tap(Vector2 pos)
	{
		this.startPosition = null;
		this.currentFinger = null;
		this.BroadcastEvent(pos, YGEvent.TYPE.TAP, null);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00020E48 File Offset: 0x0001F048
	private void PinchBegin(Vector2 pos1, Vector2 pos2)
	{
		this.BroadcastEvent(pos1, YGEvent.TYPE.RESET, null);
		this.BroadcastEvent(pos2, YGEvent.TYPE.RESET, null);
		this.startPosition = null;
		this.currentFinger = null;
		this.PinchMove(pos1, pos2, 0f);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00020EA4 File Offset: 0x0001F0A4
	private void PinchEnd(Vector2 pos1, Vector2 pos2)
	{
		YGEvent ygevent = new YGEvent();
		ygevent.type = YGEvent.TYPE.TOUCH_END;
		ygevent.startPosition = pos1;
		ygevent.position = pos2;
		ygevent.deltaPosition = pos1 - pos2;
		if (ygevent != null && !ygevent.used && !this.pauseFinalEventListener)
		{
			this.FinalEventListener.FireEvent(ygevent);
		}
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00020F04 File Offset: 0x0001F104
	private void PinchMove(Vector2 pos1, Vector2 pos2, float delta)
	{
		YGEvent ygevent = new YGEvent();
		ygevent.type = YGEvent.TYPE.PINCH;
		ygevent.startPosition = pos1;
		ygevent.position = pos2;
		ygevent.deltaPosition = ygevent.position - ygevent.startPosition;
		ygevent.distance = delta;
		if (ygevent != null && !ygevent.used && !this.pauseFinalEventListener)
		{
			this.FinalEventListener.FireEvent(ygevent);
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00020F74 File Offset: 0x0001F174
	private void LongPress(Vector2 pos)
	{
		this.BroadcastEvent(pos, YGEvent.TYPE.HOLD, null);
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00020F94 File Offset: 0x0001F194
	private void DragBegin(Vector2 pos, Vector2 startPos)
	{
		this.startPosition = new Vector2?(startPos);
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00020FA4 File Offset: 0x0001F1A4
	private void BroadcastEvent(Vector2 pos, YGEvent.TYPE type, Vector2? delta = null)
	{
		List<ITouchable> targets = this.RayHit(pos);
		YGEvent ygevent = new YGEvent();
		ygevent.type = type;
		YGEvent ygevent2 = ygevent;
		ygevent.startPosition = pos;
		ygevent2.position = pos;
		if (delta != null)
		{
			ygevent.deltaPosition = delta.Value;
		}
		ygevent = base.UpdateAndSendEvent(ygevent, targets);
		if (ygevent != null && !ygevent.used && !this.pauseFinalEventListener)
		{
			this.FinalEventListener.FireEvent(ygevent);
		}
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00021020 File Offset: 0x0001F220
	private void DragMove(Vector2 pos, Vector2 delta)
	{
		if (this.startPosition != null && (this.startPosition.Value - pos).sqrMagnitude > GUIMainView.FINGER_DRAG_RADIUS_SQR)
		{
			List<ITouchable> targets = this.RayHit(this.startPosition.Value);
			YGEvent ygevent = new YGEvent();
			ygevent.type = YGEvent.TYPE.RESET;
			YGEvent ygevent2 = ygevent;
			ygevent.startPosition = pos;
			ygevent2.position = pos;
			ygevent.deltaPosition = delta;
			base.UpdateAndSendEvent(ygevent, targets);
			this.startPosition = null;
		}
		this.BroadcastEvent(pos, YGEvent.TYPE.TOUCH_MOVE, new Vector2?(delta));
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x000210C0 File Offset: 0x0001F2C0
	private void DragStationary(Vector2 pos)
	{
		this.BroadcastEvent(pos, YGEvent.TYPE.TOUCH_STAY, null);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x000210E0 File Offset: 0x0001F2E0
	private void DragEnd(Vector2 pos)
	{
		this.startPosition = null;
		this.currentFinger = null;
	}

	// Token: 0x0400056F RID: 1391
	public const float DESKTOP_DPI_GUESS = 110f;

	// Token: 0x04000570 RID: 1392
	private const float FINGER_DRAG_RADIUS_INCHES = 0.5f;

	// Token: 0x04000571 RID: 1393
	public Vector2 defaultResolution = new Vector2(800f, 600f);

	// Token: 0x04000572 RID: 1394
	private static GUIMainView instance;

	// Token: 0x04000573 RID: 1395
	public EventDispatcher<YGEvent> FinalEventListener = new EventDispatcher<YGEvent>();

	// Token: 0x04000574 RID: 1396
	private bool pauseFinalEventListener;

	// Token: 0x04000575 RID: 1397
	private List<GUISubView> subViews = new List<GUISubView>();

	// Token: 0x04000576 RID: 1398
	private static float FINGER_DRAG_RADIUS_SQR;

	// Token: 0x04000577 RID: 1399
	private int? currentFinger;

	// Token: 0x04000578 RID: 1400
	protected Vector2? startPosition;
}
