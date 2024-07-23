using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class XMLNode : Hashtable
{
	// Token: 0x060006CC RID: 1740 RVA: 0x000190F0 File Offset: 0x000172F0
	public XMLNodeList GetNodeList(string path)
	{
		return (XMLNodeList)this.GetObject(path);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00019100 File Offset: 0x00017300
	public XMLNode GetNode(string path)
	{
		return (XMLNode)this.GetObject(path);
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00019110 File Offset: 0x00017310
	public string GetValue(string path)
	{
		return this.GetObject(path) as string;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00019120 File Offset: 0x00017320
	private object GetObject(string path)
	{
		string[] array = path.Split(new char[]
		{
			">"[0]
		});
		XMLNode xmlnode = this;
		XMLNodeList xmlnodeList = null;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (flag)
			{
				xmlnode = (XMLNode)xmlnodeList[int.Parse(array[i])];
				flag = false;
			}
			else
			{
				object obj = xmlnode[array[i]];
				if (!(obj is ArrayList))
				{
					if (i != array.Length - 1)
					{
						string text = string.Empty;
						for (int j = 0; j <= i; j++)
						{
							text = text + ">" + array[j];
						}
						Debug.Log("xml path search truncated. Wanted: " + path + " got: " + text);
					}
					return obj;
				}
				xmlnodeList = (XMLNodeList)obj;
				flag = true;
			}
		}
		if (flag)
		{
			return xmlnodeList;
		}
		return xmlnode;
	}
}
