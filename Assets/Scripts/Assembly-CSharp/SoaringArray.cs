using System;

// Token: 0x020003CD RID: 973
public class SoaringArray : SoaringObjectBase
{
	// Token: 0x06001D08 RID: 7432 RVA: 0x000B7C90 File Offset: 0x000B5E90
	public SoaringArray() : base(SoaringObjectBase.IsType.Array)
	{
		this.mCapacity = 2;
		this.mArray = new SoaringObjectBase[this.mCapacity];
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x000B7CB4 File Offset: 0x000B5EB4
	public SoaringArray(int cap) : base(SoaringObjectBase.IsType.Array)
	{
		if (cap < 1)
		{
			cap = 1;
		}
		this.mCapacity = cap;
		this.mArray = new SoaringObjectBase[this.mCapacity];
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x000B7CE0 File Offset: 0x000B5EE0
	protected override void Finalize()
	{
		try
		{
			for (int i = 0; i < this.mCapacity; i++)
			{
				this.mArray[i] = null;
			}
			this.mArray = null;
			this.mCapacity = 0;
			this.mSize = 0;
		}
		finally
		{
			base.Finalize();
		}
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x000B7D4C File Offset: 0x000B5F4C
	public int count()
	{
		return this.mSize;
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x000B7D54 File Offset: 0x000B5F54
	public int capacity()
	{
		return this.mCapacity;
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x000B7D5C File Offset: 0x000B5F5C
	public void addObject(SoaringValue obj)
	{
		this.addObject(obj);
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000B7D68 File Offset: 0x000B5F68
	public void addObject(SoaringObjectBase obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.mCapacity == this.mSize)
		{
			this.mCapacity <<= 1;
			SoaringObjectBase[] array = new SoaringObjectBase[this.mCapacity];
			for (int i = 0; i < this.mSize; i++)
			{
				array[i] = this.mArray[i];
			}
			this.mArray = array;
		}
		this.mArray[this.mSize] = obj;
		this.mSize++;
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x000B7DF0 File Offset: 0x000B5FF0
	public void fastClear()
	{
		this.mSize = 0;
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x000B7DFC File Offset: 0x000B5FFC
	public void clear()
	{
		for (int i = 0; i < this.mSize; i++)
		{
			this.mArray[i] = null;
		}
		this.mSize = 0;
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x000B7E30 File Offset: 0x000B6030
	public void fullClear()
	{
		for (int i = 0; i < this.mCapacity; i++)
		{
			this.mArray[i] = null;
		}
		this.mSize = 0;
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000B7E64 File Offset: 0x000B6064
	public void reset()
	{
		this.mCapacity = 2;
		this.mSize = 0;
		this.mArray = new SoaringObjectBase[this.mCapacity];
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000B7E88 File Offset: 0x000B6088
	public void removeObjectAtIndex(int idx)
	{
		this.mArray[idx] = this.mArray[this.mSize - 1];
		this.mSize--;
		this.mArray[this.mSize] = null;
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x000B7EC0 File Offset: 0x000B60C0
	public void removeObject(SoaringObjectBase obj)
	{
		int num = this.indexOfObject(obj);
		if (num != -1)
		{
			this.removeObjectAtIndex(num);
		}
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000B7EE4 File Offset: 0x000B60E4
	public int indexOfObject(SoaringObjectBase obj)
	{
		int result = -1;
		for (int i = 0; i < this.mSize; i++)
		{
			if (this.mArray[i] == obj)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x000B7F20 File Offset: 0x000B6120
	public SoaringObjectBase[] array()
	{
		return this.mArray;
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000B7F28 File Offset: 0x000B6128
	public void setObjectAtIndex(SoaringObjectBase obj, int index)
	{
		this.mArray[index] = obj;
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000B7F34 File Offset: 0x000B6134
	public SoaringObjectBase objectAtIndex(int index)
	{
		return this.mArray[index];
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x000B7F40 File Offset: 0x000B6140
	public SoaringValue soaringValue(int atIndex)
	{
		return (SoaringValue)this.mArray[atIndex];
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000B7F50 File Offset: 0x000B6150
	public bool containsObject(SoaringObjectBase obj)
	{
		return this.indexOfObject(obj) != -1;
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000B7F60 File Offset: 0x000B6160
	public void swapObjects(int swap, int with)
	{
		SoaringObjectBase soaringObjectBase = this.mArray[swap];
		this.mArray[swap] = this.mArray[with];
		this.mArray[with] = soaringObjectBase;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x000B7F90 File Offset: 0x000B6190
	public override string ToJsonString()
	{
		string str = "[\n";
		for (int i = 0; i < this.mSize; i++)
		{
			if (i != 0)
			{
				str += ",\n";
			}
			str += this.mArray[i].ToJsonString();
		}
		return str + "\n]";
	}

	// Token: 0x04001287 RID: 4743
	private SoaringObjectBase[] mArray;

	// Token: 0x04001288 RID: 4744
	private int mCapacity;

	// Token: 0x04001289 RID: 4745
	private int mSize;
}
