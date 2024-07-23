using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class CSharpSkeleton : MonoBehaviour
{
	// Token: 0x060000C3 RID: 195 RVA: 0x000059E8 File Offset: 0x00003BE8
	private void OnEnable()
	{
		FingerGestures.OnFingerDown += this.FingerGestures_OnFingerDown;
		FingerGestures.OnFingerStationaryBegin += this.FingerGestures_OnFingerStationaryBegin;
		FingerGestures.OnFingerStationary += this.FingerGestures_OnFingerStationary;
		FingerGestures.OnFingerStationaryEnd += this.FingerGestures_OnFingerStationaryEnd;
		FingerGestures.OnFingerMoveBegin += this.FingerGestures_OnFingerMoveBegin;
		FingerGestures.OnFingerMove += this.FingerGestures_OnFingerMove;
		FingerGestures.OnFingerMoveEnd += this.FingerGestures_OnFingerMoveEnd;
		FingerGestures.OnFingerUp += this.FingerGestures_OnFingerUp;
		FingerGestures.OnFingerLongPress += this.FingerGestures_OnFingerLongPress;
		FingerGestures.OnFingerTap += this.FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDoubleTap += this.FingerGestures_OnFingerDoubleTap;
		FingerGestures.OnFingerSwipe += this.FingerGestures_OnFingerSwipe;
		FingerGestures.OnFingerDragBegin += this.FingerGestures_OnFingerDragBegin;
		FingerGestures.OnFingerDragMove += this.FingerGestures_OnFingerDragMove;
		FingerGestures.OnFingerDragEnd += this.FingerGestures_OnFingerDragEnd;
		FingerGestures.OnLongPress += this.FingerGestures_OnLongPress;
		FingerGestures.OnTap += this.FingerGestures_OnTap;
		FingerGestures.OnDoubleTap += this.FingerGestures_OnDoubleTap;
		FingerGestures.OnSwipe += this.FingerGestures_OnSwipe;
		FingerGestures.OnDragBegin += this.FingerGestures_OnDragBegin;
		FingerGestures.OnDragMove += this.FingerGestures_OnDragMove;
		FingerGestures.OnDragEnd += this.FingerGestures_OnDragEnd;
		FingerGestures.OnPinchBegin += this.FingerGestures_OnPinchBegin;
		FingerGestures.OnPinchMove += this.FingerGestures_OnPinchMove;
		FingerGestures.OnPinchEnd += this.FingerGestures_OnPinchEnd;
		FingerGestures.OnRotationBegin += this.FingerGestures_OnRotationBegin;
		FingerGestures.OnRotationMove += this.FingerGestures_OnRotationMove;
		FingerGestures.OnRotationEnd += this.FingerGestures_OnRotationEnd;
		FingerGestures.OnTwoFingerLongPress += this.FingerGestures_OnTwoFingerLongPress;
		FingerGestures.OnTwoFingerTap += this.FingerGestures_OnTwoFingerTap;
		FingerGestures.OnTwoFingerSwipe += this.FingerGestures_OnTwoFingerSwipe;
		FingerGestures.OnTwoFingerDragBegin += this.FingerGestures_OnTwoFingerDragBegin;
		FingerGestures.OnTwoFingerDragMove += this.FingerGestures_OnTwoFingerDragMove;
		FingerGestures.OnTwoFingerDragEnd += this.FingerGestures_OnTwoFingerDragEnd;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00005C38 File Offset: 0x00003E38
	private void OnDisable()
	{
		FingerGestures.OnFingerDown -= this.FingerGestures_OnFingerDown;
		FingerGestures.OnFingerStationaryBegin -= this.FingerGestures_OnFingerStationaryBegin;
		FingerGestures.OnFingerStationary -= this.FingerGestures_OnFingerStationary;
		FingerGestures.OnFingerStationaryEnd -= this.FingerGestures_OnFingerStationaryEnd;
		FingerGestures.OnFingerMoveBegin -= this.FingerGestures_OnFingerMoveBegin;
		FingerGestures.OnFingerMove -= this.FingerGestures_OnFingerMove;
		FingerGestures.OnFingerMoveEnd -= this.FingerGestures_OnFingerMoveEnd;
		FingerGestures.OnFingerUp -= this.FingerGestures_OnFingerUp;
		FingerGestures.OnFingerLongPress -= this.FingerGestures_OnFingerLongPress;
		FingerGestures.OnFingerTap -= this.FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDoubleTap -= this.FingerGestures_OnFingerDoubleTap;
		FingerGestures.OnFingerSwipe -= this.FingerGestures_OnFingerSwipe;
		FingerGestures.OnFingerDragBegin -= this.FingerGestures_OnFingerDragBegin;
		FingerGestures.OnFingerDragMove -= this.FingerGestures_OnFingerDragMove;
		FingerGestures.OnFingerDragEnd -= this.FingerGestures_OnFingerDragEnd;
		FingerGestures.OnLongPress -= this.FingerGestures_OnLongPress;
		FingerGestures.OnTap -= this.FingerGestures_OnTap;
		FingerGestures.OnDoubleTap -= this.FingerGestures_OnDoubleTap;
		FingerGestures.OnSwipe -= this.FingerGestures_OnSwipe;
		FingerGestures.OnDragBegin -= this.FingerGestures_OnDragBegin;
		FingerGestures.OnDragMove -= this.FingerGestures_OnDragMove;
		FingerGestures.OnDragEnd -= this.FingerGestures_OnDragEnd;
		FingerGestures.OnPinchBegin -= this.FingerGestures_OnPinchBegin;
		FingerGestures.OnPinchMove -= this.FingerGestures_OnPinchMove;
		FingerGestures.OnPinchEnd -= this.FingerGestures_OnPinchEnd;
		FingerGestures.OnRotationBegin -= this.FingerGestures_OnRotationBegin;
		FingerGestures.OnRotationMove -= this.FingerGestures_OnRotationMove;
		FingerGestures.OnRotationEnd -= this.FingerGestures_OnRotationEnd;
		FingerGestures.OnTwoFingerLongPress -= this.FingerGestures_OnTwoFingerLongPress;
		FingerGestures.OnTwoFingerTap -= this.FingerGestures_OnTwoFingerTap;
		FingerGestures.OnTwoFingerSwipe -= this.FingerGestures_OnTwoFingerSwipe;
		FingerGestures.OnTwoFingerDragBegin -= this.FingerGestures_OnTwoFingerDragBegin;
		FingerGestures.OnTwoFingerDragMove -= this.FingerGestures_OnTwoFingerDragMove;
		FingerGestures.OnTwoFingerDragEnd -= this.FingerGestures_OnTwoFingerDragEnd;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005E88 File Offset: 0x00004088
	private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00005E8C File Offset: 0x0000408C
	private void FingerGestures_OnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005E90 File Offset: 0x00004090
	private void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005E94 File Offset: 0x00004094
	private void FingerGestures_OnFingerMoveEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005E98 File Offset: 0x00004098
	private void FingerGestures_OnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005E9C File Offset: 0x0000409C
	private void FingerGestures_OnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005EA0 File Offset: 0x000040A0
	private void FingerGestures_OnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005EA4 File Offset: 0x000040A4
	private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005EA8 File Offset: 0x000040A8
	private void FingerGestures_OnFingerLongPress(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005EAC File Offset: 0x000040AC
	private void FingerGestures_OnFingerTap(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005EB0 File Offset: 0x000040B0
	private void FingerGestures_OnFingerDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005EB4 File Offset: 0x000040B4
	private void FingerGestures_OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005EB8 File Offset: 0x000040B8
	private void FingerGestures_OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005EBC File Offset: 0x000040BC
	private void FingerGestures_OnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005EC0 File Offset: 0x000040C0
	private void FingerGestures_OnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005EC4 File Offset: 0x000040C4
	private void FingerGestures_OnLongPress(Vector2 fingerPos)
	{
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00005EC8 File Offset: 0x000040C8
	private void FingerGestures_OnTap(Vector2 fingerPos)
	{
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005ECC File Offset: 0x000040CC
	private void FingerGestures_OnDoubleTap(Vector2 fingerPos)
	{
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00005ED0 File Offset: 0x000040D0
	private void FingerGestures_OnSwipe(Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00005ED4 File Offset: 0x000040D4
	private void FingerGestures_OnDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00005ED8 File Offset: 0x000040D8
	private void FingerGestures_OnDragMove(Vector2 fingerPos, Vector2 delta)
	{
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00005EDC File Offset: 0x000040DC
	private void FingerGestures_OnDragEnd(Vector2 fingerPos)
	{
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00005EE0 File Offset: 0x000040E0
	private void FingerGestures_OnPinchBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00005EE4 File Offset: 0x000040E4
	private void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00005EE8 File Offset: 0x000040E8
	private void FingerGestures_OnPinchEnd(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00005EEC File Offset: 0x000040EC
	private void FingerGestures_OnRotationBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00005EF0 File Offset: 0x000040F0
	private void FingerGestures_OnRotationMove(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta)
	{
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00005EF4 File Offset: 0x000040F4
	private void FingerGestures_OnRotationEnd(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle)
	{
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00005EF8 File Offset: 0x000040F8
	private void FingerGestures_OnTwoFingerLongPress(Vector2 fingerPos)
	{
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00005EFC File Offset: 0x000040FC
	private void FingerGestures_OnTwoFingerTap(Vector2 fingerPos)
	{
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00005F00 File Offset: 0x00004100
	private void FingerGestures_OnTwoFingerSwipe(Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00005F04 File Offset: 0x00004104
	private void FingerGestures_OnTwoFingerDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00005F08 File Offset: 0x00004108
	private void FingerGestures_OnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
	{
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00005F0C File Offset: 0x0000410C
	private void FingerGestures_OnTwoFingerDragEnd(Vector2 fingerPos)
	{
	}
}
