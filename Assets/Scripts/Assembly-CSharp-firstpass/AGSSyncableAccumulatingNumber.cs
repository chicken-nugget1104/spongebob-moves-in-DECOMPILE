using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class AGSSyncableAccumulatingNumber : AGSSyncable
{
	// Token: 0x06000298 RID: 664 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
	public AGSSyncableAccumulatingNumber(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x06000299 RID: 665 RVA: 0x0000C6D0 File Offset: 0x0000A8D0
	public AGSSyncableAccumulatingNumber(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x0600029A RID: 666 RVA: 0x0000C6DC File Offset: 0x0000A8DC
	public void Increment(long delta)
	{
		this.javaObject.Call("increment", new object[]
		{
			delta
		});
	}

	// Token: 0x0600029B RID: 667 RVA: 0x0000C700 File Offset: 0x0000A900
	public void Increment(double delta)
	{
		this.javaObject.Call("increment", new object[]
		{
			delta
		});
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0000C724 File Offset: 0x0000A924
	public void Increment(int delta)
	{
		this.javaObject.Call("increment", new object[]
		{
			delta
		});
	}

	// Token: 0x0600029D RID: 669 RVA: 0x0000C748 File Offset: 0x0000A948
	public void Increment(string delta)
	{
		this.javaObject.Call("increment", new object[]
		{
			delta
		});
	}

	// Token: 0x0600029E RID: 670 RVA: 0x0000C764 File Offset: 0x0000A964
	public void Decrement(long delta)
	{
		this.javaObject.Call("decrement", new object[]
		{
			delta
		});
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0000C788 File Offset: 0x0000A988
	public void Decrement(double delta)
	{
		this.javaObject.Call("decrement", new object[]
		{
			delta
		});
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000C7AC File Offset: 0x0000A9AC
	public void Decrement(int delta)
	{
		this.javaObject.Call("decrement", new object[]
		{
			delta
		});
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
	public void Decrement(string delta)
	{
		this.javaObject.Call("decrement", new object[]
		{
			delta
		});
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x0000C7EC File Offset: 0x0000A9EC
	public long AsLong()
	{
		return this.javaObject.Call<long>("asLong", new object[0]);
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x0000C804 File Offset: 0x0000AA04
	public double AsDouble()
	{
		return this.javaObject.Call<double>("asDouble", new object[0]);
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x0000C81C File Offset: 0x0000AA1C
	public int AsInt()
	{
		return this.javaObject.Call<int>("asInt", new object[0]);
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0000C834 File Offset: 0x0000AA34
	public string AsString()
	{
		return this.javaObject.Call<string>("asString", new object[0]);
	}
}
