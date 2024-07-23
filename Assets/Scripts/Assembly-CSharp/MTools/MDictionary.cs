using System;

namespace MTools
{
	// Token: 0x020003B8 RID: 952
	public class MDictionary
	{
		// Token: 0x06001BE7 RID: 7143 RVA: 0x000B3BF0 File Offset: 0x000B1DF0
		public MDictionary()
		{
			this.mKeys = new MArray(2);
			this.mValues = new MArray(2);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000B3C10 File Offset: 0x000B1E10
		public MDictionary(MArray keys, MArray values)
		{
			this.mKeys = keys;
			if (this.mKeys == null)
			{
				this.mKeys = new MArray(2);
			}
			this.mValues = values;
			if (this.mValues == null)
			{
				this.mValues = new MArray(2);
			}
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x000B3C60 File Offset: 0x000B1E60
		public MDictionary(string[] keys, MArray values)
		{
			MArray marray = new MArray(keys.Length);
			int num = keys.Length;
			for (int i = 0; i < num; i++)
			{
				marray.addObject(keys[i]);
			}
			this.mKeys = marray;
			if (this.mKeys == null)
			{
				this.mKeys = new MArray(2);
			}
			this.mValues = values;
			if (this.mValues == null)
			{
				this.mValues = new MArray(2);
			}
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x000B3CD8 File Offset: 0x000B1ED8
		public MDictionary(int capacity)
		{
			this.mKeys = new MArray(capacity);
			this.mValues = new MArray(capacity);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000B3CF8 File Offset: 0x000B1EF8
		public static MDictionary Create(params string[] values)
		{
			MDictionary mdictionary = new MDictionary();
			if (values == null)
			{
				return mdictionary;
			}
			int num = values.Length;
			if ((num & 1) == 1)
			{
				return mdictionary;
			}
			num <<= 1;
			for (int i = 0; i < num; i++)
			{
				string val = values[i];
				string key = values[i];
				mdictionary.addValue_unsafe(val, key);
			}
			return mdictionary;
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000B3D4C File Offset: 0x000B1F4C
		~MDictionary()
		{
			this.mKeys.clear();
			this.mValues.clear();
			this.mKeys = null;
			this.mValues = null;
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000B3DA8 File Offset: 0x000B1FA8
		public int count()
		{
			return this.mKeys.count();
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000B3DB8 File Offset: 0x000B1FB8
		public MArray allKeys()
		{
			return this.mKeys;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000B3DC0 File Offset: 0x000B1FC0
		public MArray allValues()
		{
			return this.mValues;
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000B3DC8 File Offset: 0x000B1FC8
		public void addValue(object val, string key)
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
			this.mKeys.addObject(key);
			this.mValues.addObject(val);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000B3E0C File Offset: 0x000B200C
		public void setValue(object val, string key)
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
				this.mKeys.addObject(key);
				this.mValues.addObject(val);
			}
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000B3E60 File Offset: 0x000B2060
		public void addValue_unsafe(object val, string key)
		{
			if (val == null || key == null)
			{
				return;
			}
			this.mKeys.addObject(key);
			this.mValues.addObject(val);
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000B3E88 File Offset: 0x000B2088
		public object objectWithKey(string key)
		{
			object result = null;
			int num = this.mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string a = (string)this.mKeys.objectAtIndex(i);
				if (a == key)
				{
					result = this.mValues.objectAtIndex(i);
					break;
				}
			}
			return result;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000B3EE8 File Offset: 0x000B20E8
		public void removeObjectWithKey(string key)
		{
			int num = this.mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string a = (string)this.mKeys.objectAtIndex(i);
				if (a == key)
				{
					this.mValues.removeObjectAtIndex(i);
					this.mKeys.removeObjectAtIndex(i);
					break;
				}
			}
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000B3F50 File Offset: 0x000B2150
		public int indexOfObjectWithKey(string key)
		{
			int result = -1;
			int num = this.mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string a = (string)this.mKeys.objectAtIndex(i);
				if (a == key)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000B3FA4 File Offset: 0x000B21A4
		public object objectAtIndex(int index)
		{
			if (index < 0 || index >= this.count())
			{
				return null;
			}
			return this.mValues.objectAtIndex(index);
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000B3FC8 File Offset: 0x000B21C8
		public void clear()
		{
			this.mValues.clear();
			this.mKeys.clear();
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000B3FE0 File Offset: 0x000B21E0
		public bool containsKey(string key)
		{
			int num = this.mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string a = (string)this.mKeys.objectAtIndex(i);
				if (a == key)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400122A RID: 4650
		private MArray mKeys;

		// Token: 0x0400122B RID: 4651
		private MArray mValues;
	}
}
