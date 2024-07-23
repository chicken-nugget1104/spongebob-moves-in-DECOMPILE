using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class TextureLibrarian : MonoBehaviour
{
	// Token: 0x06000765 RID: 1893 RVA: 0x00031010 File Offset: 0x0002F210
	public static Material LookUp(string name)
	{
		Material material = null;
		if (!TextureLibrarian.library.TryGetValue(name, out material))
		{
			material = YGTextureLibrary.AtlasMaterial(name);
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				material = (Resources.Load(name + "_lr2") as Material);
				if (material == null)
				{
					material = (Resources.Load(name + "_lr") as Material);
				}
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
			{
				material = (Resources.Load(name + "_lr") as Material);
			}
			if (material == null)
			{
				material = (Resources.Load(name) as Material);
			}
			TextureLibrarian.library[name] = material;
		}
		TFUtils.Assert(material != null, string.Format("material '{0}' not found", name));
		return material;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x000310E0 File Offset: 0x0002F2E0
	public static string PathLookUp(string name)
	{
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			name += "_lr2";
		}
		else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
		{
			name += "_lr";
		}
		return name;
	}

	// Token: 0x04000592 RID: 1426
	private static Dictionary<string, Material> library = new Dictionary<string, Material>();
}
