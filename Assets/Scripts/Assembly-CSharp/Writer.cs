using System;
using UnityEngine;

// Token: 0x0200047A RID: 1146
public interface Writer
{
	// Token: 0x060023D4 RID: 9172
	void Write(bool value);

	// Token: 0x060023D5 RID: 9173
	void Write(byte value);

	// Token: 0x060023D6 RID: 9174
	void Write(short value);

	// Token: 0x060023D7 RID: 9175
	void Write(ushort value);

	// Token: 0x060023D8 RID: 9176
	void Write(int value);

	// Token: 0x060023D9 RID: 9177
	void Write(uint value);

	// Token: 0x060023DA RID: 9178
	void Write(float value);

	// Token: 0x060023DB RID: 9179
	void Write(double value);

	// Token: 0x060023DC RID: 9180
	void Write(Vector2 value);

	// Token: 0x060023DD RID: 9181
	void Write(Vector3 value);

	// Token: 0x060023DE RID: 9182
	void Write(AlignedBox value);

	// Token: 0x060023DF RID: 9183
	void Write(string value);
}
