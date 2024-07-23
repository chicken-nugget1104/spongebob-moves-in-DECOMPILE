using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class PersistedActionBuffer
{
	// Token: 0x06000E07 RID: 3591 RVA: 0x0005558C File Offset: 0x0005378C
	public PersistedActionBuffer(Player p, List<Dictionary<string, object>> actionList)
	{
		this.unconfirmedFile = p.CacheFile(PersistedActionBuffer.ACTION_LIST_FILE);
		this.LoadActionsIntoList(actionList, this.unconfirmed);
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x000555E0 File Offset: 0x000537E0
	public static List<Dictionary<string, object>> LoadActionList(Player p)
	{
		string text = p.CacheFile(PersistedActionBuffer.ACTION_LIST_FILE);
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		try
		{
			if (TFUtils.FileIsExists(text))
			{
				string text2 = TFUtils.ReadAllText(text);
				foreach (string text3 in text2.Split(new char[]
				{
					'\n'
				}))
				{
					if (!string.IsNullOrEmpty(text3))
					{
						TFUtils.DebugLog("Loading json: " + text3);
						Dictionary<string, object> item = (Dictionary<string, object>)Json.Deserialize(text3);
						list.Add(item);
					}
				}
			}
			else
			{
				using (FileStream fileStream = File.Create(text))
				{
					fileStream.Close();
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("No Action File Found: " + ex.Message, LogType.Warning);
		}
		return list;
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x000556F4 File Offset: 0x000538F4
	public void Record(PersistedActionBuffer.PersistedAction action)
	{
		if (!SBSettings.UseActionFile)
		{
			return;
		}
		action.AddEnvelope(TFUtils.EpochTime());
		object obj = this.unconfirmedLock;
		lock (obj)
		{
			this.unconfirmed.Add(action);
			this.RecordActionToFile(action, this.unconfirmedFile);
		}
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x00055768 File Offset: 0x00053968
	public List<PersistedActionBuffer.PersistedAction> GetAllUnackedActions()
	{
		List<PersistedActionBuffer.PersistedAction> list = new List<PersistedActionBuffer.PersistedAction>();
		object obj = this.unconfirmedLock;
		lock (obj)
		{
			list.AddRange(this.unconfirmed);
		}
		return list;
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x000557C0 File Offset: 0x000539C0
	public void DestroyCache()
	{
		object obj = this.unconfirmedLock;
		lock (obj)
		{
			TFUtils.DeleteFile(this.unconfirmedFile);
			this.unconfirmed.Clear();
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00055818 File Offset: 0x00053A18
	public void Flush()
	{
		TFUtils.DebugLog("Flushing action buffer");
		object obj = this.unconfirmedLock;
		lock (obj)
		{
			this.unconfirmed = new List<PersistedActionBuffer.PersistedAction>();
			TFUtils.TruncateFile(this.unconfirmedFile);
		}
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x0005587C File Offset: 0x00053A7C
	private void LoadFileToList(string fileName, List<PersistedActionBuffer.PersistedAction> list)
	{
		string text = TFUtils.ReadAllText(fileName);
		foreach (string text2 in text.Split(new char[]
		{
			'\n'
		}))
		{
			if (!string.IsNullOrEmpty(text2))
			{
				TFUtils.DebugLog("Loading json: " + text2);
				Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(text2);
				PersistedActionBuffer.PersistedAction persistedAction = PersistedActionBuffer.PersistedAction.FromDict(data);
				if (persistedAction != null)
				{
					list.Add(persistedAction);
				}
			}
		}
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x00055900 File Offset: 0x00053B00
	private void LoadActionsIntoList(List<Dictionary<string, object>> src, List<PersistedActionBuffer.PersistedAction> dst)
	{
		foreach (Dictionary<string, object> data in src)
		{
			PersistedActionBuffer.PersistedAction persistedAction = PersistedActionBuffer.PersistedAction.FromDict(data);
			if (persistedAction != null)
			{
				dst.Add(persistedAction);
			}
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x00055970 File Offset: 0x00053B70
	private void RecordActionToFile(PersistedActionBuffer.PersistedAction action, string fileName)
	{
		string text = Json.Serialize(action.ToDict());
		TFUtils.DebugLog("Writing json to ActionBuffer: " + text);
		File.AppendAllText(fileName, text + "\n");
	}

	// Token: 0x04000945 RID: 2373
	private const int BUFFER_SOFT_LIMIT = 1;

	// Token: 0x04000946 RID: 2374
	private List<PersistedActionBuffer.PersistedAction> unconfirmed = new List<PersistedActionBuffer.PersistedAction>();

	// Token: 0x04000947 RID: 2375
	public static string ACTION_LIST_FILE = "actions.json";

	// Token: 0x04000948 RID: 2376
	private object unconfirmedLock = new object();

	// Token: 0x04000949 RID: 2377
	private string unconfirmedFile;

	// Token: 0x020001A7 RID: 423
	public abstract class PersistedAction
	{
		// Token: 0x06000E11 RID: 3601 RVA: 0x000559AC File Offset: 0x00053BAC
		public PersistedAction(string type, Identity target)
		{
			this.type = type;
			this.target = target;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000559C4 File Offset: 0x00053BC4
		static PersistedAction()
		{
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("nb", new PersistedActionBuffer.PersistedAction.ConstructFromDict(NewBuildingAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rb", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RushBuildAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rh", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RushHungerAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cb", new PersistedActionBuffer.PersistedAction.ConstructFromDict(CompleteBuildingAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cr", new PersistedActionBuffer.PersistedAction.ConstructFromDict(CollectRentAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rr", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RushRentAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("m", new PersistedActionBuffer.PersistedAction.ConstructFromDict(MoveAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("np", new PersistedActionBuffer.PersistedAction.ConstructFromDict(PaveAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("s", new PersistedActionBuffer.PersistedAction.ConstructFromDict(SellAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("i", new PersistedActionBuffer.PersistedAction.ConstructFromDict(InitializeAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("fu", new PersistedActionBuffer.PersistedAction.ConstructFromDict(FeedUnitAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("emb", new PersistedActionBuffer.PersistedAction.ConstructFromDict(EarnMatchBonusAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cmb", new PersistedActionBuffer.PersistedAction.ConstructFromDict(CollectMatchBonusAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cs", new PersistedActionBuffer.PersistedAction.ConstructFromDict(CraftStartAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cf", new PersistedActionBuffer.PersistedAction.ConstructFromDict(CraftCompleteAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cc", new PersistedActionBuffer.PersistedAction.ConstructFromDict(CraftCollectAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("aqcc", new PersistedActionBuffer.PersistedAction.ConstructFromDict(AutoQuestCraftCollectAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rc", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RushCraftAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("qp", new PersistedActionBuffer.PersistedAction.ConstructFromDict(QuestProgressAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("qs", new PersistedActionBuffer.PersistedAction.ConstructFromDict(QuestStartAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("qc", new PersistedActionBuffer.PersistedAction.ConstructFromDict(QuestCompleteAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("aqad", new PersistedActionBuffer.PersistedAction.ConstructFromDict(PersistedActionBuffer.PersistedAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rq", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RandomQuestCreateAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("ru", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RandomQuestCleanupAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("aq", new PersistedActionBuffer.PersistedAction.ConstructFromDict(AutoQuestCreateAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("au", new PersistedActionBuffer.PersistedAction.ConstructFromDict(AutoQuestCleanupAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("lu", new PersistedActionBuffer.PersistedAction.ConstructFromDict(LevelUpAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("ne", new PersistedActionBuffer.PersistedAction.ConstructFromDict(NewExpansionAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("ds", new PersistedActionBuffer.PersistedAction.ConstructFromDict(DebrisStartAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("dc", new PersistedActionBuffer.PersistedAction.ConstructFromDict(DebrisCompleteAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rd", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RushDebrisAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("nw", new PersistedActionBuffer.PersistedAction.ConstructFromDict(NewWishAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("fw", new PersistedActionBuffer.PersistedAction.ConstructFromDict(FailWishAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("pd", new PersistedActionBuffer.PersistedAction.ConstructFromDict(PickupDropAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("pr", new PersistedActionBuffer.PersistedAction.ConstructFromDict(PurchaseResourcesAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("uf", new PersistedActionBuffer.PersistedAction.ConstructFromDict(FeatureUnlocksAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("ub", new PersistedActionBuffer.PersistedAction.ConstructFromDict(BuildingUnlocksAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("pcs", new PersistedActionBuffer.PersistedAction.ConstructFromDict(PurchaseCraftingSlotAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cap", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RewardCapAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("tc", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TreasureCollectAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("ts", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TreasureSpawnAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("tu", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TreasureUncoverAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("tt", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TreasureCooldownAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rra", new PersistedActionBuffer.PersistedAction.ConstructFromDict(ReceiveRewardAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("vr", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RestockVendorAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("va", new PersistedActionBuffer.PersistedAction.ConstructFromDict(VendingAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("rrs", new PersistedActionBuffer.PersistedAction.ConstructFromDict(RushRestockAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("sw", new PersistedActionBuffer.PersistedAction.ConstructFromDict(SpawnWandererAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("tw", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TapWandererAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("hw", new PersistedActionBuffer.PersistedAction.ConstructFromDict(HideWandererAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("df", new PersistedActionBuffer.PersistedAction.ConstructFromDict(DisableFleeAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("lr", new PersistedActionBuffer.PersistedAction.ConstructFromDict(LockRecipeAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("cca", new PersistedActionBuffer.PersistedAction.ConstructFromDict(ChangeCostumeAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("uc", new PersistedActionBuffer.PersistedAction.ConstructFromDict(UnlockCostumeAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("tsa", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TaskStartAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("tca", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TaskCompleteAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("tua", new PersistedActionBuffer.PersistedAction.ConstructFromDict(TaskUpdateAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("msa", new PersistedActionBuffer.PersistedAction.ConstructFromDict(MicroEventStartAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("mca", new PersistedActionBuffer.PersistedAction.ConstructFromDict(MicroEventCompleteAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("moa", new PersistedActionBuffer.PersistedAction.ConstructFromDict(MicroEventOpenAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("mcla", new PersistedActionBuffer.PersistedAction.ConstructFromDict(MicroEventCloseAction.FromDict));
			PersistedActionBuffer.PersistedAction.TypeRegistry.Add("sr", new PersistedActionBuffer.PersistedAction.ConstructFromDict(SpawnResidentAction.FromDict));
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0005606C File Offset: 0x0005426C
		protected static string NextTag(ulong timestamp)
		{
			return string.Format("{0}_{1}", timestamp, PersistedActionBuffer.PersistedAction.counter++);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0005609C File Offset: 0x0005429C
		public ulong GetTime()
		{
			if (SBSettings.UseActionFile)
			{
				return this.time;
			}
			return TFUtils.EpochTime();
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000560B4 File Offset: 0x000542B4
		public static PersistedActionBuffer.PersistedAction FromDict(Dictionary<string, object> data)
		{
			if (!data.ContainsKey("type"))
			{
				TFUtils.DebugLog("Attempting to create an action from malformed data! This should not have occurred, locate the source and fix it.");
				return null;
			}
			string text = (string)data["type"];
			if (PersistedActionBuffer.PersistedAction.TypeRegistry.ContainsKey(text))
			{
				PersistedActionBuffer.PersistedAction persistedAction = PersistedActionBuffer.PersistedAction.TypeRegistry[text](data);
				persistedAction.AddEnvelope(TFUtils.LoadUlong(data, "ts", 0UL), TFUtils.LoadString(data, "tag"));
				return persistedAction;
			}
			throw new InvalidOperationException("Unknown type: " + text);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00056148 File Offset: 0x00054348
		public virtual void AddEnvelope(ulong time)
		{
			this.AddEnvelope(time, PersistedActionBuffer.PersistedAction.NextTag(time));
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00056158 File Offset: 0x00054358
		public virtual void AddEnvelope(ulong time, string tag)
		{
			this.time = time;
			this.tag = tag;
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00056168 File Offset: 0x00054368
		public virtual Dictionary<string, object> ToDict()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["ts"] = this.time;
			dictionary["type"] = this.type;
			dictionary["target"] = this.target.Describe();
			dictionary["tag"] = this.tag;
			return dictionary;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x000561CC File Offset: 0x000543CC
		public string DebugToString()
		{
			return TFUtils.DebugDictToString(this.ToDict());
		}

		// Token: 0x06000E1A RID: 3610
		public abstract void Apply(Game game, ulong utcNow);

		// Token: 0x06000E1B RID: 3611 RVA: 0x000561DC File Offset: 0x000543DC
		public virtual void Confirm(Dictionary<string, object> gameState)
		{
			((Dictionary<string, object>)gameState["farm"])["last_action"] = this.tag;
		}

		// Token: 0x06000E1C RID: 3612
		public abstract void Process(Game game);

		// Token: 0x0400094A RID: 2378
		public static Dictionary<string, PersistedActionBuffer.PersistedAction.ConstructFromDict> TypeRegistry = new Dictionary<string, PersistedActionBuffer.PersistedAction.ConstructFromDict>();

		// Token: 0x0400094B RID: 2379
		private static int counter = 0;

		// Token: 0x0400094C RID: 2380
		public string type;

		// Token: 0x0400094D RID: 2381
		public Identity target;

		// Token: 0x0400094E RID: 2382
		private ulong time;

		// Token: 0x0400094F RID: 2383
		public string tag;

		// Token: 0x0200049F RID: 1183
		// (Invoke) Token: 0x060024DF RID: 9439
		public delegate PersistedActionBuffer.PersistedAction ConstructFromDict(Dictionary<string, object> dict);
	}
}
