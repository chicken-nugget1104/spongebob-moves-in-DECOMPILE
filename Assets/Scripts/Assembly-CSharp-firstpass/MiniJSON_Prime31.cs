using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Token: 0x020000B5 RID: 181
public class MiniJSON_Prime31
{
	// Token: 0x060006F3 RID: 1779 RVA: 0x0001A65C File Offset: 0x0001885C
	public static object jsonDecode(string json)
	{
		MiniJSON_Prime31.lastDecode = json;
		if (json != null)
		{
			char[] json2 = json.ToCharArray();
			int num = 0;
			bool flag = true;
			object result = MiniJSON_Prime31.parseValue(json2, ref num, ref flag);
			if (flag)
			{
				MiniJSON_Prime31.lastErrorIndex = -1;
			}
			else
			{
				MiniJSON_Prime31.lastErrorIndex = num;
			}
			return result;
		}
		return null;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0001A6A8 File Offset: 0x000188A8
	public static string jsonEncode(object json)
	{
		StringBuilder stringBuilder = new StringBuilder(2000);
		bool flag = MiniJSON_Prime31.serializeValue(json, stringBuilder);
		return (!flag) ? null : stringBuilder.ToString();
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0001A6DC File Offset: 0x000188DC
	public static bool lastDecodeSuccessful()
	{
		return MiniJSON_Prime31.lastErrorIndex == -1;
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0001A6E8 File Offset: 0x000188E8
	public static int getLastErrorIndex()
	{
		return MiniJSON_Prime31.lastErrorIndex;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0001A6F0 File Offset: 0x000188F0
	public static string getLastErrorSnippet()
	{
		if (MiniJSON_Prime31.lastErrorIndex == -1)
		{
			return string.Empty;
		}
		int num = MiniJSON_Prime31.lastErrorIndex - 5;
		int num2 = MiniJSON_Prime31.lastErrorIndex + 15;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= MiniJSON_Prime31.lastDecode.Length)
		{
			num2 = MiniJSON_Prime31.lastDecode.Length - 1;
		}
		return MiniJSON_Prime31.lastDecode.Substring(num, num2 - num + 1);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0001A758 File Offset: 0x00018958
	protected static Hashtable parseObject(char[] json, ref int index)
	{
		Hashtable hashtable = new Hashtable();
		MiniJSON_Prime31.nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = MiniJSON_Prime31.lookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				MiniJSON_Prime31.nextToken(json, ref index);
			}
			else
			{
				if (num == 2)
				{
					MiniJSON_Prime31.nextToken(json, ref index);
					return hashtable;
				}
				string text = MiniJSON_Prime31.parseString(json, ref index);
				if (text == null)
				{
					return null;
				}
				num = MiniJSON_Prime31.nextToken(json, ref index);
				if (num != 5)
				{
					return null;
				}
				bool flag2 = true;
				object value = MiniJSON_Prime31.parseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				hashtable[text] = value;
			}
		}
		return hashtable;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0001A7F8 File Offset: 0x000189F8
	protected static ArrayList parseArray(char[] json, ref int index)
	{
		ArrayList arrayList = new ArrayList();
		MiniJSON_Prime31.nextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = MiniJSON_Prime31.lookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				MiniJSON_Prime31.nextToken(json, ref index);
			}
			else
			{
				if (num == 4)
				{
					MiniJSON_Prime31.nextToken(json, ref index);
					break;
				}
				bool flag2 = true;
				object value = MiniJSON_Prime31.parseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				arrayList.Add(value);
			}
		}
		return arrayList;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0001A878 File Offset: 0x00018A78
	protected static object parseValue(char[] json, ref int index, ref bool success)
	{
		switch (MiniJSON_Prime31.lookAhead(json, index))
		{
		case 1:
			return MiniJSON_Prime31.parseObject(json, ref index);
		case 3:
			return MiniJSON_Prime31.parseArray(json, ref index);
		case 7:
			return MiniJSON_Prime31.parseString(json, ref index);
		case 8:
			return MiniJSON_Prime31.parseNumber(json, ref index);
		case 9:
			MiniJSON_Prime31.nextToken(json, ref index);
			return bool.Parse("TRUE");
		case 10:
			MiniJSON_Prime31.nextToken(json, ref index);
			return bool.Parse("FALSE");
		case 11:
			MiniJSON_Prime31.nextToken(json, ref index);
			return null;
		}
		success = false;
		return null;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0001A934 File Offset: 0x00018B34
	protected static string parseString(char[] json, ref int index)
	{
		string text = string.Empty;
		MiniJSON_Prime31.eatWhitespace(json, ref index);
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
					text = text + "&#x" + new string(array) + ";";
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

	// Token: 0x060006FC RID: 1788 RVA: 0x0001AB00 File Offset: 0x00018D00
	protected static double parseNumber(char[] json, ref int index)
	{
		MiniJSON_Prime31.eatWhitespace(json, ref index);
		int lastIndexOfNumber = MiniJSON_Prime31.getLastIndexOfNumber(json, index);
		int num = lastIndexOfNumber - index + 1;
		char[] array = new char[num];
		Array.Copy(json, index, array, 0, num);
		index = lastIndexOfNumber + 1;
		return double.Parse(new string(array));
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0001AB48 File Offset: 0x00018D48
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

	// Token: 0x060006FE RID: 1790 RVA: 0x0001AB84 File Offset: 0x00018D84
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

	// Token: 0x060006FF RID: 1791 RVA: 0x0001ABC0 File Offset: 0x00018DC0
	protected static int lookAhead(char[] json, int index)
	{
		int num = index;
		return MiniJSON_Prime31.nextToken(json, ref num);
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0001ABD8 File Offset: 0x00018DD8
	protected static int nextToken(char[] json, ref int index)
	{
		MiniJSON_Prime31.eatWhitespace(json, ref index);
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

	// Token: 0x06000701 RID: 1793 RVA: 0x0001AD94 File Offset: 0x00018F94
	protected static bool serializeObjectOrArray(object objectOrArray, StringBuilder builder)
	{
		if (objectOrArray is Hashtable)
		{
			return MiniJSON_Prime31.serializeObject((Hashtable)objectOrArray, builder);
		}
		return objectOrArray is ArrayList && MiniJSON_Prime31.serializeArray((ArrayList)objectOrArray, builder);
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0001ADC8 File Offset: 0x00018FC8
	protected static bool serializeObject(Hashtable anObject, StringBuilder builder)
	{
		builder.Append("{");
		IDictionaryEnumerator enumerator = anObject.GetEnumerator();
		bool flag = true;
		while (enumerator.MoveNext())
		{
			string aString = enumerator.Key.ToString();
			object value = enumerator.Value;
			if (!flag)
			{
				builder.Append(", ");
			}
			MiniJSON_Prime31.serializeString(aString, builder);
			builder.Append(":");
			if (!MiniJSON_Prime31.serializeValue(value, builder))
			{
				return false;
			}
			flag = false;
		}
		builder.Append("}");
		return true;
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0001AE50 File Offset: 0x00019050
	protected static bool serializeDictionary(Dictionary<string, string> dict, StringBuilder builder)
	{
		builder.Append("{");
		bool flag = true;
		foreach (KeyValuePair<string, string> keyValuePair in dict)
		{
			if (!flag)
			{
				builder.Append(", ");
			}
			MiniJSON_Prime31.serializeString(keyValuePair.Key, builder);
			builder.Append(":");
			MiniJSON_Prime31.serializeString(keyValuePair.Value, builder);
			flag = false;
		}
		builder.Append("}");
		return true;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0001AF00 File Offset: 0x00019100
	protected static bool serializeArray(ArrayList anArray, StringBuilder builder)
	{
		builder.Append("[");
		bool flag = true;
		for (int i = 0; i < anArray.Count; i++)
		{
			object value = anArray[i];
			if (!flag)
			{
				builder.Append(", ");
			}
			if (!MiniJSON_Prime31.serializeValue(value, builder))
			{
				return false;
			}
			flag = false;
		}
		builder.Append("]");
		return true;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0001AF6C File Offset: 0x0001916C
	protected static bool serializeValue(object value, StringBuilder builder)
	{
		if (value == null)
		{
			builder.Append("null");
		}
		else if (value.GetType().IsArray)
		{
			MiniJSON_Prime31.serializeArray(new ArrayList((ICollection)value), builder);
		}
		else if (value is string)
		{
			MiniJSON_Prime31.serializeString((string)value, builder);
		}
		else if (value is char)
		{
			MiniJSON_Prime31.serializeString(Convert.ToString((char)value), builder);
		}
		else if (value is Hashtable)
		{
			MiniJSON_Prime31.serializeObject((Hashtable)value, builder);
		}
		else if (value is Dictionary<string, string>)
		{
			MiniJSON_Prime31.serializeDictionary((Dictionary<string, string>)value, builder);
		}
		else if (value is ArrayList)
		{
			MiniJSON_Prime31.serializeArray((ArrayList)value, builder);
		}
		else if (value is bool && (bool)value)
		{
			builder.Append("true");
		}
		else if (value is bool && !(bool)value)
		{
			builder.Append("false");
		}
		else
		{
			if (!value.GetType().IsPrimitive)
			{
				return false;
			}
			MiniJSON_Prime31.serializeNumber(Convert.ToDouble(value), builder);
		}
		return true;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0001B0C0 File Offset: 0x000192C0
	protected static void serializeString(string aString, StringBuilder builder)
	{
		builder.Append("\"");
		foreach (char c in aString.ToCharArray())
		{
			if (c == '"')
			{
				builder.Append("\\\"");
			}
			else if (c == '\\')
			{
				builder.Append("\\\\");
			}
			else if (c == '\b')
			{
				builder.Append("\\b");
			}
			else if (c == '\f')
			{
				builder.Append("\\f");
			}
			else if (c == '\n')
			{
				builder.Append("\\n");
			}
			else if (c == '\r')
			{
				builder.Append("\\r");
			}
			else if (c == '\t')
			{
				builder.Append("\\t");
			}
			else
			{
				int num = Convert.ToInt32(c);
				if (num >= 32 && num <= 126)
				{
					builder.Append(c);
				}
				else
				{
					builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
				}
			}
		}
		builder.Append("\"");
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0001B1F8 File Offset: 0x000193F8
	protected static void serializeNumber(double number, StringBuilder builder)
	{
		builder.Append(Convert.ToString(number));
	}

	// Token: 0x0400045F RID: 1119
	private const int TOKEN_NONE = 0;

	// Token: 0x04000460 RID: 1120
	private const int TOKEN_CURLY_OPEN = 1;

	// Token: 0x04000461 RID: 1121
	private const int TOKEN_CURLY_CLOSE = 2;

	// Token: 0x04000462 RID: 1122
	private const int TOKEN_SQUARED_OPEN = 3;

	// Token: 0x04000463 RID: 1123
	private const int TOKEN_SQUARED_CLOSE = 4;

	// Token: 0x04000464 RID: 1124
	private const int TOKEN_COLON = 5;

	// Token: 0x04000465 RID: 1125
	private const int TOKEN_COMMA = 6;

	// Token: 0x04000466 RID: 1126
	private const int TOKEN_STRING = 7;

	// Token: 0x04000467 RID: 1127
	private const int TOKEN_NUMBER = 8;

	// Token: 0x04000468 RID: 1128
	private const int TOKEN_TRUE = 9;

	// Token: 0x04000469 RID: 1129
	private const int TOKEN_FALSE = 10;

	// Token: 0x0400046A RID: 1130
	private const int TOKEN_NULL = 11;

	// Token: 0x0400046B RID: 1131
	private const int BUILDER_CAPACITY = 2000;

	// Token: 0x0400046C RID: 1132
	protected static int lastErrorIndex = -1;

	// Token: 0x0400046D RID: 1133
	protected static string lastDecode = string.Empty;
}
