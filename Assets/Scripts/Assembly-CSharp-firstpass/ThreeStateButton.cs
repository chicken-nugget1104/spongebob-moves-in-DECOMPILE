using System;
using Yarg;

// Token: 0x020000D9 RID: 217
public class ThreeStateButton : BaseButton
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001F794 File Offset: 0x0001D994
	protected override bool NeedsLoad
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0001F798 File Offset: 0x0001D998
	public override void Load()
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.parent;
		this.idle = ygatlasSprite.LoadSpriteFromAtlas(this.idle.name, this.atlasIndex);
		this.hover = ygatlasSprite.LoadSpriteFromAtlas(this.hover.name, this.atlasIndex);
		this.activate = ygatlasSprite.LoadSpriteFromAtlas(this.activate.name, this.atlasIndex);
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0001F808 File Offset: 0x0001DA08
	protected override bool TouchEventHandler(YGEvent evt)
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.parent;
		YGEvent.TYPE type = evt.type;
		switch (type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			if (this.buttonState != ThreeStateButton.BUTTON_STATE.ACTIVATE)
			{
				this.buttonState = ThreeStateButton.BUTTON_STATE.ACTIVATE;
				ygatlasSprite.SetUVs(this.activate);
			}
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
		case YGEvent.TYPE.HOVER:
			if (this.buttonState != ThreeStateButton.BUTTON_STATE.HOVER)
			{
				this.buttonState = ThreeStateButton.BUTTON_STATE.HOVER;
				ygatlasSprite.SetUVs(this.hover);
			}
			return true;
		}
		if (this.buttonState != ThreeStateButton.BUTTON_STATE.IDLE)
		{
			this.buttonState = ThreeStateButton.BUTTON_STATE.IDLE;
			ygatlasSprite.SetUVs(this.idle);
		}
		return true;
	}

	// Token: 0x04000521 RID: 1313
	public int atlasIndex;

	// Token: 0x04000522 RID: 1314
	public SpriteCoordinates idle;

	// Token: 0x04000523 RID: 1315
	public SpriteCoordinates hover;

	// Token: 0x04000524 RID: 1316
	public SpriteCoordinates activate;

	// Token: 0x04000525 RID: 1317
	private ThreeStateButton.BUTTON_STATE buttonState;

	// Token: 0x020000DA RID: 218
	private enum BUTTON_STATE
	{
		// Token: 0x04000527 RID: 1319
		IDLE,
		// Token: 0x04000528 RID: 1320
		HOVER,
		// Token: 0x04000529 RID: 1321
		ACTIVATE
	}
}
