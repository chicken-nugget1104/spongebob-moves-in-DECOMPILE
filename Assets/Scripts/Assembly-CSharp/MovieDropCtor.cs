using System;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class MovieDropCtor : ItemDropCtor
{
	// Token: 0x06000D20 RID: 3360 RVA: 0x00050E0C File Offset: 0x0004F00C
	public MovieDropCtor(ItemDropDefinition definition, ulong creationTime) : base(definition, creationTime)
	{
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x00050E18 File Offset: 0x0004F018
	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new MovieDrop(position, fixedOffset, direction, this.definition, this.creationTime, onCleanupComplete);
	}
}
