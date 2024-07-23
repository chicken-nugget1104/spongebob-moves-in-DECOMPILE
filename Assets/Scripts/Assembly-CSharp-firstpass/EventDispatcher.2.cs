using System;

// Token: 0x02000069 RID: 105
public class EventDispatcher<T, U, V>
{
	// Token: 0x14000027 RID: 39
	// (add) Token: 0x060003AE RID: 942 RVA: 0x0001019C File Offset: 0x0000E39C
	// (remove) Token: 0x060003AF RID: 943 RVA: 0x000101B8 File Offset: 0x0000E3B8
	private event Action<T, U, V> eventListener;

	// Token: 0x060003B0 RID: 944 RVA: 0x000101D4 File Offset: 0x0000E3D4
	public Delegate[] GetInvocationList()
	{
		return this.eventListener.GetInvocationList();
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x000101E4 File Offset: 0x0000E3E4
	public void AddListener(Action<T, U, V> value)
	{
		if (value == null)
		{
			return;
		}
		if (this.eventListener == null)
		{
			this.eventListener = (Action<T, U, V>)Delegate.Combine(this.eventListener, value);
			return;
		}
		this.eventListener = (Action<T, U, V>)Delegate.Remove(this.eventListener, value);
		this.eventListener = (Action<T, U, V>)Delegate.Combine(this.eventListener, value);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0001024C File Offset: 0x0000E44C
	public void RemoveListener(Action<T, U, V> value)
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener = (Action<T, U, V>)Delegate.Remove(this.eventListener, value);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00010274 File Offset: 0x0000E474
	public void ClearListeners()
	{
		this.eventListener = null;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00010280 File Offset: 0x0000E480
	public void FireEvent(T arg1, U arg2, V arg3)
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener(arg1, arg2, arg3);
	}
}
