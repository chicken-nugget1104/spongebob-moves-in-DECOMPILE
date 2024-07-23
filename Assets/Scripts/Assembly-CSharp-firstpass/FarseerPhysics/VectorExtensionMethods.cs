using System;
using FarseerPhysics.Common;
using UnityEngine;

namespace FarseerPhysics
{
	// Token: 0x0200006F RID: 111
	public static class VectorExtensionMethods
	{
		// Token: 0x060003CD RID: 973 RVA: 0x000105E0 File Offset: 0x0000E7E0
		public static Transform2D To2D(this Transform tf)
		{
			Transform2D result = default(Transform2D);
			result.Set(tf.position, tf.rotation.eulerAngles.z);
			return result;
		}
	}
}
