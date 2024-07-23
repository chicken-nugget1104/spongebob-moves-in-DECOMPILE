using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class SBGUIPulseButton : SBGUIAtlasButton, IPulsable
{
	// Token: 0x0600059C RID: 1436 RVA: 0x00024024 File Offset: 0x00022224
	private SBGUIPulseButton()
	{
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0002402C File Offset: 0x0002222C
	protected override void Awake()
	{
		base.Awake();
		this.InitializePulser(this.RestingSize, this.Amplitude, this.Period);
		Action value = delegate()
		{
			this.pulser.PulseOneShot();
		};
		base.gameObject.GetComponent<TapButton>().BeginEvent.AddListener(value);
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0002407C File Offset: 0x0002227C
	public static SBGUIPulseButton Create(SBGUIElement parent, string asset, Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIPulseButton_{0}", SBGUIElement.InstanceID));
		SBGUIPulseButton sbguipulseButton = gameObject.AddComponent<SBGUIPulseButton>();
		sbguipulseButton.Initialize(parent, new Rect(0f, 0f, -1f, -1f), asset);
		sbguipulseButton.sprite.SetSize(restingSize);
		sbguipulseButton.InitializePulser(restingSize, amplitude, period, OnCompleteCallback);
		return sbguipulseButton;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x000240E4 File Offset: 0x000222E4
	public void InitializePulser(Vector2 restingSize, float amplitude, float period)
	{
		this.InitializePulser(restingSize, amplitude, period, null);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x000240F0 File Offset: 0x000222F0
	public void InitializePulser(Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		if (this.pulser != null)
		{
			this.pulser.Destroy();
		}
		this.pulser = new DeferredPulser(restingSize, amplitude, period, new Action(this.OnPulserUpdate), OnCompleteCallback);
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00024130 File Offset: 0x00022330
	public DeferredPulser Pulser
	{
		get
		{
			return this.pulser;
		}
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00024138 File Offset: 0x00022338
	public override void OnDestroy()
	{
		if (this.pulser != null)
		{
			this.pulser.Destroy();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0002415C File Offset: 0x0002235C
	private void OnPulserUpdate()
	{
		base.sprite.SetSize(this.pulser.Size);
	}

	// Token: 0x04000456 RID: 1110
	public Vector2 RestingSize;

	// Token: 0x04000457 RID: 1111
	public float Amplitude;

	// Token: 0x04000458 RID: 1112
	public float Period;

	// Token: 0x04000459 RID: 1113
	private DeferredPulser pulser;
}
