using System;
using UnityEngine;
using Yarg;

// Token: 0x020000DC RID: 220
public class TwoShadeButton : BaseButton
{
	// Token: 0x06000877 RID: 2167 RVA: 0x0001FA28 File Offset: 0x0001DC28
	protected override bool TouchEventHandler(YGEvent evt)
	{
		YGEvent.TYPE type = evt.type;
		switch (type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			if (this.buttonState != TwoShadeButton.BUTTON_STATE.ACTIVATE)
			{
				this.buttonState = TwoShadeButton.BUTTON_STATE.ACTIVATE;
				base.parent.SetColor(this.activate);
			}
			this.ResetHighlightState();
			return true;
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
			break;
		default:
			if (type != YGEvent.TYPE.RESET)
			{
				return false;
			}
			break;
		}
		this.ResetHighlightState();
		return true;
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x0001FA94 File Offset: 0x0001DC94
	public void ResetHighlightState()
	{
		if (this.buttonState != TwoShadeButton.BUTTON_STATE.IDLE)
		{
			this.buttonState = TwoShadeButton.BUTTON_STATE.IDLE;
		}
		base.parent.SetColor(this.idle);
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0001FABC File Offset: 0x0001DCBC
	protected override void OnDisable()
	{
		if (base.parent != null)
		{
			this.ResetHighlightState();
		}
		base.OnDisable();
	}

	// Token: 0x0400052D RID: 1325
	public Color idle = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x0400052E RID: 1326
	public Color activate = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x0400052F RID: 1327
	private TwoShadeButton.BUTTON_STATE buttonState;

	// Token: 0x020000DD RID: 221
	private enum BUTTON_STATE
	{
		// Token: 0x04000531 RID: 1329
		IDLE,
		// Token: 0x04000532 RID: 1330
		ACTIVATE
	}
}
