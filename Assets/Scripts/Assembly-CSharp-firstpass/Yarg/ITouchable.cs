using System;
using UnityEngine;

namespace Yarg
{
	// Token: 0x02000100 RID: 256
	public interface ITouchable
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000961 RID: 2401
		Transform tform { get; }

		// Token: 0x06000962 RID: 2402
		bool TouchEvent(YGEvent touch);
	}
}
