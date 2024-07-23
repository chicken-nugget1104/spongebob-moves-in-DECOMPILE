using System;
using System.Collections.Generic;

// Token: 0x0200040A RID: 1034
public class CoreEntity : Entity
{
	// Token: 0x06001F9F RID: 8095 RVA: 0x000C1DDC File Offset: 0x000BFFDC
	public CoreEntity(Identity id, Blueprint blueprint)
	{
		this.id = id;
		this.iproperties = blueprint.InvariableProperties();
		this.iindexer = new ReadOnlyIndexer(this.iproperties);
		this.did = (int)this.Invariable["did"];
		this.vproperties = blueprint.VariableProperties();
		this.vindexer = new ReadWriteIndexer(this.vproperties);
		this.decorators = new Dictionary<Type, Entity>();
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x000C1E58 File Offset: 0x000C0058
	public Identity Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x000C1E60 File Offset: 0x000C0060
	public int DefinitionId
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x000C1E68 File Offset: 0x000C0068
	public string BlueprintName
	{
		get
		{
			return (string)this.Invariable["blueprint"];
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x000C1E80 File Offset: 0x000C0080
	public string Name
	{
		get
		{
			return (string)this.Invariable["name"];
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x000C1E98 File Offset: 0x000C0098
	public ReadOnlyIndexer Invariable
	{
		get
		{
			return this.iindexer;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x000C1EA0 File Offset: 0x000C00A0
	public ReadWriteIndexer Variable
	{
		get
		{
			return this.vindexer;
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x000C1EA8 File Offset: 0x000C00A8
	public string SoundOnSelect
	{
		get
		{
			return (string)this.Invariable["sound_on_select"];
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x000C1EC0 File Offset: 0x000C00C0
	public string SoundOnTouch
	{
		get
		{
			return (string)this.Invariable["sound_on_touch"];
		}
	}

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000C1ED8 File Offset: 0x000C00D8
	public Entity Core
	{
		get
		{
			return this;
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x000C1EDC File Offset: 0x000C00DC
	public EntityType Type
	{
		get
		{
			return EntityType.CORE;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06001FAA RID: 8106 RVA: 0x000C1EE0 File Offset: 0x000C00E0
	public EntityType AllTypes
	{
		get
		{
			EntityType entityType = this.Type;
			foreach (Entity entity in this.decorators.Values)
			{
				EntityDecorator entityDecorator = (EntityDecorator)entity;
				entityType |= entityDecorator.Type;
			}
			return entityType;
		}
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x000C1F5C File Offset: 0x000C015C
	public void AddDecorator(Entity decorator)
	{
		if (!this.decorators.ContainsKey(decorator.GetType()))
		{
			this.decorators.Add(decorator.GetType(), decorator);
		}
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x000C1F94 File Offset: 0x000C0194
	public T GetDecorator<T>() where T : EntityDecorator
	{
		Type typeFromHandle = typeof(T);
		Entity entity = null;
		if (this.decorators.TryGetValue(typeFromHandle, out entity))
		{
			return entity as T;
		}
		TFUtils.ErrorLog("Could not find Entity decorator of type " + typeFromHandle.ToString());
		return (T)((object)Activator.CreateInstance(typeFromHandle, new object[]
		{
			this
		}));
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000C1FF8 File Offset: 0x000C01F8
	public bool HasDecorator<T>() where T : EntityDecorator
	{
		return this.decorators.ContainsKey(typeof(T));
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000C2010 File Offset: 0x000C0210
	public virtual void PatchReferences(Game game)
	{
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x000C2014 File Offset: 0x000C0214
	public void Serialize(ref Dictionary<string, object> data)
	{
		foreach (Entity entity in this.decorators.Values)
		{
			EntityDecorator entityDecorator = (EntityDecorator)entity;
			entityDecorator.SerializeDecorator(ref data);
		}
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000C2084 File Offset: 0x000C0284
	public void Deserialize(Dictionary<string, object> data)
	{
		foreach (Entity entity in this.decorators.Values)
		{
			EntityDecorator entityDecorator = (EntityDecorator)entity;
			entityDecorator.DeserializeDecorator(data);
		}
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000C20F4 File Offset: 0x000C02F4
	public static Type TypeFromString(string typeStr)
	{
		return typeof(CoreEntity);
	}

	// Token: 0x040013B6 RID: 5046
	public const string REQUEST_INTERFACE = "RequestEntityInterface";

	// Token: 0x040013B7 RID: 5047
	private Identity id;

	// Token: 0x040013B8 RID: 5048
	protected Dictionary<string, object> iproperties;

	// Token: 0x040013B9 RID: 5049
	protected ReadOnlyIndexer iindexer;

	// Token: 0x040013BA RID: 5050
	protected Dictionary<string, object> vproperties;

	// Token: 0x040013BB RID: 5051
	protected ReadWriteIndexer vindexer;

	// Token: 0x040013BC RID: 5052
	private Dictionary<Type, Entity> decorators;

	// Token: 0x040013BD RID: 5053
	private int did;
}
