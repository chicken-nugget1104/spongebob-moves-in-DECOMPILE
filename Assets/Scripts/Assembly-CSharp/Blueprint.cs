using System;
using System.Collections.Generic;

// Token: 0x02000408 RID: 1032
public class Blueprint
{
	// Token: 0x06001F92 RID: 8082 RVA: 0x000C1B54 File Offset: 0x000BFD54
	public Blueprint()
	{
		this.Disabled = false;
		this.iproperties = new Dictionary<string, object>();
		this.iindexer = new ReadWriteIndexer(this.iproperties);
		this.vproperties = new Dictionary<string, object>();
		this.vindexer = new ReadWriteIndexer(this.vproperties);
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06001F93 RID: 8083 RVA: 0x000C1BA8 File Offset: 0x000BFDA8
	public ReadWriteIndexer Invariable
	{
		get
		{
			return this.iindexer;
		}
	}

	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06001F94 RID: 8084 RVA: 0x000C1BB0 File Offset: 0x000BFDB0
	public ReadWriteIndexer Variable
	{
		get
		{
			return this.vindexer;
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000C1BB8 File Offset: 0x000BFDB8
	public EntityType PrimaryType
	{
		get
		{
			return (EntityType)((int)this.Invariable["type"]);
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06001F96 RID: 8086 RVA: 0x000C1BD0 File Offset: 0x000BFDD0
	// (set) Token: 0x06001F97 RID: 8087 RVA: 0x000C1BD8 File Offset: 0x000BFDD8
	public bool Disabled { get; set; }

	// Token: 0x06001F98 RID: 8088 RVA: 0x000C1BE4 File Offset: 0x000BFDE4
	public int? GetInstanceLimitByLevel(int level)
	{
		if (this.Invariable["instance_limit"] == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = (Dictionary<int, int>)this.Invariable["instance_limit"];
		for (int i = level; i >= 0; i--)
		{
			if (dictionary.ContainsKey(i))
			{
				return new int?(dictionary[i]);
			}
		}
		return null;
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x000C1C5C File Offset: 0x000BFE5C
	public Dictionary<string, object> InvariableProperties()
	{
		return this.iproperties;
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x000C1C64 File Offset: 0x000BFE64
	public Dictionary<string, object> VariableProperties()
	{
		return TFUtils.CloneDictionary<string, object>(this.vproperties);
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x000C1C74 File Offset: 0x000BFE74
	public override string ToString()
	{
		if (this.iproperties.ContainsKey("name") && this.iproperties.ContainsKey("type"))
		{
			return this.iproperties["type"].ToString() + " blueprint: " + (string)this.iproperties["name"];
		}
		return base.ToString();
	}

	// Token: 0x040013B0 RID: 5040
	private Dictionary<string, object> iproperties;

	// Token: 0x040013B1 RID: 5041
	private ReadWriteIndexer iindexer;

	// Token: 0x040013B2 RID: 5042
	private Dictionary<string, object> vproperties;

	// Token: 0x040013B3 RID: 5043
	private ReadWriteIndexer vindexer;
}
