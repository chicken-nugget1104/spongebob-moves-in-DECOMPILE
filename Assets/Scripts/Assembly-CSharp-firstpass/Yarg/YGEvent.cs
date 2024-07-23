using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

namespace Yarg
{
	// Token: 0x02000110 RID: 272
	public class YGEvent
	{
		// Token: 0x06000A08 RID: 2568 RVA: 0x00028128 File Offset: 0x00026328
		public YGEvent()
		{
			this.holdStartTime = Time.time;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00028148 File Offset: 0x00026348
		public YGEvent(Touch t) : this()
		{
			this.fingerId = t.fingerId;
			this.startPosition = (this.position = t.position);
			this.deltaPosition = t.deltaPosition;
			this.deltaPosition.y = this.deltaPosition.y * -1f;
			this.deltaTime = t.deltaTime;
			this.startTime = Time.time;
			this.tapCount = t.tapCount;
			switch (t.phase)
			{
			case TouchPhase.Began:
				this.type = YGEvent.TYPE.TOUCH_BEGIN;
				break;
			case TouchPhase.Moved:
				this.type = YGEvent.TYPE.TOUCH_MOVE;
				break;
			case TouchPhase.Stationary:
				this.type = YGEvent.TYPE.TOUCH_STAY;
				break;
			case TouchPhase.Ended:
				this.type = YGEvent.TYPE.TOUCH_END;
				break;
			case TouchPhase.Canceled:
				this.type = YGEvent.TYPE.TOUCH_CANCEL;
				break;
			}
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0002822C File Offset: 0x0002642C
		public YGEvent(Event e) : this()
		{
			this.fingerId = 98;
			if (e == null || !e.isMouse)
			{
				return;
			}
			this.startPosition = (this.position = e.mousePosition);
			this.deltaPosition = e.delta;
			this.startTime = Time.time;
			this.deltaTime = Time.deltaTime;
			this.tapCount = e.clickCount;
			this.type = YGEvent.TYPE.NULL;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000282A4 File Offset: 0x000264A4
		public YGEvent(YGEvent y) : this()
		{
			if (y == null)
			{
				return;
			}
			this.fingerId = y.fingerId;
			this.position = y.position;
			this.deltaPosition = y.deltaPosition;
			this.startPosition = y.startPosition;
			this.startTime = Time.time;
			this.deltaTime = y.deltaTime;
			this.tapCount = y.tapCount;
			this.type = y.type;
			this.used = y.used;
			this.holdStartTime = y.holdStartTime;
			this.direction = y.direction;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00028344 File Offset: 0x00026544
		public bool Flick
		{
			get
			{
				float num = this.deltaPosition.magnitude / this.deltaTime;
				if (num > 400f)
				{
					if (Mathf.Abs(this.deltaPosition.x) > Mathf.Abs(this.deltaPosition.y))
					{
						this.direction = ((this.deltaPosition.x >= 0f) ? YGEvent.DIRECTION.RIGHT : YGEvent.DIRECTION.LEFT);
					}
					else
					{
						this.direction = ((this.deltaPosition.y >= 0f) ? YGEvent.DIRECTION.DOWN : YGEvent.DIRECTION.UP);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x000283E0 File Offset: 0x000265E0
		public bool Hold
		{
			get
			{
				if (this.holdStartTime < 0f)
				{
					return false;
				}
				if ((this.startPosition - this.position).sqrMagnitude > 64f)
				{
					this.holdStartTime = -1f;
					return false;
				}
				if (Time.time - this.holdStartTime >= 1f)
				{
					this.type = YGEvent.TYPE.HOLD;
					this.holdStartTime = -1f;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0002845C File Offset: 0x0002665C
		public YGEvent Update(YGEvent y)
		{
			this.position = y.position;
			this.deltaPosition = y.deltaPosition;
			this.deltaTime = y.deltaTime;
			this.tapCount = y.tapCount;
			this.type = y.type;
			this.used = y.used;
			y.startPosition = this.startPosition;
			y.startTime = this.startTime;
			y.holdStartTime = this.holdStartTime;
			this.direction = y.direction;
			return y;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x000284E4 File Offset: 0x000266E4
		public void UpdateFromMouseInput()
		{
			this.startPosition = (this.position = Input.mousePosition);
			int num = 0;
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				this.type = YGEvent.TYPE.TOUCH_BEGIN;
				num = 1;
			}
			else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			{
				this.type = YGEvent.TYPE.TOUCH_END;
				num = 1;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
			{
				if (this.deltaPosition != Vector2.zero)
				{
					this.type = YGEvent.TYPE.TOUCH_MOVE;
				}
				else
				{
					this.type = YGEvent.TYPE.TOUCH_STAY;
				}
				num = 1;
			}
			else
			{
				this.type = YGEvent.TYPE.NULL;
			}
			if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1) || Input.GetMouseButton(1))
			{
				this.fingerId = 99;
			}
			YGEvent.touchCount = num;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x000285D0 File Offset: 0x000267D0
		public override string ToString()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["fingerId"] = this.fingerId;
			dictionary["position"] = this.position.ToString();
			dictionary["deltaPosition"] = this.deltaPosition.ToString();
			dictionary["startPosition"] = this.startPosition.ToString();
			dictionary["deltaTime"] = this.deltaTime;
			dictionary["startTime"] = this.startTime;
			dictionary["holdStartTime"] = this.holdStartTime;
			dictionary["tapCount"] = this.tapCount;
			dictionary["tapCount"] = this.tapCount;
			dictionary["type"] = this.type.ToString();
			dictionary["direction"] = this.direction.ToString();
			dictionary["touchCount"] = YGEvent.touchCount;
			dictionary["used"] = this.used;
			return Json.Serialize(dictionary);
		}

		// Token: 0x04000672 RID: 1650
		public const int MOUSE_LEFT = 98;

		// Token: 0x04000673 RID: 1651
		public const int MOUSE_RIGHT = 99;

		// Token: 0x04000674 RID: 1652
		public const float HOLD_DURATION = 1f;

		// Token: 0x04000675 RID: 1653
		public const float HOLD_DRIFT_RADIUS_SQUARED = 64f;

		// Token: 0x04000676 RID: 1654
		public int fingerId;

		// Token: 0x04000677 RID: 1655
		public Vector2 position;

		// Token: 0x04000678 RID: 1656
		public Vector2 deltaPosition;

		// Token: 0x04000679 RID: 1657
		public Vector2 startPosition;

		// Token: 0x0400067A RID: 1658
		public float distance;

		// Token: 0x0400067B RID: 1659
		public float deltaTime;

		// Token: 0x0400067C RID: 1660
		public float startTime;

		// Token: 0x0400067D RID: 1661
		public int tapCount;

		// Token: 0x0400067E RID: 1662
		public YGEvent.TYPE type;

		// Token: 0x0400067F RID: 1663
		public YGEvent.DIRECTION direction;

		// Token: 0x04000680 RID: 1664
		public static int touchCount;

		// Token: 0x04000681 RID: 1665
		public object param;

		// Token: 0x04000682 RID: 1666
		public bool used;

		// Token: 0x04000683 RID: 1667
		private float holdStartTime = -1f;

		// Token: 0x02000111 RID: 273
		public enum TYPE
		{
			// Token: 0x04000685 RID: 1669
			NULL,
			// Token: 0x04000686 RID: 1670
			TOUCH_BEGIN,
			// Token: 0x04000687 RID: 1671
			TOUCH_END,
			// Token: 0x04000688 RID: 1672
			TOUCH_CANCEL,
			// Token: 0x04000689 RID: 1673
			TOUCH_STAY,
			// Token: 0x0400068A RID: 1674
			TOUCH_MOVE,
			// Token: 0x0400068B RID: 1675
			HOVER,
			// Token: 0x0400068C RID: 1676
			DRAG,
			// Token: 0x0400068D RID: 1677
			FLICK,
			// Token: 0x0400068E RID: 1678
			SWIPE,
			// Token: 0x0400068F RID: 1679
			PINCH,
			// Token: 0x04000690 RID: 1680
			TAP,
			// Token: 0x04000691 RID: 1681
			RESET,
			// Token: 0x04000692 RID: 1682
			HOLD,
			// Token: 0x04000693 RID: 1683
			DISABLE,
			// Token: 0x04000694 RID: 1684
			ENABLE
		}

		// Token: 0x02000112 RID: 274
		public enum DIRECTION
		{
			// Token: 0x04000696 RID: 1686
			NULL,
			// Token: 0x04000697 RID: 1687
			UP,
			// Token: 0x04000698 RID: 1688
			DOWN,
			// Token: 0x04000699 RID: 1689
			LEFT,
			// Token: 0x0400069A RID: 1690
			RIGHT
		}
	}
}
