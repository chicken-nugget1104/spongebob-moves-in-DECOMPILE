using System;
using System.Collections;

// Token: 0x020000AE RID: 174
public class XMLParser
{
	// Token: 0x060006D2 RID: 1746 RVA: 0x000192DC File Offset: 0x000174DC
	public object Parse(string content)
	{
		XMLNode xmlnode = new XMLNode();
		xmlnode["_text"] = string.Empty;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		XMLNodeList xmlnodeList = new XMLNodeList();
		XMLNode xmlnode2 = xmlnode;
		for (int i = 0; i < content.Length; i++)
		{
			char c = content[i];
			char c2 = 'x';
			char c3 = 'x';
			char c4 = 'x';
			if (i + 1 < content.Length)
			{
				c2 = content[i + 1];
			}
			if (i + 2 < content.Length)
			{
				c3 = content[i + 2];
			}
			if (i > 0)
			{
				c4 = content[i - 1];
			}
			if (flag6)
			{
				if (c == this.QMARK && c2 == this.GT)
				{
					flag6 = false;
					i++;
				}
			}
			else if (!flag5 && c == this.LT && c2 == this.QMARK)
			{
				flag6 = true;
			}
			else if (flag7)
			{
				if (c4 == this.DASH && c == this.DASH && c2 == this.GT)
				{
					flag7 = false;
					i++;
				}
			}
			else if (!flag5 && c == this.LT && c2 == this.EXCLAMATION)
			{
				if (content.Length > i + 9 && content.Substring(i, 9) == "<![CDATA[")
				{
					flag8 = true;
					i += 8;
				}
				else
				{
					flag7 = true;
				}
			}
			else if (flag8)
			{
				if (c == this.SQR && c2 == this.SQR && c3 == this.GT)
				{
					flag8 = false;
					i += 2;
				}
				else
				{
					text4 += c;
				}
			}
			else if (flag)
			{
				if (flag2)
				{
					if (c == this.SPACE)
					{
						flag2 = false;
					}
					else if (c == this.GT)
					{
						flag2 = false;
						flag = false;
					}
					if (!flag2 && text3.Length > 0)
					{
						if (text3[0] == this.SLASH)
						{
							if (text4.Length > 0)
							{
								XMLNode xmlnode3;
								Hashtable hashtable = xmlnode3 = xmlnode2;
								string key2;
								object key = key2 = "_text";
								object arg = xmlnode3[key2];
								hashtable[key] = arg + text4;
							}
							text4 = string.Empty;
							text3 = string.Empty;
							xmlnode2 = (XMLNode)xmlnodeList[xmlnodeList.Count - 1];
							xmlnodeList.RemoveAt(xmlnodeList.Count - 1);
						}
						else
						{
							if (text4.Length > 0)
							{
								XMLNode xmlnode4;
								Hashtable hashtable2 = xmlnode4 = xmlnode2;
								string key2;
								object key3 = key2 = "_text";
								object arg = xmlnode4[key2];
								hashtable2[key3] = arg + text4;
							}
							text4 = string.Empty;
							XMLNode xmlnode5 = new XMLNode();
							xmlnode5["_text"] = string.Empty;
							xmlnode5["_name"] = text3;
							if (xmlnode2[text3] == null)
							{
								xmlnode2[text3] = new XMLNodeList();
							}
							ArrayList arrayList = (ArrayList)xmlnode2[text3];
							arrayList.Add(xmlnode5);
							xmlnodeList.Add(xmlnode2);
							xmlnode2 = xmlnode5;
							text3 = string.Empty;
						}
					}
					else
					{
						text3 += c;
					}
				}
				else if (!flag5 && c == this.SLASH && c2 == this.GT)
				{
					flag = false;
					flag3 = false;
					flag4 = false;
					if (text != null)
					{
						if (text2 != null)
						{
							xmlnode2["@" + text] = text2;
						}
						else
						{
							xmlnode2["@" + text] = true;
						}
					}
					i++;
					xmlnode2 = (XMLNode)xmlnodeList[xmlnodeList.Count - 1];
					xmlnodeList.RemoveAt(xmlnodeList.Count - 1);
					text = string.Empty;
					text2 = string.Empty;
				}
				else if (!flag5 && c == this.GT)
				{
					flag = false;
					flag3 = false;
					flag4 = false;
					if (text != null)
					{
						xmlnode2["@" + text] = text2;
					}
					text = string.Empty;
					text2 = string.Empty;
				}
				else if (flag3)
				{
					if (c == this.SPACE || c == this.EQUALS)
					{
						flag3 = false;
						flag4 = true;
					}
					else
					{
						text += c;
					}
				}
				else if (flag4)
				{
					if (c == this.QUOTE)
					{
						if (flag5)
						{
							flag4 = false;
							xmlnode2["@" + text] = text2;
							text2 = string.Empty;
							text = string.Empty;
							flag5 = false;
						}
						else
						{
							flag5 = true;
						}
					}
					else if (flag5)
					{
						text2 += c;
					}
					else if (c == this.SPACE)
					{
						flag4 = false;
						xmlnode2["@" + text] = text2;
						text2 = string.Empty;
						text = string.Empty;
					}
				}
				else if (c != this.SPACE)
				{
					flag3 = true;
					text = string.Empty + c;
					text2 = string.Empty;
					flag5 = false;
				}
			}
			else if (c == this.LT)
			{
				flag = true;
				flag2 = true;
			}
			else
			{
				text4 += c;
			}
		}
		return xmlnode;
	}

	// Token: 0x04000440 RID: 1088
	private char LT = "<"[0];

	// Token: 0x04000441 RID: 1089
	private char GT = ">"[0];

	// Token: 0x04000442 RID: 1090
	private char SPACE = " "[0];

	// Token: 0x04000443 RID: 1091
	private char QUOTE = "\""[0];

	// Token: 0x04000444 RID: 1092
	private char SLASH = "/"[0];

	// Token: 0x04000445 RID: 1093
	private char QMARK = "?"[0];

	// Token: 0x04000446 RID: 1094
	private char EQUALS = "="[0];

	// Token: 0x04000447 RID: 1095
	private char EXCLAMATION = "!"[0];

	// Token: 0x04000448 RID: 1096
	private char DASH = "-"[0];

	// Token: 0x04000449 RID: 1097
	private char SQR = "]"[0];
}
