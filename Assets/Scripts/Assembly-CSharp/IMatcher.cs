using System;
using System.Collections.Generic;

// Token: 0x0200018A RID: 394
public interface IMatcher
{
	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000D50 RID: 3408
	ICollection<string> Keys { get; }

	// Token: 0x06000D51 RID: 3409
	uint MatchAmount(Game game, Dictionary<string, object> data);

	// Token: 0x06000D52 RID: 3410
	bool IsRequired(string property);

	// Token: 0x06000D53 RID: 3411
	bool HasRequirements();

	// Token: 0x06000D54 RID: 3412
	string GetTarget(string key);

	// Token: 0x06000D55 RID: 3413
	string DescribeSubject(Game game);

	// Token: 0x06000D56 RID: 3414
	string ToString();
}
