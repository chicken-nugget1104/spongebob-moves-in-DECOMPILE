using System;
using System.Collections.Generic;

// Token: 0x02000221 RID: 545
public class ActivateHudButton : UiTargetingSessionActionDefinition
{
	// Token: 0x060011ED RID: 4589 RVA: 0x0007D6CC File Offset: 0x0007B8CC
	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		action.MarkStarted();
		TFUtils.Assert(target as SBGUIButton != null, "HudButton SessionActionDefinition expects target to be a button!");
		((SBGUIButton)target).MockClick();
		action.MarkSucceeded();
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x0007D714 File Offset: 0x0007B914
	public static ActivateHudButton Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ActivateHudButton activateHudButton = new ActivateHudButton();
		activateHudButton.Parse(data, id, startConditions, originatedFromQuest);
		return activateHudButton;
	}

	// Token: 0x04000C42 RID: 3138
	public const string TYPE = "activate_hud_button";
}
