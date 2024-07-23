using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
[AddComponentMenu("FingerGestures/Toolbox/Input Manager")]
public class TBInputManager : MonoBehaviour
{
	// Token: 0x06000131 RID: 305 RVA: 0x00006BB4 File Offset: 0x00004DB4
	private void Start()
	{
		if (!this.raycastCamera)
		{
			this.raycastCamera = Camera.main;
		}
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00006BD4 File Offset: 0x00004DD4
	private void OnEnable()
	{
		if (this.trackFingerDown)
		{
			FingerGestures.OnFingerDown += this.FingerGestures_OnFingerDown;
		}
		if (this.trackFingerUp)
		{
			FingerGestures.OnFingerUp += this.FingerGestures_OnFingerUp;
		}
		if (this.trackDrag)
		{
			FingerGestures.OnFingerDragBegin += this.FingerGestures_OnFingerDragBegin;
		}
		if (this.trackTap)
		{
			FingerGestures.OnFingerTap += this.FingerGestures_OnFingerTap;
			FingerGestures.OnFingerDoubleTap += this.FingerGestures_OnFingerDoubleTap;
		}
		if (this.trackLongPress)
		{
			FingerGestures.OnFingerLongPress += this.FingerGestures_OnFingerLongPress;
		}
		if (this.trackSwipe)
		{
			FingerGestures.OnFingerSwipe += this.FingerGestures_OnFingerSwipe;
		}
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00006C9C File Offset: 0x00004E9C
	private void OnDisable()
	{
		FingerGestures.OnFingerDown -= this.FingerGestures_OnFingerDown;
		FingerGestures.OnFingerUp -= this.FingerGestures_OnFingerUp;
		FingerGestures.OnFingerDragBegin -= this.FingerGestures_OnFingerDragBegin;
		FingerGestures.OnFingerTap -= this.FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDoubleTap -= this.FingerGestures_OnFingerDoubleTap;
		FingerGestures.OnFingerLongPress -= this.FingerGestures_OnFingerLongPress;
		FingerGestures.OnFingerSwipe -= this.FingerGestures_OnFingerSwipe;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00006D20 File Offset: 0x00004F20
	private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		TBFingerUp tbfingerUp = this.PickComponent<TBFingerUp>(fingerPos);
		if (tbfingerUp && tbfingerUp.enabled)
		{
			tbfingerUp.RaiseFingerUp(fingerIndex, fingerPos, timeHeldDown);
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00006D58 File Offset: 0x00004F58
	private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		TBFingerDown tbfingerDown = this.PickComponent<TBFingerDown>(fingerPos);
		if (tbfingerDown && tbfingerDown.enabled)
		{
			tbfingerDown.RaiseFingerDown(fingerIndex, fingerPos);
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00006D8C File Offset: 0x00004F8C
	private void FingerGestures_OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		TBDrag tbdrag = this.PickComponent<TBDrag>(startPos);
		if (tbdrag && tbdrag.enabled && !tbdrag.Dragging)
		{
			tbdrag.BeginDrag(fingerIndex, fingerPos);
			tbdrag.OnDragMove += this.draggable_OnDragMove;
			tbdrag.OnDragEnd += this.draggable_OnDragEnd;
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x00006DF0 File Offset: 0x00004FF0
	public bool ProjectScreenPointOnDragPlane(Vector3 refPos, Vector2 screenPos, out Vector3 worldPos)
	{
		worldPos = refPos;
		switch (this.dragPlaneType)
		{
		case TBInputManager.DragPlaneType.XY:
			worldPos = this.raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(refPos.z - this.raycastCamera.transform.position.z)));
			return true;
		case TBInputManager.DragPlaneType.XZ:
			worldPos = this.raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(refPos.y - this.raycastCamera.transform.position.y)));
			return true;
		case TBInputManager.DragPlaneType.ZY:
			worldPos = this.raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(refPos.x - this.raycastCamera.transform.position.x)));
			return true;
		case TBInputManager.DragPlaneType.UseCollider:
		{
			Ray ray = this.raycastCamera.ScreenPointToRay(screenPos);
			RaycastHit raycastHit;
			if (!this.dragPlaneCollider.Raycast(ray, out raycastHit, 3.4028235E+38f))
			{
				return false;
			}
			worldPos = raycastHit.point + this.dragPlaneOffset * raycastHit.normal;
			return true;
		}
		case TBInputManager.DragPlaneType.Camera:
		{
			Transform transform = this.raycastCamera.transform;
			Plane plane = new Plane(-transform.forward, refPos);
			Ray ray2 = this.raycastCamera.ScreenPointToRay(screenPos);
			float distance = 0f;
			if (!plane.Raycast(ray2, out distance))
			{
				return false;
			}
			worldPos = ray2.GetPoint(distance);
			return true;
		}
		default:
			return false;
		}
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00006FC0 File Offset: 0x000051C0
	private void draggable_OnDragMove(TBDrag sender)
	{
		Vector2 screenPos = sender.FingerPos - sender.MoveDelta;
		Vector3 b;
		Vector3 a;
		if (this.ProjectScreenPointOnDragPlane(sender.transform.position, screenPos, out b) && this.ProjectScreenPointOnDragPlane(sender.transform.position, sender.FingerPos, out a))
		{
			Vector3 b2 = a - b;
			sender.transform.position += b2;
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00007038 File Offset: 0x00005238
	private void draggable_OnDragEnd(TBDrag source)
	{
		source.OnDragMove -= this.draggable_OnDragMove;
		source.OnDragEnd -= this.draggable_OnDragEnd;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000706C File Offset: 0x0000526C
	private void FingerGestures_OnFingerTap(int fingerIndex, Vector2 fingerPos)
	{
		TBTap tbtap = this.PickComponent<TBTap>(fingerPos);
		if (tbtap && tbtap.enabled && tbtap.tapMode == TBTap.TapMode.SingleTap)
		{
			tbtap.RaiseTap(fingerIndex, fingerPos);
		}
	}

	// Token: 0x0600013B RID: 315 RVA: 0x000070AC File Offset: 0x000052AC
	private void FingerGestures_OnFingerDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
		TBTap tbtap = this.PickComponent<TBTap>(fingerPos);
		if (tbtap && tbtap.enabled && tbtap.tapMode == TBTap.TapMode.DoubleTap)
		{
			tbtap.RaiseTap(fingerIndex, fingerPos);
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000070EC File Offset: 0x000052EC
	private void FingerGestures_OnFingerLongPress(int fingerIndex, Vector2 fingerPos)
	{
		TBLongPress tblongPress = this.PickComponent<TBLongPress>(fingerPos);
		if (tblongPress && tblongPress.enabled)
		{
			tblongPress.RaiseLongPress(fingerIndex, fingerPos);
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00007120 File Offset: 0x00005320
	private void FingerGestures_OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		TBSwipe tbswipe = this.PickComponent<TBSwipe>(startPos);
		if (tbswipe && tbswipe.enabled)
		{
			tbswipe.RaiseSwipe(fingerIndex, startPos, direction, velocity);
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00007158 File Offset: 0x00005358
	public GameObject PickObject(Vector2 screenPos)
	{
		Ray ray = this.raycastCamera.ScreenPointToRay(screenPos);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 3.4028235E+38f, ~this.ignoreLayers))
		{
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000071A4 File Offset: 0x000053A4
	public T PickComponent<T>(Vector2 screenPos) where T : TBComponent
	{
		GameObject gameObject = this.PickObject(screenPos);
		if (!gameObject)
		{
			return (T)((object)null);
		}
		return gameObject.GetComponent<T>();
	}

	// Token: 0x040000A9 RID: 169
	public bool trackFingerUp = true;

	// Token: 0x040000AA RID: 170
	public bool trackFingerDown = true;

	// Token: 0x040000AB RID: 171
	public bool trackDrag = true;

	// Token: 0x040000AC RID: 172
	public bool trackTap = true;

	// Token: 0x040000AD RID: 173
	public bool trackLongPress = true;

	// Token: 0x040000AE RID: 174
	public bool trackSwipe = true;

	// Token: 0x040000AF RID: 175
	public Camera raycastCamera;

	// Token: 0x040000B0 RID: 176
	public LayerMask ignoreLayers = 0;

	// Token: 0x040000B1 RID: 177
	public TBInputManager.DragPlaneType dragPlaneType = TBInputManager.DragPlaneType.Camera;

	// Token: 0x040000B2 RID: 178
	public Collider dragPlaneCollider;

	// Token: 0x040000B3 RID: 179
	public float dragPlaneOffset;

	// Token: 0x0200001E RID: 30
	public enum DragPlaneType
	{
		// Token: 0x040000B5 RID: 181
		XY,
		// Token: 0x040000B6 RID: 182
		XZ,
		// Token: 0x040000B7 RID: 183
		ZY,
		// Token: 0x040000B8 RID: 184
		UseCollider,
		// Token: 0x040000B9 RID: 185
		Camera
	}
}
