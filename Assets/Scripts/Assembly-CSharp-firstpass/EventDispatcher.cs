using System;

// Token: 0x02000068 RID: 104
public class EventDispatcher<T, U>
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x060003A6 RID: 934 RVA: 0x00010094 File Offset: 0x0000E294
	// (remove) Token: 0x060003A7 RID: 935 RVA: 0x000100B0 File Offset: 0x0000E2B0
	private event Action<T, U> eventListener;

	// Token: 0x060003A8 RID: 936 RVA: 0x000100CC File Offset: 0x0000E2CC
	public Delegate[] GetInvocationList()
	{
		return this.eventListener.GetInvocationList();
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x000100DC File Offset: 0x0000E2DC
	public void AddListener(Action<T, U> value)
	{
		if (value == null)
		{
			return;
		}
		if (this.eventListener == null)
		{
			this.eventListener = (Action<T, U>)Delegate.Combine(this.eventListener, value);
			return;
		}
		this.eventListener = (Action<T, U>)Delegate.Remove(this.eventListener, value);
		this.eventListener = (Action<T, U>)Delegate.Combine(this.eventListener, value);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00010144 File Offset: 0x0000E344
	public void RemoveListener(Action<T, U> value)
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener = (Action<T, U>)Delegate.Remove(this.eventListener, value);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001016C File Offset: 0x0000E36C
	public void ClearListeners()
	{
		this.eventListener = null;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00010178 File Offset: 0x0000E378
	public void FireEvent(T arg1, U arg2)
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener(arg1, arg2);
	}
}
