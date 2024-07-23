using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
[AddComponentMenu("FingerGestures/Toolbox/Misc/DragOrbit")]
public class TBDragOrbit : MonoBehaviour
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005FE4 File Offset: 0x000041E4
	public float Distance
	{
		get
		{
			return this.distance;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005FEC File Offset: 0x000041EC
	// (set) Token: 0x060000EA RID: 234 RVA: 0x00005FF4 File Offset: 0x000041F4
	public float IdealDistance
	{
		get
		{
			return this.idealDistance;
		}
		set
		{
			this.idealDistance = Mathf.Clamp(value, this.minDistance, this.maxDistance);
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060000EB RID: 235 RVA: 0x00006010 File Offset: 0x00004210
	public float Yaw
	{
		get
		{
			return this.yaw;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060000EC RID: 236 RVA: 0x00006018 File Offset: 0x00004218
	// (set) Token: 0x060000ED RID: 237 RVA: 0x00006020 File Offset: 0x00004220
	public float IdealYaw
	{
		get
		{
			return this.idealYaw;
		}
		set
		{
			this.idealYaw = value;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060000EE RID: 238 RVA: 0x0000602C File Offset: 0x0000422C
	public float Pitch
	{
		get
		{
			return this.pitch;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060000EF RID: 239 RVA: 0x00006034 File Offset: 0x00004234
	// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000603C File Offset: 0x0000423C
	public float IdealPitch
	{
		get
		{
			return this.idealPitch;
		}
		set
		{
			this.idealPitch = ((!this.clampPitchAngle) ? value : TBDragOrbit.ClampAngle(value, this.minPitch, this.maxPitch));
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060000F1 RID: 241 RVA: 0x00006068 File Offset: 0x00004268
	// (set) Token: 0x060000F2 RID: 242 RVA: 0x00006070 File Offset: 0x00004270
	public Vector3 IdealPanOffset
	{
		get
		{
			return this.idealPanOffset;
		}
		set
		{
			this.idealPanOffset = value;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000607C File Offset: 0x0000427C
	public Vector3 PanOffset
	{
		get
		{
			return this.panOffset;
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00006084 File Offset: 0x00004284
	private void Start()
	{
		if (!this.panningPlane)
		{
			this.panningPlane = base.transform;
		}
		Vector3 eulerAngles = base.transform.eulerAngles;
		float num = this.initialDistance;
		this.IdealDistance = num;
		this.distance = num;
		num = eulerAngles.y;
		this.IdealYaw = num;
		this.yaw = num;
		num = eulerAngles.x;
		this.IdealPitch = num;
		this.pitch = num;
		if (base.rigidbody)
		{
			base.rigidbody.freezeRotation = true;
		}
		this.Apply();
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000611C File Offset: 0x0000431C
	private void OnEnable()
	{
		FingerGestures.OnDragMove += this.FingerGestures_OnDragMove;
		FingerGestures.OnPinchMove += this.FingerGestures_OnPinchMove;
		FingerGestures.OnTwoFingerDragMove += this.FingerGestures_OnTwoFingerDragMove;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006154 File Offset: 0x00004354
	private void OnDisable()
	{
		FingerGestures.OnDragMove -= this.FingerGestures_OnDragMove;
		FingerGestures.OnPinchMove -= this.FingerGestures_OnPinchMove;
		FingerGestures.OnTwoFingerDragMove -= this.FingerGestures_OnTwoFingerDragMove;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000618C File Offset: 0x0000438C
	private void FingerGestures_OnDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (Time.time - this.lastPanTime < 0.25f)
		{
			return;
		}
		if (this.target)
		{
			this.IdealYaw += delta.x * this.yawSensitivity * 0.02f;
			this.IdealPitch -= delta.y * this.pitchSensitivity * 0.02f;
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006204 File Offset: 0x00004404
	private void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
		if (this.allowPinchZoom)
		{
			this.IdealDistance -= delta * this.pinchZoomSensitivity;
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00006234 File Offset: 0x00004434
	private void FingerGestures_OnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (this.allowPanning)
		{
			Vector3 b = -0.02f * this.panningSensitivity * (this.panningPlane.right * delta.x + this.panningPlane.up * delta.y);
			if (this.invertPanningDirections)
			{
				this.IdealPanOffset -= b;
			}
			else
			{
				this.IdealPanOffset += b;
			}
			this.lastPanTime = Time.time;
		}
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000062D0 File Offset: 0x000044D0
	private void Apply()
	{
		if (this.smoothMotion)
		{
			this.distance = Mathf.Lerp(this.distance, this.IdealDistance, Time.deltaTime * this.smoothZoomSpeed);
			this.yaw = Mathf.Lerp(this.yaw, this.IdealYaw, Time.deltaTime * this.smoothOrbitSpeed);
			this.pitch = Mathf.Lerp(this.pitch, this.IdealPitch, Time.deltaTime * this.smoothOrbitSpeed);
		}
		else
		{
			this.distance = this.IdealDistance;
			this.yaw = this.IdealYaw;
			this.pitch = this.IdealPitch;
		}
		if (this.smoothPanning)
		{
			this.panOffset = Vector3.Lerp(this.panOffset, this.idealPanOffset, Time.deltaTime * this.smoothPanningSpeed);
		}
		else
		{
			this.panOffset = this.idealPanOffset;
		}
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
		base.transform.position = this.target.position + this.panOffset - this.distance * base.transform.forward;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006418 File Offset: 0x00004618
	private void LateUpdate()
	{
		this.Apply();
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006420 File Offset: 0x00004620
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00006460 File Offset: 0x00004660
	public void ResetPanning()
	{
		this.IdealPanOffset = Vector3.zero;
	}

	// Token: 0x0400006B RID: 107
	public Transform target;

	// Token: 0x0400006C RID: 108
	public float initialDistance = 10f;

	// Token: 0x0400006D RID: 109
	public float minDistance = 1f;

	// Token: 0x0400006E RID: 110
	public float maxDistance = 20f;

	// Token: 0x0400006F RID: 111
	public float yawSensitivity = 80f;

	// Token: 0x04000070 RID: 112
	public float pitchSensitivity = 80f;

	// Token: 0x04000071 RID: 113
	public bool clampPitchAngle = true;

	// Token: 0x04000072 RID: 114
	public float minPitch = -20f;

	// Token: 0x04000073 RID: 115
	public float maxPitch = 80f;

	// Token: 0x04000074 RID: 116
	public bool allowPinchZoom = true;

	// Token: 0x04000075 RID: 117
	public float pinchZoomSensitivity = 2f;

	// Token: 0x04000076 RID: 118
	public bool smoothMotion = true;

	// Token: 0x04000077 RID: 119
	public float smoothZoomSpeed = 3f;

	// Token: 0x04000078 RID: 120
	public float smoothOrbitSpeed = 4f;

	// Token: 0x04000079 RID: 121
	public bool allowPanning;

	// Token: 0x0400007A RID: 122
	public bool invertPanningDirections;

	// Token: 0x0400007B RID: 123
	public float panningSensitivity = 1f;

	// Token: 0x0400007C RID: 124
	public Transform panningPlane;

	// Token: 0x0400007D RID: 125
	public bool smoothPanning = true;

	// Token: 0x0400007E RID: 126
	public float smoothPanningSpeed = 8f;

	// Token: 0x0400007F RID: 127
	private float lastPanTime;

	// Token: 0x04000080 RID: 128
	private float distance = 10f;

	// Token: 0x04000081 RID: 129
	private float yaw;

	// Token: 0x04000082 RID: 130
	private float pitch;

	// Token: 0x04000083 RID: 131
	private float idealDistance;

	// Token: 0x04000084 RID: 132
	private float idealYaw;

	// Token: 0x04000085 RID: 133
	private float idealPitch;

	// Token: 0x04000086 RID: 134
	private Vector3 idealPanOffset = Vector3.zero;

	// Token: 0x04000087 RID: 135
	private Vector3 panOffset = Vector3.zero;

	// Token: 0x02000015 RID: 21
	public enum PanMode
	{
		// Token: 0x04000089 RID: 137
		Disabled,
		// Token: 0x0400008A RID: 138
		OneFinger,
		// Token: 0x0400008B RID: 139
		TwoFingers
	}
}
