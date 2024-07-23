using System;

namespace UnityEngine
{
	// Token: 0x0200006D RID: 109
	public static class BoundsExtensions
	{
		// Token: 0x060003CA RID: 970 RVA: 0x000104FC File Offset: 0x0000E6FC
		public static Rect ToRect(this Bounds value)
		{
			return new Rect(0f, 0f, 0f, 0f)
			{
				xMax = value.max.x,
				yMax = value.max.y,
				xMin = value.min.x,
				yMin = value.min.y
			};
		}
	}
}
