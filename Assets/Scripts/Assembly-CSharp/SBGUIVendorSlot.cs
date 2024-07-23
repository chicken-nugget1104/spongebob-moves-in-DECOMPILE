using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class SBGUIVendorSlot : SBGUIElement
{
	// Token: 0x060006EA RID: 1770 RVA: 0x0002C1F4 File Offset: 0x0002A3F4
	public static SBGUIVendorSlot CreateVendorSlot(Session session, SBGUIVendorScreen vendorScreen)
	{
		SBGUIVendorSlot slot = (SBGUIVendorSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/VendorSlot");
		if (slot.selectionStarburst == null)
		{
			slot.selectionStarburst = (SBGUIAtlasImage)slot.FindChild("selection_starburst");
			slot.prefabStarburstPos = slot.selectionStarburst.tform.localPosition;
		}
		if (slot.slotBackground == null)
		{
			slot.slotBackground = (SBGUIAtlasButton)slot.FindChild("slot_background");
		}
		if (slot.quantityLabel == null)
		{
			slot.quantityLabel = (SBGUILabel)slot.FindChild("quantity_label");
		}
		if (slot.quantityCircle == null)
		{
			slot.quantityCircle = (SBGUIAtlasImage)slot.FindChild("quantity_circle");
		}
		if (slot.itemIcon == null)
		{
			slot.itemIcon = (SBGUIAtlasImage)slot.FindChild("icon");
			slot.prefabItemIconPos = slot.itemIcon.tform.localPosition;
		}
		if (slot.lockedMask == null)
		{
			slot.lockedMask = (SBGUIAtlasImage)slot.FindChild("locked_mask");
		}
		slot.SetHighlight(false, false);
		slot.AttachActionToButton("slot_background", delegate()
		{
			if (!slot.empty)
			{
				session.TheSoundEffectManager.PlaySound("HighlightItem");
				vendorScreen.HighlightSlot(session, slot);
			}
			else
			{
				session.TheSoundEffectManager.PlaySound("Error");
			}
		});
		return slot;
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060006EB RID: 1771 RVA: 0x0002C3E0 File Offset: 0x0002A5E0
	public bool Empty
	{
		get
		{
			return this.empty;
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002C3E8 File Offset: 0x0002A5E8
	// (set) Token: 0x060006ED RID: 1773 RVA: 0x0002C3F0 File Offset: 0x0002A5F0
	public int SlotID
	{
		get
		{
			return this.slotId;
		}
		set
		{
			if (value >= 0)
			{
				this.slotId = value;
			}
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002C400 File Offset: 0x0002A600
	// (set) Token: 0x060006EF RID: 1775 RVA: 0x0002C408 File Offset: 0x0002A608
	public bool IsSpecial
	{
		get
		{
			return this.isSpecial;
		}
		set
		{
			this.isSpecial = value;
		}
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0002C414 File Offset: 0x0002A614
	public void SetHighlight(bool highlight, bool skipAnimation = false)
	{
		if (!skipAnimation && !this.empty)
		{
			if (highlight)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.AnimateIn(1f, new Func<float, float, float, float>(Easing.EaseOutElastic)));
			}
			else
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.AnimateOut(0.2f, new Func<float, float, float, float>(Easing.Linear)));
			}
		}
		else if (this.selectionStarburst != null)
		{
			this.selectionStarburst.SetActive(highlight);
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x0002C4A8 File Offset: 0x0002A6A8
	public void SetEmpty(bool setting, bool specialVendingSlot = false)
	{
		this.empty = setting;
		if (specialVendingSlot)
		{
			this.slotBackground.SetActive(!setting);
		}
		else
		{
			Vector3 b = new Vector3(0f, 0f, -0.04f);
			if (!setting)
			{
				this.itemIcon.tform.localPosition = this.prefabItemIconPos + b;
				this.selectionStarburst.tform.localPosition = this.prefabStarburstPos + b;
			}
			else
			{
				this.itemIcon.tform.localPosition = this.prefabItemIconPos - b;
				this.selectionStarburst.tform.localPosition = this.prefabStarburstPos - b;
			}
		}
		if (this.lockedMask != null)
		{
			this.lockedMask.SetActive(setting);
		}
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0002C584 File Offset: 0x0002A784
	private IEnumerator AnimateIn(float duration, Func<float, float, float, float> easingMethod)
	{
		if (this.selectionStarburst == null)
		{
			yield break;
		}
		this.transitioning = true;
		this.selectionStarburst.SetActive(true);
		float interp = 0f;
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			Vector3 interpPos = Easing.Vector3Easing(Vector3.zero, Vector3.one, interp, easingMethod);
			this.selectionStarburst.tform.localScale = interpPos;
			yield return null;
		}
		if (this.IsSpecial)
		{
			this.tintValue = 0.5f;
			this.scaleValue = 1f;
			this.lerpHigh = true;
			this.specialInterp = 0f;
		}
		this.transitioning = false;
		yield break;
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002C5BC File Offset: 0x0002A7BC
	private IEnumerator AnimateOut(float duration, Func<float, float, float, float> easingMethod)
	{
		if (this.selectionStarburst == null)
		{
			yield break;
		}
		this.transitioning = true;
		float interp = 0f;
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			Vector3 interpPos = Easing.Vector3Easing(Vector3.one, Vector3.zero, interp, easingMethod);
			this.selectionStarburst.tform.localScale = interpPos;
			yield return null;
		}
		this.transitioning = false;
		this.selectionStarburst.SetActive(false);
		yield break;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0002C5F4 File Offset: 0x0002A7F4
	public static string GetSessionActionId(VendorDefinition vendorDef)
	{
		return string.Format("Slot_{0}", vendorDef.did);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0002C60C File Offset: 0x0002A80C
	public void Update()
	{
		if (!this.IsSpecial && this.selectionStarburst != null && this.selectionStarburst.IsActive())
		{
			this.selectionStarburst.tform.RotateAround(new Vector3(0f, 0f, 1f), -1f * Time.deltaTime);
		}
		if (this.IsSpecial && this.selectionStarburst != null && this.selectionStarburst.IsActive() && !this.transitioning)
		{
			if (this.specialInterp <= 1f)
			{
				this.specialInterp += Time.deltaTime / 0.75f;
			}
			else
			{
				this.specialInterp = 0f;
				if (this.lerpHigh)
				{
					this.lerpHigh = false;
				}
				else
				{
					this.lerpHigh = true;
				}
			}
			if (this.lerpHigh)
			{
				this.tintValue = Mathf.SmoothStep(0.5f, 0.75f, this.specialInterp);
				this.scaleValue = Mathf.SmoothStep(1f, 1.2f, this.specialInterp);
			}
			else
			{
				this.tintValue = Mathf.SmoothStep(0.75f, 0.5f, this.specialInterp);
				this.scaleValue = Mathf.SmoothStep(1.2f, 1f, this.specialInterp);
			}
			this.selectionStarburst.GetComponent<MeshRenderer>().material.SetColor("_TintColor", new Color(this.tintValue, this.tintValue, this.tintValue, 1f));
			this.selectionStarburst.transform.localScale = new Vector3(this.scaleValue, 1f, 1f);
		}
	}

	// Token: 0x04000545 RID: 1349
	protected const float PULSE_RATE = 0.75f;

	// Token: 0x04000546 RID: 1350
	protected const float TINT_LOW = 0.5f;

	// Token: 0x04000547 RID: 1351
	protected const float TINT_HIGH = 0.75f;

	// Token: 0x04000548 RID: 1352
	protected const float SCALE_LOW = 1f;

	// Token: 0x04000549 RID: 1353
	protected const float SCALE_HIGH = 1.2f;

	// Token: 0x0400054A RID: 1354
	public SBGUIAtlasImage selectionStarburst;

	// Token: 0x0400054B RID: 1355
	public SBGUIAtlasButton slotBackground;

	// Token: 0x0400054C RID: 1356
	public SBGUILabel quantityLabel;

	// Token: 0x0400054D RID: 1357
	public SBGUIAtlasImage quantityCircle;

	// Token: 0x0400054E RID: 1358
	public SBGUIAtlasImage itemIcon;

	// Token: 0x0400054F RID: 1359
	public SBGUIAtlasImage lockedMask;

	// Token: 0x04000550 RID: 1360
	private int slotId;

	// Token: 0x04000551 RID: 1361
	private bool empty;

	// Token: 0x04000552 RID: 1362
	private Vector3 prefabItemIconPos;

	// Token: 0x04000553 RID: 1363
	private Vector3 prefabStarburstPos;

	// Token: 0x04000554 RID: 1364
	private bool isSpecial;

	// Token: 0x04000555 RID: 1365
	private bool transitioning;

	// Token: 0x04000556 RID: 1366
	protected bool lerpHigh = true;

	// Token: 0x04000557 RID: 1367
	protected float specialInterp;

	// Token: 0x04000558 RID: 1368
	protected float tintValue;

	// Token: 0x04000559 RID: 1369
	protected float scaleValue;
}
