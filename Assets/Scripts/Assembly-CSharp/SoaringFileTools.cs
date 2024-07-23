using System;
using System.IO;

// Token: 0x020003D2 RID: 978
public class SoaringFileTools
{
	// Token: 0x06001D67 RID: 7527 RVA: 0x000B8D30 File Offset: 0x000B6F30
	public static bool WriteJsonToFile(string path, SoaringDictionary data)
	{
		bool result = false;
		if (data == null || string.IsNullOrEmpty(path))
		{
			return result;
		}
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		string directoryName = Path.GetDirectoryName(path);
		SoaringDebug.Log(directoryName);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		StreamWriter streamWriter = new StreamWriter(path);
		if (streamWriter == null)
		{
			return result;
		}
		SoaringFileTools soaringFileTools = new SoaringFileTools();
		soaringFileTools.WriteDictionary(data, streamWriter);
		result = true;
		streamWriter.Flush();
		streamWriter.Close();
		return result;
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x000B8DB4 File Offset: 0x000B6FB4
	private void WriteDictionary(SoaringDictionary data, StreamWriter writer)
	{
		if (data == null)
		{
			return;
		}
		string[] array = data.allKeys();
		SoaringObjectBase[] array2 = data.allValues();
		int num = data.count();
		this._WriteRawString("{\n", writer);
		for (int i = 0; i < num; i++)
		{
			if (i != 0)
			{
				this._WriteRawString(",\n", writer);
			}
			this._WriteRawString("\"" + array[i] + "\" : ", writer);
			switch (array2[i].Type)
			{
			case SoaringObjectBase.IsType.Array:
				this.WriteArray((SoaringArray)array2[i], writer);
				break;
			case SoaringObjectBase.IsType.Dictionary:
				this.WriteDictionary((SoaringDictionary)array2[i], writer);
				break;
			case SoaringObjectBase.IsType.Object:
				this._WriteRawString("\"Error:" + array2[i].Type + "\"", writer);
				break;
			default:
				this.WriteValue((SoaringValue)array2[i], writer);
				break;
			}
		}
		this._WriteRawString("\n}", writer);
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x000B8EBC File Offset: 0x000B70BC
	private void WriteArray(SoaringArray data, StreamWriter writer)
	{
		if (data == null)
		{
			return;
		}
		SoaringObjectBase[] array = data.array();
		int num = data.count();
		this._WriteRawString("{\n", writer);
		for (int i = 0; i < num; i++)
		{
			if (i != 0)
			{
				this._WriteRawString(",\n", writer);
			}
			switch (array[i].Type)
			{
			case SoaringObjectBase.IsType.Array:
				this.WriteArray((SoaringArray)array[i], writer);
				break;
			case SoaringObjectBase.IsType.Dictionary:
				this.WriteDictionary((SoaringDictionary)array[i], writer);
				break;
			case SoaringObjectBase.IsType.Object:
				this._WriteRawString("\"Error:" + array[i].Type + "\"", writer);
				break;
			default:
				this.WriteValue((SoaringValue)array[i], writer);
				break;
			}
		}
		this._WriteRawString("\n}", writer);
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x000B8FA0 File Offset: 0x000B71A0
	private void WriteValue(SoaringValue data, StreamWriter writer)
	{
		if (data == null)
		{
			return;
		}
		this._WriteRawString(data.ToJsonString(), writer);
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000B8FB8 File Offset: 0x000B71B8
	private void _WriteRawString(string str, StreamWriter writer)
	{
		int length = str.Length;
		for (int i = 0; i < length; i++)
		{
			writer.Write(str[i]);
		}
	}

	// Token: 0x020003D3 RID: 979
	private class SoaringFileData
	{
		// Token: 0x06001D6D RID: 7533 RVA: 0x000B8FF4 File Offset: 0x000B71F4
		public virtual Stream Stream()
		{
			return this.stream;
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000B8FFC File Offset: 0x000B71FC
		public virtual SoaringObjectBase DataChunk()
		{
			return null;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000B9000 File Offset: 0x000B7200
		public virtual bool IsDone()
		{
			return false;
		}

		// Token: 0x04001297 RID: 4759
		private Stream stream;
	}
}
