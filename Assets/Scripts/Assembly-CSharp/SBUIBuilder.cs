using System;
using System.Collections.Generic;
using MTools;
using UnityEngine;
using Yarg;

// Token: 0x020000BB RID: 187
public static class SBUIBuilder
{
	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002CA1C File Offset: 0x0002AC1C
	private static GUIMainView MainView
	{
		get
		{
			if (SBUIBuilder.mainView == null)
			{
				SBUIBuilder.mainView = GUIMainView.GetInstance();
			}
			return SBUIBuilder.mainView;
		}
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002CA40 File Offset: 0x0002AC40
	public static SBUIBuilder.ScreenContext PushNewScreenContext()
	{
		SBUIBuilder.topContext = new SBUIBuilder.ScreenContext
		{
			next = SBUIBuilder.topContext
		};
		return SBUIBuilder.topContext;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002CA6C File Offset: 0x0002AC6C
	public static void ReleaseScreenContexts(SBUIBuilder.ScreenContext start, SBUIBuilder.ScreenContext end)
	{
		TFUtils.Assert(SBUIBuilder.topContext != null || (start == null && end == null), "Cannot try to release screens since there are no contexts in SBUIBuilder");
		TFUtils.Assert(start != null, "Cannot release a null context on a non-empty gui stack.");
		SBUIBuilder.ScreenContext screenContext = null;
		int num = 0;
		int num2 = 0;
		for (SBUIBuilder.ScreenContext screenContext2 = SBUIBuilder.topContext; screenContext2 != start; screenContext2 = screenContext2.next)
		{
			screenContext = screenContext2;
			num += screenContext2.layers;
		}
		for (SBUIBuilder.ScreenContext screenContext2 = start; screenContext2 != end; screenContext2 = screenContext2.next)
		{
			num2 += screenContext2.layers;
		}
		SBUIBuilder.ReleaseScreens(num, num2);
		screenContext.next = end;
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002CB0C File Offset: 0x0002AD0C
	private static SBGUIScreen OptionalCacheScreen(string key, SBUIBuilder.MakeScreen make, out bool instantiated)
	{
		instantiated = false;
		if (TFPerfUtils.MemoryLod < CommonUtils.LevelOfDetail.Standard)
		{
			SBGUIScreen result = make();
			instantiated = true;
			return result;
		}
		if (!SBUIBuilder.sCache.ContainsKey(key))
		{
			SBGUIScreen value = make();
			SBUIBuilder.sCache[key] = value;
			instantiated = true;
		}
		else
		{
			SBUIBuilder.sCache[key].MuteButtons(false);
		}
		return SBUIBuilder.sCache[key];
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0002CB7C File Offset: 0x0002AD7C
	private static SBGUIScreen CacheScreen(string key, SBUIBuilder.MakeScreen make, out bool instantiated)
	{
		instantiated = false;
		if (!SBUIBuilder.sCache.ContainsKey(key))
		{
			SBGUIScreen value = make();
			SBUIBuilder.sCache[key] = value;
			instantiated = true;
		}
		return SBUIBuilder.sCache[key];
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0002CBC0 File Offset: 0x0002ADC0
	private static void PushScreen(SBGUIScreen screen)
	{
		SBUIBuilder.topContext.layers++;
		SBGUI.GetInstance().PushGUIScreen(screen);
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0002CBE0 File Offset: 0x0002ADE0
	public static SBGUIScreen PeekTopScreen()
	{
		return SBGUI.GetInstance().PeekGUIScreen();
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0002CBEC File Offset: 0x0002ADEC
	public static SBGUIScreen ReleaseTopScreen()
	{
		return SBUIBuilder.ReleaseScreen(0);
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0002CBF4 File Offset: 0x0002ADF4
	public static SBGUIScreen ReleaseScreen(int depth)
	{
		SBUIBuilder.ScreenContext next = SBUIBuilder.topContext;
		int num = depth;
		while (num >= next.layers && true)
		{
			num -= next.layers;
			next = next.next;
		}
		next.layers--;
		TFUtils.Assert(next.layers >= 0, "Shouldn't be getting negative layer counts. Something must have gone wrong!");
		SBGUIScreen sbguiscreen = SBGUI.GetInstance().RemoveGUIScreen(depth);
		TFUtils.Assert(sbguiscreen != null, "Removed a GUI Screen at depth " + depth + ", but it's null)");
		if (!SBGUI.GetInstance().ContainsGUIScreen(sbguiscreen))
		{
			if (!SBUIBuilder.sCache.ContainsValue(sbguiscreen))
			{
				sbguiscreen.Close();
			}
			else
			{
				sbguiscreen.Deactivate();
			}
		}
		if (SBUIBuilder.sCache.ContainsValue(sbguiscreen))
		{
			sbguiscreen.OnPutIntoCache.FireEvent();
		}
		return sbguiscreen;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0002CCD0 File Offset: 0x0002AED0
	public static void ReleaseScreens(int depth, int layers)
	{
		TFUtils.Assert(depth + layers <= SBGUI.GetInstance().GUIScreenCount, string.Format("Cannot remove {0} layers at depth {1} from GUI Stack with count {2}.", layers, depth, SBGUI.GetInstance().GUIScreenCount));
		while (layers > 0)
		{
			SBUIBuilder.ReleaseScreen(depth);
			layers--;
		}
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0002CD34 File Offset: 0x0002AF34
	public static void ClearScreenCache()
	{
		foreach (string key in SBUIBuilder.sCache.Keys)
		{
			SBGUIScreen sbguiscreen = SBUIBuilder.sCache[key];
			sbguiscreen.Deactivate();
			UnityEngine.Object.Destroy(sbguiscreen.gameObject);
		}
		SBUIBuilder.sCache.Clear();
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002CDC0 File Offset: 0x0002AFC0
	public static SBGUIAcceptUI MakeAndPushAcceptUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action acceptButtonClickHandler)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUIAcceptUI)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AcceptUI");
		bool flag;
		SBGUIAcceptUI sbguiacceptUI = (SBGUIAcceptUI)SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/AcceptUI", make, out flag);
		if (flag)
		{
			SBUIBuilder.AddTrackingForResources(sbguiacceptUI, session);
		}
		SBUIBuilder.UpdateTrackingForSpecialResource(sbguiacceptUI, session);
		sbguiacceptUI.session = session;
		SBGUIButton button = sbguiacceptUI.AttachActionToButton("accept", acceptButtonClickHandler);
		SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		sbguiacceptUI.gameObject.SetActiveRecursively(true);
		sbguiacceptUI.ReactivateButton(button);
		SBUIBuilder.PushScreen(sbguiacceptUI);
		return sbguiacceptUI;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0002CE4C File Offset: 0x0002B04C
	public static SBGUIScreen MakeAndPushScratchLayer(Session session)
	{
		SBGUIScreen sbguiscreen = SBGUIScreen.Create(null, session);
		SBUIBuilder.PushScreen(sbguiscreen);
		return sbguiscreen;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0002CE68 File Offset: 0x0002B068
	public static SBGUIScreen MakeAndPushEmptyUI(Session session, Action<SBGUIEvent, Session> guiEventHandler)
	{
		SBGUIScreen sbguiscreen = SBGUIScreen.Create(null, session);
		SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		SBUIBuilder.PushScreen(sbguiscreen);
		return sbguiscreen;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0002CE8C File Offset: 0x0002B08C
	public static Action<SBGUIEvent, Session> UpdateGuiEventHandler(Session session, Action<SBGUIEvent, Session> guiEventHandler)
	{
		if (session == null)
		{
			TFUtils.Assert(false, "Session is null");
			return null;
		}
		GUIMainView instance = GUIMainView.GetInstance();
		Action<YGEvent> oldYargHandler = instance.FinalEventListener.GetListener();
		Action<SBGUIEvent, Session> result = delegate(SBGUIEvent e, Session s)
		{
			oldYargHandler(e);
		};
		instance.ClearFinalEventListener();
		instance.FinalEventListener.AddListener(delegate(YGEvent x)
		{
			session.TheCamera.HandleGUIEvent(new SBGUIEvent(x));
		});
		if (guiEventHandler != null)
		{
			instance.FinalEventListener.AddListener(delegate(YGEvent x)
			{
				guiEventHandler(new SBGUIEvent(x), session);
			});
		}
		return result;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0002CF2C File Offset: 0x0002B12C
	private static void AddTrackingForResource(SBGUIScreen screen, string labelKey, int resourceId)
	{
		SBGUILabel sbguilabel = (SBGUILabel)screen.FindChild(labelKey);
		string text = resourceId.ToString();
		screen.dynamicLabels[text] = sbguilabel;
		if (!((List<string>)screen.dynamicProperties["TrackingResourceAmounts"]).Contains(text))
		{
			((List<string>)screen.dynamicProperties["TrackingResourceAmounts"]).Add(text);
		}
		sbguilabel.Text = text;
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0002CFA0 File Offset: 0x0002B1A0
	private static void AddTrackingForResources(SBGUIScreen screen, Session session)
	{
		TFUtils.Assert(screen != null, "Cannot add tracking to a null screen");
		TFUtils.Assert(session != null, "Cannot add tracking to a null session");
		screen.session = session;
		List<string> value = new List<string>();
		screen.dynamicProperties["TrackingResourceAmounts"] = value;
		screen.UpdateCallback.ClearListeners();
		screen.UpdateCallback.AddListener(new Action<SBGUIScreen, Session>(SBUIBuilder.UpdateStandardUI));
		SBUIBuilder.AddTrackingForResource(screen, "money_label", ResourceManager.SOFT_CURRENCY);
		SBUIBuilder.AddTrackingForResource(screen, "premium_label", ResourceManager.HARD_CURRENCY);
		SBUIBuilder.AddTrackingForResource(screen, "level_label", ResourceManager.LEVEL);
		List<string> value2 = new List<string>();
		screen.dynamicProperties["TrackingResourcePercentages"] = value2;
		SBGUIProgressMeter value3 = (SBGUIProgressMeter)screen.FindChild("xp_meter");
		string text = ResourceManager.XP.ToString();
		screen.dynamicMeters[text] = value3;
		((List<string>)screen.dynamicProperties["TrackingResourcePercentages"]).Add(text);
		SBGUILabel value4 = (SBGUILabel)screen.FindChild("amount_xp_label");
		screen.dynamicLabels["amount_xp_label"] = value4;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0002D0C0 File Offset: 0x0002B2C0
	private static void UpdateTrackingForSpecialResource(SBGUIScreen screen, Session session)
	{
		SBGUIElement sbguielement = screen.FindChild("inventory_widget");
		if (SBUIBuilder.invWidgetStartPos == new Vector3(-999999f, -999999f, -999999f))
		{
			SBUIBuilder.invWidgetStartPos = sbguielement.tform.localPosition;
		}
		if (SBUIBuilder.specialBarParent == null)
		{
			SBGUIElement sbguielement2 = screen.FindChild("special_bar");
			SBUIBuilder.specialBarParent = sbguielement2.transform.parent;
			SBUIBuilder.specialBarStartPosition = sbguielement2.transform.localPosition;
		}
		bool flag = false;
		if (ResourceManager.SPECIAL_CURRENCY >= 0)
		{
			int currencyDisplayQuestTrigger = session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].CurrencyDisplayQuestTrigger;
			if (currencyDisplayQuestTrigger < 0 || session.TheGame.questManager.IsQuestActive((uint)currencyDisplayQuestTrigger) || session.TheGame.questManager.IsQuestCompleted((uint)currencyDisplayQuestTrigger))
			{
				SBGUIElement sbguielement3 = screen.FindChild("special_bar");
				if (sbguielement3 == null)
				{
					sbguielement3 = GUIMainView.GetInstance().transform.FindChild("special_bar").GetComponent<SBGUIElement>();
					sbguielement3.transform.parent = SBUIBuilder.specialBarParent;
					sbguielement3.transform.localPosition = SBUIBuilder.specialBarStartPosition;
					sbguielement3.gameObject.layer = SBUIBuilder.specialBarParent.gameObject.layer;
				}
				SBUIBuilder.AddTrackingForResource(screen, "special_label", ResourceManager.SPECIAL_CURRENCY);
				string resourceTexture = session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].GetResourceTexture();
				((SBGUIPulseImage)screen.FindChild("special_icon")).SetTextureFromAtlas(resourceTexture);
				sbguielement.tform.localPosition = SBUIBuilder.invWidgetStartPos;
				sbguielement3.SetVisible(true);
				sbguielement3.SetActive(true);
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (flag)
		{
			SBGUIElement sbguielement4 = screen.FindChild("special_bar");
			if (sbguielement4 != null)
			{
				sbguielement4.SetVisible(false);
				sbguielement4.SetActive(false);
				sbguielement4.SetParent(null);
				sbguielement.tform.localPosition = SBUIBuilder.invWidgetStartPos + new Vector3(0f, 0.309429f, 0f);
			}
		}
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0002D2F4 File Offset: 0x0002B4F4
	private static void UpdateQuestCounter(SBGUIScreen screen, Session session)
	{
		SBGUIStandardScreen sbguistandardScreen = (SBGUIStandardScreen)screen;
		sbguistandardScreen.QuestCountLabel.SetText(sbguistandardScreen.GetQuestButtonCount.ToString());
		sbguistandardScreen.HideZeroQuestCount();
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0002D328 File Offset: 0x0002B528
	public static SBGUIScreen MakeAndPushStartingProgress(Session session, Action privacyHandler, Action<SBGUIEvent, Session> guiEventHandler, bool makeLoadingBar, bool bPatchy)
	{
		string prefabName = string.Empty;
		if (bPatchy)
		{
			prefabName = "Prefabs/GUI/Screens/PatchyLoadingScreen";
		}
		else
		{
			prefabName = "Prefabs/GUI/Screens/StartingProgress";
		}
		SBGUIScreen sbguiscreen = (SBGUIScreen)SBGUI.InstantiatePrefab(prefabName);
		sbguiscreen.session = session;
		sbguiscreen.SetParent(null);
		SBScaleForLanguage component = sbguiscreen.GetComponent<SBScaleForLanguage>();
		if (component != null)
		{
			component.Scale();
		}
		sbguiscreen.AttachActionToButton("privacy_policy", privacyHandler);
		SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		SBGUIProgressMeter sbguiprogressMeter = (SBGUIProgressMeter)sbguiscreen.FindChild("progress_meter");
		sbguiscreen.dynamicMeters["loading"] = sbguiprogressMeter;
		if (!makeLoadingBar)
		{
			sbguiprogressMeter.renderer.enabled = makeLoadingBar;
			sbguiprogressMeter.gameObject.SetActiveRecursively(makeLoadingBar);
		}
		SBGUILabel sbguilabel = (SBGUILabel)sbguiscreen.FindChild("count_label");
		sbguiscreen.dynamicLabels["progress"] = sbguilabel;
		sbguilabel.renderer.enabled = makeLoadingBar;
		if (makeLoadingBar)
		{
			SBUIBuilder.MakeActivityIndicator(sbguiscreen);
		}
		SBGUIElement sbguielement = sbguiscreen.FindChild("loading_spinner");
		if (sbguielement != null)
		{
			if (sbguielement.renderer != null)
			{
				sbguielement.renderer.enabled = makeLoadingBar;
			}
			sbguielement.gameObject.SetActiveRecursively(makeLoadingBar);
		}
		SBUIBuilder.PushScreen(sbguiscreen);
		return sbguiscreen;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0002D468 File Offset: 0x0002B668
	public static SBGUIStandardScreen MakeAndPushStandardUI(Session session, bool allowHiding, Action<SBGUIEvent, Session> guiEventHandler, Action shopClickHandler, Action inventoryClickHandler, Action optionsHandler, Action editClickHandler, Action pavingClickHandler, Action<int, YGEvent> startDragOutHandler, Action<YGEvent> dragThroughHandler, Action openIAPTabHandlerSoft, Action openIAPTabHandlerHard, Action communityEventClickHandler, Action patchyClickHandler, bool editing = false)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUIStandardScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/StandardUI");
		bool flag;
		SBGUIStandardScreen screen = (SBGUIStandardScreen)SBUIBuilder.CacheScreen("Prefabs/GUI/Screens/StandardUI", make, out flag);
		screen.session = session;
		if (flag)
		{
			screen.Initialize(session);
			SBUIBuilder.AddTrackingForResources(screen, session);
			screen.AttachActionToButton("quest", delegate()
			{
				screen.ToggleQuestTracker(session, false, true);
			});
		}
		else
		{
			screen.RefreshFromCache();
		}
		SBUIBuilder.UpdateTrackingForSpecialResource(screen, session);
		screen.EnableFullHiding(allowHiding);
		SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		Action action = delegate()
		{
			Vector3 localPosition = screen.questMarker.transform.localPosition;
			localPosition.y -= 0.7f;
			screen.questMarker.transform.localPosition = localPosition;
		};
		Action action2 = delegate()
		{
			Vector3 localPosition = screen.questMarker.transform.localPosition;
			localPosition.y += 0.7f;
			screen.questMarker.transform.localPosition = localPosition;
		};
		SBGUILabel sbguilabel = (SBGUILabel)screen.FindChild("happy_label");
		sbguilabel.SetText(Language.Get("!!HAPPINESS"));
		Action action3 = delegate()
		{
			AnalyticsWrapper.LogEventButtonClick(session.TheGame);
			if (communityEventClickHandler != null)
			{
				communityEventClickHandler();
			}
		};
		screen.ClearButtonActions("marketplace");
		screen.ClearButtonActions("inventory");
		screen.ClearButtonActions("settings");
		screen.ClearButtonActions("edit");
		screen.ClearButtonActions("jj_button");
		screen.ClearButtonActions("coin_button");
		screen.ClearButtonActions("quest_scroll_up");
		screen.ClearButtonActions("quest_scroll_down");
		screen.ClearButtonActions("editpath_toggle");
		screen.ClearButtonActions("community_event");
		screen.ClearButtonActions("patchy");
		SBGUIButton sbguibutton = screen.AttachActionToButton("marketplace", shopClickHandler);
		screen.AttachActionToButton("inventory", inventoryClickHandler);
		SBGUIButton sbguibutton2 = screen.AttachActionToButton("settings", optionsHandler);
		screen.AttachActionToButton("edit", editClickHandler);
		screen.AttachActionToButton("editpath_toggle", pavingClickHandler);
		screen.AttachActionToButton("jj_button", openIAPTabHandlerHard);
		screen.AttachActionToButton("coin_button", openIAPTabHandlerSoft);
		screen.AttachActionToButton("quest_scroll_up", action);
		screen.AttachActionToButton("quest_scroll_down", action2);
		screen.AttachActionToButton("community_event", communityEventClickHandler);
		SBGUIButton sbguibutton3 = screen.AttachActionToButton("patchy", patchyClickHandler);
		if (sbguibutton3 != null)
		{
			screen.AttachActionToButton("patchy", patchyClickHandler);
		}
		screen.SetInventoryWidgetDraggingCallbacks(startDragOutHandler, dragThroughHandler);
		SBUIBuilder.PushScreen(screen);
		screen.EnableButtons(true);
		SBGUIAtlasButton sbguiatlasButton = (SBGUIAtlasButton)screen.FindChild("edit");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)screen.FindChild("edit_mode_fish_person");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)screen.FindChild("edit_mode_text");
		SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)screen.FindChild("edit_mode_fence_post");
		SBGUIAtlasImage sbguiatlasImage4 = (SBGUIAtlasImage)screen.FindChild("edit_mode_placard");
		SBGUIAtlasButton sbguiatlasButton2 = (SBGUIAtlasButton)screen.FindChild("editpath_toggle");
		SBGUILabel sbguilabel2 = (SBGUILabel)screen.FindChild("debug_info");
		List<SBGUIAtlasImage> list = new List<SBGUIAtlasImage>();
		for (int i = 0; i < 14; i++)
		{
			list.Add((SBGUIAtlasImage)screen.FindChild("edit_mode_bar_" + (i + 1)));
		}
		if (SBSettings.ShowDebug)
		{
			if (SBUIBuilder.game_revision == null)
			{
				try
				{
					MBinaryReader fileStream = ResourceUtils.GetFileStream("git_revision", null, "bytes");
					if (fileStream != null)
					{
						byte[] array = fileStream.ReadAllBytes();
						SBUIBuilder.game_revision = " Commit: ";
						for (int j = 0; j < array.Length; j++)
						{
							SBUIBuilder.game_revision += (char)array[j];
						}
						SBUIBuilder.game_revision += "\n";
					}
					sbguilabel2.Size *= 0.9f;
				}
				catch
				{
					SBUIBuilder.game_revision = string.Empty;
				}
			}
			string text = string.Empty;
			try
			{
				if (Soaring.IsInitialized)
				{
					sbguilabel2.Size *= 1.2f;
					text = " " + SoaringTime.Epoch.AddSeconds((double)SoaringTime.AdjustedServerTime).ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Timestamp Threw Error: " + ex.Message);
				SBUIBuilder.game_revision = string.Empty;
			}
			sbguilabel2.SetText(string.Concat(new string[]
			{
				"ID: ",
				Soaring.Player.UserTag,
				" Ver: ",
				SBSettings.BundleVersion,
				SBUIBuilder.game_revision,
				SBSettings.SERVER_URL,
				text
			}));
			sbguilabel2.SetVisible(true);
		}
		else
		{
			sbguilabel2.SetVisible(false);
		}
		if (editing)
		{
			sbguiatlasButton.SetTextureFromAtlas("EditModeIcon_On.png");
			sbguiatlasImage.SetVisible(true);
			sbguiatlasImage2.SetVisible(true);
			sbguiatlasImage3.SetVisible(true);
			sbguiatlasImage4.SetVisible(true);
			sbguiatlasButton2.SetVisible(true);
			sbguiatlasButton2.enabled = true;
			sbguiatlasButton2.SetTextureFromAtlas("EditToggle_Building.png");
			for (int k = 0; k < list.Count; k++)
			{
				list[k].SetVisible(true);
			}
			screen.HideElementsForEditMode(true);
			sbguibutton.EnableButtons(false);
			sbguibutton2.EnableButtons(false);
			if (sbguibutton3 != null)
			{
				sbguibutton3.SetActive(false);
			}
		}
		else
		{
			sbguiatlasButton.SetTextureFromAtlas("EditModeIcon_Off.png");
			sbguiatlasImage.SetVisible(false);
			sbguiatlasImage2.SetVisible(false);
			sbguiatlasImage3.SetVisible(false);
			sbguiatlasImage4.SetVisible(false);
			sbguiatlasButton2.SetVisible(false);
			sbguiatlasButton2.enabled = false;
			for (int l = 0; l < list.Count; l++)
			{
				list[l].SetVisible(false);
			}
			screen.HideElementsForEditMode(false);
			sbguibutton.EnableButtons(true);
			sbguibutton2.EnableButtons(true);
			if (sbguibutton3 != null)
			{
				sbguibutton3.SetActive(false);
			}
		}
		session.AddAsyncResponse("standardUI", screen, false);
		SBUIBuilder.UpdateQuestCounter(screen, session);
		screen.DisableInactiveElements();
		return screen;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002DBE0 File Offset: 0x0002BDE0
	public static SBGUIStandardScreen MakeAndPushPavingUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action acceptHandler, Action editHandler, Action inventoryHandler)
	{
		SBGUIStandardScreen sbguistandardScreen = SBUIBuilder.MakeAndPushStandardUI(session, false, guiEventHandler, null, inventoryHandler, null, editHandler, acceptHandler, null, null, null, null, null, null, true);
		SBGUIButton sbguibutton = (SBGUIButton)sbguistandardScreen.FindChild("community_event");
		if (sbguibutton != null)
		{
			sbguibutton.SetActive(false);
		}
		SBGUIAtlasButton sbguiatlasButton = (SBGUIAtlasButton)sbguistandardScreen.FindChild("editpath_toggle");
		sbguiatlasButton.SetTextureFromAtlas("EditToggle_Path.png");
		return sbguistandardScreen;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0002DC44 File Offset: 0x0002BE44
	public static SBGUIInsufficientResourcesDialog MakeAndPushInsufficientResourcesDialog(Session session, Dictionary<int, int> insufficientResourceIds, Dictionary<string, int> insufficientResourceTextures, int? rmtCost, string rmtTexture, string acceptLabel, Action okButtonHandler, Action cancelButtonHandler)
	{
		string title = Language.Get("!!PREFAB_NOT_ENOUGH_RESOURCES");
		if (insufficientResourceIds.Count == 1)
		{
			foreach (int key in insufficientResourceIds.Keys)
			{
				string str = Language.Get(session.TheGame.resourceManager.Resources[key].Name);
				title = Language.Get("!!PREFAB_NOT_ENOUGH") + " " + str;
			}
		}
		string message = Language.Get("!!PREFAB_STILL_NEED");
		SBGUIInsufficientResourcesDialog sbguiinsufficientResourcesDialog = (SBGUIInsufficientResourcesDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/InsufficientResourcesDialog");
		sbguiinsufficientResourcesDialog.session = session;
		sbguiinsufficientResourcesDialog.SetParent(null);
		sbguiinsufficientResourcesDialog.transform.localPosition = Vector3.zero;
		sbguiinsufficientResourcesDialog.AttachActionToButton("nevermind_button", cancelButtonHandler);
		sbguiinsufficientResourcesDialog.AttachActionToButton("shopping_button", okButtonHandler);
		sbguiinsufficientResourcesDialog.AttachActionToButton("TouchableBackground", cancelButtonHandler);
		if (insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.HALLOWEEN_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY_V2].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.VALENTINES_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPONGY_GAMES_CURRENCY].Did) || (ResourceManager.SPECIAL_CURRENCY >= 0 && insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].Did)))
		{
			rmtCost = null;
		}
		sbguiinsufficientResourcesDialog.SetUp(title, message, acceptLabel, insufficientResourceTextures, rmtCost, rmtTexture, string.Empty);
		if (!session.TheGame.featureManager.CheckFeature("jit_jj_purchases"))
		{
			SBGUIButton component = sbguiinsufficientResourcesDialog.FindChild("shopping_button").GetComponent<SBGUIButton>();
			component.ClickEvent -= okButtonHandler;
			component.EnableButtons(false);
			component.GetComponent<YGAtlasSprite>().SetColor(new Color(1f, 1f, 1f, 0.15f));
			sbguiinsufficientResourcesDialog.FindChild("cost_label").GetComponent<SBGUILabel>().SetColor(new Color(0.5f, 0.5f, 0.5f));
		}
		if (insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.HALLOWEEN_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.CHRISTMAS_CURRENCY_V2].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.VALENTINES_CURRENCY].Did) || insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPONGY_GAMES_CURRENCY].Did) || (ResourceManager.SPECIAL_CURRENCY >= 0 && insufficientResourceIds.ContainsKey(session.TheGame.resourceManager.Resources[ResourceManager.SPECIAL_CURRENCY].Did)))
		{
			SBGUIElement component2 = sbguiinsufficientResourcesDialog.FindChild("shopping_buttonframe").GetComponent<SBGUIElement>();
			component2.SetVisible(false);
			SBGUIButton component3 = sbguiinsufficientResourcesDialog.FindChild("shopping_button").GetComponent<SBGUIButton>();
			component3.ClickEvent -= okButtonHandler;
			component3.EnableButtons(false);
			component3.SetVisible(false);
			SBGUILabel component4 = sbguiinsufficientResourcesDialog.FindChild("shopping_label").GetComponent<SBGUILabel>();
			component4.SetVisible(false);
			SBGUILabel component5 = sbguiinsufficientResourcesDialog.FindChild("cost_label").GetComponent<SBGUILabel>();
			component5.SetVisible(false);
			SBGUIElement component6 = sbguiinsufficientResourcesDialog.FindChild("cost_marker").GetComponent<SBGUIElement>();
			component6.SetVisible(false);
			SBGUIElement component7 = sbguiinsufficientResourcesDialog.FindChild("nevermind_buttonframe").GetComponent<SBGUIElement>();
			component7.SetPosition(sbguiinsufficientResourcesDialog.GetScreenPosition().x, component7.GetScreenPosition().y, component7.WorldPosition.z);
			SBGUIButton component8 = sbguiinsufficientResourcesDialog.FindChild("nevermind_button").GetComponent<SBGUIButton>();
			component8.SetPosition(sbguiinsufficientResourcesDialog.GetScreenPosition().x, component8.GetScreenPosition().y, component8.WorldPosition.z);
		}
		SBUIBuilder.PushScreen(sbguiinsufficientResourcesDialog);
		return sbguiinsufficientResourcesDialog;
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002E150 File Offset: 0x0002C350
	public static SBGUIConfirmationDialog MakeAndPushConfirmationDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		SBGUIConfirmationDialog sbguiconfirmationDialog = (SBGUIConfirmationDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ConfirmationDialog");
		sbguiconfirmationDialog.session = session;
		if (unmutable)
		{
			(sbguiconfirmationDialog.FindChild("cancel_button") as SBGUIButton).unmutable = true;
			(sbguiconfirmationDialog.FindChild("okay_button") as SBGUIButton).unmutable = true;
		}
		if (guiEventHandler != null)
		{
			SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		}
		if (cancelButtonHandler != null)
		{
			sbguiconfirmationDialog.AttachActionToButton("cancel_button", cancelButtonHandler);
		}
		sbguiconfirmationDialog.AttachActionToButton("TouchableBackground", delegate()
		{
		});
		sbguiconfirmationDialog.AttachActionToButton("okay_button", okButtonHandler);
		sbguiconfirmationDialog.SetUp(title, message, acceptLabel, cancelLabel, resources, string.Empty);
		SBGUIStandardScreen sbguistandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sbguistandardScreen != null)
		{
			sbguistandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sbguistandardScreen);
		}
		SBUIBuilder.PushScreen(sbguiconfirmationDialog);
		return sbguiconfirmationDialog;
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0002E24C File Offset: 0x0002C44C
	public static SBGUIFoundItemScreen MakeAndPushAcknowledgeDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string texture, string acceptLabel, Action okButtonHandler)
	{
		SBGUIFoundItemScreen sbguifoundItemScreen = (SBGUIFoundItemScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/FoundItemScreen");
		sbguifoundItemScreen.session = session;
		if (guiEventHandler != null)
		{
			SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		}
		sbguifoundItemScreen.AttachActionToButton("okay", okButtonHandler);
		sbguifoundItemScreen.AttachActionToButton("TouchableBackground", delegate()
		{
		});
		sbguifoundItemScreen.Setup(title, message, texture, false, string.Empty);
		session.TheSoundEffectManager.PlaySound("Error");
		SBGUIStandardScreen sbguistandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sbguistandardScreen != null)
		{
			sbguistandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sbguistandardScreen);
		}
		SBUIBuilder.PushScreen(sbguifoundItemScreen);
		return sbguifoundItemScreen;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x0002E310 File Offset: 0x0002C510
	public static SBGUIConfirmationDialog MakeAndPushExpansionDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		SBGUIConfirmationDialog sbguiconfirmationDialog = (SBGUIConfirmationDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ExpansionDialog");
		sbguiconfirmationDialog.session = session;
		if (unmutable)
		{
			(sbguiconfirmationDialog.FindChild("cancel_button") as SBGUIButton).unmutable = true;
			(sbguiconfirmationDialog.FindChild("okay_button") as SBGUIButton).unmutable = true;
		}
		if (guiEventHandler != null)
		{
			SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		}
		if (cancelButtonHandler != null)
		{
			sbguiconfirmationDialog.AttachActionToButton("cancel_button", cancelButtonHandler);
		}
		sbguiconfirmationDialog.AttachActionToButton("TouchableBackground", delegate()
		{
		});
		sbguiconfirmationDialog.AttachActionToButton("okay_button", okButtonHandler);
		sbguiconfirmationDialog.SetUp(title, message, acceptLabel, cancelLabel, resources, Language.Get("!!PREFAB_COSTS"));
		SBGUIStandardScreen sbguistandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sbguistandardScreen != null)
		{
			sbguistandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sbguistandardScreen);
		}
		SBUIBuilder.PushScreen(sbguiconfirmationDialog);
		return sbguiconfirmationDialog;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0002E410 File Offset: 0x0002C610
	public static SBGUIGetJellyDialog MakeAndPushGetJellyDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string question, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		SBGUIGetJellyDialog sbguigetJellyDialog = (SBGUIGetJellyDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/GetJellyDialog");
		sbguigetJellyDialog.session = session;
		if (unmutable)
		{
			(sbguigetJellyDialog.FindChild("cancel_button") as SBGUIButton).unmutable = true;
			(sbguigetJellyDialog.FindChild("okay_button") as SBGUIButton).unmutable = true;
		}
		if (guiEventHandler != null)
		{
			SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		}
		if (cancelButtonHandler != null)
		{
			sbguigetJellyDialog.AttachActionToButton("cancel_button", cancelButtonHandler);
		}
		sbguigetJellyDialog.AttachActionToButton("TouchableBackground", delegate()
		{
		});
		sbguigetJellyDialog.AttachActionToButton("okay_button", okButtonHandler);
		sbguigetJellyDialog.SetUp(title, message, question, acceptLabel, cancelLabel, resources);
		SBGUIStandardScreen sbguistandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sbguistandardScreen != null)
		{
			sbguistandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sbguistandardScreen);
		}
		SBUIBuilder.PushScreen(sbguigetJellyDialog);
		return sbguigetJellyDialog;
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0002E508 File Offset: 0x0002C708
	public static SBGUIMicroConfirmDialog MakeAndPushJjMicroConfirmDialog(Session session, Action<SBGUIEvent, Session> overrideGuiEventHandler, string message, Cost.CostAtTime jjAmount, Action acceptHandler, Action cancelHandler, Vector2 screenPosition)
	{
		SBGUIMicroConfirmDialog sbguimicroConfirmDialog = (SBGUIMicroConfirmDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/HardSpendMicroconfirm");
		if (overrideGuiEventHandler != null)
		{
			SBUIBuilder.UpdateGuiEventHandler(session, overrideGuiEventHandler);
		}
		sbguimicroConfirmDialog.AttachActionToButton("cancel_button", cancelHandler);
		sbguimicroConfirmDialog.AttachActionToButton("touch_mask", cancelHandler);
		sbguimicroConfirmDialog.AttachActionToButton("modality_enforcer", cancelHandler);
		sbguimicroConfirmDialog.AttachActionToButton("okay_button", acceptHandler);
		SBGUIShadowedLabel component = sbguimicroConfirmDialog.FindChild("message").GetComponent<SBGUIShadowedLabel>();
		component.Text = Language.Get("!!PREFAB_CONFIRM_PURCHASE_SHORT");
		sbguimicroConfirmDialog.SetHardAmount(jjAmount(TFUtils.EpochTime()).ResourceAmounts[ResourceManager.HARD_CURRENCY]);
		SBGUIStandardScreen sbguistandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		if (sbguistandardScreen != null)
		{
			sbguistandardScreen.EnableUI(false);
			session.AddAsyncResponse("standard_screen", sbguistandardScreen);
		}
		Bounds totalBounds = sbguimicroConfirmDialog.TotalBounds;
		int num = (int)(totalBounds.size.x / 0.01f * GUIView.ResolutionScaleFactor());
		int num2 = (int)(totalBounds.size.y / 0.01f * GUIView.ResolutionScaleFactor());
		if (screenPosition.x - (float)(num / 2) < 0f)
		{
			screenPosition.x = (float)(num / 2);
		}
		if (screenPosition.x + (float)(num / 2) > (float)Screen.width)
		{
			screenPosition.x = (float)(Screen.width - num / 2);
		}
		if (screenPosition.y - (float)(num2 / 2) < 0f)
		{
			screenPosition.y = (float)(num2 / 2);
		}
		if (screenPosition.y + (float)(num2 / 2) > (float)Screen.height)
		{
			screenPosition.y = (float)(Screen.height - num2 / 2);
		}
		sbguimicroConfirmDialog.SetScreenPosition(screenPosition);
		SBUIBuilder.PushScreen(sbguimicroConfirmDialog);
		return sbguimicroConfirmDialog;
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0002E6CC File Offset: 0x0002C8CC
	public static SBGUICharacterDialog MakeAndAddDialogSequence(SBGUIScreen parent, Session session, List<object> sequence, Action<int> dialogChangeHandler)
	{
		SBGUICharacterDialog sbguicharacterDialog = (SBGUICharacterDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CharacterChat");
		sbguicharacterDialog.session = session;
		sbguicharacterDialog.SetParent(parent);
		sbguicharacterDialog.transform.localPosition = Vector3.zero;
		sbguicharacterDialog.LoadSequence(sequence);
		sbguicharacterDialog.DialogChange.AddListener(dialogChangeHandler);
		return sbguicharacterDialog;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0002E71C File Offset: 0x0002C91C
	public static SBGUIQuestDialog MakeAndAddQuestStartDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		SBGUIQuestDialog sbguiquestDialog = (SBGUIQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestStart");
		sbguiquestDialog.session = session;
		sbguiquestDialog.SetParent(parent);
		sbguiquestDialog.transform.localPosition = Vector3.zero;
		sbguiquestDialog.SetupQuestDialogInfo(title, icon);
		sbguiquestDialog.SetRewardIcons(session, rewards, string.Empty);
		return sbguiquestDialog;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0002E770 File Offset: 0x0002C970
	public static SBGUIAutoQuestStatusDialog MakeAndAddAutoQuestStartDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps, Action allDoneButton, Action makeButton)
	{
		SBGUIAutoQuestStatusDialog dialog = (SBGUIAutoQuestStatusDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AutoQuestStatus");
		dialog.session = session;
		dialog.SetParent(parent);
		dialog.transform.localPosition = Vector3.zero;
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		string stepPrefabName = "Prefabs/GUI/Widgets/AutoQuest_Step";
		if (allDoneButton != null)
		{
			dialog.AttachActionToButton("done", allDoneButton);
		}
		dialog.ReadyEvent += delegate()
		{
			dialog.CreateScrollRegionUI(screen, chunks, steps, makeButton, stepPrefabName);
		};
		dialog.SetupDialogInfo(dialogHeading, dialogBody, portrait, rewards, steps, questDef);
		return dialog;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0002E860 File Offset: 0x0002CA60
	public static SBGUIChunkQuestDialog MakeAndAddQuestChunkStartDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps, Action findButton)
	{
		SBGUIChunkQuestDialog dialog = (SBGUIChunkQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestStatusChunk");
		dialog.session = session;
		dialog.SetParent(parent);
		dialog.transform.localPosition = Vector3.zero;
		string name = Language.Get(questDef.Name);
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		string stepPrefabName = "Prefabs/GUI/Widgets/QuestStartChunk_Step";
		dialog.ReadyEvent += delegate()
		{
			dialog.CreateScrollRegionUI(screen, chunks, steps, findButton, stepPrefabName);
		};
		dialog.SetupChunkDialogInfo(dialogHeading, dialogBody, portrait, name, false, questDef);
		dialog.SetQuestLineInfo(questDef.QuestLine, null, session.TheGame.questManager.GetQuestLineProgress(questDef), true);
		dialog.SetRewardIcons(session, rewards, string.Empty);
		dialog.CenterRewards();
		return dialog;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0002E99C File Offset: 0x0002CB9C
	public static SBGUIQuestDialog MakeAndPushQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action okButton, Action findButton)
	{
		List<Reward> rewards = new List<Reward>
		{
			questDef.Reward
		};
		string key = Language.Get(questDef.Name);
		bool hasCountField = questDef.End.Chunks[0].Condition.hasCountField;
		string prefabName = "Prefabs/GUI/Screens/QuestStatus";
		string name = Language.Get(key);
		string icon = questDef.Icon;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		SBUIBuilder.MakeScreen make = () => (SBGUIQuestDialog)SBGUI.InstantiatePrefab(prefabName);
		bool flag;
		SBGUIQuestDialog sbguiquestDialog = (SBGUIQuestDialog)SBUIBuilder.OptionalCacheScreen(prefabName, make, out flag);
		if (flag)
		{
			sbguiquestDialog.session = session;
			sbguiquestDialog.SetParent(null);
			sbguiquestDialog.transform.localPosition = Vector3.zero;
			if (okButton != null)
			{
				sbguiquestDialog.AttachActionToButton("okay", okButton);
				sbguiquestDialog.AttachActionToButton("TouchableBackground", okButton);
			}
		}
		else
		{
			sbguiquestDialog.gameObject.SetActiveRecursively(true);
		}
		if (session.TheGame.resourceManager.PlayerLevelAmount < 5)
		{
			sbguiquestDialog.SetupQuestDialogInfo(name, icon, steps, hasCountField);
		}
		else
		{
			sbguiquestDialog.SetupQuestDialogInfo(name, icon, steps, hasCountField, chunks, findButton);
		}
		sbguiquestDialog.SetRewardIcons(session, rewards, string.Empty);
		SBUIBuilder.PushScreen(sbguiquestDialog);
		return sbguiquestDialog;
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0002EAEC File Offset: 0x0002CCEC
	public static SBGUIAutoQuestStatusDialog MakeAndPushAutoQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action okButton, Action allDoneButton, Action makeButton)
	{
		List<Reward> pRewards = new List<Reward>
		{
			questDef.Reward
		};
		string prefabName = "Prefabs/GUI/Screens/AutoQuestStatus";
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		SBUIBuilder.MakeScreen make = () => (SBGUIAutoQuestStatusDialog)SBGUI.InstantiatePrefab(prefabName);
		bool flag;
		SBGUIAutoQuestStatusDialog dialog = (SBGUIAutoQuestStatusDialog)SBUIBuilder.OptionalCacheScreen(prefabName, make, out flag);
		if (flag)
		{
			dialog.session = session;
			dialog.SetParent(null);
			dialog.transform.localPosition = Vector3.zero;
			if (okButton != null)
			{
				dialog.AttachActionToButton("okay", okButton);
				dialog.AttachActionToButton("TouchableBackground", okButton);
			}
			if (allDoneButton != null)
			{
				dialog.AttachActionToButton("done", allDoneButton);
			}
			dialog.ReadyEvent += delegate()
			{
				dialog.CreateScrollRegionUI(screen, chunks, steps, makeButton, null);
			};
		}
		else
		{
			dialog.gameObject.SetActiveRecursively(true);
			dialog.CreateScrollRegionUI(screen, chunks, steps, makeButton, null);
		}
		dialog.SetupDialogInfo(dialogHeading, dialogBody, portrait, pRewards, steps, questDef);
		SBUIBuilder.PushScreen(dialog);
		return dialog;
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0002EC94 File Offset: 0x0002CE94
	public static SBGUIChunkQuestDialog MakeAndPushChunkQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action findButton, Action okButton)
	{
		List<Reward> rewards = new List<Reward>
		{
			questDef.Reward
		};
		string name = Language.Get(questDef.Name);
		string prefabName = "Prefabs/GUI/Screens/QuestStatusChunk";
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		List<QuestBookendInfo.ChunkConditions> chunks = questDef.End.Chunks;
		SBUIBuilder.MakeScreen make = () => (SBGUIChunkQuestDialog)SBGUI.InstantiatePrefab(prefabName);
		bool flag;
		SBGUIChunkQuestDialog dialog = (SBGUIChunkQuestDialog)SBUIBuilder.OptionalCacheScreen(prefabName, make, out flag);
		if (flag)
		{
			dialog.session = session;
			dialog.SetParent(null);
			dialog.transform.localPosition = Vector3.zero;
			if (okButton != null)
			{
				dialog.AttachActionToButton("okay", okButton);
				dialog.AttachActionToButton("TouchableBackground", okButton);
			}
			dialog.ReadyEvent += delegate()
			{
				dialog.CreateScrollRegionUI(screen, chunks, steps, findButton, null);
			};
		}
		else
		{
			dialog.gameObject.SetActiveRecursively(true);
			dialog.CreateScrollRegionUI(screen, chunks, steps, findButton, null);
		}
		dialog.SetupChunkDialogInfo(dialogHeading, dialogBody, portrait, name, false, questDef);
		dialog.SetQuestLineInfo(questDef.QuestLine, null, session.TheGame.questManager.GetQuestLineProgress(questDef), true);
		dialog.SetRewardIcons(session, rewards, string.Empty);
		if (flag)
		{
			dialog.CenterRewards();
		}
		SBUIBuilder.PushScreen(dialog);
		return dialog;
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0002EE7C File Offset: 0x0002D07C
	public static SBGUIQuestDialog MakeAndAddQuestCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		SBGUIQuestDialog sbguiquestDialog = (SBGUIQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestComplete");
		sbguiquestDialog.session = session;
		sbguiquestDialog.SetParent(parent);
		sbguiquestDialog.transform.localPosition = Vector3.zero;
		sbguiquestDialog.SetupQuestDialogInfo(title, icon);
		sbguiquestDialog.SetRewardIcons(session, rewards, string.Empty);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sbguiquestDialog;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0002EED4 File Offset: 0x0002D0D4
	public static SBGUIAutoQuestCompleteDialog MakeAndAddAutoQuestCompleteDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef)
	{
		SBGUIAutoQuestCompleteDialog sbguiautoQuestCompleteDialog = (SBGUIAutoQuestCompleteDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AutoQuestCompleteScreen");
		sbguiautoQuestCompleteDialog.session = session;
		sbguiautoQuestCompleteDialog.SetParent(parent);
		sbguiautoQuestCompleteDialog.transform.localPosition = Vector3.zero;
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		sbguiautoQuestCompleteDialog.SetupDialogInfo(dialogHeading, dialogBody, portrait, rewards, questDef);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sbguiautoQuestCompleteDialog;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0002EF3C File Offset: 0x0002D13C
	public static SBGUIChunkQuestDialog MakeAndAddQuestChunkCompleteDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps)
	{
		SBGUIChunkQuestDialog sbguichunkQuestDialog = (SBGUIChunkQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestCompleteChunk");
		sbguichunkQuestDialog.session = session;
		sbguichunkQuestDialog.SetParent(parent);
		sbguichunkQuestDialog.transform.localPosition = Vector3.zero;
		string name = Language.Get(questDef.Name);
		string dialogHeading = questDef.DialogHeading;
		string dialogBody = questDef.DialogBody;
		string portrait = questDef.Portrait;
		sbguichunkQuestDialog.SetupChunkDialogInfo(dialogHeading, dialogBody, portrait, name, true, questDef);
		sbguichunkQuestDialog.SetQuestLineInfo(questDef.QuestLine, session.TheGame.questManager.GetQuestLineLastProgress(questDef), session.TheGame.questManager.GetQuestLineProgress(questDef), false);
		sbguichunkQuestDialog.SetRewardIcons(session, rewards, string.Empty);
		sbguichunkQuestDialog.CenterRewards();
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sbguichunkQuestDialog;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002EFF8 File Offset: 0x0002D1F8
	public static SBGUIQuestDialog MakeAndAddBootyQuestCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		SBGUIQuestDialog sbguiquestDialog = (SBGUIQuestDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/BootyQuestComplete");
		sbguiquestDialog.session = session;
		sbguiquestDialog.SetParent(parent);
		sbguiquestDialog.transform.localPosition = Vector3.zero;
		sbguiquestDialog.SetupQuestDialogInfo(title, icon);
		sbguiquestDialog.SetRewardIcons(session, rewards, string.Empty);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sbguiquestDialog;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x0002F050 File Offset: 0x0002D250
	public static SBGUIQuestLineDialog MakeAndAddQuestLineStartDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		SBGUIQuestLineDialog sbguiquestLineDialog = (SBGUIQuestLineDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestLineStart");
		sbguiquestLineDialog.session = session;
		sbguiquestLineDialog.SetParent(parent);
		sbguiquestLineDialog.transform.localPosition = Vector3.zero;
		rewardName = string.Empty;
		sbguiquestLineDialog.SetupQuestLineDialogInfo(dialogHeading, dialogBody, portrait, rewardTexture, rewardName);
		if (rewards.Count > 0)
		{
			sbguiquestLineDialog.SetRewardIcons(session, rewards, string.Empty);
			sbguiquestLineDialog.CenterRewards();
		}
		else
		{
			sbguiquestLineDialog.ToggleRewardWindow(false);
		}
		TFUtils.DebugLog("SBUIBuilder, Made Quest Line Start Dialog " + sbguiquestLineDialog.name);
		return sbguiquestLineDialog;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0002F0E4 File Offset: 0x0002D2E4
	public static SBGUIQuestLineDialog MakeAndAddQuestLineCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		SBGUIQuestLineDialog sbguiquestLineDialog = (SBGUIQuestLineDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/QuestLineComplete");
		sbguiquestLineDialog.session = session;
		sbguiquestLineDialog.SetParent(parent);
		sbguiquestLineDialog.transform.localPosition = Vector3.zero;
		sbguiquestLineDialog.SetupQuestLineDialogInfo(dialogHeading, dialogBody, portrait, rewardTexture, rewardName);
		if (rewards.Count > 0)
		{
			sbguiquestLineDialog.SetRewardIcons(session, rewards, string.Empty);
			sbguiquestLineDialog.CenterRewards();
		}
		else
		{
			sbguiquestLineDialog.ToggleRewardWindow(false);
		}
		TFUtils.DebugLog("SBUIBuilder, Made Quest Line Complete Dialog " + sbguiquestLineDialog.name);
		return sbguiquestLineDialog;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0002F170 File Offset: 0x0002D370
	public static SBGUICharacterBusyScreen MakeAndPushUnitBusyUI(SBGUIStandardScreen screen, Session session, Simulated pSimulated, Task pTask, Action pFeedWishAction, Action pRushWishAction, Action pRushTaskAction, Action closeButton)
	{
		string prefabName = "Prefabs/GUI/Screens/CharacterBusyScreen";
		SBUIBuilder.MakeScreen make = () => (SBGUICharacterBusyScreen)SBGUI.InstantiatePrefab(prefabName);
		bool flag;
		SBGUICharacterBusyScreen sbguicharacterBusyScreen = (SBGUICharacterBusyScreen)SBUIBuilder.OptionalCacheScreen(prefabName, make, out flag);
		AndroidBack.getInstance().push(closeButton, sbguicharacterBusyScreen);
		if (flag)
		{
			sbguicharacterBusyScreen.session = session;
			sbguicharacterBusyScreen.SetParent(null);
			sbguicharacterBusyScreen.transform.localPosition = Vector3.zero;
			if (closeButton != null)
			{
				sbguicharacterBusyScreen.AttachActionToButton("close", closeButton);
				sbguicharacterBusyScreen.AttachActionToButton("TouchableBackground", closeButton);
			}
		}
		else
		{
			sbguicharacterBusyScreen.gameObject.SetActiveRecursively(true);
		}
		sbguicharacterBusyScreen.SetupDialogInfo(pSimulated, pTask, pFeedWishAction, pRushWishAction, pRushTaskAction);
		SBUIBuilder.PushScreen(sbguicharacterBusyScreen);
		return sbguicharacterBusyScreen;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0002F22C File Offset: 0x0002D42C
	public static SBGUICharacterIdleScreen MakeAndPushUnitIdleUI(SBGUIStandardScreen screen, Session session, Simulated pSimulated, List<TaskData> pTaskDatas, Action pFeedWishAction, Action pRushWishAction, Action<int> pDoTaskAction, Action closeButton)
	{
		string prefabName = "Prefabs/GUI/Screens/CharacterIdleScreen";
		SBUIBuilder.MakeScreen make = () => (SBGUICharacterIdleScreen)SBGUI.InstantiatePrefab(prefabName);
		bool flag;
		SBGUICharacterIdleScreen dialog = (SBGUICharacterIdleScreen)SBUIBuilder.OptionalCacheScreen(prefabName, make, out flag);
		AndroidBack.getInstance().push(closeButton, dialog);
		if (flag)
		{
			dialog.session = session;
			dialog.SetParent(null);
			dialog.transform.localPosition = Vector3.zero;
			if (closeButton != null)
			{
				dialog.AttachActionToButton("close", closeButton);
				dialog.AttachActionToButton("TouchableBackground", closeButton);
			}
			dialog.SetupDialogInfo(pSimulated, pFeedWishAction, pRushWishAction, pDoTaskAction);
			dialog.ReadyEvent += delegate()
			{
				dialog.CreateScrollRegionUI(pTaskDatas);
			};
		}
		else
		{
			SBUIBuilder.ReleaseTopScreen();
			dialog.gameObject.SetActiveRecursively(true);
			dialog.SetupDialogInfo(pSimulated, pFeedWishAction, pRushWishAction, pDoTaskAction);
			dialog.CreateScrollRegionUI(pTaskDatas);
		}
		SBUIBuilder.PushScreen(dialog);
		return dialog;
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x0002F364 File Offset: 0x0002D564
	public static SBGUIProgressDialog MakeAndAddProgressDialog(SBGUIScreen parent, Session session, string title, string description, Cost rush_cost, Action onRush, Action onClose)
	{
		SBGUIProgressDialog sbguiprogressDialog = (SBGUIProgressDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/BuildingProgress");
		sbguiprogressDialog.session = session;
		sbguiprogressDialog.SetParent(parent);
		sbguiprogressDialog.transform.localPosition = Vector3.zero;
		if (rush_cost == null || rush_cost.ResourceAmounts[rush_cost.GetOnlyCostKey()] <= 0)
		{
			sbguiprogressDialog.Setup(title, description, onClose);
		}
		else
		{
			sbguiprogressDialog.Setup(title, description, onClose, false, rush_cost, onRush);
		}
		return sbguiprogressDialog;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x0002F3E0 File Offset: 0x0002D5E0
	public static SBGUITimebar MakeAndAddTimebar(Session session, SBGUIScreen parent, uint ownerDid, string description, ulong completeTime, ulong totalTime, float duration, Cost rushCost, Action onRush, SBGUITimebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		if (SBUIBuilder.sTimebarPool == null)
		{
			SBUIBuilder.sTimebarPool = TFPool<SBGUITimebar>.CreatePool(10, delegate
			{
				SBGUITimebar sbguitimebar2 = (SBGUITimebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Timebar");
				sbguitimebar2.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
				sbguitimebar2.gameObject.SetActiveRecursively(false);
				return sbguitimebar2;
			});
		}
		SBGUITimebar sbguitimebar = SBUIBuilder.sTimebarPool.Create(() => (SBGUITimebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Timebar"));
		sbguitimebar.gameObject.SetActiveRecursively(true);
		sbguitimebar.Setup(session, ownerDid, description, completeTime, totalTime, duration, rushCost, onRush, hPosition, onFinish, pTaskCharacterDIDs, pTaskCharacterClicked);
		sbguitimebar.SetParent(parent, false);
		Vector3 localPosition = hPosition();
		sbguitimebar.transform.localPosition = localPosition;
		return sbguitimebar;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0002F48C File Offset: 0x0002D68C
	public static void ReleaseTimebar(SBGUITimebar timebar)
	{
		timebar.MuteButtons(false);
		timebar.SetParent(null);
		timebar.gameObject.SetActiveRecursively(false);
		if (SBUIBuilder.sTimebarPool != null)
		{
			SBUIBuilder.sTimebarPool.Release(timebar);
		}
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0002F4CC File Offset: 0x0002D6CC
	public static void ReleaseTimebars()
	{
		if (SBUIBuilder.sTimebarPool != null)
		{
			Deactivate<SBGUITimebar> deactivateDelegate = delegate(SBGUITimebar timebar)
			{
				timebar.MuteButtons(false);
				timebar.SetParent(null);
				timebar.gameObject.SetActiveRecursively(false);
			};
			SBUIBuilder.sTimebarPool.Clear(deactivateDelegate);
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002F50C File Offset: 0x0002D70C
	public static SBGUINamebar MakeAndAddNamebar(Session session, SBGUIScreen parent, string name, SBGUINamebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		if (SBUIBuilder.sNamebarPool == null)
		{
			SBUIBuilder.sNamebarPool = TFPool<SBGUINamebar>.CreatePool(10, delegate
			{
				SBGUINamebar sbguinamebar2 = (SBGUINamebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Namebar");
				sbguinamebar2.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
				sbguinamebar2.gameObject.SetActiveRecursively(false);
				return sbguinamebar2;
			});
		}
		SBGUINamebar sbguinamebar = SBUIBuilder.sNamebarPool.Create(() => (SBGUINamebar)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/Namebar"));
		sbguinamebar.gameObject.SetActiveRecursively(true);
		sbguinamebar.Setup(session, name, hPosition, onFinish, pTaskCharacterDIDs, pTaskCharacterClicked);
		sbguinamebar.SetParent(parent, false);
		Vector3 v = hPosition();
		sbguinamebar.SetScreenPosition(v);
		return sbguinamebar;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0002F5AC File Offset: 0x0002D7AC
	public static void ReleaseNamebar(SBGUINamebar namebar)
	{
		namebar.MuteButtons(false);
		namebar.SetParent(null);
		namebar.gameObject.SetActiveRecursively(false);
		if (SBUIBuilder.sNamebarPool != null)
		{
			SBUIBuilder.sNamebarPool.Release(namebar);
		}
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0002F5EC File Offset: 0x0002D7EC
	public static void ReleaseNamebars()
	{
		if (SBUIBuilder.sNamebarPool != null)
		{
			Deactivate<SBGUINamebar> deactivateDelegate = delegate(SBGUINamebar namebar)
			{
				namebar.MuteButtons(false);
				namebar.SetParent(null);
				namebar.gameObject.SetActiveRecursively(false);
			};
			SBUIBuilder.sNamebarPool.Clear(deactivateDelegate);
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0002F62C File Offset: 0x0002D82C
	public static SBGUIElement MakeAndAddInteractionStrip(Session session, uint ownerDid, SBGUIScreen parent, ICollection<IControlBinding> controls)
	{
		List<SBUIBuilder.InteractionStripButtonInfo> list = new List<SBUIBuilder.InteractionStripButtonInfo>();
		foreach (IControlBinding controlBinding in controls)
		{
			TFUtils.Assert(SBUIBuilder.controlToTextureMap.ContainsKey(controlBinding.GetType()), "No texture has been declared for binding type " + controlBinding.GetType() + ". Add it to the control-to-texture map.");
			SBUIBuilder.InteractionStripButtonInfo item = new SBUIBuilder.InteractionStripButtonInfo(SBUIBuilder.controlToTextureMap[controlBinding.GetType()], controlBinding);
			list.Add(item);
		}
		SBGUIElement sbguielement = SBUIBuilder.MakeGenericInteractionStrip(session, ownerDid, list);
		sbguielement.SetVisible(true);
		YGAtlasSprite component = sbguielement.GetComponent<YGAtlasSprite>();
		if (controls.Count < 2)
		{
			sbguielement.SetVisible(false);
		}
		else
		{
			component.SetSize(new Vector2(102f * (float)controls.Count, 112f));
		}
		sbguielement.SetParent(parent);
		sbguielement.transform.localPosition = new Vector3(0f, 0f, -1f);
		return sbguielement;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002F750 File Offset: 0x0002D950
	private static SBGUIElement MakeGenericInteractionStrip(Session session, uint ownerDid, List<SBUIBuilder.InteractionStripButtonInfo> buttonInfos)
	{
		if (SBUIBuilder.sInteractionStrip == null)
		{
			SBUIBuilder.sInteractionStrip = SBUIBuilder.CreateInteractionStripCache();
		}
		SBGUIElement sbguielement = SBUIBuilder.sInteractionStrip;
		sbguielement.gameObject.SetActiveRecursively(true);
		SBGUIButton sbguibutton = (SBGUIButton)sbguielement.FindChild("ButtonStandinBig");
		sbguibutton.gameObject.SetActiveRecursively(false);
		sbguibutton.SetActive(false);
		float num = 0.01f;
		float num2 = (float)sbguibutton.Width * 0.01f + num;
		float num3 = (float)sbguibutton.Width * 0.01f + num2 * (float)(buttonInfos.Count - 2) + num;
		num3 *= -0.5f;
		float num4 = -0.65f;
		float z = -1f;
		foreach (SBUIBuilder.InteractionStripButtonInfo interactionStripButtonInfo in buttonInfos)
		{
			SBGUIButton sbguibutton2 = SBUIBuilder.sInteractionStripButtons[interactionStripButtonInfo.textureToUse];
			sbguibutton2.gameObject.SetActiveRecursively(true);
			sbguibutton2.SessionActionId = interactionStripButtonInfo.control.DecorateSessionActionId(ownerDid);
			sbguibutton2.SetParent(sbguielement);
			sbguibutton2.ClickEvent += SBUIBuilder.InteractionStripButtonHandlerClosure(session, interactionStripButtonInfo.control.Action);
			sbguibutton2.transform.localPosition = new Vector3(num3, num4, z);
			if (buttonInfos.Count == 1)
			{
				sbguibutton2.transform.localPosition = new Vector3(num3, num4 + 0.3f, z);
			}
			num3 += num2;
			sbguibutton2.ResetSize();
			interactionStripButtonInfo.control.DynamicButton = sbguibutton2;
			SBGUILabel sbguilabel = (SBGUILabel)sbguibutton2.FindChild("label");
			sbguilabel.SetActive(false);
			if (interactionStripButtonInfo.control.Label != null)
			{
				sbguilabel.SetText(interactionStripButtonInfo.control.Label);
				sbguilabel.SetActive(true);
			}
		}
		return sbguielement;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x0002F944 File Offset: 0x0002DB44
	private static Action InteractionStripButtonHandlerClosure(Session session, Action<Session> action)
	{
		return delegate()
		{
			action(session);
		};
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002F974 File Offset: 0x0002DB74
	public static void UpdateAcceptPlacementButton(SBGUIButton button, Session session)
	{
		bool enabled = Session.TheDebugManager.debugPlaceObjects || session.TheGame.simulation.PlacementQuery(session.TheGame.selected, false) != Simulation.Placement.RESULT.INVALID;
		SBUIBuilder.UpdateButton(button, enabled);
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002F9C0 File Offset: 0x0002DBC0
	public static void UpdateButton(SBGUIButton button, bool enabled)
	{
		YGAtlasSprite component = button.GetComponent<YGAtlasSprite>();
		if (enabled)
		{
			component.SetAlpha(1f);
		}
		else
		{
			component.SetAlpha(0.25f);
		}
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0002F9F8 File Offset: 0x0002DBF8
	private static void SwapButtonTexture(SBGUIElement parent, string buttonName, string textureToUse)
	{
		SBGUIButton sbguibutton = parent.FindChild(buttonName) as SBGUIButton;
		TFUtils.Assert(sbguibutton != null, string.Format("Couldn't find button {0}", buttonName));
		YGAtlasSprite component = sbguibutton.GetComponent<YGAtlasSprite>();
		component.sprite = component.LoadSpriteFromAtlas(textureToUse, component.atlasIndex);
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x0002FA44 File Offset: 0x0002DC44
	public static void MakeActivityIndicator(SBGUIScreen parent)
	{
		SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/ActivityIndicator");
		sbguiactivityIndicator.SetParent(parent);
		sbguiactivityIndicator.transform.localPosition = new Vector3(0f, 0f, 24f);
		sbguiactivityIndicator.InitActivityIndicator();
		sbguiactivityIndicator.renderer.enabled = false;
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0002FA9C File Offset: 0x0002DC9C
	public static SBGUIMarketplaceScreen MakeAndPushMarketplaceDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action closeClickHandler, Action<SBMarketOffer> purchaseClickHandler, EntityManager entityMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Catalog catalog)
	{
		bool flag;
		SBGUIMarketplaceScreen sbguimarketplaceScreen = (SBGUIMarketplaceScreen)SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/MarketplaceScreen", () => (SBGUIScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/MarketplaceScreen"), out flag);
		AndroidBack.getInstance().push(closeClickHandler, sbguimarketplaceScreen);
		if (flag)
		{
			sbguimarketplaceScreen.AttachActionToButton("close", delegate()
			{
				closeClickHandler();
			});
			sbguimarketplaceScreen.OfferClickedEvent.AddListener(purchaseClickHandler);
			sbguimarketplaceScreen.LocalizeInitialLabel();
			sbguimarketplaceScreen.SetManagers(session);
			sbguimarketplaceScreen.session = session;
			SBUIBuilder.MakeActivityIndicator(sbguimarketplaceScreen);
		}
		else
		{
			sbguimarketplaceScreen.gameObject.SetActiveRecursively(true);
			SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)sbguimarketplaceScreen.FindChild("info_window");
			sbguiatlasImage.SetActive(false);
			sbguimarketplaceScreen.ClearButtonActions("close");
			sbguimarketplaceScreen.AttachActionToButton("close", delegate()
			{
				closeClickHandler();
			});
			sbguimarketplaceScreen.ClearButtonActions("TouchableBackground");
			sbguimarketplaceScreen.AttachActionToButton("TouchableBackground", closeClickHandler);
			sbguimarketplaceScreen.OfferClickedEvent.ClearListeners();
			sbguimarketplaceScreen.OfferClickedEvent.AddListener(purchaseClickHandler);
			sbguimarketplaceScreen.SetupTabCategories();
		}
		SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		sbguimarketplaceScreen.ViewCurrentTab();
		SBUIBuilder.PushScreen(sbguimarketplaceScreen);
		return sbguimarketplaceScreen;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x0002FBDC File Offset: 0x0002DDDC
	public static SBGUICraftingScreen MakeAndPushCraftingUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action closeClickHandler, Action<SBGUICraftingScreen, CraftingRecipe> craftRecipeHandler, Action<int> rushCraftHandler, Action<CraftingRecipe> setSelected, CraftingCookbook cookbook, CraftingRecipe highlightedRecipe, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked, int effectiveSlotCount, int maxSlotCount)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUICraftingScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CraftingScreen");
		bool flag;
		SBGUICraftingScreen crafting = (SBGUICraftingScreen)SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/CraftingScreen", make, out flag);
		AndroidBack.getInstance().push(closeClickHandler, crafting);
		if (flag)
		{
			crafting.Setup(session, cookbook, rushCraftHandler, maxSlotCount);
			crafting.AttachActionToButton("close", closeClickHandler);
			crafting.AttachActionToButton("TouchableBackground", closeClickHandler);
			SBUIBuilder.CraftingScreenGraphicalSetup(crafting, cookbook);
			crafting.ReadyEvent += delegate()
			{
				crafting.CreateUI(cookbook, highlightedRecipe, effectiveSlotCount, maxSlotCount, setSelected);
				session.TheGame.sessionActionManager.RequestProcess(session.TheGame);
			};
		}
		else
		{
			SBUIBuilder.CraftingScreenGraphicalSetup(crafting, cookbook);
			crafting.gameObject.SetActiveRecursively(true);
			crafting.CreateUI(cookbook, highlightedRecipe, effectiveSlotCount, maxSlotCount, setSelected);
			crafting.ShowScrollRegion(true);
		}
		crafting.CreateNonScrollUI(pTaskCharacterDIDs, pTaskCharacterClicked);
		crafting.ClearButtonActions("accept_button");
		Action action = delegate()
		{
			if (crafting.selectedSlot != null)
			{
				craftRecipeHandler(crafting, crafting.selectedSlot.recipe);
			}
			crafting.UpdateResources(session);
		};
		crafting.AttachActionToButton("accept_button", action);
		crafting.MakeRecipeClickedEvent.ClearListeners();
		Action<CraftingRecipe> value = delegate(CraftingRecipe recipe)
		{
			craftRecipeHandler(crafting, recipe);
		};
		crafting.MakeRecipeClickedEvent.AddListener(value);
		SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		SBUIBuilder.PushScreen(crafting);
		return crafting;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x0002FDF0 File Offset: 0x0002DFF0
	public static SBGUIVendorScreen MakeAndPushVendorUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action backHandler, Action<VendingInstance> vendorInstanceHandler, Action rushHandler, VendorDefinition vendorDef, Dictionary<int, VendingInstance> vendingInstances, VendingInstance specialVendingInstance, VendingDecorator vendingEntity, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUIVendorScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/VendorScreen");
		bool flag;
		SBGUIVendorScreen vendorScreen = (SBGUIVendorScreen)SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/VendorScreen", make, out flag);
		AndroidBack.getInstance().push(backHandler, vendorScreen);
		vendorScreen.Setup(session, vendorDef);
		if (flag)
		{
			vendorScreen.AttachActionToButton("close", backHandler);
			vendorScreen.AttachActionToButton("TouchableBackground", backHandler);
			vendorScreen.AttachActionToButton("skip_button", rushHandler);
			vendorScreen.UpdateVendingInstanceSlots(session);
		}
		else
		{
			vendorScreen.gameObject.SetActiveRecursively(true);
			vendorScreen.UpdateVendingInstanceSlots(session);
		}
		vendorScreen.CreateNonScrollUI(pTaskCharacterDIDs, pTaskCharacterClicked);
		Action action = delegate()
		{
			if (vendorScreen.lastSelectedSlot != null)
			{
				VendingInstance vendingInstance = session.TheGame.vendingManager.GetVendingInstance(vendingEntity.Id, vendorScreen.lastSelectedSlot.SlotID);
				if (vendingInstance == null)
				{
					vendingInstance = session.TheGame.vendingManager.GetSpecialInstance(vendingEntity.Id);
				}
				vendorInstanceHandler(vendingInstance);
			}
		};
		vendorScreen.ClearButtonActions("buy_button");
		vendorScreen.AttachActionToButton("buy_button", action);
		SBUIBuilder.UpdateGuiEventHandler(session, guiEventHandler);
		SBUIBuilder.PushScreen(vendorScreen);
		return vendorScreen;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0002FF5C File Offset: 0x0002E15C
	private static void CraftingScreenGraphicalSetup(SBGUICraftingScreen crafting, CraftingCookbook cookbook)
	{
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)crafting.FindChild("window");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)crafting.FindChild("_inset");
		List<int> backgroundColor = cookbook.backgroundColor;
		Color color;
		if (cookbook.backgroundColor != null)
		{
			color = new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f);
		}
		else
		{
			TFUtils.WarningLog("Cookbook " + cookbook.identity + " does not have a background.color defined");
			color = new Color(1f, 1f, 1f);
		}
		sbguiatlasImage.SetColor(color);
		sbguiatlasImage2.SetColor(color);
		SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)crafting.FindChild("_title");
		if (sbguiatlasImage3)
		{
			sbguiatlasImage3.SetTextureFromAtlas(cookbook.titleTexture);
			sbguiatlasImage3.ResetSize();
		}
		SBGUIAtlasImage sbguiatlasImage4 = (SBGUIAtlasImage)crafting.FindChild("left_title_icon");
		if (sbguiatlasImage4)
		{
			sbguiatlasImage4.SetTextureFromAtlas(cookbook.titleIconTexture);
		}
		SBGUIAtlasImage sbguiatlasImage5 = (SBGUIAtlasImage)crafting.FindChild("right_title_icon");
		if (sbguiatlasImage5)
		{
			sbguiatlasImage5.SetTextureFromAtlas(cookbook.titleIconTexture);
		}
		SBGUIAtlasButton sbguiatlasButton = (SBGUIAtlasButton)crafting.FindChild("close");
		if (sbguiatlasButton)
		{
			sbguiatlasButton.SetTextureFromAtlas(cookbook.cancelButtonTexture);
		}
		SBGUILabel sbguilabel = (SBGUILabel)crafting.FindChild("button_label");
		if (sbguilabel)
		{
			sbguilabel.SetText(cookbook.buttonLabel);
		}
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x000300F8 File Offset: 0x0002E2F8
	public static SBGUICreditsScreen MakeAndPushCreditsUI(Session session, Action closeClickHandler)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUICreditsScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CreditsScreen");
		bool flag;
		SBGUICreditsScreen credits = (SBGUICreditsScreen)SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/CreditsScreen", make, out flag);
		AndroidBack.getInstance().push(closeClickHandler, credits);
		if (flag)
		{
			credits.Setup(session);
			credits.AttachActionToButton("close", closeClickHandler);
			credits.AttachActionToButton("TouchableBackground", closeClickHandler);
			credits.ReadyEvent += delegate()
			{
				credits.CreateUI();
			};
		}
		else
		{
			credits.gameObject.SetActiveRecursively(true);
			credits.CreateUI();
		}
		SBUIBuilder.PushScreen(credits);
		return credits;
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x000301D4 File Offset: 0x0002E3D4
	public static SBGUIDebugScreen MakeAndParentDebugUI(Session session, SBGUIScreen parent, Action closeClickHandler, Action toggleFramerateCounter, Action toggleFreeEditMode, Action saveFreeEditProgress, Action toggleHitBoxes, Action toggleFootprints, Action toggleExpansionBorders, Action addMoney, Action addJJ, Action addSpecialCurrency, Action addFoods, Action toggleRMT, Action deleteServerGame, Action resetEventItems, Action toggleFreeCameraMode, Action completeAllQuests, Action levelUp, Action logDump, Action unlockDecos, Action addHourSimulation, Action incrementDailyBonus, Action fastFoward, Action addOneLevel, Action reset_device_id)
	{
		SBGUIDebugScreen sbguidebugScreen = (SBGUIDebugScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/DebugScreen");
		AndroidBack.getInstance().push(closeClickHandler, sbguidebugScreen);
		sbguidebugScreen.SetParent(parent);
		sbguidebugScreen.transform.localPosition = Vector3.zero;
		sbguidebugScreen.AttachActionToButton("close", closeClickHandler);
		sbguidebugScreen.AttachActionToButton("TouchableBackground", closeClickHandler);
		sbguidebugScreen.AttachActionToButton("framerate_counter", toggleFramerateCounter);
		sbguidebugScreen.AttachActionToButton("free_edit_button", toggleFreeEditMode);
		sbguidebugScreen.AttachActionToButton("save_free_edit_button", saveFreeEditProgress);
		sbguidebugScreen.AttachActionToButton("toggle_hit_boxes_button", toggleHitBoxes);
		sbguidebugScreen.AttachActionToButton("toggle_footprints_button", toggleFootprints);
		sbguidebugScreen.AttachActionToButton("toggle_expansion_borders_button", toggleExpansionBorders);
		sbguidebugScreen.AttachActionToButton("add_money_button", addMoney);
		sbguidebugScreen.AttachActionToButton("add_JJ_button", addJJ);
		sbguidebugScreen.AttachActionToButton("add_special_currency_button", addSpecialCurrency);
		sbguidebugScreen.AttachActionToButton("add_foods_button", addFoods);
		sbguidebugScreen.AttachActionToButton("toggle_rmt_button", toggleRMT);
		sbguidebugScreen.AttachActionToButton("delete_server_game_button", deleteServerGame);
		sbguidebugScreen.AttachActionToButton("reset_community_event_items_button", resetEventItems);
		sbguidebugScreen.AttachActionToButton("toggle_free_camera", toggleFreeCameraMode);
		sbguidebugScreen.AttachActionToButton("complete_all_quests", completeAllQuests);
		sbguidebugScreen.AttachActionToButton("level_up", levelUp);
		sbguidebugScreen.AttachActionToButton("log_dump", logDump);
		sbguidebugScreen.AttachActionToButton("unlock_decos", unlockDecos);
		sbguidebugScreen.AttachActionToButton("add_hour_sim_button", addHourSimulation);
		sbguidebugScreen.AttachActionToButton("increment_daily_bonus", incrementDailyBonus);
		sbguidebugScreen.AttachActionToButton("fast_forward", fastFoward);
		sbguidebugScreen.AttachActionToButton("add_level_button", addOneLevel);
		sbguidebugScreen.AttachActionToButton("reset_device_id", reset_device_id);
		sbguidebugScreen.Setup(session);
		return sbguidebugScreen;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00030378 File Offset: 0x0002E578
	public static SBGUIClearingScreen MakeAndPushClearingUI(string cost, Action okButtonHandler, Action cancelButtonHandler)
	{
		SBGUIClearingScreen sbguiclearingScreen = (SBGUIClearingScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ClearingScreen");
		sbguiclearingScreen.AttachActionToButton("cancel_button", cancelButtonHandler);
		sbguiclearingScreen.AttachActionToButton("okay_button", okButtonHandler);
		sbguiclearingScreen.AttachActionToButton("TouchableBackground", cancelButtonHandler);
		sbguiclearingScreen.SetUp("Clear Debris?", "Removing this item costs:", cost);
		SBUIBuilder.PushScreen(sbguiclearingScreen);
		return sbguiclearingScreen;
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x000303D4 File Offset: 0x0002E5D4
	public static SBGUIInventoryScreen MakeAndPushInventoryDialog(Session session, EntityManager entityMgr, SoundEffectManager sfxMgr, Action closeClickHandler, Action<SBInventoryItem> buildingClickHandler, Action<SBInventoryItem> inventoryClickHandler)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUIInventoryScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/InventoryScreen");
		bool flag;
		SBGUIInventoryScreen sbguiinventoryScreen = (SBGUIInventoryScreen)SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/InventoryScreen", make, out flag);
		AndroidBack.getInstance().push(closeClickHandler, sbguiinventoryScreen);
		if (flag)
		{
			sbguiinventoryScreen.AttachActionToButton("close", closeClickHandler);
			sbguiinventoryScreen.AttachActionToButton("TouchableBackground", closeClickHandler);
			sbguiinventoryScreen.BuildingSlotClickedEvent.AddListener(buildingClickHandler);
			sbguiinventoryScreen.MovieSlotClickedEvent.AddListener(inventoryClickHandler);
			sbguiinventoryScreen.SetManagers(session);
			SBUIBuilder.MakeActivityIndicator(sbguiinventoryScreen);
		}
		else
		{
			sbguiinventoryScreen.gameObject.SetActiveRecursively(true);
		}
		sbguiinventoryScreen.ViewCurrentTab();
		SBUIBuilder.PushScreen(sbguiinventoryScreen);
		return sbguiinventoryScreen;
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00030484 File Offset: 0x0002E684
	public static SBGUICommunityEventScreen MakeAndPushCommunityEventDialog(Session session, Action closeClickHandler, Action<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> purchaseHandler)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUICommunityEventScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/CommunityEventScreen");
		bool flag;
		SBGUICommunityEventScreen sbguicommunityEventScreen = (SBGUICommunityEventScreen)SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/CommunityEventScreen", make, out flag);
		AndroidBack.getInstance().push(closeClickHandler, sbguicommunityEventScreen);
		if (flag)
		{
			sbguicommunityEventScreen.AttachActionToButton("close", closeClickHandler);
			sbguicommunityEventScreen.BuyButtonClickedEvent.AddListener(purchaseHandler);
			sbguicommunityEventScreen.SetManagers(session);
			sbguicommunityEventScreen.SetupButtons();
		}
		else
		{
			sbguicommunityEventScreen.gameObject.SetActiveRecursively(true);
			sbguicommunityEventScreen.BuyButtonClickedEvent.ClearListeners();
			sbguicommunityEventScreen.BuyButtonClickedEvent.AddListener(purchaseHandler);
		}
		sbguicommunityEventScreen.ViewCurrentTab();
		SBUIBuilder.PushScreen(sbguicommunityEventScreen);
		return sbguicommunityEventScreen;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x00030530 File Offset: 0x0002E730
	public static SBGUILevelUpScreen MakeAndAddLevelUpDialog(SBGUIScreen parent, Session session, LevelUpDialogInputData inputData)
	{
		SBGUILevelUpScreen sbguilevelUpScreen = (SBGUILevelUpScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/LevelUpScreen");
		session.AddAsyncResponse("levelup_screen", sbguilevelUpScreen);
		sbguilevelUpScreen.SetParent(parent);
		sbguilevelUpScreen.transform.localPosition = Vector3.zero;
		sbguilevelUpScreen.SetManagers(session.TheGame.entities, session.TheGame.resourceManager, session.TheSoundEffectManager);
		sbguilevelUpScreen.Setup(session, inputData);
		sbguilevelUpScreen.session = session;
		sbguilevelUpScreen.CreateUI(session, inputData);
		session.PlaySeaflowerAndBubbleScreenSwipeEffect();
		return sbguilevelUpScreen;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x000305B0 File Offset: 0x0002E7B0
	public static SBGUIFoundItemScreen MakeAndAddFoundItemScreen(Session session, SBGUIScreen parent)
	{
		SBGUIFoundItemScreen sbguifoundItemScreen = (SBGUIFoundItemScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/FoundItemScreen");
		sbguifoundItemScreen.SetParent(parent);
		sbguifoundItemScreen.transform.localPosition = Vector3.zero;
		session.PlayBubbleScreenSwipeEffect();
		return sbguifoundItemScreen;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x000305EC File Offset: 0x0002E7EC
	public static SBGUIExplanationDialog MakeAndAddExplanationDialog(SBGUIScreen parent)
	{
		SBGUIExplanationDialog sbguiexplanationDialog = (SBGUIExplanationDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/ExplanationDialog");
		sbguiexplanationDialog.SetParent(parent);
		sbguiexplanationDialog.transform.localPosition = Vector3.zero;
		return sbguiexplanationDialog;
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x00030624 File Offset: 0x0002E824
	public static SBGUIMoveInDialog MakeAndAddMoveInDialog(SBGUIScreen parent)
	{
		SBGUIMoveInDialog sbguimoveInDialog = (SBGUIMoveInDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/MoveInDialog");
		sbguimoveInDialog.SetParent(parent);
		sbguimoveInDialog.transform.localPosition = Vector3.zero;
		return sbguimoveInDialog;
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0003065C File Offset: 0x0002E85C
	public static SBGUIDailyBonusDialog MakeAndAddDailyBonusDialog(SBGUIScreen parent)
	{
		SBGUIDailyBonusDialog sbguidailyBonusDialog = (SBGUIDailyBonusDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/DailyBonusDialog");
		sbguidailyBonusDialog.SetParent(parent);
		sbguidailyBonusDialog.transform.localPosition = Vector3.zero;
		return sbguidailyBonusDialog;
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x00030694 File Offset: 0x0002E894
	public static SBGUISpongyGamesDialog MakeAndAddSpongyGamesDialog(SBGUIScreen parent)
	{
		SBGUISpongyGamesDialog sbguispongyGamesDialog = (SBGUISpongyGamesDialog)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/SpongyGamesDialog");
		sbguispongyGamesDialog.SetParent(parent);
		sbguispongyGamesDialog.transform.localPosition = Vector3.zero;
		return sbguispongyGamesDialog;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x000306CC File Offset: 0x0002E8CC
	public static SBGUIScreen MakeAndPushAgeGateDialog(Action backHandler, Action submitHandler, Action cancelHandler, Action inputHandler)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUIScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/AgeGateScreen");
		bool flag;
		SBGUIScreen sbguiscreen = SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/AgeGateScreen", make, out flag);
		AndroidBack.getInstance().push(backHandler, sbguiscreen);
		if (flag)
		{
			sbguiscreen.AttachActionToButton("close", backHandler);
			sbguiscreen.AttachActionToButton("submit", submitHandler);
			sbguiscreen.AttachActionToButton("cancel", cancelHandler);
			sbguiscreen.AttachActionToButton("input_box", inputHandler);
			sbguiscreen.AttachActionToButton("TouchableBackground", backHandler);
		}
		else
		{
			sbguiscreen.gameObject.SetActiveRecursively(true);
		}
		SBUIBuilder.PushScreen(sbguiscreen);
		return sbguiscreen;
	}

	// Token: 0x06000745 RID: 1861 RVA: 0x00030770 File Offset: 0x0002E970
	public static SBGUIScreen MakeAndPushOptionsDialog(Action backHandler, Action moreNickHandler, Action toggleSFXHandler, Action toggleMusicHandler, Action achievementsHandler, Action creditsHandler, Action privacyHandler, Action eulaHandler, Action debugHandler, Action parentsHandler)
	{
		SBUIBuilder.MakeScreen make = () => (SBGUIScreen)SBGUI.InstantiatePrefab("Prefabs/GUI/Screens/OptionsScreen");
		bool flag;
		SBGUIScreen sbguiscreen = SBUIBuilder.OptionalCacheScreen("Prefabs/GUI/Screens/OptionsScreen", make, out flag);
		AndroidBack.getInstance().push(backHandler, sbguiscreen);
		if (flag)
		{
			sbguiscreen.AttachActionToButton("close", backHandler);
			sbguiscreen.AttachActionToButton("more_nick", moreNickHandler);
			sbguiscreen.AttachActionToButton("toggle_sfx", toggleSFXHandler);
			sbguiscreen.AttachActionToButton("toggle_music", toggleMusicHandler);
			sbguiscreen.AttachActionToButton("credits", creditsHandler);
			sbguiscreen.AttachActionToButton("privacy_policy", privacyHandler);
			sbguiscreen.AttachActionToButton("eula", eulaHandler);
			sbguiscreen.AttachActionToButton("parents", parentsHandler);
			sbguiscreen.AttachActionToButton("TouchableBackground", backHandler);
			if (SBSettings.ShowDebug)
			{
				sbguiscreen.AttachActionToButton("debug", debugHandler);
			}
			else
			{
				SBGUIButton sbguibutton = (SBGUIButton)sbguiscreen.FindChild("debug");
				UnityEngine.Object.Destroy(sbguibutton.gameObject);
			}
		}
		else
		{
			sbguiscreen.gameObject.SetActiveRecursively(true);
		}
		bool @int = PlayerPrefs.GetInt(MusicManager.MUSIC_ENABLED) != 0;
		GameObject gameObject = GameObject.Find("toggle_music");
		ToggleButton component = gameObject.GetComponent<ToggleButton>();
		if (@int)
		{
			component.TurnOn();
		}
		else
		{
			component.TurnOff();
		}
		@int = (PlayerPrefs.GetInt(SoundEffectManager.SOUND_ENABLED) != 0);
		gameObject = GameObject.Find("toggle_sfx");
		component = gameObject.GetComponent<ToggleButton>();
		if (@int)
		{
			component.TurnOn();
		}
		else
		{
			component.TurnOff();
		}
		SBUIBuilder.PushScreen(sbguiscreen);
		return sbguiscreen;
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00030918 File Offset: 0x0002EB18
	private static string GetResourcePrefix(int resourceId)
	{
		return string.Empty;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x00030920 File Offset: 0x0002EB20
	private static void UpdateStandardUI(SBGUIScreen screen, Session session)
	{
		TFUtils.Assert(session != null, "No session given to UpdateStandardUI!");
		foreach (string text in ((List<string>)screen.dynamicProperties["TrackingResourceAmounts"]))
		{
			int resourceId = int.Parse(text);
			int num = session.TheGame.resourceManager.Query(resourceId);
			screen.dynamicLabels[text].Text = SBUIBuilder.GetResourcePrefix(resourceId) + num.ToString();
		}
		foreach (string text2 in ((List<string>)screen.dynamicProperties["TrackingResourcePercentages"]))
		{
			IResourceProgressCalculator resourceCalculator = session.TheGame.resourceCalculatorManager.GetResourceCalculator(int.Parse(text2));
			float num2 = session.TheGame.resourceManager.QueryProgressPercentage(resourceCalculator);
			screen.dynamicMeters[text2].Progress = num2 / 100f;
		}
		SBGUILabel sbguilabel = screen.dynamicLabels["amount_xp_label"];
		sbguilabel.Text = session.TheGame.resourceManager.QueryProgressFraction(session.TheGame.levelingManager);
		SBUIBuilder.UpdateQuestCounter(screen, session);
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x00030AC0 File Offset: 0x0002ECC0
	private static SBGUIElement CreateInteractionStripCache()
	{
		SBGUIElement sbguielement = SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/SimulatedInteractionStrip");
		sbguielement.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		sbguielement.gameObject.SetActiveRecursively(false);
		SBGUIButton original = (SBGUIButton)sbguielement.FindChild("ButtonStandinBig");
		foreach (string text in SBUIBuilder.controlToTextureMap.Values)
		{
			SBGUIButton sbguibutton = (SBGUIButton)UnityEngine.Object.Instantiate(original);
			YGAtlasSprite component = sbguibutton.GetComponent<YGAtlasSprite>();
			component.sprite = component.LoadSpriteFromAtlas(text, component.atlasIndex);
			sbguibutton.gameObject.SetActiveRecursively(false);
			sbguibutton.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
			SBUIBuilder.sInteractionStripButtons[text] = sbguibutton;
		}
		return sbguielement;
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00030BD4 File Offset: 0x0002EDD4
	public static void ReleaseInteractionStrip(SBGUIElement strip)
	{
		strip.MuteButtons(false);
		foreach (SBGUIButton sbguibutton in SBUIBuilder.sInteractionStripButtons.Values)
		{
			sbguibutton.ClearClickEvents();
			sbguibutton.SetParent(null);
			sbguibutton.gameObject.SetActiveRecursively(false);
		}
		strip.SetParent(null);
		strip.gameObject.SetActiveRecursively(false);
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00030C6C File Offset: 0x0002EE6C
	public static void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
	{
		SBUIBuilder.CreateErrorDialog(session, title, message, okButtonLabel, okHandler, null, null, messageScale, titleScale);
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00030C8C File Offset: 0x0002EE8C
	public static void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, string cancelButtonLabel, Action cancelHandler, float messageScale, float titleScale)
	{
		Action androidOk = delegate()
		{
			okHandler();
		};
		Action okButtonHandler = delegate()
		{
			okHandler();
			AndroidBack.getInstance().pop(androidOk);
		};
		Action cancelButtonHandler = delegate()
		{
			cancelHandler();
			AndroidBack.getInstance().pop(androidOk);
		};
		if (cancelHandler == null || cancelButtonLabel == null)
		{
			cancelButtonHandler = null;
			cancelButtonLabel = null;
		}
		SBGUIConfirmationDialog sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, title, message, okButtonLabel, cancelButtonLabel, null, okButtonHandler, cancelButtonHandler, false);
		AndroidBack.getInstance().push(androidOk, sbguiconfirmationDialog);
		SBGUILabel sbguilabel = (SBGUILabel)sbguiconfirmationDialog.FindChild("message_label");
		YGTextAtlasSprite component = sbguilabel.GetComponent<YGTextAtlasSprite>();
		component.scale = new Vector2(messageScale, messageScale);
		SBGUILabel sbguilabel2 = (SBGUILabel)sbguiconfirmationDialog.FindChild("title_label");
		YGTextAtlasSprite component2 = sbguilabel2.GetComponent<YGTextAtlasSprite>();
		component2.scale = new Vector2(titleScale, titleScale);
		sbguiconfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
		sbguiconfirmationDialog.tform.localPosition = Vector3.zero;
	}

	// Token: 0x04000565 RID: 1381
	public const float kErrorMessageScale = 0.85f;

	// Token: 0x04000566 RID: 1382
	public const float kErrorTitleScale = 0.45f;

	// Token: 0x04000567 RID: 1383
	public const float ENABLED_ALPHA = 1f;

	// Token: 0x04000568 RID: 1384
	public const float DISABLED_ALPHA = 0.25f;

	// Token: 0x04000569 RID: 1385
	public const int MAX_EDIT_MODE_BARS = 14;

	// Token: 0x0400056A RID: 1386
	private const string TRACKING_RESOURCE_AMOUNTS = "TrackingResourceAmounts";

	// Token: 0x0400056B RID: 1387
	private const string TRACKING_RESOURCE_PERCENTAGES = "TrackingResourcePercentages";

	// Token: 0x0400056C RID: 1388
	private static GUIMainView mainView;

	// Token: 0x0400056D RID: 1389
	private static Dictionary<string, SBGUIScreen> sCache = new Dictionary<string, SBGUIScreen>();

	// Token: 0x0400056E RID: 1390
	private static SBUIBuilder.ScreenContext topContext = null;

	// Token: 0x0400056F RID: 1391
	private static string game_revision = null;

	// Token: 0x04000570 RID: 1392
	private static Vector3 invWidgetStartPos = new Vector3(-999999f, -999999f, -999999f);

	// Token: 0x04000571 RID: 1393
	private static Transform specialBarParent = null;

	// Token: 0x04000572 RID: 1394
	private static Vector3 specialBarStartPosition = Vector3.zero;

	// Token: 0x04000573 RID: 1395
	private static TFPool<SBGUITimebar> sTimebarPool;

	// Token: 0x04000574 RID: 1396
	private static TFPool<SBGUINamebar> sNamebarPool;

	// Token: 0x04000575 RID: 1397
	private static Dictionary<int, string> ResourcePrefixes;

	// Token: 0x04000576 RID: 1398
	private static readonly Dictionary<Type, string> controlToTextureMap = new Dictionary<Type, string>
	{
		{
			typeof(Session.AcceptPlacementControl),
			"ActionStrip_Accept.png"
		},
		{
			typeof(Session.RejectControl),
			"ActionStrip_Cancel.png"
		},
		{
			typeof(Session.RushControl),
			"IconJellyfishJelly.png"
		},
		{
			typeof(Session.SellControl),
			"ActionStrip_Sell_new.png"
		},
		{
			typeof(Session.StashControl),
			"ActionStrip_Stash.png"
		},
		{
			typeof(Session.RotateControl),
			"EditModeFlipIconOn.png"
		},
		{
			typeof(Session.ClearDebrisControl),
			"ActionStrip_Debris.png"
		},
		{
			typeof(Session.BrowseRecipesControl),
			"ActionStrip_Enter.png"
		}
	};

	// Token: 0x04000577 RID: 1399
	private static Dictionary<string, SBGUIButton> sInteractionStripButtons = new Dictionary<string, SBGUIButton>();

	// Token: 0x04000578 RID: 1400
	private static SBGUIElement sInteractionStrip;

	// Token: 0x020000BC RID: 188
	public class ScreenContext
	{
		// Token: 0x06000761 RID: 1889 RVA: 0x00030F88 File Offset: 0x0002F188
		public override string ToString()
		{
			string text = (this.next != null) ? this.next.ToString() : "null";
			return string.Concat(new object[]
			{
				"[layers: ",
				this.layers,
				"]->",
				text
			});
		}

		// Token: 0x0400058D RID: 1421
		public int layers;

		// Token: 0x0400058E RID: 1422
		public SBUIBuilder.ScreenContext next;
	}

	// Token: 0x020000BD RID: 189
	private class InteractionStripButtonInfo
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x00030FE4 File Offset: 0x0002F1E4
		public InteractionStripButtonInfo(string textureToUse, IControlBinding control)
		{
			this.textureToUse = textureToUse;
			this.control = control;
		}

		// Token: 0x0400058F RID: 1423
		public const string PREFAB_NAME = "ButtonStandinBig";

		// Token: 0x04000590 RID: 1424
		public string textureToUse;

		// Token: 0x04000591 RID: 1425
		public IControlBinding control;
	}

	// Token: 0x0200049D RID: 1181
	// (Invoke) Token: 0x060024D7 RID: 9431
	public delegate SBGUIScreen MakeScreen();
}
