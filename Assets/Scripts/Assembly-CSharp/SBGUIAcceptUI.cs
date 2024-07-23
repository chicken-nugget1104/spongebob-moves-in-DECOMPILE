using System;

// Token: 0x0200005A RID: 90
public class SBGUIAcceptUI : SBGUIScreen
{
	// Token: 0x06000396 RID: 918 RVA: 0x00011E70 File Offset: 0x00010070
	public override void Deactivate()
	{
		base.ClearButtonActions("accept");
		base.Deactivate();
	}
}
