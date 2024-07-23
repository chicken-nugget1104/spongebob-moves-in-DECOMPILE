using System;
using System.Collections.Generic;

// Token: 0x02000415 RID: 1045
public abstract class EntityDecorator : Entity
{
	// Token: 0x06002006 RID: 8198 RVA: 0x000C2A10 File Offset: 0x000C0C10
	public EntityDecorator(Entity toDecorate)
	{
		this.core = toDecorate.Core;
		toDecorate.AddDecorator(this);
		TFUtils.Assert(this.core != null, "checking that we are not null");
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x000C2A4C File Offset: 0x000C0C4C
	public void AddDecorator(Entity entity)
	{
		if (entity is CoreEntity)
		{
			this.core = entity;
		}
		else
		{
			TFUtils.Assert(this.core != null, "CoreEntity is not set yet");
			this.core.AddDecorator(entity);
		}
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06002008 RID: 8200 RVA: 0x000C2A88 File Offset: 0x000C0C88
	public Identity Id
	{
		get
		{
			return this.core.Id;
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06002009 RID: 8201 RVA: 0x000C2A98 File Offset: 0x000C0C98
	public int DefinitionId
	{
		get
		{
			return this.core.DefinitionId;
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x0600200A RID: 8202 RVA: 0x000C2AA8 File Offset: 0x000C0CA8
	public EntityType AllTypes
	{
		get
		{
			return this.core.AllTypes;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x0600200B RID: 8203 RVA: 0x000C2AB8 File Offset: 0x000C0CB8
	public virtual EntityType Type
	{
		get
		{
			return this.core.Type;
		}
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x0600200C RID: 8204 RVA: 0x000C2AC8 File Offset: 0x000C0CC8
	public string BlueprintName
	{
		get
		{
			return this.core.BlueprintName;
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x0600200D RID: 8205 RVA: 0x000C2AD8 File Offset: 0x000C0CD8
	public string Name
	{
		get
		{
			return this.core.Name;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x0600200E RID: 8206 RVA: 0x000C2AE8 File Offset: 0x000C0CE8
	public ReadOnlyIndexer Invariable
	{
		get
		{
			return this.core.Invariable;
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x0600200F RID: 8207 RVA: 0x000C2AF8 File Offset: 0x000C0CF8
	public ReadWriteIndexer Variable
	{
		get
		{
			return this.core.Variable;
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06002010 RID: 8208 RVA: 0x000C2B08 File Offset: 0x000C0D08
	public virtual string SoundOnTouch
	{
		get
		{
			return this.core.SoundOnTouch;
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06002011 RID: 8209 RVA: 0x000C2B18 File Offset: 0x000C0D18
	public virtual string SoundOnSelect
	{
		get
		{
			return this.core.SoundOnSelect;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06002012 RID: 8210 RVA: 0x000C2B28 File Offset: 0x000C0D28
	public Entity Core
	{
		get
		{
			return this.core;
		}
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000C2B30 File Offset: 0x000C0D30
	public T GetDecorator<T>() where T : EntityDecorator
	{
		return this.core.GetDecorator<T>();
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x000C2B40 File Offset: 0x000C0D40
	public bool HasDecorator<T>() where T : EntityDecorator
	{
		return this.core.HasDecorator<T>();
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000C2B50 File Offset: 0x000C0D50
	public virtual void SerializeDecorator(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x000C2B54 File Offset: 0x000C0D54
	public virtual void DeserializeDecorator(Dictionary<string, object> data)
	{
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x000C2B58 File Offset: 0x000C0D58
	public void Serialize(ref Dictionary<string, object> data)
	{
		this.core.Serialize(ref data);
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000C2B68 File Offset: 0x000C0D68
	public void Deserialize(Dictionary<string, object> data)
	{
		this.core.Deserialize(data);
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000C2B78 File Offset: 0x000C0D78
	public virtual void PatchReferences(Game game)
	{
		this.core.PatchReferences(game);
	}

	// Token: 0x040013C0 RID: 5056
	protected Entity core;
}
