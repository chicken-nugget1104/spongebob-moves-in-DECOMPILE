using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
public abstract class ItemDropCtor
{
	// Token: 0x06000D02 RID: 3330 RVA: 0x000506F8 File Offset: 0x0004E8F8
	protected ItemDropCtor(ItemDropDefinition definition, ulong creationTime)
	{
		this.definition = definition;
		this.creationTime = creationTime;
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00050710 File Offset: 0x0004E910
	public ItemDropDefinition Definition
	{
		get
		{
			return this.definition;
		}
	}

	// Token: 0x06000D04 RID: 3332
	public abstract ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete);

	// Token: 0x040008C9 RID: 2249
	protected ItemDropDefinition definition;

	// Token: 0x040008CA RID: 2250
	protected ulong creationTime;
}
