using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class RecipeDrop : ItemDrop
{
	// Token: 0x06000D22 RID: 3362 RVA: 0x00050E30 File Offset: 0x0004F030
	public RecipeDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, ItemDropDefinition definition, ulong creationTime, Action callback) : base(position, fixedOffset, direction, definition, creationTime, callback)
	{
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x00050E44 File Offset: 0x0004F044
	protected override void OnCollectionAnimationComplete(Session session)
	{
		session.TheSoundEffectManager.PlaySound("RecipeCollected");
		session.TheGame.craftManager.UnlockRecipe(this.definition.Did, session.TheGame);
		CraftingRecipe recipeById = session.TheGame.craftManager.GetRecipeById(this.definition.Did);
		string arg = Language.Get(recipeById.recipeName);
		string resourceTexture = session.TheGame.resourceManager.Resources[recipeById.productId].GetResourceTexture();
		FoundItemDialogInputData item = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), arg), resourceTexture, "Beat_FoundRecipe");
		session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
		{
			item
		}, uint.MaxValue);
		this.onCleanupComplete();
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x00050F28 File Offset: 0x0004F128
	protected override bool UpdateCleanup(Session session, Camera camera, bool updateCollectionTimer)
	{
		return base.ExplodeInPlace(session, camera, updateCollectionTimer, "Prefabs/FX/Fx_Glass_Break", "RecipeDropExplodeInPlace");
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x00050F40 File Offset: 0x0004F140
	protected override void PlayTapAnimation(Session session)
	{
		session.TheSoundEffectManager.PlaySound("TapFallenRecipeItem");
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x00050F54 File Offset: 0x0004F154
	protected override void PlayRewardAmountTextAnim(Session session)
	{
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x00050F58 File Offset: 0x0004F158
	public static Vector2 GetScreenCollectionDestination()
	{
		return new Vector2((float)Screen.width * 0.854f, (float)Screen.height * 0.073f);
	}

	// Token: 0x040008D8 RID: 2264
	private int id;
}
