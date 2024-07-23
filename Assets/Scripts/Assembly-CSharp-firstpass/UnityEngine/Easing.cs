using System;

namespace UnityEngine
{
	// Token: 0x0200005F RID: 95
	public static class Easing
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
		public static Vector3 Vector3Easing(Vector3 start, Vector3 end, float value, Func<float, float, float, float> easingMethod)
		{
			Vector3 result;
			result.x = easingMethod(start.x, end.x, value);
			result.y = easingMethod(start.y, end.y, value);
			result.z = easingMethod(start.z, end.z, value);
			return result;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000DB04 File Offset: 0x0000BD04
		public static float Linear(float start, float end, float value)
		{
			return Mathf.Lerp(start, end, value);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000DB10 File Offset: 0x0000BD10
		public static float Clerp(float start, float end, float value)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Mathf.Abs((num2 - num) / 2f);
			float result;
			if (end - start < -num3)
			{
				float num4 = (num2 - start + end) * value;
				result = start + num4;
			}
			else if (end - start > num3)
			{
				float num4 = -(num2 - end + start) * value;
				result = start + num4;
			}
			else
			{
				result = start + (end - start) * value;
			}
			return result;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000DB88 File Offset: 0x0000BD88
		public static float Spring(float start, float end, float value)
		{
			value = Mathf.Clamp01(value);
			value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
			return start + (end - start) * value;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000DBEC File Offset: 0x0000BDEC
		public static float EaseInQuad(float start, float end, float value)
		{
			end -= start;
			return end * value * value + start;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000DBFC File Offset: 0x0000BDFC
		public static float EaseOutQuad(float start, float end, float value)
		{
			end -= start;
			return -end * value * (value - 2f) + start;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public static float EaseInOutQuad(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value + start;
			}
			value -= 1f;
			return -end / 2f * (value * (value - 2f) - 1f) + start;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		public static float EaseInCubic(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value + start;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		public static float EaseOutCubic(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * (value * value * value + 1f) + start;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		public static float EaseInOutCubic(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value * value + start;
			}
			value -= 2f;
			return end / 2f * (value * value * value + 2f) + start;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DCF0 File Offset: 0x0000BEF0
		public static float EaseInQuart(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value * value + start;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DD04 File Offset: 0x0000BF04
		public static float EaseOutQuart(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return -end * (value * value * value * value - 1f) + start;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DD34 File Offset: 0x0000BF34
		public static float EaseInOutQuart(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value * value * value + start;
			}
			value -= 2f;
			return -end / 2f * (value * value * value * value - 2f) + start;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000DD90 File Offset: 0x0000BF90
		public static float EaseInQuint(float start, float end, float value)
		{
			end -= start;
			return end * value * value * value * value * value + start;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		public static float EaseOutQuint(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * (value * value * value * value * value + 1f) + start;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DDC8 File Offset: 0x0000BFC8
		public static float EaseInOutQuint(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * value * value * value * value * value + start;
			}
			value -= 2f;
			return end / 2f * (value * value * value * value * value + 2f) + start;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DE24 File Offset: 0x0000C024
		public static float EaseInSine(float start, float end, float value)
		{
			end -= start;
			return -end * Mathf.Cos(value / 1f * 1.5707964f) + end + start;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000DE44 File Offset: 0x0000C044
		public static float EaseOutSine(float start, float end, float value)
		{
			end -= start;
			return end * Mathf.Sin(value / 1f * 1.5707964f) + start;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000DE64 File Offset: 0x0000C064
		public static float EaseInOutSine(float start, float end, float value)
		{
			end -= start;
			return -end / 2f * (Mathf.Cos(3.1415927f * value / 1f) - 1f) + start;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000DE9C File Offset: 0x0000C09C
		public static float EaseInExpo(float start, float end, float value)
		{
			end -= start;
			return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000DED0 File Offset: 0x0000C0D0
		public static float EaseOutExpo(float start, float end, float value)
		{
			end -= start;
			return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000DEFC File Offset: 0x0000C0FC
		public static float EaseInOutExpo(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
			}
			value -= 1f;
			return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000DF70 File Offset: 0x0000C170
		public static float EaseInCirc(float start, float end, float value)
		{
			end -= start;
			return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000DF90 File Offset: 0x0000C190
		public static float EaseOutCirc(float start, float end, float value)
		{
			value -= 1f;
			end -= start;
			return end * Mathf.Sqrt(1f - value * value) + start;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		public static float EaseInOutCirc(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
			}
			value -= 2f;
			return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000E030 File Offset: 0x0000C230
		public static float EaseInBounce(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			return end - Easing.EaseOutBounce(0f, end, num - value) + start;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000E05C File Offset: 0x0000C25C
		public static float EaseOutBounce(float start, float end, float value)
		{
			value /= 1f;
			end -= start;
			if (value < 0.36363637f)
			{
				return end * (7.5625f * value * value) + start;
			}
			if (value < 0.72727275f)
			{
				value -= 0.54545456f;
				return end * (7.5625f * value * value + 0.75f) + start;
			}
			if ((double)value < 0.9090909090909091)
			{
				value -= 0.8181818f;
				return end * (7.5625f * value * value + 0.9375f) + start;
			}
			value -= 0.95454544f;
			return end * (7.5625f * value * value + 0.984375f) + start;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000E104 File Offset: 0x0000C304
		public static float EaseInOutBounce(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			if (value < num / 2f)
			{
				return Easing.EaseInBounce(0f, end, value * 2f) * 0.5f + start;
			}
			return Easing.EaseOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000E168 File Offset: 0x0000C368
		public static float EaseInBack(float start, float end, float value)
		{
			end -= start;
			value /= 1f;
			float num = 1.70158f;
			return end * value * value * ((num + 1f) * value - num) + start;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000E19C File Offset: 0x0000C39C
		public static float EaseOutBack(float start, float end, float value)
		{
			float num = 1.4f;
			end -= start;
			value = value / 1f - 1f;
			return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000E1DC File Offset: 0x0000C3DC
		public static float EaseInOutBack(float start, float end, float value)
		{
			float num = 1.70158f;
			end -= start;
			value /= 0.5f;
			if (value < 1f)
			{
				num *= 1.525f;
				return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
			}
			value -= 2f;
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000E25C File Offset: 0x0000C45C
		public static float Punch(float amplitude, float value)
		{
			if (value == 0f)
			{
				return 0f;
			}
			if (value == 1f)
			{
				return 0f;
			}
			float num = 0.3f;
			float num2 = num / 6.2831855f * Mathf.Asin(0f);
			return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000E2D4 File Offset: 0x0000C4D4
		public static float EaseInElastic(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			if (value == 0f)
			{
				return start;
			}
			if ((value /= num) == 1f)
			{
				return start + end;
			}
			float num4;
			if (num3 == 0f || num3 < Mathf.Abs(end))
			{
				num3 = end;
				num4 = num2 / 4f;
			}
			else
			{
				num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
			}
			return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000E38C File Offset: 0x0000C58C
		public static float EaseOutElastic(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			if (value == 0f)
			{
				return start;
			}
			if ((value /= num) == 1f)
			{
				return start + end;
			}
			float num4;
			if (num3 == 0f || num3 < Mathf.Abs(end))
			{
				num3 = end;
				num4 = num2 / 4f;
			}
			else
			{
				num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
			}
			return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000E43C File Offset: 0x0000C63C
		public static float EaseInOutElastic(float start, float end, float value)
		{
			end -= start;
			float num = 1f;
			float num2 = num * 0.3f;
			float num3 = 0f;
			if (value == 0f)
			{
				return start;
			}
			if ((value /= num / 2f) == 2f)
			{
				return start + end;
			}
			float num4;
			if (num3 == 0f || num3 < Mathf.Abs(end))
			{
				num3 = end;
				num4 = num2 / 4f;
			}
			else
			{
				num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
			}
			if (value < 1f)
			{
				return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
			}
			return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
		}
	}
}
