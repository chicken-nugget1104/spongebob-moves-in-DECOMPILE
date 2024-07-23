using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Yarg
{
	// Token: 0x020000E0 RID: 224
	[Serializable]
	public sealed class FontAtlas : ILoadable
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x0001FC98 File Offset: 0x0001DE98
		public string filename
		{
			get
			{
				if (this.info.files != null)
				{
					foreach (string text in this.info.files)
					{
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x1700008F RID: 143
		public FontAtlas.CharData this[char chr]
		{
			get
			{
				this.BuildCharDictionary();
				FontAtlas.CharData result;
				this.chars.TryGetValue(chr, out result);
				return result;
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001FD10 File Offset: 0x0001DF10
		public int Kerning(int _first, int _second)
		{
			this.BuildKernDictionary();
			int result = 0;
			this.kernSearch.first = _first;
			this.kernSearch.second = _second;
			this.kernings.TryGetValue(this.kernSearch, out result);
			return result;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001FD54 File Offset: 0x0001DF54
		private void BuildCharDictionary()
		{
			if (this.chars == null || this.chars.Count != this.info.count)
			{
				if (this.charArray == null || this.charArray.Length == 0)
				{
					this.Load();
				}
				this.chars = new Dictionary<char, FontAtlas.CharData>(this.charArray.Length);
				for (int i = 0; i < this.charArray.Length; i++)
				{
					FontAtlas.CharData value = this.charArray[i].CharData();
					this.chars[value.letter] = value;
				}
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001FDF4 File Offset: 0x0001DFF4
		private void BuildKernDictionary()
		{
			if (this.kernings == null || this.kernings.Count != this.info.kernCount)
			{
				if (this.kernArray == null || this.kernArray.Length == 0)
				{
					this.Load();
				}
				this.kernings = new Dictionary<FontAtlas.KernPair, int>();
				for (int i = 0; i < this.kernArray.Length; i++)
				{
					FontAtlas.KernData kernData = this.kernArray[i];
					this.kernings[new FontAtlas.KernPair(kernData.first, kernData.second)] = kernData.amount;
				}
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001FE94 File Offset: 0x0001E094
		private static FontAtlas.FontPair GetKeyValuePair(StringReader line, StringBuilder buffer)
		{
			buffer.Length = 0;
			while ((ushort)line.Peek() == 32)
			{
				line.Read();
			}
			string text = null;
			int num = 0;
			bool flag = false;
			while (num != -1)
			{
				num = line.Read();
				char c = (char)num;
				char c2 = c;
				switch (c2)
				{
				case ' ':
					if (text == null)
					{
						text = buffer.ToString();
						buffer.Length = 0;
					}
					if (flag)
					{
						buffer.Append(c);
					}
					else
					{
						num = -1;
					}
					break;
				default:
					if (c2 != '=')
					{
						if (num != -1)
						{
							buffer.Append(c);
						}
					}
					else if (flag)
					{
						buffer.Append(c);
					}
					else
					{
						text = buffer.ToString();
						buffer.Length = 0;
					}
					break;
				case '"':
					if (flag)
					{
						int num2 = line.Peek();
						if ((ushort)num2 == 32 || num2 == -1)
						{
							num = -1;
						}
						else
						{
							buffer.Append(c);
						}
					}
					else
					{
						flag = true;
					}
					break;
				}
			}
			object v;
			if (flag)
			{
				v = buffer.ToString();
			}
			else if (buffer.Length == 0)
			{
				v = null;
			}
			else
			{
				string text2 = buffer.ToString();
				if (text2.IndexOf(',') != -1)
				{
					string[] array = text2.Split(new char[]
					{
						','
					});
					v = Array.ConvertAll<string, int>(array, (string x) => int.Parse(x));
				}
				else
				{
					v = int.Parse(buffer.ToString());
				}
			}
			return new FontAtlas.FontPair(text, v);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00020048 File Offset: 0x0001E248
		public void Load()
		{
			if (this.fnt == null || string.IsNullOrEmpty(this.fnt.text))
			{
				return;
			}
			FontAtlas fontAtlas = FontAtlas.Load(this.fnt.text);
			this.chars = fontAtlas.chars;
			this.charArray = fontAtlas.charArray;
			this.kernArray = fontAtlas.kernArray;
			this.info = fontAtlas.info;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000200C0 File Offset: 0x0001E2C0
		public static FontAtlas Load(string fnt)
		{
			StringBuilder buffer = new StringBuilder();
			FontAtlas fontAtlas = new FontAtlas();
			List<FontAtlas.SerializedCharData> list = new List<FontAtlas.SerializedCharData>();
			List<FontAtlas.KernData> list2 = new List<FontAtlas.KernData>();
			FontAtlas.FontData fontData = new FontAtlas.FontData();
			List<string> list3 = new List<string>();
			using (StringReader stringReader = new StringReader(fnt))
			{
				for (;;)
				{
					string text = stringReader.ReadLine();
					if (text == null)
					{
						break;
					}
					StringReader stringReader2 = new StringReader(text);
					FontAtlas.FontPair keyValuePair = FontAtlas.GetKeyValuePair(stringReader2, buffer);
					if (keyValuePair.key != null)
					{
						string key = keyValuePair.key;
						if (key != null)
						{
							if (FontAtlas.<>f__switch$map2 == null)
							{
								FontAtlas.<>f__switch$map2 = new Dictionary<string, int>(5)
								{
									{
										"info",
										0
									},
									{
										"common",
										1
									},
									{
										"page",
										2
									},
									{
										"char",
										3
									},
									{
										"kerning",
										4
									}
								};
							}
							int num;
							if (FontAtlas.<>f__switch$map2.TryGetValue(key, out num))
							{
								FontAtlas.DATATYPE datatype;
								switch (num)
								{
								case 0:
									datatype = FontAtlas.DATATYPE.INFO;
									break;
								case 1:
									datatype = FontAtlas.DATATYPE.COMMON;
									break;
								case 2:
									datatype = FontAtlas.DATATYPE.PAGE;
									break;
								case 3:
									datatype = FontAtlas.DATATYPE.CHAR;
									break;
								case 4:
									datatype = FontAtlas.DATATYPE.KERN;
									break;
								default:
									continue;
								}
								Dictionary<string, object> dictionary = new Dictionary<string, object>();
								for (;;)
								{
									keyValuePair = FontAtlas.GetKeyValuePair(stringReader2, buffer);
									if (keyValuePair.key == null)
									{
										break;
									}
									dictionary[keyValuePair.key] = keyValuePair.value;
								}
								stringReader2.Dispose();
								switch (datatype)
								{
								case FontAtlas.DATATYPE.INFO:
								{
									fontData.face = (string)dictionary["face"];
									fontData.size = (int)dictionary["size"];
									fontData.bold = ((int)dictionary["bold"] == 1);
									fontData.italic = ((int)dictionary["italic"] == 1);
									fontData.charset = (string)dictionary["charset"];
									fontData.unicode = ((int)dictionary["unicode"] == 1);
									fontData.stretchH = (int)dictionary["stretchH"];
									fontData.smooth = ((int)dictionary["smooth"] == 1);
									fontData.aa = ((int)dictionary["aa"] == 1);
									int[] array = (int[])dictionary["padding"];
									fontData.padding = new RectOffset(array[0], array[1], array[2], array[3]);
									int[] array2 = (int[])dictionary["spacing"];
									fontData.spacing = new Vector2((float)array2[0], (float)array2[1]);
									break;
								}
								case FontAtlas.DATATYPE.COMMON:
									fontData.lineHeight = (int)dictionary["lineHeight"];
									fontData._base = (int)dictionary["base"];
									fontData.scale = new Vector2((float)((int)dictionary["scaleW"]), (float)((int)dictionary["scaleH"]));
									fontData.pages = (int)dictionary["pages"];
									fontData.packed = ((int)dictionary["packed"] == 1);
									break;
								case FontAtlas.DATATYPE.PAGE:
									list3.Add((string)dictionary["file"]);
									break;
								case FontAtlas.DATATYPE.CHAR:
								{
									FontAtlas.SerializedCharData serializedCharData = new FontAtlas.SerializedCharData();
									serializedCharData.chnl = (int)dictionary["chnl"];
									serializedCharData.id = (int)dictionary["id"];
									serializedCharData.size = new Rect((float)((int)dictionary["x"]), (float)((int)dictionary["y"]), (float)((int)dictionary["width"]), (float)((int)dictionary["height"]));
									serializedCharData.offset = new Vector2((float)(-(float)((int)dictionary["xoffset"])), (float)((int)dictionary["yoffset"]));
									serializedCharData.xadvance = (int)dictionary["xadvance"];
									serializedCharData.page = (int)dictionary["page"];
									string text2 = (string)dictionary["letter"];
									if (text2.Equals("space"))
									{
										serializedCharData.letter = 32;
									}
									else
									{
										serializedCharData.letter = (int)text2.ToCharArray()[0];
									}
									list.Add(serializedCharData);
									break;
								}
								case FontAtlas.DATATYPE.KERN:
									list2.Add(new FontAtlas.KernData((int)dictionary["first"], (int)dictionary["second"], (int)dictionary["amount"]));
									break;
								}
							}
						}
					}
				}
			}
			fontData.files = list3.ToArray();
			fontData.count = list.Count;
			fontData.kernCount = list2.Count;
			fontAtlas.info = fontData;
			fontAtlas.charArray = list.ToArray();
			fontAtlas.kernArray = list2.ToArray();
			return fontAtlas;
		}

		// Token: 0x04000537 RID: 1335
		public TextAsset fnt;

		// Token: 0x04000538 RID: 1336
		public Material material;

		// Token: 0x04000539 RID: 1337
		public FontAtlas.FontData info;

		// Token: 0x0400053A RID: 1338
		private Dictionary<FontAtlas.KernPair, int> kernings;

		// Token: 0x0400053B RID: 1339
		private Dictionary<char, FontAtlas.CharData> chars;

		// Token: 0x0400053C RID: 1340
		public FontAtlas.SerializedCharData[] charArray;

		// Token: 0x0400053D RID: 1341
		public FontAtlas.KernData[] kernArray;

		// Token: 0x0400053E RID: 1342
		[NonSerialized]
		private FontAtlas.KernPair kernSearch;

		// Token: 0x020000E1 RID: 225
		[Serializable]
		public sealed class SerializedCharData
		{
			// Token: 0x0600088A RID: 2186 RVA: 0x0002066C File Offset: 0x0001E86C
			public FontAtlas.CharData CharData()
			{
				return new FontAtlas.CharData
				{
					id = this.id,
					size = this.size,
					offset = this.offset,
					xadvance = this.xadvance,
					page = this.page,
					chnl = this.chnl,
					letter = (char)this.letter
				};
			}

			// Token: 0x04000541 RID: 1345
			public int id;

			// Token: 0x04000542 RID: 1346
			public Rect size;

			// Token: 0x04000543 RID: 1347
			public Vector2 offset;

			// Token: 0x04000544 RID: 1348
			public int xadvance;

			// Token: 0x04000545 RID: 1349
			public int page;

			// Token: 0x04000546 RID: 1350
			public int chnl;

			// Token: 0x04000547 RID: 1351
			public int letter;
		}

		// Token: 0x020000E2 RID: 226
		public struct CharData
		{
			// Token: 0x04000548 RID: 1352
			public int id;

			// Token: 0x04000549 RID: 1353
			public Rect size;

			// Token: 0x0400054A RID: 1354
			public Vector2 offset;

			// Token: 0x0400054B RID: 1355
			public int xadvance;

			// Token: 0x0400054C RID: 1356
			public int page;

			// Token: 0x0400054D RID: 1357
			public int chnl;

			// Token: 0x0400054E RID: 1358
			public char letter;
		}

		// Token: 0x020000E3 RID: 227
		[Serializable]
		public sealed class FontData
		{
			// Token: 0x0400054F RID: 1359
			public string face;

			// Token: 0x04000550 RID: 1360
			public int size;

			// Token: 0x04000551 RID: 1361
			public bool bold;

			// Token: 0x04000552 RID: 1362
			public bool italic;

			// Token: 0x04000553 RID: 1363
			public string charset;

			// Token: 0x04000554 RID: 1364
			public bool unicode;

			// Token: 0x04000555 RID: 1365
			public int stretchH;

			// Token: 0x04000556 RID: 1366
			public bool smooth;

			// Token: 0x04000557 RID: 1367
			public bool aa;

			// Token: 0x04000558 RID: 1368
			public RectOffset padding;

			// Token: 0x04000559 RID: 1369
			public Vector2 spacing;

			// Token: 0x0400055A RID: 1370
			public int lineHeight;

			// Token: 0x0400055B RID: 1371
			public int _base;

			// Token: 0x0400055C RID: 1372
			public Vector2 scale;

			// Token: 0x0400055D RID: 1373
			public int pages;

			// Token: 0x0400055E RID: 1374
			public bool packed;

			// Token: 0x0400055F RID: 1375
			public string[] files;

			// Token: 0x04000560 RID: 1376
			public int count;

			// Token: 0x04000561 RID: 1377
			public int kernCount;
		}

		// Token: 0x020000E4 RID: 228
		[Serializable]
		public sealed class KernData
		{
			// Token: 0x0600088C RID: 2188 RVA: 0x000206E8 File Offset: 0x0001E8E8
			public KernData(int _first, int _second, int _amount)
			{
				this.first = _first;
				this.second = _second;
				this.amount = _amount;
			}

			// Token: 0x04000562 RID: 1378
			public int first;

			// Token: 0x04000563 RID: 1379
			public int second;

			// Token: 0x04000564 RID: 1380
			public int amount;
		}

		// Token: 0x020000E5 RID: 229
		private enum DATATYPE
		{
			// Token: 0x04000566 RID: 1382
			INFO,
			// Token: 0x04000567 RID: 1383
			COMMON,
			// Token: 0x04000568 RID: 1384
			PAGE,
			// Token: 0x04000569 RID: 1385
			CHAR,
			// Token: 0x0400056A RID: 1386
			KERN
		}

		// Token: 0x020000E6 RID: 230
		private struct KernPair
		{
			// Token: 0x0600088D RID: 2189 RVA: 0x00020708 File Offset: 0x0001E908
			public KernPair(int _first, int _second)
			{
				this.first = _first;
				this.second = _second;
			}

			// Token: 0x0600088E RID: 2190 RVA: 0x00020718 File Offset: 0x0001E918
			public override int GetHashCode()
			{
				return (this.first * 251 + this.second) * 251 + this.second;
			}

			// Token: 0x0600088F RID: 2191 RVA: 0x00020748 File Offset: 0x0001E948
			public override bool Equals(object other)
			{
				return other is FontAtlas.KernPair && this.Equals((FontAtlas.KernPair)other);
			}

			// Token: 0x06000890 RID: 2192 RVA: 0x00020768 File Offset: 0x0001E968
			public bool Equals(FontAtlas.KernPair other)
			{
				return this.first == other.first && this.second == other.second;
			}

			// Token: 0x0400056B RID: 1387
			public int first;

			// Token: 0x0400056C RID: 1388
			public int second;
		}

		// Token: 0x020000E7 RID: 231
		private struct FontPair
		{
			// Token: 0x06000891 RID: 2193 RVA: 0x0002079C File Offset: 0x0001E99C
			public FontPair(string k, object v)
			{
				this.key = k;
				this.value = v;
			}

			// Token: 0x0400056D RID: 1389
			public string key;

			// Token: 0x0400056E RID: 1390
			public object value;
		}
	}
}
