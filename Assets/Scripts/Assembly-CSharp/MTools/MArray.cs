using System;
using UnityEngine;

namespace MTools
{
	// Token: 0x020003B0 RID: 944
	public class MArray
	{
		// Token: 0x06001B46 RID: 6982 RVA: 0x000B1994 File Offset: 0x000AFB94
		public MArray()
		{
			this.reset();
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000B19A4 File Offset: 0x000AFBA4
		public MArray(MArray arr)
		{
			if (arr == null)
			{
				this.reset();
			}
			else
			{
				this.mCapacity = arr.mCapacity;
				this.mArray = new object[this.mCapacity];
				this.mSize = arr.mSize;
				for (int i = 0; i < this.mSize; i++)
				{
					this.mArray = arr.mArray;
				}
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000B1A14 File Offset: 0x000AFC14
		public MArray(int cap)
		{
			if (cap < 1)
			{
				cap = 1;
			}
			this.mCapacity = cap;
			this.mArray = new object[this.mCapacity];
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x000B1A4C File Offset: 0x000AFC4C
		public static void SaveArrayReport()
		{
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000B1A50 File Offset: 0x000AFC50
		public static void ClearArrayReport()
		{
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000B1A54 File Offset: 0x000AFC54
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

		// Token: 0x06001B4C RID: 6988 RVA: 0x000B1AC0 File Offset: 0x000AFCC0
		public int count()
		{
			return this.mSize;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000B1AC8 File Offset: 0x000AFCC8
		public int capacity()
		{
			return this.mCapacity;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000B1AD0 File Offset: 0x000AFCD0
		public void addObject(object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (this.mCapacity == this.mSize)
			{
				this.mCapacity <<= 1;
				object[] array = new object[this.mCapacity];
				for (int i = 0; i < this.mSize; i++)
				{
					array[i] = this.mArray[i];
				}
				this.mArray = array;
			}
			this.mArray[this.mSize] = obj;
			this.mSize++;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x000B1B58 File Offset: 0x000AFD58
		public void fastClear()
		{
			this.mSize = 0;
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x000B1B64 File Offset: 0x000AFD64
		public void clear()
		{
			for (int i = 0; i < this.mSize; i++)
			{
				this.mArray[i] = null;
			}
			this.mSize = 0;
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x000B1B98 File Offset: 0x000AFD98
		public void fullClear()
		{
			for (int i = 0; i < this.mCapacity; i++)
			{
				this.mArray[i] = null;
			}
			this.mSize = 0;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x000B1BCC File Offset: 0x000AFDCC
		public void reset()
		{
			this.mCapacity = 2;
			this.mSize = 0;
			this.mArray = new object[this.mCapacity];
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x000B1BF0 File Offset: 0x000AFDF0
		public void removeObjectAtIndex(int idx)
		{
			this.mArray[idx] = this.mArray[this.mSize - 1];
			this.mSize--;
			this.mArray[this.mSize] = null;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x000B1C28 File Offset: 0x000AFE28
		public void removeObject(object obj)
		{
			int num = this.indexOfObject(obj);
			if (num != -1)
			{
				this.removeObjectAtIndex(num);
			}
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x000B1C4C File Offset: 0x000AFE4C
		public int indexOfObject(object obj)
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

		// Token: 0x06001B56 RID: 6998 RVA: 0x000B1C88 File Offset: 0x000AFE88
		public int indexOfEquivelentObject(object obj)
		{
			int result = -1;
			for (int i = 0; i < this.mSize; i++)
			{
				if (obj.Equals(this.mArray[i]))
				{
					result = i;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000B1CCC File Offset: 0x000AFECC
		public object[] array()
		{
			return this.mArray;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000B1CD4 File Offset: 0x000AFED4
		public void setObjectAtIndex(object obj, int index)
		{
			this.mArray[index] = obj;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000B1CE0 File Offset: 0x000AFEE0
		public object objectAtIndex(int index)
		{
			return this.mArray[index];
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000B1CEC File Offset: 0x000AFEEC
		public bool containsObject(object obj)
		{
			return this.indexOfObject(obj) != -1;
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x000B1CFC File Offset: 0x000AFEFC
		public void swapObjects(int swap, int with)
		{
			object obj = this.mArray[swap];
			this.mArray[swap] = this.mArray[with];
			this.mArray[with] = obj;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x000B1D2C File Offset: 0x000AFF2C
		public void randomize()
		{
			for (int i = this.mSize - 1; i > 0; i--)
			{
				this.swapObjects(i, UnityEngine.Random.Range(0, i));
			}
		}

		// Token: 0x04001211 RID: 4625
		public const int MArray_Allocated = 0;

		// Token: 0x04001212 RID: 4626
		private object[] mArray;

		// Token: 0x04001213 RID: 4627
		private int mCapacity;

		// Token: 0x04001214 RID: 4628
		private int mSize;
	}
}
