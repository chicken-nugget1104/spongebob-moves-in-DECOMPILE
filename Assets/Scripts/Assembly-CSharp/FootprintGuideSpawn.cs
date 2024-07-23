using System;
using UnityEngine;

// Token: 0x02000227 RID: 551
public class FootprintGuideSpawn : SessionActionSpawn
{
	// Token: 0x0600120D RID: 4621 RVA: 0x0007DFF8 File Offset: 0x0007C1F8
	public FootprintGuideSpawn()
	{
		if (FootprintGuideSpawn.template == null || FootprintGuideSpawn.template.IsDestroyed)
		{
			FootprintGuideSpawn.template = FootprintGuideSpawn.CreateTemplate();
		}
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0007E030 File Offset: 0x0007C230
	public void Spawn(Game game, SessionActionTracker parentAction, Vector3 position, float width, float height)
	{
		FootprintGuideSpawn footprintGuideSpawn = new FootprintGuideSpawn();
		footprintGuideSpawn.RegisterNewInstance(game, parentAction, position, width, height);
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x0007E050 File Offset: 0x0007C250
	protected void RegisterNewInstance(Game game, SessionActionTracker parentAction, Vector3 position, float width, float height)
	{
		base.RegisterNewInstance(game, parentAction);
		this.sprite = FootprintGuideSpawn.template;
		this.sprite.Resize(new Vector2(-0.5f * width, -0.5f * height), width, height);
		this.sprite.Position = position;
		this.sprite.Visible = true;
		this.sprite.Color = Simulated.COLOR_FOOTPRINT_FREE;
		this.sprite.OnUpdate(game.simulation.TheCamera, null);
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x0007E0D4 File Offset: 0x0007C2D4
	public override void Destroy()
	{
		if (this.sprite != null)
		{
			this.sprite.Visible = false;
			this.sprite = null;
		}
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x0007E0F4 File Offset: 0x0007C2F4
	private static BasicSprite CreateTemplate()
	{
		float num = 20f;
		float num2 = 20f;
		Vector2 center = new Vector2(-0.5f * num, -0.5f * num2);
		BasicSprite basicSprite = new BasicSprite("Materials/unique/footprint", null, center, num, num2);
		basicSprite.PublicInitialize();
		basicSprite.Name = "FootprintGuide";
		return basicSprite;
	}

	// Token: 0x04000C59 RID: 3161
	private const string MATERIAL = "Materials/unique/footprint";

	// Token: 0x04000C5A RID: 3162
	private static BasicSprite template = FootprintGuideSpawn.CreateTemplate();

	// Token: 0x04000C5B RID: 3163
	private BasicSprite sprite;
}
