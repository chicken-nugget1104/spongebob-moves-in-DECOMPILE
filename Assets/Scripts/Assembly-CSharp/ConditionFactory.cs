using System;
using System.Collections.Generic;

// Token: 0x02000129 RID: 297
public static class ConditionFactory
{
	// Token: 0x06000AD1 RID: 2769 RVA: 0x00042E70 File Offset: 0x00041070
	public static ICondition FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		if (!dict.ContainsKey("type"))
		{
			TFUtils.Assert(dict.ContainsKey("type"), "Quest Condition does not contain a 'type' and will be unusable. Data=" + TFUtils.DebugDictToString(dict));
		}
		if (!dict.ContainsKey("id"))
		{
			TFUtils.Assert(dict.ContainsKey("id"), "Quest Condition does not contain a 'id' and will be unusable. Data=" + TFUtils.DebugDictToString(dict));
		}
		string text = (string)dict["type"];
		ICondition result = null;
		string text2 = text;
		switch (text2)
		{
		case "tree":
			return ConditionTree.FromDict(dict);
		case "query_simulated":
			return QuerySimulatedCondition.FromDict(dict);
		case "update_resource":
			return ModifyResourceAmountCondition.FromDict(dict);
		case "collect_rent":
			return CollectRentCondition.FromDict(dict);
		case "rush_rent":
			return RushRentCondition.FromDict(dict);
		case "place":
			return PlaceCondition.FromDict(dict);
		case "complete_building":
			return CompleteBuildingCondition.FromDict(dict);
		case "move":
			return MoveCondition.FromDict(dict);
		case "pave":
			return PaveCondition.FromDict(dict);
		case "feed_unit":
			return FeedUnitCondition.FromDict(dict);
		case "collect_match_bonus":
			return CollectMatchBonusCondition.FromDict(dict);
		case "redeem_reward":
			return RedeemRewardsCondition.FromDict(dict);
		case "got_unlockable":
			return GotUnlockableCondition.FromDict(dict);
		case "constant":
			return ConstantCondition.FromDict(dict);
		case "complete_quest":
			return CompleteQuestCondition.FromDict(dict);
		case "auto_quest_all_done":
			return AutoQuestAllDoneCondition.FromDict(dict);
		case "start_quest":
			return StartQuestCondition.FromDict(dict);
		case "progress_quest":
			return ProgressQuestCondition.FromDict(dict);
		case "expand":
			return ExpandCondition.FromDict(dict);
		case "remove_debris":
			return RemoveDebrisCondition.FromDict(dict);
		case "start_building":
			return StartBuildingCondition.FromDict(dict);
		case "craft_start":
			return CraftStartCondition.FromDict(dict);
		case "craft_ready":
			return CraftReadyCondition.FromDict(dict);
		case "craft_collect":
			return CraftCollectCondition.FromDict(dict);
		case "auto_quest_craft_collect":
			return AutoQuestCraftCollectCondition.FromDict(dict);
		case "purchase_production_slot":
			return PurchaseCraftingSlotCondition.FromDict(dict);
		case "constructed":
			return ConstructedCondition.FromDict(dict);
		case "tap_wanderer":
			return TapWandererCondition.FromDict(dict);
		case "hide_wanderer":
			return HideWandererCondition.FromDict(dict);
		case "button_tap":
			return ButtonTapCondition.FromDict(dict);
		case "task_complete":
			return TaskCompleteCondition.FromDict(dict);
		case "task_start":
			return TaskStartCondition.FromDict(dict);
		case "change_costume":
			return ChangeCostumeCondition.FromDict(dict);
		}
		TFUtils.Assert(false, "This Condition uses an unknown condition type: " + text);
		return result;
	}
}
