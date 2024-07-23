using System;

// Token: 0x0200043B RID: 1083
public class ConstantGenerator : ResultGenerator
{
	// Token: 0x06002163 RID: 8547 RVA: 0x000CE624 File Offset: 0x000CC824
	public ConstantGenerator(string val)
	{
		this.val = val;
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x000CE634 File Offset: 0x000CC834
	public string GetResult()
	{
		return this.val;
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x000CE63C File Offset: 0x000CC83C
	public string GetExpectedValue()
	{
		return this.val;
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x000CE644 File Offset: 0x000CC844
	public string GetLowestValue()
	{
		return this.val;
	}

	// Token: 0x040014AB RID: 5291
	private string val;
}
