using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class TFPool<T>
{
	// Token: 0x0600227B RID: 8827 RVA: 0x000D3AC0 File Offset: 0x000D1CC0
	public static TFPool<T> CreatePool(int size, Alloc<T> allocDelegate)
	{
		TFPool<T> tfpool = new TFPool<T>();
		for (int i = 0; i < size; i++)
		{
			tfpool.AllocateToPool(allocDelegate);
		}
		return tfpool;
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x000D3AF0 File Offset: 0x000D1CF0
	public int AllocateToPool(Alloc<T> allocDelegate)
	{
		TFUtils.Assert(allocDelegate != null, "TFPool.AllocateToPool requires valid allocDelegate");
		T t = allocDelegate();
		this.inactiveList.Push(t);
		return this.SizeOfPool;
	}

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x0600227D RID: 8829 RVA: 0x000D3B28 File Offset: 0x000D1D28
	public int SizeOfPool
	{
		get
		{
			return this.inactiveList.Count;
		}
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x000D3B38 File Offset: 0x000D1D38
	public T Create(Alloc<T> allocDelegate = null)
	{
		T t;
		if (this.inactiveList.Count > 0)
		{
			t = this.inactiveList.Pop();
		}
		else
		{
			if (allocDelegate == null)
			{
				throw new UnityException("TFPool.Create(): Pool is empty and no alloc is provided.");
			}
			t = allocDelegate();
		}
		this.activeSet.Add(t);
		return t;
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x000D3B94 File Offset: 0x000D1D94
	public bool Release(T item)
	{
		if (this.activeSet.Remove(item))
		{
			this.inactiveList.Push(item);
			return true;
		}
		return false;
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000D3BC4 File Offset: 0x000D1DC4
	public void Clear(Deactivate<T> deactivateDelegate = null)
	{
		Deactivate<T> deactivate;
		if (deactivateDelegate != null)
		{
			deactivate = deactivateDelegate;
		}
		else
		{
			deactivate = delegate(T t)
			{
			};
		}
		Deactivate<T> deactivate2 = deactivate;
		foreach (T t2 in this.activeSet)
		{
			deactivate2(t2);
			this.inactiveList.Push(t2);
		}
		this.activeSet.Clear();
	}

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06002281 RID: 8833 RVA: 0x000D3C6C File Offset: 0x000D1E6C
	public HashSet<T> ActiveSet
	{
		get
		{
			return this.activeSet;
		}
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000D3C74 File Offset: 0x000D1E74
	public void Purge(Deactivate<T> deactivateDelegate = null)
	{
		this.Clear(null);
		Deactivate<T> deactivate;
		if (deactivateDelegate != null)
		{
			deactivate = deactivateDelegate;
		}
		else
		{
			deactivate = delegate(T t)
			{
			};
		}
		Deactivate<T> deactivate2 = deactivate;
		while (this.inactiveList.Count > 0)
		{
			deactivate2(this.inactiveList.Pop());
		}
	}

	// Token: 0x04001548 RID: 5448
	private Stack<T> inactiveList = new Stack<T>();

	// Token: 0x04001549 RID: 5449
	private HashSet<T> activeSet = new HashSet<T>();
}
