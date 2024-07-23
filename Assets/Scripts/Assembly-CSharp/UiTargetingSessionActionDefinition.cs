using System;
using System.Collections.Generic;

// Token: 0x0200025C RID: 604
public abstract class UiTargetingSessionActionDefinition : SessionActionDefinition
{
	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06001356 RID: 4950 RVA: 0x000854DC File Offset: 0x000836DC
	public string Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06001357 RID: 4951 RVA: 0x000854E4 File Offset: 0x000836E4
	public string DynamicSubTarget
	{
		get
		{
			return this.dynamicSubTarget;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06001358 RID: 4952 RVA: 0x000854EC File Offset: 0x000836EC
	public string DynamicScrolledSubTarget
	{
		get
		{
			return this.dynamicScrolledSubTarget;
		}
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x000854F4 File Offset: 0x000836F4
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		this.Parse(data, id, startConditions, originatedFromQuest, null);
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x00085504 File Offset: 0x00083704
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest, string target)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		string text = this.target = TFUtils.TryLoadString(data, "target");
		if (target == null && text != null)
		{
			this.target = text;
		}
		else if (target != null && text == null)
		{
			this.target = target;
		}
		else if (target != null && text != null)
		{
			TFUtils.ErrorLog("UiTargetingSessionActionDefinition had both a procedural target and a loaded target. Must be one or the other!\ndefinition=" + this);
		}
		else if (target == null && text == null)
		{
			TFUtils.ErrorLog("UiTargetingSessionActionDefinition had neither a procedural target nor a loaded target. Must be one or the other!\ndefinition=" + this);
		}
		this.dynamicSubTarget = TFUtils.TryLoadString(data, "dynamic_subtarget");
		this.dynamicScrolledSubTarget = TFUtils.TryLoadString(data, "dynamic_scrolled_subtarget");
		TFUtils.Assert(this.dynamicSubTarget == null || this.dynamicScrolledSubTarget == null, "Cannot have both a dynamic sub target and a dynamic scrolled sub target. There can be only 1.");
		if (data.ContainsKey("restrict_clicks"))
		{
			this.restrict = (bool)data["restrict_clicks"];
		}
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x00085618 File Offset: 0x00083818
	public virtual void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (this.restrict)
		{
			this.targets.Add(target);
			RestrictInteraction.AddWhitelistElement(target);
			RestrictInteraction.AddWhitelistSimulated(session.TheGame.simulation, int.MinValue);
			RestrictInteraction.AddWhitelistExpansion(session.TheGame.simulation, int.MinValue);
		}
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0008566C File Offset: 0x0008386C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["target"] = this.target;
		dictionary["dynamic_subtarget"] = this.dynamicSubTarget;
		dictionary["dynamic_scrolled_subtarget"] = this.dynamicScrolledSubTarget;
		return dictionary;
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x000856B4 File Offset: 0x000838B4
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			base.ToString(),
			"UiInteractingSessionActionDefinition:(target=",
			this.target,
			", subTarget=",
			this.dynamicSubTarget,
			", scrolledSubTarget=",
			this.dynamicScrolledSubTarget,
			", restrict_clicks=",
			this.restrict,
			")"
		});
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x00085728 File Offset: 0x00083928
	public override void OnDestroy(Game game)
	{
		if (this.targets.Count > 0)
		{
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
		}
		foreach (SBGUIElement sbguielement in this.targets)
		{
			if (sbguielement != null)
			{
				RestrictInteraction.RemoveWhitelistElement(sbguielement);
			}
		}
		this.targets.Clear();
	}

	// Token: 0x04000D6C RID: 3436
	protected const string TARGET = "target";

	// Token: 0x04000D6D RID: 3437
	private const string DYNAMIC_SUBTARGET = "dynamic_subtarget";

	// Token: 0x04000D6E RID: 3438
	private const string DYNAMIC_SCROLLED_SUBTARGET = "dynamic_scrolled_subtarget";

	// Token: 0x04000D6F RID: 3439
	private string target;

	// Token: 0x04000D70 RID: 3440
	private string dynamicSubTarget;

	// Token: 0x04000D71 RID: 3441
	private string dynamicScrolledSubTarget;

	// Token: 0x04000D72 RID: 3442
	private bool restrict;

	// Token: 0x04000D73 RID: 3443
	private List<SBGUIElement> targets = new List<SBGUIElement>();
}
