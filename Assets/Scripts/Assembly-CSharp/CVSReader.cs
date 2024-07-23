using System;
using System.IO;
using MTools;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class CVSReader
{
	// Token: 0x0600021E RID: 542 RVA: 0x0000A6C0 File Offset: 0x000088C0
	public CVSReader(string filePath)
	{
		this.Open(filePath);
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000A6E0 File Offset: 0x000088E0
	public CVSReader(Stream stream)
	{
		this.Open(stream);
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000A700 File Offset: 0x00008900
	public CVSReader()
	{
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000A718 File Offset: 0x00008918
	public int GetRowCount()
	{
		return this.MAX_ROWS;
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000A720 File Offset: 0x00008920
	public int GetColCount()
	{
		return this.MAX_COLUMNS;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000A728 File Offset: 0x00008928
	public int GetCellBytesOffset()
	{
		return this.CELLS_BYTES_OFFSET;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000A730 File Offset: 0x00008930
	public int GetKeyBytesOffset()
	{
		return this.KEY_BYTES_OFFSET;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000A738 File Offset: 0x00008938
	public CSVTypeInfo[] GetTypeInfoTable()
	{
		return this.typeLookUp;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000A740 File Offset: 0x00008940
	~CVSReader()
	{
		this.Close();
		this.reader = null;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000A784 File Offset: 0x00008984
	public void Close()
	{
		if (this.isFileOpen)
		{
			try
			{
				if (this.reader != null)
				{
					this.reader.Close();
				}
				this.reader = null;
				this.isFileOpen = false;
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000A7F4 File Offset: 0x000089F4
	public bool IsOpen()
	{
		return this.isFileOpen;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x0000A7FC File Offset: 0x000089FC
	public bool Open(string path)
	{
		if (this.isFileOpen)
		{
			return false;
		}
		if (!File.Exists(path))
		{
			Debug.Log("CSVReader: Couldn't open file: " + path);
			return false;
		}
		try
		{
			this.reader = new StreamReader(path);
			this.isFileOpen = true;
		}
		catch
		{
			this.isFileOpen = false;
		}
		this.ParseTypeLine();
		return this.isFileOpen;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000A880 File Offset: 0x00008A80
	public bool Open(Stream stream)
	{
		if (this.isFileOpen)
		{
			return false;
		}
		if (stream == null)
		{
			Debug.Log("CSVReader: Invalid File Stream");
			return false;
		}
		try
		{
			this.reader = new StreamReader(stream);
			this.isFileOpen = true;
		}
		catch
		{
			this.isFileOpen = false;
		}
		this.ParseTypeLine();
		return this.isFileOpen;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000A8FC File Offset: 0x00008AFC
	public string ReadLine()
	{
		return this.reader.ReadLine();
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000A90C File Offset: 0x00008B0C
	private bool IsSkipLine(string str)
	{
		char[] array = str.ToCharArray();
		for (int i = 0; i < str.Length; i++)
		{
			if (array[i] != ',' && array[i] != '_')
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000A950 File Offset: 0x00008B50
	public MArray ParseLine(ref string key)
	{
		string text = this.ReadLine();
		while (text != null && this.IsSkipLine(text))
		{
			text = this.ReadLine();
		}
		if (text == null)
		{
			return null;
		}
		char[] array = text.ToCharArray();
		int num = 0;
		if (array[num] == ',')
		{
			num++;
			while (num < text.Length && array[num] == ',')
			{
				if (array[num] == ',')
				{
					num++;
				}
			}
			if (num > text.Length)
			{
				return null;
			}
		}
		MArray marray = new MArray(array.Length);
		MObject obj = null;
		int num2 = 0;
		string text2 = null;
		sbyte b = -1;
		for (int i = num; i <= array.Length; i++)
		{
			if (i != array.Length && array[i] == '"')
			{
				if ((int)b == -1)
				{
					b = 0;
				}
				else
				{
					b = 1;
				}
			}
			if (i == array.Length || (array[i] == ',' && (int)b != 0))
			{
				b = -1;
				text2 = new string(array, num, i - num);
				text2 = CVSReader.TrimString(text2);
				if (key == null)
				{
					key = text2;
					this.KEY_BYTES_OFFSET += 2;
					this.KEY_BYTES_OFFSET += 1 * text2.Length;
				}
				try
				{
					switch (this.typeLookUp[num2].id)
					{
					case TypeID.TYPE_STRING:
					{
						int length = text2.Length;
						if (length >= 3 && text2[0] == '"')
						{
							text2 = text2.Substring(1, length - 2);
						}
						obj = new MObject(text2);
						this.CELLS_BYTES_OFFSET += 2;
						this.CELLS_BYTES_OFFSET += 1 * length;
						break;
					}
					case TypeID.TYPE_INT:
						obj = new MObject(int.Parse(text2));
						this.CELLS_BYTES_OFFSET += 4;
						break;
					case TypeID.TYPE_FLOAT:
						obj = new MObject(float.Parse(text2));
						this.CELLS_BYTES_OFFSET += 4;
						break;
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"CSVReader failed to convert: ",
						text2,
						". string length: ",
						text2.Length
					}));
					throw ex;
				}
				marray.addObject(obj);
				num2++;
				num = i;
				num++;
			}
		}
		return marray;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000ABDC File Offset: 0x00008DDC
	private void ParseTypeLine()
	{
		string text = this.ReadLine();
		char[] array = text.ToCharArray();
		int num = 0;
		int num2 = 0;
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == ',')
				{
					num2++;
				}
			}
			this.MAX_COLUMNS = num2 + 1;
		}
		this.typeLookUp = new CSVTypeInfo[this.MAX_COLUMNS];
		int num3 = 0;
		for (int j = num; j <= array.Length; j++)
		{
			if (j == array.Length || array[j] == ',')
			{
				string data = new string(array, num, j - num);
				this.SetTypeData(data, num3);
				num3++;
				num = j;
				num++;
			}
		}
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000ACA4 File Offset: 0x00008EA4
	private void SetTypeData(string data, int colNum)
	{
		char[] value = data.ToCharArray();
		int num = 0;
		if (data[num] == 'i')
		{
			this.typeLookUp[colNum].id = TypeID.TYPE_INT;
		}
		else if (data[num] == 'f')
		{
			this.typeLookUp[colNum].id = TypeID.TYPE_FLOAT;
		}
		else if (data[num] == 's')
		{
			this.typeLookUp[colNum].id = TypeID.TYPE_STRING;
		}
		else
		{
			this.typeLookUp[colNum].id = TypeID.TYPE_UNKNOWN;
		}
		num = 2;
		int length = data.Length;
		this.typeLookUp[colNum].colName = new string(value, num, length - num);
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000AD64 File Offset: 0x00008F64
	private static string TrimString(string str)
	{
		str = str.Trim();
		while (str.Length > 0)
		{
			char c = str[0];
			if (c == '\0' || c >= ' ')
			{
				break;
			}
			str = str.Remove(0, 1);
		}
		while (str.Length > 0)
		{
			char c2 = str[str.Length - 1];
			if (c2 == '\0' || c2 >= ' ')
			{
				break;
			}
			str = str.Remove(str.Length - 1);
		}
		return str;
	}

	// Token: 0x04000126 RID: 294
	private int CELLS_BYTES_OFFSET;

	// Token: 0x04000127 RID: 295
	private int KEY_BYTES_OFFSET;

	// Token: 0x04000128 RID: 296
	private int MAX_ROWS = -1;

	// Token: 0x04000129 RID: 297
	private int MAX_COLUMNS = -1;

	// Token: 0x0400012A RID: 298
	private CSVTypeInfo[] typeLookUp;

	// Token: 0x0400012B RID: 299
	private bool isFileOpen;

	// Token: 0x0400012C RID: 300
	private StreamReader reader;
}
