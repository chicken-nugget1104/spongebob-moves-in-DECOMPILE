using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200024D RID: 589
public class SessionActionTextPromptPrefab : SBGUIElement
{
	// Token: 0x060012FA RID: 4858 RVA: 0x000835F4 File Offset: 0x000817F4
	public void SetLabel(string text)
	{
		this.label.SetText(text);
		this.label.AdjustText(this.labelBoundary);
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x00083614 File Offset: 0x00081814
	public void SetAnchoredPosition(TextPrompt.Anchor position)
	{
		this.frame.transform.localPosition = this.anchors[position].transform.localPosition + this.offsets[position];
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x00083658 File Offset: 0x00081858
	public void SetClickCallback(Action clickCallback)
	{
		this.frame.ClearClickEvents();
		this.frame.ClickEvent += clickCallback;
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x00083674 File Offset: 0x00081874
	protected override void Awake()
	{
		this.frame = base.FindChild("speech_bubble").gameObject.GetComponent<SBGUIButton>();
		this.label = (SBGUILabel)base.FindChild("label");
		this.labelBoundary = (SBGUIAtlasImage)base.FindChild("label_boundary");
		SBGUIElement sbguielement = base.FindChild("top");
		SBGUIElement sbguielement2 = base.FindChild("center");
		SBGUIElement sbguielement3 = base.FindChild("bottom");
		TFUtils.Assert(this.frame != null, "Could not find gameobject child named 'speech_bubble'!");
		TFUtils.Assert(this.label != null, "Could not find gameobject child named 'label'!");
		TFUtils.Assert(sbguielement != null, "Could not find gameobject child named 'top'!");
		TFUtils.Assert(sbguielement2 != null, "Could not find gameobject child named 'center'!");
		TFUtils.Assert(sbguielement3 != null, "Could not find gameobject child named 'bottom'!");
		this.anchors[TextPrompt.Anchor.Top] = sbguielement;
		this.anchors[TextPrompt.Anchor.Center] = sbguielement2;
		this.anchors[TextPrompt.Anchor.Bottom] = sbguielement3;
		this.offsets[TextPrompt.Anchor.Top] = new Vector3(this.TopOffset.x, this.TopOffset.y, this.ZDepth);
		this.offsets[TextPrompt.Anchor.Center] = new Vector3(this.CenterOffset.x, this.CenterOffset.y, this.ZDepth);
		this.offsets[TextPrompt.Anchor.Bottom] = new Vector3(this.BottomOffset.x, this.BottomOffset.y, this.ZDepth);
		if (TFUtils.GetDeviceLandscapeAspectRatio() == "3:2")
		{
			base.transform.localScale = this.LowResolutionScale;
		}
	}

	// Token: 0x04000D1C RID: 3356
	public float ZDepth = 0.1f;

	// Token: 0x04000D1D RID: 3357
	public Vector2 BottomOffset = Vector2.zero;

	// Token: 0x04000D1E RID: 3358
	public Vector2 CenterOffset = Vector2.zero;

	// Token: 0x04000D1F RID: 3359
	public Vector2 TopOffset = Vector2.zero;

	// Token: 0x04000D20 RID: 3360
	public Vector3 LowResolutionScale = Vector3.one;

	// Token: 0x04000D21 RID: 3361
	private SBGUIButton frame;

	// Token: 0x04000D22 RID: 3362
	private SBGUILabel label;

	// Token: 0x04000D23 RID: 3363
	private SBGUIAtlasImage labelBoundary;

	// Token: 0x04000D24 RID: 3364
	private Dictionary<TextPrompt.Anchor, SBGUIElement> anchors = new Dictionary<TextPrompt.Anchor, SBGUIElement>();

	// Token: 0x04000D25 RID: 3365
	private Dictionary<TextPrompt.Anchor, Vector3> offsets = new Dictionary<TextPrompt.Anchor, Vector3>();
}
