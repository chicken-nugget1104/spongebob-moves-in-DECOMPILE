using System;
using System.Collections.Generic;

// Token: 0x020000E6 RID: 230
public class PaveAction : PersistedTriggerableAction
{
	// Token: 0x06000870 RID: 2160 RVA: 0x000369B8 File Offset: 0x00034BB8
	public PaveAction(Identity id, List<PaveAction.PaveElement> path, Cost cost) : base("np", id)
	{
		this.path = path;
		this.cost = cost;
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000871 RID: 2161 RVA: 0x000369D4 File Offset: 0x00034BD4
	public TriggerableMixin Triggerable
	{
		get
		{
			return this.triggerable;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000872 RID: 2162 RVA: 0x000369DC File Offset: 0x00034BDC
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x000369E0 File Offset: 0x00034BE0
	public new static PaveAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		List<PaveAction.PaveElement> list = new List<PaveAction.PaveElement>();
		List<object> list2 = TFUtils.LoadList<object>(data, "path");
		foreach (object obj in list2)
		{
			Dictionary<string, object> d = (Dictionary<string, object>)obj;
			GridPosition position = new GridPosition(TFUtils.LoadInt(d, "row"), TFUtils.LoadInt(d, "col"));
			list.Add(new PaveAction.PaveElement(position));
		}
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new PaveAction(id, list, cost);
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00036AB8 File Offset: 0x00034CB8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		List<object> list = new List<object>();
		foreach (PaveAction.PaveElement paveElement in this.path)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["col"] = paveElement.position.col;
			dictionary2["row"] = paveElement.position.row;
			list.Add(dictionary2);
		}
		dictionary["path"] = list;
		dictionary["cost"] = this.cost.ToDict();
		return dictionary;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00036B90 File Offset: 0x00034D90
	public override void Apply(Game game, ulong utcNow)
	{
		foreach (PaveAction.PaveElement paveElement in this.path)
		{
			game.terrain.ChangePath(new GridPosition(paveElement.position.row, paveElement.position.col));
		}
		game.resourceManager.Apply(this.cost, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00036C30 File Offset: 0x00034E30
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<GridPosition> list = new List<GridPosition>();
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["pavement"];
		foreach (object obj in list2)
		{
			Dictionary<string, object> d = (Dictionary<string, object>)obj;
			GridPosition newPos = new GridPosition(TFUtils.LoadInt(d, "row"), TFUtils.LoadInt(d, "col"));
			int num = this.path.FindIndex((PaveAction.PaveElement pe) => pe.position == newPos);
			if (num >= 0)
			{
				this.path.RemoveAt(num);
			}
			else
			{
				list.Add(newPos);
			}
		}
		foreach (PaveAction.PaveElement paveElement in this.path)
		{
			list.Add(new GridPosition(paveElement.position.row, paveElement.position.col));
		}
		list2.Clear();
		foreach (GridPosition gridPosition in list)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["row"] = gridPosition.row;
			dictionary["col"] = gridPosition.col;
			list2.Add(dictionary);
		}
		ResourceManager.ApplyCostToGameState(this.cost, gameState);
		base.Confirm(gameState);
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00036E38 File Offset: 0x00035038
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["pave_type"] = 1;
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00036E4C File Offset: 0x0003504C
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x04000617 RID: 1559
	public const string PAVE = "np";

	// Token: 0x04000618 RID: 1560
	public List<PaveAction.PaveElement> path;

	// Token: 0x04000619 RID: 1561
	public Cost cost;

	// Token: 0x020000E7 RID: 231
	public class PaveElement
	{
		// Token: 0x06000879 RID: 2169 RVA: 0x00036E80 File Offset: 0x00035080
		public PaveElement(GridPosition position)
		{
			this.position = position;
		}

		// Token: 0x0400061A RID: 1562
		public GridPosition position;
	}
}
