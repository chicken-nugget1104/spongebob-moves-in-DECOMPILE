using System;
using UnityEngine;

// Token: 0x02000179 RID: 377
public abstract class ItemDrop
{
	// Token: 0x06000CEB RID: 3307 RVA: 0x0004FA08 File Offset: 0x0004DC08
	protected ItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action onCleanupComplete)
	{
		this.definition = definition;
		this.position = position;
		this.fixedOffset = fixedOffset;
		this.velocity = direction * this.initialSpeed;
		this.isFlying = false;
		this.velocity.z = this.velocity.z + 150f;
		this.creationTime = creationTime;
		this.onCleanupComplete = onCleanupComplete;
		this.dropID = new Identity();
		BasicSprite basicSprite = (BasicSprite)this.definition.DisplayController;
		Vector3 vector;
		basicSprite.Rotation.ToAngleAxis(out this.startingAngle, out vector);
		if (direction.x < direction.y)
		{
			this.dropToTheRight = true;
		}
		this.autoCollectLock = definition.ForceTapToCollect;
		float time = Time.time;
		if (this.autoCollectLock)
		{
			this.rewardBouncer = new JumpPattern(UnityEngine.Random.Range(-150f, -200f), UnityEngine.Random.Range(15f, 20f), 0.25f, UnityEngine.Random.Range(0.2f, 0.3f), 0f, time, Vector2.one);
		}
		else
		{
			this.rewardBouncer = new JumpPattern(UnityEngine.Random.Range(-125f, -150f), UnityEngine.Random.Range(2f, 7f), 0.25f, UnityEngine.Random.Range(0.2f, 0.3f), 0f, time, Vector2.one);
		}
		if (SBSettings.DebugDisplayControllers)
		{
			float width = this.definition.DisplayController.HitObject.Width;
			float height = this.definition.DisplayController.HitObject.Height;
			Vector2 center = new Vector2(-0.5f * width, -0.5f * height);
			BasicSprite basicSprite2 = new BasicSprite("Materials/unique/footprint", null, center, width, height, new QuadHitObject(center, width, height));
			basicSprite2.PublicInitialize();
			basicSprite2.Name = "RewardDropDebug_" + this.dropID;
			basicSprite2.Visible = true;
			basicSprite2.Color = Color.blue;
			basicSprite2.Alpha = 0.2f;
			basicSprite2.Resize(this.definition.DisplayController.HitObject.Center, this.definition.DisplayController.HitObject.Width, this.definition.DisplayController.HitObject.Height);
			this.debugDisplayController = basicSprite2;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0004FCC4 File Offset: 0x0004DEC4
	public virtual int Value
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000CED RID: 3309 RVA: 0x0004FCC8 File Offset: 0x0004DEC8
	// (set) Token: 0x06000CEE RID: 3310 RVA: 0x0004FCD0 File Offset: 0x0004DED0
	public virtual Vector3 Position
	{
		get
		{
			return this.position;
		}
		set
		{
			this.position = value;
		}
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0004FCDC File Offset: 0x0004DEDC
	public void Pickup()
	{
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			BasicSprite basicSprite = (BasicSprite)this.definition.DisplayController;
			Vector3 scale = basicSprite.Scale;
			scale.x = 1f;
			scale.y = 1f;
			basicSprite.Scale = scale;
		}
		this.cleanupTimerStarted = true;
		this.cleanupTime = Time.time - 1f;
		this.rewardDropTapParticleSystemRequestDelegate = new ItemDrop.RewardDropTapParticleSystemRequestDelegate(this);
		this.autoCollectLock = false;
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0004FD58 File Offset: 0x0004DF58
	public void AutoPickup()
	{
		if (!this.autoCollectLock)
		{
			this.Pickup();
		}
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x0004FD6C File Offset: 0x0004DF6C
	public bool HandleTap(Session session, Ray ray)
	{
		if (this.cleanupTimerStarted && this.definition.DisplayController.Intersects(ray))
		{
			this.Pickup();
			this.PlayTapAnimation(session);
			return true;
		}
		return false;
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0004FDAC File Offset: 0x0004DFAC
	public void PlaySoftCurrencyDropTapParticles(Session session)
	{
		session.TheGame.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Coin_Shower", 0, 0, 0f, this.rewardDropTapParticleSystemRequestDelegate);
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x0004FDE4 File Offset: 0x0004DFE4
	public void CleanUpRewardDropTapParticles(Session session)
	{
		session.TheGame.simulation.particleSystemManager.RemoveRequestWithDelegate(this.rewardDropTapParticleSystemRequestDelegate);
	}

	// Token: 0x06000CF4 RID: 3316
	protected abstract void OnCollectionAnimationComplete(Session session);

	// Token: 0x06000CF5 RID: 3317
	protected abstract void PlayRewardAmountTextAnim(Session session);

	// Token: 0x06000CF6 RID: 3318
	protected abstract void PlayTapAnimation(Session session);

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0004FE04 File Offset: 0x0004E004
	public Identity DropID
	{
		get
		{
			return this.dropID;
		}
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x0004FE0C File Offset: 0x0004E00C
	public bool OnUpdate(Session session, Camera camera, bool updateCollectionTimer)
	{
		BasicSprite basicSprite = (BasicSprite)this.definition.DisplayController;
		bool flag2;
		if (this.cleanupTimerStarted)
		{
			bool flag = this.UpdateCleanup(session, camera, updateCollectionTimer);
			flag2 = !flag;
			if (flag)
			{
				this.definition.DisplayController.Visible = false;
				RewardManager.ReleaseDisplayController(session.TheGame.simulation, this.definition.DisplayController);
				if (this.debugDisplayController != null)
				{
					this.debugDisplayController.Destroy();
				}
			}
		}
		else
		{
			flag2 = true;
			if (this.numLandings < 4)
			{
				float num;
				Vector3 vector;
				basicSprite.Rotation.ToAngleAxis(out num, out vector);
				float num2 = this.startingAngle - num;
				if (this.rotatingOnDrop)
				{
					if (this.dropToTheRight)
					{
						basicSprite.Rotate(new Vector3(0f, 0f, this.rotationSpeedForDrop * Time.deltaTime));
					}
					else
					{
						basicSprite.Rotate(new Vector3(0f, 0f, -this.rotationSpeedForDrop * Time.deltaTime));
					}
				}
				if (num2 < 5f && this.numLandings >= 2 && this.rotatingOnDrop)
				{
					basicSprite.ResetRotation();
					this.rotatingOnDrop = false;
				}
				this.velocity.z = this.velocity.z - 800f * Time.deltaTime;
				Vector3 a = this.position;
				a += this.velocity * Time.deltaTime;
				if (a.z < 0f)
				{
					a.z = 0f;
				}
				if (a.z == 0f)
				{
					this.rotationSpeedForDrop -= this.rotationSpeedForDrop * 0.1f * Time.deltaTime;
					this.numLandings++;
					this.velocity.z = this.velocity.z * -this.landingDampeningFactor;
					if (this.definition.Did == 5)
					{
						session.TheSoundEffectManager.PlaySound("happiness_star_bounce");
					}
					else if (this.definition.Did == 1 || this.definition.Did == 2 || this.definition.Did == 3)
					{
						session.TheSoundEffectManager.PlaySound("ItemDropBounce");
					}
					else
					{
						session.TheSoundEffectManager.PlaySound("special_item_bounce");
					}
				}
				this.position = a;
				this.definition.DisplayController.Position = this.position + this.fixedOffset;
			}
			else
			{
				basicSprite.ResetRotation();
				this.StartCleanupTimer(camera);
				if (this.debugDisplayController != null)
				{
					this.debugDisplayController.OnUpdate(camera, null);
					this.debugDisplayController.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
					this.debugDisplayController.Position = this.definition.DisplayController.Position;
				}
			}
		}
		if (!flag2)
		{
			this.OnCollectionAnimationComplete(session);
			session.TheGame.dropManager.ExecutePickupTrigger(session.TheGame, this.dropID);
		}
		return flag2;
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x00050130 File Offset: 0x0004E330
	private void StartCleanupTimer(Camera camera)
	{
		if (!this.cleanupTimerStarted)
		{
			this.cleanupTimerStarted = true;
			Vector3 b = camera.WorldToScreenPoint(this.position);
			Vector3 vector = new Vector3(this.definition.CleanupScreenDestination.x, this.definition.CleanupScreenDestination.y, 0f) - b;
			this.velocity = new Vector3(vector.x, vector.y, 0f);
			this.velocity.Normalize();
			this.velocity *= 15f;
			float time = Time.time;
			this.cleanupTime = time + 4f;
		}
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x000501E8 File Offset: 0x0004E3E8
	protected virtual bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		float time = Time.time;
		if (updateCollectionTimer)
		{
			if (time >= this.cleanupTime && !this.autoCollectLock)
			{
				Vector3 vector = camera.WorldToScreenPoint(this.position);
				Vector3 vector2 = new Vector3(this.definition.CleanupScreenDestination.x, this.definition.CleanupScreenDestination.y, 0f) - vector;
				this.velocity = new Vector3(vector2.x, vector2.y, 0f);
				this.velocity.Normalize();
				this.velocity *= 15f;
				Vector3 a = vector;
				a += this.velocity;
				this.position = camera.ScreenToWorldPoint(a);
				this.definition.DisplayController.Position = this.position + this.fixedOffset;
				BasicSprite basicSprite = (BasicSprite)this.definition.DisplayController;
				this.isFlying = true;
				if (this.velocity.x < this.velocity.y)
				{
					basicSprite.Rotate(new Vector3(0f, 0f, (float)(-(float)this.rotationSpeedForCollect)));
				}
				else
				{
					basicSprite.Rotate(new Vector3(0f, 0f, (float)this.rotationSpeedForCollect));
				}
				bool flag = Mathf.Abs(a.x - this.definition.CleanupScreenDestination.x) <= 30f && Mathf.Abs(a.y - this.definition.CleanupScreenDestination.y) <= 30f;
				bool flag2 = a.x < 0f || a.y < 0f || a.x > camera.pixelWidth || a.y > camera.pixelHeight;
				if (!this.playedRewardAmountTextAnim)
				{
					this.PlayRewardAmountTextAnim(session);
				}
				this.playedRewardAmountTextAnim = true;
				return flag || flag2;
			}
			this.BounceReward(camera, Time.time, this.rewardBouncer);
		}
		return false;
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x0005042C File Offset: 0x0004E62C
	protected void BounceReward(Camera camera, float seconds, JumpPattern bouncer)
	{
		float d = 0f;
		float num;
		Vector2 vector;
		bouncer.ValueAndSquishAtTime(seconds, out num, out vector);
		BasicSprite basicSprite = (BasicSprite)this.definition.DisplayController;
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			basicSprite.Scale = TFUtils.ExpandVector(vector);
		}
		if (vector.y < 1f)
		{
			this.didStartJumping = true;
		}
		if (this.didStartJumping)
		{
			float num2 = (vector.y - 1f) * basicSprite.Height;
			d = num + num2;
		}
		basicSprite.Position = this.position + this.fixedOffset + basicSprite.Up * d;
		this.isFlying = false;
		basicSprite.OnUpdate(camera, null);
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x000504F0 File Offset: 0x0004E6F0
	private void StartPopTimer()
	{
		if (!this.popTimerStarted)
		{
			this.popTimerStarted = true;
			float time = Time.time;
			this.popTime = time + 1f;
		}
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x00050524 File Offset: 0x0004E724
	protected bool ExplodeInPlace(Session session, Camera camera, bool updateCollectionTimer, string particleFX, string soundName)
	{
		float time = Time.time;
		if (updateCollectionTimer)
		{
			if (time >= this.cleanupTime && !this.autoCollectLock)
			{
				BasicSprite basicSprite = (BasicSprite)this.definition.DisplayController;
				if (!TFPerfUtils.IsNonScalingDevice())
				{
					Vector3 scale = basicSprite.Scale;
					scale.x += 0.1f;
					scale.y += 0.1f;
					basicSprite.Scale = scale;
				}
				basicSprite.Alpha -= 0.1f;
				if (basicSprite.Alpha <= 0f && !this.popTimerStarted)
				{
					basicSprite.Alpha = 0f;
					this.StartPopTimer();
					session.TheSoundEffectManager.PlaySound(soundName);
					session.TheGame.simulation.particleSystemManager.RequestParticles(particleFX, 0, 0, 0f, this.rewardDropTapParticleSystemRequestDelegate);
					RestrictInteraction.AddWhitelistSimulated(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.AddWhitelistExpansion(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
				}
				else if (this.popTimerStarted && time >= this.popTime)
				{
					RestrictInteraction.RemoveWhitelistSimulated(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.RemoveWhitelistExpansion(session.TheGame.simulation, int.MinValue);
					RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
					return true;
				}
			}
			else
			{
				this.BounceReward(camera, Time.time, this.rewardBouncer);
			}
		}
		return false;
	}

	// Token: 0x040008A5 RID: 2213
	private const float CLEANUP_DELAY = 4f;

	// Token: 0x040008A6 RID: 2214
	private const float CLEANUP_SPEED = 15f;

	// Token: 0x040008A7 RID: 2215
	private const float DROP_GRAVITY = 800f;

	// Token: 0x040008A8 RID: 2216
	private const float CLEANUP_TOLERANCE = 30f;

	// Token: 0x040008A9 RID: 2217
	private const float ROTATION_TOLERANCE = 5f;

	// Token: 0x040008AA RID: 2218
	private const int MAX_LANDINGS = 4;

	// Token: 0x040008AB RID: 2219
	private const float POP_DELAY = 1f;

	// Token: 0x040008AC RID: 2220
	private const float POP_ALPHA_RATE = 0.1f;

	// Token: 0x040008AD RID: 2221
	private const float POP_SCALE_RATE = 0.1f;

	// Token: 0x040008AE RID: 2222
	public ItemDropDefinition definition;

	// Token: 0x040008AF RID: 2223
	protected ulong creationTime;

	// Token: 0x040008B0 RID: 2224
	protected Vector3 position;

	// Token: 0x040008B1 RID: 2225
	protected float cleanupTime;

	// Token: 0x040008B2 RID: 2226
	protected float popTime;

	// Token: 0x040008B3 RID: 2227
	protected Action onCleanupComplete;

	// Token: 0x040008B4 RID: 2228
	protected bool autoCollectLock = true;

	// Token: 0x040008B5 RID: 2229
	protected bool isFlying;

	// Token: 0x040008B6 RID: 2230
	private Vector3 fixedOffset;

	// Token: 0x040008B7 RID: 2231
	private Vector3 velocity;

	// Token: 0x040008B8 RID: 2232
	private bool dropToTheRight;

	// Token: 0x040008B9 RID: 2233
	private bool cleanupTimerStarted;

	// Token: 0x040008BA RID: 2234
	private bool popTimerStarted;

	// Token: 0x040008BB RID: 2235
	private int numLandings;

	// Token: 0x040008BC RID: 2236
	private Identity dropID;

	// Token: 0x040008BD RID: 2237
	private float initialSpeed = UnityEngine.Random.Range(50f, 70f);

	// Token: 0x040008BE RID: 2238
	private float landingDampeningFactor = UnityEngine.Random.Range(0.4f, 0.65f);

	// Token: 0x040008BF RID: 2239
	private ItemDrop.RewardDropTapParticleSystemRequestDelegate rewardDropTapParticleSystemRequestDelegate;

	// Token: 0x040008C0 RID: 2240
	private float rotationSpeedForDrop = UnityEngine.Random.Range(300f, 600f);

	// Token: 0x040008C1 RID: 2241
	private int rotationSpeedForCollect = UnityEngine.Random.Range(5, 20);

	// Token: 0x040008C2 RID: 2242
	private float startingAngle;

	// Token: 0x040008C3 RID: 2243
	private bool rotatingOnDrop = true;

	// Token: 0x040008C4 RID: 2244
	private BasicSprite debugDisplayController;

	// Token: 0x040008C5 RID: 2245
	private bool playedRewardAmountTextAnim;

	// Token: 0x040008C6 RID: 2246
	private JumpPattern rewardBouncer;

	// Token: 0x040008C7 RID: 2247
	private bool didStartJumping;

	// Token: 0x0200017A RID: 378
	private class RewardDropTapParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x000506B4 File Offset: 0x0004E8B4
		public RewardDropTapParticleSystemRequestDelegate(ItemDrop item)
		{
			this.item = item;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x000506C4 File Offset: 0x0004E8C4
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x000506C8 File Offset: 0x0004E8C8
		public Vector3 Position
		{
			get
			{
				BasicSprite basicSprite = (BasicSprite)this.item.definition.DisplayController;
				return basicSprite.Position;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x000506F4 File Offset: 0x0004E8F4
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x040008C8 RID: 2248
		protected ItemDrop item;
	}
}
