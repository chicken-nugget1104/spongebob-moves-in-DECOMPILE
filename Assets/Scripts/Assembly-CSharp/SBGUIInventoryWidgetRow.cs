using System;
using UnityEngine;
using Yarg;

// Token: 0x02000086 RID: 134
[RequireComponent(typeof(DragButton))]
[RequireComponent(typeof(YG2DRectangle))]
public class SBGUIInventoryWidgetRow : SBGUIElement
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000513 RID: 1299 RVA: 0x00020354 File Offset: 0x0001E554
	public int Product
	{
		get
		{
			return this.productId;
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0002035C File Offset: 0x0001E55C
	public void Initialize(SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback, string textureName)
	{
		TFUtils.Assert(this.Icon != null, "Must set an Icon on this InventoryWidgetRow");
		TFUtils.Assert(this.Label != null, "Must set a Label on this InventoryWidgetRow");
		this.sfxMgr = sfxMgr;
		this.onDragCallback = onDragCallback;
		this.onUiEventCallback = onUiEventCallback;
		this.Icon.InitializePulser(this.Icon.RestingSize, this.Icon.Amplitude, this.Icon.Period, new Action(this.ResetToNeutral));
		this.Icon.SetTextureFromAtlas(textureName);
		DragButton component = this.Icon.GetComponent<DragButton>();
		component.DragEvent.AddListener(new Action<YGEvent>(this.HandleDragEvent));
		this.ResetToNeutral();
		base.GetComponent<DragButton>().DragEvent.AddListener(this.onUiEventCallback);
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00020430 File Offset: 0x0001E630
	public override void SetVisible(bool viz)
	{
		base.SetVisible(viz);
		this.Icon.SetVisible(viz);
		this.Label.SetVisible(viz);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00020454 File Offset: 0x0001E654
	public void SetRecipeIcon(string texture)
	{
		this.Icon.SetTextureFromAtlas(texture);
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00020464 File Offset: 0x0001E664
	public void SetProductToTrack(int productId)
	{
		this.productId = productId;
		this.SessionActionId = "GrubWidget_Row_" + productId;
		this.Icon.SessionActionId = "GrubWidget_Icon_" + productId;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x000204AC File Offset: 0x0001E6AC
	private void SetAmount(int quantity)
	{
		if (this.amount != quantity)
		{
			this.amount = quantity;
			this.Label.SetText(quantity.ToString());
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x000204E0 File Offset: 0x0001E6E0
	public void OnUpdate(ResourceManager resourceMgr, float topHideThreshold, float bottomHideThreshold)
	{
		Vector2 screenPosition = base.GetScreenPosition();
		if (screenPosition.y > bottomHideThreshold || screenPosition.y < topHideThreshold)
		{
			if (base.IsActive())
			{
				this.SetActive(false);
			}
		}
		else if (!base.IsActive())
		{
			this.SetActive(true);
		}
		this.SetAmount(resourceMgr.Query(this.productId) - this.fakeDeduction);
		this.fakeDeduction = 0;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00020558 File Offset: 0x0001E758
	public void PulseError()
	{
		this.PulseError(3);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00020564 File Offset: 0x0001E764
	public void PulseError(int count)
	{
		this.Icon.Pulser.PulseOneShot(count);
		this.Label.SetColor(Color.red);
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00020588 File Offset: 0x0001E788
	public void IncrementDeductionsForTick()
	{
		this.fakeDeduction++;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00020598 File Offset: 0x0001E798
	private void ResetToNeutral()
	{
		this.Label.SetColor(Color.white);
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x000205AC File Offset: 0x0001E7AC
	private void HandleDragEvent(YGEvent evt)
	{
		this.onUiEventCallback(evt);
		if (evt.type == YGEvent.TYPE.TOUCH_BEGIN)
		{
			if (this.amount > 0)
			{
				this.onDragCallback(this.productId, evt);
			}
			else
			{
				this.sfxMgr.PlaySound("Error");
				this.PulseError(0);
			}
		}
	}

	// Token: 0x040003DC RID: 988
	public SBGUIPulseButton Icon;

	// Token: 0x040003DD RID: 989
	public SBGUILabel Label;

	// Token: 0x040003DE RID: 990
	private int amount;

	// Token: 0x040003DF RID: 991
	private int productId;

	// Token: 0x040003E0 RID: 992
	private int fakeDeduction;

	// Token: 0x040003E1 RID: 993
	private Action<int, YGEvent> onDragCallback;

	// Token: 0x040003E2 RID: 994
	private Action<YGEvent> onUiEventCallback;

	// Token: 0x040003E3 RID: 995
	private SoundEffectManager sfxMgr;
}
