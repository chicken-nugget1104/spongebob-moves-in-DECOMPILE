using System;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class ResourceDropCtor : ItemDropCtor
{
	// Token: 0x06000D31 RID: 3377 RVA: 0x0005148C File Offset: 0x0004F68C
	public ResourceDropCtor(ItemDropDefinition definition, int amount, ulong creationTime) : base(definition, creationTime)
	{
		this.amount = amount;
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000D32 RID: 3378 RVA: 0x000514A0 File Offset: 0x0004F6A0
	public int Amount
	{
		get
		{
			return this.amount;
		}
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x000514A8 File Offset: 0x0004F6A8
	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new ResourceDrop(position, fixedOffset, direction, this.definition, this.creationTime, this.amount, onCleanupComplete);
	}

	// Token: 0x040008DA RID: 2266
	private int amount;
}
