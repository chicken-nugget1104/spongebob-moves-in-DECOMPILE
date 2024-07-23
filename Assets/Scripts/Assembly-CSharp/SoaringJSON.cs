using System;
using System.Globalization;

// Token: 0x020003D4 RID: 980
public class SoaringJSON
{
	// Token: 0x06001D71 RID: 7537 RVA: 0x000B900C File Offset: 0x000B720C
	public static SoaringDictionary jsonDecode(string json, SoaringDictionary tables)
	{
		SoaringDictionary result = null;
		if (json != null)
		{
			char[] json2 = json.ToCharArray();
			int num = 0;
			result = SoaringJSON.parseObject(json2, ref num, tables);
		}
		return result;
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000B9038 File Offset: 0x000B7238
	public static SoaringDictionary jsonDecode(byte[] json, SoaringDictionary tables)
	{
		SoaringDictionary result = null;
		if (json != null)
		{
			int num = 0;
			result = SoaringJSON.parseObjectRaw(json, ref num, tables);
		}
		return result;
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x000B905C File Offset: 0x000B725C
	protected static SoaringDictionary parseObject(char[] json, ref int index, SoaringDictionary table)
	{
		if (table == null)
		{
			table = new SoaringDictionary();
		}
		SoaringJSON.nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = SoaringJSON.lookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				SoaringJSON.nextToken(json, ref index);
			}
			else
			{
				if (num == 2)
				{
					SoaringJSON.nextToken(json, ref index);
					return table;
				}
				string text = SoaringJSON.parseString(json, ref index);
				if (text == null)
				{
					return null;
				}
				num = SoaringJSON.nextToken(json, ref index);
				if (num != 5)
				{
					return null;
				}
				bool flag2 = true;
				SoaringObjectBase val = SoaringJSON.parseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				table.addValue(val, text);
			}
		}
		return table;
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x000B9104 File Offset: 0x000B7304
	protected static SoaringDictionary parseObjectRaw(byte[] json, ref int index, SoaringDictionary table)
	{
		if (table == null)
		{
			table = new SoaringDictionary();
		}
		SoaringJSON.nextTokenRaw(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = SoaringJSON.lookAheadRaw(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				SoaringJSON.nextTokenRaw(json, ref index);
			}
			else
			{
				if (num == 2)
				{
					SoaringJSON.nextTokenRaw(json, ref index);
					return table;
				}
				string text = SoaringJSON.parseStringRaw(json, ref index);
				if (text == null)
				{
					return null;
				}
				num = SoaringJSON.nextTokenRaw(json, ref index);
				if (num != 5)
				{
					return null;
				}
				bool flag2 = true;
				SoaringObjectBase val = SoaringJSON.parseValueRaw(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				table.addValue(val, text);
			}
		}
		return table;
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x000B91AC File Offset: 0x000B73AC
	protected static SoaringArray parseArray(char[] json, ref int index)
	{
		SoaringArray soaringArray = new SoaringArray();
		SoaringJSON.nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = SoaringJSON.lookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				SoaringJSON.nextToken(json, ref index);
			}
			else
			{
				if (num == 4)
				{
					SoaringJSON.nextToken(json, ref index);
					break;
				}
				bool flag2 = true;
				SoaringObjectBase obj = SoaringJSON.parseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				soaringArray.addObject(obj);
			}
		}
		return soaringArray;
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x000B922C File Offset: 0x000B742C
	protected static SoaringArray parseArrayRaw(byte[] json, ref int index)
	{
		SoaringArray soaringArray = new SoaringArray();
		SoaringJSON.nextTokenRaw(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = SoaringJSON.lookAheadRaw(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				SoaringJSON.nextTokenRaw(json, ref index);
			}
			else
			{
				if (num == 4)
				{
					SoaringJSON.nextTokenRaw(json, ref index);
					break;
				}
				bool flag2 = true;
				SoaringObjectBase obj = SoaringJSON.parseValueRaw(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				soaringArray.addObject(obj);
			}
		}
		return soaringArray;
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x000B92AC File Offset: 0x000B74AC
	protected static SoaringObjectBase parseValue(char[] json, ref int index, ref bool success)
	{
		switch (SoaringJSON.lookAhead(json, index))
		{
		case 1:
			return SoaringJSON.parseObject(json, ref index, null);
		case 3:
			return SoaringJSON.parseArray(json, ref index);
		case 7:
			return new SoaringValue(SoaringJSON.parseString(json, ref index));
		case 8:
			return SoaringJSON.parseNumber(json, ref index);
		case 9:
			SoaringJSON.nextToken(json, ref index);
			return new SoaringValue(true);
		case 10:
			SoaringJSON.nextToken(json, ref index);
			return new SoaringValue(false);
		case 11:
			SoaringJSON.nextToken(json, ref index);
			return new SoaringNullValue();
		}
		success = false;
		return null;
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x000B9358 File Offset: 0x000B7558
	protected static SoaringObjectBase parseValueRaw(byte[] json, ref int index, ref bool success)
	{
		switch (SoaringJSON.lookAheadRaw(json, index))
		{
		case 1:
			return SoaringJSON.parseObjectRaw(json, ref index, null);
		case 3:
			return SoaringJSON.parseArrayRaw(json, ref index);
		case 7:
			return new SoaringValue(SoaringJSON.parseStringRaw(json, ref index));
		case 8:
			return SoaringJSON.parseNumberRaw(json, ref index);
		case 9:
			SoaringJSON.nextTokenRaw(json, ref index);
			return new SoaringValue(true);
		case 10:
			SoaringJSON.nextTokenRaw(json, ref index);
			return new SoaringValue(false);
		case 11:
			SoaringJSON.nextTokenRaw(json, ref index);
			return new SoaringNullValue();
		}
		success = false;
		return null;
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x000B9404 File Offset: 0x000B7604
	protected static string parseString(char[] json, ref int index)
	{
		string text = string.Empty;
		SoaringJSON.eatWhitespace(json, ref index);
		char c = json[index++];
		bool flag = false;
		while (!flag)
		{
			if (index == json.Length)
			{
				break;
			}
			c = json[index++];
			if (c == '"')
			{
				flag = true;
				break;
			}
			if (c == '\\')
			{
				if (index == json.Length)
				{
					break;
				}
				c = json[index++];
				if (c == '"')
				{
					text += '"';
				}
				else if (c == '\\')
				{
					text += '\\';
				}
				else if (c == '/')
				{
					text += '/';
				}
				else if (c == 'b')
				{
					text += '\b';
				}
				else if (c == 'f')
				{
					text += '\f';
				}
				else if (c == 'n')
				{
					text += '\n';
				}
				else if (c == 'r')
				{
					text += '\r';
				}
				else if (c == 't')
				{
					text += '\t';
				}
				else if (c == 'u')
				{
					int num = json.Length - index;
					if (num < 4)
					{
						break;
					}
					char[] array = new char[4];
					Array.Copy(json, index, array, 0, 4);
					uint utf = uint.Parse(new string(array), NumberStyles.HexNumber);
					text += char.ConvertFromUtf32((int)utf);
					index += 4;
				}
			}
			else
			{
				text += c;
			}
		}
		if (!flag)
		{
			return null;
		}
		return text;
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x000B95D8 File Offset: 0x000B77D8
	protected static string parseStringRaw(byte[] json, ref int index)
	{
		string text = string.Empty;
		SoaringJSON.eatWhitespaceRaw(json, ref index);
		char c = (char)json[index++];
		bool flag = false;
		while (!flag)
		{
			if (index == json.Length)
			{
				break;
			}
			c = (char)json[index++];
			if (c == '"')
			{
				flag = true;
				break;
			}
			if (c == '\\')
			{
				if (index == json.Length)
				{
					break;
				}
				c = (char)json[index++];
				if (c == '"')
				{
					text += '"';
				}
				else if (c == '\\')
				{
					text += '\\';
				}
				else if (c == '/')
				{
					text += '/';
				}
				else if (c == 'b')
				{
					text += '\b';
				}
				else if (c == 'f')
				{
					text += '\f';
				}
				else if (c == 'n')
				{
					text += '\n';
				}
				else if (c == 'r')
				{
					text += '\r';
				}
				else if (c == 't')
				{
					text += '\t';
				}
				else if (c == 'u')
				{
					int num = json.Length - index;
					if (num < 4)
					{
						break;
					}
					char[] array = new char[4];
					Array.Copy(json, index, array, 0, 4);
					uint utf = uint.Parse(new string(array), NumberStyles.HexNumber);
					text += char.ConvertFromUtf32((int)utf);
					index += 4;
				}
			}
			else
			{
				text += c;
			}
		}
		if (!flag)
		{
			return null;
		}
		return text;
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x000B97B0 File Offset: 0x000B79B0
	protected static SoaringValue parseNumber(char[] json, ref int index)
	{
		SoaringJSON.eatWhitespace(json, ref index);
		int lastIndexOfNumber = SoaringJSON.getLastIndexOfNumber(json, index);
		int num = lastIndexOfNumber - index + 1;
		char[] array = new char[num];
		Array.Copy(json, index, array, 0, num);
		index = lastIndexOfNumber + 1;
		double num2 = double.Parse(new string(array));
		SoaringValue result;
		if (num2 == (double)((long)num2))
		{
			result = new SoaringValue((long)num2);
		}
		else
		{
			result = new SoaringValue(num2);
		}
		return result;
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x000B981C File Offset: 0x000B7A1C
	protected static SoaringValue parseNumberRaw(byte[] json, ref int index)
	{
		SoaringJSON.eatWhitespaceRaw(json, ref index);
		int lastIndexOfNumberRaw = SoaringJSON.getLastIndexOfNumberRaw(json, index);
		int num = lastIndexOfNumberRaw - index + 1;
		char[] array = new char[num];
		Array.Copy(json, index, array, 0, num);
		index = lastIndexOfNumberRaw + 1;
		double num2 = double.Parse(new string(array));
		SoaringValue result;
		if (num2 == (double)((long)num2))
		{
			result = new SoaringValue((long)num2);
		}
		else
		{
			result = new SoaringValue(num2);
		}
		return result;
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x000B9888 File Offset: 0x000B7A88
	protected static int getLastIndexOfNumber(char[] json, int index)
	{
		int i;
		for (i = index; i < json.Length; i++)
		{
			if ("0123456789+-.eE".IndexOf(json[i]) == -1)
			{
				break;
			}
		}
		return i - 1;
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x000B98C4 File Offset: 0x000B7AC4
	protected static int getLastIndexOfNumberRaw(byte[] json, int index)
	{
		int i;
		for (i = index; i < json.Length; i++)
		{
			if ("0123456789+-.eE".IndexOf((char)json[i]) == -1)
			{
				break;
			}
		}
		return i - 1;
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x000B9904 File Offset: 0x000B7B04
	protected static void eatWhitespace(char[] json, ref int index)
	{
		while (index < json.Length)
		{
			if (" \t\n\r".IndexOf(json[index]) == -1)
			{
				break;
			}
			index++;
		}
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x000B9940 File Offset: 0x000B7B40
	protected static void eatWhitespaceRaw(byte[] json, ref int index)
	{
		while (index < json.Length)
		{
			if (" \t\n\r".IndexOf((char)json[index]) == -1)
			{
				break;
			}
			index++;
		}
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x000B9974 File Offset: 0x000B7B74
	protected static int lookAhead(char[] json, int index)
	{
		int num = index;
		return SoaringJSON.nextToken(json, ref num);
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x000B998C File Offset: 0x000B7B8C
	protected static int lookAheadRaw(byte[] json, int index)
	{
		int num = index;
		return SoaringJSON.nextTokenRaw(json, ref num);
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x000B99A4 File Offset: 0x000B7BA4
	protected static int nextToken(char[] json, ref int index)
	{
		SoaringJSON.eatWhitespace(json, ref index);
		if (index == json.Length)
		{
			return 0;
		}
		char c = json[index];
		index++;
		char c2 = c;
		switch (c2)
		{
		case '"':
			return 7;
		default:
			switch (c2)
			{
			case '[':
				return 3;
			default:
			{
				switch (c2)
				{
				case '{':
					return 1;
				case '}':
					return 2;
				}
				index--;
				int num = json.Length - index;
				if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
				{
					index += 5;
					return 10;
				}
				if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
				{
					index += 4;
					return 9;
				}
				if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
				{
					index += 4;
					return 11;
				}
				return 0;
			}
			case ']':
				return 4;
			}
			break;
		case ',':
			return 6;
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
			return 8;
		case ':':
			return 5;
		}
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x000B9B60 File Offset: 0x000B7D60
	protected static int nextTokenRaw(byte[] json, ref int index)
	{
		SoaringJSON.eatWhitespaceRaw(json, ref index);
		if (index == json.Length)
		{
			return 0;
		}
		char c = (char)json[index];
		index++;
		char c2 = c;
		switch (c2)
		{
		case '"':
			return 7;
		default:
			switch (c2)
			{
			case '[':
				return 3;
			default:
			{
				switch (c2)
				{
				case '{':
					return 1;
				case '}':
					return 2;
				}
				index--;
				int num = json.Length - index;
				if (num >= 5 && json[index] == 102 && json[index + 1] == 97 && json[index + 2] == 108 && json[index + 3] == 115 && json[index + 4] == 101)
				{
					index += 5;
					return 10;
				}
				if (num >= 4 && json[index] == 116 && json[index + 1] == 114 && json[index + 2] == 117 && json[index + 3] == 101)
				{
					index += 4;
					return 9;
				}
				if (num >= 4 && json[index] == 110 && json[index + 1] == 117 && json[index + 2] == 108 && json[index + 3] == 108)
				{
					index += 4;
					return 11;
				}
				return 0;
			}
			case ']':
				return 4;
			}
			break;
		case ',':
			return 6;
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
			return 8;
		case ':':
			return 5;
		}
	}

	// Token: 0x04001298 RID: 4760
	private const int TOKEN_NONE = 0;

	// Token: 0x04001299 RID: 4761
	private const int TOKEN_CURLY_OPEN = 1;

	// Token: 0x0400129A RID: 4762
	private const int TOKEN_CURLY_CLOSE = 2;

	// Token: 0x0400129B RID: 4763
	private const int TOKEN_SQUARED_OPEN = 3;

	// Token: 0x0400129C RID: 4764
	private const int TOKEN_SQUARED_CLOSE = 4;

	// Token: 0x0400129D RID: 4765
	private const int TOKEN_COLON = 5;

	// Token: 0x0400129E RID: 4766
	private const int TOKEN_COMMA = 6;

	// Token: 0x0400129F RID: 4767
	private const int TOKEN_STRING = 7;

	// Token: 0x040012A0 RID: 4768
	private const int TOKEN_NUMBER = 8;

	// Token: 0x040012A1 RID: 4769
	private const int TOKEN_TRUE = 9;

	// Token: 0x040012A2 RID: 4770
	private const int TOKEN_FALSE = 10;

	// Token: 0x040012A3 RID: 4771
	private const int TOKEN_NULL = 11;

	// Token: 0x040012A4 RID: 4772
	private const int BUILDER_CAPACITY = 2000;
}
