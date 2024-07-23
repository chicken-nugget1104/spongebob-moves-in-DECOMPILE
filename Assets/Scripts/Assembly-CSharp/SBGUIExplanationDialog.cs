using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class SBGUIExplanationDialog : SBGUIModalDialog
{
	// Token: 0x060004C5 RID: 1221 RVA: 0x0001E1D8 File Offset: 0x0001C3D8
	public void Setup(string message)
	{
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("dialog_label");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("dialog_label_boundary");
		Debug.LogError("dialogBoundary " + sbguiatlasImage);
		sbguilabel.SetText(message);
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("character_icon");
		sbguiatlasImage2.SetTextureFromAtlas("MrKrabsPortrait_Whaddayamean", true, false, true, false, false, 0);
		try
		{
			if (null != sbguiatlasImage)
			{
				sbguilabel.AdjustText(sbguiatlasImage);
			}
		}
		catch (Exception ex)
		{
			TFUtils.DebugLog("-------dialogLabel.AdjustText(dialogBoundary) Exception-------------");
		}
	}
}
