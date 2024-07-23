using System;
using UnityEngine;

namespace MTools
{
	// Token: 0x020003B1 RID: 945
	public class MArray<T>
	{
		// Token: 0x06001B5D RID: 7005 RVA: 0x000B1D60 File Offset: 0x000AFF60
		public MArray()
		{
			this.reset();
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x000B1D70 File Offset: 0x000AFF70
		public MArray(int cap)
		{
			if (cap < 1)
			{
				cap = 1;
			}
			this.mCapacity = cap;
			this.mArray = new T[this.mCapacity];
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000B1DA8 File Offset: 0x000AFFA8
		protected override void Finalize()
		{
			try
			{
				for (int i = 0; i < this.mCapacity; i++)
				{
					this.mArray[i] = default(T);
				}
				this.mArray = null;
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000B1E10 File Offset: 0x000B0010
		public int count()
		{
			return this.mSize;
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000B1E18 File Offset: 0x000B0018
		public int capacity()
		{
			return this.mCapacity;
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000B1E20 File Offset: 0x000B0020
		public void addObject(T obj)
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
			this.mArray[this.mSize] = obj;
			this.mSize++;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000B1EB8 File Offset: 0x000B00B8
		public void clear()
		{
			this.mSize = 0;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x000B1EC4 File Offset: 0x000B00C4
		public void reset()
		{
			this.mCapacity = 2;
			this.mSize = 0;
			this.mArray = new T[this.mCapacity];
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x000B1EE8 File Offset: 0x000B00E8
		public void removeObjectAtIndex(int idx)
		{
			this.mArray[idx] = this.mArray[this.mSize - 1];
			this.mSize--;
			this.mArray[this.mSize] = default(T);
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x000B1F3C File Offset: 0x000B013C
		public void setObjectAtIndex(T obj, int index)
		{
			this.mArray[index] = obj;
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000B1F4C File Offset: 0x000B014C
		public T objectAtIndex(int index)
		{
			return this.mArray[index];
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000B1F5C File Offset: 0x000B015C
		public void swapObjects(int swap, int with)
		{
			T t = this.mArray[swap];
			this.mArray[swap] = this.mArray[with];
			this.mArray[with] = t;
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000B1F9C File Offset: 0x000B019C
		public T[] array()
		{
			return this.mArray;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000B1FA4 File Offset: 0x000B01A4
		public T[] resizedArray()
		{
			T[] array = new T[this.mSize];
			for (int i = 0; i < this.mSize; i++)
			{
				array[i] = this.mArray[i];
			}
			return array;
		}

		// Token: 0x1700039F RID: 927
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

		// Token: 0x06001B6D RID: 7021 RVA: 0x000B2008 File Offset: 0x000B0208
		public void randomize()
		{
			for (int i = this.mSize - 1; i > 0; i--)
			{
				this.swapObjects(i, UnityEngine.Random.Range(0, i));
			}
		}

		// Token: 0x04001215 RID: 4629
		private T[] mArray;

		// Token: 0x04001216 RID: 4630
		private int mCapacity;

		// Token: 0x04001217 RID: 4631
		private int mSize;
	}
}
