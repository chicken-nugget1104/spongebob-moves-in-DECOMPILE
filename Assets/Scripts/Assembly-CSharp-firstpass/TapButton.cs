using System;
using Yarg;

// Token: 0x020000D8 RID: 216
public class TapButton : BaseButton
{
	// Token: 0x0600086A RID: 2154 RVA: 0x0001F6E4 File Offset: 0x0001D8E4
	protected override bool TouchEventHandler(YGEvent evt)
	{
		if (base.enabled)
		{
			YGEvent.TYPE type = evt.type;
			if (type == YGEvent.TYPE.TOUCH_BEGIN)
			{
				this.didBegin = true;
				this.BeginEvent.FireEvent();
				return true;
			}
			if (type != YGEvent.TYPE.TOUCH_END)
			{
				if (type == YGEvent.TYPE.TAP)
				{
					this.didBegin = false;
					if (this.buttonRdy)
					{
						this.TapEvent.FireEvent();
						this.buttonRdy = false;
						base.Invoke("ResetButtonCD", this.cdtime);
					}
					return true;
				}
			}
			else
			{
				if (this.didBegin)
				{
					this.didBegin = false;
					return true;
				}
				return false;
			}
		}
		return false;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0001F780 File Offset: 0x0001D980
	private void ResetButtonCD()
	{
		this.buttonRdy = true;
	}

	// Token: 0x0400051C RID: 1308
	public EventDispatcher TapEvent = new EventDispatcher();

	// Token: 0x0400051D RID: 1309
	public EventDispatcher BeginEvent = new EventDispatcher();

	// Token: 0x0400051E RID: 1310
	private float cdtime = 0.1f;

	// Token: 0x0400051F RID: 1311
	private bool buttonRdy = true;

	// Token: 0x04000520 RID: 1312
	private bool didBegin;
}
