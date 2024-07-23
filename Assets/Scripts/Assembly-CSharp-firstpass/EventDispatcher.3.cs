using System;

// Token: 0x0200006A RID: 106
public class EventDispatcher<T>
{
	// Token: 0x14000028 RID: 40
	// (add) Token: 0x060003B6 RID: 950 RVA: 0x000102A4 File Offset: 0x0000E4A4
	// (remove) Token: 0x060003B7 RID: 951 RVA: 0x000102C0 File Offset: 0x0000E4C0
	private event Action<T> eventListener;

	// Token: 0x060003B8 RID: 952 RVA: 0x000102DC File Offset: 0x0000E4DC
	public void SetListener(Action<T> value)
	{
		this.eventListener = value;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x000102E8 File Offset: 0x0000E4E8
	public Action<T> GetListener()
	{
		return this.eventListener;
	}

	// Token: 0x060003BA RID: 954 RVA: 0x000102F0 File Offset: 0x0000E4F0
	public void AddListener(Action<T> value)
	{
		if (value == null)
		{
			return;
		}
		if (this.eventListener == null)
		{
			this.eventListener = (Action<T>)Delegate.Combine(this.eventListener, value);
			return;
		}
		this.eventListener = (Action<T>)Delegate.Remove(this.eventListener, value);
		this.eventListener = (Action<T>)Delegate.Combine(this.eventListener, value);
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00010358 File Offset: 0x0000E558
	public void RemoveListener(Action<T> value)
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener = (Action<T>)Delegate.Remove(this.eventListener, value);
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00010380 File Offset: 0x0000E580
	public void ClearListeners()
	{
		this.eventListener = null;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0001038C File Offset: 0x0000E58C
	public void FireEvent(T message)
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener(message);
	}
}
