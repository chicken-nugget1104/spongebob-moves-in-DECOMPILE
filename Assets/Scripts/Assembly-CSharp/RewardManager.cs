using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public static class RewardManager
{
	// Token: 0x06000F7C RID: 3964 RVA: 0x000631A0 File Offset: 0x000613A0
	public static void ApplyToGameState(Reward reward, ulong collectionTime, Dictionary<string, object> gameState)
	{
		foreach (KeyValuePair<int, int> keyValuePair in reward.ResourceAmounts)
		{
			ResourceManager.AddAmountToGameState(keyValuePair.Key, keyValuePair.Value, gameState);
		}
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		foreach (KeyValuePair<int, int> keyValuePair2 in reward.BuildingAmounts)
		{
			int key = keyValuePair2.Key;
			int value = keyValuePair2.Value;
			Blueprint blueprint = EntityManager.GetBlueprint("building", key, false);
			List<object> list2 = (List<object>)reward.BuildingLabels[key.ToString()];
			for (int i = 0; i < value; i++)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				if (reward.BuildingPositions == null || !reward.BuildingPositions.ContainsKey(key))
				{
					dictionary["did"] = key;
					dictionary["extensions"] = (uint)(blueprint.PrimaryType | EntityType.CORE);
					dictionary["label"] = (string)list2[i];
					dictionary["x"] = null;
					dictionary["y"] = null;
					dictionary["flip"] = false;
					dictionary["build_finish_time"] = collectionTime;
					ActivatableDecorator.Serialize(ref dictionary, collectionTime);
					if (blueprint.Invariable.ContainsKey("time.production") && blueprint.Invariable["time.production"] != null)
					{
						dictionary["rent_ready_time"] = collectionTime + (ulong)blueprint.Invariable["time.production"];
					}
					list.Add(dictionary);
				}
			}
		}
		List<object> list3 = (List<object>)((Dictionary<string, object>)gameState["farm"])["recipes"];
		foreach (int num in reward.RecipeUnlocks)
		{
			list3.Add(num);
		}
		List<object> list4 = (List<object>)((Dictionary<string, object>)gameState["farm"])["costumes"];
		foreach (int num2 in reward.CostumeUnlocks)
		{
			list4.Add(num2);
		}
		List<object> list5 = (List<object>)((Dictionary<string, object>)gameState["farm"])["movies"];
		foreach (int num3 in reward.MovieUnlocks)
		{
			list5.Add(num3);
		}
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x00063568 File Offset: 0x00061768
	public static RewardManager.RewardDropResults GenerateRewardDrops(Reward reward, Simulation simulation, Simulated simulated, ulong utcNow, bool bonusReward = false)
	{
		Vector3 dropPosition = new Vector3(simulated.ThoughtDisplayController.Position.x - simulated.ThoughtDisplayController.Width / 2f, simulated.ThoughtDisplayController.Position.y, simulated.ThoughtDisplayController.Position.z);
		return RewardManager.GenerateRewardDrops(reward, simulation, dropPosition, utcNow, bonusReward);
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x000635D8 File Offset: 0x000617D8
	public static RewardManager.RewardDropResults GenerateRewardDrops(Reward reward, Simulation simulation, Vector3 dropPosition, ulong utcNow, bool bonusReward = false)
	{
		if (reward == null)
		{
			TFUtils.ErrorLog("RewardManager.GenerateRewardDrops - reward is null");
			return null;
		}
		int amount = simulation.resourceManager.Resources[ResourceManager.LEVEL].Amount;
		int num = 0;
		int num2 = 0;
		int amountOfCurrentRewardToDrop = 0;
		List<ItemDropCtor> list = new List<ItemDropCtor>();
		List<Identity> list2 = new List<Identity>();
		foreach (KeyValuePair<int, int> keyValuePair in reward.ResourceAmounts)
		{
			int key = keyValuePair.Key;
			int value = keyValuePair.Value;
			if (key == ResourceManager.SOFT_CURRENCY)
			{
				num2 = value;
			}
			else if (key == ResourceManager.XP)
			{
				num = value;
			}
			else if (key == ResourceManager.VALENTINES_CURRENCY || key == ResourceManager.CHRISTMAS_CURRENCY || key == ResourceManager.CHRISTMAS_CURRENCY_V2 || key == ResourceManager.HALLOWEEN_CURRENCY || key == ResourceManager.SPONGY_GAMES_CURRENCY || key == ResourceManager.SPECIAL_CURRENCY)
			{
				amountOfCurrentRewardToDrop = value;
			}
			simulation.analytics.LogResourceDrop(simulation.resourceManager.Resources[keyValuePair.Key].Name, value, amount);
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in reward.ResourceAmounts)
		{
			int key2 = keyValuePair2.Key;
			int value2 = keyValuePair2.Value;
			if (key2 == ResourceManager.SOFT_CURRENCY && !bonusReward)
			{
				RewardManager.GenerateDividedRewardDrops(simulation, key2, list, utcNow, num2, num);
			}
			else if (key2 == ResourceManager.XP && !bonusReward)
			{
				RewardManager.GenerateDividedRewardDrops(simulation, key2, list, utcNow, num, num2);
			}
			else if ((key2 == ResourceManager.VALENTINES_CURRENCY || key2 == ResourceManager.CHRISTMAS_CURRENCY || key2 == ResourceManager.CHRISTMAS_CURRENCY_V2 || key2 == ResourceManager.HALLOWEEN_CURRENCY || key2 == ResourceManager.SPONGY_GAMES_CURRENCY || key2 == ResourceManager.SPECIAL_CURRENCY) && !bonusReward)
			{
				RewardManager.GenerateDividedRewardDrops(simulation, key2, list, utcNow, amountOfCurrentRewardToDrop, num);
			}
			else
			{
				for (int i = 0; i < value2; i++)
				{
					string resourceTexture = simulation.resourceManager.Resources[key2].GetResourceTexture(1);
					IDisplayController displayController = simulation.rewardDropManager.CreateDrop(simulation.resourceManager.Resources[key2]);
					Vector2 screenCollectionDestination = ResourceDrop.GetScreenCollectionDestination(key2);
					ItemDropDefinition itemDropDefinition = new ItemDropDefinition(key2, displayController, screenCollectionDestination, simulation.resourceManager.Resources[key2].ForceTapToCollect);
					itemDropDefinition.DisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
					list.Add(new ResourceDropCtor(itemDropDefinition, 1, utcNow));
					if (resourceTexture != null)
					{
						displayController.UpdateMaterialOrTexture(resourceTexture);
					}
				}
			}
		}
		foreach (KeyValuePair<int, int> keyValuePair3 in reward.BuildingAmounts)
		{
			int key3 = keyValuePair3.Key;
			int value3 = keyValuePair3.Value;
			List<object> list3 = (List<object>)reward.BuildingLabels[key3.ToString()];
			for (int j = 0; j < value3; j++)
			{
				Blueprint blueprint = EntityManager.GetBlueprint("building", key3, false);
				IDisplayController displayController2 = (IDisplayController)blueprint.Invariable["display"];
				displayController2 = simulation.rewardDropManager.CreateDrop((float)((int)displayController2.Width), (float)((int)displayController2.Height), null, displayController2.MaterialName);
				Vector2 screenCollectionDestination2 = BuildingDrop.GetScreenCollectionDestination();
				ItemDropDefinition itemDropDefinition2 = new ItemDropDefinition(key3, displayController2, screenCollectionDestination2, false);
				itemDropDefinition2.DisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
				list.Add(new BuildingDropCtor(itemDropDefinition2, new Identity((string)list3[j]), utcNow));
				simulation.analytics.LogBuildingDrop((string)blueprint.Invariable["name"], 1, amount);
			}
		}
		foreach (int num3 in reward.RecipeUnlocks)
		{
			IDisplayController displayController3 = simulation.rewardDropManager.CreateDrop("RecipeIcon.png");
			Vector2 screenCollectionDestination3 = RecipeDrop.GetScreenCollectionDestination();
			ItemDropDefinition itemDropDefinition3 = new ItemDropDefinition(num3, displayController3, screenCollectionDestination3, true);
			itemDropDefinition3.DisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
			list.Add(new RecipeDropCtor(itemDropDefinition3, utcNow));
			CraftingRecipe recipeById = simulation.game.craftManager.GetRecipeById(num3);
			simulation.analytics.LogRecipeDrop(recipeById.recipeTag, amount);
		}
		foreach (int num4 in reward.MovieUnlocks)
		{
			IDisplayController displayController4 = simulation.rewardDropManager.CreateDrop("MovieIcon.png");
			Vector2 screenCollectionDestination4 = MovieDrop.GetScreenCollectionDestination();
			ItemDropDefinition itemDropDefinition4 = new ItemDropDefinition(num4, displayController4, screenCollectionDestination4, true);
			itemDropDefinition4.DisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
			list.Add(new MovieDropCtor(itemDropDefinition4, utcNow));
			simulation.analytics.LogMovieDrop(string.Format("movie_{0:0000}", num4), amount);
		}
		simulation.DropManager.AddDrops(dropPosition, list, list2, simulation);
		return new RewardManager.RewardDropResults(reward.BuildingLabels, list2);
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x00063BE0 File Offset: 0x00061DE0
	private static void GenerateDividedRewardDrops(Simulation simulation, int resourceDid, List<ItemDropCtor> rewardDrops, ulong utcNow, int amountOfCurrentRewardToDrop, int amountOfNextRewardToDrop)
	{
		int num = (!TFPerfUtils.IsNonParticleDevice()) ? UnityEngine.Random.Range(1, 6) : 1;
		if (amountOfCurrentRewardToDrop > num)
		{
			int num2 = amountOfCurrentRewardToDrop / num;
			int num3 = amountOfCurrentRewardToDrop % num;
			for (int i = 0; i < num; i++)
			{
				string resourceTexture;
				if (i == num - 1)
				{
					resourceTexture = simulation.resourceManager.Resources[resourceDid].GetResourceTexture(num2 + num3);
				}
				else
				{
					resourceTexture = simulation.resourceManager.Resources[resourceDid].GetResourceTexture(num2);
				}
				IDisplayController displayController = simulation.rewardDropManager.CreateDrop(simulation.resourceManager.Resources[resourceDid]);
				Vector2 screenCollectionDestination = ResourceDrop.GetScreenCollectionDestination(resourceDid);
				ItemDropDefinition itemDropDefinition = new ItemDropDefinition(resourceDid, displayController, screenCollectionDestination, simulation.resourceManager.Resources[resourceDid].ForceTapToCollect);
				itemDropDefinition.DisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
				if (resourceTexture != null)
				{
					displayController.UpdateMaterialOrTexture(resourceTexture);
				}
				if (i == num - 1)
				{
					rewardDrops.Add(new ResourceDropCtor(itemDropDefinition, num2 + num3, utcNow));
				}
				else
				{
					rewardDrops.Add(new ResourceDropCtor(itemDropDefinition, num2, utcNow));
				}
			}
		}
		else
		{
			for (int j = 0; j < amountOfCurrentRewardToDrop; j++)
			{
				string resourceTexture2 = simulation.resourceManager.Resources[resourceDid].GetResourceTexture(1);
				IDisplayController displayController2 = simulation.rewardDropManager.CreateDrop(simulation.resourceManager.Resources[resourceDid]);
				Vector2 screenCollectionDestination2 = ResourceDrop.GetScreenCollectionDestination(resourceDid);
				ItemDropDefinition itemDropDefinition2 = new ItemDropDefinition(resourceDid, displayController2, screenCollectionDestination2, simulation.resourceManager.Resources[resourceDid].ForceTapToCollect);
				itemDropDefinition2.DisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
				rewardDrops.Add(new ResourceDropCtor(itemDropDefinition2, 1, utcNow));
				if (resourceTexture2 != null)
				{
					displayController2.UpdateMaterialOrTexture(resourceTexture2);
				}
			}
		}
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x00063DC4 File Offset: 0x00061FC4
	public static bool ReleaseDisplayController(Simulation simulation, IDisplayController dc)
	{
		return simulation.rewardDropManager.ReleaseDrop(dc);
	}

	// Token: 0x020001C4 RID: 452
	public class RewardDropResults
	{
		// Token: 0x06000F81 RID: 3969 RVA: 0x00063DD4 File Offset: 0x00061FD4
		public RewardDropResults(Dictionary<string, object> buildingLabels, List<Identity> dropIdentities)
		{
			this.buildingLabels = buildingLabels;
			this.dropIdentities = dropIdentities;
		}

		// Token: 0x04000A77 RID: 2679
		public Dictionary<string, object> buildingLabels;

		// Token: 0x04000A78 RID: 2680
		public List<Identity> dropIdentities;
	}
}
