using System;

// Token: 0x02000413 RID: 1043
public class WorkableDecorator : EntityDecorator
{
	// Token: 0x06001FF2 RID: 8178 RVA: 0x000C29F0 File Offset: 0x000C0BF0
	public WorkableDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x000C29FC File Offset: 0x000C0BFC
	// (set) Token: 0x06001FF4 RID: 8180 RVA: 0x000C2A04 File Offset: 0x000C0C04
	public Identity Worker
	{
		get
		{
			return this.worker;
		}
		set
		{
			this.worker = value;
		}
	}

	// Token: 0x040013BF RID: 5055
	private Identity worker;
}
