using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class ItemDropManager
{
	// Token: 0x170001C4 RID: 452
	// (set) Token: 0x06000D0C RID: 3340 RVA: 0x000507B8 File Offset: 0x0004E9B8
	public Action DialogNeededCallback
	{
		set
		{
			this.dialogNeededCallback = value;
		}
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x000507C4 File Offset: 0x0004E9C4
	public void AddPickupTrigger(Dictionary<string, object> newTrigger)
	{
		if (!newTrigger.ContainsKey("dropID"))
		{
			return;
		}
		string key = (string)newTrigger["dropID"];
		this.pickupTriggers[key] = newTrigger;
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x00050800 File Offset: 0x0004EA00
	public void RemovePickupTrigger(Identity dropID)
	{
		string key = dropID.Describe();
		this.pickupTriggers.Remove(key);
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00050824 File Offset: 0x0004EA24
	public static void AddPickupTriggerToGameState(Dictionary<string, object> gamestate, Dictionary<string, object> newTrigger)
	{
		((List<object>)((Dictionary<string, object>)gamestate["farm"])["drop_pickups"]).Add(newTrigger);
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x0005084C File Offset: 0x0004EA4C
	public static void RemovePickupTriggerFromGameState(Dictionary<string, object> gamestate, string dropID)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gamestate["farm"];
		((List<object>)dictionary["drop_pickups"]).RemoveAll(delegate(object obj)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			return dictionary2.ContainsKey("dropID") && (string)dictionary2["dropID"] == dropID;
		});
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x0005089C File Offset: 0x0004EA9C
	private void DoPickupTrigger(Game game, Identity dropID, Dictionary<string, object> triggerDict)
	{
		if (!triggerDict.ContainsKey("target"))
		{
			return;
		}
		Identity id = new Identity((string)triggerDict["target"]);
		PickupDropAction action = new PickupDropAction(id, dropID);
		game.Record(action);
		ITrigger trigger = Trigger.FromDict(triggerDict);
		game.triggerRouter.RouteTrigger(trigger, game);
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x000508F4 File Offset: 0x0004EAF4
	public void ExecutePickupTrigger(Game game, Identity dropID)
	{
		string key = dropID.Describe();
		if (!this.pickupTriggers.ContainsKey(key))
		{
			return;
		}
		Dictionary<string, object> triggerDict = this.pickupTriggers[key];
		this.pickupTriggers.Remove(key);
		this.DoPickupTrigger(game, dropID, triggerDict);
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00050940 File Offset: 0x0004EB40
	public void ExecuteAllPickupTriggers(Game game)
	{
		foreach (KeyValuePair<string, Dictionary<string, object>> keyValuePair in this.pickupTriggers)
		{
			Dictionary<string, object> value = keyValuePair.Value;
			Identity dropID = new Identity(keyValuePair.Key);
			this.DoPickupTrigger(game, dropID, value);
		}
		this.pickupTriggers.Clear();
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x000509C8 File Offset: 0x0004EBC8
	public void AddDrops(Vector3 initialPosition, List<ItemDropCtor> dropCtors, List<Identity> dropIDs, Simulation simulation)
	{
		Ray ray = simulation.TheCamera.ScreenPointToRay(Vector3.zero);
		float num = (initialPosition - simulation.TheCamera.transform.position).magnitude * 0.5f;
		Vector3 fixedOffset = ray.direction * -num;
		foreach (ItemDropCtor itemDropCtor in dropCtors)
		{
			Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f, 0f), UnityEngine.Random.Range(-1f, 0f), 1f);
			direction.Normalize();
			ItemDrop itemDrop = itemDropCtor.CreateItemDrop(initialPosition, fixedOffset, direction, new Action(this.OnDialogNeeded));
			this.pendingItemDrops.Add(itemDrop);
			dropIDs.Add(itemDrop.DropID);
		}
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x00050AD0 File Offset: 0x0004ECD0
	private void OnDialogNeeded()
	{
		this.dialogNeededCallback();
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00050AE0 File Offset: 0x0004ECE0
	public void OnUpdate(Session session, Camera camera, bool updateCollectionTimer)
	{
		if (this.clearDrops)
		{
			this.clearDrops = false;
			this.PickupAll();
		}
		if (this.DelayItemDrop <= 0f)
		{
			if (this.pendingItemDrops.Count != 0)
			{
				int index = this.pendingItemDrops.Count - 1;
				ItemDrop item = this.pendingItemDrops[index];
				this.pendingItemDrops.RemoveAt(index);
				this.itemDrops.Add(item);
			}
			this.DelayItemDrop = this.delayBetweenItemDrop;
		}
		this.DelayItemDrop -= 1f;
		int count = this.itemDrops.Count;
		if (count != 0)
		{
			List<ItemDrop> list = null;
			List<ItemDrop> list2 = new List<ItemDrop>(this.itemDrops);
			for (int i = 0; i < count; i++)
			{
				if (!list2[i].OnUpdate(session, camera, updateCollectionTimer))
				{
					if (list == null)
					{
						list = new List<ItemDrop>();
					}
					list.Add(list2[i]);
				}
			}
			if (list != null)
			{
				int count2 = list.Count;
				for (int j = 0; j < count2; j++)
				{
					this.itemDrops.Remove(list[j]);
				}
			}
		}
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x00050C18 File Offset: 0x0004EE18
	public void MarkForClearCurrentDrops()
	{
		this.clearDrops = true;
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x00050C24 File Offset: 0x0004EE24
	private void PickupAll()
	{
		int count = this.pendingItemDrops.Count;
		for (int i = 0; i < count; i++)
		{
			this.itemDrops.Add(this.pendingItemDrops[i]);
		}
		this.pendingItemDrops.Clear();
		count = this.itemDrops.Count;
		for (int j = 0; j < count; j++)
		{
			this.itemDrops[j].AutoPickup();
		}
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x00050CA0 File Offset: 0x0004EEA0
	public bool ProcessTap(Session session, Ray ray)
	{
		bool result = false;
		int count = this.itemDrops.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.itemDrops[i].HandleTap(session, ray))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x040008CF RID: 2255
	private const float DELAY_BETWEEN_ITEM_DROP = 5f;

	// Token: 0x040008D0 RID: 2256
	private const float DELAY_TICK = 1f;

	// Token: 0x040008D1 RID: 2257
	private volatile Action dialogNeededCallback;

	// Token: 0x040008D2 RID: 2258
	private List<ItemDrop> itemDrops = new List<ItemDrop>();

	// Token: 0x040008D3 RID: 2259
	private List<ItemDrop> pendingItemDrops = new List<ItemDrop>();

	// Token: 0x040008D4 RID: 2260
	private Dictionary<string, Dictionary<string, object>> pickupTriggers = new Dictionary<string, Dictionary<string, object>>();

	// Token: 0x040008D5 RID: 2261
	private float DelayItemDrop;

	// Token: 0x040008D6 RID: 2262
	private float delayBetweenItemDrop = UnityEngine.Random.Range(4f, 7f);

	// Token: 0x040008D7 RID: 2263
	private bool clearDrops;
}
