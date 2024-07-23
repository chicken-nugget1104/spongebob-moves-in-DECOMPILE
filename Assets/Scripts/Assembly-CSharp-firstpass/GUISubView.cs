using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x020000E9 RID: 233
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(YG2DWorld))]
public class GUISubView : GUIView, ITouchable
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x00021114 File Offset: 0x0001F314
	public Transform tform
	{
		get
		{
			return (!(this._tform != null)) ? (this._tform = base.transform) : this._tform;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0002114C File Offset: 0x0001F34C
	private GUIView ParentView
	{
		get
		{
			if (this._parentView == null)
			{
				this._parentView = GUIView.GetParentView(base.transform.parent);
			}
			return this._parentView;
		}
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0002117C File Offset: 0x0001F37C
	public void SetRegion(ScrollRegion rgn)
	{
		this.region = rgn;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00021188 File Offset: 0x0001F388
	public virtual bool TouchEvent(YGEvent evt)
	{
		this.targets = this.RayHit(evt.position);
		return base.UpdateAndSendEvent(evt, this.targets).used;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x000211BC File Offset: 0x0001F3BC
	protected override List<ITouchable> RayHit(Vector2 pos)
	{
		this.targets = base.RayHit(pos);
		if (this.region != null)
		{
			this.targets.Add(this.region);
		}
		return this.targets;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x000211F4 File Offset: 0x0001F3F4
	protected override void OnDisable()
	{
		this._parentView = null;
		base.OnDisable();
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00021204 File Offset: 0x0001F404
	private void OnDestroy()
	{
		GUIMainView instance = GUIMainView.GetInstance();
		instance.RemoveSubView(this);
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00021220 File Offset: 0x0001F420
	public static GUISubView Create(Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = string.Format("GUISubView_{0}", (uint)gameObject.GetInstanceID());
		gameObject.transform.parent = parent;
		GUISubView guisubView = gameObject.AddComponent<GUISubView>();
		Camera cam = guisubView.Cam;
		cam.orthographic = true;
		cam.farClipPlane = 20f;
		cam.nearClipPlane = 0f;
		cam.clearFlags = CameraClearFlags.Depth;
		cam.cullingMask = 1 << LayerMask.NameToLayer("__GUI__");
		return guisubView;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x000212A4 File Offset: 0x0001F4A4
	public bool ContainsPoint(Vector2 point)
	{
		return this.viewRect.Contains(point);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x000212C0 File Offset: 0x0001F4C0
	public void SetPortal(Rect p)
	{
		this.viewRect = p;
		this.ResizePortal();
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x000212D0 File Offset: 0x0001F4D0
	protected override void ResizePortal()
	{
		Camera cam = this.ParentView.Cam;
		Vector3 vector = cam.WorldToViewportPoint(new Vector2(this.viewRect.xMax, this.viewRect.yMax));
		Vector3 vector2 = cam.WorldToViewportPoint(new Vector2(this.viewRect.xMin, this.viewRect.yMin));
		Rect rect = new Rect(0f, 0f, 0f, 0f);
		rect.xMax = vector.x;
		rect.yMax = vector.y;
		rect.xMin = vector2.x;
		rect.yMin = vector2.y;
		base.Cam.rect = rect;
		this.pixelScale = (float)Screen.height * 0.01f * rect.height;
		base.ResizePortal();
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x000213B8 File Offset: 0x0001F5B8
	private void OnDrawGizmos()
	{
		Bounds totalBounds = base.GetTotalBounds();
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(totalBounds.center, totalBounds.size);
	}

	// Token: 0x04000579 RID: 1401
	[NonSerialized]
	protected Transform _tform;

	// Token: 0x0400057A RID: 1402
	private GUIView _parentView;

	// Token: 0x0400057B RID: 1403
	private ScrollRegion region;

	// Token: 0x0400057C RID: 1404
	private Rect viewRect;
}
