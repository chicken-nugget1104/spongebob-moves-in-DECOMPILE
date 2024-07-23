using System;
using System.Collections.Generic;

// Token: 0x0200041B RID: 1051
public class Factory<Key, Base>
{
	// Token: 0x06002075 RID: 8309 RVA: 0x000CA1C0 File Offset: 0x000C83C0
	public void Register(Key key, Ctor<Base> ctor)
	{
		this.ctors.Add(key, ctor);
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000CA1D0 File Offset: 0x000C83D0
	public Base Create(Key key)
	{
		TFUtils.Assert(this.ctors.ContainsKey(key), string.Format("Missing Factory Item: {0}", key));
		return this.ctors[key].Create();
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x000CA210 File Offset: 0x000C8410
	public Base Create(Key key, Identity id)
	{
		TFUtils.Assert(this.ctors.ContainsKey(key), string.Format("Missing Factory Item: {0}", key));
		return this.ctors[key].Create(id);
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x000CA250 File Offset: 0x000C8450
	public void Reset()
	{
		this.ctors.Clear();
	}

	// Token: 0x040013EC RID: 5100
	private Dictionary<Key, Ctor<Base>> ctors = new Dictionary<Key, Ctor<Base>>();
}
