using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public interface Reader
{
	// Token: 0x0600217F RID: 8575
	void Read(out bool value);

	// Token: 0x06002180 RID: 8576
	void Read(out byte value);

	// Token: 0x06002181 RID: 8577
	void Read(out short value);

	// Token: 0x06002182 RID: 8578
	void Read(out ushort value);

	// Token: 0x06002183 RID: 8579
	void Read(out int value);

	// Token: 0x06002184 RID: 8580
	void Read(out uint value);

	// Token: 0x06002185 RID: 8581
	void Read(out float value);

	// Token: 0x06002186 RID: 8582
	void Read(out double value);

	// Token: 0x06002187 RID: 8583
	void Read(out Vector2 value);

	// Token: 0x06002188 RID: 8584
	void Read(out Vector3 value);

	// Token: 0x06002189 RID: 8585
	void Read(out AlignedBox value);

	// Token: 0x0600218A RID: 8586
	void Read(out string value);
}
