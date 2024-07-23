using System;
using System.IO;
using System.Runtime.CompilerServices;
using MTools;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class DatabaseManager
{
	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000233 RID: 563 RVA: 0x0000AE00 File Offset: 0x00009000
	public bool HasData
	{
		get
		{
			return this.dictionarySheets != null && this.dictionarySheets.count() > 0;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000234 RID: 564 RVA: 0x0000AE20 File Offset: 0x00009020
	public static DatabaseManager Instance
	{
		get
		{
			if (DatabaseManager.sInstance == null)
			{
				DatabaseManager.sInstance = new DatabaseManager();
				DatabaseManager.sInstance.Initialize();
			}
			return DatabaseManager.sInstance;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000235 RID: 565 RVA: 0x0000AE48 File Offset: 0x00009048
	public int SheetCount
	{
		get
		{
			if (!this.bInitialized)
			{
				return 0;
			}
			if (this.sheetTypeInfo == null)
			{
				return 0;
			}
			return this.sheetTypeInfo.Length;
		}
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000AE78 File Offset: 0x00009078
	private void Initialize()
	{
		if (this.bInitialized)
		{
			return;
		}
		this.bInitialized = true;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000AE90 File Offset: 0x00009090
	public int GetNumRows(string sheetName)
	{
		int sheetIndex = this.GetSheetIndex(sheetName);
		return this.sheetTypeInfo[sheetIndex].numRow;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000AEB4 File Offset: 0x000090B4
	public bool HasRow(string sheetName, string rowName)
	{
		MDictionary mdictionary = this.dictionarySheets.objectWithKey(sheetName) as MDictionary;
		return mdictionary != null && mdictionary.containsKey(rowName);
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000AEE4 File Offset: 0x000090E4
	public bool HasRow(int sheetIDX, string rowName)
	{
		MDictionary mdictionary = this.dictionarySheets.objectAtIndex(sheetIDX) as MDictionary;
		return mdictionary != null && mdictionary.containsKey(rowName);
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000AF14 File Offset: 0x00009114
	public MArray GetEntireRow(string sheetName, string rowName)
	{
		MDictionary mdictionary = this.dictionarySheets.objectWithKey(sheetName) as MDictionary;
		if (mdictionary == null)
		{
			Debug.Log("DatabaseManager: failed to fetch data sheet named " + sheetName);
			return null;
		}
		return mdictionary.objectWithKey(rowName) as MArray;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000AF58 File Offset: 0x00009158
	public string[] GetHeaderRow(string sheetName)
	{
		int sheetIndex = this.GetSheetIndex(sheetName);
		DatabaseManager.SheetInfo sheetInfo = this.sheetTypeInfo[sheetIndex];
		string[] array = new string[sheetInfo.numCol];
		for (int i = 0; i < sheetInfo.numCol; i++)
		{
			array[i] = sheetInfo.typeInfo[i].colName;
		}
		return array;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000AFB0 File Offset: 0x000091B0
	public int GetSheetIndex(string sheetName)
	{
		if (this.dictionarySheets == null)
		{
			return -1;
		}
		return this.dictionarySheets.indexOfObjectWithKey(sheetName);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000AFCC File Offset: 0x000091CC
	public int GetRowIndex(int sheetID, string rowID)
	{
		if (sheetID < 0 || rowID == null)
		{
			return -1;
		}
		MDictionary mdictionary = this.dictionarySheets.objectAtIndex(sheetID) as MDictionary;
		return mdictionary.indexOfObjectWithKey(rowID);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000B004 File Offset: 0x00009204
	public MArray GetSheetKeys(string sheetName)
	{
		MDictionary mdictionary = this.dictionarySheets.objectWithKey(sheetName) as MDictionary;
		if (mdictionary == null)
		{
			return new MArray();
		}
		return mdictionary.allKeys();
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000B038 File Offset: 0x00009238
	public int GetColumnIndexInSheet(int sheetIdx, string columnName)
	{
		string value = columnName.ToLower();
		DatabaseManager.SheetInfo sheetInfo = this.sheetTypeInfo[sheetIdx];
		for (int i = 0; i < sheetInfo.numCol; i++)
		{
			if (sheetInfo.typeInfo[i].colName.Equals(value))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000240 RID: 576 RVA: 0x0000B08C File Offset: 0x0000928C
	public int GetColumnIndexInSheet(string sheetName, string columnName)
	{
		int sheetIndex = this.GetSheetIndex(sheetName);
		return this.GetColumnIndexInSheet(sheetIndex, columnName);
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000B0AC File Offset: 0x000092AC
	public int GetIntCell(string sheetName, string rowName, string columnName)
	{
		MObject cell = this.GetCell(sheetName, rowName, columnName, false);
		return cell.valueAsInt();
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000B0CC File Offset: 0x000092CC
	public int GetIntCell(string sheetName, string rowName, int columnName)
	{
		MObject cell = this.GetCell(sheetName, rowName, columnName, false);
		return cell.valueAsInt();
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000B0EC File Offset: 0x000092EC
	public string GetStringCell(string sheetName, string rowName, string columnName)
	{
		MObject cell = this.GetCell(sheetName, rowName, columnName, true);
		if (cell == null)
		{
			return null;
		}
		return cell.valueAsString();
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000B114 File Offset: 0x00009314
	public float GetFloatCell(string sheetName, string rowName, string columnName)
	{
		MObject cell = this.GetCell(sheetName, rowName, columnName, false);
		return cell.valueAsFloat();
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000B134 File Offset: 0x00009334
	public int GetIntCell(int sheetID, int rowID, string columnName)
	{
		MObject cell = this.GetCell(sheetID, rowID, columnName, false);
		return cell.valueAsInt();
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000B154 File Offset: 0x00009354
	public string GetStringCell(int sheetID, int rowID, string columnName)
	{
		MObject cell = this.GetCell(sheetID, rowID, columnName, true);
		if (cell == null)
		{
			return null;
		}
		return cell.valueAsString();
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000B17C File Offset: 0x0000937C
	public float GetFloatCell(int sheetID, int rowID, string columnName)
	{
		MObject cell = this.GetCell(sheetID, rowID, columnName, false);
		return cell.valueAsFloat();
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000B19C File Offset: 0x0000939C
	public int GetIntCell(int sheetID, int rowID, int columnID)
	{
		MObject cell = this.GetCell(sheetID, rowID, columnID, false);
		return cell.valueAsInt();
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000B1BC File Offset: 0x000093BC
	public string GetStringCell(int sheetID, int rowID, int columnID)
	{
		MObject cell = this.GetCell(sheetID, rowID, columnID, true);
		if (cell == null)
		{
			return null;
		}
		return cell.valueAsString();
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000B1E4 File Offset: 0x000093E4
	public float GetFloatCell(int sheetID, int rowID, int columnID)
	{
		MObject cell = this.GetCell(sheetID, rowID, columnID, false);
		return cell.valueAsFloat();
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000B204 File Offset: 0x00009404
	private MObject GetCell(string sheetName, string rowName, string columnName, bool failOk = false)
	{
		MArray entireRow = this.GetEntireRow(sheetName, rowName);
		if (entireRow == null)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find row: " + rowName + " in sheet: " + sheetName);
			}
			return null;
		}
		int columnIndexInSheet = this.GetColumnIndexInSheet(sheetName, columnName);
		if (columnIndexInSheet < 0)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find column: " + columnName + " in sheet: " + sheetName);
			}
			return null;
		}
		return entireRow.objectAtIndex(columnIndexInSheet) as MObject;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000B27C File Offset: 0x0000947C
	private MObject GetCell(string sheetName, string rowName, int columnIndex, bool failOk = false)
	{
		MArray entireRow = this.GetEntireRow(sheetName, rowName);
		if (entireRow == null)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find row: " + rowName + " in sheet: " + sheetName);
			}
			return null;
		}
		if (columnIndex < 0)
		{
			if (!failOk)
			{
				Debug.LogError("DatabaseManager: failed to find column: " + columnIndex);
			}
			return null;
		}
		return entireRow.objectAtIndex(columnIndex) as MObject;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x0000B2E8 File Offset: 0x000094E8
	private MObject GetCell(int sheetID, int rowID, string columnName, bool failOk = false)
	{
		if (sheetID < 0 || rowID < 0)
		{
			if (!failOk)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"DatabaseManager: failed to find row: ",
					columnName,
					" in sheet: ",
					sheetID
				}));
			}
			return null;
		}
		MDictionary mdictionary = this.dictionarySheets.objectAtIndex(sheetID) as MDictionary;
		MArray marray = mdictionary.objectAtIndex(rowID) as MArray;
		int columnIndexInSheet = this.GetColumnIndexInSheet(sheetID, columnName);
		if (columnIndexInSheet < 0)
		{
			if (!failOk)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"DatabaseManager: failed to find column: ",
					columnName,
					" in sheet: ",
					sheetID
				}));
			}
			return null;
		}
		return marray.objectAtIndex(columnIndexInSheet) as MObject;
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000B3AC File Offset: 0x000095AC
	private MObject GetCell(int sheetID, int rowID, int columnID, bool failOk = false)
	{
		if (sheetID < 0 || rowID < 0 || columnID < 0)
		{
			if (!failOk)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"DatabaseManager: failed to find row: ",
					columnID,
					" in sheet: ",
					sheetID
				}));
			}
			return null;
		}
		MDictionary mdictionary = this.dictionarySheets.objectAtIndex(sheetID) as MDictionary;
		MArray marray = mdictionary.objectAtIndex(rowID) as MArray;
		return marray.objectAtIndex(columnID) as MObject;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0000B434 File Offset: 0x00009634
	public void SaveBinaryInstruction(string fileName)
	{
		string filePath = ResourceUtils.GetFilePath(fileName, null, false);
		if (filePath == null)
		{
			Debug.Log("DatabaseManager: Failed to get path to " + fileName);
		}
		MBinaryWriter mbinaryWriter = new MBinaryWriter();
		mbinaryWriter.Open(filePath, true, true);
		int length = this.sheetTypeInfo.GetLength(0);
		mbinaryWriter.Write(1);
		mbinaryWriter.Write(length);
		for (int i = 0; i < length; i++)
		{
			DatabaseManager.SheetInfo sheetInfo = this.sheetTypeInfo[i];
			mbinaryWriter.Write(sheetInfo.indexInDatabase);
			mbinaryWriter.Write(sheetInfo.keyBytesOffset);
			mbinaryWriter.Write(sheetInfo.cellBytesOffset);
			mbinaryWriter.Write(sheetInfo.numRow);
			mbinaryWriter.Write(sheetInfo.numCol);
			mbinaryWriter.Write(sheetInfo.fileName);
			for (int j = 0; j < sheetInfo.numCol; j++)
			{
				mbinaryWriter.Write(sheetInfo.typeInfo[j].colName);
				mbinaryWriter.Write((int)sheetInfo.typeInfo[j].id);
			}
		}
		mbinaryWriter.Close();
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000B544 File Offset: 0x00009744
	public void LoadBinaryInstruction(string fileName)
	{
		MBinaryReader fileStream = ResourceUtils.GetFileStream(fileName);
		if (fileStream == null)
		{
			Debug.Log("DatabaseManager.LoadBinaryInstructions: Failed to get path to " + fileName);
		}
		int num = fileStream.ReadInt();
		if (1 > num)
		{
			return;
		}
		int num2 = fileStream.ReadInt();
		this.sheetTypeInfo = new DatabaseManager.SheetInfo[num2];
		for (int i = 0; i < num2; i++)
		{
			DatabaseManager.SheetInfo sheetInfo = new DatabaseManager.SheetInfo();
			sheetInfo.indexInDatabase = fileStream.ReadInt();
			sheetInfo.keyBytesOffset = fileStream.ReadInt();
			sheetInfo.cellBytesOffset = fileStream.ReadInt();
			sheetInfo.numRow = fileStream.ReadInt();
			sheetInfo.numCol = fileStream.ReadInt();
			sheetInfo.fileName = fileStream.ReadString();
			sheetInfo.typeInfo = new CSVTypeInfo[sheetInfo.numCol];
			for (int j = 0; j < sheetInfo.numCol; j++)
			{
				sheetInfo.typeInfo[j].colName = fileStream.ReadString();
				sheetInfo.typeInfo[j].id = (TypeID)fileStream.ReadInt();
			}
			this.sheetTypeInfo[i] = sheetInfo;
		}
		fileStream.Close();
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000B664 File Offset: 0x00009864
	public void SaveBinaryData(string fileName)
	{
		string writePath = ResourceUtils.GetWritePath(fileName, null, 1);
		if (writePath == null)
		{
			Debug.Log("DatabaseManager: Failed to get path to " + fileName);
		}
		MBinaryWriter mbinaryWriter = new MBinaryWriter(writePath);
		MArray marray = this.dictionarySheets.allKeys();
		MArray marray2 = this.dictionarySheets.allValues();
		for (int i = 0; i < marray.count(); i++)
		{
			mbinaryWriter.Write(i);
			string val = marray.objectAtIndex(i) as string;
			mbinaryWriter.Write(val);
			MDictionary dictionary = marray2.objectAtIndex(i) as MDictionary;
			this.WriteDictionaryToBinaryData(mbinaryWriter, dictionary, i);
		}
		mbinaryWriter.Close();
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000B70C File Offset: 0x0000990C
	public bool LoadDatabaseFromInstruction(string instructionFileName, string dbDataFileName)
	{
		this.LoadBinaryInstruction(instructionFileName);
		MBinaryReader fileStream = ResourceUtils.GetFileStream(dbDataFileName);
		if (fileStream == null)
		{
			Debug.Log("DatabaseManager.LoadDatabase: Failed to get path to " + dbDataFileName);
		}
		this.dictionarySheets = new MDictionary(this.sheetTypeInfo.Length);
		for (int i = 0; i < this.sheetTypeInfo.Length; i++)
		{
			fileStream.ReadInt();
			string key = fileStream.ReadString();
			MDictionary val = this.ReadSheetDictionaryFromBinaryData(fileStream, i);
			this.dictionarySheets.addValue(val, key);
		}
		return this.sheetTypeInfo.Length > 0;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000B798 File Offset: 0x00009998
	private MArray ReadAllKeysFromBinaryData(MBinaryReader reader, int numKeys)
	{
		MArray marray = new MArray(numKeys);
		for (int i = 0; i < numKeys; i++)
		{
			string obj = reader.ReadString();
			marray.addObject(obj);
		}
		return marray;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000B7D0 File Offset: 0x000099D0
	private MArray ReadAllValuesFromBinaryData(MBinaryReader reader, int numColumns, int sheetID)
	{
		MArray marray = new MArray(numColumns);
		for (int i = 0; i < numColumns; i++)
		{
			MObject mobject = new MObject();
			switch (this.sheetTypeInfo[sheetID].typeInfo[i].id)
			{
			case TypeID.TYPE_STRING:
				mobject.setValueAsString(reader.ReadString());
				break;
			case TypeID.TYPE_INT:
				mobject.setValueAsInt(reader.ReadInt());
				break;
			case TypeID.TYPE_FLOAT:
				mobject.setValueAsFloat(reader.ReadFloat());
				break;
			}
			marray.addObject(mobject);
		}
		return marray;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000B86C File Offset: 0x00009A6C
	private MDictionary ReadSheetDictionaryFromBinaryData(MBinaryReader reader, int sheetID)
	{
		int numRow = this.sheetTypeInfo[sheetID].numRow;
		MArray keys = this.ReadAllKeysFromBinaryData(reader, numRow);
		MArray marray = new MArray(numRow);
		for (int i = 0; i < numRow; i++)
		{
			MArray obj = this.ReadAllValuesFromBinaryData(reader, this.sheetTypeInfo[sheetID].numCol, sheetID);
			marray.addObject(obj);
		}
		return new MDictionary(keys, marray);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000B8D4 File Offset: 0x00009AD4
	private void WriteAllKeysToBinaryData(MBinaryWriter writer, MArray keys)
	{
		for (int i = 0; i < keys.count(); i++)
		{
			string val = keys.objectAtIndex(i) as string;
			writer.Write(val);
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000B90C File Offset: 0x00009B0C
	private void WriterAllValuesToBinaryData(MBinaryWriter writer, MArray rowData, int sheetID)
	{
		for (int i = 0; i < rowData.count(); i++)
		{
			MObject mobject = rowData.objectAtIndex(i) as MObject;
			switch (this.sheetTypeInfo[sheetID].typeInfo[i].id)
			{
			case TypeID.TYPE_STRING:
				writer.Write(mobject.valueAsString());
				break;
			case TypeID.TYPE_INT:
				writer.Write(mobject.valueAsInt());
				break;
			case TypeID.TYPE_FLOAT:
				writer.Write(mobject.valueAsFloat());
				break;
			}
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000B9A8 File Offset: 0x00009BA8
	private void WriteDictionaryToBinaryData(MBinaryWriter writer, MDictionary dictionary, int sheetID)
	{
		this.WriteAllKeysToBinaryData(writer, dictionary.allKeys());
		MArray marray = dictionary.allValues();
		for (int i = 0; i < marray.count(); i++)
		{
			MArray rowData = marray.objectAtIndex(i) as MArray;
			this.WriterAllValuesToBinaryData(writer, rowData, sheetID);
		}
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000B9F8 File Offset: 0x00009BF8
	public bool LoadDatabaseFromCSV(string fileName = "Database_LookUp.csv")
	{
		this.dictionarySheets = this.LoadDataSheet(fileName, -1);
		MArray marray = this.dictionarySheets.allKeys();
		MArray marray2 = this.dictionarySheets.allValues();
		this.sheetTypeInfo = new DatabaseManager.SheetInfo[marray.count()];
		for (int i = 0; i < marray2.count(); i++)
		{
			MArray marray3 = marray2.objectAtIndex(i) as MArray;
			MObject mobject = marray3.objectAtIndex(1) as MObject;
			string fileName2 = mobject.valueAsString();
			MDictionary val = this.LoadDataSheet(fileName2, i);
			string key = marray.objectAtIndex(i) as string;
			this.dictionarySheets.setValue(val, key);
		}
		return marray.count() > 0;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000BAB0 File Offset: 0x00009CB0
	public Stream LoadFileData(string fileName)
	{
		Debug.Log("Database Loading: " + fileName);
		Stream rawFileStream = ResourceUtils.GetRawFileStream("Contents/Spreadsheets/" + fileName, null, null, 1);
		if (rawFileStream == null)
		{
			rawFileStream = ResourceUtils.GetRawFileStream("Spreadsheets/" + fileName, null, null, 6);
		}
		return rawFileStream;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000BB00 File Offset: 0x00009D00
	private unsafe MDictionary LoadDataSheet(string fileName, int sheetNum)
	{
		Stream stream = this.LoadFileData(fileName);
		if (stream == null)
		{
			Debug.LogError("DatabaseManager: Failed to get path to " + fileName);
			return new MDictionary(0);
		}
		CVSReader cvsreader = new CVSReader();
		if (!cvsreader.Open(stream))
		{
			Debug.Log("DatabaseManager: file can't open.");
		}
		cvsreader.ReadLine();
		MDictionary mdictionary = new MDictionary(cvsreader.GetRowCount());
		string key = null;
		for (MArray val = cvsreader.ParseLine(ref key); val != null; val = cvsreader.ParseLine(ref key))
		{
			mdictionary.addValue(val, key);
			key = null;
		}
		if (sheetNum != -1)
		{
			DatabaseManager.SheetInfo sheetInfo = new DatabaseManager.SheetInfo();
			sheetInfo.indexInDatabase = sheetNum;
			sheetInfo.keyBytesOffset = cvsreader.GetKeyBytesOffset();
			sheetInfo.cellBytesOffset = cvsreader.GetCellBytesOffset();
			sheetInfo.numRow = mdictionary.count();
			sheetInfo.numCol = cvsreader.GetColCount();
			sheetInfo.fileName = fileName;
			sheetInfo.typeInfo = cvsreader.GetTypeInfoTable();
			int numCol = sheetInfo.numCol;
			int i = 0;
			while (i < numCol)
			{
				fixed (string text = sheetInfo.typeInfo[i].colName)
				{
					fixed (char* ptr = text + RuntimeHelpers.OffsetToStringData / 2)
					{
						char* ptr2 = ptr;
						for (char c = *ptr2; c != '\0'; c = *ptr2)
						{
							if (c > '@' && c < '[')
							{
								*ptr2 = c + ' ';
							}
							ptr2++;
						}
						text = null;
						i++;
					}
				}
			}
			this.sheetTypeInfo[sheetNum] = sheetInfo;
		}
		return mdictionary;
	}

	// Token: 0x0400012D RID: 301
	private const int cVersion = 1;

	// Token: 0x0400012E RID: 302
	private bool bInitialized;

	// Token: 0x0400012F RID: 303
	private DatabaseManager.SheetInfo[] sheetTypeInfo;

	// Token: 0x04000130 RID: 304
	private MDictionary dictionarySheets;

	// Token: 0x04000131 RID: 305
	private static DatabaseManager sInstance;

	// Token: 0x02000036 RID: 54
	private class SheetInfo : UnityEngine.Object
	{
		// Token: 0x04000132 RID: 306
		public int indexInDatabase;

		// Token: 0x04000133 RID: 307
		public int keyBytesOffset;

		// Token: 0x04000134 RID: 308
		public int cellBytesOffset;

		// Token: 0x04000135 RID: 309
		public int numCol;

		// Token: 0x04000136 RID: 310
		public int numRow;

		// Token: 0x04000137 RID: 311
		public string fileName;

		// Token: 0x04000138 RID: 312
		public CSVTypeInfo[] typeInfo;
	}
}
