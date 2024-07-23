using System;
using UnityEngine;

// Token: 0x0200023E RID: 574
public class QuestReminderBanner : ClickableUiPointer
{
	// Token: 0x06001284 RID: 4740 RVA: 0x0007FF70 File Offset: 0x0007E170
	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement parentElement, SBGUIScreen containingScreen, Action clickHandler, string barTexture, string circleTexture)
	{
		QuestReminderBanner questReminderBanner = new QuestReminderBanner();
		questReminderBanner.Initialize(game, parentAction, parentElement, containingScreen, clickHandler, this.offset + new Vector3(0f, 0f, 1f), base.Rotation, base.Alpha, base.Scale, barTexture, circleTexture);
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x0007FFC8 File Offset: 0x0007E1C8
	protected void Initialize(Game game, SessionActionTracker action, SBGUIElement parentElement, SBGUIScreen containingScreen, Action clickHandler, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, string barTexture, string circleTexture)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale, parentElement, containingScreen, "Prefabs/GUI/Widgets/QuestReminder_Banner");
		if (barTexture != null)
		{
			SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)base.Element.FindChild("QuestReminder_Bar");
			sbguipulseButton.SetTextureFromAtlas(barTexture);
		}
		if (circleTexture != null)
		{
			SBGUIPulseImage sbguipulseImage = (SBGUIPulseImage)base.Element;
			sbguipulseImage.SetTextureFromAtlas(circleTexture);
		}
		this.bannerSubElement = base.Element.FindChild("QuestReminder_Bar").gameObject.GetComponent<SBGUIPulseButton>();
		SBGUIPulseButton component = this.bannerSubElement.gameObject.GetComponent<SBGUIPulseButton>();
		component.ClickEvent += clickHandler;
		TFUtils.Assert(this.bannerSubElement != null, "Could not find child Quest Reminder Bar on prefab!");
		base.Element.gameObject.transform.localPosition = new Vector3(0f, 0f, 0.05f);
		SBGUIPulseImage component2 = base.Element.gameObject.GetComponent<SBGUIPulseImage>();
		SBGUIAtlasImage component3 = base.Element.gameObject.GetComponent<SBGUIAtlasImage>();
		component2.InitializePulser(component3.Size, 1.5f, 0.25f);
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			component2.Pulser.PulseOneShot();
		}
		this.periodicSquisher = new JumpPattern(-1f, 2f, 0.5f, 0.15f, 0f, Time.time, Vector2.one);
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x00080124 File Offset: 0x0007E324
	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (this.periodicSquisher != null)
		{
			float num;
			Vector2 v;
			this.periodicSquisher.ValueAndSquishAtTime(Time.time, out num, out v);
			if (!TFPerfUtils.IsNonScalingDevice())
			{
				this.bannerSubElement.gameObject.transform.localScale = v;
			}
		}
		if (this.parentAction.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || this.parentAction.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE || this.parentAction.Status == SessionActionTracker.StatusCode.OBLITERATED)
		{
			this.Destroy();
			return SessionActionManager.SpawnReturnCode.KILL;
		}
		return SessionActionManager.SpawnReturnCode.KEEP_ALIVE;
	}

	// Token: 0x04000CB8 RID: 3256
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/QuestReminder_Banner";

	// Token: 0x04000CB9 RID: 3257
	private SBGUIPulseButton bannerSubElement;

	// Token: 0x04000CBA RID: 3258
	private JumpPattern periodicSquisher;
}
