using System;
using UnityEngine;

// Token: 0x0200022F RID: 559
public class GuideArrow : ClickableUiPointer
{
	// Token: 0x06001238 RID: 4664 RVA: 0x0007EBEC File Offset: 0x0007CDEC
	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement elementTarget, SBGUIScreen containingScreen)
	{
		GuideArrow guideArrow = new GuideArrow();
		guideArrow.Initialize(game, parentAction, this.offset + new Vector3(0f, 0f, -0.4f), base.Rotation, base.Alpha, base.Scale, elementTarget, containingScreen);
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x0007EC3C File Offset: 0x0007CE3C
	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale, elementTarget, containingScreen, "Prefabs/GUI/Widgets/TutorialPointer");
		this.bouncer = new JumpPattern(-7f, 0.55f, 0.18f, 0.3f, 0f, Time.time, base.Element.transform.localScale);
		Material sharedMaterial = base.Element.renderer.sharedMaterial;
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
		{
			base.Element.renderer.material = (Resources.Load("Materials/lod/TutorialPointer_lr") as Material);
		}
		else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			base.Element.renderer.material = (Resources.Load("Materials/lod/TutorialPointer_lr2") as Material);
		}
		else
		{
			base.Element.renderer.material = (Resources.Load("Materials/lod/TutorialPointer") as Material);
		}
		if (sharedMaterial != null)
		{
			Resources.UnloadAsset(sharedMaterial);
		}
		Transform parent = elementTarget.transform.parent;
		SBGUIElement component = parent.GetComponent<SBGUIElement>();
		if (component != null)
		{
			component.EnableRejectButton(false);
		}
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0007ED68 File Offset: 0x0007CF68
	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		float num;
		Vector2 vector;
		this.bouncer.ValueAndSquishAtTime(Time.time, out num, out vector);
		if (base.Element != null && base.Element.gameObject != null)
		{
			Vector3 b = new Vector3(base.Direction.x * num, base.Direction.y * num, 0f);
			base.Element.gameObject.transform.localPosition = this.offset + b;
			base.Element.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, -base.Rotation);
			base.Element.gameObject.transform.localScale = TFUtils.ExpandVector(vector);
		}
		return base.OnUpdate(game);
	}

	// Token: 0x04000C84 RID: 3204
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/TutorialPointer";

	// Token: 0x04000C85 RID: 3205
	private JumpPattern bouncer;
}
