using System;

// Token: 0x020003DA RID: 986
public class SoaringValue : SoaringObjectBase
{
	// Token: 0x06001DA0 RID: 7584 RVA: 0x000BA024 File Offset: 0x000B8224
	public SoaringValue(int val) : base(SoaringObjectBase.IsType.Int)
	{
		this.IntVal = (long)val;
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x000BA038 File Offset: 0x000B8238
	public SoaringValue(long val) : base(SoaringObjectBase.IsType.Int)
	{
		this.IntVal = val;
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x000BA048 File Offset: 0x000B8248
	public SoaringValue(float val) : base(SoaringObjectBase.IsType.Float)
	{
		this.FloatVal = (double)val;
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x000BA05C File Offset: 0x000B825C
	public SoaringValue(double val) : base(SoaringObjectBase.IsType.Float)
	{
		this.FloatVal = val;
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000BA06C File Offset: 0x000B826C
	public SoaringValue(string val) : base(SoaringObjectBase.IsType.String)
	{
		this.StringVal = val;
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x000BA07C File Offset: 0x000B827C
	public SoaringValue(bool val) : base(SoaringObjectBase.IsType.Boolean)
	{
		this.IntVal = ((!val) ? 0L : 1L);
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x000BA09C File Offset: 0x000B829C
	public override string ToString()
	{
		string result;
		if (this.mType == SoaringObjectBase.IsType.Int)
		{
			result = this.IntVal.ToString();
		}
		else if (this.mType == SoaringObjectBase.IsType.Float)
		{
			result = this.FloatVal.ToString();
		}
		else if (this.mType == SoaringObjectBase.IsType.String)
		{
			result = this.StringVal;
		}
		else if (this.mType == SoaringObjectBase.IsType.Boolean)
		{
			result = ((this.IntVal == 0L) ? "false" : "true");
		}
		else
		{
			result = string.Empty;
		}
		return result;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x000BA130 File Offset: 0x000B8330
	public override string ToJsonString()
	{
		string result;
		if (this.mType == SoaringObjectBase.IsType.Int)
		{
			result = this.IntVal.ToString();
		}
		else if (this.mType == SoaringObjectBase.IsType.Float)
		{
			result = this.FloatVal.ToString();
		}
		else if (this.mType == SoaringObjectBase.IsType.String)
		{
			result = "\"" + this.ProtectString(this.StringVal) + "\"";
		}
		else if (this.mType == SoaringObjectBase.IsType.Boolean)
		{
			result = ((this.IntVal == 0L) ? "false" : "true");
		}
		else
		{
			result = "\"\"";
		}
		return result;
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x000BA1D8 File Offset: 0x000B83D8
	public string ProtectString(string initial)
	{
		if (string.IsNullOrEmpty(initial))
		{
			return initial;
		}
		return initial.Replace("\"", "\\\"");
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x000BA1F8 File Offset: 0x000B83F8
	public static implicit operator SoaringValue(int b)
	{
		return new SoaringValue(b);
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000BA200 File Offset: 0x000B8400
	public static implicit operator SoaringValue(long b)
	{
		return new SoaringValue(b);
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x000BA208 File Offset: 0x000B8408
	public static implicit operator SoaringValue(float b)
	{
		return new SoaringValue(b);
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x000BA210 File Offset: 0x000B8410
	public static implicit operator SoaringValue(double b)
	{
		return new SoaringValue(b);
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x000BA218 File Offset: 0x000B8418
	public static implicit operator SoaringValue(string b)
	{
		return new SoaringValue(b);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x000BA220 File Offset: 0x000B8420
	public static implicit operator SoaringValue(bool b)
	{
		return new SoaringValue(b);
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x000BA228 File Offset: 0x000B8428
	public static implicit operator int(SoaringValue b)
	{
		int result = 0;
		if (b == null)
		{
			return result;
		}
		if (b.mType == SoaringObjectBase.IsType.Int)
		{
			result = (int)b.IntVal;
		}
		return result;
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x000BA254 File Offset: 0x000B8454
	public static implicit operator long(SoaringValue b)
	{
		long result = 0L;
		if (b == null)
		{
			return result;
		}
		if (b.mType == SoaringObjectBase.IsType.Int)
		{
			result = b.IntVal;
		}
		return result;
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x000BA280 File Offset: 0x000B8480
	public static implicit operator float(SoaringValue b)
	{
		float result = float.NaN;
		if (b == null)
		{
			return result;
		}
		if (b.mType == SoaringObjectBase.IsType.Float)
		{
			result = (float)b.FloatVal;
		}
		return result;
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x000BA2B0 File Offset: 0x000B84B0
	public static implicit operator double(SoaringValue b)
	{
		double result = double.NaN;
		if (b == null)
		{
			return result;
		}
		if (b.mType == SoaringObjectBase.IsType.Float)
		{
			result = b.FloatVal;
		}
		return result;
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x000BA2E4 File Offset: 0x000B84E4
	public static implicit operator bool(SoaringValue b)
	{
		bool result = false;
		if (b == null)
		{
			return result;
		}
		if (b.mType == SoaringObjectBase.IsType.Boolean)
		{
			result = (b.IntVal != 0L);
		}
		return result;
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x000BA31C File Offset: 0x000B851C
	public static implicit operator string(SoaringValue b)
	{
		if (b == null)
		{
			return string.Empty;
		}
		return b.ToString();
	}

	// Token: 0x040012C4 RID: 4804
	protected long IntVal;

	// Token: 0x040012C5 RID: 4805
	protected double FloatVal;

	// Token: 0x040012C6 RID: 4806
	protected string StringVal;
}
