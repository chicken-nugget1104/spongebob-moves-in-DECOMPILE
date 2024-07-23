using System;

namespace MTools
{
	// Token: 0x020003BD RID: 957
	public class MStringHash
	{
		// Token: 0x06001C34 RID: 7220 RVA: 0x000B47E4 File Offset: 0x000B29E4
		public MStringHash(MStringHash.MStringHashDelegate del, int cap)
		{
			this.setHashArray(del, cap);
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000B47F4 File Offset: 0x000B29F4
		public MStringHash(MStringHash.MStringHashDelegate del)
		{
			this.setHashArray(del, 2);
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000B4804 File Offset: 0x000B2A04
		public MStringHash(int cap)
		{
			this.setHashArray(null, cap);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000B4814 File Offset: 0x000B2A14
		public MStringHash()
		{
			this.setHashArray(null, 2);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000B4824 File Offset: 0x000B2A24
		private void setHashArray(MStringHash.MStringHashDelegate del, int cap)
		{
			if (cap <= 0)
			{
				cap = 2;
			}
			if (del == null)
			{
				del = new MStringHash.MStringHashDelegate(this._SoftHash);
			}
			this.mKeyHash = del;
			this.mValues = new MArray(cap);
			this.mKeys = new MArray<int>(cap);
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x000B4864 File Offset: 0x000B2A64
		public void addObjectWithKey(object obj, string key)
		{
			int num = this.mKeyHash(key);
			int num2 = this.indexOfObjectWithKey(num);
			if (num2 != -1)
			{
				this.mValues.setObjectAtIndex(obj, num2);
			}
			else
			{
				this.mValues.addObject(obj);
				this.mKeys.addObject(num);
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000B48B8 File Offset: 0x000B2AB8
		public void setObjectWithKey(object obj, string key)
		{
			int num = this.mKeyHash(key);
			int num2 = this.indexOfObjectWithKey(num);
			if (num2 != -1)
			{
				this.mValues.setObjectAtIndex(obj, num2);
			}
			else
			{
				this.mValues.addObject(obj);
				this.mKeys.addObject(num);
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000B490C File Offset: 0x000B2B0C
		public void removeObjectWithKey(string key)
		{
			int key2 = this.mKeyHash(key);
			int num = this.indexOfObjectWithKey(key2);
			if (num != -1)
			{
				this.mValues.removeObjectAtIndex(num);
				this.mKeys.removeObjectAtIndex(num);
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x000B4950 File Offset: 0x000B2B50
		public void clear()
		{
			this.mValues.clear();
			this.mKeys.clear();
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000B4968 File Offset: 0x000B2B68
		public object objectWithKey(string key)
		{
			int key2 = this.mKeyHash(key);
			int num = this.indexOfObjectWithKey(key2);
			if (num != -1)
			{
				return this.mValues.objectAtIndex(num);
			}
			return null;
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000B49A0 File Offset: 0x000B2BA0
		private int _SoftHash(string key)
		{
			int num = 0;
			int length = key.Length;
			for (int i = 0; i < length; i++)
			{
				num += (int)key[i];
			}
			return num + length;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000B49D8 File Offset: 0x000B2BD8
		public int indexOfObjectWithKey(string key)
		{
			int num = this.mKeyHash(key);
			int[] array = this.mKeys.array();
			int num2 = this.mKeys.count();
			for (int i = 0; i < num2; i++)
			{
				if (array[i] == num)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000B4A28 File Offset: 0x000B2C28
		public int indexOfObjectWithKey(int key)
		{
			int[] array = this.mKeys.array();
			int num = this.mKeys.count();
			for (int i = 0; i < num; i++)
			{
				if (array[i] == key)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x04001236 RID: 4662
		private MArray<int> mKeys;

		// Token: 0x04001237 RID: 4663
		private MArray mValues;

		// Token: 0x04001238 RID: 4664
		private MStringHash.MStringHashDelegate mKeyHash;

		// Token: 0x020004BA RID: 1210
		// (Invoke) Token: 0x0600254B RID: 9547
		public delegate int MStringHashDelegate(string key);
	}
}
