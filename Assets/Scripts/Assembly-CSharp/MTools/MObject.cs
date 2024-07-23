using System;

namespace MTools
{
	// Token: 0x020003BB RID: 955
	public class MObject
	{
		// Token: 0x06001C05 RID: 7173 RVA: 0x000B4610 File Offset: 0x000B2810
		public MObject()
		{
			this.mObjVal = null;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000B4620 File Offset: 0x000B2820
		public MObject(object val)
		{
			this.mObjVal = val;
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x000B4630 File Offset: 0x000B2830
		public MObject(float val)
		{
			this.mDoubleVal = (double)val;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x000B4640 File Offset: 0x000B2840
		public MObject(int val)
		{
			this.mLongVal = (long)val;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000B4650 File Offset: 0x000B2850
		public MObject(bool val)
		{
			this.mLongVal = ((!val) ? 0L : 1L);
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x000B466C File Offset: 0x000B286C
		~MObject()
		{
			this.mObjVal = null;
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x000B46A8 File Offset: 0x000B28A8
		public void setValueAsObject(object v)
		{
			this.mObjVal = v;
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x000B46B4 File Offset: 0x000B28B4
		public void setValueAsBool(bool v)
		{
			this.mLongVal = ((!v) ? 0L : 1L);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x000B46CC File Offset: 0x000B28CC
		public void setValueAsString(string v)
		{
			this.mObjVal = v;
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x000B46D8 File Offset: 0x000B28D8
		public void setValueAsInt(int v)
		{
			this.mLongVal = (long)v;
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x000B46E4 File Offset: 0x000B28E4
		public void setValueAsFloat(float v)
		{
			this.mDoubleVal = (double)v;
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x000B46F0 File Offset: 0x000B28F0
		public void setValueAsLong(long v)
		{
			this.mLongVal = v;
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000B46FC File Offset: 0x000B28FC
		public void setValueAsULong(ulong v)
		{
			this.mLongVal = (long)v;
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000B4708 File Offset: 0x000B2908
		public void setValueAsDouble(double v)
		{
			this.mDoubleVal = v;
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x000B4714 File Offset: 0x000B2914
		public object valueAsObject()
		{
			return this.mObjVal;
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x000B471C File Offset: 0x000B291C
		public bool valueAsBool()
		{
			return this.mLongVal != 0L;
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x000B472C File Offset: 0x000B292C
		public string valueAsString()
		{
			return (string)this.mObjVal;
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x000B473C File Offset: 0x000B293C
		public int valueAsInt()
		{
			return (int)this.mLongVal;
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000B4748 File Offset: 0x000B2948
		public float valueAsFloat()
		{
			return (float)this.mDoubleVal;
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x000B4754 File Offset: 0x000B2954
		public ulong valueAsULong()
		{
			return (ulong)this.mLongVal;
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000B475C File Offset: 0x000B295C
		public long valueAsLong()
		{
			return this.mLongVal;
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x000B4764 File Offset: 0x000B2964
		public double valueAsDouble()
		{
			return this.mDoubleVal;
		}

		// Token: 0x04001233 RID: 4659
		private object mObjVal;

		// Token: 0x04001234 RID: 4660
		private long mLongVal;

		// Token: 0x04001235 RID: 4661
		private double mDoubleVal;
	}
}
