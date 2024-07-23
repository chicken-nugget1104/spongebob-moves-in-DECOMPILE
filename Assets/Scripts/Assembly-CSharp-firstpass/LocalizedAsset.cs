using System;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class LocalizedAsset : MonoBehaviour
{
	// Token: 0x060006D6 RID: 1750 RVA: 0x00019990 File Offset: 0x00017B90
	public void Awake()
	{
		LocalizedAsset.LocalizeAsset(this.localizeTarget);
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x000199A0 File Offset: 0x00017BA0
	public void LocalizeAsset()
	{
		LocalizedAsset.LocalizeAsset(this.localizeTarget);
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x000199B0 File Offset: 0x00017BB0
	public static void LocalizeAsset(UnityEngine.Object target)
	{
		if (target == null)
		{
			Debug.LogError("LocalizedAsset target is null");
			return;
		}
		if (target.GetType() == typeof(GUITexture))
		{
			GUITexture guitexture = (GUITexture)target;
			if (guitexture.texture != null)
			{
				Texture texture = (Texture)Language.GetAsset(guitexture.texture.name);
				if (texture != null)
				{
					guitexture.texture = texture;
				}
			}
		}
		else if (target.GetType() == typeof(Material))
		{
			Material material = (Material)target;
			if (material.mainTexture != null)
			{
				Texture texture2 = (Texture)Language.GetAsset(material.mainTexture.name);
				if (texture2 != null)
				{
					material.mainTexture = texture2;
				}
			}
		}
		else if (target.GetType() == typeof(MeshRenderer))
		{
			MeshRenderer meshRenderer = (MeshRenderer)target;
			if (meshRenderer.material.mainTexture != null)
			{
				Texture texture3 = (Texture)Language.GetAsset(meshRenderer.material.mainTexture.name);
				if (texture3 != null)
				{
					meshRenderer.material.mainTexture = texture3;
				}
			}
		}
		else
		{
			Debug.LogError("Could not localize this object type: " + target.GetType());
		}
	}

	// Token: 0x0400044D RID: 1101
	public UnityEngine.Object localizeTarget;
}
