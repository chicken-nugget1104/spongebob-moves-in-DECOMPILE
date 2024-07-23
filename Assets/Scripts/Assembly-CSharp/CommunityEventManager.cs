using System;
using System.Collections.Generic;
using MTools;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class CommunityEventManager
{
	// Token: 0x06000A99 RID: 2713 RVA: 0x00041450 File Offset: 0x0003F650
	public CommunityEventManager(Session pSession)
	{
		this.m_pSession = pSession;
		this.LoadCommunityEventsFromSpreadsheets();
		SoaringCommunityEventManager.GetValueFinished += this.HandleSoaringCallFinished;
		SoaringCommunityEventManager.SetValueFinished += this.HandleSoaringCallFinished;
		SoaringCommunityEventManager.AquireGiftFinished += this.HandleSoaringCallFinished;
		SoaringCommunityEventManager.AquireGiftFinished += this.HandleSoaringAquireGiftFinished;
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x000414F4 File Offset: 0x0003F6F4
	public Session GetSession()
	{
		return this.m_pSession;
	}

	// Token: 0x1700016B RID: 363
	// (set) Token: 0x06000A9C RID: 2716 RVA: 0x000414FC File Offset: 0x0003F6FC
	public Action DialogNeededCallback
	{
		set
		{
			this.dialogNeededCallback = value;
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00041508 File Offset: 0x0003F708
	public void DialogNeeded()
	{
		if (this.dialogNeededCallback != null)
		{
			this.dialogNeededCallback();
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00041524 File Offset: 0x0003F724
	public void QuestComplete(uint nQuestID)
	{
		CommunityEvent activeEvent = this.GetActiveEvent();
		if (activeEvent != null && (activeEvent.m_sID == CommunityEventManager._sSpongyGamesEventID || activeEvent.m_sID == CommunityEventManager._sSpongyGamesLastDayEventID) && (long)activeEvent.m_nQuestPrereqID == (long)((ulong)nQuestID) && (this.m_pSession.TheGame.simulation.FindSimulated(new int?(CommunityEventManager._nColiseumDID)) != null || this.m_pSession.TheGame.inventory.HasItem(new int?(CommunityEventManager._nColiseumDID))))
		{
			Soaring.FireEvent("spongy_games_banner", null);
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x000415CC File Offset: 0x0003F7CC
	private void HandleSoaringAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		if (!bSuccess || this.m_pSession == null)
		{
			return;
		}
		string text = pContext.soaringValue("eventDid").ToString();
		int num = pContext.soaringValue("giftDid");
		CommunityEvent communityEvent = (CommunityEvent)this.m_pCommunityEventDefinitions.objectWithKey(text);
		if (communityEvent == null)
		{
			return;
		}
		SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(text);
		if (@event == null)
		{
			return;
		}
		if (pData.soaringValue("already_acquired"))
		{
			return;
		}
		bool flag = pContext.soaringValue("purchased");
		Cost cost = null;
		Dictionary<int, int> dictionary = null;
		if (flag)
		{
			int amount = this.m_pSession.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount;
			int num2 = @event.GetReward(num).m_nValue - amount;
			if (num2 > 0)
			{
				dictionary = new Dictionary<int, int>();
				dictionary.Add(communityEvent.m_nValueID, num2);
				dictionary.Add(ResourceManager.HARD_CURRENCY, -pContext.soaringValue("purchaseCost"));
			}
			int value = pContext.soaringValue("purchaseCost");
			cost = new Cost(new Dictionary<int, int>
			{
				{
					ResourceManager.HARD_CURRENCY,
					value
				}
			});
		}
		CommunityEvent.Reward reward = communityEvent.GetReward(num);
		string text2 = string.Empty;
		Reward reward2;
		if (reward.m_sType == "recipe")
		{
			CraftingRecipe recipeById = this.m_pSession.TheGame.craftManager.GetRecipeById(num);
			text2 = recipeById.recipeName;
			reward2 = new Reward(dictionary, null, null, new List<int>
			{
				num
			}, null, null, null, null, false, null);
			FoundItemDialogInputData item = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get(recipeById.recipeName)), reward.m_sTexture, "Beat_FoundRecipe");
			this.m_pSession.TheGame.dialogPackageManager.AddDialogInputBatch(this.m_pSession.TheGame, new List<DialogInputData>
			{
				item
			}, uint.MaxValue);
			this.DialogNeeded();
			if (flag)
			{
				this.m_pSession.analytics.LogPurchaseEventReward(recipeById.recipeName, cost, this.m_pSession.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount);
			}
		}
		else
		{
			List<int> list = null;
			Dictionary<int, Vector2> buildingPositions = null;
			int[] landIDs = reward.LandIDs;
			int num3 = landIDs.Length;
			if (num3 > 0)
			{
				list = new List<int>(num3);
				for (int i = 0; i < num3; i++)
				{
					list.Add(landIDs[i]);
				}
				buildingPositions = new Dictionary<int, Vector2>
				{
					{
						num,
						new Vector2((float)reward.m_nAutoPlaceX, (float)reward.m_nAutoPlaceY)
					}
				};
			}
			reward2 = new Reward(dictionary, new Dictionary<int, int>
			{
				{
					num,
					1
				}
			}, buildingPositions, null, null, null, list, null, false, null);
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, num, true);
			text2 = (string)blueprint.Invariable["name"];
			FoundItemDialogInputData item2 = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get(text2)), reward.m_sTexture, "Beat_FoundRecipe");
			this.m_pSession.TheGame.dialogPackageManager.AddDialogInputBatch(this.m_pSession.TheGame, new List<DialogInputData>
			{
				item2
			}, uint.MaxValue);
			if (reward.m_nDialogSequenceID >= 0)
			{
				this.m_pSession.TheGame.questManager.AddDialogSequences(this.m_pSession.TheGame, 1U, (uint)reward.m_nDialogSequenceID, new List<Reward>(), 0U, false);
			}
			this.DialogNeeded();
			if (flag)
			{
				this.m_pSession.analytics.LogPurchaseEventReward((string)blueprint.Invariable["name"], cost, this.m_pSession.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount);
			}
		}
		this.m_pSession.TheGame.ApplyReward(reward2, TFUtils.EpochTime(), false);
		this.m_pSession.TheGame.ModifyGameState(new ReceiveRewardAction(reward2, num.ToString()));
		AnalyticsWrapper.LogRecievedEventItem(this.m_pSession.TheGame, num, text2);
		if (num == CommunityEventManager._nColiseumDID)
		{
			this.m_fPreviousBannerPingTime = 0f;
			this.CheckEventBannerPing();
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00041A58 File Offset: 0x0003FC58
	private void HandleSoaringCallFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string text = pContext.soaringValue("eventDid").ToString();
		CommunityEvent communityEvent = (CommunityEvent)this.m_pCommunityEventDefinitions.objectWithKey(text);
		if (communityEvent == null)
		{
			return;
		}
		if (Soaring.CommunityEventManager.GetEvent(text) == null)
		{
			communityEvent.SetActive(false);
		}
		else
		{
			communityEvent.SetActive(true);
		}
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00041AB4 File Offset: 0x0003FCB4
	public CommunityEvent GetActiveEvent()
	{
		bool isOnline = Soaring.IsOnline;
		int num = this.m_pCommunityEventDefinitions.count();
		for (int i = 0; i < num; i++)
		{
			CommunityEvent communityEvent = (CommunityEvent)this.m_pCommunityEventDefinitions.objectAtIndex(i);
			if (isOnline)
			{
				if (communityEvent.m_bActive)
				{
					return communityEvent;
				}
			}
			else
			{
				DateTime utcNow = DateTime.UtcNow;
				if (utcNow > communityEvent.m_pStartDate && utcNow < communityEvent.m_pEndDate)
				{
					return communityEvent;
				}
			}
		}
		return null;
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00041B40 File Offset: 0x0003FD40
	public CommunityEvent[] GetEvents()
	{
		int num = this.m_pCommunityEventDefinitions.count();
		CommunityEvent[] array = new CommunityEvent[num];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			array[num2++] = (CommunityEvent)this.m_pCommunityEventDefinitions.objectAtIndex(i);
		}
		return array;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00041B90 File Offset: 0x0003FD90
	public void OnUpdate(Session pSession)
	{
		if (this.m_pSession == null || pSession != this.m_pSession)
		{
			this.m_pSession = pSession;
		}
		this.CheckValueUpdate();
		this.CheckClaimRewardUpdate();
		this.CheckEventBannerPing();
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00041BD0 File Offset: 0x0003FDD0
	private void CheckValueUpdate()
	{
		this.m_fUpdateEventValueTimer += Time.deltaTime;
		if (Time.time - this.m_fPreviousValueUpdateTime <= 10f)
		{
			return;
		}
		if (this.m_fUpdateEventValueTimer >= 300f)
		{
			this.UpdateValuesToSoaring();
			this.m_fUpdateEventValueTimer = 0f;
			return;
		}
		int num = this.m_pCommunityEventDefinitions.count();
		for (int i = 0; i < num; i++)
		{
			CommunityEvent communityEvent = (CommunityEvent)this.m_pCommunityEventDefinitions.objectAtIndex(i);
			if (communityEvent.m_bActive)
			{
				SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(communityEvent.m_sID);
				if (@event != null)
				{
					bool flag = false;
					int amount = this.m_pSession.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount;
					int num2 = @event.CommunityRewards.Length;
					for (int j = 0; j < num2; j++)
					{
						SoaringCommunityEvent.Reward reward = @event.CommunityRewards[j];
						if (!reward.m_bUnlocked)
						{
							if (amount >= reward.m_nValue)
							{
								this.UpdateValueToSoaring(communityEvent);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						num2 = @event.IndividualRewards.Length;
						for (int k = 0; k < num2; k++)
						{
							SoaringCommunityEvent.Reward reward = @event.IndividualRewards[k];
							if (!reward.m_bUnlocked)
							{
								if (amount >= reward.m_nValue)
								{
									this.UpdateValueToSoaring(communityEvent);
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x00041D74 File Offset: 0x0003FF74
	private void CheckClaimRewardUpdate()
	{
		if (Time.time - this.m_fPreviousRewardUpdateTime <= 10f)
		{
			return;
		}
		int num = this.m_pCommunityEventDefinitions.count();
		for (int i = 0; i < num; i++)
		{
			CommunityEvent communityEvent = (CommunityEvent)this.m_pCommunityEventDefinitions.objectAtIndex(i);
			if (communityEvent.m_bActive)
			{
				if (communityEvent.m_nQuestPrereqID < 0 || this.m_pSession.TheGame.questManager.IsQuestCompleted((uint)communityEvent.m_nQuestPrereqID))
				{
					SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(communityEvent.m_sID);
					if (@event != null)
					{
						int num2 = @event.CommunityRewards.Length;
						for (int j = 0; j < num2; j++)
						{
							SoaringCommunityEvent.Reward reward = @event.CommunityRewards[j];
							if (!reward.m_bAcquired && reward.m_bUnlocked)
							{
								this.AquireEventGift(communityEvent, reward);
							}
						}
						num2 = @event.IndividualRewards.Length;
						for (int k = 0; k < num2; k++)
						{
							SoaringCommunityEvent.Reward reward = @event.IndividualRewards[k];
							if (!reward.m_bAcquired && reward.m_bUnlocked)
							{
								this.AquireEventGift(communityEvent, reward);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x00041EC0 File Offset: 0x000400C0
	private void CheckEventBannerPing()
	{
		if (SBSettings.CommunityEventBannerPing <= 0f)
		{
			return;
		}
		if (Time.time - this.m_fPreviousBannerPingTime <= SBSettings.CommunityEventBannerPing)
		{
			return;
		}
		CommunityEvent activeEvent = this.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		if ((activeEvent.m_sID == CommunityEventManager._sSpongyGamesEventID || activeEvent.m_sID == CommunityEventManager._sSpongyGamesLastDayEventID) && this.m_pSession.TheGame.questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID) && (this.m_pSession.TheGame.simulation.FindSimulated(new int?(CommunityEventManager._nColiseumDID)) != null || this.m_pSession.TheGame.inventory.HasItem(new int?(CommunityEventManager._nColiseumDID))))
		{
			Soaring.FireEvent("spongy_games_banner", null);
		}
		this.m_fPreviousBannerPingTime = Time.time;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00041FAC File Offset: 0x000401AC
	private void UpdateValuesToSoaring()
	{
		int num = this.m_pCommunityEventDefinitions.count();
		for (int i = 0; i < num; i++)
		{
			CommunityEvent communityEvent = (CommunityEvent)this.m_pCommunityEventDefinitions.objectAtIndex(i);
			if (communityEvent.m_bActive)
			{
				this.UpdateValueToSoaring(communityEvent);
			}
		}
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00041FFC File Offset: 0x000401FC
	private void UpdateValueToSoaring(CommunityEvent pEvent)
	{
		if (pEvent == null)
		{
			return;
		}
		SBMISoaring.SetEventValue(this.m_pSession, int.Parse(pEvent.m_sID), this.m_pSession.TheGame.resourceManager.Resources[pEvent.m_nValueID].Amount, null);
		this.m_fPreviousValueUpdateTime = Time.time;
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00042064 File Offset: 0x00040264
	private void AquireEventGift(CommunityEvent pEvent, SoaringCommunityEvent.Reward pSoaringReward)
	{
		SBMISoaring.AquireEventGift(this.m_pSession, int.Parse(pEvent.m_sID), pSoaringReward.m_nID, 0, false, null);
		this.m_fPreviousRewardUpdateTime = Time.time;
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x000420A8 File Offset: 0x000402A8
	private void LoadCommunityEventsFromSpreadsheets()
	{
		this.m_pCommunityEventDefinitions = new MDictionary();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "CommunityEvents";
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		Dictionary<int, Dictionary<string, object>> dictionary = new Dictionary<int, Dictionary<string, object>>();
		List<int> list = new List<int>();
		string b = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
				if (dictionary.ContainsKey(intCell) || list.Contains(intCell))
				{
					TFUtils.ErrorLog("Community Event Collision! DID: " + intCell.ToString());
				}
				else if (instance.GetIntCell(sheetIndex, rowIndex, "enabled") != 1)
				{
					list.Add(intCell);
				}
				else
				{
					dictionary2.Add("did", intCell);
					dictionary2.Add("value_did", instance.GetIntCell(sheetIndex, rowIndex, "currency type"));
					dictionary2.Add("lifetime_contribute_cap", instance.GetIntCell(sheetIndex, rowIndex, "lifetime contribution cap"));
					dictionary2.Add("prerequisite_quest", instance.GetIntCell(sheetIndex, rowIndex, "prerequisite quest"));
					dictionary2.Add("hide_ui", instance.GetIntCell(sheetIndex, rowIndex, "hide ui") == 1);
					dictionary2.Add("name", instance.GetStringCell(sheetName, rowName, "name"));
					dictionary2.Add("start_date", instance.GetStringCell(sheetName, rowName, "start date"));
					dictionary2.Add("end_date", instance.GetStringCell(sheetName, rowName, "end date"));
					dictionary2.Add("event_button_texture", instance.GetStringCell(sheetName, rowName, "event button texture"));
					dictionary2.Add("tab_one_texture", instance.GetStringCell(sheetName, rowName, "tab texture 1"));
					dictionary2.Add("tab_two_texture", instance.GetStringCell(sheetName, rowName, "tab texture 2"));
					dictionary2.Add("left_banner_texture", instance.GetStringCell(sheetName, rowName, "header texture"));
					dictionary2.Add("right_banner_texture", instance.GetStringCell(sheetName, rowName, "title backing texture"));
					dictionary2.Add("right_banner_title", instance.GetStringCell(sheetName, rowName, "title"));
					dictionary2.Add("right_banner_description", instance.GetStringCell(sheetName, rowName, "individual tab header"));
					dictionary2.Add("individual_footer_text", instance.GetStringCell(sheetName, rowName, "individual tab footer"));
					dictionary2.Add("community_header_text", instance.GetStringCell(sheetName, rowName, "community tab header"));
					dictionary2.Add("community_footer_text", instance.GetStringCell(sheetName, rowName, "community tab footer"));
					dictionary2.Add("community_footer_all_unlocks_text", instance.GetStringCell(sheetName, rowName, "community tab footer all unlocks"));
					dictionary2.Add("community_footer_texture", instance.GetStringCell(sheetName, rowName, "community tab footer texture"));
					dictionary2.Add("quest_icon", instance.GetStringCell(sheetName, rowName, "quest list event reminder icon"));
					dictionary.Add(intCell, dictionary2);
				}
			}
		}
		sheetName = "CommunityEventItems";
		sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		int num2 = -1;
		for (int j = 0; j < num; j++)
		{
			string rowName = j.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
				if (num2 == -1)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "max unlocked land slots");
				}
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "event did");
				if (!dictionary.ContainsKey(intCell))
				{
					if (!list.Contains(intCell))
					{
						TFUtils.ErrorLog("Community Event can not be found for event item! Event DID: " + intCell.ToString());
					}
				}
				else
				{
					Dictionary<string, object> dictionary2 = dictionary[intCell];
					List<object> list2;
					if (dictionary2.ContainsKey("rewards"))
					{
						list2 = (List<object>)dictionary2["rewards"];
					}
					else
					{
						list2 = new List<object>();
					}
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
					dictionary3.Add("threshold", instance.GetIntCell(sheetIndex, rowIndex, "threshold"));
					dictionary3.Add("width", instance.GetIntCell(sheetIndex, rowIndex, "width"));
					dictionary3.Add("height", instance.GetIntCell(sheetIndex, rowIndex, "height"));
					dictionary3.Add("dialog_sequence", instance.GetIntCell(sheetIndex, rowIndex, "dialog sequence"));
					dictionary3.Add("auto_place_y", instance.GetIntCell(sheetIndex, rowIndex, "auto place y"));
					dictionary3.Add("auto_place_x", instance.GetIntCell(sheetIndex, rowIndex, "auto place x"));
					dictionary3.Add("hide_name_when_locked", instance.GetIntCell(sheetIndex, rowIndex, "hide name when locked") == 1);
					dictionary3.Add("item_name", instance.GetStringCell(sheetName, rowName, "item name"));
					dictionary3.Add("type", instance.GetStringCell(sheetName, rowName, "type"));
					dictionary3.Add("entity_type", instance.GetStringCell(sheetName, rowName, "reward type"));
					dictionary3.Add("texture", instance.GetStringCell(sheetName, rowName, "texture"));
					string stringCell = instance.GetStringCell(sheetName, rowName, "Locked Texture");
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						dictionary3.Add("locked_texture", stringCell);
					}
					else
					{
						dictionary3.Add("locked_texture", null);
					}
					List<int> list3 = new List<int>();
					for (int k = 1; k <= num2; k++)
					{
						int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "unlocked land did " + k.ToString());
						if (intCell2 >= 0)
						{
							list3.Add(intCell2);
						}
					}
					dictionary3.Add("land_dids", list3);
					list2.Add(dictionary3);
					dictionary2["rewards"] = list2;
					dictionary[intCell] = dictionary2;
				}
			}
		}
		foreach (KeyValuePair<int, Dictionary<string, object>> keyValuePair in dictionary)
		{
			CommunityEvent communityEvent = new CommunityEvent(keyValuePair.Value);
			this.m_pCommunityEventDefinitions.addValue(communityEvent, communityEvent.m_sID);
		}
	}

	// Token: 0x0400072F RID: 1839
	private const int m_nUpdateEventValueTimeLimit = 300;

	// Token: 0x04000730 RID: 1840
	private const int m_nUpdateEventMinWaitTime = 10;

	// Token: 0x04000731 RID: 1841
	public static Dictionary<string, object> _pEventStatusDialogData;

	// Token: 0x04000732 RID: 1842
	public static string _sSpongyGamesEventID = "1";

	// Token: 0x04000733 RID: 1843
	public static string _sSpongyGamesLastDayEventID = "2";

	// Token: 0x04000734 RID: 1844
	public static string _sChrismas14EventID = "3";

	// Token: 0x04000735 RID: 1845
	public static int _nColiseumDID = 20403;

	// Token: 0x04000736 RID: 1846
	public static string _sValentines15EventID = "4";

	// Token: 0x04000737 RID: 1847
	private volatile Action dialogNeededCallback;

	// Token: 0x04000738 RID: 1848
	private MDictionary m_pCommunityEventDefinitions;

	// Token: 0x04000739 RID: 1849
	private float m_fUpdateEventValueTimer;

	// Token: 0x0400073A RID: 1850
	private float m_fPreviousValueUpdateTime;

	// Token: 0x0400073B RID: 1851
	private float m_fPreviousRewardUpdateTime;

	// Token: 0x0400073C RID: 1852
	private float m_fPreviousBannerPingTime;

	// Token: 0x0400073D RID: 1853
	private Session m_pSession;
}
