using System;
using System.Collections.Generic;

// Token: 0x02000439 RID: 1081
public class PriorityQueue<T> where T : IComparable<T>
{
	// Token: 0x06002152 RID: 8530 RVA: 0x000CE420 File Offset: 0x000CC620
	public PriorityQueue()
	{
		this.values = new List<T>();
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06002153 RID: 8531 RVA: 0x000CE434 File Offset: 0x000CC634
	public int Count
	{
		get
		{
			return this.values.Count;
		}
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x000CE444 File Offset: 0x000CC644
	public bool Empty()
	{
		return 0 == this.values.Count;
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000CE454 File Offset: 0x000CC654
	public void Push(T value)
	{
		this.values.Add(value);
		this.Sort();
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x000CE468 File Offset: 0x000CC668
	public T Pop()
	{
		int index = this.values.Count - 1;
		T result = this.values[index];
		this.values.RemoveAt(index);
		return result;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000CE4A0 File Offset: 0x000CC6A0
	public T Find(Predicate<T> predicate)
	{
		return this.values.Find(predicate);
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000CE4B0 File Offset: 0x000CC6B0
	public void Sort()
	{
		this.values.Sort();
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x000CE4C0 File Offset: 0x000CC6C0
	public void Clear()
	{
		this.values.Clear();
	}

	// Token: 0x040014A7 RID: 5287
	private List<T> values;
}
