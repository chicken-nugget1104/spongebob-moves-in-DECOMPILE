using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000250 RID: 592
public static class SessionActionUiHelper
{
	// Token: 0x06001321 RID: 4897 RVA: 0x00083F00 File Offset: 0x00082100
	public static void HandleCommonSessionActions(Session session, List<SBGUIScreen> screens, SessionActionTracker action)
	{
		string type = action.Definition.Type;
		if (SessionActionUiHelper.APPLICABLE_TARGETING_TYPES.Contains(type))
		{
			SessionActionUiHelper.<HandleCommonSessionActions>c__AnonStorey108 <HandleCommonSessionActions>c__AnonStorey2 = new SessionActionUiHelper.<HandleCommonSessionActions>c__AnonStorey108();
			<HandleCommonSessionActions>c__AnonStorey2.definition = (UiTargetingSessionActionDefinition)action.Definition;
			string target = <HandleCommonSessionActions>c__AnonStorey2.definition.Target;
			SBGUIScreen screen;
			foreach (SBGUIScreen screen2 in screens)
			{
				screen = screen2;
				SBGUIElement sbguielement = screen.FindChildSessionActionId(target, false);
				if (sbguielement == null && screen is SBGUIScrollableDialog)
				{
					SessionActionUiHelper.LoadDialogBuffer();
					SBGUIScrollableDialog sbguiscrollableDialog = screen as SBGUIScrollableDialog;
					if (sbguiscrollableDialog.region != null && sbguiscrollableDialog.region.subViewMarker != null)
					{
						SBGUIElement[] componentsInChildren = sbguiscrollableDialog.region.subViewMarker.GetComponentsInChildren<SBGUIElement>(false);
						for (int i = componentsInChildren.Length - 1; i > 0; i--)
						{
							if (componentsInChildren[i].SessionActionId == target)
							{
								sbguielement = componentsInChildren[i];
								break;
							}
						}
					}
				}
				if (sbguielement != null)
				{
					if (<HandleCommonSessionActions>c__AnonStorey2.definition.DynamicSubTarget != null)
					{
						sbguielement = sbguielement.FindDynamicSubElementSessionActionId(<HandleCommonSessionActions>c__AnonStorey2.definition.DynamicSubTarget, false);
						TFUtils.Assert(sbguielement != null, "Had problems finding the DynamicSubTarget(" + <HandleCommonSessionActions>c__AnonStorey2.definition.DynamicSubTarget + ")");
						<HandleCommonSessionActions>c__AnonStorey2.definition.Handle(session, action, sbguielement, screen);
					}
					else if (<HandleCommonSessionActions>c__AnonStorey2.definition.DynamicScrolledSubTarget != null)
					{
						SBGUISlottedScrollableDialog sbguislottedScrollableDialog = (SBGUISlottedScrollableDialog)sbguielement;
						sbguislottedScrollableDialog.FindDynamicSubElementInScrollRegionSessionActionIdAsync(<HandleCommonSessionActions>c__AnonStorey2.definition.DynamicScrolledSubTarget, delegate(SBGUIElement foundElement)
						{
							<HandleCommonSessionActions>c__AnonStorey2.definition.Handle(session, action, foundElement, screen);
						});
					}
					else
					{
						<HandleCommonSessionActions>c__AnonStorey2.definition.Handle(session, action, sbguielement, screen);
					}
				}
			}
		}
		else if (SessionActionUiHelper.APPLICABLE_GENERAL_TYPES.Contains(type))
		{
			TextPrompt textPrompt = (TextPrompt)action.Definition;
			foreach (SBGUIScreen containingScreen in screens)
			{
				textPrompt.Handle(session, action, containingScreen);
			}
		}
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x00084200 File Offset: 0x00082400
	private static IEnumerator LoadDialogBuffer()
	{
		yield return new WaitForSeconds(3f);
		yield break;
	}

	// Token: 0x04000D39 RID: 3385
	private static List<string> APPLICABLE_TARGETING_TYPES = new List<string>
	{
		"activate_hud_button",
		"quest_reminder",
		"tutorial_hand_pointer",
		"point_at_element",
		"screenmask_element"
	};

	// Token: 0x04000D3A RID: 3386
	private static List<string> APPLICABLE_GENERAL_TYPES = new List<string>
	{
		"text_prompt"
	};
}
