using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200023A RID: 570
public class PointAtSimulation : SimulationSessionActionDefinition
{
	// Token: 0x06001271 RID: 4721 RVA: 0x0007FB7C File Offset: 0x0007DD7C
	private PointAtSimulation()
	{
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x0007FB90 File Offset: 0x0007DD90
	public static PointAtSimulation Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PointAtSimulation pointAtSimulation = new PointAtSimulation();
		pointAtSimulation.Parse(data, id, startConditions, originatedFromQuest);
		return pointAtSimulation;
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x0007FBB0 File Offset: 0x0007DDB0
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.pointer.Parse(data, true, Vector3.zero, 1f);
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x0007FBE8 File Offset: 0x0007DDE8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> result = base.ToDict();
		this.pointer.AddToDict(ref result);
		return result;
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0007FC0C File Offset: 0x0007DE0C
	public void SpawnPointer(Session session, SessionActionTracker tracker)
	{
		this.pointer.Spawn(session.TheGame, tracker);
		base.FrameCamera(session.TheCamera, this.pointer.TargetPosition);
	}

	// Token: 0x04000CAB RID: 3243
	public const string TYPE = "point_at_simulation";

	// Token: 0x04000CAC RID: 3244
	private PointAtSimulation.SimulationLocationPointer pointer = new PointAtSimulation.SimulationLocationPointer();

	// Token: 0x0200023B RID: 571
	public class SimulationLocationPointer : SimulationPointer
	{
		// Token: 0x06001277 RID: 4727 RVA: 0x0007FC50 File Offset: 0x0007DE50
		public void Spawn(Game game, SessionActionTracker parentAction)
		{
			PointAtSimulation.SimulationLocationPointer simulationLocationPointer = new PointAtSimulation.SimulationLocationPointer();
			simulationLocationPointer.Initialize(game, parentAction, this.offset, base.Alpha, base.Scale);
		}
	}
}
