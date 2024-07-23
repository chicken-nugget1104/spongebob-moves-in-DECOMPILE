using System;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class RecipeDropCtor : ItemDropCtor
{
	// Token: 0x06000D28 RID: 3368 RVA: 0x00050F78 File Offset: 0x0004F178
	public RecipeDropCtor(ItemDropDefinition definition, ulong creationTime) : base(definition, creationTime)
	{
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x00050F84 File Offset: 0x0004F184
	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new RecipeDrop(position, fixedOffset, direction, this.definition, this.creationTime, onCleanupComplete);
	}
}
