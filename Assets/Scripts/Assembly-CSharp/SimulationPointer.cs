using System;
using UnityEngine;

// Token: 0x02000251 RID: 593
public abstract class SimulationPointer : VisualSpawn
{
	// Token: 0x06001324 RID: 4900 RVA: 0x0008421C File Offset: 0x0008241C
	public void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale)
	{
		base.Initialize(game, action, offset, 0f, alpha, scale);
		this.bouncer = new JumpPattern(-200f, 12f, 0.25f, 0.18f, 0f, Time.time, Vector2.one);
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
		{
			this.sprite = new BasicSprite("Materials/lod/TutorialPointer_lr", null, Vector2.zero, 17f, 29f);
		}
		else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			this.sprite = new BasicSprite("Materials/lod/TutorialPointer_lr2", null, Vector2.zero, 17f, 29f);
		}
		else
		{
			this.sprite = new BasicSprite("Materials/lod/TutorialPointer", null, Vector2.zero, 17f, 29f);
		}
		this.sprite.PublicInitialize();
		this.sprite.Billboard(new BillboardDelegate(SBCamera.BillboardDefinition));
		TFUtils.Assert(base.Alpha == 1f, "Simulated Pointers do not support alpha blending yet. Needs to be implemented");
		base.NormalizeRotationAndPushToEdge(0f, 0f);
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00084334 File Offset: 0x00082534
	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale, Simulated parentSimulated)
	{
		this.simulated = parentSimulated;
		if (this.simulated != null)
		{
			this.simHandler = delegate()
			{
				if (action.Status != SessionActionTracker.StatusCode.OBLITERATED)
				{
					action.MarkSucceeded();
				}
			};
			this.simulated.AddClickListener(this.simHandler);
		}
		this.Initialize(game, action, offset, alpha, scale);
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x00084398 File Offset: 0x00082598
	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale, TerrainSlot slot)
	{
		this.slot = slot;
		if (this.slot != null)
		{
			this.slotHandler = delegate()
			{
				if (action.Status != SessionActionTracker.StatusCode.OBLITERATED)
				{
					action.MarkSucceeded();
				}
			};
			this.slot.AddClickListener(this.slotHandler);
		}
		this.Initialize(game, action, offset, alpha, scale);
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x000843FC File Offset: 0x000825FC
	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		float num;
		Vector2 vector;
		this.bouncer.ValueAndSquishAtTime(Time.time, out num, out vector);
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			this.sprite.Scale = TFUtils.ExpandVector(vector);
		}
		Vector3 b = new Vector3(0f, 0f, num + this.sprite.Height * this.sprite.Scale.y);
		this.sprite.Position = this.TargetPosition + b;
		this.sprite.OnUpdate(game.simulation.TheCamera, null);
		if (this.simulated != null && !this.simulated.Visible)
		{
			base.ParentAction.MarkFailed();
		}
		return base.OnUpdate(game);
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06001328 RID: 4904 RVA: 0x000844C8 File Offset: 0x000826C8
	public virtual Vector3 TargetPosition
	{
		get
		{
			return this.offset;
		}
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x000844D0 File Offset: 0x000826D0
	public override void Destroy()
	{
		if (this.simulated != null)
		{
			this.simulated.RemoveClickListener(this.simHandler);
		}
		if (this.slot != null)
		{
			this.slot.RemoveClickListener(this.slotHandler);
		}
		if (this.sprite != null)
		{
			this.sprite.Destroy();
		}
	}

	// Token: 0x04000D3B RID: 3387
	private Action simHandler;

	// Token: 0x04000D3C RID: 3388
	private Action slotHandler;

	// Token: 0x04000D3D RID: 3389
	protected TerrainSlot slot;

	// Token: 0x04000D3E RID: 3390
	protected Simulated simulated;

	// Token: 0x04000D3F RID: 3391
	private BasicSprite sprite;

	// Token: 0x04000D40 RID: 3392
	private JumpPattern bouncer;
}
