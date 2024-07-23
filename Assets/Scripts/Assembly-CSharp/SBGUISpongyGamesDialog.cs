using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class SBGUISpongyGamesDialog : SBGUIScreen
{
	// Token: 0x06000629 RID: 1577 RVA: 0x00026EF0 File Offset: 0x000250F0
	public void Setup(SpongyGamesDialogInputData pInputData)
	{
		GameObject gameObject = null;
		int num = 4;
		SBGUIAtlasImage[] array = new SBGUIAtlasImage[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (SBGUIAtlasImage)base.FindChild("character_portrait_" + i.ToString());
			if (i == 0)
			{
				gameObject = array[i].gameObject.transform.parent.gameObject;
			}
		}
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("description_one_boundary");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("description_one_label");
		GameObject gameObject2 = sbguiatlasImage.gameObject.transform.parent.gameObject;
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("description_two_boundary");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("description_two_label");
		GameObject gameObject3 = sbguiatlasImage2.gameObject.transform.parent.gameObject;
		SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)base.FindChild("event_icon");
		SBGUIAtlasImage sbguiatlasImage4 = (SBGUIAtlasImage)base.FindChild("verticle_bar");
		SBGUILabel sbguilabel3 = (SBGUILabel)base.FindChild("event_results_label");
		SBGUILabel sbguilabel4 = (SBGUILabel)base.FindChild("event_title_label");
		SBGUILabel sbguilabel5 = (SBGUILabel)base.FindChild("todays_event_label");
		SBGUILabel sbguilabel6 = (SBGUILabel)base.FindChild("first_day_title_label");
		SBGUIAtlasImage sbguiatlasImage5 = (SBGUIAtlasImage)base.FindChild("first_day_character_portrait");
		GameObject gameObject4 = sbguiatlasImage5.gameObject.transform.parent.gameObject;
		SBGUILabel sbguilabel7 = (SBGUILabel)base.FindChild("last_day_character_label");
		SBGUIAtlasImage sbguiatlasImage6 = (SBGUIAtlasImage)base.FindChild("last_day_character_portrait");
		SBGUIAtlasImage sbguiatlasImage7 = (SBGUIAtlasImage)base.FindChild("last_day_description_two_boundary");
		SBGUILabel sbguilabel8 = (SBGUILabel)base.FindChild("last_day_description_two_label");
		SBGUIAtlasImage sbguiatlasImage8 = (SBGUIAtlasImage)base.FindChild("last_day_reward_portrait");
		SBGUILabel sbguilabel9 = (SBGUILabel)base.FindChild("last_day_title_one_label");
		GameObject gameObject5 = sbguilabel9.gameObject.transform.parent.gameObject;
		Dictionary<string, object> eventData = pInputData.EventData;
		int num2 = TFUtils.LoadInt(eventData, "event_days");
		int num3 = TFUtils.LoadInt(eventData, "day");
		string text = TFUtils.TryLoadString(eventData, "title");
		if (text == null)
		{
			text = string.Empty;
		}
		string text2 = TFUtils.TryLoadString(eventData, "description_one");
		if (text2 == null)
		{
			text2 = string.Empty;
		}
		string text3 = TFUtils.TryLoadString(eventData, "description_two");
		if (text3 == null)
		{
			text3 = string.Empty;
		}
		string text4 = TFUtils.TryLoadString(eventData, "event_portrait");
		if (text4 == null)
		{
			text4 = string.Empty;
		}
		string text5 = TFUtils.TryLoadString(eventData, "event_name");
		if (text5 == null)
		{
			text5 = string.Empty;
		}
		List<int> list = TFUtils.TryLoadList<int>(eventData, "characters");
		int count = list.Count;
		if (num3 == 1)
		{
			gameObject5.SetActive(false);
			gameObject4.SetActive(true);
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			gameObject3.SetActive(true);
			sbguiatlasImage3.SetActive(true);
			sbguiatlasImage4.SetActive(true);
			sbguilabel3.SetActive(false);
			sbguilabel4.SetActive(true);
			sbguilabel5.SetActive(true);
		}
		else if (num3 == num2)
		{
			gameObject5.SetActive(true);
			gameObject4.SetActive(false);
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
			gameObject3.SetActive(false);
			sbguiatlasImage3.SetActive(false);
			sbguiatlasImage4.SetActive(false);
			sbguilabel3.SetActive(false);
			sbguilabel4.SetActive(false);
			sbguilabel5.SetActive(false);
		}
		else
		{
			gameObject5.SetActive(false);
			gameObject4.SetActive(false);
			gameObject.SetActive(true);
			gameObject2.SetActive(true);
			gameObject3.SetActive(true);
			sbguiatlasImage3.SetActive(true);
			sbguiatlasImage4.SetActive(true);
			sbguilabel3.SetActive(true);
			sbguilabel4.SetActive(true);
			sbguilabel5.SetActive(true);
		}
		if (gameObject4.activeSelf)
		{
			sbguiatlasImage5.SetActive(false);
			if (count > 0)
			{
				Blueprint blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, list[0], true);
				if (blueprint != null && blueprint.Invariable.ContainsKey("quest_reminder_icon"))
				{
					sbguiatlasImage5.SetActive(true);
					sbguiatlasImage5.SetTextureFromAtlas((string)blueprint.Invariable["quest_reminder_icon"], true, false, true, false, false, 0);
				}
			}
			sbguilabel6.SetText(Language.Get(text));
		}
		else if (gameObject5.activeSelf)
		{
			sbguilabel7.SetText(Language.Get(text5));
			sbguilabel9.SetText(Language.Get(text));
			sbguilabel8.SetText(Language.Get(text3));
			sbguiatlasImage6.SetActive(false);
			if (count > 0)
			{
				Blueprint blueprint2 = EntityManager.GetBlueprint(EntityType.RESIDENT, list[0], true);
				if (blueprint2 != null && blueprint2.Invariable.ContainsKey("quest_reminder_icon"))
				{
					sbguiatlasImage6.SetActive(true);
					sbguiatlasImage6.SetTextureFromAtlas((string)blueprint2.Invariable["quest_reminder_icon"], true, false, true, false, false, 0);
				}
			}
			sbguiatlasImage8.SetTextureFromAtlas(text4, true, false, true, false, false, 0);
		}
		else
		{
			int num4 = 0;
			float num5 = 0.4f;
			for (int j = 0; j < num; j++)
			{
				SBGUIAtlasImage sbguiatlasImage9 = array[j];
				if (j >= count)
				{
					sbguiatlasImage9.SetActive(false);
				}
				else
				{
					int did = list[j];
					Blueprint blueprint3 = EntityManager.GetBlueprint(EntityType.RESIDENT, did, true);
					if (blueprint3 == null)
					{
						sbguiatlasImage9.SetActive(false);
					}
					else if (!blueprint3.Invariable.ContainsKey("quest_reminder_icon"))
					{
						sbguiatlasImage9.SetActive(false);
					}
					else
					{
						sbguiatlasImage9.SetActive(true);
						sbguiatlasImage9.SetTextureFromAtlas((string)blueprint3.Invariable["quest_reminder_icon"], true, false, true, false, false, 0);
						Transform transform = sbguiatlasImage9.transform;
						if (j <= 1)
						{
							transform.localPosition = new Vector3(transform.localPosition.x, num5, transform.localPosition.z);
						}
						else
						{
							transform.localPosition = new Vector3(transform.localPosition.x, -num5, transform.localPosition.z);
						}
						num4++;
					}
				}
			}
			if (num4 <= 2)
			{
				for (int k = 0; k < num; k++)
				{
					SBGUIAtlasImage sbguiatlasImage9 = array[k];
					if (sbguiatlasImage9.IsActive())
					{
						Transform transform = sbguiatlasImage9.transform;
						transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
					}
				}
			}
			sbguilabel3.SetText(Language.Get(text));
		}
		if (gameObject2.activeInHierarchy)
		{
			sbguilabel.SetText(Language.Get(text2));
		}
		if (gameObject3.activeInHierarchy)
		{
			sbguilabel2.SetText(Language.Get(text3));
		}
		if (sbguilabel4.gameObject.activeInHierarchy)
		{
			sbguilabel4.SetText(Language.Get(text5));
		}
		if (sbguiatlasImage3.gameObject.activeInHierarchy)
		{
			sbguiatlasImage3.SetTextureFromAtlas(text4, true, false, true, false, false, 0);
		}
	}
}
