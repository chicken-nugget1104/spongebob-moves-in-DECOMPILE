using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	// Token: 0x02000002 RID: 2
	public class EventBuilder
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002100 File Offset: 0x00000300
		public EventBuilder AddParam(string key, object value)
		{
			if (value == null)
			{
				return this;
			}
			if (value.GetType() == typeof(ProductBuilder))
			{
				ProductBuilder productBuilder = value as ProductBuilder;
				value = productBuilder.ToDictionary();
			}
			else if (value.GetType() == typeof(EventBuilder))
			{
				EventBuilder eventBuilder = value as EventBuilder;
				value = eventBuilder.ToDictionary();
			}
			this.dict.Add(key, value);
			return this;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002170 File Offset: 0x00000370
		public Dictionary<string, object> ToDictionary()
		{
			return this.dict;
		}

		// Token: 0x04000001 RID: 1
		private Dictionary<string, object> dict = new Dictionary<string, object>();
	}
}
