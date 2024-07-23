using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Yarg
{
	// Token: 0x020000EF RID: 239
	[Serializable]
	public sealed class AtlasMetaData
	{
		// Token: 0x06000907 RID: 2311 RVA: 0x0002276C File Offset: 0x0002096C
		public AtlasMetaData()
		{
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00022774 File Offset: 0x00020974
		public AtlasMetaData(Dictionary<string, object> source)
		{
			this.image = (string)source["image"];
			this.name = (string)source["name"];
			this.premultipliedAlpha = (bool)source["premultipliedAlpha"];
			this.scale = Convert.ToSingle(source["scale"]);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)source["size"];
			if (dictionary != null)
			{
				this.size = new Rect(0f, 0f, Convert.ToSingle(dictionary["w"]), Convert.ToSingle(dictionary["h"]));
			}
			this.invScale.x = 1f / this.size.width;
			this.invScale.y = 1f / this.size.height;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00022864 File Offset: 0x00020A64
		public AtlasMetaData(BinaryReader reader)
		{
			this.name = TextureAtlas._ReadString(reader);
			this.image = TextureAtlas._ReadString(reader);
			this.premultipliedAlpha = reader.ReadBoolean();
			this.size.x = reader.ReadSingle();
			this.size.y = reader.ReadSingle();
			this.size.width = reader.ReadSingle();
			this.size.height = reader.ReadSingle();
			this.invScale.x = 1f / this.size.width;
			this.invScale.y = 1f / this.size.height;
			this.scale = reader.ReadSingle();
		}

		// Token: 0x040005A7 RID: 1447
		public string image;

		// Token: 0x040005A8 RID: 1448
		public Rect size;

		// Token: 0x040005A9 RID: 1449
		public string name;

		// Token: 0x040005AA RID: 1450
		public bool premultipliedAlpha;

		// Token: 0x040005AB RID: 1451
		public float scale;

		// Token: 0x040005AC RID: 1452
		[HideInInspector]
		[NonSerialized]
		public Vector2 invScale;
	}
}
