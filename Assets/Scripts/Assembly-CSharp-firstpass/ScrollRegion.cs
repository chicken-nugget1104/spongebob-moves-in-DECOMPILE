using System;
using UnityEngine;
using Yarg;

// Token: 0x020000EC RID: 236
public class ScrollRegion : MonoBehaviour, ITouchable
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060008E6 RID: 2278 RVA: 0x00021FB4 File Offset: 0x000201B4
	public Transform tform
	{
		get
		{
			return (!(this._tform != null)) ? (this._tform = base.transform) : this._tform;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00021FEC File Offset: 0x000201EC
	public Transform SubViewTform
	{
		get
		{
			return this.subView.transform;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060008E8 RID: 2280 RVA: 0x00021FFC File Offset: 0x000201FC
	// (set) Token: 0x060008E9 RID: 2281 RVA: 0x00022010 File Offset: 0x00020210
	public bool Visible
	{
		get
		{
			return this.subView.Cam.enabled;
		}
		set
		{
			YG2DBody[] componentsInChildren = this.subView.gameObject.GetComponentsInChildren<YG2DBody>();
			foreach (YG2DBody yg2DBody in componentsInChildren)
			{
				yg2DBody.enabled = value;
			}
			this.subView.Cam.enabled = value;
		}
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00022060 File Offset: 0x00020260
	private void SendPostInitializationReadyEvent()
	{
		if (!this.mainViewReady || !this.subViewReady)
		{
			return;
		}
		this.ReadyEvent.FireEvent();
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00022090 File Offset: 0x00020290
	private void CreateSubView()
	{
		this.subView = this.mainView.CreateSubView();
		this.subView.ReadyEvent.AddListener(delegate()
		{
			this.subViewReady = true;
			this.subView.SetRegion(this);
			this.MatchSubView();
			this.SendPostInitializationReadyEvent();
		});
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x000220C0 File Offset: 0x000202C0
	private void OnEnable()
	{
		this.mainView = GUIMainView.GetInstance();
		this.mainView.ReadyEvent.AddListener(delegate()
		{
			this.mainViewReady = true;
			if (this.subView == null)
			{
				this.CreateSubView();
			}
			else
			{
				this.subView.gameObject.SetActiveRecursively(true);
				this.mainView.AddSubView(this.subView);
				this.subView.RegisterTouchable(this.tform.GetInstanceID(), this);
			}
			this.SendPostInitializationReadyEvent();
		});
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x000220EC File Offset: 0x000202EC
	private void MoveChildrenToSubView()
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			transform.gameObject.SetActiveRecursively(false);
			transform.parent = this.subView.tform;
			transform.gameObject.SetActiveRecursively(true);
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00022180 File Offset: 0x00020380
	private void OnDisable()
	{
		this.mainView.RemoveSubView(this.subView);
		if (this.subView != null)
		{
			this.subView.gameObject.SetActiveRecursively(false);
			this.subView.UnregisterTouchable(this.tform.GetInstanceID());
		}
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x000221D8 File Offset: 0x000203D8
	private void OnDestroy()
	{
		if (this.subView != null)
		{
			UnityEngine.Object.Destroy(this.subView.gameObject);
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x000221FC File Offset: 0x000203FC
	public Bounds GetTotalBounds()
	{
		return this.subView.GetTotalBounds();
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0002220C File Offset: 0x0002040C
	public Rect GetWorldRect()
	{
		return this.worldRect;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00022214 File Offset: 0x00020414
	public Vector3 ScreenToWorld(Vector3 pos)
	{
		Vector3 result = this.subView.ScreenToWorld(pos);
		result.z = this.subView.tform.position.z;
		return result;
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x00022254 File Offset: 0x00020454
	public void MatchSubView()
	{
		Vector2 vector = this.size * 0.01f;
		Vector3 position = base.transform.position;
		this.worldRect = new Rect(position.x, position.y, vector.x, vector.y);
		this.subView.SetPortal(this.worldRect);
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x000222B8 File Offset: 0x000204B8
	public void ResetContents(YGEvent evt)
	{
		evt.type = YGEvent.TYPE.RESET;
		this.subView.TouchEvent(evt);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x000222D0 File Offset: 0x000204D0
	public virtual bool TouchEvent(YGEvent evt)
	{
		return this.ScrollEvent.FireEvent(evt);
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x000222E0 File Offset: 0x000204E0
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Vector2 vector = this.size * 0.01f;
		Vector3 position = base.transform.position;
		Rect rect = new Rect(position.x, position.y, vector.x, vector.y);
		Vector3 center = rect.center;
		center.z = position.z;
		Vector3 vector2 = new Vector3(rect.width, rect.height, 0f);
		Gizmos.DrawWireCube(center, vector2);
	}

	// Token: 0x04000592 RID: 1426
	public Vector2 size = new Vector2(200f, 100f);

	// Token: 0x04000593 RID: 1427
	private GUIMainView mainView;

	// Token: 0x04000594 RID: 1428
	public GUISubView subView;

	// Token: 0x04000595 RID: 1429
	private Rect worldRect;

	// Token: 0x04000596 RID: 1430
	private Transform _tform;

	// Token: 0x04000597 RID: 1431
	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	// Token: 0x04000598 RID: 1432
	public YGEventDispatcher ScrollEvent = new YGEventDispatcher();

	// Token: 0x04000599 RID: 1433
	private bool mainViewReady;

	// Token: 0x0400059A RID: 1434
	private bool subViewReady;
}
