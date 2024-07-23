using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class SBGUIScrollBar : SBGUIElement
{
	// Token: 0x060005D8 RID: 1496 RVA: 0x00025468 File Offset: 0x00023668
	protected override void OnEnable()
	{
		TFUtils.Assert(this.region != null, "Scrollbar must have an associated SBGUIScrollRegion");
		base.OnEnable();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00025488 File Offset: 0x00023688
	public Rect GetWorldRect()
	{
		return this.scrollBar.GetWorldRect();
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00025498 File Offset: 0x00023698
	public void SetThumbSize(float percent)
	{
		Vector2 size = this.thumb.Size;
		if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL)
		{
			size.x = this.scrollBar.Size.x * percent;
		}
		else if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.VERTICAL)
		{
			size.y = this.scrollBar.Size.y * percent;
		}
		this.thumb.Size = size;
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00025514 File Offset: 0x00023714
	public void UpdateScroll(float thumbLoc)
	{
		Vector3 position = this.thumb.tform.position;
		Rect worldRect = this.GetWorldRect();
		if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL)
		{
			position.x = worldRect.xMin + Mathf.Lerp(0f, worldRect.width, thumbLoc);
		}
		else if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.VERTICAL)
		{
			position.y = worldRect.yMax - Mathf.Lerp(0f, worldRect.height, thumbLoc);
		}
		this.thumb.tform.position = position;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x000255A8 File Offset: 0x000237A8
	public void Reset()
	{
		this.UpdateScroll(0f);
	}

	// Token: 0x04000471 RID: 1137
	public SBGUIScrollRegion.SCROLL_DIRECTION scrollDirection = SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL;

	// Token: 0x04000472 RID: 1138
	public SBGUIImage scrollBar;

	// Token: 0x04000473 RID: 1139
	public SBGUIImage thumb;

	// Token: 0x04000474 RID: 1140
	public SBGUIScrollRegion region;
}
