using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class ResidentEntity : EntityDecorator
{
	// Token: 0x0600219F RID: 8607 RVA: 0x000CF26C File Offset: 0x000CD46C
	public ResidentEntity(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x060021A0 RID: 8608 RVA: 0x000CF280 File Offset: 0x000CD480
	public override EntityType Type
	{
		get
		{
			if (this.Wanderer)
			{
				return EntityType.WANDERER;
			}
			return EntityType.RESIDENT;
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x060021A1 RID: 8609 RVA: 0x000CF294 File Offset: 0x000CD494
	public bool Disabled
	{
		get
		{
			return (bool)this.Invariable["disabled"];
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x060021A2 RID: 8610 RVA: 0x000CF2AC File Offset: 0x000CD4AC
	public float TimerDuration
	{
		get
		{
			return (float)this.Invariable["timer_duration"];
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x060021A3 RID: 8611 RVA: 0x000CF2C4 File Offset: 0x000CD4C4
	public RewardDefinition FavoriteReward
	{
		get
		{
			return (RewardDefinition)this.Invariable["favorite_reward"];
		}
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x060021A4 RID: 8612 RVA: 0x000CF2DC File Offset: 0x000CD4DC
	public RewardDefinition SatisfiedReward
	{
		get
		{
			return (RewardDefinition)this.Invariable["satisfaction_reward"];
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x060021A5 RID: 8613 RVA: 0x000CF2F4 File Offset: 0x000CD4F4
	// (set) Token: 0x060021A6 RID: 8614 RVA: 0x000CF30C File Offset: 0x000CD50C
	public ulong HungryAt
	{
		get
		{
			return (ulong)this.Variable["hungry_at"];
		}
		set
		{
			this.Variable["hungry_at"] = value;
		}
	}

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x060021A7 RID: 8615 RVA: 0x000CF324 File Offset: 0x000CD524
	// (set) Token: 0x060021A8 RID: 8616 RVA: 0x000CF33C File Offset: 0x000CD53C
	public ulong FullnessLength
	{
		get
		{
			return (ulong)this.Variable["fullness_length"];
		}
		set
		{
			this.Variable["fullness_length"] = value;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x060021A9 RID: 8617 RVA: 0x000CF354 File Offset: 0x000CD554
	// (set) Token: 0x060021AA RID: 8618 RVA: 0x000CF36C File Offset: 0x000CD56C
	public Cost FullnessRushCostFull
	{
		get
		{
			return (Cost)this.Variable["fullness_rush_cost"];
		}
		set
		{
			this.Variable["fullness_rush_cost"] = value;
		}
	}

	// Token: 0x060021AB RID: 8619 RVA: 0x000CF380 File Offset: 0x000CD580
	public Cost FullnessRushCostNow()
	{
		Cost cost = new Cost(this.FullnessRushCostFull);
		double num = this.HungryAt - TFUtils.EpochTime();
		float percentLeft = (float)(num / this.FullnessLength);
		cost.Prorate(percentLeft);
		return cost;
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x000CF3BC File Offset: 0x000CD5BC
	public float FullnessPercentage()
	{
		double num = this.HungryAt - TFUtils.EpochTime();
		return Mathf.Clamp01(1f - (float)(num / this.FullnessLength));
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x060021AD RID: 8621 RVA: 0x000CF3F0 File Offset: 0x000CD5F0
	// (set) Token: 0x060021AE RID: 8622 RVA: 0x000CF430 File Offset: 0x000CD630
	public Identity Residence
	{
		get
		{
			if (!this.Variable.ContainsKey("residence"))
			{
				return Identity.Null();
			}
			return (Identity)this.Variable["residence"];
		}
		set
		{
			this.Variable["residence"] = value;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x060021AF RID: 8623 RVA: 0x000CF444 File Offset: 0x000CD644
	// (set) Token: 0x060021B0 RID: 8624 RVA: 0x000CF48C File Offset: 0x000CD68C
	public int? HungerResourceId
	{
		get
		{
			return (!this.Variable.ContainsKey("wish_product_id")) ? null : ((int?)this.Variable["wish_product_id"]);
		}
		set
		{
			this.Variable["wish_product_id"] = value;
		}
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x060021B1 RID: 8625 RVA: 0x000CF4A4 File Offset: 0x000CD6A4
	// (set) Token: 0x060021B2 RID: 8626 RVA: 0x000CF4EC File Offset: 0x000CD6EC
	public int? PreviousResourceId
	{
		get
		{
			return (!this.Variable.ContainsKey("prev_wish_product_id")) ? null : ((int?)this.Variable["prev_wish_product_id"]);
		}
		set
		{
			this.Variable["prev_wish_product_id"] = value;
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x060021B3 RID: 8627 RVA: 0x000CF504 File Offset: 0x000CD704
	// (set) Token: 0x060021B4 RID: 8628 RVA: 0x000CF54C File Offset: 0x000CD74C
	public int? CostumeDID
	{
		get
		{
			return (!this.Variable.ContainsKey("costume_did")) ? null : ((int?)this.Variable["costume_did"]);
		}
		set
		{
			this.Variable["costume_did"] = value;
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x060021B5 RID: 8629 RVA: 0x000CF564 File Offset: 0x000CD764
	public int? DefaultCostumeDID
	{
		get
		{
			return (int?)this.Invariable["default_costume_did"];
		}
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x060021B6 RID: 8630 RVA: 0x000CF57C File Offset: 0x000CD77C
	public int WishTableDID
	{
		get
		{
			return (int)this.Invariable["wish_table_did"];
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x060021B7 RID: 8631 RVA: 0x000CF594 File Offset: 0x000CD794
	public int GrossItemsWishTableDID
	{
		get
		{
			return (int)this.Invariable["gross_items_wish_table_id"];
		}
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x060021B8 RID: 8632 RVA: 0x000CF5AC File Offset: 0x000CD7AC
	public int ForbiddenItemsWishTableDID
	{
		get
		{
			return (int)this.Invariable["forbidden_items_wish_table_id"];
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x060021B9 RID: 8633 RVA: 0x000CF5C4 File Offset: 0x000CD7C4
	// (set) Token: 0x060021BA RID: 8634 RVA: 0x000CF60C File Offset: 0x000CD80C
	public int? TemptedItemDID
	{
		get
		{
			return (!this.Variable.ContainsKey("product_id")) ? null : ((int?)this.Variable["product_id"]);
		}
		set
		{
			this.Variable["product_id"] = value;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x060021BB RID: 8635 RVA: 0x000CF624 File Offset: 0x000CD824
	public int WishCooldownMin
	{
		get
		{
			return (int)this.Invariable["wish_cooldown_min"];
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x060021BC RID: 8636 RVA: 0x000CF63C File Offset: 0x000CD83C
	public int WishCooldownMax
	{
		get
		{
			return (int)this.Invariable["wish_cooldown_max"];
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x060021BD RID: 8637 RVA: 0x000CF654 File Offset: 0x000CD854
	public int WishDuration
	{
		get
		{
			return (int)this.Invariable["wish_duration"];
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x060021BE RID: 8638 RVA: 0x000CF66C File Offset: 0x000CD86C
	// (set) Token: 0x060021BF RID: 8639 RVA: 0x000CF684 File Offset: 0x000CD884
	public ulong? WishExpiresAt
	{
		get
		{
			return (ulong?)this.Variable["wish_expires_at"];
		}
		set
		{
			this.Variable["wish_expires_at"] = value;
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x060021C0 RID: 8640 RVA: 0x000CF69C File Offset: 0x000CD89C
	// (set) Token: 0x060021C1 RID: 8641 RVA: 0x000CF6B4 File Offset: 0x000CD8B4
	public ulong? HideExpiresAt
	{
		get
		{
			return (ulong?)this.Variable["hide_expires_at"];
		}
		set
		{
			this.Variable["hide_expires_at"] = value;
		}
	}

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x060021C2 RID: 8642 RVA: 0x000CF6CC File Offset: 0x000CD8CC
	public int HideDuration
	{
		get
		{
			return (int)this.Invariable["hide_duration"];
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x060021C3 RID: 8643 RVA: 0x000CF6E4 File Offset: 0x000CD8E4
	public int AutoQuestIntro
	{
		get
		{
			if (this.Invariable.ContainsKey("auto_quest_intro"))
			{
				return (int)this.Invariable["auto_quest_intro"];
			}
			return -1;
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x060021C4 RID: 8644 RVA: 0x000CF720 File Offset: 0x000CD920
	public int AutoQuestOutro
	{
		get
		{
			if (this.Invariable.ContainsKey("auto_quest_outro"))
			{
				return (int)this.Invariable["auto_quest_outro"];
			}
			return -1;
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x060021C5 RID: 8645 RVA: 0x000CF75C File Offset: 0x000CD95C
	public string DialogPortrait
	{
		get
		{
			if (this.Invariable.ContainsKey("character_dialog_portrait"))
			{
				return (string)this.Invariable["character_dialog_portrait"];
			}
			return null;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x060021C6 RID: 8646 RVA: 0x000CF798 File Offset: 0x000CD998
	public string QuestReminderIcon
	{
		get
		{
			if (this.Invariable.ContainsKey("quest_reminder_icon"))
			{
				return (string)this.Invariable["quest_reminder_icon"];
			}
			return null;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x060021C7 RID: 8647 RVA: 0x000CF7D4 File Offset: 0x000CD9D4
	// (set) Token: 0x060021C8 RID: 8648 RVA: 0x000CF834 File Offset: 0x000CDA34
	public bool? DisableFlee
	{
		get
		{
			bool value = false;
			if (!this.Variable.ContainsKey("disable_flee"))
			{
				return new bool?(value);
			}
			bool? flag = (bool?)this.Variable["disable_flee"];
			if (flag != null)
			{
				return new bool?(flag.Value);
			}
			return new bool?(value);
		}
		set
		{
			this.Variable["disable_flee"] = value;
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x060021C9 RID: 8649 RVA: 0x000CF84C File Offset: 0x000CDA4C
	public bool? DisableIfWillFlee
	{
		get
		{
			bool value = false;
			if (!this.Invariable.ContainsKey("disable_if_will_flee"))
			{
				return new bool?(value);
			}
			bool? flag = (bool?)this.Invariable["disable_if_will_flee"];
			if (flag != null)
			{
				return new bool?(flag.Value);
			}
			return new bool?(value);
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x060021CA RID: 8650 RVA: 0x000CF8AC File Offset: 0x000CDAAC
	public bool? JoinPaytables
	{
		get
		{
			bool value = true;
			if (!this.Invariable.ContainsKey("join_paytables"))
			{
				return new bool?(value);
			}
			bool? flag = (bool?)this.Invariable["join_paytables"];
			if (flag != null)
			{
				return new bool?(flag.Value);
			}
			return new bool?(value);
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x060021CB RID: 8651 RVA: 0x000CF90C File Offset: 0x000CDB0C
	public List<uint> BonusPaytableIds
	{
		get
		{
			return (List<uint>)this.Invariable["match_bonus_paytables"];
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x060021CC RID: 8652 RVA: 0x000CF924 File Offset: 0x000CDB24
	// (set) Token: 0x060021CD RID: 8653 RVA: 0x000CF93C File Offset: 0x000CDB3C
	public Paytable[] BonusPaytables
	{
		get
		{
			return (Paytable[])this.Variable["bonus_paytable"];
		}
		set
		{
			this.Variable["bonus_paytable"] = value;
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x060021CE RID: 8654 RVA: 0x000CF950 File Offset: 0x000CDB50
	// (set) Token: 0x060021CF RID: 8655 RVA: 0x000CF968 File Offset: 0x000CDB68
	public Reward MatchBonus
	{
		get
		{
			return (Reward)this.Variable["match_bonus"];
		}
		set
		{
			this.Variable["match_bonus"] = value;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x060021D0 RID: 8656 RVA: 0x000CF97C File Offset: 0x000CDB7C
	// (set) Token: 0x060021D1 RID: 8657 RVA: 0x000CF9B8 File Offset: 0x000CDBB8
	public bool Wanderer
	{
		get
		{
			return this.Variable.ContainsKey("wanderer") && (bool)this.Variable["wanderer"];
		}
		set
		{
			this.Variable["wanderer"] = value;
		}
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000CF9D0 File Offset: 0x000CDBD0
	public static Dictionary<string, object> GetWandererGameState(Dictionary<string, object> gameState, Identity unitId)
	{
		return ResidentEntity.GetGameState(gameState, unitId, "wanderers");
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000CF9E0 File Offset: 0x000CDBE0
	public static Dictionary<string, object> GetWandererGameState(Dictionary<string, object> gameState, int did)
	{
		return ResidentEntity.GetGameState(gameState, did, "wanderers");
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000CF9F0 File Offset: 0x000CDBF0
	public static Dictionary<string, object> GetUnitGameState(Dictionary<string, object> gameState, Identity unitId)
	{
		return ResidentEntity.GetGameState(gameState, unitId, "units");
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000CFA00 File Offset: 0x000CDC00
	public static Dictionary<string, object> GetUnitGameState(Dictionary<string, object> gameState, int did)
	{
		return ResidentEntity.GetGameState(gameState, did, "units");
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000CFA10 File Offset: 0x000CDC10
	private static Dictionary<string, object> GetGameState(Dictionary<string, object> gameState, Identity unitId, string key)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])[key];
		string targetString = unitId.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		return (Dictionary<string, object>)list.Find(match);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000CFA68 File Offset: 0x000CDC68
	private static Dictionary<string, object> GetGameState(Dictionary<string, object> gameState, int did, string key)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])[key];
		Predicate<object> match = (object b) => TFUtils.LoadInt((Dictionary<string, object>)b, "did") == did;
		return (Dictionary<string, object>)list.Find(match);
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000CFABC File Offset: 0x000CDCBC
	public static void UpdateHungerTimeInGameState(Dictionary<string, object> gameState, Identity unitId, ulong hungerReadyTime)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, unitId);
		ResidentEntity.UpdateHungerTimeInUnitState(unitGameState, hungerReadyTime);
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000CFAD8 File Offset: 0x000CDCD8
	public static void UpdateHungerTimeInUnitState(Dictionary<string, object> unitState, ulong hungerReadyTime)
	{
		if (unitState.ContainsKey("feed_ready_time"))
		{
			unitState["feed_ready_time"] = hungerReadyTime;
		}
		else
		{
			unitState.Add("feed_ready_time", hungerReadyTime);
		}
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000CFB14 File Offset: 0x000CDD14
	public static void SetActiveStatusInUnitState(Dictionary<string, object> unitState, bool active)
	{
		unitState["active"] = active;
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x060021DB RID: 8667 RVA: 0x000CFB28 File Offset: 0x000CDD28
	// (set) Token: 0x060021DC RID: 8668 RVA: 0x000CFB30 File Offset: 0x000CDD30
	public RewardDefinition ForcedBonusReward
	{
		get
		{
			return this.forcedBonusReward;
		}
		set
		{
			this.forcedBonusReward = value;
		}
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000CFB3C File Offset: 0x000CDD3C
	public void StartCheckForIdle()
	{
		this.StartCheckForIdle((int)this.Invariable["idle.cooldown.min"], (int)this.Invariable["idle.cooldown.max"] + 1);
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000CFB7C File Offset: 0x000CDD7C
	public void StartCheckForIdle(int nDurationMin, int nDurationMax)
	{
		this.idleTimer = TFUtils.EpochTime();
		this.timeToNextIdle = UnityEngine.Random.Range(nDurationMin, nDurationMax);
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000CFB98 File Offset: 0x000CDD98
	public void StopCheckForIdle()
	{
		this.idleTimer = 0UL;
		this.timeToNextIdle = 0;
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000CFBAC File Offset: 0x000CDDAC
	public bool CheckForIdle()
	{
		if (this.idleTimer == 0UL || this.timeToNextIdle == 0)
		{
			return false;
		}
		ulong num = TFUtils.EpochTime() - this.idleTimer;
		return num >= (ulong)((long)this.timeToNextIdle);
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000CFBF0 File Offset: 0x000CDDF0
	public void ClearCheckForIdle()
	{
		this.idleTimer = TFUtils.EpochTime() - 1UL;
		this.timeToNextIdle = -1;
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x000CFC08 File Offset: 0x000CDE08
	public void StartCheckForResume()
	{
		this.StartCheckForResume((int)this.Invariable["idle.duration.min"], (int)this.Invariable["idle.duration.max"] + 1);
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000CFC48 File Offset: 0x000CDE48
	public void StartCheckForResume(int nDurationMin, int nDurationMax)
	{
		this.resumeTimer = TFUtils.EpochTime();
		this.timeToNextResume = UnityEngine.Random.Range(nDurationMin, nDurationMax);
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000CFC64 File Offset: 0x000CDE64
	public void StopCheckForResume()
	{
		this.resumeTimer = 0UL;
		this.timeToNextResume = 0;
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x000CFC78 File Offset: 0x000CDE78
	public bool CheckForResume()
	{
		if (this.resumeTimer == 0UL || this.timeToNextResume == 0)
		{
			return false;
		}
		ulong num = TFUtils.EpochTime() - this.resumeTimer;
		return num >= (ulong)((long)this.timeToNextResume);
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x060021E6 RID: 8678 RVA: 0x000CFCBC File Offset: 0x000CDEBC
	// (set) Token: 0x060021E7 RID: 8679 RVA: 0x000CFD00 File Offset: 0x000CDF00
	public bool HomeAvailability
	{
		get
		{
			return (!this.Invariable.ContainsKey("go_home_exempt") || !(bool)this.Invariable["go_home_exempt"]) && this.homeAvailability;
		}
		set
		{
			this.homeAvailability = value;
		}
	}

	// Token: 0x040014BB RID: 5307
	public const string HUNGRY_AT = "hungry_at";

	// Token: 0x040014BC RID: 5308
	public const string FULLNESS_LENGTH = "fullness_length";

	// Token: 0x040014BD RID: 5309
	public const string FULLNESS_RUSH_COST = "fullness_rush_cost";

	// Token: 0x040014BE RID: 5310
	public const string WISH_PRODUCT_ID = "wish_product_id";

	// Token: 0x040014BF RID: 5311
	public const string PREV_WISH_PRODUCT_ID = "prev_wish_product_id";

	// Token: 0x040014C0 RID: 5312
	public const string WISH_EXPIRES_AT = "wish_expires_at";

	// Token: 0x040014C1 RID: 5313
	public const string WISH_COOLDOWN_MIN = "wish_cooldown_min";

	// Token: 0x040014C2 RID: 5314
	public const string WISH_COOLDOWN_MAX = "wish_cooldown_max";

	// Token: 0x040014C3 RID: 5315
	public const string WISH_DURATION = "wish_duration";

	// Token: 0x040014C4 RID: 5316
	public const string LOADED_BONUS_PAYTABLES = "match_bonus_paytables";

	// Token: 0x040014C5 RID: 5317
	public const string MATCH_BONUS = "match_bonus";

	// Token: 0x040014C6 RID: 5318
	public const string HIDE_EXPIRES_AT = "hide_expires_at";

	// Token: 0x040014C7 RID: 5319
	public const string HIDE_DURATION = "hide_duration";

	// Token: 0x040014C8 RID: 5320
	public const string DISABLE_FLEE = "disable_flee";

	// Token: 0x040014C9 RID: 5321
	public const string DISABLE_IF_WILL_FLEE = "disable_if_will_flee";

	// Token: 0x040014CA RID: 5322
	public const string JOIN_PAYTABLES = "join_paytables";

	// Token: 0x040014CB RID: 5323
	public const string COSTUME_DID = "costume_did";

	// Token: 0x040014CC RID: 5324
	public const string DEFAULT_COSTUME_DID = "default_costume_did";

	// Token: 0x040014CD RID: 5325
	public const string GROSS_ITEM_ID = "gross_items_wish_table_id";

	// Token: 0x040014CE RID: 5326
	public const string FORBIDDEN_ITEM_ID = "forbidden_items_wish_table_id";

	// Token: 0x040014CF RID: 5327
	public const string TEMPTED_ITEM_ID = "tempted_item_id";

	// Token: 0x040014D0 RID: 5328
	private const string BONUS_PAYTABLE = "bonus_paytable";

	// Token: 0x040014D1 RID: 5329
	protected RewardDefinition forcedBonusReward;

	// Token: 0x040014D2 RID: 5330
	public Task m_pTask;

	// Token: 0x040014D3 RID: 5331
	public Vector2 m_pTaskTargetPosition = Vector2.zero;

	// Token: 0x040014D4 RID: 5332
	public bool m_bReachedTaskTarget;

	// Token: 0x040014D5 RID: 5333
	private ulong idleTimer;

	// Token: 0x040014D6 RID: 5334
	private int timeToNextIdle;

	// Token: 0x040014D7 RID: 5335
	private ulong resumeTimer;

	// Token: 0x040014D8 RID: 5336
	private int timeToNextResume;

	// Token: 0x040014D9 RID: 5337
	private bool homeAvailability;
}
