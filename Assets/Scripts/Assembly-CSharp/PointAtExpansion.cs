using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class PointAtExpansion : SimulationSessionActionDefinition
{
	// Token: 0x06001258 RID: 4696 RVA: 0x0007F3A0 File Offset: 0x0007D5A0
	private PointAtExpansion()
	{
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x0007F3B4 File Offset: 0x0007D5B4
	public static PointAtExpansion Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtExpansion pointAtExpansion = new PointAtExpansion();
		pointAtExpansion.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtExpansion;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x0007F3D4 File Offset: 0x0007D5D4
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.targetDid = TFUtils.LoadInt(data, "slot_id");
		this.pointer.Parse(data, true, Vector3.zero, 1f);
		if (data.ContainsKey("restrict_clicks"))
		{
			this.restrict = (bool)data["restrict_clicks"];
		}
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0007F440 File Offset: 0x0007D640
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> result = base.ToDict();
		this.pointer.AddToDict(ref result);
		return result;
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x0007F464 File Offset: 0x0007D664
	public void SpawnPointer(Session session, SessionActionTracker tracker)
	{
		Game theGame = session.TheGame;
		TFUtils.Assert(theGame.terrain.ExpansionSlots.ContainsKey(this.targetDid), "Missing a target Slot for Point at Expansion: " + this.targetDid);
		TerrainSlot terrainSlot = theGame.terrain.ExpansionSlots[this.targetDid];
		this.pointer.Spawn(theGame, tracker, terrainSlot);
		this.RestrictExpansion(theGame.simulation);
		base.FrameCamera(session.TheCamera, terrainSlot.Position);
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x0007F4F0 File Offset: 0x0007D6F0
	public void RestrictExpansion(Simulation simulation)
	{
		if (this.restrict)
		{
			this.restricted = true;
			RestrictInteraction.AddWhitelistExpansion(simulation, this.targetDid);
			RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			RestrictInteraction.AddWhitelistSimulated(simulation, int.MinValue);
		}
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x0007F528 File Offset: 0x0007D728
	public override void OnDestroy(Game game)
	{
		if (this.restricted)
		{
			RestrictInteraction.RemoveWhitelistExpansion(game.simulation, this.targetDid);
			RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
			RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
			this.restricted = false;
		}
	}

	// Token: 0x04000C94 RID: 3220
	public const string TYPE = "point_at_expansion";

	// Token: 0x04000C95 RID: 3221
	private const string SLOT_ID = "slot_id";

	// Token: 0x04000C96 RID: 3222
	private PointAtExpansion.SimulationExpansionPointer pointer = new PointAtExpansion.SimulationExpansionPointer();

	// Token: 0x04000C97 RID: 3223
	private int targetDid;

	// Token: 0x04000C98 RID: 3224
	private bool restrict;

	// Token: 0x04000C99 RID: 3225
	private bool restricted;

	// Token: 0x02000237 RID: 567
	public class SimulationExpansionPointer : SimulationPointer
	{
		// Token: 0x06001260 RID: 4704 RVA: 0x0007F570 File Offset: 0x0007D770
		public void Spawn(Game game, SessionActionTracker parentAction, TerrainSlot target)
		{
			TFUtils.Assert(target != null, "Must specify a target slot.");
			PointAtExpansion.SimulationExpansionPointer simulationExpansionPointer = new PointAtExpansion.SimulationExpansionPointer();
			simulationExpansionPointer.Initialize(game, parentAction, this.offset, base.Alpha, base.Scale, target);
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x0007F5B0 File Offset: 0x0007D7B0
		public override Vector3 TargetPosition
		{
			get
			{
				TFUtils.Assert(this.slot != null, "Trying to target a slot, but slot target is null!");
				return TFUtils.ExpandVector(this.slot.Position);
			}
		}
	}
}
