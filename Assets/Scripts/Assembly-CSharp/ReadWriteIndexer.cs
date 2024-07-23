using System;
using System.Collections.Generic;

// Token: 0x02000407 RID: 1031
public class ReadWriteIndexer
{
	// Token: 0x06001F8C RID: 8076 RVA: 0x000C1AC4 File Offset: 0x000BFCC4
	public ReadWriteIndexer(Dictionary<string, object> properties)
	{
		this.properties = properties;
	}

	// Token: 0x1700044A RID: 1098
	public object this[string property]
	{
		get
		{
			return this.properties[property];
		}
		set
		{
			this.properties[property] = value;
		}
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x000C1AF4 File Offset: 0x000BFCF4
	public bool ContainsKey(string property)
	{
		return this.properties.ContainsKey(property);
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x000C1B04 File Offset: 0x000BFD04
	public bool TryGetValue(string property, out object value)
	{
		value = null;
		this.properties.TryGetValue(property, out value);
		return value != null;
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x000C1B20 File Offset: 0x000BFD20
	public void Remove(string property)
	{
		if (!this.properties.ContainsKey(property))
		{
			throw new InvalidOperationException("No key: " + property);
		}
		this.properties.Remove(property);
	}

	// Token: 0x040013AF RID: 5039
	private Dictionary<string, object> properties;
}
