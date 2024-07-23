using System;
using UnityEngine;

// Token: 0x02000258 RID: 600
public class TextPromptSpawn : SessionActionSpawn
{
	// Token: 0x06001343 RID: 4931 RVA: 0x00084E0C File Offset: 0x0008300C
	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIScreen parentScreen, string text, TextPrompt.Anchor anchor)
	{
		TextPromptSpawn textPromptSpawn = new TextPromptSpawn();
		textPromptSpawn.RegisterNewInstance(game, parentAction, parentScreen, text, anchor);
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x00084E2C File Offset: 0x0008302C
	protected void RegisterNewInstance(Game game, SessionActionTracker parentAction, SBGUIScreen parentScreen, string text, TextPrompt.Anchor anchor)
	{
		base.RegisterNewInstance(game, parentAction);
		this.uiMixin.OnRegisterNewInstance(parentAction, parentScreen);
		this.prompt = this.GetPrefab();
		this.prompt.SetParent(parentScreen);
		this.prompt.transform.localPosition = Vector3.zero;
		this.prompt.SetLabel(text);
		this.anchorTarget = anchor;
		this.prompt.SetAnchoredPosition(anchor);
		if (!parentAction.ManualSuccess)
		{
			this.prompt.SetClickCallback(delegate
			{
				parentAction.MarkSucceeded(false);
			});
		}
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x00084EDC File Offset: 0x000830DC
	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (this.prompt != null)
		{
			this.prompt.SetAnchoredPosition(this.anchorTarget);
		}
		return base.OnUpdate(game);
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x00084F08 File Offset: 0x00083108
	public override void Destroy()
	{
		this.uiMixin.Destroy();
		if (this.prompt != null)
		{
			this.prompt.MuteButtons(false);
			this.prompt.SetParent(null);
			this.prompt.SetActive(false);
		}
		this.instanceCount--;
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x00084F64 File Offset: 0x00083164
	private SessionActionTextPromptPrefab GetPrefab()
	{
		TFUtils.Assert(this.instanceCount < 1, "Text Prompts only support 1 instance at a time!");
		if (TextPromptSpawn.cachedPrefab == null && this.instanceCount < 1)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Prefabs/GUI/Widgets/TextPrompt"));
			TextPromptSpawn.cachedPrefab = gameObject.GetComponent<SessionActionTextPromptPrefab>();
		}
		else
		{
			TextPromptSpawn.cachedPrefab.MuteButtons(false);
		}
		TextPromptSpawn.cachedPrefab.gameObject.SetActiveRecursively(true);
		this.instanceCount++;
		return TextPromptSpawn.cachedPrefab;
	}

	// Token: 0x04000D56 RID: 3414
	private UiSpawnMixin uiMixin = new UiSpawnMixin();

	// Token: 0x04000D57 RID: 3415
	private static SessionActionTextPromptPrefab cachedPrefab;

	// Token: 0x04000D58 RID: 3416
	private SessionActionTextPromptPrefab prompt;

	// Token: 0x04000D59 RID: 3417
	private int instanceCount;

	// Token: 0x04000D5A RID: 3418
	private TextPrompt.Anchor anchorTarget;
}
