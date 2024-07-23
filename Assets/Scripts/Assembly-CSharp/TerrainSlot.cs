using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000467 RID: 1127
public class TerrainSlot
{
	// Token: 0x06002380 RID: 9088 RVA: 0x000DB394 File Offset: 0x000D9594
	public TerrainSlot(Dictionary<string, object> data)
	{
		int row = TFUtils.LoadInt(data, "row");
		int col = TFUtils.LoadInt(data, "col");
		this.did = TFUtils.LoadInt(data, "did");
		this.tier = TFUtils.LoadInt(data, "tier");
		this.position = new GridPosition(row, col);
		if (data.ContainsKey("is_boardwalk"))
		{
			this.isBoardwalk = TFUtils.LoadBool(data, "is_boardwalk");
		}
		else
		{
			this.isBoardwalk = false;
		}
		if (data.ContainsKey("cost") && data["cost"] != null)
		{
			this.cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		}
		this.debris = TerrainSlot.LoadExpansionObjectData((List<object>)data["debris"]);
		this.landmarks = TerrainSlot.LoadExpansionObjectData((List<object>)data["landmarks"]);
		this.sectors = new List<GridPosition>();
		foreach (object obj in ((List<object>)data["sectors"]))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)obj;
			int row2 = TFUtils.LoadInt(d, "row");
			int col2 = TFUtils.LoadInt(d, "col");
			this.sectors.Add(new GridPosition(row2, col2));
		}
		int num = 120;
		this.outlinePoints = new List<Vector3>();
		foreach (object obj2 in ((List<object>)data["outline"]))
		{
			Dictionary<string, object> d2 = (Dictionary<string, object>)obj2;
			int num2 = TFUtils.LoadInt(d2, "row");
			int num3 = TFUtils.LoadInt(d2, "col");
			this.outlinePoints.Add(new Vector3((float)(num3 * num), (float)(num2 * num), 0f));
		}
		this.requiredSlots = ((List<object>)data["required_slots"]).ConvertAll<int>((object x) => Convert.ToInt32(x));
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06002382 RID: 9090 RVA: 0x000DB61C File Offset: 0x000D981C
	public int Id
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06002383 RID: 9091 RVA: 0x000DB624 File Offset: 0x000D9824
	public int Tier
	{
		get
		{
			return this.tier;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06002384 RID: 9092 RVA: 0x000DB62C File Offset: 0x000D982C
	public bool IsBoardwalk
	{
		get
		{
			return this.isBoardwalk;
		}
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06002385 RID: 9093 RVA: 0x000DB634 File Offset: 0x000D9834
	public Vector3 Position
	{
		get
		{
			return new Vector3((float)(20 * this.position.col), (float)(20 * this.position.row));
		}
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000DB65C File Offset: 0x000D985C
	public static void MakeRealtySignPrototype(DisplayControllerManager dcm)
	{
		Vector2 center = new Vector2(0f, -10f);
		TerrainSlot.defaultSign = new BasicSprite(null, "RealtySign.png", center, 11f, 20f, new QuadHitObject(center, 22f, 40f));
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000DB6A8 File Offset: 0x000D98A8
	public static List<TerrainSlotObject> LoadExpansionObjectData(List<object> data)
	{
		List<TerrainSlotObject> list = new List<TerrainSlotObject>();
		foreach (object obj in data)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			TerrainSlotObject item = default(TerrainSlotObject);
			item.did = TFUtils.LoadInt(dictionary, "did");
			if (dictionary.ContainsKey("label"))
			{
				item.id = new Identity((string)dictionary["label"]);
			}
			int row = TFUtils.LoadInt(dictionary, "y");
			int col = TFUtils.LoadInt(dictionary, "x");
			item.position = new GridPosition(row, col);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06002388 RID: 9096 RVA: 0x000DB788 File Offset: 0x000D9988
	public static List<object> SerializeExpansionObjectData(List<TerrainSlotObject> data)
	{
		List<object> list = new List<object>();
		foreach (TerrainSlotObject terrainSlotObject in data)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = terrainSlotObject.did;
			dictionary["label"] = terrainSlotObject.id.Describe();
			dictionary["x"] = terrainSlotObject.position.X;
			dictionary["y"] = terrainSlotObject.position.Y;
			list.Add(dictionary);
		}
		return list;
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x000DB85C File Offset: 0x000D9A5C
	public bool Available(HashSet<int> purchasedSlots, Game game)
	{
		if (purchasedSlots.Contains(this.Id))
		{
			return false;
		}
		foreach (int item in this.requiredSlots)
		{
			if (!purchasedSlots.Contains(item))
			{
				return false;
			}
		}
		return !this.isBoardwalk || game.featureManager.CheckFeature("purchase_expansions_boardwalk");
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x000DB908 File Offset: 0x000D9B08
	public void Display(DisplayControllerManager manager, BillboardDelegate billboard)
	{
		if (this.sign == null)
		{
			this.sign = TerrainSlot.defaultSign.Clone(manager);
			this.sign.Billboard(billboard);
			this.sign.Visible = false;
		}
	}

	// Token: 0x0600238B RID: 9099 RVA: 0x000DB94C File Offset: 0x000D9B4C
	public bool CheckTap(Ray ray)
	{
		return this.sign != null && this.sign.Visible && this.sign.Intersects(ray);
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x000DB984 File Offset: 0x000D9B84
	public void OnUpdate(Camera camera)
	{
		if (this.sign != null)
		{
			this.sign.Visible = true;
			this.sign.Position = this.Position;
			this.sign.OnUpdate(camera, null);
		}
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x000DB9C8 File Offset: 0x000D9BC8
	public void ClearSign()
	{
		if (this.sign != null)
		{
			this.sign.Destroy();
			this.sign = null;
		}
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000DB9E8 File Offset: 0x000D9BE8
	public void DrawOutline()
	{
		if (this.outlinePoints.Count <= 1 || null != this.outline)
		{
			return;
		}
		int vertexCount = this.outlinePoints.Count * 2 + 1;
		this.outline = new GameObject("TerrainSlotOutline");
		this.outline.AddComponent(typeof(LineRenderer));
		LineRenderer lineRenderer = (LineRenderer)this.outline.renderer;
		lineRenderer.SetWidth(1f, 1f);
		lineRenderer.SetVertexCount(vertexCount);
		for (int i = 0; i < this.outlinePoints.Count; i++)
		{
			lineRenderer.SetPosition(i, this.outlinePoints[i]);
		}
		lineRenderer.SetPosition(this.outlinePoints.Count, this.outlinePoints[0]);
		for (int j = 1; j <= this.outlinePoints.Count; j++)
		{
			lineRenderer.SetPosition(this.outlinePoints.Count + j, this.outlinePoints[this.outlinePoints.Count - j]);
		}
		lineRenderer.material = (Material)Resources.Load("Materials/unique/outline");
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000DBB20 File Offset: 0x000D9D20
	public void ClearOutline()
	{
		if (null != this.outline)
		{
			UnityEngine.Object.Destroy(this.outline);
			this.outline = null;
		}
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000DBB48 File Offset: 0x000D9D48
	public void AddClickListener(Action handler)
	{
		this.clickListeners.Add(handler);
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000DBB58 File Offset: 0x000D9D58
	public bool RemoveClickListener(Action handler)
	{
		return this.clickListeners.Remove(handler);
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x000DBB68 File Offset: 0x000D9D68
	public void HandleSelection()
	{
		foreach (Action action in this.clickListeners.ToArray())
		{
			action();
		}
	}

	// Token: 0x040015C5 RID: 5573
	private static BasicSprite defaultSign;

	// Token: 0x040015C6 RID: 5574
	public Cost cost;

	// Token: 0x040015C7 RID: 5575
	public List<TerrainSlotObject> debris;

	// Token: 0x040015C8 RID: 5576
	public List<TerrainSlotObject> landmarks;

	// Token: 0x040015C9 RID: 5577
	public List<GridPosition> sectors;

	// Token: 0x040015CA RID: 5578
	public List<Vector3> outlinePoints;

	// Token: 0x040015CB RID: 5579
	public List<int> requiredSlots;

	// Token: 0x040015CC RID: 5580
	public volatile bool inUse;

	// Token: 0x040015CD RID: 5581
	private int did;

	// Token: 0x040015CE RID: 5582
	private int tier;

	// Token: 0x040015CF RID: 5583
	private bool isBoardwalk;

	// Token: 0x040015D0 RID: 5584
	private GridPosition position;

	// Token: 0x040015D1 RID: 5585
	private IDisplayController sign;

	// Token: 0x040015D2 RID: 5586
	private GameObject outline;

	// Token: 0x040015D3 RID: 5587
	private List<Action> clickListeners = new List<Action>();
}
