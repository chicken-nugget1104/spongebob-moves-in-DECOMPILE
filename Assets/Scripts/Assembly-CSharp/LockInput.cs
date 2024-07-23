using System;
using System.Collections.Generic;

// Token: 0x02000231 RID: 561
public class LockInput : SessionActionDefinition
{
	// Token: 0x0600123E RID: 4670 RVA: 0x0007EE54 File Offset: 0x0007D054
	public static LockInput Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		LockInput lockInput = new LockInput();
		lockInput.Parse(data, id, startConditions, originatedFromQuest);
		return lockInput;
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x0007EE74 File Offset: 0x0007D074
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0007EE88 File Offset: 0x0007D088
	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		RestrictInteraction.AddWhitelistSimulated(session.TheGame.simulation, int.MinValue);
		RestrictInteraction.AddWhitelistExpansion(session.TheGame.simulation, int.MinValue);
		RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
		this.activated = true;
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x0007EED8 File Offset: 0x0007D0D8
	public override void OnDestroy(Game game)
	{
		if (this.activated)
		{
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			this.activated = false;
		}
	}

	// Token: 0x04000C86 RID: 3206
	public const string TYPE = "lock_input";

	// Token: 0x04000C87 RID: 3207
	private bool activated;
}
