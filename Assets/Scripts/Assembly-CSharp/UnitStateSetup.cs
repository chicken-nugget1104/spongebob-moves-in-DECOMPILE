﻿using System;
using System.Collections.Generic;

// Token: 0x02000445 RID: 1093
public static class UnitStateSetup
{
	// Token: 0x060021E8 RID: 8680 RVA: 0x000CFD0C File Offset: 0x000CDF0C
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("idle", Simulated.Resident.Idle);
		actions.Add("idle_full", Simulated.Resident.IdleFull);
		actions.Add("idle_wishing", Simulated.Resident.IdleWishing);
		actions.Add("start_wander", Simulated.Resident.StartingWanderCycle);
		actions.Add("moving", Simulated.Resident.Moving);
		actions.Add("residing", Simulated.Resident.Residing);
		actions.Add("wander_full", Simulated.Resident.WanderingFull);
		actions.Add("wander_hungry", Simulated.Resident.WanderingHungry);
		actions.Add("wishing", Simulated.Resident.WishingForFood);
		actions.Add("tempted", Simulated.Resident.Tempted);
		actions.Add("not_tempted", Simulated.Resident.NotTempted);
		actions.Add("wait_delivery", Simulated.Resident.WaitingForDelivery);
		actions.Add("try_eat", Simulated.Resident.TryEat);
		actions.Add("eat", Simulated.Resident.Eating);
		actions.Add("try_spin", Simulated.Resident.TryBonusSpin);
		actions.Add("wait_bonus", Simulated.Resident.WaitingForCollectBonus);
		actions.Add("cheer_bonus", Simulated.Resident.CheeringAfterBonus);
		actions.Add("rush_full", Simulated.Resident.RushingFullness);
		actions.Add("cheer_reward", Simulated.Resident.Cheering);
		actions.Add("go_home", Simulated.Resident.GoHome);
		actions.Add("store_resident", Simulated.Resident.StoreResident);
		actions.Add("requesting_interface", Simulated.Resident.RequestingInterface);
		actions.Add("reflecting", Simulated.Resident.Reflecting);
		actions.Add("task_delegating", Simulated.Resident.TaskDelegating);
		actions.Add("task_idle", Simulated.Resident.TaskIdle);
		actions.Add("task_wander", Simulated.Resident.TaskWander);
		actions.Add("task_moving", Simulated.Resident.TaskMoving);
		actions.Add("task_enter", Simulated.Resident.TaskEnter);
		actions.Add("task_enter_feed", Simulated.Resident.TaskEnterFeed);
		actions.Add("task_stand", Simulated.Resident.TaskStand);
		actions.Add("task_collect_reward", Simulated.Resident.TaskCollectReward);
		actions.Add("task_cheer_after_collect", Simulated.Resident.TaskCheerAfterCollect);
		foreach (Simulated.StateAction state in actions.Values)
		{
			machine.AddState(state);
		}
		machine.AddTransition(actions["idle"], Command.TYPE.MOVE, actions["moving"]);
		machine.AddTransition(actions["idle"], Command.TYPE.WANDER, actions["start_wander"]);
		machine.AddTransition(actions["idle"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["idle"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.RESUME_FULL, actions["wander_full"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.EXPAND, actions["requesting_interface"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.TEMPT, actions["not_tempted"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.HUNGER, actions["wander_hungry"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.COMPLETE, actions["wander_full"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.RUSH, actions["rush_full"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["idle_wishing"], Command.TYPE.RESUME_WISHING, actions["wishing"]);
		machine.AddTransition(actions["idle_wishing"], Command.TYPE.ABORT, actions["wander_hungry"]);
		machine.AddTransition(actions["idle_wishing"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["idle_wishing"], Command.TYPE.TEMPT, actions["tempted"]);
		machine.AddTransition(actions["idle_wishing"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["idle_wishing"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["idle_wishing"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["moving"], Command.TYPE.RESIDE, actions["residing"]);
		machine.AddTransition(actions["moving"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["residing"], Command.TYPE.COMPLETE, actions["wander_full"]);
		machine.AddTransition(actions["residing"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["start_wander"], Command.TYPE.WANDER, actions["wander_full"]);
		machine.AddTransition(actions["start_wander"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.COMPLETE, actions["wander_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.TEMPT, actions["not_tempted"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.HUNGER, actions["wander_hungry"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.EXPAND, actions["requesting_interface"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.IDLE_PAUSE, actions["idle_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.RUSH, actions["rush_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["rush_full"], Command.TYPE.COMPLETE, actions["reflecting"]);
		machine.AddTransition(actions["not_tempted"], Command.TYPE.COMPLETE, actions["start_wander"]);
		machine.AddTransition(actions["wander_hungry"], Command.TYPE.COMPLETE, actions["wander_hungry"]);
		machine.AddTransition(actions["wander_hungry"], Command.TYPE.WISH, actions["wishing"]);
		machine.AddTransition(actions["wander_hungry"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["wander_hungry"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["wander_hungry"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["wishing"], Command.TYPE.ABORT, actions["wander_hungry"]);
		machine.AddTransition(actions["wishing"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["wishing"], Command.TYPE.TEMPT, actions["tempted"]);
		machine.AddTransition(actions["wishing"], Command.TYPE.IDLE_PAUSE, actions["idle_wishing"]);
		machine.AddTransition(actions["wishing"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["wishing"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["wishing"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["tempted"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["tempted"], Command.TYPE.ABORT, actions["wishing"]);
		machine.AddTransition(actions["try_eat"], Command.TYPE.COMPLETE, actions["eat"]);
		machine.AddTransition(actions["try_eat"], Command.TYPE.WISH, actions["wishing"]);
		machine.AddTransition(actions["try_eat"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["eat"], Command.TYPE.COMPLETE, actions["try_spin"]);
		machine.AddTransition(actions["try_spin"], Command.TYPE.ABORT, actions["start_wander"]);
		machine.AddTransition(actions["try_spin"], Command.TYPE.COMPLETE, actions["wait_bonus"]);
		machine.AddTransition(actions["wait_bonus"], Command.TYPE.CLICKED, actions["cheer_bonus"]);
		machine.AddTransition(actions["cheer_reward"], Command.TYPE.COMPLETE, actions["start_wander"]);
		machine.AddTransition(actions["cheer_bonus"], Command.TYPE.COMPLETE, actions["start_wander"]);
		machine.AddTransition(actions["go_home"], Command.TYPE.STORE_RESIDENT, actions["store_resident"]);
		machine.AddTransition(actions["requesting_interface"], Command.TYPE.COMPLETE, actions["reflecting"]);
		machine.AddTransition(actions["reflecting"], Command.TYPE.RESUME_FULL, actions["wander_full"]);
		machine.AddTransition(actions["reflecting"], Command.TYPE.RESUME_WISHING, actions["wander_hungry"]);
		machine.AddTransition(actions["reflecting"], Command.TYPE.TASK, actions["task_delegating"]);
		machine.AddTransition(actions["task_delegating"], Command.TYPE.WANDER, actions["task_wander"]);
		machine.AddTransition(actions["task_delegating"], Command.TYPE.MOVE, actions["task_moving"]);
		machine.AddTransition(actions["task_delegating"], Command.TYPE.ENTER, actions["task_enter_feed"]);
		machine.AddTransition(actions["task_delegating"], Command.TYPE.RUSH_TASK, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_delegating"], Command.TYPE.COMPLETE, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_delegating"], Command.TYPE.ABORT, actions["reflecting"]);
		machine.AddTransition(actions["task_idle"], Command.TYPE.RUSH_TASK, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_idle"], Command.TYPE.COMPLETE, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_idle"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["task_idle"], Command.TYPE.RUSH, actions["rush_full"]);
		machine.AddTransition(actions["task_idle"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["task_idle"], Command.TYPE.ABORT, actions["task_wander"]);
		machine.AddTransition(actions["task_wander"], Command.TYPE.RUSH_TASK, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_wander"], Command.TYPE.COMPLETE, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_wander"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["task_wander"], Command.TYPE.RUSH, actions["rush_full"]);
		machine.AddTransition(actions["task_wander"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["task_wander"], Command.TYPE.IDLE_PAUSE, actions["task_idle"]);
		machine.AddTransition(actions["task_moving"], Command.TYPE.RUSH_TASK, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_moving"], Command.TYPE.ENTER, actions["task_enter"]);
		machine.AddTransition(actions["task_moving"], Command.TYPE.STAND, actions["task_stand"]);
		machine.AddTransition(actions["task_moving"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["task_moving"], Command.TYPE.RUSH, actions["rush_full"]);
		machine.AddTransition(actions["task_moving"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["task_enter"], Command.TYPE.RUSH_TASK, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_enter"], Command.TYPE.COMPLETE, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_enter"], Command.TYPE.RUSH, actions["rush_full"]);
		machine.AddTransition(actions["task_enter"], Command.TYPE.FEED, actions["task_enter_feed"]);
		machine.AddTransition(actions["task_enter_feed"], Command.TYPE.TASK, actions["task_enter"]);
		machine.AddTransition(actions["task_stand"], Command.TYPE.RUSH_TASK, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_stand"], Command.TYPE.COMPLETE, actions["task_collect_reward"]);
		machine.AddTransition(actions["task_stand"], Command.TYPE.CLICKED, actions["requesting_interface"]);
		machine.AddTransition(actions["task_stand"], Command.TYPE.RUSH, actions["rush_full"]);
		machine.AddTransition(actions["task_stand"], Command.TYPE.FEED, actions["try_eat"]);
		machine.AddTransition(actions["task_collect_reward"], Command.TYPE.CLICKED, actions["task_cheer_after_collect"]);
		machine.AddTransition(actions["task_cheer_after_collect"], Command.TYPE.COMPLETE, actions["reflecting"]);
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000D0CA0 File Offset: 0x000CEEA0
	public static void GenerateFriendsStates(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("idle", Simulated.Resident.Idle);
		actions.Add("idle_full", Simulated.Resident.IdleFull);
		actions.Add("idle_wishing", Simulated.Resident.IdleWishing);
		actions.Add("start_wander", Simulated.Resident.StartingWanderCycle);
		actions.Add("moving", Simulated.Resident.Moving);
		actions.Add("residing", Simulated.Resident.Residing);
		actions.Add("wander_full", Simulated.Resident.WanderingFull);
		actions.Add("wander_hungry", Simulated.Resident.WanderingHungry);
		actions.Add("wishing", Simulated.Resident.WishingForFood);
		actions.Add("tempted", Simulated.Resident.Tempted);
		actions.Add("not_tempted", Simulated.Resident.NotTempted);
		actions.Add("wait_delivery", Simulated.Resident.WaitingForDelivery);
		actions.Add("try_eat", Simulated.Resident.TryEat);
		actions.Add("eat", Simulated.Resident.Eating);
		actions.Add("try_spin", Simulated.Resident.TryBonusSpin);
		actions.Add("wait_bonus", Simulated.Resident.WaitingForCollectBonus);
		actions.Add("cheer_bonus", Simulated.Resident.CheeringAfterBonus);
		actions.Add("cheer_reward", Simulated.Resident.Cheering);
		actions.Add("go_home", Simulated.Resident.GoHome);
		actions.Add("store_resident", Simulated.Resident.StoreResident);
		foreach (Simulated.StateAction state in actions.Values)
		{
			machine.AddState(state);
		}
		machine.AddTransition(actions["idle"], Command.TYPE.MOVE, actions["moving"]);
		machine.AddTransition(actions["idle"], Command.TYPE.WANDER, actions["start_wander"]);
		machine.AddTransition(actions["idle"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.RESUME_FULL, actions["wander_full"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.TEMPT, actions["not_tempted"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.COMPLETE, actions["wander_full"]);
		machine.AddTransition(actions["idle_full"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["moving"], Command.TYPE.RESIDE, actions["residing"]);
		machine.AddTransition(actions["residing"], Command.TYPE.COMPLETE, actions["wander_full"]);
		machine.AddTransition(actions["start_wander"], Command.TYPE.WANDER, actions["wander_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.COMPLETE, actions["wander_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.TEMPT, actions["not_tempted"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.CLICKED, actions["wander_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.EXPAND, actions["wander_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.IDLE_PAUSE, actions["idle_full"]);
		machine.AddTransition(actions["wander_full"], Command.TYPE.GO_HOME, actions["go_home"]);
		machine.AddTransition(actions["not_tempted"], Command.TYPE.COMPLETE, actions["start_wander"]);
		machine.AddTransition(actions["wait_bonus"], Command.TYPE.CLICKED, actions["cheer_bonus"]);
		machine.AddTransition(actions["cheer_reward"], Command.TYPE.COMPLETE, actions["start_wander"]);
		machine.AddTransition(actions["cheer_bonus"], Command.TYPE.COMPLETE, actions["start_wander"]);
		machine.AddTransition(actions["go_home"], Command.TYPE.STORE_RESIDENT, actions["store_resident"]);
	}
}
