using System;

// Token: 0x020003D0 RID: 976
public class SoaringDictionary : SoaringObjectBase
{
	// Token: 0x06001D3D RID: 7485 RVA: 0x000B8494 File Offset: 0x000B6694
	public SoaringDictionary() : base(SoaringObjectBase.IsType.Dictionary)
	{
		this.mCapacity = 2;
		this.mKeys = new string[this.mCapacity];
		this.mValues = new SoaringArray(this.mCapacity);
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x000B84D4 File Offset: 0x000B66D4
	public SoaringDictionary(int capacity) : base(SoaringObjectBase.IsType.Dictionary)
	{
		this.mCapacity = capacity;
		this.mKeys = new string[capacity];
		this.mValues = new SoaringArray(capacity);
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x000B8508 File Offset: 0x000B6708
	public SoaringDictionary(string json_data) : base(SoaringObjectBase.IsType.Dictionary)
	{
		this.mCapacity = 2;
		this.mKeys = new string[this.mCapacity];
		this.mValues = new SoaringArray(this.mCapacity);
		this.ReadJson(json_data);
		if (this.mKeys == null || this.mValues == null)
		{
			this.mCapacity = 2;
			this.mKeys = new string[this.mCapacity];
			this.mValues = new SoaringArray(this.mCapacity);
		}
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x000B858C File Offset: 0x000B678C
	public SoaringDictionary(byte[] json_data) : base(SoaringObjectBase.IsType.Dictionary)
	{
		this.mCapacity = 2;
		this.mKeys = new string[this.mCapacity];
		this.mValues = new SoaringArray(this.mCapacity);
		this.ReadJson(json_data);
		if (this.mKeys == null || this.mValues == null)
		{
			this.mCapacity = 2;
			this.mKeys = new string[this.mCapacity];
			this.mValues = new SoaringArray(this.mCapacity);
		}
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x000B8610 File Offset: 0x000B6810
	~SoaringDictionary()
	{
		if (this.mValues != null)
		{
			this.mValues.clear();
		}
		this.mCapacity = 0;
		this.mSize = 0;
		this.mKeys = null;
		this.mValues = null;
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x000B8678 File Offset: 0x000B6878
	public int count()
	{
		return this.mSize;
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x000B8680 File Offset: 0x000B6880
	public string[] allKeys()
	{
		return this.mKeys;
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x000B8688 File Offset: 0x000B6888
	public SoaringObjectBase[] allValues()
	{
		return this.mValues.array();
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x000B8698 File Offset: 0x000B6898
	public SoaringDictionary makeCopy()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(this.mCapacity);
		for (int i = 0; i < this.mSize; i++)
		{
			soaringDictionary.addValue_unsafe(this.mValues.objectAtIndex(i), this.mKeys[i]);
		}
		return soaringDictionary;
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000B86E4 File Offset: 0x000B68E4
	public void CopyExisting(SoaringDictionary dictionary)
	{
		this.clear();
		int num = dictionary.count();
		for (int i = 0; i < num; i++)
		{
			this.addValue_unsafe(dictionary.objectAtIndex(i), dictionary.allKeys()[i]);
		}
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x000B8728 File Offset: 0x000B6928
	private void add_key(string key)
	{
		if (key == null)
		{
			return;
		}
		if (this.mSize == this.mCapacity)
		{
			this.mCapacity <<= 1;
			string[] array = new string[this.mCapacity];
			for (int i = 0; i < this.mSize; i++)
			{
				array[i] = this.mKeys[i];
			}
			this.mKeys = array;
		}
		this.mKeys[this.mSize] = key;
		this.mSize++;
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x000B87B0 File Offset: 0x000B69B0
	public void addValue(SoaringValue val, string key)
	{
		this.addValue(val, key);
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000B87BC File Offset: 0x000B69BC
	public void addValue(SoaringObjectBase val, string key)
	{
		if (val == null || key == null)
		{
			return;
		}
		int num = this.indexOfObjectWithKey(key);
		if (num != -1)
		{
			return;
		}
		this.add_key(key);
		this.mValues.addObject(val);
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x000B87FC File Offset: 0x000B69FC
	public void setValue(SoaringValue val, string key)
	{
		this.setValue(val, key);
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x000B8808 File Offset: 0x000B6A08
	public void setValue(SoaringObjectBase val, string key)
	{
		if (val == null || key == null)
		{
			return;
		}
		int num = this.indexOfObjectWithKey(key);
		if (num != -1)
		{
			this.mValues.setObjectAtIndex(val, num);
		}
		else
		{
			this.add_key(key);
			this.mValues.addObject(val);
		}
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x000B8858 File Offset: 0x000B6A58
	public void addValue_unsafe(SoaringObjectBase val, string key)
	{
		if (val == null || key == null)
		{
			return;
		}
		this.add_key(key);
		this.mValues.addObject(val);
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000B8888 File Offset: 0x000B6A88
	public SoaringObjectBase objectWithKey(string key)
	{
		SoaringObjectBase result = null;
		for (int i = 0; i < this.mSize; i++)
		{
			if (this.mKeys[i] == key)
			{
				result = this.mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000B88D4 File Offset: 0x000B6AD4
	public SoaringObjectBase objectWithKey(string key, bool ignoreCase)
	{
		SoaringObjectBase result = null;
		for (int i = 0; i < this.mSize; i++)
		{
			if (string.Compare(this.mKeys[i], key, ignoreCase) == 0)
			{
				result = this.mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000B8924 File Offset: 0x000B6B24
	public SoaringValue soaringValue(string key)
	{
		SoaringValue result = null;
		for (int i = 0; i < this.mSize; i++)
		{
			if (this.mKeys[i] == key)
			{
				result = (SoaringValue)this.mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x000B8978 File Offset: 0x000B6B78
	public SoaringObjectBase objectWithType(SoaringObjectBase.IsType type)
	{
		SoaringObjectBase result = null;
		for (int i = 0; i < this.mSize; i++)
		{
			SoaringObjectBase soaringObjectBase = this.mValues.objectAtIndex(i);
			if (soaringObjectBase.Type == type)
			{
				result = this.mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x000B89CC File Offset: 0x000B6BCC
	public void removeObjectWithKey(string key)
	{
		for (int i = 0; i < this.mSize; i++)
		{
			if (this.mKeys[i] == key)
			{
				this.mValues.removeObjectAtIndex(i);
				this.mKeys[i] = this.mKeys[this.mSize - 1];
				this.mSize--;
				this.mKeys[this.mSize] = null;
				break;
			}
		}
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x000B8A48 File Offset: 0x000B6C48
	public int indexOfObjectWithKey(string key)
	{
		int result = -1;
		for (int i = 0; i < this.mSize; i++)
		{
			if (this.mKeys[i] == key)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x000B8A8C File Offset: 0x000B6C8C
	public SoaringObjectBase objectAtIndex(int index)
	{
		if (index < 0 || index >= this.count())
		{
			return null;
		}
		return this.mValues.objectAtIndex(index);
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x000B8AB0 File Offset: 0x000B6CB0
	public void removeObjectAtIndex(int index)
	{
		if (index < this.count())
		{
			this.mValues.removeObjectAtIndex(index);
			this.mKeys[index] = this.mKeys[this.mSize - 1];
			this.mSize--;
			this.mKeys[this.mSize] = null;
		}
	}

	// Token: 0x170003D5 RID: 981
	public SoaringObjectBase this[string i]
	{
		get
		{
			return this.objectWithKey(i);
		}
		set
		{
			this.addValue(value, i);
		}
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x000B8B20 File Offset: 0x000B6D20
	public void clear()
	{
		if (this.mKeys != null)
		{
			for (int i = 0; i < this.mSize; i++)
			{
				this.mKeys[i] = null;
			}
		}
		this.mSize = 0;
		if (this.mValues != null)
		{
			this.mValues.clear();
		}
	}

	// Token: 0x06001D58 RID: 7512 RVA: 0x000B8B78 File Offset: 0x000B6D78
	public bool containsKey(string key)
	{
		for (int i = 0; i < this.mSize; i++)
		{
			if (this.mKeys[i] == key)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x000B8BB4 File Offset: 0x000B6DB4
	public override string ToJsonString()
	{
		string str = "{\n";
		SoaringObjectBase[] array = this.allValues();
		for (int i = 0; i < this.mSize; i++)
		{
			if (i != 0)
			{
				str += ",\n";
			}
			str = str + "\"" + this.mKeys[i] + "\" : ";
			str += array[i].ToJsonString();
		}
		return str + "\n}";
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x000B8C2C File Offset: 0x000B6E2C
	private void ReadJson(string json)
	{
		if (json == null)
		{
			return;
		}
		this.clear();
		SoaringJSON.jsonDecode(json, this);
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x000B8C44 File Offset: 0x000B6E44
	private void ReadJson(byte[] json)
	{
		if (json == null)
		{
			return;
		}
		this.clear();
		SoaringJSON.jsonDecode(json, this);
	}

	// Token: 0x04001291 RID: 4753
	private string[] mKeys;

	// Token: 0x04001292 RID: 4754
	private int mCapacity;

	// Token: 0x04001293 RID: 4755
	private int mSize;

	// Token: 0x04001294 RID: 4756
	private SoaringArray mValues;
}
