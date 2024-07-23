using System;
using System.Collections.Generic;

// Token: 0x02000406 RID: 1030
public class ReadOnlyIndexer
{
	// Token: 0x06001F88 RID: 8072 RVA: 0x000C1A78 File Offset: 0x000BFC78
	public ReadOnlyIndexer(Dictionary<string, object> properties)
	{
		this.properties = properties;
	}

	// Token: 0x17000449 RID: 1097
	public object this[string property]
	{
		get
		{
			return this.properties[property];
		}
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x000C1A98 File Offset: 0x000BFC98
	public bool ContainsKey(string property)
	{
		return this.properties.ContainsKey(property);
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x000C1AA8 File Offset: 0x000BFCA8
	public bool TryGetValue(string property, out object value)
	{
		value = null;
		this.properties.TryGetValue(property, out value);
		return value != null;
	}

	// Token: 0x040013AE RID: 5038
	private Dictionary<string, object> properties;
}
