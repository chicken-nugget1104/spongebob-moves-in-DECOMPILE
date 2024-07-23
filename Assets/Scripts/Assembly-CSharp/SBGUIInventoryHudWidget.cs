using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x02000081 RID: 129
public class SBGUIInventoryHudWidget : SBGUIElement
{
	// Token: 0x060004EC RID: 1260 RVA: 0x0001EB68 File Offset: 0x0001CD68
	public void Setup(Game game, CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Action<YGEvent, int?> interactCallback, float bottomHideThreshold)
	{
		this.bottomHideThreshold = bottomHideThreshold;
		foreach (SBGUIInventoryWidgetTab sbguiinventoryWidgetTab in this.Tabs)
		{
			sbguiinventoryWidgetTab.Setup(game, craftMgr, vendingMgr, resourceMgr, sfxMgr, new Action<YGEvent>(this.HandleUiEvent), new Action<int, YGEvent>(this.OnDraggedProduct), new SBGUIInventoryWidgetTab.GetNewRow(this.GetNextRow), new SBGUIInventoryWidgetTab.CloseRows(this.CloseRows), new SBGUIInventoryWidgetTab.OpenAllTabs(this.ActivateAllTabs));
		}
		TFUtils.Assert(this.RowAnchor != null, "Must specify a Row Anchor for this element. this=" + this.ToString());
		TFUtils.Assert(this.RowHideMarker != null, "Must specify a Row Hide Marker for this element. this=" + this.ToString());
		this.initialAnchorPosition = this.RowAnchor.transform.localPosition;
		this.interactCallback = interactCallback;
		craftMgr.UnlockedEvent.AddListener(delegate
		{
			this.UpdateRecipes(craftMgr, vendingMgr, resourceMgr);
		});
		this.firstUpdateInit = true;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0001ECCC File Offset: 0x0001CECC
	public void UpdateRecipes(CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr)
	{
		this.CloseRows();
		foreach (SBGUIInventoryWidgetTab sbguiinventoryWidgetTab in this.Tabs)
		{
			sbguiinventoryWidgetTab.UpdateRecipes(craftMgr, vendingMgr, resourceMgr);
		}
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0001ED3C File Offset: 0x0001CF3C
	public SBGUIInventoryWidgetRow GetNextRow(SBGUIInventoryWidgetRow rowType, int fromCookbookId, bool fromIsVendor)
	{
		Vector3 b = Vector3.zero;
		if (this.currentCount == 0)
		{
			this.lastOpenedCookbook = fromCookbookId;
			this.lastOpenedIsVendor = fromIsVendor;
		}
		else if (this.currentCount <= this.currentRows.Count)
		{
			SBGUIInventoryWidgetRow sbguiinventoryWidgetRow = this.currentRows[this.currentCount - 1];
			b = sbguiinventoryWidgetRow.WorldPosition + new Vector3(0f, -this.RowOffset, 0f) - this.RowAnchor.WorldPosition;
		}
		SBGUIInventoryWidgetRow sbguiinventoryWidgetRow2;
		if (this.currentCount >= this.currentRows.Count)
		{
			sbguiinventoryWidgetRow2 = (SBGUIInventoryWidgetRow)UnityEngine.Object.Instantiate(rowType);
			sbguiinventoryWidgetRow2.SetParent(this.RowAnchor);
			sbguiinventoryWidgetRow2.WorldPosition = this.RowAnchor.WorldPosition + b;
			this.currentRows.Add(sbguiinventoryWidgetRow2);
			this.initialAnchorPosition = this.RowAnchor.transform.localPosition;
		}
		else
		{
			sbguiinventoryWidgetRow2 = this.currentRows[this.currentCount];
			if (sbguiinventoryWidgetRow2.GetType().Equals(rowType.GetType()))
			{
				sbguiinventoryWidgetRow2.SetActive(true);
			}
			else
			{
				this.currentRows[this.currentCount] = (SBGUIInventoryWidgetRow)UnityEngine.Object.Instantiate(rowType);
				this.currentRows[this.currentCount].SetParent(this.RowAnchor);
				this.currentRows[this.currentCount].WorldPosition = sbguiinventoryWidgetRow2.WorldPosition;
				UnityEngine.Object.Destroy(sbguiinventoryWidgetRow2);
				sbguiinventoryWidgetRow2 = this.currentRows[this.currentCount];
			}
		}
		this.interactCallback(null, null);
		this.currentCount++;
		return sbguiinventoryWidgetRow2;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0001EEFC File Offset: 0x0001D0FC
	public bool ActivateTab(int cookbookId, bool isVendor)
	{
		bool flag = this.currentCount > 0;
		bool flag2 = false;
		if (flag && cookbookId != this.lastOpenedCookbook && isVendor != this.lastOpenedIsVendor)
		{
			this.CloseRows();
		}
		if (!flag || (cookbookId != this.lastOpenedCookbook && isVendor != this.lastOpenedIsVendor))
		{
			foreach (SBGUIInventoryWidgetTab sbguiinventoryWidgetTab in this.Tabs)
			{
				flag2 |= sbguiinventoryWidgetTab.TryActivateTab(cookbookId, isVendor, true);
			}
		}
		this.lastOpenedCookbook = cookbookId;
		this.lastOpenedIsVendor = isVendor;
		return !flag && flag2;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0001EFD0 File Offset: 0x0001D1D0
	public bool ActivateAllTabs()
	{
		bool flag = false;
		if (base.IsActive())
		{
			if (this.currentCount <= 0)
			{
				foreach (SBGUIInventoryWidgetTab sbguiinventoryWidgetTab in this.Tabs)
				{
					flag |= sbguiinventoryWidgetTab.ActivateTab(false);
				}
			}
			this.lastOpenedCookbook = 1;
			this.lastOpenedIsVendor = false;
			this.footerAnchor.SetActive(true);
			this.backingSprite.gameObject.SetActiveRecursively(true);
		}
		return flag;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0001F084 File Offset: 0x0001D284
	public void Tidy()
	{
		this.dragMode = SBGUIInventoryHudWidget.DragMode.None;
		this.primedEvtPosition = Vector2.zero;
		this.primedRowAnchorPositionScreen = Vector2.zero;
		this.bottomLockRowAnchorPositionScreen = null;
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
	public void TryPulseResourceError(int resourceId)
	{
		SBGUIInventoryWidgetRow sbguiinventoryWidgetRow = this.currentRows.Find((SBGUIInventoryWidgetRow i) => i.Product == resourceId);
		if (sbguiinventoryWidgetRow != null)
		{
			sbguiinventoryWidgetRow.PulseError();
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x0001F104 File Offset: 0x0001D304
	public void IncrementDeductionsForTick(int resourceId)
	{
		if (this.currentCount > 0)
		{
			SBGUIInventoryWidgetRow sbguiinventoryWidgetRow = this.currentRows.Find((SBGUIInventoryWidgetRow i) => i.Product == resourceId);
			if (sbguiinventoryWidgetRow == null)
			{
				Debug.LogError("Could not find row with productID(" + resourceId + ")");
			}
			else
			{
				sbguiinventoryWidgetRow.IncrementDeductionsForTick();
			}
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0001F178 File Offset: 0x0001D378
	private void OnDraggedProduct(int productId, YGEvent triggeringEvent)
	{
		TFUtils.Assert(this.StartDragOutCallback != null, "Must assign a dragout callback before reacting to dragout!");
		this.dragMode = SBGUIInventoryHudWidget.DragMode.DraggingOut;
		this.StartDragOutCallback(productId, triggeringEvent);
		this.interactCallback(null, new int?(productId));
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x0001F1C4 File Offset: 0x0001D3C4
	public void CloseRows()
	{
		if (this.currentRows.Count == 0)
		{
			return;
		}
		foreach (SBGUIInventoryWidgetTab sbguiinventoryWidgetTab in this.Tabs)
		{
			sbguiinventoryWidgetTab.Close();
		}
		foreach (SBGUIInventoryWidgetRow sbguiinventoryWidgetRow in this.currentRows)
		{
			sbguiinventoryWidgetRow.SetActive(false);
		}
		this.RowAnchor.transform.localPosition = this.initialAnchorPosition;
		this.currentCount = 0;
		this.interactCallback(null, null);
		this.footerAnchor.SetActive(false);
		this.backingSprite.gameObject.SetActiveRecursively(false);
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x0001F2E0 File Offset: 0x0001D4E0
	public void OnUpdate(ResourceManager resourceMgr)
	{
		if (this.firstUpdateInit)
		{
			Vector3 position = this.footerAnchor.tform.position;
			Vector2 screenPosition = this.footerAnchor.GetScreenPosition();
			Vector3 v = new Vector3(screenPosition.x, this.bottomHideThreshold, this.footerAnchor.tform.position.z);
			this.footerAnchor.SetScreenPosition(v);
			this.firstUpdateInit = false;
			this.backingSprite.transform.position = (position + this.footerAnchor.tform.position) * 0.5f;
			this.backingSprite.transform.Translate(new Vector3(0f, 0f, 0.5f), Space.World);
			this.backingSprite.size.y = Math.Abs(screenPosition.y - this.footerAnchor.GetScreenPosition().y);
			YGFrameAtlasSprite ygframeAtlasSprite = this.backingSprite;
			ygframeAtlasSprite.size.y = ygframeAtlasSprite.size.y * (2f / GUIView.ResolutionScaleFactor());
			this.backingSprite.SetSize(this.backingSprite.size);
		}
		if (this.currentCount > 0 && base.IsActive())
		{
			float y = base.GetScreenPosition().y;
			int count = this.currentRows.Count;
			for (int i = 0; i < count; i++)
			{
				this.currentRows[i].OnUpdate(resourceMgr, y, this.bottomHideThreshold);
			}
		}
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x0001F480 File Offset: 0x0001D680
	private void HandleUiEvent(YGEvent evt)
	{
		if (evt.type == YGEvent.TYPE.TOUCH_BEGIN)
		{
			this.primedRowAnchorPositionScreen = this.RowAnchor.GetScreenPosition();
			this.primedEvtPosition = evt.position;
			if (this.dragMode != SBGUIInventoryHudWidget.DragMode.DraggingOut)
			{
				this.dragMode = SBGUIInventoryHudWidget.DragMode.PrimedForScrolling;
			}
		}
		else if (evt.type == YGEvent.TYPE.TOUCH_END || evt.type == YGEvent.TYPE.TOUCH_CANCEL)
		{
			if (this.DragThroughCallback != null)
			{
				this.DragThroughCallback(evt);
			}
			this.Tidy();
		}
		Vector2 b = this.primedEvtPosition - evt.position;
		b.x = 0f;
		if (this.dragMode == SBGUIInventoryHudWidget.DragMode.PrimedForScrolling && Math.Abs(b.y) >= 2f)
		{
			this.dragMode = SBGUIInventoryHudWidget.DragMode.Scrolling;
		}
		if (this.dragMode == SBGUIInventoryHudWidget.DragMode.Scrolling)
		{
			this.RowAnchor.SetScreenPosition(this.primedRowAnchorPositionScreen + b);
			this.EnforceScrollLimits();
		}
		if (this.dragMode == SBGUIInventoryHudWidget.DragMode.DraggingOut && this.DragThroughCallback != null)
		{
			this.DragThroughCallback(evt);
		}
		this.interactCallback(evt, null);
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0001F5AC File Offset: 0x0001D7AC
	private void DetectScrolLimits()
	{
		if (this.currentRows.Count == 0)
		{
			this.didScrollTooHigh = false;
			this.didScrollTooLow = false;
			return;
		}
		if (this.RowAnchor.transform.localPosition.y <= this.initialAnchorPosition.y)
		{
			this.didScrollTooHigh = true;
			this.didScrollTooLow = false;
		}
		else
		{
			this.didScrollTooHigh = false;
			this.didScrollTooLow = this.IsTooLow();
		}
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0001F628 File Offset: 0x0001D828
	private bool IsTooLow()
	{
		float y = this.currentRows[this.currentRows.Count - 1].GetScreenPosition().y;
		return y < (float)(Screen.height / 2);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x0001F668 File Offset: 0x0001D868
	private void EnforceScrollLimits()
	{
		this.DetectScrolLimits();
		if (this.didScrollTooHigh)
		{
			this.RowAnchor.transform.localPosition = this.initialAnchorPosition;
		}
		else if (this.didScrollTooLow)
		{
			if (this.bottomLockRowAnchorPositionScreen != null)
			{
				this.RowAnchor.SetPosition(this.bottomLockRowAnchorPositionScreen.Value);
			}
			else
			{
				while (this.IsTooLow())
				{
					Vector3 b = new Vector3(0f, (float)(-(float)Screen.height) * 0.001f, 0f);
					this.RowAnchor.transform.position += b;
					if (this.RowAnchor.transform.localPosition.y <= this.initialAnchorPosition.y)
					{
						this.RowAnchor.transform.localPosition = this.initialAnchorPosition;
						break;
					}
				}
				this.bottomLockRowAnchorPositionScreen = new Vector2?(this.RowAnchor.GetScreenPosition());
			}
			this.RowAnchor.transform.localPosition = new Vector3(this.RowAnchor.transform.localPosition.x, this.RowAnchor.transform.localPosition.y, this.initialAnchorPosition.z);
		}
		base.UpdateColliderTransforms();
	}

	// Token: 0x040003A5 RID: 933
	public const string SHOW = "ShowInventoryHudWidget";

	// Token: 0x040003A6 RID: 934
	public const string RESET_SIMULATION_DRAG = "ResetSimulationDrag";

	// Token: 0x040003A7 RID: 935
	public const string GOOD_DELIVERY_REQUEST = "GoodDeliveryRequest";

	// Token: 0x040003A8 RID: 936
	public const string GOOD_RETURN_REQUEST = "GoodReturnRequest";

	// Token: 0x040003A9 RID: 937
	public const string PULSE_RESOURCE_ERROR = "PulseResourceError";

	// Token: 0x040003AA RID: 938
	private const int ALL_COOKBOOKS = -1;

	// Token: 0x040003AB RID: 939
	public List<SBGUIInventoryWidgetTab> Tabs;

	// Token: 0x040003AC RID: 940
	public SBGUIElement RowAnchor;

	// Token: 0x040003AD RID: 941
	public GameObject RowHideMarker;

	// Token: 0x040003AE RID: 942
	public float RowOffset;

	// Token: 0x040003AF RID: 943
	public Action<int, YGEvent> StartDragOutCallback;

	// Token: 0x040003B0 RID: 944
	public Action<YGEvent> DragThroughCallback;

	// Token: 0x040003B1 RID: 945
	public SBGUIElement footerAnchor;

	// Token: 0x040003B2 RID: 946
	public YGFrameAtlasSprite backingSprite;

	// Token: 0x040003B3 RID: 947
	private List<SBGUIInventoryWidgetRow> currentRows = new List<SBGUIInventoryWidgetRow>();

	// Token: 0x040003B4 RID: 948
	private int currentCount;

	// Token: 0x040003B5 RID: 949
	private float bottomHideThreshold;

	// Token: 0x040003B6 RID: 950
	private Vector3 initialAnchorPosition;

	// Token: 0x040003B7 RID: 951
	private bool didScrollTooHigh;

	// Token: 0x040003B8 RID: 952
	private bool didScrollTooLow;

	// Token: 0x040003B9 RID: 953
	private Vector2 primedEvtPosition = Vector2.zero;

	// Token: 0x040003BA RID: 954
	private Vector2 primedRowAnchorPositionScreen = Vector2.zero;

	// Token: 0x040003BB RID: 955
	private Vector2? bottomLockRowAnchorPositionScreen;

	// Token: 0x040003BC RID: 956
	private Action<YGEvent, int?> interactCallback;

	// Token: 0x040003BD RID: 957
	private int lastOpenedCookbook = 1;

	// Token: 0x040003BE RID: 958
	private bool lastOpenedIsVendor;

	// Token: 0x040003BF RID: 959
	private SBGUIInventoryHudWidget.DragMode dragMode;

	// Token: 0x040003C0 RID: 960
	private bool firstUpdateInit;

	// Token: 0x02000082 RID: 130
	private enum DragMode
	{
		// Token: 0x040003C2 RID: 962
		None,
		// Token: 0x040003C3 RID: 963
		PrimedForScrolling,
		// Token: 0x040003C4 RID: 964
		Scrolling,
		// Token: 0x040003C5 RID: 965
		DraggingOut
	}
}
