using System;
using UnityEngine;

// Token: 0x02000222 RID: 546
public abstract class ClickableUiPointer : VisualSpawn
{
	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0007D748 File Offset: 0x0007B948
	public SBGUIElement Element
	{
		get
		{
			return this.element;
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060011F1 RID: 4593 RVA: 0x0007D750 File Offset: 0x0007B950
	protected SBGUIElement Parent
	{
		get
		{
			return this.parentElement;
		}
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x0007D758 File Offset: 0x0007B958
	protected virtual void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen, string pointerPrefab)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale);
		this.uiMixin.OnRegisterNewInstance(action, containingScreen);
		this.parentElement = elementTarget;
		this.element = SBGUI.InstantiatePrefab(pointerPrefab);
		this.element.SetParent(this.parentElement);
		this.element.gameObject.transform.localPosition = Vector3.zero;
		this.element.gameObject.transform.localScale = scale;
		this.element.Alpha = base.Alpha;
		if (!base.ParentAction.ManualSuccess && base.ParentAction.Definition.UsingDefaultSucceedConditions)
		{
			SBGUIButton[] componentsInChildren = this.parentElement.GetComponentsInChildren<SBGUIButton>(true);
			SBGUIButton button;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				button = componentsInChildren[i];
				Action succeedOnClick = null;
				succeedOnClick = delegate()
				{
					if (this != null && this.ParentAction != null && this.ParentAction.Status != SessionActionTracker.StatusCode.FINISHED_FAILURE && this.ParentAction.Status != SessionActionTracker.StatusCode.OBLITERATED)
					{
						this.ParentAction.MarkSucceeded(false);
					}
					if (this != null && button != null)
					{
						button.ClickEvent -= succeedOnClick;
					}
				};
				button.ClickEvent += succeedOnClick;
			}
		}
		Bounds totalBounds = this.parentElement.TotalBounds;
		float widthOver = totalBounds.size.x * 0.5f * 0.01f;
		float heightOver = totalBounds.size.y * 0.5f * 0.01f;
		base.NormalizeRotationAndPushToEdge(widthOver, heightOver);
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x0007D8E8 File Offset: 0x0007BAE8
	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (base.ParentAction.Status == SessionActionTracker.StatusCode.STARTED && (!this.ElementIsInGoodState(this.parentElement) || !this.ElementIsInGoodState(this.element)))
		{
			base.ParentAction.MarkFailed();
		}
		return base.OnUpdate(game);
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x0007D93C File Offset: 0x0007BB3C
	public bool ElementIsInGoodState(SBGUIElement element)
	{
		return element != null && element.gameObject != null && element.IsActive() && element.gameObject.renderer != null && element.gameObject.renderer.enabled;
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x0007D9A0 File Offset: 0x0007BBA0
	public override void Destroy()
	{
		this.uiMixin.Destroy();
		if (this.element != null && this.element.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.element.gameObject);
		}
	}

	// Token: 0x04000C43 RID: 3139
	private UiSpawnMixin uiMixin = new UiSpawnMixin();

	// Token: 0x04000C44 RID: 3140
	private SBGUIElement element;

	// Token: 0x04000C45 RID: 3141
	private SBGUIElement parentElement;
}
