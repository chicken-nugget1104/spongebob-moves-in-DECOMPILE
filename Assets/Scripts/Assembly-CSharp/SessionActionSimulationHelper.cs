using System;
using System.Collections.Generic;

// Token: 0x0200024B RID: 587
public static class SessionActionSimulationHelper
{
	// Token: 0x060012EF RID: 4847 RVA: 0x00082C38 File Offset: 0x00080E38
	public static void HandleCommonSessionActions(Session session, List<SBGUIScreen> screens, SessionActionTracker action)
	{
		string type = action.Definition.Type;
		switch (type)
		{
		case "point_at_simulated":
		{
			PointAtSimulated pointAtSimulated = (PointAtSimulated)action.Definition;
			SessionActionSimulationHelper.CreateSpawn(session, action, pointAtSimulated.TargetSelected, pointAtSimulated.TargetId, pointAtSimulated.TargetDid, pointAtSimulated.SubHudSubTarget, pointAtSimulated, null);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "point_at_simulation":
		{
			PointAtSimulation pointAtSimulation = (PointAtSimulation)action.Definition;
			pointAtSimulation.SpawnPointer(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "point_at_expansion":
		{
			PointAtExpansion pointAtExpansion = (PointAtExpansion)action.Definition;
			pointAtExpansion.SpawnPointer(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "screenmask_simulated":
		{
			ScreenMaskSimulation screenMaskSimulation = (ScreenMaskSimulation)action.Definition;
			SessionActionSimulationHelper.CreateSpawn(session, action, screenMaskSimulation.TargetSelected, screenMaskSimulation.TargetId, screenMaskSimulation.TargetDid, screenMaskSimulation.SubHudSubTarget, null, screenMaskSimulation);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "screenmask_simulation":
		{
			ScreenMaskSimulation screenMaskSimulation2 = (ScreenMaskSimulation)action.Definition;
			screenMaskSimulation2.SpawnSimulationMask(session.TheGame, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "screenmask_expansion":
		{
			ScreenMaskSimulation screenMaskSimulation3 = (ScreenMaskSimulation)action.Definition;
			screenMaskSimulation3.SpawnExpansionMask(session.TheGame, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "footprint_guide":
		{
			FootprintGuide footprintGuide = (FootprintGuide)action.Definition;
			footprintGuide.SpawnFootprint(session.TheGame, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "preplace_simulated_request":
		{
			PreplaceSimulatedRequest preplaceSimulatedRequest = (PreplaceSimulatedRequest)action.Definition;
			preplaceSimulatedRequest.Preplace(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "force_wish":
		{
			ForceResidentHunger forceResidentHunger = (ForceResidentHunger)action.Definition;
			forceResidentHunger.Handle(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "force_bonus_reward":
		{
			ForceResidentBonusReward forceResidentBonusReward = (ForceResidentBonusReward)action.Definition;
			forceResidentBonusReward.Handle(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "force_crafting_instance_ready":
		{
			ForceCraftingInstanceReady forceCraftingInstanceReady = (ForceCraftingInstanceReady)action.Definition;
			forceCraftingInstanceReady.Handle(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "force_crafting_instance_slot":
		{
			ForceCraftingInstanceSlot forceCraftingInstanceSlot = (ForceCraftingInstanceSlot)action.Definition;
			forceCraftingInstanceSlot.Handle(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "force_rent_ready":
		{
			ForceRentReady forceRentReady = (ForceRentReady)action.Definition;
			forceRentReady.Handle(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "force_produce":
		{
			ForceProduce forceProduce = (ForceProduce)action.Definition;
			forceProduce.Handle(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "force_treasure_spawn":
		{
			ForceTreasureSpawn forceTreasureSpawn = (ForceTreasureSpawn)action.Definition;
			forceTreasureSpawn.Handle(session, action);
			break;
		}
		case "spawn_wanderer":
		{
			SpawnWanderer spawnWanderer = (SpawnWanderer)action.Definition;
			spawnWanderer.Handle(session, action);
			break;
		}
		case "spawn_resident":
		{
			SpawnResident spawnResident = (SpawnResident)action.Definition;
			spawnResident.Handle(session, action);
			break;
		}
		case "disable_flee":
		{
			DisableFlee disableFlee = (DisableFlee)action.Definition;
			disableFlee.Handle(session, action);
			break;
		}
		case "lock_recipe":
		{
			LockRecipe lockRecipe = (LockRecipe)action.Definition;
			lockRecipe.Handle(session, action);
			break;
		}
		case "lock_input":
		{
			LockInput lockInput = (LockInput)action.Definition;
			lockInput.Handle(session, action);
			SessionActionSimulationHelper.DeactivateQuestTracker(session);
			break;
		}
		case "mock_click_simulated":
		{
			MockClickSimulated mockClickSimulated = (MockClickSimulated)action.Definition;
			if (mockClickSimulated.TargetDid != null || mockClickSimulated.TargetId != null)
			{
				Simulated simulated = null;
				if (mockClickSimulated.TargetId != null)
				{
					simulated = session.TheGame.simulation.FindSimulated(mockClickSimulated.TargetId);
				}
				else if (mockClickSimulated.TargetDid != null)
				{
					simulated = session.TheGame.simulation.FindSimulated(new int?(mockClickSimulated.TargetDid.Value));
				}
				mockClickSimulated.HandleClick(session, action, simulated);
			}
			break;
		}
		case "mock_click_simulated_cancel":
		{
			MockClickSimulatedCancel mockClickSimulatedCancel = (MockClickSimulatedCancel)action.Definition;
			mockClickSimulatedCancel.HandleCancel(session, action);
			break;
		}
		case "start_micro_event":
		{
			StartMicroEvent startMicroEvent = (StartMicroEvent)action.Definition;
			startMicroEvent.Handle(session, action);
			break;
		}
		case "complete_micro_event":
		{
			CompleteMicroEvent completeMicroEvent = (CompleteMicroEvent)action.Definition;
			completeMicroEvent.Handle(session, action);
			break;
		}
		}
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x000831E0 File Offset: 0x000813E0
	private static void CreateSpawn(Session session, SessionActionTracker action, bool targetSelected, Identity targetId, int? targetDid, string subHudSubTarget, PointAtSimulated pointAtSimul, ScreenMaskSimulation screenMaskSimul)
	{
		Simulated simulated = null;
		if (targetSelected)
		{
			if (targetId != null)
			{
				if (session.TheGame.selected.Id == targetId)
				{
					simulated = session.TheGame.selected;
				}
			}
			else if (targetDid != null)
			{
				if (session.TheGame.selected.entity.DefinitionId == targetDid.Value)
				{
					simulated = session.TheGame.selected;
				}
			}
			else if (subHudSubTarget != null)
			{
				if (targetId != null)
				{
					if (session.TheGame.selected.Id == targetId)
					{
						simulated = session.TheGame.selected;
					}
				}
				else if (targetDid != null && session.TheGame.selected.entity.DefinitionId == targetDid.Value)
				{
					simulated = session.TheGame.selected;
				}
				SBGUIScreen scratchScreen = session.TheGame.simulation.scratchScreen;
				SBGUIElement sbguielement = scratchScreen.FindChildSessionActionId(SessionActionSimulationHelper.DecorateSessionActionId(0U, subHudSubTarget), false);
				if (sbguielement != null)
				{
					if (pointAtSimul != null)
					{
						pointAtSimul.SpawnSubHudPointer(session, action, simulated, sbguielement, scratchScreen);
					}
					else
					{
						screenMaskSimul.SpawnSubHudMask(session.TheGame, action, sbguielement, scratchScreen);
					}
				}
			}
		}
		else if (targetId != null)
		{
			simulated = session.TheGame.simulation.FindSimulated(targetId);
		}
		else if (targetDid != null)
		{
			simulated = session.TheGame.simulation.FindSimulated(targetDid);
		}
		if (simulated != null)
		{
			if (subHudSubTarget == null)
			{
				if (pointAtSimul != null)
				{
					pointAtSimul.SpawnSimulatedPointer(session, action, simulated, null, null);
				}
				else
				{
					screenMaskSimul.SpawnSimulatedMask(session.TheGame, action, simulated);
				}
			}
			else
			{
				SBGUIScreen scratchScreen2 = session.TheGame.simulation.scratchScreen;
				SBGUIElement sbguielement2 = scratchScreen2.FindChildSessionActionId(SessionActionSimulationHelper.DecorateSessionActionId((uint)simulated.entity.DefinitionId, subHudSubTarget), false);
				if (sbguielement2 != null)
				{
					if (pointAtSimul != null)
					{
						pointAtSimul.SpawnSubHudPointer(session, action, simulated, sbguielement2, scratchScreen2);
					}
					else
					{
						screenMaskSimul.SpawnSubHudMask(session.TheGame, action, sbguielement2, scratchScreen2);
					}
				}
			}
		}
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x00083404 File Offset: 0x00081604
	public static void EnableHandler(Session session, bool enabled)
	{
		string id = "simulation";
		if (enabled)
		{
			if (!session.TheGame.sessionActionManager.ExistsActionHandler(id))
			{
				session.TheGame.sessionActionManager.SetActionHandler(id, session, null, new SessionActionManager.Handler(SessionActionSimulationHelper.HandleCommonSessionActions));
			}
		}
		else if (session.TheGame.sessionActionManager.ExistsActionHandler(id))
		{
			session.TheGame.sessionActionManager.ClearActionHandler(id, session);
		}
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x00083480 File Offset: 0x00081680
	public static string DecorateSessionActionId(uint ownerDid, string targetToken)
	{
		return "SubHud_" + targetToken;
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x00083490 File Offset: 0x00081690
	private static void DeactivateQuestTracker(Session session)
	{
	}
}
