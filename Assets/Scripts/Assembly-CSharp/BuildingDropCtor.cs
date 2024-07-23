using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class BuildingDropCtor : ItemDropCtor
{
	// Token: 0x06000CE9 RID: 3305 RVA: 0x0004F9A8 File Offset: 0x0004DBA8
	public BuildingDropCtor(ItemDropDefinition definition, Identity id, ulong creationTime) : base(definition, creationTime)
	{
		this.id = id;
		this.definition.DisplayController.Scale = new Vector3(0.7f, 0.7f, 0.7f);
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x0004F9E8 File Offset: 0x0004DBE8
	public override ItemDrop CreateItemDrop(Vector3 position, Vector3 fixedOffset, Vector3 direction, Action onCleanupComplete)
	{
		return new BuildingDrop(position, fixedOffset, direction, this.definition, this.creationTime, this.id, onCleanupComplete);
	}

	// Token: 0x040008A3 RID: 2211
	private const float BUILDING_DROP_SCALE = 0.7f;

	// Token: 0x040008A4 RID: 2212
	private Identity id;
}
