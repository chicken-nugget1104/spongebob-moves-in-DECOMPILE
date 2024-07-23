using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000238 RID: 568
public class PointAtSimulated : SimulationSessionActionDefinition
{
	// Token: 0x06001262 RID: 4706 RVA: 0x0007F5E0 File Offset: 0x0007D7E0
	private PointAtSimulated()
	{
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001263 RID: 4707 RVA: 0x0007F5F0 File Offset: 0x0007D7F0
	public Identity TargetId
	{
		get
		{
			return this.targetId;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001264 RID: 4708 RVA: 0x0007F5F8 File Offset: 0x0007D7F8
	public int? TargetDid
	{
		get
		{
			return this.targetDid;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06001265 RID: 4709 RVA: 0x0007F600 File Offset: 0x0007D800
	public string SubHudSubTarget
	{
		get
		{
			return this.subHudSubTarget;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001266 RID: 4710 RVA: 0x0007F608 File Offset: 0x0007D808
	public bool TargetSelected
	{
		get
		{
			return this.targetSelected;
		}
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x0007F610 File Offset: 0x0007D810
	public static PointAtSimulated Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtSimulated pointAtSimulated = new PointAtSimulated();
		pointAtSimulated.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtSimulated;
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x0007F630 File Offset: 0x0007D830
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		string text = TFUtils.TryLoadString(data, "instance_id");
		if (text != null)
		{
			this.targetId = new Identity(text);
		}
		this.targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
		this.targetSelected = (data.ContainsKey("selected") && (bool)data["selected"]);
		this.subHudSubTarget = TFUtils.TryLoadString(data, "subhud_subtarget");
		if (this.subHudSubTarget == null)
		{
			this.simPointer = new PointAtSimulated.SimulationTargetPointer();
			this.simPointer.Parse(data, false, new Vector3(-5f, -5f, 0f), 1f);
		}
		else
		{
			this.subHudPointer = new GuideArrow();
			this.subHudPointer.Parse(data, false, new Vector3(0f, -0.3f, 0f), 0.01f);
		}
		if (data.ContainsKey("restrict_clicks"))
		{
			this.restrict = (bool)data["restrict_clicks"];
		}
		if (data.ContainsKey("frame_camera"))
		{
			this.frameCamera = (bool)data["frame_camera"];
		}
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x0007F778 File Offset: 0x0007D978
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		if (this.simPointer != null)
		{
			this.simPointer.AddToDict(ref dictionary);
		}
		if (this.subHudPointer != null)
		{
			this.subHudPointer.AddToDict(ref dictionary);
		}
		if (this.targetId != null)
		{
			dictionary["instance_id"] = this.targetId;
		}
		int? num = this.targetDid;
		if (num != null)
		{
			dictionary["definition_id"] = this.targetDid;
		}
		if (this.targetSelected)
		{
			dictionary["selected"] = this.targetSelected;
		}
		dictionary["subhud_subtarget"] = this.subHudSubTarget;
		dictionary["restrict_clicks"] = this.restrict;
		dictionary["frame_camera"] = this.frameCamera;
		return dictionary;
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x0007F860 File Offset: 0x0007DA60
	public void SpawnSimulatedPointer(Session session, SessionActionTracker tracker, Simulated target, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
		TFUtils.Assert(this.subHudPointer == null, "This PointAtSimulated was parsed to point at a SubHud and not a Simulated!");
		List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(target.entity.DefinitionId, target.Id, true);
		if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].m_pTaskData.m_eTaskType == TaskData._eTaskType.eEnter)
		{
			return;
		}
		this.simPointer.Spawn(session.TheGame, tracker, target);
		this.RestrictSimulated(session.TheGame.simulation);
		if (this.frameCamera)
		{
			base.FrameCamera(session.TheCamera, target.PositionCenter);
		}
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x0007F908 File Offset: 0x0007DB08
	public void SpawnSubHudPointer(Session session, SessionActionTracker tracker, Simulated target, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
		TFUtils.Assert(this.simPointer == null, "This PointAtSimulated was parsed to point at a Simulated and not its subHud!");
		this.subHudUi = subTarget;
		this.subHudPointer.Spawn(session.TheGame, tracker, subTarget, subTargetContainer);
		this.RestrictSimulated(session.TheGame.simulation);
		if (this.frameCamera)
		{
			base.FrameCamera(session.TheCamera, target.PositionCenter);
		}
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x0007F974 File Offset: 0x0007DB74
	public void RestrictSimulated(Simulation simulation)
	{
		if (this.restrict)
		{
			this.restricted = true;
			TFUtils.Assert(this.targetId != null || this.targetDid != null, "Restricted Point At Simulated action has no valid target restriction - must specify target ID or DID.");
			if (this.subHudUi != null)
			{
				RestrictInteraction.AddWhitelistElement(this.subHudUi);
			}
			else if (this.targetId != null)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, this.targetId);
			}
			else if (this.targetDid != null)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, this.targetDid.Value);
			}
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.AddWhitelistSimulated(simulation, int.MinValue);
			RestrictInteraction.AddWhitelistExpansion(simulation, int.MinValue);
		}
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x0007FA34 File Offset: 0x0007DC34
	public override void OnDestroy(Game game)
	{
		if (this.restricted)
		{
			if (this.subHudPointer != null && this.subHudUi != null)
			{
				RestrictInteraction.RemoveWhitelistElement(this.subHudUi);
			}
			else if (this.targetId != null)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, this.targetId);
			}
			else if (this.targetDid != null)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, this.targetDid.Value);
			}
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			this.restricted = false;
		}
	}

	// Token: 0x04000C9A RID: 3226
	public const string TYPE = "point_at_simulated";

	// Token: 0x04000C9B RID: 3227
	private const string SELECTED = "selected";

	// Token: 0x04000C9C RID: 3228
	private const string INSTANCE_ID = "instance_id";

	// Token: 0x04000C9D RID: 3229
	private const string DEFINITION_ID = "definition_id";

	// Token: 0x04000C9E RID: 3230
	private const string SUBHUD_SUBTARGET = "subhud_subtarget";

	// Token: 0x04000C9F RID: 3231
	private const string RESTRICT_CLICKS = "restrict_clicks";

	// Token: 0x04000CA0 RID: 3232
	private const string FRAME_CAMERA = "frame_camera";

	// Token: 0x04000CA1 RID: 3233
	private PointAtSimulated.SimulationTargetPointer simPointer;

	// Token: 0x04000CA2 RID: 3234
	private GuideArrow subHudPointer;

	// Token: 0x04000CA3 RID: 3235
	private SBGUIElement subHudUi;

	// Token: 0x04000CA4 RID: 3236
	private Identity targetId;

	// Token: 0x04000CA5 RID: 3237
	private int? targetDid;

	// Token: 0x04000CA6 RID: 3238
	private bool targetSelected;

	// Token: 0x04000CA7 RID: 3239
	private bool restrict;

	// Token: 0x04000CA8 RID: 3240
	private bool restricted;

	// Token: 0x04000CA9 RID: 3241
	private bool frameCamera = true;

	// Token: 0x04000CAA RID: 3242
	private string subHudSubTarget;

	// Token: 0x02000239 RID: 569
	public class SimulationTargetPointer : SimulationPointer
	{
		// Token: 0x0600126F RID: 4719 RVA: 0x0007FAF8 File Offset: 0x0007DCF8
		public void Spawn(Game game, SessionActionTracker parentAction, Simulated target)
		{
			TFUtils.Assert(target != null, "Must specify a target simulated.");
			PointAtSimulated.SimulationTargetPointer simulationTargetPointer = new PointAtSimulated.SimulationTargetPointer();
			simulationTargetPointer.Initialize(game, parentAction, this.offset, base.Alpha, base.Scale, target);
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x0007FB38 File Offset: 0x0007DD38
		public override Vector3 TargetPosition
		{
			get
			{
				TFUtils.Assert(this.simulated != null, "Trying to target a simulated, but simulated target is null!");
				return TFUtils.ExpandVector(this.simulated.Position) + this.simulated.ThoughtDisplayOffsetWorld;
			}
		}
	}
}
