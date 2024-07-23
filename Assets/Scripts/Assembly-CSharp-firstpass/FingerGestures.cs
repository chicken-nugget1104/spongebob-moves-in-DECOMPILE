using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000083 RID: 131
public abstract class FingerGestures : MonoBehaviour
{
	// Token: 0x1400003F RID: 63
	// (add) Token: 0x06000498 RID: 1176 RVA: 0x00012EDC File Offset: 0x000110DC
	// (remove) Token: 0x06000499 RID: 1177 RVA: 0x00012EF4 File Offset: 0x000110F4
	public static event FingerGestures.FingerDownEventHandler OnFingerDown;

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x0600049A RID: 1178 RVA: 0x00012F0C File Offset: 0x0001110C
	// (remove) Token: 0x0600049B RID: 1179 RVA: 0x00012F24 File Offset: 0x00011124
	public static event FingerGestures.FingerUpEventHandler OnFingerUp;

	// Token: 0x14000041 RID: 65
	// (add) Token: 0x0600049C RID: 1180 RVA: 0x00012F3C File Offset: 0x0001113C
	// (remove) Token: 0x0600049D RID: 1181 RVA: 0x00012F54 File Offset: 0x00011154
	public static event FingerGestures.FingerStationaryBeginEventHandler OnFingerStationaryBegin;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x0600049E RID: 1182 RVA: 0x00012F6C File Offset: 0x0001116C
	// (remove) Token: 0x0600049F RID: 1183 RVA: 0x00012F84 File Offset: 0x00011184
	public static event FingerGestures.FingerStationaryEventHandler OnFingerStationary;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x060004A0 RID: 1184 RVA: 0x00012F9C File Offset: 0x0001119C
	// (remove) Token: 0x060004A1 RID: 1185 RVA: 0x00012FB4 File Offset: 0x000111B4
	public static event FingerGestures.FingerStationaryEndEventHandler OnFingerStationaryEnd;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x060004A2 RID: 1186 RVA: 0x00012FCC File Offset: 0x000111CC
	// (remove) Token: 0x060004A3 RID: 1187 RVA: 0x00012FE4 File Offset: 0x000111E4
	public static event FingerGestures.FingerMoveEventHandler OnFingerMoveBegin;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x060004A4 RID: 1188 RVA: 0x00012FFC File Offset: 0x000111FC
	// (remove) Token: 0x060004A5 RID: 1189 RVA: 0x00013014 File Offset: 0x00011214
	public static event FingerGestures.FingerMoveEventHandler OnFingerMove;

	// Token: 0x14000046 RID: 70
	// (add) Token: 0x060004A6 RID: 1190 RVA: 0x0001302C File Offset: 0x0001122C
	// (remove) Token: 0x060004A7 RID: 1191 RVA: 0x00013044 File Offset: 0x00011244
	public static event FingerGestures.FingerMoveEventHandler OnFingerMoveEnd;

	// Token: 0x14000047 RID: 71
	// (add) Token: 0x060004A8 RID: 1192 RVA: 0x0001305C File Offset: 0x0001125C
	// (remove) Token: 0x060004A9 RID: 1193 RVA: 0x00013074 File Offset: 0x00011274
	public static event FingerGestures.FingerLongPressEventHandler OnFingerLongPress;

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x060004AA RID: 1194 RVA: 0x0001308C File Offset: 0x0001128C
	// (remove) Token: 0x060004AB RID: 1195 RVA: 0x000130A4 File Offset: 0x000112A4
	public static event FingerGestures.FingerDragBeginEventHandler OnFingerDragBegin;

	// Token: 0x14000049 RID: 73
	// (add) Token: 0x060004AC RID: 1196 RVA: 0x000130BC File Offset: 0x000112BC
	// (remove) Token: 0x060004AD RID: 1197 RVA: 0x000130D4 File Offset: 0x000112D4
	public static event FingerGestures.FingerDragMoveEventHandler OnFingerDragMove;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x060004AE RID: 1198 RVA: 0x000130EC File Offset: 0x000112EC
	// (remove) Token: 0x060004AF RID: 1199 RVA: 0x00013104 File Offset: 0x00011304
	public static event FingerGestures.FingerDragEndEventHandler OnFingerDragStationary;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060004B0 RID: 1200 RVA: 0x0001311C File Offset: 0x0001131C
	// (remove) Token: 0x060004B1 RID: 1201 RVA: 0x00013134 File Offset: 0x00011334
	public static event FingerGestures.FingerDragEndEventHandler OnFingerDragEnd;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x060004B2 RID: 1202 RVA: 0x0001314C File Offset: 0x0001134C
	// (remove) Token: 0x060004B3 RID: 1203 RVA: 0x00013164 File Offset: 0x00011364
	public static event FingerGestures.FingerTapEventHandler OnFingerTap;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x060004B4 RID: 1204 RVA: 0x0001317C File Offset: 0x0001137C
	// (remove) Token: 0x060004B5 RID: 1205 RVA: 0x00013194 File Offset: 0x00011394
	public static event FingerGestures.FingerTapEventHandler OnFingerDoubleTap;

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x060004B6 RID: 1206 RVA: 0x000131AC File Offset: 0x000113AC
	// (remove) Token: 0x060004B7 RID: 1207 RVA: 0x000131C4 File Offset: 0x000113C4
	public static event FingerGestures.FingerSwipeEventHandler OnFingerSwipe;

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x060004B8 RID: 1208 RVA: 0x000131DC File Offset: 0x000113DC
	// (remove) Token: 0x060004B9 RID: 1209 RVA: 0x000131F4 File Offset: 0x000113F4
	public static event FingerGestures.LongPressEventHandler OnLongPress;

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x060004BA RID: 1210 RVA: 0x0001320C File Offset: 0x0001140C
	// (remove) Token: 0x060004BB RID: 1211 RVA: 0x00013224 File Offset: 0x00011424
	public static event FingerGestures.DragBeginEventHandler OnDragBegin;

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x060004BC RID: 1212 RVA: 0x0001323C File Offset: 0x0001143C
	// (remove) Token: 0x060004BD RID: 1213 RVA: 0x00013254 File Offset: 0x00011454
	public static event FingerGestures.DragMoveEventHandler OnDragMove;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x060004BE RID: 1214 RVA: 0x0001326C File Offset: 0x0001146C
	// (remove) Token: 0x060004BF RID: 1215 RVA: 0x00013284 File Offset: 0x00011484
	public static event FingerGestures.DragEndEventHandler OnDragStationary;

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x060004C0 RID: 1216 RVA: 0x0001329C File Offset: 0x0001149C
	// (remove) Token: 0x060004C1 RID: 1217 RVA: 0x000132B4 File Offset: 0x000114B4
	public static event FingerGestures.DragEndEventHandler OnDragEnd;

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x060004C2 RID: 1218 RVA: 0x000132CC File Offset: 0x000114CC
	// (remove) Token: 0x060004C3 RID: 1219 RVA: 0x000132E4 File Offset: 0x000114E4
	public static event FingerGestures.TapEventHandler OnTap;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x060004C4 RID: 1220 RVA: 0x000132FC File Offset: 0x000114FC
	// (remove) Token: 0x060004C5 RID: 1221 RVA: 0x00013314 File Offset: 0x00011514
	public static event FingerGestures.TapEventHandler OnDoubleTap;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x060004C6 RID: 1222 RVA: 0x0001332C File Offset: 0x0001152C
	// (remove) Token: 0x060004C7 RID: 1223 RVA: 0x00013344 File Offset: 0x00011544
	public static event FingerGestures.SwipeEventHandler OnSwipe;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x060004C8 RID: 1224 RVA: 0x0001335C File Offset: 0x0001155C
	// (remove) Token: 0x060004C9 RID: 1225 RVA: 0x00013374 File Offset: 0x00011574
	public static event FingerGestures.PinchEventHandler OnPinchBegin;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x060004CA RID: 1226 RVA: 0x0001338C File Offset: 0x0001158C
	// (remove) Token: 0x060004CB RID: 1227 RVA: 0x000133A4 File Offset: 0x000115A4
	public static event FingerGestures.PinchMoveEventHandler OnPinchMove;

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x060004CC RID: 1228 RVA: 0x000133BC File Offset: 0x000115BC
	// (remove) Token: 0x060004CD RID: 1229 RVA: 0x000133D4 File Offset: 0x000115D4
	public static event FingerGestures.PinchEventHandler OnPinchEnd;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x060004CE RID: 1230 RVA: 0x000133EC File Offset: 0x000115EC
	// (remove) Token: 0x060004CF RID: 1231 RVA: 0x00013404 File Offset: 0x00011604
	public static event FingerGestures.RotationBeginEventHandler OnRotationBegin;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x060004D0 RID: 1232 RVA: 0x0001341C File Offset: 0x0001161C
	// (remove) Token: 0x060004D1 RID: 1233 RVA: 0x00013434 File Offset: 0x00011634
	public static event FingerGestures.RotationMoveEventHandler OnRotationMove;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x060004D2 RID: 1234 RVA: 0x0001344C File Offset: 0x0001164C
	// (remove) Token: 0x060004D3 RID: 1235 RVA: 0x00013464 File Offset: 0x00011664
	public static event FingerGestures.RotationEndEventHandler OnRotationEnd;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x060004D4 RID: 1236 RVA: 0x0001347C File Offset: 0x0001167C
	// (remove) Token: 0x060004D5 RID: 1237 RVA: 0x00013494 File Offset: 0x00011694
	public static event FingerGestures.DragBeginEventHandler OnTwoFingerDragBegin;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x060004D6 RID: 1238 RVA: 0x000134AC File Offset: 0x000116AC
	// (remove) Token: 0x060004D7 RID: 1239 RVA: 0x000134C4 File Offset: 0x000116C4
	public static event FingerGestures.DragMoveEventHandler OnTwoFingerDragMove;

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x060004D8 RID: 1240 RVA: 0x000134DC File Offset: 0x000116DC
	// (remove) Token: 0x060004D9 RID: 1241 RVA: 0x000134F4 File Offset: 0x000116F4
	public static event FingerGestures.DragEndEventHandler OnTwoFingerDragStationary;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x060004DA RID: 1242 RVA: 0x0001350C File Offset: 0x0001170C
	// (remove) Token: 0x060004DB RID: 1243 RVA: 0x00013524 File Offset: 0x00011724
	public static event FingerGestures.DragEndEventHandler OnTwoFingerDragEnd;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x060004DC RID: 1244 RVA: 0x0001353C File Offset: 0x0001173C
	// (remove) Token: 0x060004DD RID: 1245 RVA: 0x00013554 File Offset: 0x00011754
	public static event FingerGestures.TapEventHandler OnTwoFingerTap;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x060004DE RID: 1246 RVA: 0x0001356C File Offset: 0x0001176C
	// (remove) Token: 0x060004DF RID: 1247 RVA: 0x00013584 File Offset: 0x00011784
	public static event FingerGestures.SwipeEventHandler OnTwoFingerSwipe;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x060004E0 RID: 1248 RVA: 0x0001359C File Offset: 0x0001179C
	// (remove) Token: 0x060004E1 RID: 1249 RVA: 0x000135B4 File Offset: 0x000117B4
	public static event FingerGestures.LongPressEventHandler OnTwoFingerLongPress;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x060004E2 RID: 1250 RVA: 0x000135CC File Offset: 0x000117CC
	// (remove) Token: 0x060004E3 RID: 1251 RVA: 0x000135E4 File Offset: 0x000117E4
	public static event FingerGestures.FingersUpdatedEventDelegate OnFingersUpdated;

