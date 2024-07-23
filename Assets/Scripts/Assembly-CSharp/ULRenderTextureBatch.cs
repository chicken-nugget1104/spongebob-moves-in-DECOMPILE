using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000489 RID: 1161
public class ULRenderTextureBatch
{
	// Token: 0x0600245A RID: 9306 RVA: 0x000DE754 File Offset: 0x000DC954
	public ULRenderTextureBatch(int workingLayer)
	{
		this.batchList = new ArrayList();
		this.renderTextureCameraRig = new ULRenderTextureCameraRig(workingLayer);
	}

	// Token: 0x0600245B RID: 9307 RVA: 0x000DE774 File Offset: 0x000DC974
	public ULRenderTextureBatchEntry AddEntry(GameObject subject, int squareSize, string shaderIdentifier, ULRenderTextureCameraRig.RelativeCamDelegate camDelegate)
	{
		ULRenderTextureBatchEntry ulrenderTextureBatchEntry = new ULRenderTextureBatchEntry();
		ulrenderTextureBatchEntry.subject = subject;
		ulrenderTextureBatchEntry.target = new ULRenderTexture(squareSize, subject.name + "_material", shaderIdentifier);
		ulrenderTextureBatchEntry.camDelegate = camDelegate;
		this.batchList.Add(ulrenderTextureBatchEntry);
		return ulrenderTextureBatchEntry;
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x000DE7C4 File Offset: 0x000DC9C4
	public ULRenderTextureBatchEntry AddEntry(GameObject subject, ULRenderTexture target, ULRenderTextureCameraRig.RelativeCamDelegate camDelegate)
	{
		ULRenderTextureBatchEntry ulrenderTextureBatchEntry = new ULRenderTextureBatchEntry();
		ulrenderTextureBatchEntry.subject = subject;
		ulrenderTextureBatchEntry.target = target;
		ulrenderTextureBatchEntry.camDelegate = camDelegate;
		this.batchList.Add(ulrenderTextureBatchEntry);
		return ulrenderTextureBatchEntry;
	}

	// Token: 0x0600245D RID: 9309 RVA: 0x000DE7FC File Offset: 0x000DC9FC
	public void BatchUpdate(bool useCamDelegate)
	{
		foreach (object obj in this.batchList)
		{
			ULRenderTextureBatchEntry ulrenderTextureBatchEntry = (ULRenderTextureBatchEntry)obj;
			this.renderTextureCameraRig.RenderSubjectToTexture(ulrenderTextureBatchEntry.subject, ulrenderTextureBatchEntry.target, (!useCamDelegate) ? null : ulrenderTextureBatchEntry.camDelegate);
		}
	}

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x0600245E RID: 9310 RVA: 0x000DE890 File Offset: 0x000DCA90
	public ULRenderTextureCameraRig CameraRig
	{
		get
		{
			return this.renderTextureCameraRig;
		}
	}

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x0600245F RID: 9311 RVA: 0x000DE898 File Offset: 0x000DCA98
	public ArrayList BatchList
	{
		get
		{
			return this.batchList;
		}
	}

	// Token: 0x04001652 RID: 5714
	private ArrayList batchList;

	// Token: 0x04001653 RID: 5715
	private ULRenderTextureCameraRig renderTextureCameraRig;
}
