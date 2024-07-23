using System;
using System.Collections.Generic;

// Token: 0x02000414 RID: 1044
public interface Entity
{
	// Token: 0x06001FF5 RID: 8181
	T GetDecorator<T>() where T : EntityDecorator;

	// Token: 0x06001FF6 RID: 8182
	bool HasDecorator<T>() where T : EntityDecorator;

	// Token: 0x06001FF7 RID: 8183
	void AddDecorator(Entity decorator);

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06001FF8 RID: 8184
	Identity Id { get; }

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06001FF9 RID: 8185
	int DefinitionId { get; }

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06001FFA RID: 8186
	EntityType AllTypes { get; }

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06001FFB RID: 8187
	EntityType Type { get; }

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06001FFC RID: 8188
	string BlueprintName { get; }

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06001FFD RID: 8189
	string Name { get; }

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06001FFE RID: 8190
	ReadOnlyIndexer Invariable { get; }

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06001FFF RID: 8191
	ReadWriteIndexer Variable { get; }

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06002000 RID: 8192
	string SoundOnTouch { get; }

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06002001 RID: 8193
	string SoundOnSelect { get; }

	// Token: 0x06002002 RID: 8194
	void PatchReferences(Game game);

	// Token: 0x06002003 RID: 8195
	void Serialize(ref Dictionary<string, object> data);

	// Token: 0x06002004 RID: 8196
	void Deserialize(Dictionary<string, object> data);

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06002005 RID: 8197
	Entity Core { get; }
}
