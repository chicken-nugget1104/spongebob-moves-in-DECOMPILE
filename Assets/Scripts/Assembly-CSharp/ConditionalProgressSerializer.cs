using System;
using System.Collections.Generic;

// Token: 0x0200012F RID: 303
public class ConditionalProgressSerializer
{
	// Token: 0x06000AF5 RID: 2805 RVA: 0x00043C8C File Offset: 0x00041E8C
	public ConditionalProgress DeserializeProgress(ICollection<object> loadedData)
	{
		List<uint> list = new List<uint>(loadedData.Count);
		foreach (object value in loadedData)
		{
			list.Add(Convert.ToUInt32(value));
		}
		return new ConditionalProgress(list);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00043D04 File Offset: 0x00041F04
	public List<object> SerializeProgress(ConditionalProgress progress)
	{
		return TFUtils.CloneAndCastList<uint, object>(progress.MetIds);
	}
}
