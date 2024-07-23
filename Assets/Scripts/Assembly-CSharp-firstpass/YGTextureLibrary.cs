using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x020000F2 RID: 242
[ExecuteInEditMode]
public class YGTextureLibrary : MonoBehaviour
{
	// Token: 0x06000926 RID: 2342 RVA: 0x000234A4 File Offset: 0x000216A4
	private void Awake()
	{
		YGTextureLibrary.atlases = new TextureAtlas[this.textureAtlases.Length];
		for (int i = 0; i < YGTextureLibrary.atlases.Length; i++)
		{
			YGTextureLibrary.atlases[i] = this.textureAtlases[i];
		}
		this.LoadAtlases();
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x000234F0 File Offset: 0x000216F0
	public void incrementTextureDuplicates(string name)
	{
		YGTextureLibrary.TextureTracker textureTracker = null;
		if (YGTextureLibrary.textures.TryGetValue(YGTextureLibrary.ActualName(name), out textureTracker))
		{
			textureTracker.count++;
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00023524 File Offset: 0x00021724
	public void ThrowTextureNotFoundException(string name)
	{
		throw new Exception("Texture Not Found: " + name);
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00023538 File Offset: 0x00021738
	public static string ActualName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return name;
		}
		int length = name.Length;
		if (length < 4)
		{
			return name;
		}
		if (name[length - 4] == '.')
		{
			return name.Substring(0, length - 4);
		}
		return name;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00023580 File Offset: 0x00021780
	public Texture LoadTexture(string name)
	{
		YGTextureLibrary.TextureTracker textureTracker = null;
		if (string.IsNullOrEmpty(name))
		{
			this.ThrowTextureNotFoundException("NULL");
		}
		string text = YGTextureLibrary.ActualName(name);
		bool flag = false;
		if (YGTextureLibrary.textures.TryGetValue(text, out textureTracker))
		{
			flag = (textureTracker.texture == null);
		}
		if (flag)
		{
			if (CommonUtils.TextureLod() != CommonUtils.LevelOfDetail.High)
			{
				string path = textureTracker.atlasAndCoords.atlas.FullTexturePath + "/_lr/" + text;
				textureTracker.texture = (Texture)Resources.Load(path, typeof(Texture));
			}
			if (textureTracker.texture == null)
			{
				string path2 = textureTracker.atlasAndCoords.atlas.FullTexturePath + "/" + text;
				textureTracker.texture = (Texture)Resources.Load(path2, typeof(Texture));
				if (textureTracker.texture == null)
				{
					this.ThrowTextureNotFoundException(name);
				}
			}
			textureTracker.count = 1;
		}
		else
		{
			textureTracker.count++;
		}
		return textureTracker.texture;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00023698 File Offset: 0x00021898
	public Texture LoadUnmanagedAtlasTexture(AtlasAndCoords coords)
	{
		if (coords == null)
		{
			Debug.LogError("Invalid Coordinate Data");
			return null;
		}
		Texture texture = null;
		if (CommonUtils.TextureLod() != CommonUtils.LevelOfDetail.High)
		{
			string path = coords.atlas.FullTexturePath + "/_lr/" + coords.atlasCoords.name;
			texture = (Texture)Resources.Load(path, typeof(Texture));
		}
		if (texture == null)
		{
			string path2 = coords.atlas.FullTexturePath + "/" + coords.atlasCoords.name;
			texture = (Texture)Resources.Load(path2, typeof(Texture));
			if (texture == null)
			{
				this.ThrowTextureNotFoundException(base.name);
			}
		}
		return texture;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00023758 File Offset: 0x00021958
	public Texture LoadUnmanagedAtlasTexture(YGTextureLibrary.TextureTracker tracker)
	{
		if (tracker == null)
		{
			return null;
		}
		return this.LoadUnmanagedAtlasTexture(tracker.atlasAndCoords);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00023770 File Offset: 0x00021970
	public Texture LoadUnmanagedAtlasTexture(string name)
	{
		Texture result = null;
		YGTextureLibrary.TextureTracker textureTracker = null;
		if (YGTextureLibrary.textures.TryGetValue(YGTextureLibrary.ActualName(name), out textureTracker))
		{
			result = this.LoadUnmanagedAtlasTexture(textureTracker.atlasAndCoords);
		}
		return result;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x000237A8 File Offset: 0x000219A8
	public void UnLoadTexture(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		YGTextureLibrary.TextureTracker textureTracker = null;
		string key = YGTextureLibrary.ActualName(name);
		if (YGTextureLibrary.textures.TryGetValue(key, out textureTracker))
		{
			if (textureTracker.count < 2)
			{
				if (textureTracker.texture != null)
				{
					Resources.UnloadAsset(textureTracker.texture);
					textureTracker.texture = null;
					textureTracker.count = 0;
				}
			}
			else
			{
				textureTracker.count--;
			}
		}
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00023828 File Offset: 0x00021A28
	public void LoadAtlases()
	{
		int num = 0;
		foreach (TextureAtlas textureAtlas in this.textureAtlases)
		{
			textureAtlas.Load();
			if (textureAtlas.addToSpriteMap)
			{
				num += textureAtlas.SpriteCount();
			}
		}
		YGTextureLibrary.textures = new Dictionary<string, YGTextureLibrary.TextureTracker>(num + 1);
		foreach (TextureAtlas textureAtlas2 in this.textureAtlases)
		{
			if (textureAtlas2.addToSpriteMap)
			{
				textureAtlas2.AddAllTextureCoords(YGTextureLibrary.textures);
			}
		}
		if (this.fontMaps.Length > 0)
		{
			YGTextureLibrary.<LoadAtlases>c__AnonStoreyC <LoadAtlases>c__AnonStoreyC = new YGTextureLibrary.<LoadAtlases>c__AnonStoreyC();
			<LoadAtlases>c__AnonStoreyC.maps = new List<TextAsset>(this.fontMaps);
			<LoadAtlases>c__AnonStoreyC.maps.Sort((TextAsset x, TextAsset y) => x.name.CompareTo(y.name));
			this.fontMaps = <LoadAtlases>c__AnonStoreyC.maps.ToArray();
			List<FontAtlas> list = new List<FontAtlas>(this.fontAtlases);
			int i;
			for (i = 0; i < <LoadAtlases>c__AnonStoreyC.maps.Count; i++)
			{
				if (list.Find((FontAtlas x) => x.fnt.name == <LoadAtlases>c__AnonStoreyC.maps[i].name) == null)
				{
					list.Add(new FontAtlas
					{
						fnt = <LoadAtlases>c__AnonStoreyC.maps[i]
					});
				}
			}
			this.fontAtlases = list.ToArray();
		}
		foreach (FontAtlas fontAtlas in this.fontAtlases)
		{
			fontAtlas.Load();
		}
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x000239F8 File Offset: 0x00021BF8
	public void LoadAtlasResources(string name)
	{
		foreach (TextureAtlas textureAtlas in this.textureAtlases)
		{
			if (!(textureAtlas.name != name))
			{
				textureAtlas.LoadMaterial();
				textureAtlas.RefreshMaterial();
			}
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00023A48 File Offset: 0x00021C48
	public static Material AtlasMaterial(string name)
	{
		foreach (TextureAtlas textureAtlas in YGTextureLibrary.atlases)
		{
			if (!(textureAtlas.name != name) || !(textureAtlas.FullName() != name))
			{
				return textureAtlas.GetAtlasMaterial();
			}
		}
		return null;
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x00023AA4 File Offset: 0x00021CA4
	public YGTextureLibrary.FoundMaterial FindSpriteMaterial(string asset)
	{
		YGTextureLibrary.FoundMaterial result = default(YGTextureLibrary.FoundMaterial);
		YGTextureLibrary.TextureTracker textureTracker = null;
		if (YGTextureLibrary.textures.TryGetValue(YGTextureLibrary.ActualName(asset), out textureTracker))
		{
			result.lib = this;
			result.material = textureTracker.atlasAndCoords.atlas.GetAtlasMaterial();
			for (int i = 0; i < this.textureAtlases.Length; i++)
			{
				if (textureTracker.atlasAndCoords.atlas == this.textureAtlases[i])
				{
					result.index = i;
					break;
				}
			}
			result.name = asset;
			result.coords = textureTracker.atlasAndCoords.atlasCoords;
		}
		else
		{
			result.index = -1;
		}
		return result;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00023B58 File Offset: 0x00021D58
	public static AtlasAndCoords GetAtlasCoords(string spriteName)
	{
		YGTextureLibrary.TextureTracker textureTracker = null;
		if (YGTextureLibrary.textures.TryGetValue(YGTextureLibrary.ActualName(spriteName), out textureTracker))
		{
			return textureTracker.atlasAndCoords;
		}
		Debug.LogError("Failed to find sprite coordinates: " + spriteName);
		return null;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00023B98 File Offset: 0x00021D98
	public static bool HasAtlasCoords(string spriteName)
	{
		return YGTextureLibrary.textures.ContainsKey(YGTextureLibrary.ActualName(spriteName));
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00023BAC File Offset: 0x00021DAC
	public bool UnloadAtlasResources(string name)
	{
		for (int i = 0; i < this.textureAtlases.Length; i++)
		{
			if (this.textureAtlases[i].name == name)
			{
				this.textureAtlases[i].UnloadAtlasResources();
				return true;
			}
		}
		return false;
	}

	// Token: 0x040005CA RID: 1482
	public const int INITIAL_TEXTURE_CAPACITY = 64;

	// Token: 0x040005CB RID: 1483
	public TextureAtlas[] textureAtlases = new TextureAtlas[0];

	// Token: 0x040005CC RID: 1484
	public TextAsset[] fontMaps = new TextAsset[0];

	// Token: 0x040005CD RID: 1485
	public FontAtlas[] fontAtlases = new FontAtlas[0];

	// Token: 0x040005CE RID: 1486
	public Material materialPrototype;

	// Token: 0x040005CF RID: 1487
	private static Dictionary<string, YGTextureLibrary.TextureTracker> textures = new Dictionary<string, YGTextureLibrary.TextureTracker>();

	// Token: 0x040005D0 RID: 1488
	[HideInInspector]
	public bool bShowingDialog;

	// Token: 0x040005D1 RID: 1489
	private static TextureAtlas[] atlases = null;

	// Token: 0x020000F3 RID: 243
	public class TextureTracker
	{
		// Token: 0x040005D3 RID: 1491
		public AtlasAndCoords atlasAndCoords;

		// Token: 0x040005D4 RID: 1492
		public Texture texture;

		// Token: 0x040005D5 RID: 1493
		public int count;
	}

	// Token: 0x020000F4 RID: 244
	public struct FoundMaterial
	{
		// Token: 0x040005D6 RID: 1494
		public Material material;

		// Token: 0x040005D7 RID: 1495
		public YGTextureLibrary lib;

		// Token: 0x040005D8 RID: 1496
		public int index;

		// Token: 0x040005D9 RID: 1497
		public string name;

		// Token: 0x040005DA RID: 1498
		public AtlasCoords coords;
	}
}
