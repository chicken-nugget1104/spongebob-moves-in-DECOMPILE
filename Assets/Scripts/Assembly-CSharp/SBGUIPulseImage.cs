using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class SBGUIPulseImage : SBGUIAtlasImage, IPulsable
{
	// Token: 0x060005A5 RID: 1445 RVA: 0x00024184 File Offset: 0x00022384
	private SBGUIPulseImage()
	{
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0002418C File Offset: 0x0002238C
	public static SBGUIPulseImage Create(SBGUIElement parent, string asset, Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIPulseImage_{0}", SBGUIElement.InstanceID));
		SBGUIPulseImage sbguipulseImage = gameObject.AddComponent<SBGUIPulseImage>();
		sbguipulseImage.Initialize(parent, new Rect(0f, 0f, -1f, -1f), asset);
		sbguipulseImage.sprite.SetSize(restingSize);
		sbguipulseImage.InitializePulser(restingSize, amplitude, period, OnCompleteCallback);
		return sbguipulseImage;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x000241F4 File Offset: 0x000223F4
	public void InitializePulser(Vector2 restingSize, float amplitude, float period)
	{
		this.InitializePulser(restingSize, amplitude, period, null);
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00024200 File Offset: 0x00022400
	public void InitializePulser(Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		this.pulser = new DeferredPulser(restingSize, amplitude, period, new Action(this.OnPulserUpdate), OnCompleteCallback);
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00024220 File Offset: 0x00022420
	public DeferredPulser Pulser
	{
		get
		{
			return this.pulser;
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00024228 File Offset: 0x00022428
	public void Destroy()
	{
		this.pulser.Destroy();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00024240 File Offset: 0x00022440
	private void OnPulserUpdate()
	{
		if (this != null && base.sprite != null)
		{
			base.sprite.SetSize(this.pulser.Size);
		}
	}

	// Token: 0x0400045A RID: 1114
	private DeferredPulser pulser;
}
