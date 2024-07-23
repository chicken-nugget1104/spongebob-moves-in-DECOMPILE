using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200047D RID: 1149
public class OrderedSet<T> : IEnumerable, ICollection<T>, IEnumerable<T>
{
	// Token: 0x060023F6 RID: 9206 RVA: 0x000DD60C File Offset: 0x000DB80C
	public OrderedSet() : this(EqualityComparer<T>.Default)
	{
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000DD61C File Offset: 0x000DB81C
	public OrderedSet(IEqualityComparer<T> comparer)
	{
		this.dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
		this.linkedList = new LinkedList<T>();
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000DD63C File Offset: 0x000DB83C
	void ICollection<!0>.Add(T item)
	{
		this.Add(item);
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000DD648 File Offset: 0x000DB848
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x060023FA RID: 9210 RVA: 0x000DD650 File Offset: 0x000DB850
	public int Count
	{
		get
		{
			return this.dictionary.Count;
		}
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x060023FB RID: 9211 RVA: 0x000DD660 File Offset: 0x000DB860
	public virtual bool IsReadOnly
	{
		get
		{
			return this.dictionary.IsReadOnly;
		}
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000DD670 File Offset: 0x000DB870
	public void Clear()
	{
		this.linkedList.Clear();
		this.dictionary.Clear();
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000DD688 File Offset: 0x000DB888
	public bool Remove(T item)
	{
		LinkedListNode<T> node;
		if (!this.dictionary.TryGetValue(item, out node))
		{
			return false;
		}
		this.dictionary.Remove(item);
		this.linkedList.Remove(node);
		return true;
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000DD6C8 File Offset: 0x000DB8C8
	public IEnumerator<T> GetEnumerator()
	{
		return this.linkedList.GetEnumerator();
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000DD6DC File Offset: 0x000DB8DC
	public bool Contains(T item)
	{
		return this.dictionary.ContainsKey(item);
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000DD6EC File Offset: 0x000DB8EC
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.linkedList.CopyTo(array, arrayIndex);
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000DD6FC File Offset: 0x000DB8FC
	public bool Add(T item)
	{
		if (this.dictionary.ContainsKey(item))
		{
			return false;
		}
		LinkedListNode<T> value = this.linkedList.AddLast(item);
		this.dictionary.Add(item, value);
		return true;
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000DD738 File Offset: 0x000DB938
	public T Last()
	{
		return this.linkedList.Last.Value;
	}

	// Token: 0x04001631 RID: 5681
	private readonly IDictionary<T, LinkedListNode<T>> dictionary;

	// Token: 0x04001632 RID: 5682
	private readonly LinkedList<T> linkedList;
}
