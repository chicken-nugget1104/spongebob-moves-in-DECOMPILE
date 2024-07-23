using System;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class TutorialHandDragGuide : ClickableUiPointer
{
	// Token: 0x06001349 RID: 4937 RVA: 0x00084FFC File Offset: 0x000831FC
	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement elementTarget, SBGUIScreen containingScreen, Simulated simulatedTarget, string iconTexture, float duration)
	{
		TutorialHandDragGuide tutorialHandDragGuide = new TutorialHandDragGuide();
		tutorialHandDragGuide.Initialize(game, parentAction, this.offset, base.Rotation, base.Alpha, base.Scale, elementTarget, containingScreen, simulatedTarget, iconTexture, duration);
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x00085038 File Offset: 0x00083238
	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen, Simulated simulatedTarget, string iconTexture, float duration)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale, elementTarget, containingScreen, "Prefabs/GUI/Widgets/TutorialHandGuide");
		this.period = duration * 2f;
		this.sinusoid = new Sinusoid(0f, 1f, this.period, 0f);
		this.simulatedTarget = simulatedTarget;
		this.subHandTransform = base.Element.FindChild("hand");
		this.subIcon = base.Element.FindChild("icon").GetComponent<SBGUIPulseImage>();
		this.subIcon.InitializePulser(this.subIcon.Size, 2f, 0.2f);
		this.subIcon.SetTextureFromAtlas(iconTexture);
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x000850F4 File Offset: 0x000832F4
	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (base.Element != null && base.Element.gameObject != null)
		{
			Vector3 from = TFUtils.ExpandVector(base.Parent.GetScreenPosition()) + this.offset;
			Vector3 vector = game.simulation.TheCamera.WorldToScreenPoint(this.simulatedTarget.DisplayController.Position);
			Vector3 to = new Vector3(vector.x, SBGUI.GetScreenHeight() - vector.y, 0f) + this.offset;
			Vector3 v = Vector3.Lerp(from, to, this.sinusoid.ValueAtTime(this.timeAccumulated));
			base.Element.SetScreenPosition(v);
			if (this.timeAccumulated > this.period / 2f)
			{
				base.Element.transform.localScale = Vector3.zero;
			}
			else
			{
				base.Element.transform.localScale = new Vector3(2f, 2f, 2f);
			}
			if (this.timeAccumulated == 0f)
			{
				this.subIcon.Pulser.PulseOneShot();
			}
			float num = this.timeAccumulated / this.period * 80f;
			this.subHandTransform.transform.localRotation = Quaternion.Euler(0f, 0f, num);
			this.subIcon.transform.localRotation = Quaternion.Euler(0f, 0f, -num);
			this.timeAccumulated += Time.deltaTime;
			if (this.timeAccumulated > this.period)
			{
				this.timeAccumulated = 0f;
			}
		}
		return base.OnUpdate(game);
	}

	// Token: 0x04000D5B RID: 3419
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/TutorialHandGuide";

	// Token: 0x04000D5C RID: 3420
	private Sinusoid sinusoid;

	// Token: 0x04000D5D RID: 3421
	private Simulated simulatedTarget;

	// Token: 0x04000D5E RID: 3422
	private SBGUIElement subHandTransform;

	// Token: 0x04000D5F RID: 3423
	private SBGUIPulseImage subIcon;

	// Token: 0x04000D60 RID: 3424
	private float timeAccumulated;

	// Token: 0x04000D61 RID: 3425
	private float period;
}
