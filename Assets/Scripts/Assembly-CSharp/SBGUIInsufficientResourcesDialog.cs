using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class SBGUIInsufficientResourcesDialog : SBGUIModalDialog
{
	// Token: 0x060004E5 RID: 1253 RVA: 0x0001E8B4 File Offset: 0x0001CAB4
	protected override void Awake()
	{
		base.Awake();
		this.messageLabel = (SBGUILabel)base.FindChild("message_label");
		this.titleLabel = (SBGUILabel)base.FindChild("title_label");
		this.costMarker = base.FindChild("cost_marker");
		this.storeButtonLabel = (SBGUILabel)base.FindChild("shopping_label");
		this.buyWithLabel = (SBGUILabel)base.FindChild("cost_label");
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0001E930 File Offset: 0x0001CB30
	private void Start()
	{
		this.rewardCenter = this.rewardMarker.tform.position;
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x0001E948 File Offset: 0x0001CB48
	public void SetUp(string title, string message, string storeLabel, Dictionary<string, int> resources, int? rmtCost, string rmtTexture, string prefix)
	{
		this.titleLabel.SetText(title);
		this.messageLabel.SetText(message);
		if (this.buyWithLabel != null)
		{
			this.buyWithLabel.SetText(Language.Get("!!PREFAB_BUY_WITH"));
		}
		if (this.storeButtonLabel != null)
		{
			this.storeButtonLabel.SetText(storeLabel);
		}
		if (resources == null)
		{
			return;
		}
		if (rmtCost != null)
		{
			SBGUIRewardWidget.Create(this.rewardWidgetPrefab, this.costMarker, 0f, rmtTexture, rmtCost.Value, string.Empty);
		}
		foreach (KeyValuePair<string, int> keyValuePair in resources)
		{
			this.AddItem(keyValuePair.Key, keyValuePair.Value, prefix);
		}
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0001EA54 File Offset: 0x0001CC54
	public override void AddItem(string texture, int amount, string prefix)
	{
		base.AddItem(texture, amount, prefix);
		base.View.RefreshEvent += this.CenterRewards;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0001EA84 File Offset: 0x0001CC84
	private new void CenterRewards()
	{
		Vector3 vector = this.rewardMarker.TotalBounds.center - this.rewardCenter;
		Vector3 localPosition = this.rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		this.rewardMarker.tform.localPosition = localPosition;
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0001EAE8 File Offset: 0x0001CCE8
	public Vector2 GetHardSpendPosition()
	{
		Vector2 result = base.View.WorldToScreen(this.storeButtonLabel.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	// Token: 0x0400039E RID: 926
	private SBGUILabel messageLabel;

	// Token: 0x0400039F RID: 927
	private SBGUILabel titleLabel;

	// Token: 0x040003A0 RID: 928
	private SBGUILabel storeButtonLabel;

	// Token: 0x040003A1 RID: 929
	private SBGUILabel buyWithLabel;

	// Token: 0x040003A2 RID: 930
	private SBGUIElement costMarker;

	// Token: 0x040003A3 RID: 931
	private SBGUIRewardWidget rmtCost;

	// Token: 0x040003A4 RID: 932
	private Vector3 rewardCenter;
}
