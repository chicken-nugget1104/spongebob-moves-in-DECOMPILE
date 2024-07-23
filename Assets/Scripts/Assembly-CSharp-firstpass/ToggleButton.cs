using System;
using Yarg;

// Token: 0x020000DB RID: 219
public class ToggleButton : BaseButton
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001F8C8 File Offset: 0x0001DAC8
	protected override bool NeedsLoad
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0001F8CC File Offset: 0x0001DACC
	public override void Load()
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.parent;
		this.enabledSprite = ygatlasSprite.LoadSpriteFromAtlas(this.enabledSprite.name, ygatlasSprite.atlasIndex);
		this.disabledSprite = ygatlasSprite.LoadSpriteFromAtlas(this.disabledSprite.name, ygatlasSprite.atlasIndex);
		if (this.buttonEnabled)
		{
			this.TurnOn();
		}
		else
		{
			this.TurnOff();
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0001F93C File Offset: 0x0001DB3C
	public void TurnOn()
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.parent;
		ygatlasSprite.SetUVs(this.enabledSprite);
		this.buttonEnabled = true;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0001F968 File Offset: 0x0001DB68
	public void TurnOff()
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.parent;
		ygatlasSprite.SetUVs(this.disabledSprite);
		this.buttonEnabled = false;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0001F994 File Offset: 0x0001DB94
	protected override bool TouchEventHandler(YGEvent evt)
	{
		YGEvent.TYPE type = evt.type;
		if (type != YGEvent.TYPE.TAP)
		{
			return false;
		}
		if (this.buttonEnabled)
		{
			this.TurnOff();
		}
		else
		{
			this.TurnOn();
		}
		return true;
	}

	// Token: 0x0400052A RID: 1322
	public SpriteCoordinates enabledSprite;

	// Token: 0x0400052B RID: 1323
	public SpriteCoordinates disabledSprite;

	// Token: 0x0400052C RID: 1324
	private bool buttonEnabled = true;
}
