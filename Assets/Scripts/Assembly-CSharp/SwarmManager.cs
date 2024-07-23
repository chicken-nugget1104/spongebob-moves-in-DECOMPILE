using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025F RID: 607
public class SwarmManager
{
	// Token: 0x06001383 RID: 4995 RVA: 0x00086234 File Offset: 0x00084434
	public SwarmManager()
	{
		this.outsideList = new List<ResidentEntity>();
		this.transitionList = new List<ResidentEntity>();
		this.inHomeList = new Dictionary<Simulated, Identity>();
		if (CommonUtils.TextureLod() < CommonUtils.LevelOfDetail.Standard)
		{
			this.minSoftResidents = 0;
			this.maxSoftResidents = 1000;
		}
		else
		{
			this.minSoftResidents = 0;
			this.maxSoftResidents = 1000;
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06001385 RID: 4997 RVA: 0x000862A0 File Offset: 0x000844A0
	public static SwarmManager Instance
	{
		get
		{
			if (SwarmManager._instance == null)
			{
				SwarmManager._instance = new SwarmManager();
			}
			return SwarmManager._instance;
		}
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x000862BC File Offset: 0x000844BC
	public void Cleanup()
	{
		this.outsideList.Clear();
		this.transitionList.Clear();
		this.inHomeList.Clear();
		SwarmManager._instance = null;
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x000862E8 File Offset: 0x000844E8
	public void AddResident(ResidentEntity entity)
	{
		if (entity == null)
		{
			TFUtils.WarningLog("Tried to add a null resident entity to swarm manager.");
			return;
		}
		if (!this.outsideList.Contains(entity))
		{
			this.outsideList.Add(entity);
		}
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00086324 File Offset: 0x00084524
	public void RemoveResident(ResidentEntity entity, Simulated building)
	{
		if (entity == null)
		{
			TFUtils.WarningLog("Tried to remove a null resident entity to swarm manager.");
			return;
		}
		if (this.outsideList.Contains(entity))
		{
			this.outsideList.Remove(entity);
		}
		if (this.transitionList.Contains(entity))
		{
			this.transitionList.Remove(entity);
		}
		if (building != null && this.inHomeList.ContainsKey(building))
		{
			this.inHomeList.Remove(building);
		}
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x000863A4 File Offset: 0x000845A4
	public void SwitchResident(ResidentEntity entity)
	{
		if (entity == null)
		{
			TFUtils.WarningLog("Tried to switch a null resident entity to swarm manager.");
			return;
		}
		if (this.outsideList.Contains(entity))
		{
			this.outsideList.Remove(entity);
		}
		if (!this.transitionList.Contains(entity))
		{
			this.transitionList.Add(entity);
		}
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x00086400 File Offset: 0x00084600
	public void RestoreResidents(Simulation simulation, Simulated building)
	{
		if (!this.inHomeList.ContainsKey(building))
		{
			return;
		}
		List<int> residentDids = building.GetEntity<BuildingEntity>().ResidentDids;
		foreach (int value in residentDids)
		{
			if (Simulated.Building.TryAddResident(simulation, building, new int?(value), null) == null)
			{
				TFUtils.ErrorLog("SwarmManager.RestoreResident - Failed to retrieve resident from building");
			}
			if (this.inHomeList.ContainsKey(building))
			{
				this.inHomeList.Remove(building);
			}
		}
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x000864B8 File Offset: 0x000846B8
	private void MoveResidentOutside(Simulation simulation)
	{
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x000864BC File Offset: 0x000846BC
	private List<ResidentEntity> GetValidOutsideList()
	{
		List<ResidentEntity> list = new List<ResidentEntity>();
		foreach (ResidentEntity item in this.outsideList)
		{
			list.Add(item);
		}
		return list;
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x0008652C File Offset: 0x0008472C
	private void MoveResidentInside(Simulation simulation)
	{
		ResidentEntity residentEntity = null;
		List<ResidentEntity> validOutsideList = this.GetValidOutsideList();
		while (residentEntity == null && validOutsideList.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, validOutsideList.Count);
			residentEntity = validOutsideList[index];
			if (!residentEntity.HomeAvailability)
			{
				validOutsideList.Remove(residentEntity);
				residentEntity = null;
			}
		}
		if (residentEntity == null)
		{
			return;
		}
		this.SwitchResident(residentEntity);
		Simulated simulated = simulation.FindSimulated(residentEntity.Residence);
		if (simulated == null)
		{
			TFUtils.ErrorLog("Failed to find residents home");
		}
		simulation.FindSimulated(residentEntity.Id).ClearPendingCommands();
		simulation.Router.Send(GoHomeCommand.Create(residentEntity.Id, residentEntity.Id, simulated.PointOfInterest));
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x000865E4 File Offset: 0x000847E4
	public void StoreResident(Simulation simulation, ResidentEntity entity)
	{
		if (entity == null)
		{
			TFUtils.ErrorLog("Tried to store a null resident in swarm manager");
		}
		if (this.outsideList.Contains(entity))
		{
			this.outsideList.Remove(entity);
		}
		if (this.transitionList.Contains(entity))
		{
			this.transitionList.Remove(entity);
		}
		Simulated key = simulation.FindSimulated(entity.Residence);
		this.inHomeList.Add(key, entity.Id);
		ulong hungryAt = entity.HungryAt;
		ulong num = TFUtils.EpochTime();
		ulong hungryAt2 = hungryAt - num;
		entity.HungryAt = hungryAt2;
		Simulated simulated = simulation.FindSimulated(entity.Id);
		simulation.RemoveSimulated(simulated);
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x0008668C File Offset: 0x0008488C
	public void ResidentInIdle(Simulation simulation, ResidentEntity entity)
	{
		Simulated simulated = simulation.FindSimulated(entity.Residence);
		if (simulated == null)
		{
			TFUtils.ErrorLog("Failed to find residents home");
		}
		simulation.FindSimulated(entity.Id).ClearPendingCommands();
		simulation.Router.Send(GoHomeCommand.Create(entity.Id, entity.Id, simulated.PointOfInterest));
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x000866EC File Offset: 0x000848EC
	public void OnUpdate(Simulation simulation, float dT)
	{
		this.nextShuffle -= dT;
		if (this.nextShuffle <= 0f)
		{
			this.ShuffleResident(simulation);
			this.nextShuffle = dT + UnityEngine.Random.Range(10f, 30f);
		}
		if (this.outsideList.Count < this.minSoftResidents)
		{
			this.MoveResidentOutside(simulation);
		}
		else if (this.outsideList.Count > this.maxSoftResidents)
		{
			this.MoveResidentInside(simulation);
		}
		if (this.transitionList.Count > 0)
		{
			foreach (ResidentEntity residentEntity in this.transitionList)
			{
				if (residentEntity.Invariable["action"] is Simulated.Resident.IdleState)
				{
					this.ResidentInIdle(simulation, residentEntity);
				}
			}
		}
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x000867FC File Offset: 0x000849FC
	private void ShuffleResident(Simulation simulation)
	{
	}

	// Token: 0x04000D8E RID: 3470
	public const int LOW_MEM_SOFT_MIN = 0;

	// Token: 0x04000D8F RID: 3471
	public const int LOW_MEM_SOFT_MAX = 1000;

	// Token: 0x04000D90 RID: 3472
	public const int HIGH_MEM_SOFT_MIN = 0;

	// Token: 0x04000D91 RID: 3473
	public const int HIGH_MEM_SOFT_MAX = 1000;

	// Token: 0x04000D92 RID: 3474
	private const float SHUFFLE_INTERVAL_MIN = 10f;

	// Token: 0x04000D93 RID: 3475
	private const float SHUFFLE_INTERVAL_MAX = 30f;

	// Token: 0x04000D94 RID: 3476
	private static SwarmManager _instance;

	// Token: 0x04000D95 RID: 3477
	private float nextShuffle;

	// Token: 0x04000D96 RID: 3478
	private List<ResidentEntity> outsideList;

	// Token: 0x04000D97 RID: 3479
	private List<ResidentEntity> transitionList;

	// Token: 0x04000D98 RID: 3480
	private Dictionary<Simulated, Identity> inHomeList;

	// Token: 0x04000D99 RID: 3481
	private int minSoftResidents;

	// Token: 0x04000D9A RID: 3482
	private int maxSoftResidents;
}
