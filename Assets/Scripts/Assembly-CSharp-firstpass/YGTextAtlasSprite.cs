using System;
using UnityEngine;
using Yarg;

// Token: 0x0200010C RID: 268
public class YGTextAtlasSprite : YGTextSprite
{
	// Token: 0x060009E9 RID: 2537 RVA: 0x00026F60 File Offset: 0x00025160
	protected override void OnEnable()
	{
		Material atlasMaterial = base.View.Library.textureAtlases[this.atlasIndex].GetAtlasMaterial();
		if (base.renderer.sharedMaterial != atlasMaterial)
		{
			base.SetMaterial(atlasMaterial);
		}
		base.OnEnable();
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00026FB0 File Offset: 0x000251B0
	public override FontAtlas.CharData GetCharOffset(char chr, FontAtlas atlas)
	{
		if (this.sprite == null)
		{
			Debug.LogError(string.Format("sprite isn't initialized for {0}", base.gameObject.name));
			Debug.Break();
		}
		FontAtlas.CharData result = atlas[chr];
		result.size.x = result.size.x + this.sprite.coords.x;
		result.size.y = result.size.y + this.sprite.coords.y;
		if (atlas.filename.Contains("cyrillic"))
		{
			result.size.x = result.size.x - 4f;
			result.size.y = result.size.y - 4f;
		}
		else
		{
			result.size.x = result.size.x - 1f;
			result.size.y = result.size.y - 1f;
			result.size.width = result.size.width + 2f;
			result.size.height = result.size.height + 2f;
		}
		return result;
	}
}
