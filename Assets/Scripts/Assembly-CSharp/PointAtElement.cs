using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class PointAtElement : UiTargetingSessionActionDefinition
{
	// Token: 0x06001253 RID: 4691 RVA: 0x0007F200 File Offset: 0x0007D400
	private PointAtElement()
	{
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x0007F214 File Offset: 0x0007D414
	public static PointAtElement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtElement pointAtElement = new PointAtElement();
		pointAtElement.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtElement;
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x0007F234 File Offset: 0x0007D434
	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		TFUtils.Assert(target.SessionActionId == base.Target || target.name == base.DynamicSubTarget || target.name == base.DynamicScrolledSubTarget, string.Format("Calling handle on an element that does not match an expected target! Expected {0}, {1}, or {2}. Found {3}/{4}.", new object[]
		{
			base.Target,
			base.DynamicSubTarget,
			base.DynamicScrolledSubTarget,
			target.SessionActionId,
			target.name
		}));
		if (!this.pointer.ElementIsInGoodState(target))
		{
			return;
		}
		foreach (SBGUIElement sbguielement in target.GetComponentsInChildren<SBGUIElement>())
		{
			if (sbguielement.name != null && sbguielement.name.Contains("TutorialPointer"))
			{
				return;
			}
		}
		containingScreen.UsedInSessionAction = true;
		this.pointer.Spawn(session.TheGame, action, target, containingScreen);
		base.Handle(session, action, target, containingScreen);
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x0007F34C File Offset: 0x0007D54C
	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, originatedFromQuest);
		this.pointer.Parse(data, false, Vector3.zero, 0.01f);
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x0007F37C File Offset: 0x0007D57C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> result = base.ToDict();
		this.pointer.AddToDict(ref result);
		return result;
	}

	// Token: 0x04000C92 RID: 3218
	public const string TYPE = "point_at_element";

	// Token: 0x04000C93 RID: 3219
	private GuideArrow pointer = new GuideArrow();
}
