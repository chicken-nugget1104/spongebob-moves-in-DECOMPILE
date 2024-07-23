using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class UpsightContentRequest : MonoBehaviour
{
	// Token: 0x060007F3 RID: 2035 RVA: 0x0001E674 File Offset: 0x0001C874
	private void Start()
	{
		Upsight.sendContentRequest(this.placementID, this.showsOverlayImmediately, this.shouldAnimate, null);
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0001E690 File Offset: 0x0001C890
	private void OnEnable()
	{
		UpsightManager.contentRequestLoadedEvent += this.contentRequestLoaded;
		UpsightManager.contentRequestFailedEvent += this.contentRequestFailed;
		UpsightManager.contentWillDisplayEvent += this.contentWillDisplay;
		UpsightManager.contentDismissedEvent += this.contentDismissed;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0001E6E4 File Offset: 0x0001C8E4
	private void OnDisable()
	{
		UpsightManager.contentRequestLoadedEvent -= this.contentRequestLoaded;
		UpsightManager.contentRequestFailedEvent -= this.contentRequestFailed;
		UpsightManager.contentWillDisplayEvent -= this.contentWillDisplay;
		UpsightManager.contentDismissedEvent -= this.contentDismissed;
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0001E738 File Offset: 0x0001C938
	private void contentRequestLoaded(string placementID)
	{
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x0001E73C File Offset: 0x0001C93C
	private void contentRequestFailed(string placementID, string error)
	{
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0001E740 File Offset: 0x0001C940
	private void contentWillDisplay(string placementID)
	{
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0001E744 File Offset: 0x0001C944
	private void contentDismissed(string placementID, string dismissType)
	{
	}

	// Token: 0x040004ED RID: 1261
	public string placementID;

	// Token: 0x040004EE RID: 1262
	public bool showsOverlayImmediately = true;

	// Token: 0x040004EF RID: 1263
	public bool shouldAnimate = true;
}
