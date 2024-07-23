using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Yarg
{
	// Token: 0x020000EE RID: 238
	public sealed class AtlasCoords
	{
		// Token: 0x060008FE RID: 2302 RVA: 0x000224C4 File Offset: 0x000206C4
		public AtlasCoords()
		{
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000224CC File Offset: 0x000206CC
		public AtlasCoords(string key, Dictionary<string, object> source)
		{
			this.name = key;
			Dictionary<string, object> dictionary = (Dictionary<string, object>)source["frame"];
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)source["spriteOffset"];
			Dictionary<string, object> dictionary3 = (Dictionary<string, object>)source["spriteSize"];
			this.rotated = (bool)source["rotated"];
			this.trimmed = (bool)source["trimmed"];
			if (dictionary != null)
			{
				this.frame = new Rect(Convert.ToSingle(dictionary["x"]), Convert.ToSingle(dictionary["y"]), Convert.ToSingle(dictionary["w"]), Convert.ToSingle(dictionary["h"]));
			}
			if (dictionary3 != null)
			{
				this.spriteSize = new Rect(0f, 0f, Convert.ToSingle(dictionary3["w"]), Convert.ToSingle(dictionary3["h"]));
			}
			Dictionary<string, object> dictionary4 = (Dictionary<string, object>)source["sourceSize"];
			if (dictionary4 != null)
			{
				this.spriteSourceSize = new Vector2(Convert.ToSingle(dictionary4["w"]), Convert.ToSingle(dictionary4["h"]));
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00022614 File Offset: 0x00020814
		public AtlasCoords(BinaryReader reader, int version)
		{
			this.name = TextureAtlas._ReadString(reader);
			this.frame.x = reader.ReadSingle();
			this.frame.y = reader.ReadSingle();
			this.frame.width = reader.ReadSingle();
			this.frame.height = reader.ReadSingle();
			this.spriteSize.x = reader.ReadSingle();
			this.spriteSize.y = reader.ReadSingle();
			this.spriteSize.width = reader.ReadSingle();
			this.spriteSize.height = reader.ReadSingle();
			if (version == 2)
			{
				reader.ReadSingle();
				reader.ReadSingle();
			}
			this.spriteSourceSize.x = reader.ReadSingle();
			this.spriteSourceSize.y = reader.ReadSingle();
			this.properties = reader.ReadByte();
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x00022714 File Offset: 0x00020914
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x00022700 File Offset: 0x00020900
		public bool trimmed
		{
			get
			{
				return (this.properties & 1) == 1;
			}
			set
			{
				this.properties |= 1;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x00022738 File Offset: 0x00020938
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x00022724 File Offset: 0x00020924
		public bool rotated
		{
			get
			{
				return (this.properties & 2) == 2;
			}
			set
			{
				this.properties |= 2;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0002275C File Offset: 0x0002095C
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x00022748 File Offset: 0x00020948
		public bool processed
		{
			get
			{
				return (this.properties & 4) == 4;
			}
			set
			{
				this.properties |= 4;
			}
		}

		// Token: 0x040005A2 RID: 1442
		public string name;

		// Token: 0x040005A3 RID: 1443
		public Rect frame;

		// Token: 0x040005A4 RID: 1444
		public Vector2 spriteSourceSize;

		// Token: 0x040005A5 RID: 1445
		public Rect spriteSize;

		// Token: 0x040005A6 RID: 1446
		public byte properties;
	}
}
