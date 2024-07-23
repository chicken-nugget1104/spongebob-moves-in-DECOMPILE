using System;
using FarseerPhysics.Collision;
using UnityEngine;

namespace FarseerPhysics
{
	// Token: 0x0200006E RID: 110
	public static class RectExtensionMethods
	{
		// Token: 0x060003CB RID: 971 RVA: 0x00010580 File Offset: 0x0000E780
		public static AABB ToAABB(this Rect rect)
		{
			return new AABB(rect.center, rect.width, rect.height);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000105A8 File Offset: 0x0000E7A8
		public static Rect ToRect(this AABB bbox)
		{
			return new Rect(bbox.UpperBound.x, bbox.UpperBound.y, bbox.LowerBound.x, bbox.LowerBound.y);
		}
	}
}
