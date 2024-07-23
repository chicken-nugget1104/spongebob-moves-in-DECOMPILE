using System;

// Token: 0x02000410 RID: 1040
public class PurchasableDecorator : EntityDecorator
{
	// Token: 0x06001FDF RID: 8159 RVA: 0x000C2780 File Offset: 0x000C0980
	public PurchasableDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x000C278C File Offset: 0x000C098C
	// (set) Token: 0x06001FE1 RID: 8161 RVA: 0x000C27C8 File Offset: 0x000C09C8
	public bool Purchased
	{
		get
		{
			return !this.Variable.ContainsKey("purchased") || (bool)this.Variable["purchased"];
		}
		set
		{
			this.Variable["purchased"] = value;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x000C27E0 File Offset: 0x000C09E0
	public override string SoundOnTouch
	{
		get
		{
			if (this.Purchased)
			{
				return base.SoundOnTouch;
			}
			return (string)this.Invariable["sound_on_touch_error"];
		}
	}
}
