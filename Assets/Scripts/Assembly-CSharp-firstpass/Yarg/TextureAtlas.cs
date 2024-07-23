using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MiniJSON;
using UnityEngine;

namespace Yarg
{
	// Token: 0x020000F1 RID: 241
	[Serializable]
	public sealed class TextureAtlas : ILoadable
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x0002292C File Offset: 0x00020B2C
		public TextureAtlas()
		{
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00022934 File Offset: 0x00020B34
		public TextureAtlas(Dictionary<string, object> source)
		{
			this._Load(source);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00022944 File Offset: 0x00020B44
		public TextureAtlas(string fileName)
		{
			this._Load(fileName);
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00022954 File Offset: 0x00020B54
		public string FullTexturePath
		{
			get
			{
				if (this.fullTexturePath == null)
				{
					this.fullTexturePath = "Textures/Atlases/" + this.texturePathName;
				}
				return this.fullTexturePath;
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00022980 File Offset: 0x00020B80
		public Material GetAtlasMaterial()
		{
			if (this.material == null)
			{
				this.LoadMaterial();
			}
			return this.material;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x000229A0 File Offset: 0x00020BA0
		private void BuildDictionary()
		{
			if (this.frameArray == null || this.frameArray.Length == 0)
			{
				this.Load();
			}
			else if (this.frames != null && this.frames.Count == this.frameArray.Length)
			{
				return;
			}
			if (this.frames == null)
			{
				Debug.LogError("BuildDictionary: FAILED: NO FRAMES: SHOULD NEVER GET HERE");
			}
		}

		// Token: 0x170000A0 RID: 160
		public AtlasCoords this[string name]
		{
			get
			{
				this.BuildDictionary();
				AtlasCoords result = null;
				this.frames.TryGetValue(YGTextureLibrary.ActualName(name), out result);
				return result;
			}
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00022A38 File Offset: 0x00020C38
		public ICollection<string> GetNames()
		{
			this.BuildDictionary();
			return this.frames.Keys;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00022A4C File Offset: 0x00020C4C
		public void _Load(Dictionary<string, object> source)
		{
			this.frames = new Dictionary<string, AtlasCoords>();
			if (source.ContainsKey("frames"))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)source["frames"];
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					string key = YGTextureLibrary.ActualName(keyValuePair.Key);
					this.frames[key] = new AtlasCoords(key, (Dictionary<string, object>)keyValuePair.Value);
				}
				this.frameArray = new List<AtlasCoords>(this.frames.Values).ToArray();
			}
			if (source.ContainsKey("meta"))
			{
				this.meta = new AtlasMetaData((Dictionary<string, object>)source["meta"]);
				int num = this.meta.name.LastIndexOf('.');
				if (num >= 0)
				{
					this.name = this.meta.name.Substring(0, num);
				}
				else
				{
					this.name = this.meta.name;
				}
			}
			this.fullName = "Materials/lod/" + this.name;
			this.fullTexturePath = this.FullTexturePath;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00022BB0 File Offset: 0x00020DB0
		public void _Load(string fileName)
		{
			BinaryReader binaryReader = null;
			try
			{
				string str = Application.persistentDataPath + "/Contents/Textures/AtlasCoordinates/cmp/";
				if (Language.CurrentLanguage() != LanguageCode.EN)
				{
					this.RefreshLanguages();
					string str2 = Language.LocalizedEnglishAssetName(fileName);
					string path = str + str2 + ".txt";
					if (File.Exists(path))
					{
						binaryReader = new BinaryReader(File.OpenRead(path));
					}
					else
					{
						TextAsset textAsset = (TextAsset)Resources.Load("Textures/AtlasCoordinates/cmp/" + str2, typeof(TextAsset));
						if (textAsset != null)
						{
							binaryReader = new BinaryReader(new MemoryStream(textAsset.bytes));
							Resources.UnloadAsset(textAsset);
						}
					}
				}
				if (binaryReader == null)
				{
					string path2 = str + fileName + ".txt";
					if (File.Exists(path2))
					{
						binaryReader = new BinaryReader(File.OpenRead(path2));
					}
					else
					{
						TextAsset textAsset2 = (TextAsset)Resources.Load("Textures/AtlasCoordinates/cmp/" + fileName, typeof(TextAsset));
						if (textAsset2 == null)
						{
							throw new Exception("Failed To Load Atlas: " + fileName);
						}
						binaryReader = new BinaryReader(new MemoryStream(textAsset2.bytes));
						Resources.UnloadAsset(textAsset2);
					}
				}
				int num = binaryReader.ReadInt32();
				if (num > 3 || num < 2)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Invalid Atlas Version: ",
						num,
						" vs ",
						3
					}));
				}
				this.meta = new AtlasMetaData(binaryReader);
				int num2 = this.meta.name.LastIndexOf('.');
				if (num2 >= 0)
				{
					this.name = this.meta.name.Substring(0, num2);
				}
				else
				{
					this.name = this.meta.name;
				}
				this.fullName = "Materials/lod/" + this.name;
				int num3 = binaryReader.ReadInt32();
				this.frameArray = new AtlasCoords[num3];
				this.frames = new Dictionary<string, AtlasCoords>(num3);
				for (int i = 0; i < num3; i++)
				{
					this.frameArray[i] = new AtlasCoords(binaryReader, num);
					this.frames.Add(this.frameArray[i].name, this.frameArray[i]);
				}
				binaryReader.Close();
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"TextureAtlas: Error: Load: ",
					this.jsonFileName,
					" ",
					ex.Message,
					"\n",
					ex.StackTrace
				}));
			}
			this.fullTexturePath = this.FullTexturePath;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00022E88 File Offset: 0x00021088
		public string FullName()
		{
			if (this.fullName == null)
			{
				this.fullName = "Materials/lod/" + this.jsonFileName;
			}
			return this.fullName;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00022EB4 File Offset: 0x000210B4
		private Material LoadMaterial(string file, bool async_load = false)
		{
			return Resources.Load(file, typeof(Material)) as Material;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00022ECC File Offset: 0x000210CC
		private void RefreshLanguages()
		{
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(Path.Combine(Application.persistentDataPath, "Contents"));
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00022EEC File Offset: 0x000210EC
		public void LoadMaterial()
		{
			if (this.useSingleTexture)
			{
				return;
			}
			string text = "Materials/lod/";
			if (this.name.ToLower().Contains("localized_"))
			{
				this.RefreshLanguages();
				this.name = Language.LocalizedEnglishAssetName(this.name);
			}
			text += this.name;
			CommonUtils.LevelOfDetail levelOfDetail = CommonUtils.TextureLod();
			string str = string.Empty;
			if (levelOfDetail == CommonUtils.LevelOfDetail.Low)
			{
				str = "_lr2";
			}
			else if (levelOfDetail == CommonUtils.LevelOfDetail.Standard)
			{
				str = "_lr";
			}
			if (this.material != null)
			{
				return;
			}
			string text2 = CommonUtils.TextureForDeviceOverride(this.name);
			UnityEngine.Object @object;
			if (text2 != this.name)
			{
				Debug.LogError("LoadMaterialName: Override: " + text2);
				@object = FileSystemCoordinator.LoadAsset(text2);
				if (@object != null)
				{
					this.material = (Material)@object;
					return;
				}
				this.material = this.LoadMaterial("Materials/lod/" + text2, false);
				if (this.material != null)
				{
					return;
				}
			}
			@object = FileSystemCoordinator.LoadAsset(this.name + str);
			if (@object != null)
			{
				this.material = (Material)@object;
				return;
			}
			this.material = this.LoadMaterial(text + str, false);
			if (this.material == null && levelOfDetail != CommonUtils.LevelOfDetail.High)
			{
				this.material = this.LoadMaterial(text, false);
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002306C File Offset: 0x0002126C
		public void RefreshMaterial()
		{
			if (this.material != null)
			{
				this.material.shader = Shader.Find(this.material.shader.name);
			}
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x000230AC File Offset: 0x000212AC
		public void Load()
		{
			TextureAtlas textureAtlas = new TextureAtlas(this.jsonFileName);
			this.frames = textureAtlas.frames;
			this.frameArray = textureAtlas.frameArray;
			this.meta = textureAtlas.meta;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x000230EC File Offset: 0x000212EC
		public void LoadJsonAtlas()
		{
			TextAsset textAsset = null;
			TextureAtlas textureAtlas = TextureAtlas.LoadJsonAtlas(textAsset.text);
			this.frames = textureAtlas.frames;
			this.frameArray = textureAtlas.frameArray;
			this.meta = textureAtlas.meta;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002312C File Offset: 0x0002132C
		public static TextureAtlas LoadJsonAtlas(string json)
		{
			Dictionary<string, object> source = (Dictionary<string, object>)Json.Deserialize(json);
			return new TextureAtlas(source);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00023150 File Offset: 0x00021350
		public void AdjustUVsToFrame(AtlasCoords coords, ref float u0, ref float u1, ref float v0, ref float v1)
		{
			float x = this.meta.invScale.x;
			float y = this.meta.invScale.y;
			u0 = (coords.frame.x + u0 * coords.frame.width) * x;
			u1 = (coords.frame.x + u1 * coords.frame.width) * x;
			float num = this.meta.size.height - coords.frame.y - coords.frame.height;
			v0 = (num + coords.frame.height * v0) * y;
			v1 = (num + coords.frame.height * v1) * y;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00023210 File Offset: 0x00021410
		public void GetUVs(AtlasCoords coords, ref Rect rect)
		{
			float x = this.meta.invScale.x;
			float y = this.meta.invScale.y;
			rect.xMin = coords.frame.x * x;
			rect.xMax = (coords.frame.x + coords.frame.width) * x;
			float num = this.meta.size.height - coords.frame.y - coords.frame.height;
			rect.yMin = num * y;
			rect.yMax = (num + coords.frame.height) * y;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x000232B8 File Offset: 0x000214B8
		public void Proccess(AtlasCoords coordData, string name)
		{
			if (!this.useRenderTexture)
			{
				return;
			}
			if (coordData == null)
			{
				Debug.LogError("Null Coordinate Data: " + name);
				return;
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x000232E0 File Offset: 0x000214E0
		public int SpriteCount()
		{
			if (this.frameArray == null)
			{
				return 0;
			}
			return this.frameArray.Length;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000232F8 File Offset: 0x000214F8
		public void AddAllTextureCoords(Dictionary<string, YGTextureLibrary.TextureTracker> textureData)
		{
			foreach (AtlasCoords atlasCoords in this.frameArray)
			{
				AtlasAndCoords atlasAndCoords = new AtlasAndCoords();
				atlasAndCoords.atlas = this;
				atlasAndCoords.atlasCoords = atlasCoords;
				YGTextureLibrary.TextureTracker textureTracker = new YGTextureLibrary.TextureTracker();
				textureTracker.atlasAndCoords = atlasAndCoords;
				textureData.Add(atlasCoords.name, textureTracker);
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00023358 File Offset: 0x00021558
		public void UnloadAtlasResources()
		{
			if (this.material == null)
			{
				return;
			}
			if (this.materialTextures != null)
			{
				for (int i = 0; i < this.materialTextures.Length; i++)
				{
					Texture texture = this.material.GetTexture(this.materialTextures[i]);
					if (!(texture == null))
					{
						Resources.UnloadAsset(texture);
					}
				}
				if (this.materialTextures.Length != 0)
				{
					this.material = null;
				}
			}
			if (this.material != null)
			{
				Resources.UnloadAsset(this.material.mainTexture);
			}
			this.material = null;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00023404 File Offset: 0x00021604
		public static string _ReadString(BinaryReader reader)
		{
			byte b = reader.ReadByte();
			if (b == 0)
			{
				return string.Empty;
			}
			byte[] array = reader.ReadBytes((int)b);
			StringBuilder stringBuilder = new StringBuilder((int)b);
			for (int i = 0; i < (int)b; i++)
			{
				stringBuilder.Append((char)array[i]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040005AF RID: 1455
		public const string MaterialPath_Resources = "Materials/lod/";

		// Token: 0x040005B0 RID: 1456
		public const string MaterialJMatPath_Persistant = "Contents/Materials/jmat/";

		// Token: 0x040005B1 RID: 1457
		public const string MaterialJMatPath_Resources = "Materials/jmat/";

		// Token: 0x040005B2 RID: 1458
		public const string TexturePath_Persistant = "Contents/Textures/";

		// Token: 0x040005B3 RID: 1459
		public const string AtlasJsonPath_Resources = "Textures/AtlasCoordinates/";

		// Token: 0x040005B4 RID: 1460
		public const string AtlasPath_CMP = "Textures/AtlasCoordinates/cmp/";

		// Token: 0x040005B5 RID: 1461
		public const string AtlasPath_Persistant = "Contents/Textures/AtlasCoordinates/";

		// Token: 0x040005B6 RID: 1462
		public const string lowRez2Option = "_lr2";

		// Token: 0x040005B7 RID: 1463
		public const string lowRezOption = "_lr";

		// Token: 0x040005B8 RID: 1464
		public const int SPRITE_UV_ATLAS_VERSION = 2;

		// Token: 0x040005B9 RID: 1465
		public const int COMPATIBLE_ATLAS_VERSION = 2;

		// Token: 0x040005BA RID: 1466
		public const int COMPILED_ATLAS_VERSION = 3;

		// Token: 0x040005BB RID: 1467
		public string name;

		// Token: 0x040005BC RID: 1468
		public string jsonFileName;

		// Token: 0x040005BD RID: 1469
		public string texturePathName;

		// Token: 0x040005BE RID: 1470
		public bool addToSpriteMap;

		// Token: 0x040005BF RID: 1471
		public bool useDeviceTypeForMaterials;

		// Token: 0x040005C0 RID: 1472
		private Material material;

		// Token: 0x040005C1 RID: 1473
		public string[] materialTextures;

		// Token: 0x040005C2 RID: 1474
		public AtlasMetaData meta;

		// Token: 0x040005C3 RID: 1475
		public bool useSingleTexture;

		// Token: 0x040005C4 RID: 1476
		public bool useRenderTexture;

		// Token: 0x040005C5 RID: 1477
		[HideInInspector]
		[NonSerialized]
		private Dictionary<string, AtlasCoords> frames;

		// Token: 0x040005C6 RID: 1478
		[HideInInspector]
		[NonSerialized]
		private AtlasCoords[] frameArray;

		// Token: 0x040005C7 RID: 1479
		[HideInInspector]
		[NonSerialized]
		public string fullName;

		// Token: 0x040005C8 RID: 1480
		[HideInInspector]
		[NonSerialized]
		private string fullTexturePath;

		// Token: 0x040005C9 RID: 1481
		[HideInInspector]
		[NonSerialized]
		private RenderTextureBuffer textureBuffer;
	}
}
