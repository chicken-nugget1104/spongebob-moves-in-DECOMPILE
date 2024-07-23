using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class SBContentPatcher : EventDispatcher<string>
{
	// Token: 0x06000330 RID: 816 RVA: 0x00010254 File Offset: 0x0000E454
	public SBContentPatcher()
	{
		this.soaring_delegate = new SBContentPatcher.SoaringVersionsDelegate();
		this.soaring_delegate.patcher = this;
		Soaring.AddDelegate(this.soaring_delegate);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0001028C File Offset: 0x0000E48C
	~SBContentPatcher()
	{
	}

	// Token: 0x06000332 RID: 818 RVA: 0x000102C4 File Offset: 0x0000E4C4
	public void RemoveDelegate()
	{
		if (this.soaring_delegate != null)
		{
			Soaring.RemoveDelegate(this.soaring_delegate);
		}
		this.soaring_delegate = null;
	}

	// Token: 0x06000333 RID: 819 RVA: 0x000102E4 File Offset: 0x0000E4E4
	public void LoadManifest(bool checkForUpdates)
	{
		if (SBSettings.BypassPatching)
		{
			SoaringDebug.Log("Bypass Patching");
			base.FireEvent("patchingNotNecessary");
		}
		else
		{
			SoaringDebug.Log("Checking for Updates");
			Soaring.CheckFilesForUpdates(checkForUpdates);
		}
	}

	// Token: 0x04000221 RID: 545
	public const string PATCHING_DONE_EVENT = "patchingDone";

	// Token: 0x04000222 RID: 546
	public const string PATCHING_NECESSARY_EVENT = "patchingNecessary";

	// Token: 0x04000223 RID: 547
	public const string PATCHING_NOT_NECESSARY_EVENT = "patchingNotNecessary";

	// Token: 0x04000224 RID: 548
	private SBContentPatcher.SoaringVersionsDelegate soaring_delegate;

	// Token: 0x0200004C RID: 76
	private class SoaringVersionsDelegate : SoaringDelegate
	{
		// Token: 0x06000335 RID: 821 RVA: 0x00010330 File Offset: 0x0000E530
		public override void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
		{
			if (this.patcher == null || data != null)
			{
				return;
			}
			if (state == SoaringState.Success)
			{
				this.patcher.FireEvent("patchingDone");
				this.patcher.RemoveDelegate();
				this.patcher = null;
			}
			else if (state == SoaringState.Fail)
			{
				if (error == null)
				{
					this.patcher.FireEvent("patchingDone");
					this.patcher.RemoveDelegate();
					this.patcher = null;
				}
				else if (error.ErrorCode == 33)
				{
					SoaringDebug.Log(error, LogType.Error);
					this.patcher.FireEvent("patchingNotNecessary");
					this.patcher.RemoveDelegate();
					this.patcher = null;
				}
				else
				{
					SoaringDebug.Log("SBContentPatcher: " + error, LogType.Error);
					this.patcher.FireEvent("patchingDone");
					this.patcher.RemoveDelegate();
					SoaringInternal.instance.TriggerOfflineMode(true);
					this.patcher = null;
				}
			}
			else if (state == SoaringState.Update && error == null)
			{
				SoaringDebug.Log("SBContentPatcher: Need Patching", LogType.Error);
				this.patcher.FireEvent("patchingNecessary");
			}
		}

		// Token: 0x04000225 RID: 549
		public SBContentPatcher patcher;
	}
}
