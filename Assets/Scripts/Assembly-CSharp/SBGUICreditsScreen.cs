using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class SBGUICreditsScreen : SBGUIScrollableDialog
{
	// Token: 0x0600046C RID: 1132 RVA: 0x0001BEE8 File Offset: 0x0001A0E8
	public void Setup(Session session)
	{
		this.session = session;
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001BEF4 File Offset: 0x0001A0F4
	public void CreateUI()
	{
		SBGUICreditsSlot component = this.slotPrefab.GetComponent<SBGUICreditsSlot>();
		SBGUIImage sbguiimage = (SBGUIImage)component.FindChild("slot_boundary");
		Vector2 vector = sbguiimage.Size * 0.01f;
		Rect scrollSize = new Rect(0f, 0f, vector.x, vector.y);
		this.region.ResetScroll(scrollSize);
		this.region.ResetToMinScroll();
		this.CreateCreditsSlot(this.session, this.region.Marker, Vector3.zero);
		base.StartCoroutine(this.ScrollingCredits());
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001BF90 File Offset: 0x0001A190
	private IEnumerator ScrollingCredits()
	{
		yield return null;
		bool keepScrolling = true;
		while (keepScrolling)
		{
			if (this.region.WasRecentlyTouched)
			{
				keepScrolling = false;
			}
			this.region.momentum.TrackForSmoothing(this.region.subViewMarker.tform.position + new Vector3(0f, 0.005f, 0f));
			this.region.momentum.CalculateSmoothVelocity();
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001BFAC File Offset: 0x0001A1AC
	public override void Deactivate()
	{
		base.StopCoroutine("ScrollingCredits");
		SBGUICreditsScreen.slotPool.Clear(delegate(SBGUICreditsSlot slot)
		{
			slot.Deactivate();
		});
		base.Deactivate();
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
	private SBGUICreditsSlot CreateCreditsSlot(Session session, SBGUIElement anchor, Vector3 offset)
	{
		SBGUICreditsSlot sbguicreditsSlot = SBGUICreditsScreen.slotPool.Create(new Alloc<SBGUICreditsSlot>(SBGUICreditsSlot.MakeCreditsSlot));
		sbguicreditsSlot.SetActive(true);
		sbguicreditsSlot.Setup(session, this, anchor, offset);
		return sbguicreditsSlot;
	}

	// Token: 0x0400036F RID: 879
	public GameObject slotPrefab;

	// Token: 0x04000370 RID: 880
	protected static TFPool<SBGUICreditsSlot> slotPool = new TFPool<SBGUICreditsSlot>();
}
