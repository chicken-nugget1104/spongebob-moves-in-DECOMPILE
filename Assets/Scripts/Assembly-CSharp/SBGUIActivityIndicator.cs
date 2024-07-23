using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class SBGUIActivityIndicator : SBGUIElement
{
	// Token: 0x06000398 RID: 920 RVA: 0x00011E8C File Offset: 0x0001008C
	public void InitActivityIndicator()
	{
		this.icon = (SBGUIAtlasImage)base.FindChild("icon");
		this.text = (SBGUILabel)base.FindChild("text");
		this.degPerSecond = -this.piPerSecond * 180f / 3.1415927f;
		this.iconCenter = this.icon.transform.position;
	}

	// Token: 0x17000070 RID: 112
	// (set) Token: 0x06000399 RID: 921 RVA: 0x00011EF4 File Offset: 0x000100F4
	public Vector3 Center
	{
		set
		{
			this.icon.transform.localPosition = value;
			this.iconCenter = this.icon.transform.position;
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00011F28 File Offset: 0x00010128
	public void StartActivityIndicator()
	{
		if (!this.running)
		{
			this.icon.SetVisible(true);
			this.text.SetVisible(true);
			this.running = true;
		}
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00011F60 File Offset: 0x00010160
	public void StopActivityIndicator()
	{
		if (this.running)
		{
			this.icon.SetVisible(false);
			this.text.SetVisible(false);
			this.running = false;
		}
	}

	// Token: 0x0600039C RID: 924 RVA: 0x00011F98 File Offset: 0x00010198
	public void Update()
	{
		if (this.running)
		{
			this.icon.transform.RotateAround(this.iconCenter, this.icon.transform.forward, this.degPerSecond * Time.deltaTime);
		}
	}

	// Token: 0x04000257 RID: 599
	public float piPerSecond;

	// Token: 0x04000258 RID: 600
	private SBGUIAtlasImage icon;

	// Token: 0x04000259 RID: 601
	private SBGUILabel text;

	// Token: 0x0400025A RID: 602
	private Vector3 iconCenter;

	// Token: 0x0400025B RID: 603
	private bool running;

	// Token: 0x0400025C RID: 604
	private float degPerSecond;
}
