using System;
using UnityEngine;

// Token: 0x02000459 RID: 1113
public class TFMath
{
	// Token: 0x0600226B RID: 8811 RVA: 0x000D397C File Offset: 0x000D1B7C
	public static float ClampF(float input, float min, float max)
	{
		return Math.Max(min, Math.Min(max, input));
	}

	// Token: 0x0600226C RID: 8812 RVA: 0x000D398C File Offset: 0x000D1B8C
	public static float Modulo(float dividend, float divisor)
	{
		float num = (float)((dividend >= 0f) ? 1 : -1);
		return num * (Mathf.Abs(dividend) % divisor);
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x000D39B8 File Offset: 0x000D1BB8
	public static int Modulo(int dividend, int divisor)
	{
		int num = (dividend >= 0) ? 1 : -1;
		return num * (Mathf.Abs(dividend) % divisor);
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000D39E0 File Offset: 0x000D1BE0
	public static float Quadratic(float a, float b, float c, float x)
	{
		return a * (x * x) + b * x + c;
	}
}
