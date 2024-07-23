using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class SBGUIInventoryScreen : SBGUITabbedScrollableDialog
{
	// Token: 0x060004FC RID: 1276 RVA: 0x0001F7F8 File Offset: 0x0001D9F8
	protected override Vector2 GetSlotSize()
	{
		YGSprite component = this.slotPrefab.GetComponent<YGSprite>();
		return component.size * 0.01f;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0001F824 File Offset: 0x0001DA24
	public float GetMainWindowZ()
	{
		return base.FindChild("window").transform.localPosition.z;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x0001F850 File Offset: 0x0001DA50
	protected override void LoadCategories(Session session)
	{
		this.categories = new Dictionary<string, SBTabCategory>();
		Catalog catalog = session.TheGame.catalog;
		foreach (object obj in ((List<object>)catalog.CatalogDict["inventory"]))
		{
			SBInventoryCategory sbinventoryCategory = new SBInventoryCategory((Dictionary<string, object>)obj);
			this.categories[sbinventoryCategory.Name] = sbinventoryCategory;
			this.AddCategory(sbinventoryCategory, session, sbinventoryCategory.Name, sbinventoryCategory.Type, sbinventoryCategory.Texture);
		}
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x0001F910 File Offset: 0x0001DB10
	private void AddCategory(SBInventoryCategory category, Session session, string name, string type, string texture)
	{
		category.Name = name;
		category.Type = type;
		category.Texture = texture;
		if (type != null)
		{
			if (SBGUIInventoryScreen.<>f__switch$map2 == null)
			{
				SBGUIInventoryScreen.<>f__switch$map2 = new Dictionary<string, int>(2)
				{
					{
						"building",
						0
					},
					{
						"movie",
						1
					}
				};
			}
			int num;
			if (SBGUIInventoryScreen.<>f__switch$map2.TryGetValue(type, out num))
			{
				if (num == 0)
				{
					category.NumItems = session.TheGame.inventory.GetNumUniqueItems();
					return;
				}
				if (num == 1)
				{
					category.NumItems = session.TheGame.movieManager.UnlockedMovies.Count;
					return;
				}
			}
		}
		TFUtils.Assert(false, "Unknown category type: " + type);
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0001F9E4 File Offset: 0x0001DBE4
	protected override IEnumerator BuildTabCoroutine(string tabName)
	{
		if (!this.categories.ContainsKey(tabName))
		{
			TFUtils.WarningLog(string.Format("Category {0} not found: ", tabName));
		}
		else
		{
			base.ClearCachedSlotInfos();
			this.region.ClearSlotActions();
			if (!this.tabContents.TryGetValue(tabName, out this.currentTab))
			{
				SBGUIElement anchor = SBGUIElement.Create(this.region.Marker);
				anchor.name = string.Format(tabName, new object[0]);
				anchor.transform.localPosition = Vector3.zero;
				this.tabContents[tabName] = anchor;
				this.currentTab = anchor;
			}
			yield return null;
			SBTabCategory category = this.categories[tabName];
			this.LoadSlotInfo(category, this.currentTab);
		}
		yield break;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0001FA10 File Offset: 0x0001DC10
	private void LoadSlotInfo(SBTabCategory tabCategory, SBGUIElement anchor)
	{
		SBInventoryCategory sbinventoryCategory = (SBInventoryCategory)tabCategory;
		int num = 0;
		if (sbinventoryCategory.Type == "building")
		{
			List<SBInventoryItem> items = this.session.TheGame.inventory.GetItems();
			items.Sort();
			foreach (SBInventoryItem invItem in items)
			{
				this.region.SetupSlotActions.Insert(num, this.SetupSlotClosure(this.session, anchor, invItem, this.BuildingSlotClickedEvent, this.GetSlotOffset(num)));
				string text = SBGUIInventorySlot.CalculateSlotName(invItem);
				if (this.sessionActionIdSearchRequests.Contains(text))
				{
					this.sessionActionSlotMap[text] = num;
				}
				num++;
			}
		}
		else if (sbinventoryCategory.Type == "movie")
		{
			foreach (int id in this.session.TheGame.movieManager.UnlockedMovies)
			{
				MovieInfo movieInfoById = this.session.TheGame.movieManager.GetMovieInfoById(id);
				SBInventoryItem invItem2 = new SBInventoryItem(null, null, "movie", movieInfoById.Name, movieInfoById.Description, movieInfoById.MovieInfoTexture, false, movieInfoById.MovieFile);
				this.region.SetupSlotActions.Insert(num, this.SetupSlotClosure(this.session, anchor, invItem2, this.MovieSlotClickedEvent, this.GetSlotOffset(num)));
				string text2 = SBGUIInventorySlot.CalculateSlotName(invItem2);
				if (this.sessionActionIdSearchRequests.Contains(text2))
				{
					this.sessionActionSlotMap[text2] = num;
				}
				num++;
			}
		}
		base.PostLoadRegionContentInfo(this.region.SetupSlotActions.Count);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001FC38 File Offset: 0x0001DE38
	protected override SBGUIScrollListElement MakeSlot()
	{
		return SBGUIInventorySlot.MakeInventorySlot();
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0001FC40 File Offset: 0x0001DE40
	protected override Rect CalculateTabContentsSize(string tabName)
	{
		SBInventoryCategory sbinventoryCategory = (SBInventoryCategory)this.categories[tabName];
		return base.CalculateScrollRegionSize(sbinventoryCategory.NumItems);
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0001FC6C File Offset: 0x0001DE6C
	private Action<SBGUIScrollListElement> SetupSlotClosure(Session session, SBGUIElement anchor, SBInventoryItem invItem, EventDispatcher<SBInventoryItem> itemClickedEvent, Vector3 offset)
	{
		return delegate(SBGUIScrollListElement slot)
		{
			((SBGUIInventorySlot)slot).Setup(session, anchor, invItem, itemClickedEvent, offset);
		};
	}

	// Token: 0x040003C6 RID: 966
	public GameObject slotPrefab;

	// Token: 0x040003C7 RID: 967
	public EventDispatcher<SBInventoryItem> BuildingSlotClickedEvent = new EventDispatcher<SBInventoryItem>();

	// Token: 0x040003C8 RID: 968
	public EventDispatcher<SBInventoryItem> MovieSlotClickedEvent = new EventDispatcher<SBInventoryItem>();
}
