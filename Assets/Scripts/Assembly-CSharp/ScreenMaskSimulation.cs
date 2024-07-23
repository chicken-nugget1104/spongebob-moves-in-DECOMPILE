using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000241 RID: 577
public class ScreenMaskSimulation : SessionActionDefinition
{
	// Token: 0x06001297 RID: 4759 RVA: 0x000803C4 File Offset: 0x0007E5C4
	private ScreenMaskSimulation(ScreenMaskSpawn.ScreenMaskType maskType)
	{
		this.maskType = maskType;
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x000803E0 File Offset: 0x0007E5E0
	public static ScreenMaskSimulation Create(ScreenMaskSpawn.ScreenMaskType maskType, Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ScreenMaskSimulation screenMaskSimulation = new ScreenMaskSimulation(maskType);
		screenMaskSimulation.Parse(data, id, startConditions, originatedFromQuest);
		return screenMaskSimulation;
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06001299 RID: 4761 RVA: 0x00080400 File Offset: 0x0007E600
	public Identity TargetId
	{
		get
		{
			return this.targetId;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x0600129A RID: 4762 RVA: 0x00080408 File Offset: 0x0007E608
	public int? TargetDid
	{
		get
		{
			return this.targetDid;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600129B RID: 4763 RVA: 0x00080410 File Offset: 0x0007E610
	public string SubHudSubTarget
	{
		get
		{
			return this.subHudSubTarget;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x0600129C RID: 4764 RVA: 0x00080418 File Offset: 0x0007E618
	public bool TargetSelected
	{
		get
		{
			return this.targetSelected;
		}
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x00080420 File Offset: 0x0007E620
	public void SpawnSimulationMask(Game game, SessionActionTracker tracker)
	{
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.SIMULATION, game, tracker, null, null, null, null, this.radius, this.texture, this.offset, false);
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0008044C File Offset: 0x0007E64C
	public void SpawnSimulatedMask(Game game, SessionActionTracker tracker, Simulated target)
	{
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.SIMULATED, game, tracker, null, null, target, null, this.radius, this.texture, this.offset, false);
		this.RestrictSimulated(game.simulation);
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x00080484 File Offset: 0x0007E684
	public void SpawnSubHudMask(Game game, SessionActionTracker tracker, SBGUIElement subTarget, SBGUIScreen subTargetContainer)
	{
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.ELEMENT, game, tracker, subTarget, subTargetContainer, null, null, this.radius, this.texture, this.offset, false);
		this.subHudUi = subTarget;
		this.RestrictSimulated(game.simulation);
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x000804C4 File Offset: 0x0007E6C4
	public void SpawnExpansionMask(Game game, SessionActionTracker tracker)
	{
		TFUtils.Assert(this.slotId != null && game.terrain.ExpansionSlots.ContainsKey(this.slotId.Value), "Trying to startup an Expansion Mask without a valid slot id given");
		TerrainSlot slot = game.terrain.ExpansionSlots[this.slotId.Value];
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.EXPANSION, game, tracker, null, null, null, slot, this.radius, this.texture, this.offset, false);
		this.RestrictExpansion(game.simulation);
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x00080550 File Offset: 0x0007E750
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.radius = (float)TFUtils.LoadInt(data, "radius") * 0.01f;
		this.texture = TFUtils.TryLoadString(data, "texture");
		if (data.ContainsKey("offset"))
		{
			TFUtils.LoadVector3(out this.offset, TFUtils.LoadDict(data, "offset"));
		}
		if (this.maskType == ScreenMaskSpawn.ScreenMaskType.SIMULATED)
		{
			string text = TFUtils.TryLoadString(data, "instance_id");
			if (text != null)
			{
				this.targetId = new Identity(text);
			}
			this.targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
			this.targetSelected = (data.ContainsKey("selected") && (bool)data["selected"]);
			this.subHudSubTarget = TFUtils.TryLoadString(data, "subhud_subtarget");
		}
		if (this.maskType == ScreenMaskSpawn.ScreenMaskType.EXPANSION)
		{
			TFUtils.Assert(data.ContainsKey("slot_id"), "Setup an Expansion Screenmask without defining a 'slot_id' in the definition.");
			this.slotId = TFUtils.TryLoadInt(data, "slot_id");
		}
		if ((this.maskType == ScreenMaskSpawn.ScreenMaskType.SIMULATED || this.maskType == ScreenMaskSpawn.ScreenMaskType.EXPANSION) && data.ContainsKey("restrict_clicks"))
		{
			this.restrict = (bool)data["restrict_clicks"];
		}
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x000806A8 File Offset: 0x0007E8A8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["radius"] = this.radius;
		dictionary["texture"] = this.texture;
		dictionary["offset"] = this.offset;
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
		return dictionary;
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x00080788 File Offset: 0x0007E988
	public void RestrictSimulated(Simulation simulation)
	{
		if (this.restrict)
		{
			this.restricted = true;
			TFUtils.Assert(this.targetId != null || this.targetDid != null, "Restricted Screenmask Simulated action has no valid target restriction - must specify target ID or DID.");
			if (this.targetId != null)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, this.targetId);
			}
			else if (this.targetDid != null)
			{
				RestrictInteraction.AddWhitelistSimulated(simulation, this.targetDid.Value);
			}
			if (this.subHudUi != null)
			{
				RestrictInteraction.AddWhitelistElement(this.subHudUi);
			}
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.AddWhitelistExpansion(simulation, int.MinValue);
		}
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x00080838 File Offset: 0x0007EA38
	public void RestrictExpansion(Simulation simulation)
	{
		if (this.restrict)
		{
			this.restricted = true;
			TFUtils.Assert(this.slotId != null, "Restricted Screenmask Expansion action has no valid target restriction - must specify target ID or DID.");
			RestrictInteraction.AddWhitelistExpansion(simulation, this.slotId.Value);
			RestrictInteraction.AddWhitelistSimulated(simulation, int.MinValue);
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
		}
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x00080894 File Offset: 0x0007EA94
	public override void OnDestroy(Game game)
	{
		if (this.restricted)
		{
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			if (this.targetId != null)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, this.targetId);
			}
			else if (this.targetDid != null)
			{
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, this.targetDid.Value);
			}
			if (this.subHudUi != null)
			{
				RestrictInteraction.RemoveWhitelistElement(this.subHudUi);
			}
			if (this.slotId != null)
			{
				RestrictInteraction.RemoveWhitelistExpansion(game.simulation, this.slotId.Value);
				RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			}
			else
			{
				RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
			}
			this.restricted = false;
		}
	}

	// Token: 0x04000CC6 RID: 3270
	public const string TYPE_SIMULATED = "screenmask_simulated";

	// Token: 0x04000CC7 RID: 3271
	public const string TYPE_SIMULATION = "screenmask_simulation";

	// Token: 0x04000CC8 RID: 3272
	public const string TYPE_EXPANSION = "screenmask_expansion";

	// Token: 0x04000CC9 RID: 3273
	private const string RADIUS = "radius";

	// Token: 0x04000CCA RID: 3274
	private const string TEXTURE = "texture";

	// Token: 0x04000CCB RID: 3275
	private const string OFFSET = "offset";

	// Token: 0x04000CCC RID: 3276
	private const string SELECTED = "selected";

	// Token: 0x04000CCD RID: 3277
	private const string INSTANCE_ID = "instance_id";

	// Token: 0x04000CCE RID: 3278
	private const string DEFINITION_ID = "definition_id";

	// Token: 0x04000CCF RID: 3279
	private const string SLOT_ID = "slot_id";

	// Token: 0x04000CD0 RID: 3280
	private const string SUBHUD_SUBTARGET = "subhud_subtarget";

	// Token: 0x04000CD1 RID: 3281
	private ScreenMaskSpawn.ScreenMaskType maskType;

	// Token: 0x04000CD2 RID: 3282
	private float radius;

	// Token: 0x04000CD3 RID: 3283
	private Vector3 offset = Vector3.zero;

	// Token: 0x04000CD4 RID: 3284
	private string texture;

	// Token: 0x04000CD5 RID: 3285
	private Identity targetId;

	// Token: 0x04000CD6 RID: 3286
	private int? targetDid;

	// Token: 0x04000CD7 RID: 3287
	private bool targetSelected;

	// Token: 0x04000CD8 RID: 3288
	private string subHudSubTarget;

	// Token: 0x04000CD9 RID: 3289
	private SBGUIElement subHudUi;

	// Token: 0x04000CDA RID: 3290
	private int? slotId;

	// Token: 0x04000CDB RID: 3291
	private bool restrict;

	// Token: 0x04000CDC RID: 3292
	private bool restricted;
}
