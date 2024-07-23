using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniJSON
{
	// Token: 0x020000B1 RID: 177
	public static class Json
	{
		// Token: 0x060006D9 RID: 1753 RVA: 0x00019B10 File Offset: 0x00017D10
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return Json.Parser.Parse(json);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00019B20 File Offset: 0x00017D20
		public static string Serialize(object obj)
		{
			return Json.Serializer.Serialize(obj);
		}

		// Token: 0x020000B2 RID: 178
		private sealed class Parser : IDisposable
		{
			// Token: 0x060006DB RID: 1755 RVA: 0x00019B28 File Offset: 0x00017D28
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			// Token: 0x060006DC RID: 1756 RVA: 0x00019B3C File Offset: 0x00017D3C
			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			// Token: 0x060006DD RID: 1757 RVA: 0x00019B60 File Offset: 0x00017D60
			public static object Parse(string jsonString)
			{
				object result;
				using (Json.Parser parser = new Json.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			// Token: 0x060006DE RID: 1758 RVA: 0x00019BB0 File Offset: 0x00017DB0
			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			// Token: 0x060006DF RID: 1759 RVA: 0x00019BC4 File Offset: 0x00017DC4
			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case Json.Parser.TOKEN.NONE:
						goto IL_37;
					default:
						if (nextToken != Json.Parser.TOKEN.COMMA)
						{
							string text = this.ParseString();
							if (text == null)
							{
								goto Block_2;
							}
							if (this.NextToken != Json.Parser.TOKEN.COLON)
							{
								goto Block_3;
							}
							this.json.Read();
							dictionary[text] = this.ParseValue();
						}
						break;
					case Json.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			// Token: 0x060006E0 RID: 1760 RVA: 0x00019C50 File Offset: 0x00017E50
			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					Json.Parser.TOKEN nextToken = this.NextToken;
					Json.Parser.TOKEN token = nextToken;
					switch (token)
					{
					case Json.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						break;
					default:
					{
						if (token == Json.Parser.TOKEN.NONE)
						{
							return null;
						}
						object item = this.ParseByToken(nextToken);
						list.Add(item);
						break;
					}
					case Json.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			// Token: 0x060006E1 RID: 1761 RVA: 0x00019CCC File Offset: 0x00017ECC
			private object ParseValue()
			{
				Json.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			// Token: 0x060006E2 RID: 1762 RVA: 0x00019CE8 File Offset: 0x00017EE8
			private object ParseByToken(Json.Parser.TOKEN token)
			{
				switch (token)
				{
				case Json.Parser.TOKEN.CURLY_OPEN:
					return this.ParseObject();
				case Json.Parser.TOKEN.SQUARED_OPEN:
					return this.ParseArray();
				case Json.Parser.TOKEN.STRING:
					return this.ParseString();
				case Json.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case Json.Parser.TOKEN.TRUE:
					return true;
				case Json.Parser.TOKEN.FALSE:
					return false;
				case Json.Parser.TOKEN.NULL:
					return null;
				}
				return null;
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x00019D60 File Offset: 0x00017F60
			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char nextChar = this.NextChar;
					char c = nextChar;
					if (c != '"')
					{
						if (c != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							char c2 = nextChar;
							switch (c2)
							{
							case 'n':
								stringBuilder.Append('\n');
								break;
							default:
								if (c2 != '"' && c2 != '/' && c2 != '\\')
								{
									if (c2 != 'b')
									{
										if (c2 == 'f')
										{
											stringBuilder.Append('\f');
										}
									}
									else
									{
										stringBuilder.Append('\b');
									}
								}
								else
								{
									stringBuilder.Append(nextChar);
								}
								break;
							case 'r':
								stringBuilder.Append('\r');
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								char[] array = new char[4];
								for (int i = 0; i < 4; i++)
								{
									array[i] = this.NextChar;
								}
								stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
								break;
							}
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060006E4 RID: 1764 RVA: 0x00019EF8 File Offset: 0x000180F8
			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, out num2);
				return num2;
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x00019F3C File Offset: 0x0001813C
			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00019F78 File Offset: 0x00018178
			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00019F8C File Offset: 0x0001818C
			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00019FA0 File Offset: 0x000181A0
			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!Json.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00019FF4 File Offset: 0x000181F4
			private Json.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return Json.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					switch (peekChar)
					{
					case '"':
						return Json.Parser.TOKEN.STRING;
					default:
						switch (peekChar)
						{
						case '[':
							return Json.Parser.TOKEN.SQUARED_OPEN;
						default:
						{
							switch (peekChar)
							{
							case '{':
								return Json.Parser.TOKEN.CURLY_OPEN;
							case '}':
								this.json.Read();
								return Json.Parser.TOKEN.CURLY_CLOSE;
							}
							string nextWord = this.NextWord;
							switch (nextWord)
							{
							case "false":
								return Json.Parser.TOKEN.FALSE;
							case "true":
								return Json.Parser.TOKEN.TRUE;
							case "null":
								return Json.Parser.TOKEN.NULL;
							}
							return Json.Parser.TOKEN.NONE;
						}
						case ']':
							this.json.Read();
							return Json.Parser.TOKEN.SQUARED_CLOSE;
						}
						break;
					case ',':
						this.json.Read();
						return Json.Parser.TOKEN.COMMA;
					case '-':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						return Json.Parser.TOKEN.NUMBER;
					case ':':
						return Json.Parser.TOKEN.COLON;
					}
				}
			}

			// Token: 0x0400044E RID: 1102
			private const string WORD_BREAK = "{}[],:\"";

			// Token: 0x0400044F RID: 1103
			private StringReader json;

			// Token: 0x020000B3 RID: 179
			private enum TOKEN
			{
				// Token: 0x04000452 RID: 1106
				NONE,
				// Token: 0x04000453 RID: 1107
				CURLY_OPEN,
				// Token: 0x04000454 RID: 1108
				CURLY_CLOSE,
				// Token: 0x04000455 RID: 1109
				SQUARED_OPEN,
				// Token: 0x04000456 RID: 1110
				SQUARED_CLOSE,
				// Token: 0x04000457 RID: 1111
				COLON,
				// Token: 0x04000458 RID: 1112
				COMMA,
				// Token: 0x04000459 RID: 1113
				STRING,
				// Token: 0x0400045A RID: 1114
				NUMBER,
				// Token: 0x0400045B RID: 1115
				TRUE,
				// Token: 0x0400045C RID: 1116
				FALSE,
				// Token: 0x0400045D RID: 1117
				NULL
			}
		}

		// Token: 0x020000B4 RID: 180
		private sealed class Serializer
		{
			// Token: 0x060006EA RID: 1770 RVA: 0x0001A16C File Offset: 0x0001836C
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			// Token: 0x060006EB RID: 1771 RVA: 0x0001A180 File Offset: 0x00018380
			public static string Serialize(object obj)
			{
				Json.Serializer serializer = new Json.Serializer();
				serializer.SerializeValue(obj);
				return serializer.builder.ToString();
			}

			// Token: 0x060006EC RID: 1772 RVA: 0x0001A1A8 File Offset: 0x000183A8
			private void SerializeValue(object value)
			{
				string str;
				IList anArray;
				IDictionary obj;
				if (value == null)
				{
					this.builder.Append("null");
				}
				else if ((str = (value as string)) != null)
				{
					this.SerializeString(str);
				}
				else if (value is bool)
				{
					this.builder.Append((!(bool)value) ? "false" : "true");
				}
				else if ((anArray = (value as IList)) != null)
				{
					this.SerializeArray(anArray);
				}
				else if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj);
				}
				else if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
				}
				else
				{
					this.SerializeOther(value);
				}
			}

			// Token: 0x060006ED RID: 1773 RVA: 0x0001A27C File Offset: 0x0001847C
			private void SerializeObject(IDictionary obj)
			{
				bool flag = true;
				this.builder.Append('{');
				foreach (object obj2 in obj.Keys)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeString(obj2.ToString());
					this.builder.Append(':');
					this.SerializeValue(obj[obj2]);
					flag = false;
				}
				this.builder.Append('}');
			}

			// Token: 0x060006EE RID: 1774 RVA: 0x0001A33C File Offset: 0x0001853C
			private void SerializeArray(IList anArray)
			{
				this.builder.Append('[');
				bool flag = true;
				foreach (object value in anArray)
				{
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value);
					flag = false;
				}
				this.builder.Append(']');
			}

			// Token: 0x060006EF RID: 1775 RVA: 0x0001A3D8 File Offset: 0x000185D8
			private void SerializeString(string str)
			{
				this.builder.Append('"');
				char[] array = str.ToCharArray();
				foreach (char c in array)
				{
					char c2 = c;
					switch (c2)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					default:
						if (c2 != '"')
						{
							if (c2 != '\\')
							{
								int num = Convert.ToInt32(c);
								if (num >= 32 && num <= 126)
								{
									this.builder.Append(c);
								}
								else
								{
									this.builder.Append("\\u");
									this.builder.Append(num.ToString("x4"));
								}
							}
							else
							{
								this.builder.Append("\\\\");
							}
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					}
				}
				this.builder.Append('"');
			}

			// Token: 0x060006F0 RID: 1776 RVA: 0x0001A554 File Offset: 0x00018754
			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R"));
				}
				else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
				}
				else if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R"));
				}
				else
				{
					this.SerializeString(value.ToString());
				}
			}

			// Token: 0x0400045E RID: 1118
			private StringBuilder builder;
		}
	}
}
