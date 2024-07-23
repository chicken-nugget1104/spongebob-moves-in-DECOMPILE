using System;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class ScreenAnchor : MonoBehaviour
{
	// Token: 0x060008E2 RID: 2274 RVA: 0x00021DDC File Offset: 0x0001FFDC
	private void OnEnable()
	{
		if (!this.registerAnchorEvent)
		{
			return;
		}
		this.view = GUIView.GetParentView(base.transform);
		this.view.RefreshEvent += this.view.SnapAnchors;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00021E18 File Offset: 0x00020018
	private void OnDisable()
	{
		if (!this.registerAnchorEvent)
		{
			return;
		}
		this.view.RefreshEvent -= this.view.SnapAnchors;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00021E50 File Offset: 0x00020050
	public void SnapAnchor(Camera cam)
	{
		Vector3 zero = Vector3.zero;
		switch (this.anchor)
		{
		case SpritePivot.LowerCenter:
		case SpritePivot.MiddleCenter:
		case SpritePivot.UpperCenter:
			zero.x = 0.5f;
			break;
		case SpritePivot.LowerLeft:
		case SpritePivot.MiddleLeft:
		case SpritePivot.UpperLeft:
			zero.x = 0f;
			break;
		case SpritePivot.LowerRight:
		case SpritePivot.MiddleRight:
		case SpritePivot.UpperRight:
			zero.x = 1f;
			break;
		}
		switch (this.anchor)
		{
		case SpritePivot.LowerCenter:
		case SpritePivot.LowerLeft:
		case SpritePivot.LowerRight:
			zero.y = 0f;
			break;
		case SpritePivot.MiddleCenter:
		case SpritePivot.MiddleLeft:
		case SpritePivot.MiddleRight:
			zero.y = 0.5f;
			break;
		case SpritePivot.UpperCenter:
		case SpritePivot.UpperLeft:
		case SpritePivot.UpperRight:
			zero.y = 1f;
			break;
		}
		zero.z = -cam.transform.position.z;
		Vector3 position = cam.ViewportToWorldPoint(zero);
		position.z = cam.transform.position.z;
		base.transform.position = position;
	}

	// Token: 0x0400058F RID: 1423
	public bool registerAnchorEvent = true;

	// Token: 0x04000590 RID: 1424
	public SpritePivot anchor = SpritePivot.MiddleCenter;

	// Token: 0x04000591 RID: 1425
	private GUIView view;
}
