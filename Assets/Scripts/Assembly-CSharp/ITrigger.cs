using System;
using System.Collections.Generic;

// Token: 0x02000269 RID: 617
public interface ITrigger
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06001407 RID: 5127
	string Type { get; }

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06001408 RID: 5128
	ulong TimeStamp { get; }

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06001409 RID: 5129
	Dictionary<string, object> Data { get; }
}
