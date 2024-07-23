using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class SBGUICharacterIdleScreen : SBGUIScrollableDialog
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x00016488 File Offset: 0x00014688
	// (set) Token: 0x060003FB RID: 1019 RVA: 0x00016490 File Offset: 0x00014690
	public int? m_nCostumeDID { get; private set; }

	// Token: 0x060003FC RID: 1020 RVA: 0x0001649C File Offset: 0x0001469C
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x000164A8 File Offset: 0x000146A8
	public void CreateScrollRegionUI(List<TaskData> pTaskDatas)
	{
		this.scrollSize = Vector2.zero;
		if (this.region.Marker.transform.GetChildCount() > 0)
		{
			SBGUIElement[] componentsInChildren = this.region.Marker.GetComponentsInChildren<SBGUIElement>();
			foreach (SBGUIElement sbguielement in componentsInChildren)
			{
				if (!(sbguielement == this.region.Marker))
				{
					UnityEngine.Object.Destroy(sbguielement.gameObject);
				}
			}
		}
		int num = (this.m_nCostumeDID == null) ? -1 : this.m_nCostumeDID.Value;
		if ((num != -1 && this.session.TheGame.costumeManager.IsCostumeUnlocked(num)) || this.costumeCount == 0)
		{
			this.m_pDialogueBubble.SetActive(false);
			this.m_pCostumeUnlockLabel.SetActive(false);
			this.m_pTasksTitleLabel.SetText(Language.Get("!!TASK_UI_STUFF_DO"));
		}
		this.m_pTaskDatas = pTaskDatas;
		object obj = this.session.CheckAsyncRequest("purchasedCostume");
		if (obj != null)
		{
			this.SetArrowList((int)obj);
			return;
		}
		List<int> tasksCompleting = this.session.TheGame.questManager.GetTasksCompleting();
		List<TaskData> list = new List<TaskData>();
		List<TaskData> list2 = new List<TaskData>();
		List<TaskData> list3 = new List<TaskData>();
		int num2 = pTaskDatas.Count;
		for (int j = 0; j < num2; j++)
		{
			TaskData taskData = pTaskDatas[j];
			TaskManager.TaskBlockedStatus taskBlockedStatus = this.session.TheGame.taskManager.GetTaskBlockedStatus(this.session.TheGame, taskData, num);
			if (taskBlockedStatus.m_eTaskBlockedType != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				if (!taskData.m_bHiddenUntilUnlocked && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eSourceCostume) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eMicroEvent) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eRepeatable) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestUnlock) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eActiveQuest) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
				{
					list3.Add(taskData);
				}
			}
			else if (tasksCompleting.Contains(taskData.m_nDID))
			{
				list.Add(taskData);
			}
			else
			{
				list2.Add(taskData);
			}
		}
		list.Sort();
		list2.Sort();
		list3.Sort();
		num2 = list.Count;
		for (int k = num2 - 1; k >= 0; k--)
		{
			list2.Insert(0, list[k]);
		}
		int num3 = 0;
		int count = list2.Count;
		int count2 = list3.Count;
		num2 = count + count2;
		for (int l = 0; l < num2; l++)
		{
			TaskData pTaskData;
			if (l < count)
			{
				pTaskData = list2[l];
			}
			else
			{
				pTaskData = list3[l - count];
			}
			string prefabName = "Prefabs/GUI/Widgets/TaskWidget";
			SBGUITaskWidget sbguitaskWidget = (SBGUITaskWidget)SBGUI.InstantiatePrefab(prefabName);
			sbguitaskWidget.SessionActionId = "task_widget_" + pTaskData.m_sName;
			SBGUIImage sbguiimage = (SBGUIImage)sbguitaskWidget.FindChild("window");
			sbguitaskWidget.SetParent(this.stepsMarker);
			sbguitaskWidget.tform.localPosition = Vector3.zero;
			sbguitaskWidget.tform.localPosition = new Vector3(0f, -((sbguiimage.Size.y + 20f) * 0.01f) * (float)num3, 0f);
			this.scrollSize += (sbguiimage.Size + new Vector2(20f, 20f)) * 0.01f;
			float y = sbguitaskWidget.transform.localPosition.y;
			sbguitaskWidget.SetParent(this.region.Marker);
			Vector3 localPosition = new Vector3(0f, y, 0f);
			sbguitaskWidget.transform.localPosition = localPosition;
			Action pDoTaskAction = delegate()
			{
				this.m_pDoTaskAction(pTaskData.m_nDID);
			};
			sbguitaskWidget.SetData(this.session, pDoTaskAction, pTaskData, num, this.costumeCount);
			num3++;
		}
		Rect rect = new Rect(0f, 0f, this.scrollSize.x, this.scrollSize.y);
		this.region.ResetScroll(rect);
		this.region.ResetToMinScroll();
		if (this.region.scrollBar.IsActive())
		{
			this.region.scrollBar.Reset();
		}
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x00016980 File Offset: 0x00014B80
	public void SetupDialogInfo(Simulated pSimulated, Action pFeedWishAction, Action pRushWishAction, Action<int> pDoTaskAction)
	{
		this.stepsMarker = base.FindChild("steps_marker");
		this.window = (SBGUIAtlasImage)base.FindChild("window");
		this.m_pCharacterWishWidget = (SBGUICharacterWishWidget)base.FindChild("wish_widget");
		this.m_pCharacterWishWidget.SetData(this.session, pSimulated, pFeedWishAction, pRushWishAction);
		this.m_pDoTaskAction = pDoTaskAction;
		this.m_pArrowList = (SBGUIArrowList)base.FindChild("character_portrait_parent");
		this.m_pCharacterNameLabel = (SBGUILabel)base.FindChild("character_name_label");
		this.m_pCostumeUnlockLabel = (SBGUILabel)base.FindChild("costume_unlock_label");
		this.m_pTasksTitleLabel = (SBGUILabel)base.FindChild("tasks_title_label");
		this.m_pDialogueBubble = (SBGUIAtlasImage)base.FindChild("Dialogue_bubble");
		ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
		List<CostumeManager.Costume> costumesForUnit = this.session.TheGame.costumeManager.GetCostumesForUnit(pSimulated.entity.DefinitionId, true, false);
		int nNumCostumes = costumesForUnit.Count;
		this.costumeCount = nNumCostumes;
		this.checkBoxes = new List<SBGUIAtlasImage>();
		this.ticks = new List<SBGUIAtlasImage>();
		this.popupTexts = new List<SBGUILabel>();
		Action<int> pSelectedItemChanged = delegate(int nCostumeDID)
		{
			this.ClearList();
			if (nNumCostumes > 0 && !this.session.TheGame.costumeManager.IsCostumeUnlocked(nCostumeDID))
			{
				this.SetupDialogBubble(nCostumeDID);
			}
			this.m_nCostumeDID = new int?(nCostumeDID);
			this.CreateScrollRegionUI(this.m_pTaskDatas);
		};
		Action<int> pItemClick = delegate(int nCostumeDID)
		{
			this.m_nCostumeDID = new int?(nCostumeDID);
			this.m_pArrowList.SetSelectedID(nCostumeDID);
		};
		if (nNumCostumes <= 0)
		{
			this.m_pArrowList.SetData(this.session, new List<SBGUIArrowList.ListItemData>
			{
				new SBGUIArrowList.ListItemData(0, entity.DialogPortrait, false)
			}, 0, null, pSelectedItemChanged, pItemClick);
		}
		else
		{
			List<SBGUIArrowList.ListItemData> list = new List<SBGUIArrowList.ListItemData>(nNumCostumes);
			for (int i = 0; i < nNumCostumes; i++)
			{
				list.Add(new SBGUIArrowList.ListItemData(costumesForUnit[i].m_nDID, costumesForUnit[i].m_sPortrait, !this.session.TheGame.costumeManager.IsCostumeUnlocked(costumesForUnit[i].m_nDID)));
			}
			this.m_pArrowList.SetData(this.session, list, (entity.CostumeDID == null) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value, null, pSelectedItemChanged, pItemClick);
		}
		this.m_nCostumeDID = entity.CostumeDID;
		this.m_pCharacterNameLabel.SetText(Language.Get(entity.Name));
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x00016C10 File Offset: 0x00014E10
	private void SetupDialogBubble(int costumeDID)
	{
		CostumeManager.Costume costume = this.session.TheGame.costumeManager.GetCostume(costumeDID);
		this.m_pDialogueBubble.SetActive(!this.session.TheGame.costumeManager.IsCostumeUnlocked(costumeDID));
		this.m_pCostumeUnlockLabel.SetActive(true);
		this.m_pTasksTitleLabel.SetText(Language.Get("!!PREFAB_LOCKED"));
		this.CreateCheckBox(costume.m_nCriteriaCount);
		float num = this.GenerateTextandTick(costume);
		this.m_pDialogueBubble.Size = new Vector2(this.m_pDialogueBubble.Size.x, num + 80f);
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00016CB8 File Offset: 0x00014EB8
	private void PositionObject(SBGUIElement obj, Vector3 loc)
	{
		obj.SetParent(this.m_pCostumeUnlockLabel);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localPosition = loc;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x00016CF0 File Offset: 0x00014EF0
	public void ClearList()
	{
		for (int i = 0; i < this.popupTexts.Count; i++)
		{
			UnityEngine.Object.Destroy(this.popupTexts[i].gameObject);
		}
		this.popupTexts.Clear();
		for (int j = 0; j < this.ticks.Count; j++)
		{
			UnityEngine.Object.Destroy(this.ticks[j].gameObject);
		}
		this.ticks.Clear();
		for (int k = 0; k < this.checkBoxes.Count; k++)
		{
			UnityEngine.Object.Destroy(this.checkBoxes[k].gameObject);
		}
		this.checkBoxes.Clear();
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00016DB4 File Offset: 0x00014FB4
	private float GenerateTextandTick(CostumeManager.Costume costume)
	{
		string prefabName = "Prefabs/GUI/Screens/PopUpText";
		string prefabName2 = "Prefabs/GUI/Screens/CheckBoxTick";
		int num = 0;
		float num2 = 0f;
		if (costume.m_nUnlockLevel > 0)
		{
			SBGUILabel sbguilabel = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			sbguilabel.SetText("Reach Level " + costume.m_nUnlockLevel);
			this.PositionObject(sbguilabel, new Vector3(0.2f, this.checkBoxLocPos.y - (float)num * 0.7f, 0f));
			this.popupTexts.Add(sbguilabel);
			num2 += (float)sbguilabel.Height + 40f;
			if (this.session.TheGame.resourceManager.PlayerLevelAmount >= costume.m_nUnlockLevel)
			{
				SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				this.PositionObject(sbguiatlasImage, new Vector3(this.checkBoxLocPos.x, this.checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				this.ticks.Add(sbguiatlasImage);
			}
			num++;
		}
		if (costume.m_nUnlockAssetDid > 0)
		{
			SBGUILabel sbguilabel2 = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, costume.m_nUnlockAssetDid, true);
			sbguilabel2.SetText("Buy and Place " + Language.Get((string)blueprint.Invariable["name"]));
			this.PositionObject(sbguilabel2, new Vector3(0.2f, this.checkBoxLocPos.y - (float)num * 0.7f, 0f));
			this.popupTexts.Add(sbguilabel2);
			num2 += (float)sbguilabel2.Height + 40f;
			if (this.session.TheGame.inventory.HasItem(new int?(costume.m_nUnlockAssetDid)) || this.session.TheGame.simulation.FindSimulated(new int?(costume.m_nUnlockAssetDid)) != null)
			{
				SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				this.PositionObject(sbguiatlasImage2, new Vector3(this.checkBoxLocPos.x, this.checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				this.ticks.Add(sbguiatlasImage2);
			}
			num++;
		}
		if (costume.m_nUnlockQuest1 > 0)
		{
			SBGUILabel sbguilabel3 = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			sbguilabel3.SetText(Language.Get(costume.m_sUnlockQuest1Descript));
			this.PositionObject(sbguilabel3, new Vector3(0.2f, this.checkBoxLocPos.y - (float)num * 0.7f, 0f));
			this.popupTexts.Add(sbguilabel3);
			num2 += (float)sbguilabel3.Height + 40f;
			if (this.session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest1))
			{
				SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				this.PositionObject(sbguiatlasImage3, new Vector3(this.checkBoxLocPos.x, this.checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				this.ticks.Add(sbguiatlasImage3);
			}
			num++;
		}
		if (costume.m_nUnlockQuest2 > 0)
		{
			SBGUILabel sbguilabel4 = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			sbguilabel4.SetText(Language.Get(costume.m_sUnlockQuest2Descript));
			this.PositionObject(sbguilabel4, new Vector3(0.2f, this.checkBoxLocPos.y - (float)num * 0.7f, 0f));
			this.popupTexts.Add(sbguilabel4);
			num2 += (float)sbguilabel4.Height + 40f;
			if (this.session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest2))
			{
				SBGUIAtlasImage sbguiatlasImage4 = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				this.PositionObject(sbguiatlasImage4, new Vector3(this.checkBoxLocPos.x, this.checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				this.ticks.Add(sbguiatlasImage4);
			}
			num++;
		}
		return num2;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x000171C4 File Offset: 0x000153C4
	private void CreateCheckBox(int count)
	{
		for (int i = 0; i < count; i++)
		{
			string prefabName = "Prefabs/GUI/Screens/CheckBox";
			SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName);
			this.PositionObject(sbguiatlasImage, new Vector3(this.checkBoxLocPos.x, this.checkBoxLocPos.y - (float)i * 0.7f, 0f));
			this.checkBoxes.Add(sbguiatlasImage);
		}
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00017234 File Offset: 0x00015434
	public Vector2 GetWishWidgetRushButtonPosition()
	{
		return this.m_pCharacterWishWidget.GetRushWishButtonPosition();
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x00017244 File Offset: 0x00015444
	public void SetArrowList(int id)
	{
		this.m_pArrowList.SetSelectedID(id);
	}

	// Token: 0x040002BA RID: 698
	public const int STEP_GAP = 20;

	// Token: 0x040002BB RID: 699
	private const float CHECKBOX_GAP = 0.7f;

	// Token: 0x040002BC RID: 700
	private const float TEXT_GAP = 40f;

	// Token: 0x040002BD RID: 701
	private Vector3 checkBoxLocPos = new Vector3(-0.05f, 0.35f, 0f);

	// Token: 0x040002BE RID: 702
	private int costumeCount;

	// Token: 0x040002BF RID: 703
	private Vector2 scrollSize;

	// Token: 0x040002C0 RID: 704
	private SBGUIAtlasImage window;

	// Token: 0x040002C1 RID: 705
	private SBGUIElement stepsMarker;

	// Token: 0x040002C2 RID: 706
	private SBGUILabel m_pCharacterNameLabel;

	// Token: 0x040002C3 RID: 707
	private SBGUILabel m_pCostumeUnlockLabel;

	// Token: 0x040002C4 RID: 708
	private SBGUILabel m_pTasksTitleLabel;

	// Token: 0x040002C5 RID: 709
	private SBGUICharacterWishWidget m_pCharacterWishWidget;

	// Token: 0x040002C6 RID: 710
	private Action<int> m_pDoTaskAction;

	// Token: 0x040002C7 RID: 711
	private SBGUIArrowList m_pArrowList;

	// Token: 0x040002C8 RID: 712
	private List<TaskData> m_pTaskDatas;

	// Token: 0x040002C9 RID: 713
	private SBGUIAtlasImage m_pDialogueBubble;

	// Token: 0x040002CA RID: 714
	private List<SBGUIAtlasImage> checkBoxes;

	// Token: 0x040002CB RID: 715
	private List<SBGUIAtlasImage> ticks;

	// Token: 0x040002CC RID: 716
	private List<SBGUILabel> popupTexts;
}
