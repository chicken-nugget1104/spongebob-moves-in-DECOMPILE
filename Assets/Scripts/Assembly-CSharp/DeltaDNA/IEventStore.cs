using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	// Token: 0x02000006 RID: 6
	public interface IEventStore : IDisposable
	{
		// Token: 0x0600002E RID: 46
		bool Push(string obj);

		// Token: 0x0600002F RID: 47
		bool Swap();

		// Token: 0x06000030 RID: 48
		List<string> Read();

		// Token: 0x06000031 RID: 49
		void Clear();
	}
}
