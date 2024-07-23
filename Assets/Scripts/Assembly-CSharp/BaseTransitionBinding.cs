using System;

// Token: 0x0200032B RID: 811
public abstract class BaseTransitionBinding
{
	// Token: 0x060017B2 RID: 6066 RVA: 0x0009D204 File Offset: 0x0009B404
	protected void Initialize(Action<Session> action)
	{
		this.action = action;
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x0009D210 File Offset: 0x0009B410
	public void Apply(Session session)
	{
		this.action(session);
	}

	// Token: 0x04000FB3 RID: 4019
	private Action<Session> action;
}
