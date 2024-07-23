using System;

// Token: 0x020003CE RID: 974
public class SoaringArray<T> : SoaringObjectBase where T : SoaringObjectBase
{
	// Token: 0x06001D1D RID: 7453 RVA: 0x000B7FF0 File Offset: 0x000B61F0
	public SoaringArray() : base(SoaringObjectBase.IsType.Array)
	{
		this.mCapacity = 2;
		this.mArray = new T[this.mCapacity];
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x000B8014 File Offset: 0x000B6214
	public SoaringArray(int cap) : base(SoaringObjectBase.IsType.Array)
	{
		if (cap < 1)
		{
			cap = 1;
		}
		this.mCapacity = cap;
		this.mArray = new T[this.mCapacity];
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x000B8040 File Offset: 0x000B6240
	protected override void Finalize()
	{
		try
		{
			for (int i = 0; i < this.mCapacity; i++)
			{
				this.mArray[i] = (T)((object)null);
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

	// Token: 0x170003D1 RID: 977
	public T this[int index]
	{
		get
		{
			return this.mArray[index];
		}
		set
		{
			this.mArray[index] = value;
		}
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x000B80D4 File Offset: 0x000B62D4
	public int count()
	{
		return this.mSize;
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x000B80DC File Offset: 0x000B62DC
	public int capacity()
	{
		return this.mCapacity;
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x000B80E4 File Offset: 0x000B62E4
	public void addObject(SoaringValue obj)
	{
		this.addObject(obj);
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x000B80F0 File Offset: 0x000B62F0
	public void addObject(SoaringObjectBase obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this.mCapacity == this.mSize)
		{
			this.mCapacity <<= 1;
			T[] array = new T[this.mCapacity];
			for (int i = 0; i < this.mSize; i++)
			{
				array[i] = this.mArray[i];
			}
			this.mArray = array;
		}
		this.mArray[this.mSize] = (T)((object)obj);
		this.mSize++;
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x000B8188 File Offset: 0x000B6388
	public void fastClear()
	{
		this.mSize = 0;
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x000B8194 File Offset: 0x000B6394
	public void clear()
	{
		for (int i = 0; i < this.mSize; i++)
		{
			this.mArray[i] = (T)((object)null);
		}
		this.mSize = 0;
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x000B81D4 File Offset: 0x000B63D4
	public void fullClear()
	{
		for (int i = 0; i < this.mCapacity; i++)
		{
			this.mArray[i] = (T)((object)null);
		}
		this.mSize = 0;
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x000B8214 File Offset: 0x000B6414
	public void reset()
	{
		this.mCapacity = 2;
		this.mSize = 0;
		this.mArray = new T[this.mCapacity];
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000B8238 File Offset: 0x000B6438
	public void removeObjectAtIndex(int idx)
	{
		this.mArray[idx] = this.mArray[this.mSize - 1];
		this.mSize--;
		this.mArray[this.mSize] = (T)((object)null);
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x000B828C File Offset: 0x000B648C
	public void removeObject(SoaringObjectBase obj)
	{
		int num = this.indexOfObject(obj);
		if (num != -1)
		{
			this.removeObjectAtIndex(num);
		}
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x000B82B0 File Offset: 0x000B64B0
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

	// Token: 0x06001D2D RID: 7469 RVA: 0x000B82F8 File Offset: 0x000B64F8
	public T[] array()
	{
		return this.mArray;
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x000B8300 File Offset: 0x000B6500
	public void setObjectAtIndex(T obj, int index)
	{
		this.mArray[index] = obj;
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x000B8310 File Offset: 0x000B6510
	public T objectAtIndex(int index)
	{
		return this.mArray[index];
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x000B8320 File Offset: 0x000B6520
	public bool containsObject(T obj)
	{
		return this.indexOfObject(obj) != -1;
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x000B8334 File Offset: 0x000B6534
	public void swapObjects(int swap, int with)
	{
		T t = this.mArray[swap];
		this.mArray[swap] = this.mArray[with];
		this.mArray[with] = t;
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x000B8374 File Offset: 0x000B6574
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

	// Token: 0x0400128A RID: 4746
	private T[] mArray;

	// Token: 0x0400128B RID: 4747
	private int mCapacity;

	// Token: 0x0400128C RID: 4748
	private int mSize;
}
