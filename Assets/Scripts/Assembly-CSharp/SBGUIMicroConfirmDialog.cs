using System;

// Token: 0x0200008F RID: 143
public class SBGUIMicroConfirmDialog : SBGUIScreen
{
	// Token: 0x06000570 RID: 1392 RVA: 0x0002311C File Offset: 0x0002131C
	protected override void Awake()
	{
		base.Awake();
		this.hardAmountLabel = base.FindChild("amount").GetComponent<SBGUIShadowedLabel>();
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0002313C File Offset: 0x0002133C
	public void SetHardAmount(int amount)
	{
		this.hardAmountLabel.Text = amount + "?";
	}

	// Token: 0x04000437 RID: 1079
	private SBGUIShadowedLabel hardAmountLabel;
}
