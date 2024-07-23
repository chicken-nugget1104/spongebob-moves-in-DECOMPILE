using System;
using Yarg;

// Token: 0x020000D7 RID: 215
public class DragButton : BaseButton
{
	// Token: 0x06000867 RID: 2151 RVA: 0x0001F648 File Offset: 0x0001D848
	private void SendDrag(YGEvent evt)
	{
		this.DragEvent.FireEvent(evt);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0001F658 File Offset: 0x0001D858
	protected override bool TouchEventHandler(YGEvent evt)
	{
		YGEvent.TYPE type = evt.type;
		switch (type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
		case YGEvent.TYPE.TOUCH_MOVE:
			this.SendDrag(evt);
			return true;
		default:
			return type == YGEvent.TYPE.RESET;
		}
	}

	// Token: 0x0400051B RID: 1307
	public EventDispatcher<YGEvent> DragEvent = new EventDispatcher<YGEvent>();
}
