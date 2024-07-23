using System;
using UnityEngine;

// Token: 0x02000078 RID: 120
public class SBGUIDebugScreen : SBGUIModalDialog
{
	// Token: 0x06000486 RID: 1158 RVA: 0x0001D35C File Offset: 0x0001B55C
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0001D368 File Offset: 0x0001B568
	public void Setup(Session session)
	{
		this.toggleFreeEditLabel = (SBGUILabel)base.FindChild("free_edit_label");
		this.toggleFramerateCounterLabel = (SBGUILabel)base.FindChild("framerate_counter_label");
		this.toggleHitBoxesLabel = (SBGUILabel)base.FindChild("toggle_hit_boxes_label");
		this.toggleFootprintsLabel = (SBGUILabel)base.FindChild("toggle_footprints_label");
		this.toggleExpansionBordersLabel = (SBGUILabel)base.FindChild("toggle_expansion_borders_label");
		this.toggleFreeCameraLabel = (SBGUILabel)base.FindChild("toggle_free_camera_label");
		this.simTimeLabel = (SBGUILabel)base.FindChild("sim_time_label");
		this.bundleVersionLabel = (SBGUILabel)base.FindChild("bundle_version_label");
		string deviceLanguage = Language.getDeviceLanguage();
		string deviceLocale = Language.getDeviceLocale();
		string text = string.Empty;
		if (SoaringInternal.Campaign != null)
		{
			text = SoaringInternal.Campaign.Group;
		}
		this.bundleVersionLabel.SetText(string.Concat(new string[]
		{
			"Version: ",
			SBSettings.BundleVersion,
			" U: ",
			Application.unityVersion,
			"\nLanguage: ",
			deviceLanguage,
			" Local: ",
			deviceLocale,
			" Group: ",
			text
		}));
		this.Refresh();
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001D4B0 File Offset: 0x0001B6B0
	public void Refresh()
	{
		if (Session.TheDebugManager.debugPlaceObjects)
		{
			this.toggleFreeEditLabel.SetText("Free Edit Mode: ON");
		}
		else
		{
			this.toggleFreeEditLabel.SetText("Free Edit Mode: OFF");
		}
		if (Session.TheDebugManager.framerateCounter)
		{
			this.toggleFramerateCounterLabel.SetText("Framerate Counter: ON");
		}
		else
		{
			this.toggleFramerateCounterLabel.SetText("Framerate Counter: OFF");
		}
		if (Session.TheDebugManager.showHitBoxes)
		{
			this.toggleHitBoxesLabel.SetText("Hit Boxes: ON");
		}
		else
		{
			this.toggleHitBoxesLabel.SetText("Hit Boxes: OFF");
		}
		if (Session.TheDebugManager.showFootprints)
		{
			this.toggleFootprintsLabel.SetText("Footprints: ON");
		}
		else
		{
			this.toggleFootprintsLabel.SetText("Footprints: OFF");
		}
		if (Session.TheDebugManager.showExpansionBorders)
		{
			this.toggleExpansionBordersLabel.SetText("Expansion Borders: ON");
		}
		else
		{
			this.toggleExpansionBordersLabel.SetText("Expansion Borders: OFF");
		}
		if (Session.TheDebugManager.freeCameraMode)
		{
			this.toggleFreeCameraLabel.SetText("Free Camera: ON");
		}
		else
		{
			this.toggleFreeCameraLabel.SetText("Free Camera: OFF");
		}
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0001D604 File Offset: 0x0001B804
	private new void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		string str = string.Format("{0:ddd, MMM d, yyyy}", utcNow);
		DateTime dateTime = new DateTime(utcNow.Ticks + Convert.ToInt64(TFUtils.AddTimeOffset) * 10000000L);
		TimeSpan timeSpan = dateTime.Subtract(utcNow);
		int num = (int)timeSpan.TotalDays / 7;
		string str2 = string.Concat(new string[]
		{
			"|+",
			num.ToString("00"),
			":",
			(timeSpan.Days - num * 7).ToString("00"),
			":",
			timeSpan.Hours.ToString("00"),
			":",
			timeSpan.Minutes.ToString("00"),
			":",
			timeSpan.Seconds.ToString("00")
		});
		this.simTimeLabel.SetText(str + str2);
	}

	// Token: 0x04000384 RID: 900
	private SBGUILabel toggleFreeEditLabel;

	// Token: 0x04000385 RID: 901
	private SBGUILabel toggleFramerateCounterLabel;

	// Token: 0x04000386 RID: 902
	private SBGUILabel toggleHitBoxesLabel;

	// Token: 0x04000387 RID: 903
	private SBGUILabel toggleFootprintsLabel;

	// Token: 0x04000388 RID: 904
	private SBGUILabel toggleExpansionBordersLabel;

	// Token: 0x04000389 RID: 905
	private SBGUILabel toggleFreeCameraLabel;

	// Token: 0x0400038A RID: 906
	private SBGUILabel bundleVersionLabel;

	// Token: 0x0400038B RID: 907
	private SBGUILabel simTimeLabel;
}