	// Token: 0x060004E4 RID: 1252 RVA: 0x000135FC File Offset: 0x000117FC
	internal static void RaiseOnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDown != null)
		{
			FingerGestures.OnFingerDown(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00013614 File Offset: 0x00011814
	internal static void RaiseOnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		if (FingerGestures.OnFingerUp != null)
		{
			FingerGestures.OnFingerUp(fingerIndex, fingerPos, timeHeldDown);
		}
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00013630 File Offset: 0x00011830
	internal static void RaiseOnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerStationaryBegin != null)
		{
			FingerGestures.OnFingerStationaryBegin(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00013648 File Offset: 0x00011848
	internal static void RaiseOnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		if (FingerGestures.OnFingerStationary != null)
		{
			FingerGestures.OnFingerStationary(fingerIndex, fingerPos, elapsedTime);
		}
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00013664 File Offset: 0x00011864
	internal static void RaiseOnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		if (FingerGestures.OnFingerStationaryEnd != null)
		{
			FingerGestures.OnFingerStationaryEnd(fingerIndex, fingerPos, elapsedTime);
		}
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00013680 File Offset: 0x00011880
	internal static void RaiseOnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerMoveBegin != null)
		{
			FingerGestures.OnFingerMoveBegin(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00013698 File Offset: 0x00011898
	internal static void RaiseOnFingerMove(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerMove != null)
		{
			FingerGestures.OnFingerMove(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x000136B0 File Offset: 0x000118B0
	internal static void RaiseOnFingerMoveEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerMoveEnd != null)
		{
			FingerGestures.OnFingerMoveEnd(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x000136C8 File Offset: 0x000118C8
	internal static void RaiseOnFingerLongPress(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerLongPress != null)
		{
			FingerGestures.OnFingerLongPress(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x000136E0 File Offset: 0x000118E0
	internal static void RaiseOnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		if (FingerGestures.OnFingerDragBegin != null)
		{
			FingerGestures.OnFingerDragBegin(fingerIndex, fingerPos, startPos);
		}
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x000136FC File Offset: 0x000118FC
	internal static void RaiseOnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		if (FingerGestures.OnFingerDragMove != null)
		{
			FingerGestures.OnFingerDragMove(fingerIndex, fingerPos, delta);
		}
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00013718 File Offset: 0x00011918
	internal static void RaiseOnFingerDragStationary(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDragStationary != null)
		{
			FingerGestures.OnFingerDragStationary(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00013730 File Offset: 0x00011930
	internal static void RaiseOnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDragEnd != null)
		{
			FingerGestures.OnFingerDragEnd(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00013748 File Offset: 0x00011948
	internal static void RaiseOnFingerTap(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerTap != null)
		{
			FingerGestures.OnFingerTap(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00013760 File Offset: 0x00011960
	internal static void RaiseOnFingerDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDoubleTap != null)
		{
			FingerGestures.OnFingerDoubleTap(fingerIndex, fingerPos);
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00013778 File Offset: 0x00011978
	internal static void RaiseOnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if (FingerGestures.OnFingerSwipe != null)
		{
			FingerGestures.OnFingerSwipe(fingerIndex, startPos, direction, velocity);
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00013794 File Offset: 0x00011994
	internal static void RaiseOnLongPress(Vector2 fingerPos)
	{
		if (FingerGestures.OnLongPress != null)
		{
			FingerGestures.OnLongPress(fingerPos);
		}
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x000137AC File Offset: 0x000119AC
	internal static void RaiseOnDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
		if (FingerGestures.OnDragBegin != null)
		{
			FingerGestures.OnDragBegin(fingerPos, startPos);
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x000137C4 File Offset: 0x000119C4
	internal static void RaiseOnDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (FingerGestures.OnDragMove != null)
		{
			FingerGestures.OnDragMove(fingerPos, delta);
		}
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x000137DC File Offset: 0x000119DC
	internal static void RaiseOnDragEnd(Vector2 fingerPos)
	{
		if (FingerGestures.OnDragEnd != null)
		{
			FingerGestures.OnDragEnd(fingerPos);
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000137F4 File Offset: 0x000119F4
	internal static void RaiseOnDragStationary(Vector2 fingerPos)
	{
		if (FingerGestures.OnDragStationary != null)
		{
			FingerGestures.OnDragStationary(fingerPos);
		}
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0001380C File Offset: 0x00011A0C
	internal static void RaiseOnTap(Vector2 fingerPos)
	{
		if (FingerGestures.OnTap != null)
		{
			FingerGestures.OnTap(fingerPos);
		}
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00013824 File Offset: 0x00011A24
	internal static void RaiseOnDoubleTap(Vector2 fingerPos)
	{
		if (FingerGestures.OnDoubleTap != null)
		{
			FingerGestures.OnDoubleTap(fingerPos);
		}
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001383C File Offset: 0x00011A3C
	internal static void RaiseOnSwipe(Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if (FingerGestures.OnSwipe != null)
		{
			FingerGestures.OnSwipe(startPos, direction, velocity);
		}
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x00013858 File Offset: 0x00011A58
	internal static void RaiseOnPinchBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
		if (FingerGestures.OnPinchBegin != null)
		{
			FingerGestures.OnPinchBegin(fingerPos1, fingerPos2);
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00013870 File Offset: 0x00011A70
	internal static void RaiseOnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
		if (FingerGestures.OnPinchMove != null)
		{
			FingerGestures.OnPinchMove(fingerPos1, fingerPos2, delta);
		}
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0001388C File Offset: 0x00011A8C
	internal static void RaiseOnPinchEnd(Vector2 fingerPos1, Vector2 fingerPos2)
	{
		if (FingerGestures.OnPinchEnd != null)
		{
			FingerGestures.OnPinchEnd(fingerPos1, fingerPos2);
		}
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000138A4 File Offset: 0x00011AA4
	internal static void RaiseOnRotationBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
		if (FingerGestures.OnRotationBegin != null)
		{
			FingerGestures.OnRotationBegin(fingerPos1, fingerPos2);
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000138BC File Offset: 0x00011ABC
	internal static void RaiseOnRotationMove(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta)
	{
		if (FingerGestures.OnRotationMove != null)
		{
			FingerGestures.OnRotationMove(fingerPos1, fingerPos2, rotationAngleDelta);
		}
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000138D8 File Offset: 0x00011AD8
	internal static void RaiseOnRotationEnd(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle)
	{
		if (FingerGestures.OnRotationEnd != null)
		{
			FingerGestures.OnRotationEnd(fingerPos1, fingerPos2, totalRotationAngle);
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x000138F4 File Offset: 0x00011AF4
	internal static void RaiseOnTwoFingerLongPress(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerLongPress != null)
		{
			FingerGestures.OnTwoFingerLongPress(fingerPos);
		}
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0001390C File Offset: 0x00011B0C
	internal static void RaiseOnTwoFingerDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
		if (FingerGestures.OnTwoFingerDragBegin != null)
		{
			FingerGestures.OnTwoFingerDragBegin(fingerPos, startPos);
		}
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00013924 File Offset: 0x00011B24
	internal static void RaiseOnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (FingerGestures.OnTwoFingerDragMove != null)
		{
			FingerGestures.OnTwoFingerDragMove(fingerPos, delta);
		}
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x0001393C File Offset: 0x00011B3C
	internal static void RaiseOnTwoFingerDragStationary(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerDragStationary != null)
		{
			FingerGestures.OnTwoFingerDragStationary(fingerPos);
		}
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00013954 File Offset: 0x00011B54
	internal static void RaiseOnTwoFingerDragEnd(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerDragEnd != null)
		{
			FingerGestures.OnTwoFingerDragEnd(fingerPos);
		}
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0001396C File Offset: 0x00011B6C
	internal static void RaiseOnTwoFingerTap(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerTap != null)
		{
			FingerGestures.OnTwoFingerTap(fingerPos);
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00013984 File Offset: 0x00011B84
	internal static void RaiseOnTwoFingerSwipe(Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if (FingerGestures.OnTwoFingerSwipe != null)
		{
			FingerGestures.OnTwoFingerSwipe(startPos, direction, velocity);
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000509 RID: 1289 RVA: 0x000139A0 File Offset: 0x00011BA0
	public static FingerGestures Instance
	{
		get
		{
			return FingerGestures.instance;
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x000139A8 File Offset: 0x00011BA8
	public static FingerGestures.Finger GetFinger(int index)
	{
		return FingerGestures.instance.fingers[index];
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600050B RID: 1291 RVA: 0x000139B8 File Offset: 0x00011BB8
	public static FingerGestures.IFingerList Touches
	{
		get
		{
			if (FingerGestures.instance == null)
			{
				Debug.LogError("Null FG instance!");
			}
			if (FingerGestures.instance.touches == null)
			{
				Debug.LogError("Null instance.touches!");
			}
			return FingerGestures.instance.touches;
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x000139F8 File Offset: 0x00011BF8
	protected virtual void Awake()
	{
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x000139FC File Offset: 0x00011BFC
	protected virtual void OnEnable()
	{
		if (FingerGestures.instance == null)
		{
			FingerGestures.instance = this;
			this.InitFingers(this.MaxFingers);
		}
		else if (FingerGestures.instance != this)
		{
			if (FingerGestures.loggingGestures)
			{
				Debug.LogWarning("There is already an instance of FingerGestures created (" + FingerGestures.instance.name + "). Destroying new one.");
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00013A74 File Offset: 0x00011C74
	protected virtual void Start()
	{
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00013A78 File Offset: 0x00011C78
	protected virtual void OnDisable()
	{
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00013A7C File Offset: 0x00011C7C
	protected virtual void Update()
	{
		this.UpdateFingers();
		if (FingerGestures.OnFingersUpdated != null)
		{
			FingerGestures.OnFingersUpdated();
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000511 RID: 1297
	public abstract int MaxFingers { get; }

	// Token: 0x06000512 RID: 1298
	protected abstract FingerGestures.FingerPhase GetPhase(FingerGestures.Finger finger);

	// Token: 0x06000513 RID: 1299
	protected abstract Vector2 GetPosition(FingerGestures.Finger finger);

	// Token: 0x06000514 RID: 1300 RVA: 0x00013A98 File Offset: 0x00011C98
	private void InitFingers(int count)
	{
		this.fingers = new FingerGestures.Finger[count];
		for (int i = 0; i < count; i++)
		{
			this.fingers[i] = new FingerGestures.Finger(i);
		}
		this.touches = new FingerGestures.FingerList();
		this.InitDefaultComponents();
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00013AE4 File Offset: 0x00011CE4
	private void UpdateFingers()
	{
		this.touches.Clear();
		foreach (FingerGestures.Finger finger in this.fingers)
		{
			Vector2 newPos = Vector2.zero;
			FingerGestures.FingerPhase phase = this.GetPhase(finger);
			if (phase != FingerGestures.FingerPhase.None)
			{
				newPos = this.GetPosition(finger);
			}
			finger.Update(phase, newPos);
			if (finger.IsDown)
			{
				this.touches.Add(finger);
			}
		}
		foreach (FingerGestures.Finger finger2 in this.fingers)
		{
			finger2.PostUpdate();
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000516 RID: 1302 RVA: 0x00013B88 File Offset: 0x00011D88
	// (set) Token: 0x06000517 RID: 1303 RVA: 0x00013B94 File Offset: 0x00011D94
	public static FingerGestures.GlobalTouchFilterDelegate GlobalTouchFilter
	{
		get
		{
			return FingerGestures.instance.globalTouchFilterFunc;
		}
		set
		{
			FingerGestures.instance.globalTouchFilterFunc = value;
		}
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00013BA4 File Offset: 0x00011DA4
	protected bool ShouldProcessTouch(int fingerIndex, Vector2 position)
	{
		return this.globalTouchFilterFunc == null || this.globalTouchFilterFunc(fingerIndex, position);
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00013BC0 File Offset: 0x00011DC0
	private T CreateDefaultComponent<T>(T prefab, Transform parent) where T : FGComponent
	{
		T result = UnityEngine.Object.Instantiate(prefab) as T;
		result.gameObject.name = prefab.name;
		result.transform.parent = parent;
		return result;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00013C18 File Offset: 0x00011E18
	private T CreateDefaultGlobalComponent<T>(T prefab) where T : FGComponent
	{
		return this.CreateDefaultComponent<T>(prefab, this.globalComponentNode);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00013C28 File Offset: 0x00011E28
	private T CreateDefaultFingerComponent<T>(FingerGestures.Finger finger, T prefab) where T : FGComponent
	{
		return this.CreateDefaultComponent<T>(prefab, this.fingerComponentNodes[finger.Index]);
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x00013C40 File Offset: 0x00011E40
	public static FingerGestures.DefaultComponents Defaults
	{
		get
		{
			return FingerGestures.instance.defaultComponents;
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00013C4C File Offset: 0x00011E4C
	private Transform CreateNode(string name, Transform parent)
	{
		return new GameObject(name)
		{
			transform = 
			{
				parent = parent
			}
		}.transform;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00013C74 File Offset: 0x00011E74
	private void InitDefaultComponents()
	{
		int num = this.fingers.Length;
		if (this.globalComponentNode)
		{
			UnityEngine.Object.Destroy(this.globalComponentNode.gameObject);
		}
		if (this.fingerComponentNodes != null)
		{
			foreach (Transform transform in this.fingerComponentNodes)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		this.globalComponentNode = this.CreateNode("Global Components", base.transform);
		this.fingerComponentNodes = new Transform[num];
		for (int j = 0; j < this.fingerComponentNodes.Length; j++)
		{
			this.fingerComponentNodes[j] = this.CreateNode("Finger" + j, base.transform);
		}
		this.defaultComponents = new FingerGestures.DefaultComponents(num);
		if (this.defaultCompFlags.globalGestures.enabled)
		{
			this.InitGlobalGestures();
		}
		if (this.defaultCompFlags.perFinger.enabled)
		{
			foreach (FingerGestures.Finger finger in this.fingers)
			{
				this.InitDefaultComponents(finger);
			}
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00013DB4 File Offset: 0x00011FB4
	private void InitGlobalGestures()
	{
		if (this.defaultCompFlags.globalGestures.longPress)
		{
			LongPressGestureRecognizer longPressGestureRecognizer = this.CreateDefaultGlobalComponent<LongPressGestureRecognizer>(this.defaultPrefabs.longPress);
			longPressGestureRecognizer.OnLongPress += delegate(LongPressGestureRecognizer rec)
			{
				FingerGestures.RaiseOnLongPress(rec.Position);
			};
			this.defaultComponents.LongPress = longPressGestureRecognizer;
		}
		if (this.defaultCompFlags.globalGestures.twoFingerLongPress)
		{
			LongPressGestureRecognizer longPressGestureRecognizer2 = this.CreateDefaultGlobalComponent<LongPressGestureRecognizer>(this.defaultPrefabs.twoFingerLongPress);
			longPressGestureRecognizer2.RequiredFingerCount = 2;
			longPressGestureRecognizer2.OnLongPress += delegate(LongPressGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTwoFingerLongPress(rec.Position);
			};
			this.defaultComponents.TwoFingerLongPress = longPressGestureRecognizer2;
		}
		if (this.defaultCompFlags.globalGestures.drag)
		{
			DragGestureRecognizer dragGestureRecognizer = this.CreateDefaultGlobalComponent<DragGestureRecognizer>(this.defaultPrefabs.drag);
			dragGestureRecognizer.OnDragBegin += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnDragBegin(rec.Position, rec.StartPosition);
			};
			dragGestureRecognizer.OnDragMove += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnDragMove(rec.Position, rec.MoveDelta);
			};
			dragGestureRecognizer.OnDragStationary += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnDragStationary(rec.Position);
			};
			dragGestureRecognizer.OnDragEnd += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnDragEnd(rec.Position);
			};
			this.defaultComponents.Drag = dragGestureRecognizer;
		}
		if (this.defaultCompFlags.globalGestures.twoFingerDrag)
		{
			DragGestureRecognizer dragGestureRecognizer2 = this.CreateDefaultGlobalComponent<DragGestureRecognizer>(this.defaultPrefabs.twoFingerDrag);
			dragGestureRecognizer2.RequiredFingerCount = 2;
			dragGestureRecognizer2.OnDragBegin += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTwoFingerDragBegin(rec.Position, rec.StartPosition);
			};
			dragGestureRecognizer2.OnDragMove += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTwoFingerDragMove(rec.Position, rec.MoveDelta);
			};
			dragGestureRecognizer2.OnDragStationary += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTwoFingerDragStationary(rec.Position);
			};
			dragGestureRecognizer2.OnDragEnd += delegate(DragGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTwoFingerDragEnd(rec.Position);
			};
			this.defaultComponents.TwoFingerDrag = dragGestureRecognizer2;
		}
		if (this.defaultCompFlags.globalGestures.swipe)
		{
			SwipeGestureRecognizer swipeGestureRecognizer = this.CreateDefaultGlobalComponent<SwipeGestureRecognizer>(this.defaultPrefabs.swipe);
			swipeGestureRecognizer.OnSwipe += delegate(SwipeGestureRecognizer rec)
			{
				FingerGestures.RaiseOnSwipe(rec.StartPosition, rec.Direction, rec.Velocity);
			};
			this.defaultComponents.Swipe = swipeGestureRecognizer;
		}
		if (this.defaultCompFlags.globalGestures.twoFingerSwipe)
		{
			SwipeGestureRecognizer swipeGestureRecognizer2 = this.CreateDefaultGlobalComponent<SwipeGestureRecognizer>(this.defaultPrefabs.twoFingerSwipe);
			swipeGestureRecognizer2.RequiredFingerCount = 2;
			swipeGestureRecognizer2.OnSwipe += delegate(SwipeGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTwoFingerSwipe(rec.StartPosition, rec.Direction, rec.Velocity);
			};
			this.defaultComponents.TwoFingerSwipe = swipeGestureRecognizer2;
		}
		if (this.defaultCompFlags.globalGestures.tap)
		{
			TapGestureRecognizer tapGestureRecognizer = this.CreateDefaultGlobalComponent<TapGestureRecognizer>(this.defaultPrefabs.tap);
			tapGestureRecognizer.OnTap += delegate(TapGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTap(rec.Position);
			};
			this.defaultComponents.Tap = tapGestureRecognizer;
		}
		if (this.defaultCompFlags.globalGestures.doubleTap)
		{
			MultiTapGestureRecognizer multiTapGestureRecognizer = this.CreateDefaultGlobalComponent<MultiTapGestureRecognizer>(this.defaultPrefabs.doubleTap);
			multiTapGestureRecognizer.OnTap += delegate(MultiTapGestureRecognizer rec)
			{
				FingerGestures.RaiseOnDoubleTap(rec.Position);
			};
			this.defaultComponents.DoubleTap = multiTapGestureRecognizer;
		}
		if (this.defaultCompFlags.globalGestures.twoFingerTap)
		{
			TapGestureRecognizer tapGestureRecognizer2 = this.CreateDefaultGlobalComponent<TapGestureRecognizer>(this.defaultPrefabs.twoFingerTap);
			tapGestureRecognizer2.RequiredFingerCount = 2;
			tapGestureRecognizer2.OnTap += delegate(TapGestureRecognizer rec)
			{
				FingerGestures.RaiseOnTwoFingerTap(rec.Position);
			};
			this.defaultComponents.TwoFingerTap = tapGestureRecognizer2;
		}
		if (this.defaultCompFlags.globalGestures.pinch)
		{
			PinchGestureRecognizer pinchGestureRecognizer = this.CreateDefaultGlobalComponent<PinchGestureRecognizer>(this.defaultPrefabs.pinch);
			pinchGestureRecognizer.OnPinchBegin += delegate(PinchGestureRecognizer rec)
			{
				FingerGestures.RaiseOnPinchBegin(rec.GetPosition(0), rec.GetPosition(1));
			};
			pinchGestureRecognizer.OnPinchMove += delegate(PinchGestureRecognizer rec)
			{
				FingerGestures.RaiseOnPinchMove(rec.GetPosition(0), rec.GetPosition(1), rec.Delta);
			};
			pinchGestureRecognizer.OnPinchEnd += delegate(PinchGestureRecognizer rec)
			{
				FingerGestures.RaiseOnPinchEnd(rec.GetPosition(0), rec.GetPosition(1));
			};
			this.defaultComponents.Pinch = pinchGestureRecognizer;
		}
		if (this.defaultCompFlags.globalGestures.rotation)
		{
			RotationGestureRecognizer rotationGestureRecognizer = this.CreateDefaultGlobalComponent<RotationGestureRecognizer>(this.defaultPrefabs.rotation);
			rotationGestureRecognizer.OnRotationBegin += delegate(RotationGestureRecognizer rec)
			{
				FingerGestures.RaiseOnRotationBegin(rec.GetPosition(0), rec.GetPosition(1));
			};
			rotationGestureRecognizer.OnRotationMove += delegate(RotationGestureRecognizer rec)
			{
				FingerGestures.RaiseOnRotationMove(rec.GetPosition(0), rec.GetPosition(1), rec.RotationDelta);
			};
			rotationGestureRecognizer.OnRotationEnd += delegate(RotationGestureRecognizer rec)
			{
				FingerGestures.RaiseOnRotationEnd(rec.GetPosition(0), rec.GetPosition(1), rec.TotalRotation);
			};
			this.defaultComponents.Rotation = rotationGestureRecognizer;
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00014308 File Offset: 0x00012508
	private void InitDefaultComponents(FingerGestures.Finger finger)
	{
		FingerGestures.ITouchFilter touchFilter = new FingerGestures.SingleFingerFilter(finger);
		FingerGestures.DefaultComponents.FingerComponents fingerComponents = this.defaultComponents.Fingers[finger.Index];
		if (this.defaultCompFlags.perFinger.touch)
		{
			finger.OnDown += this.PerFinger_OnDown;
			finger.OnUp += this.PerFinger_OnUp;
		}
		if (this.defaultCompFlags.perFinger.motion)
		{
			FingerMotionDetector fingerMotionDetector = this.CreateDefaultFingerComponent<FingerMotionDetector>(finger, this.defaultPrefabs.fingerMotion);
			fingerMotionDetector.Finger = finger;
			fingerMotionDetector.OnMoveBegin += this.PerFinger_OnMoveBegin;
			fingerMotionDetector.OnMove += this.PerFinger_OnMove;
			fingerMotionDetector.OnMoveEnd += this.PerFinger_OnMoveEnd;
			fingerMotionDetector.OnStationaryBegin += this.PerFinger_OnStationaryBegin;
			fingerMotionDetector.OnStationary += this.PerFinger_OnStationary;
			fingerMotionDetector.OnStationaryEnd += this.PerFinger_OnStationaryEnd;
			fingerComponents.Motion = fingerMotionDetector;
		}
		if (this.defaultCompFlags.perFinger.longPress)
		{
			LongPressGestureRecognizer longPressGestureRecognizer = this.CreateDefaultFingerComponent<LongPressGestureRecognizer>(finger, this.defaultPrefabs.fingerLongPress);
			longPressGestureRecognizer.TouchFilter = touchFilter;
			longPressGestureRecognizer.OnLongPress += this.PerFinger_OnLongPress;
			fingerComponents.LongPress = longPressGestureRecognizer;
		}
		if (this.defaultCompFlags.perFinger.drag)
		{
			DragGestureRecognizer dragGestureRecognizer = this.CreateDefaultFingerComponent<DragGestureRecognizer>(finger, this.defaultPrefabs.fingerDrag);
			dragGestureRecognizer.TouchFilter = touchFilter;
			dragGestureRecognizer.OnDragBegin += this.PerFinger_OnDragBegin;
			dragGestureRecognizer.OnDragMove += this.PerFinger_OnDragMove;
			dragGestureRecognizer.OnDragStationary += this.PerFinger_OnDragStationary;
			dragGestureRecognizer.OnDragEnd += this.PerFinger_OnDragEnd;
			fingerComponents.Drag = dragGestureRecognizer;
		}
		if (this.defaultCompFlags.perFinger.swipe)
		{
			SwipeGestureRecognizer swipeGestureRecognizer = this.CreateDefaultFingerComponent<SwipeGestureRecognizer>(finger, this.defaultPrefabs.fingerSwipe);
			swipeGestureRecognizer.TouchFilter = touchFilter;
			swipeGestureRecognizer.OnSwipe += this.PerFinger_OnSwipe;
			fingerComponents.Swipe = swipeGestureRecognizer;
		}
		if (this.defaultCompFlags.perFinger.tap)
		{
			TapGestureRecognizer tapGestureRecognizer = this.CreateDefaultFingerComponent<TapGestureRecognizer>(finger, this.defaultPrefabs.fingerTap);
			tapGestureRecognizer.TouchFilter = touchFilter;
			tapGestureRecognizer.OnTap += this.PerFinger_OnTap;
			fingerComponents.Tap = tapGestureRecognizer;
		}
		if (this.defaultCompFlags.perFinger.doubleTap)
		{
			MultiTapGestureRecognizer multiTapGestureRecognizer = this.CreateDefaultFingerComponent<MultiTapGestureRecognizer>(finger, this.defaultPrefabs.fingerDoubleTap);
			multiTapGestureRecognizer.TouchFilter = touchFilter;
			multiTapGestureRecognizer.OnTap += this.PerFinger_OnDoubleTap;
			fingerComponents.DoubleTap = multiTapGestureRecognizer;
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000145BC File Offset: 0x000127BC
	private static FingerGestures.Finger GetFingerFromTouchFilter(GestureRecognizer recognizer)
	{
		FingerGestures.SingleFingerFilter singleFingerFilter = recognizer.TouchFilter as FingerGestures.SingleFingerFilter;
		if (singleFingerFilter != null)
		{
			return singleFingerFilter.Finger;
		}
		return null;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x000145E4 File Offset: 0x000127E4
	private void PerFinger_OnDown(FingerGestures.Finger source)
	{
		FingerGestures.RaiseOnFingerDown(source.Index, source.Position);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x000145F8 File Offset: 0x000127F8
	private void PerFinger_OnUp(FingerGestures.Finger source)
	{
		FingerGestures.RaiseOnFingerUp(source.Index, source.Position, Time.time - source.StarTime);
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00014624 File Offset: 0x00012824
	private void PerFinger_OnStationaryBegin(FingerMotionDetector source)
	{
		FingerGestures.RaiseOnFingerStationaryBegin(source.Finger.Index, source.AnchorPos);
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00014648 File Offset: 0x00012848
	private void PerFinger_OnStationary(FingerMotionDetector source)
	{
		FingerGestures.RaiseOnFingerStationary(source.Finger.Index, source.Finger.Position, source.ElapsedStationaryTime);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00014678 File Offset: 0x00012878
	private void PerFinger_OnStationaryEnd(FingerMotionDetector source)
	{
		FingerGestures.RaiseOnFingerStationaryEnd(source.Finger.Index, source.Finger.PreviousPosition, source.ElapsedStationaryTime);
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x000146A8 File Offset: 0x000128A8
	private void PerFinger_OnMoveBegin(FingerMotionDetector source)
	{
		FingerGestures.RaiseOnFingerMoveBegin(source.Finger.Index, source.AnchorPos);
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x000146CC File Offset: 0x000128CC
	private void PerFinger_OnMove(FingerMotionDetector source)
	{
		FingerGestures.RaiseOnFingerMove(source.Finger.Index, source.Finger.Position);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x000146F4 File Offset: 0x000128F4
	private void PerFinger_OnMoveEnd(FingerMotionDetector source)
	{
		FingerGestures.RaiseOnFingerMoveEnd(source.Finger.Index, source.Finger.Position);
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001471C File Offset: 0x0001291C
	private void PerFinger_OnDragBegin(DragGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerDragBegin(fingerFromTouchFilter.Index, source.Position, source.StartPosition);
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00014748 File Offset: 0x00012948
	private void PerFinger_OnDragMove(DragGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerDragMove(fingerFromTouchFilter.Index, source.Position, source.MoveDelta);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00014774 File Offset: 0x00012974
	private void PerFinger_OnDragStationary(DragGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerDragStationary(fingerFromTouchFilter.Index, source.Position);
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0001479C File Offset: 0x0001299C
	private void PerFinger_OnDragEnd(DragGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerDragEnd(fingerFromTouchFilter.Index, source.Position);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x000147C4 File Offset: 0x000129C4
	private void PerFinger_OnLongPress(LongPressGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerLongPress(fingerFromTouchFilter.Index, source.Position);
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x000147EC File Offset: 0x000129EC
	private void PerFinger_OnSwipe(SwipeGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerSwipe(fingerFromTouchFilter.Index, source.StartPosition, source.Direction, source.Velocity);
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00014820 File Offset: 0x00012A20
	private void PerFinger_OnTap(TapGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerTap(fingerFromTouchFilter.Index, source.Position);
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00014848 File Offset: 0x00012A48
	private void PerFinger_OnDoubleTap(MultiTapGestureRecognizer source)
	{
		FingerGestures.Finger fingerFromTouchFilter = FingerGestures.GetFingerFromTouchFilter(source);
		FingerGestures.RaiseOnFingerDoubleTap(fingerFromTouchFilter.Index, source.Position);
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00014870 File Offset: 0x00012A70
	public static FingerGestures.SwipeDirection GetSwipeDirection(Vector3 dir, float tolerance)
	{
		float num = Mathf.Clamp01(1f - tolerance);
		if (Vector2.Dot(dir, Vector2.right) >= num)
		{
			return FingerGestures.SwipeDirection.Right;
		}
		if (Vector2.Dot(dir, -Vector2.right) >= num)
		{
			return FingerGestures.SwipeDirection.Left;
		}
		if (Vector2.Dot(dir, Vector2.up) >= num)
		{
			return FingerGestures.SwipeDirection.Up;
		}
		if (Vector2.Dot(dir, -Vector2.up) >= num)
		{
			return FingerGestures.SwipeDirection.Down;
		}
		return FingerGestures.SwipeDirection.None;
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x000148F8 File Offset: 0x00012AF8
	public static bool AllFingersMoving(params FingerGestures.Finger[] fingers)
	{
		if (fingers.Length == 0)
		{
			return false;
		}
		foreach (FingerGestures.Finger finger in fingers)
		{
			if (finger.Phase != FingerGestures.FingerPhase.Moved)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00014938 File Offset: 0x00012B38
	public static bool FingersMovedInOppositeDirections(FingerGestures.Finger finger0, FingerGestures.Finger finger1, float minDOT)
	{
		float num = Vector2.Dot(finger0.DeltaPosition.normalized, finger1.DeltaPosition.normalized);
		return num < minDOT;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0001496C File Offset: 0x00012B6C
	public static float SignedAngle(Vector2 from, Vector2 to)
	{
		float y = from.x * to.y - from.y * to.x;
		return Mathf.Atan2(y, Vector2.Dot(from, to));
	}

	// Token: 0x0400026F RID: 623
	protected static bool loggingGestures;

	// Token: 0x04000270 RID: 624
	private static FingerGestures instance;

	// Token: 0x04000271 RID: 625
	private FingerGestures.Finger[] fingers;

	// Token: 0x04000272 RID: 626
	private FingerGestures.FingerList touches;

	// Token: 0x04000273 RID: 627
	private FingerGestures.GlobalTouchFilterDelegate globalTouchFilterFunc;

	// Token: 0x04000274 RID: 628
	public FingerGesturesPrefabs defaultPrefabs;

	// Token: 0x04000275 RID: 629
	private Transform globalComponentNode;

	// Token: 0x04000276 RID: 630
	private Transform[] fingerComponentNodes;

	// Token: 0x04000277 RID: 631
	public FingerGestures.DefaultComponentCreationFlags defaultCompFlags;

	// Token: 0x04000278 RID: 632
	private FingerGestures.DefaultComponents defaultComponents;

	// Token: 0x02000084 RID: 132
	public enum FingerPhase
	{
		// Token: 0x040002B5 RID: 693
		None,
		// Token: 0x040002B6 RID: 694
		Began,
		// Token: 0x040002B7 RID: 695
		Moved,
		// Token: 0x040002B8 RID: 696
		Stationary,
		// Token: 0x040002B9 RID: 697
		Ended
	}

	// Token: 0x02000085 RID: 133
	public class Finger
	{
		// Token: 0x0600054B RID: 1355 RVA: 0x00014B90 File Offset: 0x00012D90
		public Finger(int index)
		{
			this.index = index;
		}

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x0600054C RID: 1356 RVA: 0x00014BE0 File Offset: 0x00012DE0
		// (remove) Token: 0x0600054D RID: 1357 RVA: 0x00014BFC File Offset: 0x00012DFC
		public event FingerGestures.Finger.FingerEventDelegate OnDown;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x0600054E RID: 1358 RVA: 0x00014C18 File Offset: 0x00012E18
		// (remove) Token: 0x0600054F RID: 1359 RVA: 0x00014C34 File Offset: 0x00012E34
		public event FingerGestures.Finger.FingerEventDelegate OnUp;

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00014C50 File Offset: 0x00012E50
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00014C58 File Offset: 0x00012E58
		public FingerGestures.FingerPhase Phase
		{
			get
			{
				return this.phase;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00014C60 File Offset: 0x00012E60
		public bool IsDown
		{
			get
			{
				return this.down;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x00014C68 File Offset: 0x00012E68
		public bool WasDown
		{
			get
			{
				return this.wasDown;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x00014C70 File Offset: 0x00012E70
		public float StarTime
		{
			get
			{
				return this.startTime;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x00014C78 File Offset: 0x00012E78
		public Vector2 StartPosition
		{
			get
			{
				return this.startPos;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00014C80 File Offset: 0x00012E80
		public Vector2 Position
		{
			get
			{
				return this.pos;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x00014C88 File Offset: 0x00012E88
		public Vector2 PreviousPosition
		{
			get
			{
				return this.prevPos;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00014C90 File Offset: 0x00012E90
		public Vector2 DeltaPosition
		{
			get
			{
				return this.deltaPos;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x00014C98 File Offset: 0x00012E98
		public float DistanceFromStart
		{
			get
			{
				return this.distFromStart;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x00014CA0 File Offset: 0x00012EA0
		public bool Filtered
		{
			get
			{
				return this.filteredOut;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00014CA8 File Offset: 0x00012EA8
		public override string ToString()
		{
			return "Finger" + this.index;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00014CC0 File Offset: 0x00012EC0
		internal void Update(FingerGestures.FingerPhase newPhase, Vector2 newPos)
		{
			if (this.filteredOut)
			{
				if (newPhase == FingerGestures.FingerPhase.Ended || newPhase == FingerGestures.FingerPhase.None)
				{
					this.filteredOut = false;
				}
				newPhase = FingerGestures.FingerPhase.None;
			}
			if (this.phase != newPhase)
			{
				if (newPhase == FingerGestures.FingerPhase.None && this.phase != FingerGestures.FingerPhase.Ended)
				{
					if (FingerGestures.loggingGestures)
					{
						Debug.LogWarning("Correcting bad FingerPhase transition (FingerPhase.Ended skipped)");
					}
					this.Update(FingerGestures.FingerPhase.Ended, this.PreviousPosition);
					return;
				}
				if (!this.down && (newPhase == FingerGestures.FingerPhase.Moved || newPhase == FingerGestures.FingerPhase.Stationary))
				{
					if (FingerGestures.loggingGestures)
					{
						Debug.LogWarning("Correcting bad FingerPhase transition (FingerPhase.Began skipped)");
					}
					this.Update(FingerGestures.FingerPhase.Began, newPos);
					return;
				}
				if ((this.down && newPhase == FingerGestures.FingerPhase.Began) || (!this.down && newPhase == FingerGestures.FingerPhase.Ended))
				{
					if (FingerGestures.loggingGestures)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"Invalid state FingerPhase transition from ",
							this.phase,
							" to ",
							newPhase,
							" - Skipping."
						}));
					}
					return;
				}
			}
			else if (newPhase == FingerGestures.FingerPhase.Began || newPhase == FingerGestures.FingerPhase.Ended)
			{
				if (FingerGestures.loggingGestures)
				{
					Debug.LogWarning("Duplicated FingerPhase." + newPhase.ToString() + " - skipping.");
				}
				return;
			}
			if (newPhase == FingerGestures.FingerPhase.Began && !FingerGestures.instance.ShouldProcessTouch(this.index, newPos))
			{
				this.filteredOut = true;
				newPhase = FingerGestures.FingerPhase.None;
			}
			if (newPhase != FingerGestures.FingerPhase.None)
			{
				if (newPhase == FingerGestures.FingerPhase.Ended)
				{
					this.down = false;
				}
				else
				{
					if (newPhase == FingerGestures.FingerPhase.Began)
					{
						this.down = true;
						this.startPos = newPos;
						this.prevPos = newPos;
						this.startTime = Time.time;
					}
					this.prevPos = this.pos;
					this.pos = newPos;
					this.deltaPos = this.pos - this.prevPos;
					this.distFromStart = Vector3.Distance(this.startPos, this.pos);
				}
			}
			this.phase = newPhase;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00014ECC File Offset: 0x000130CC
		internal void PostUpdate()
		{
			if (this.wasDown != this.down)
			{
				if (this.down)
				{
					if (this.OnDown != null)
					{
						this.OnDown(this);
					}
				}
				else if (this.OnUp != null)
				{
					this.OnUp(this);
				}
			}
			this.wasDown = this.down;
		}

		// Token: 0x040002BA RID: 698
		private int index;

		// Token: 0x040002BB RID: 699
		private bool wasDown;

		// Token: 0x040002BC RID: 700
		private bool down;

		// Token: 0x040002BD RID: 701
		private bool filteredOut = true;

		// Token: 0x040002BE RID: 702
		private float startTime;

		// Token: 0x040002BF RID: 703
		private FingerGestures.FingerPhase phase;

		// Token: 0x040002C0 RID: 704
		private Vector2 startPos = Vector2.zero;

		// Token: 0x040002C1 RID: 705
		private Vector2 pos = Vector2.zero;

		// Token: 0x040002C2 RID: 706
		private Vector2 prevPos = Vector2.zero;

		// Token: 0x040002C3 RID: 707
		private Vector2 deltaPos = Vector2.zero;

		// Token: 0x040002C4 RID: 708
		private float distFromStart;

		// Token: 0x02000116 RID: 278
		// (Invoke) Token: 0x06000A1E RID: 2590
		public delegate void FingerEventDelegate(FingerGestures.Finger finger);
	}

	// Token: 0x02000086 RID: 134
	[Serializable]
	public class DefaultComponentCreationFlags
	{
		// Token: 0x040002C7 RID: 711
		public FingerGestures.DefaultComponentCreationFlags.PerFinger perFinger;

		// Token: 0x040002C8 RID: 712
		public FingerGestures.DefaultComponentCreationFlags.GlobalGestures globalGestures;

		// Token: 0x02000087 RID: 135
		[Serializable]
		public class PerFinger
		{
			// Token: 0x040002C9 RID: 713
			public bool enabled = true;

			// Token: 0x040002CA RID: 714
			public bool touch = true;

			// Token: 0x040002CB RID: 715
			public bool motion = true;

			// Token: 0x040002CC RID: 716
			public bool longPress = true;

			// Token: 0x040002CD RID: 717
			public bool drag = true;

			// Token: 0x040002CE RID: 718
			public bool swipe = true;

			// Token: 0x040002CF RID: 719
			public bool tap = true;

			// Token: 0x040002D0 RID: 720
			public bool doubleTap = true;
		}

		// Token: 0x02000088 RID: 136
		[Serializable]
		public class GlobalGestures
		{
			// Token: 0x040002D1 RID: 721
			public bool enabled = true;

			// Token: 0x040002D2 RID: 722
			public bool longPress = true;

			// Token: 0x040002D3 RID: 723
			public bool drag = true;

			// Token: 0x040002D4 RID: 724
			public bool swipe = true;

			// Token: 0x040002D5 RID: 725
			public bool tap = true;

			// Token: 0x040002D6 RID: 726
			public bool doubleTap = true;

			// Token: 0x040002D7 RID: 727
			public bool pinch = true;

			// Token: 0x040002D8 RID: 728
			public bool rotation = true;

			// Token: 0x040002D9 RID: 729
			public bool twoFingerLongPress = true;

			// Token: 0x040002DA RID: 730
			public bool twoFingerDrag = true;

			// Token: 0x040002DB RID: 731
			public bool twoFingerSwipe = true;

			// Token: 0x040002DC RID: 732
			public bool twoFingerTap = true;
		}
	}

	// Token: 0x02000089 RID: 137
	public class DefaultComponents
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x00014FF0 File Offset: 0x000131F0
		public DefaultComponents(int fingerCount)
		{
			this.fingers = new FingerGestures.DefaultComponents.FingerComponents[fingerCount];
			for (int i = 0; i < this.fingers.Length; i++)
			{
				this.fingers[i] = new FingerGestures.DefaultComponents.FingerComponents();
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x00015038 File Offset: 0x00013238
		public FingerGestures.DefaultComponents.FingerComponents[] Fingers
		{
			get
			{
				return this.fingers;
			}
		}

		// Token: 0x040002DD RID: 733
		private FingerGestures.DefaultComponents.FingerComponents[] fingers;

		// Token: 0x040002DE RID: 734
		public LongPressGestureRecognizer LongPress;

		// Token: 0x040002DF RID: 735
		public DragGestureRecognizer Drag;

		// Token: 0x040002E0 RID: 736
		public TapGestureRecognizer Tap;

		// Token: 0x040002E1 RID: 737
		public MultiTapGestureRecognizer DoubleTap;

		// Token: 0x040002E2 RID: 738
		public SwipeGestureRecognizer Swipe;

		// Token: 0x040002E3 RID: 739
		public PinchGestureRecognizer Pinch;

		// Token: 0x040002E4 RID: 740
		public RotationGestureRecognizer Rotation;

		// Token: 0x040002E5 RID: 741
		public LongPressGestureRecognizer TwoFingerLongPress;

		// Token: 0x040002E6 RID: 742
		public DragGestureRecognizer TwoFingerDrag;

		// Token: 0x040002E7 RID: 743
		public TapGestureRecognizer TwoFingerTap;

		// Token: 0x040002E8 RID: 744
		public SwipeGestureRecognizer TwoFingerSwipe;

		// Token: 0x0200008A RID: 138
		public class FingerComponents
		{
			// Token: 0x040002E9 RID: 745
			public FingerMotionDetector Motion;

			// Token: 0x040002EA RID: 746
			public LongPressGestureRecognizer LongPress;

			// Token: 0x040002EB RID: 747
			public DragGestureRecognizer Drag;

			// Token: 0x040002EC RID: 748
			public TapGestureRecognizer Tap;

			// Token: 0x040002ED RID: 749
			public MultiTapGestureRecognizer DoubleTap;

			// Token: 0x040002EE RID: 750
			public SwipeGestureRecognizer Swipe;
		}
	}

	// Token: 0x0200008B RID: 139
	public interface IFingerList : IEnumerable, IEnumerable<FingerGestures.Finger>
	{
		// Token: 0x1700005E RID: 94
		FingerGestures.Finger this[int index]
		{
			get;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000565 RID: 1381
		int Count { get; }

		// Token: 0x06000566 RID: 1382
		Vector2 GetAveragePosition();

		// Token: 0x06000567 RID: 1383
		Vector2 GetAveragePreviousPosition();

		// Token: 0x06000568 RID: 1384
		float GetAverageDistanceFromStart();

		// Token: 0x06000569 RID: 1385
		FingerGestures.Finger GetOldest();
	}

	// Token: 0x0200008C RID: 140
	public class FingerList : IEnumerable, IEnumerable<FingerGestures.Finger>, FingerGestures.IFingerList
	{
		// Token: 0x0600056A RID: 1386 RVA: 0x00015048 File Offset: 0x00013248
		public FingerList()
		{
			this.list = new List<FingerGestures.Finger>();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001505C File Offset: 0x0001325C
		public FingerList(List<FingerGestures.Finger> list)
		{
			this.list = list;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001506C File Offset: 0x0001326C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000060 RID: 96
		public FingerGestures.Finger this[int index]
		{
			get
			{
				return this.list[index];
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00015084 File Offset: 0x00013284
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00015094 File Offset: 0x00013294
		public IEnumerator<FingerGestures.Finger> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x000150A8 File Offset: 0x000132A8
		public void Add(FingerGestures.Finger touch)
		{
			this.list.Add(touch);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000150B8 File Offset: 0x000132B8
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000150C8 File Offset: 0x000132C8
		public Vector2 AverageVector(FingerGestures.FingerList.FingerPropertyGetterDelegate<Vector2> getProperty)
		{
			Vector2 vector = Vector2.zero;
			if (this.Count > 0)
			{
				foreach (FingerGestures.Finger finger in this.list)
				{
					vector += getProperty(finger);
				}
				vector /= (float)this.Count;
			}
			return vector;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00015158 File Offset: 0x00013358
		public float AverageFloat(FingerGestures.FingerList.FingerPropertyGetterDelegate<float> getProperty)
		{
			float num = 0f;
			if (this.Count > 0)
			{
				foreach (FingerGestures.Finger finger in this.list)
				{
					num += getProperty(finger);
				}
				num /= (float)this.Count;
			}
			return num;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000151E0 File Offset: 0x000133E0
		private static Vector2 GetFingerPosition(FingerGestures.Finger finger)
		{
			return finger.Position;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x000151E8 File Offset: 0x000133E8
		private static Vector2 GetFingerPreviousPosition(FingerGestures.Finger finger)
		{
			return finger.PreviousPosition;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000151F0 File Offset: 0x000133F0
		private static float GetFingerDistanceFromStart(FingerGestures.Finger finger)
		{
			return finger.DistanceFromStart;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000151F8 File Offset: 0x000133F8
		public Vector2 GetAveragePosition()
		{
			return this.AverageVector(new FingerGestures.FingerList.FingerPropertyGetterDelegate<Vector2>(FingerGestures.FingerList.GetFingerPosition));
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001520C File Offset: 0x0001340C
		public Vector2 GetAveragePreviousPosition()
		{
			return this.AverageVector(new FingerGestures.FingerList.FingerPropertyGetterDelegate<Vector2>(FingerGestures.FingerList.GetFingerPreviousPosition));
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00015220 File Offset: 0x00013420
		public float GetAverageDistanceFromStart()
		{
			return this.AverageFloat(new FingerGestures.FingerList.FingerPropertyGetterDelegate<float>(FingerGestures.FingerList.GetFingerDistanceFromStart));
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00015234 File Offset: 0x00013434
		public FingerGestures.Finger GetOldest()
		{
			FingerGestures.Finger finger = null;
			foreach (FingerGestures.Finger finger2 in this.list)
			{
				if (finger == null || finger2.StarTime < finger.StarTime)
				{
					finger = finger2;
				}
			}
			return finger;
		}

		// Token: 0x040002EF RID: 751
		private List<FingerGestures.Finger> list;

		// Token: 0x02000117 RID: 279
		// (Invoke) Token: 0x06000A22 RID: 2594
		public delegate T FingerPropertyGetterDelegate<T>(FingerGestures.Finger finger);
	}

	// Token: 0x0200008D RID: 141
	[Flags]
	public enum SwipeDirection
	{
		// Token: 0x040002F1 RID: 753
		Right = 1,
		// Token: 0x040002F2 RID: 754
		Left = 2,
		// Token: 0x040002F3 RID: 755
		Up = 4,
		// Token: 0x040002F4 RID: 756
		Down = 8,
		// Token: 0x040002F5 RID: 757
		None = 0,
		// Token: 0x040002F6 RID: 758
		All = 15,
		// Token: 0x040002F7 RID: 759
		Vertical = 12,
		// Token: 0x040002F8 RID: 760
		Horizontal = 3
	}

	// Token: 0x0200008E RID: 142
	public interface ITouchFilter
	{
		// Token: 0x0600057B RID: 1403
		FingerGestures.IFingerList Apply(FingerGestures.IFingerList touches);
	}

	// Token: 0x0200008F RID: 143
	public class SingleFingerFilter : FingerGestures.ITouchFilter
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x000152B0 File Offset: 0x000134B0
		public SingleFingerFilter(FingerGestures.Finger finger)
		{
			this.finger = finger;
			this.fingerList.Add(finger);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x000152E4 File Offset: 0x000134E4
		public FingerGestures.Finger Finger
		{
			get
			{
				return this.finger;
			}
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x000152EC File Offset: 0x000134EC
		public FingerGestures.IFingerList Apply(FingerGestures.IFingerList touches)
		{
			foreach (FingerGestures.Finger finger in touches)
			{
				if (finger == this.Finger)
				{
					return this.fingerList;
				}
			}
			return this.emptyList;
		}

		// Token: 0x040002F9 RID: 761
		private FingerGestures.FingerList fingerList = new FingerGestures.FingerList();

		// Token: 0x040002FA RID: 762
		private FingerGestures.FingerList emptyList = new FingerGestures.FingerList();

		// Token: 0x040002FB RID: 763
		private FingerGestures.Finger finger;
	}

	// Token: 0x02000118 RID: 280
	// (Invoke) Token: 0x06000A26 RID: 2598
	public delegate void FingerDownEventHandler(int fingerIndex, Vector2 fingerPos);

	// Token: 0x02000119 RID: 281
	// (Invoke) Token: 0x06000A2A RID: 2602
	public delegate void FingerUpEventHandler(int fingerIndex, Vector2 fingerPos, float timeHeldDown);

	// Token: 0x0200011A RID: 282
	// (Invoke) Token: 0x06000A2E RID: 2606
	public delegate void FingerStationaryBeginEventHandler(int fingerIndex, Vector2 fingerPos);

	// Token: 0x0200011B RID: 283
	// (Invoke) Token: 0x06000A32 RID: 2610
	public delegate void FingerStationaryEventHandler(int fingerIndex, Vector2 fingerPos, float elapsedTime);

	// Token: 0x0200011C RID: 284
	// (Invoke) Token: 0x06000A36 RID: 2614
	public delegate void FingerStationaryEndEventHandler(int fingerIndex, Vector2 fingerPos, float elapsedTime);

	// Token: 0x0200011D RID: 285
	// (Invoke) Token: 0x06000A3A RID: 2618
	public delegate void FingerMoveEventHandler(int fingerIndex, Vector2 fingerPos);

	// Token: 0x0200011E RID: 286
	// (Invoke) Token: 0x06000A3E RID: 2622
	public delegate void FingerLongPressEventHandler(int fingerIndex, Vector2 fingerPos);

	// Token: 0x0200011F RID: 287
	// (Invoke) Token: 0x06000A42 RID: 2626
	public delegate void FingerTapEventHandler(int fingerIndex, Vector2 fingerPos);

	// Token: 0x02000120 RID: 288
	// (Invoke) Token: 0x06000A46 RID: 2630
	public delegate void FingerSwipeEventHandler(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity);

	// Token: 0x02000121 RID: 289
	// (Invoke) Token: 0x06000A4A RID: 2634
	public delegate void FingerDragBeginEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 startPos);

	// Token: 0x02000122 RID: 290
	// (Invoke) Token: 0x06000A4E RID: 2638
	public delegate void FingerDragMoveEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 delta);

	// Token: 0x02000123 RID: 291
	// (Invoke) Token: 0x06000A52 RID: 2642
	public delegate void FingerDragEndEventHandler(int fingerIndex, Vector2 fingerPos);

	// Token: 0x02000124 RID: 292
	// (Invoke) Token: 0x06000A56 RID: 2646
	public delegate void LongPressEventHandler(Vector2 fingerPos);

	// Token: 0x02000125 RID: 293
	// (Invoke) Token: 0x06000A5A RID: 2650
	public delegate void TapEventHandler(Vector2 fingerPos);

	// Token: 0x02000126 RID: 294
	// (Invoke) Token: 0x06000A5E RID: 2654
	public delegate void SwipeEventHandler(Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity);

	// Token: 0x02000127 RID: 295
	// (Invoke) Token: 0x06000A62 RID: 2658
	public delegate void DragBeginEventHandler(Vector2 fingerPos, Vector2 startPos);

	// Token: 0x02000128 RID: 296
	// (Invoke) Token: 0x06000A66 RID: 2662
	public delegate void DragMoveEventHandler(Vector2 fingerPos, Vector2 delta);

	// Token: 0x02000129 RID: 297
	// (Invoke) Token: 0x06000A6A RID: 2666
	public delegate void DragEndEventHandler(Vector2 fingerPos);

	// Token: 0x0200012A RID: 298
	// (Invoke) Token: 0x06000A6E RID: 2670
	public delegate void PinchEventHandler(Vector2 fingerPos1, Vector2 fingerPos2);

	// Token: 0x0200012B RID: 299
	// (Invoke) Token: 0x06000A72 RID: 2674
	public delegate void PinchMoveEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float delta);

	// Token: 0x0200012C RID: 300
	// (Invoke) Token: 0x06000A76 RID: 2678
	public delegate void RotationBeginEventHandler(Vector2 fingerPos1, Vector2 fingerPos2);

	// Token: 0x0200012D RID: 301
	// (Invoke) Token: 0x06000A7A RID: 2682
	public delegate void RotationMoveEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta);

	// Token: 0x0200012E RID: 302
	// (Invoke) Token: 0x06000A7E RID: 2686
	public delegate void RotationEndEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle);

	// Token: 0x0200012F RID: 303
	// (Invoke) Token: 0x06000A82 RID: 2690
	public delegate void FingersUpdatedEventDelegate();

	// Token: 0x02000130 RID: 304
	// (Invoke) Token: 0x06000A86 RID: 2694
	public delegate bool GlobalTouchFilterDelegate(int fingerIndex, Vector2 position);
}
