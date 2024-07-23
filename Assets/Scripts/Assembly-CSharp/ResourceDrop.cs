using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class ResourceDrop : ItemDrop
{
	// Token: 0x06000D2A RID: 3370 RVA: 0x00050F9C File Offset: 0x0004F19C
	public ResourceDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, int amount, Action callback) : base(position, fixedOffset, direction, definition, creationTime, callback)
	{
		this.amount = amount;
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x00050FB8 File Offset: 0x0004F1B8
	public static string MakeResourceKey(int did)
	{
		return "ItemDropReachedHud_" + did;
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00050FCC File Offset: 0x0004F1CC
	public override int Value
	{
		get
		{
			return this.amount;
		}
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x00050FD4 File Offset: 0x0004F1D4
	protected override void OnCollectionAnimationComplete(Session session)
	{
		int numQueuedDialogInputs = session.TheGame.dialogPackageManager.GetNumQueuedDialogInputs();
		string key = ResourceDrop.MakeResourceKey(this.definition.Did);
		int? num = (int?)session.CheckAsyncRequest(key);
		int num2 = (num == null) ? 0 : num.Value;
		num2 += this.Value;
		session.AddAsyncResponse(key, num2);
		ResourceManager resourceManager = session.TheGame.resourceManager;
		if (resourceManager.Resources[this.definition.Did].CollectedSound != null)
		{
			if (this.definition.Did == 3)
			{
				if (this.amount >= 8)
				{
					session.TheSoundEffectManager.PlaySound("coin_big");
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("coin_single");
				}
			}
			else
			{
				session.TheSoundEffectManager.PlaySound(resourceManager.Resources[this.definition.Did].CollectedSound);
			}
		}
		else
		{
			session.TheSoundEffectManager.PlaySound("ItemCollected");
		}
		List<Reward> list = null;
		IResourceProgressCalculator resourceCalculator = session.TheGame.simulation.resourceCalculatorManager.GetResourceCalculator(this.definition.Did);
		if (resourceCalculator != null)
		{
			resourceCalculator.GetRewardsForIncreasingResource(session.TheGame.simulation, resourceManager.Resources, this.amount, out list);
		}
		resourceManager.Add(this.definition.Did, this.amount, session.TheGame);
		if (list != null)
		{
			TFUtils.Assert(this.definition.Did == ResourceManager.XP, "The resource calculator derivative rewards only works for level-ups as a result of gaining XP! This should not be happening!");
			int num3 = 0;
			foreach (Reward reward in list)
			{
				resourceManager.Add(ResourceManager.LEVEL, 1, session.TheGame);
				if (reward != null)
				{
					session.TheGame.ApplyReward(reward, this.creationTime, false);
					int level = resourceManager.Query(ResourceManager.LEVEL);
					session.TheGame.ModifyGameState(new LevelUpAction(level, reward, TFUtils.EpochTime()));
				}
				num3++;
			}
			if (list.Count > 0)
			{
				LevelUpDialogInputData item = new LevelUpDialogInputData(resourceManager.Query(ResourceManager.LEVEL), list);
				session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
				{
					item
				}, uint.MaxValue);
				if (numQueuedDialogInputs == 0 && this.onCleanupComplete != null)
				{
					this.onCleanupComplete();
				}
			}
		}
		base.CleanUpRewardDropTapParticles(session);
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x000512A4 File Offset: 0x0004F4A4
	protected override void PlayTapAnimation(Session session)
	{
		if (this.isFlying)
		{
			return;
		}
		if (this.definition.Did == ResourceManager.SOFT_CURRENCY)
		{
			base.PlaySoftCurrencyDropTapParticles(session);
		}
		if (session.TheGame.resourceManager.Resources[this.definition.Did].TapSound != null)
		{
			session.TheSoundEffectManager.PlaySound(session.TheGame.resourceManager.Resources[this.definition.Did].TapSound);
		}
		else
		{
			session.TheSoundEffectManager.PlaySound("TapFallenItem");
		}
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x0005134C File Offset: 0x0004F54C
	protected override void PlayRewardAmountTextAnim(Session session)
	{
		if (session.TheState.GetType().Equals(typeof(Session.Playing)))
		{
			Session.Playing playing = (Session.Playing)session.TheState;
			Vector3 v = session.TheCamera.WorldPointToScreenPoint(this.position);
			v.x += (float)Screen.width * 0.0075f;
			v.y -= (float)Screen.height * 0.0075f;
			playing.DisappearingResourceAmount(v, this.amount);
		}
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x000513E4 File Offset: 0x0004F5E4
	public static Vector2 GetScreenCollectionDestination(int resourceDid)
	{
		if (resourceDid == ResourceManager.SOFT_CURRENCY)
		{
			return new Vector2((float)Screen.width * 0.912f, (float)Screen.height * 0.956f);
		}
		if (resourceDid == ResourceManager.HARD_CURRENCY)
		{
			return new Vector2((float)Screen.width * 0.912f, (float)Screen.height * 0.909f);
		}
		if (resourceDid == ResourceManager.XP)
		{
			return new Vector2((float)Screen.width * 0.3955f, (float)Screen.height * 0.9427f);
		}
		return new Vector2((float)Screen.width * 0.95f, (float)Screen.height * 0.82f);
	}

	// Token: 0x040008D9 RID: 2265
	private int amount;
}
