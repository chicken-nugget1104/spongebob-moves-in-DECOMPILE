using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
[RequireComponent(typeof(Camera))]
[AddComponentMenu("FingerGestures/Toolbox/Misc/Pinch-Zoom")]
public class TBPinchZoom : MonoBehaviour
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060000FF RID: 255 RVA: 0x0000649C File Offset: 0x0000469C
	// (set) Token: 0x06000100 RID: 256 RVA: 0x000064A4 File Offset: 0x000046A4
	public Vector3 DefaultPos
	{
		get
		{
			return this.defaultPos;
		}
		set
		{
			this.defaultPos = value;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000101 RID: 257 RVA: 0x000064B0 File Offset: 0x000046B0
	// (set) Token: 0x06000102 RID: 258 RVA: 0x000064B8 File Offset: 0x000046B8
	public float DefaultFov
	{
		get
		{
			return this.defaultFov;
		}
		set
		{
			this.defaultFov = value;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000103 RID: 259 RVA: 0x000064C4 File Offset: 0x000046C4
	// (set) Token: 0x06000104 RID: 260 RVA: 0x000064CC File Offset: 0x000046CC
	public float DefaultOrthoSize
	{
		get
		{
			return this.defaultOrthoSize;
		}
		set
		{
			this.defaultOrthoSize = value;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000105 RID: 261 RVA: 0x000064D8 File Offset: 0x000046D8
	// (set) Token: 0x06000106 RID: 262 RVA: 0x000064E0 File Offset: 0x000046E0
	public float ZoomAmount
	{
		get
		{
			return this.zoomAmount;
		}
		set
		{
			this.zoomAmount = Mathf.Clamp(value, this.minZoomAmount, this.maxZoomAmount);
			TBPinchZoom.ZoomMethod zoomMethod = this.zoomMethod;
			if (zoomMethod != TBPinchZoom.ZoomMethod.Position)
			{
				if (zoomMethod == TBPinchZoom.ZoomMethod.FOV)
				{
					if (base.camera.orthographic)
					{
						base.camera.orthographicSize = Mathf.Max(this.defaultOrthoSize - this.zoomAmount, 0.1f);
					}
					else
					{
						base.camera.fov = Mathf.Max(this.defaultFov - this.zoomAmount, 0.1f);
					}
				}
			}
			else
			{
				base.transform.position = this.defaultPos + this.zoomAmount * base.transform.forward;
			}
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000065B0 File Offset: 0x000047B0
	private void Start()
	{
		this.SetDefaults();
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000065B8 File Offset: 0x000047B8
	public void SetDefaults()
	{
		this.DefaultPos = base.transform.position;
		this.DefaultFov = base.camera.fov;
		this.DefaultOrthoSize = base.camera.orthographicSize;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x000065F8 File Offset: 0x000047F8
	private void OnEnable()
	{
		FingerGestures.OnPinchMove += this.FingerGestures_OnPinchMove;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0000660C File Offset: 0x0000480C
	private void OnDisable()
	{
		FingerGestures.OnPinchMove -= this.FingerGestures_OnPinchMove;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00006620 File Offset: 0x00004820
	private void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
		this.ZoomAmount += this.zoomSpeed * delta;
	}

	// Token: 0x0400008C RID: 140
	public TBPinchZoom.ZoomMethod zoomMethod;

	// Token: 0x0400008D RID: 141
	public float zoomSpeed = 1.5f;

	// Token: 0x0400008E RID: 142
	public float minZoomAmount;

	// Token: 0x0400008F RID: 143
	public float maxZoomAmount = 50f;

	// Token: 0x04000090 RID: 144
	private Vector3 defaultPos = Vector3.zero;

	// Token: 0x04000091 RID: 145
	private float defaultFov;

	// Token: 0x04000092 RID: 146
	private float defaultOrthoSize;

	// Token: 0x04000093 RID: 147
	private float zoomAmount;

	// Token: 0x02000017 RID: 23
	public enum ZoomMethod
	{
		// Token: 0x04000095 RID: 149
		Position,
		// Token: 0x04000096 RID: 150
		FOV
	}
}
