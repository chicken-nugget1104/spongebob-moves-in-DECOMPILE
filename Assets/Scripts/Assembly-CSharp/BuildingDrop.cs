using System;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class BuildingDrop : ItemDrop
{
	// Token: 0x06000CE4 RID: 3300 RVA: 0x0004F844 File Offset: 0x0004DA44
	public BuildingDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Identity id, Action callback) : base(position, fixedOffset, direction, definition, creationTime, callback)
	{
		this.id = id;
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0004F860 File Offset: 0x0004DA60
	protected override void OnCollectionAnimationComplete(Session session)
	{
		session.TheSoundEffectManager.PlaySound("BuildingCollected");
		BuildingEntity decorator = session.TheGame.entities.Create(EntityType.BUILDING, this.definition.Did, this.id, true).GetDecorator<BuildingEntity>();
		decorator.GetDecorator<ErectableDecorator>().ErectionCompleteTime = new ulong?(this.creationTime);
		decorator.GetDecorator<ActivatableDecorator>().Activated = this.creationTime;
		session.TheGame.inventory.AddItem(decorator, null);
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0004F8E4 File Offset: 0x0004DAE4
	protected override void PlayTapAnimation(Session session)
	{
		session.TheSoundEffectManager.PlaySound("TapFallenBuildingItem");
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0004F8F8 File Offset: 0x0004DAF8
	protected override void PlayRewardAmountTextAnim(Session session)
	{
		if (session.TheState.GetType().Equals(typeof(Session.Playing)))
		{
			Session.Playing playing = (Session.Playing)session.TheState;
			Vector3 v = session.TheCamera.WorldPointToScreenPoint(this.position);
			v.x += (float)Screen.width * 0.0075f;
			v.y -= (float)Screen.height * 0.0075f;
			playing.DisappearingResourceAmount(v, 1);
		}
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0004F988 File Offset: 0x0004DB88
	public static Vector2 GetScreenCollectionDestination()
	{
		return new Vector2((float)Screen.width * 0.854f, (float)Screen.height * 0.073f);
	}

	// Token: 0x040008A2 RID: 2210
	private Identity id;
}
