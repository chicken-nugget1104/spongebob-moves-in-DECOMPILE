using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class SBGUIScreen : SBGUIElement
{
	// Token: 0x060005CE RID: 1486 RVA: 0x000251F8 File Offset: 0x000233F8
	protected override void Awake()
	{
		this.SetParent(null);
		Vector3 localPosition = base.tform.localPosition;
		localPosition.x = 0f;
		localPosition.y = 0f;
		base.tform.localPosition = localPosition;
		this.dynamicLabels = new Dictionary<string, SBGUILabel>();
		this.dynamicMeters = new Dictionary<string, SBGUIProgressMeter>();
		this.dynamicProperties = new Dictionary<string, object>();
		SBGUIButton sbguibutton = (SBGUIButton)base.FindChild("touch_mask");
		if (sbguibutton != null)
		{
			SBGUIButton sbguibutton2 = (SBGUIButton)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/TouchableBackground");
			sbguibutton2.name = "TouchableBackground";
			sbguibutton2.SetParent(this);
			float num = GUIMainView.GetInstance().camera.farClipPlane + 5f;
			sbguibutton2.tform.localPosition = new Vector3(0f, 0f, num);
			sbguibutton.tform.localPosition = new Vector3(sbguibutton.tform.localPosition.x, sbguibutton.tform.localPosition.y, num - 0.1f);
		}
		base.Awake();
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00025314 File Offset: 0x00023514
	public override void AttachAnalyticsToButton(string buttonName, SBGUIButton button)
	{
		base.StartTimer();
		button.ClickEvent += delegate()
		{
			int playerLevel = -1;
			if (this.session != null)
			{
				if (this.session.TheGame != null)
				{
					playerLevel = this.session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
				}
				if (this.session.analytics != null)
				{
					this.session.analytics.LogDialog(this.GetType().ToString(), buttonName, this.ElapsedTime, playerLevel);
				}
			}
		};
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00025350 File Offset: 0x00023550
	public static SBGUIScreen Create(SBGUIElement parent, Session session)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIScreen_{0}", SBGUIElement.InstanceID));
		SBGUIScreen sbguiscreen = gameObject.AddComponent<SBGUIScreen>();
		sbguiscreen.Initialize(parent, session);
		return sbguiscreen;
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00025388 File Offset: 0x00023588
	public virtual void Close()
	{
		this.UsedInSessionAction = false;
		this.Deactivate();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x000253A4 File Offset: 0x000235A4
	public virtual void Deactivate()
	{
		GUIMainView.GetInstance().Library.bShowingDialog = false;
		this.MuteButtons(false);
		if (base.gameObject.active)
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x000253E4 File Offset: 0x000235E4
	private void Initialize(SBGUIElement parent, Session session)
	{
		this.session = session;
		this.dynamicLabels = new Dictionary<string, SBGUILabel>();
		this.dynamicMeters = new Dictionary<string, SBGUIProgressMeter>();
		this.dynamicProperties = new Dictionary<string, object>();
		base.SetTransformParent(parent);
		base.transform.localPosition = Vector3.zero;
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00025430 File Offset: 0x00023630
	public virtual void Update()
	{
		this.UpdateCallback.FireEvent(this, this.session);
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00025444 File Offset: 0x00023644
	// (set) Token: 0x060005D6 RID: 1494 RVA: 0x0002544C File Offset: 0x0002364C
	public virtual bool UsedInSessionAction
	{
		get
		{
			return this.usedInSessionAction;
		}
		set
		{
			this.usedInSessionAction = value;
		}
	}

	// Token: 0x04000469 RID: 1129
	public Dictionary<string, SBGUILabel> dynamicLabels;

	// Token: 0x0400046A RID: 1130
	public Dictionary<string, SBGUIProgressMeter> dynamicMeters;

	// Token: 0x0400046B RID: 1131
	public Dictionary<string, object> dynamicProperties;

	// Token: 0x0400046C RID: 1132
	public EventDispatcher<SBGUIScreen, Session> UpdateCallback = new EventDispatcher<SBGUIScreen, Session>();

	// Token: 0x0400046D RID: 1133
	public EventDispatcher OnPutIntoCache = new EventDispatcher();

	// Token: 0x0400046E RID: 1134
	public Session session;

	// Token: 0x0400046F RID: 1135
	protected List<SBGUIScreen> modalDialogs = new List<SBGUIScreen>();

	// Token: 0x04000470 RID: 1136
	private bool usedInSessionAction;
}
