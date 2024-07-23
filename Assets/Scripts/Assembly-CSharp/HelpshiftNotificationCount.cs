using System;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class HelpshiftNotificationCount : MonoBehaviour
{
	// Token: 0x060001F4 RID: 500 RVA: 0x0000995C File Offset: 0x00007B5C
	public HelpshiftNotificationCount()
	{
		this.m_iCount = 0;
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000996C File Offset: 0x00007B6C
	public int Count
	{
		get
		{
			return this.m_iCount;
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00009974 File Offset: 0x00007B74
	public void didReceiveInAppNotificationCount(string count)
	{
		int iCount = 0;
		int.TryParse(count, out iCount);
		this.m_iCount = iCount;
		this.UpdateCountNumber();
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000999C File Offset: 0x00007B9C
	public void ResetCount()
	{
		this.m_iCount = 0;
		this.UpdateCountNumber();
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x000099AC File Offset: 0x00007BAC
	public void UpdateCountNumber()
	{
		SBGUIStandardScreen sbguistandardScreen = null;
		SBGUIScreen[] componentsInChildren = GameObject.Find("GUIMainView").GetComponentsInChildren<SBGUIScreen>();
		foreach (SBGUIScreen sbguiscreen in componentsInChildren)
		{
			if (sbguiscreen.name.Contains("StandardUI"))
			{
				sbguistandardScreen = (SBGUIStandardScreen)sbguiscreen;
			}
		}
		if (sbguistandardScreen != null)
		{
			sbguistandardScreen.HelpshiftNotificationCount = this.m_iCount;
			sbguistandardScreen.ShowHelpshiftNotification();
		}
	}

	// Token: 0x04000110 RID: 272
	private int m_iCount;
}
