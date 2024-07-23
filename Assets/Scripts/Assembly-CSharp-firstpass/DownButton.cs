using System;
using Yarg;

// Token: 0x020000D6 RID: 214
public class DownButton : TapButton
{
	// Token: 0x06000865 RID: 2149 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
	protected override bool TouchEventHandler(YGEvent evt)
	{
		YGEvent.TYPE type = evt.type;
		switch (type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			if (!this.triggered)
			{
				this.TapEvent.FireEvent();
				this.triggered = true;
			}
			return true;
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
			break;
		case YGEvent.TYPE.TOUCH_STAY:
			return false;
		default:
			if (type != YGEvent.TYPE.RESET)
			{
				return false;
			}
			break;
		}
		this.triggered = false;
		return true;
	}

	// Token: 0x0400051A RID: 1306
	private bool triggered;
}
