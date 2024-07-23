using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class SBGUICharacterDialog : SBGUIModalDialog
{
	// Token: 0x060003EC RID: 1004 RVA: 0x00015EB8 File Offset: 0x000140B8
	protected override void Awake()
	{
		this.characterIcon = (SBGUIAtlasImage)base.FindChild("character_icon");
		this.dialogText = (SBGUILabel)base.FindChild("dialog_label");
		this.skipButton = (SBGUIButton)base.FindChild("skip_button");
		this.speechBubble = (SBGUIAtlasImage)base.FindChild("speech_bubble");
		this.dialogBoundary = (SBGUIAtlasImage)base.FindChild("dialog_area_boundary");
		this.speechBubble.SetAlpha(0f);
		this.skipButton.SetAlpha(0f);
		this.dialogText.SetAlpha(0f);
		this.m_pPortraitSize = this.characterIcon.Size;
		this.m_pSkipAction = delegate()
		{
			this.skipButton.MockClick();
		};
		base.Awake();
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00015F94 File Offset: 0x00014194
	private void Start()
	{
		this.AttachAnalyticsToButton("skip", this.skipButton);
		this.skipButton.ClickEvent += delegate()
		{
			if (this.currentlyTyping)
			{
				this.StopTyping();
				this.dialogText.SetText(this.localizedPrompt);
				AndroidBack.getInstance().pop(this.m_pSkipAction);
				AndroidBack.getInstance().push(this.m_pSkipAction, this);
			}
			else
			{
				this.ShowDialog(this.dialogIndex + 1);
			}
		};
		if (this.autoPlay && this.prompts.Count > 0)
		{
			this.StartSequence();
		}
		this.characterPosition = this.characterIcon.tform.localPosition;
		this.viewBounds = GUIMainView.GetInstance().ViewBounds();
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00016014 File Offset: 0x00014214
	private void StartSequence()
	{
		if (this.dialogIndex < 0)
		{
			this.dialogIndex = 0;
		}
		this.ShowDialog(0);
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x00016030 File Offset: 0x00014230
	public void LoadSequence(List<object> sequence)
	{
		this.prompts.Clear();
		foreach (object obj in sequence)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			this.prompts.Add(new DialogPrompt(dict));
		}
		this.StartSequence();
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x000160B4 File Offset: 0x000142B4
	public void ShowDialog(int index)
	{
		if (index >= this.prompts.Count || index < 0)
		{
			base.StartCoroutine(this.AnimateOut(0.2f, delegate
			{
				this.SetActive(false);
				this.dialogIndex = 0;
				this.DialogChange.FireEvent(-1);
			}));
			return;
		}
		AndroidBack.getInstance().pop(this.m_pSkipAction);
		AndroidBack.getInstance().push(this.m_pSkipAction, this);
		if (!base.IsActive())
		{
			this.SetActive(true);
		}
		this.dialogIndex = index;
		Action transitionComplete = delegate()
		{
			this.localizedPrompt = Language.Get(this.currentPrompt.text);
			this.AdjustText();
			if (!string.IsNullOrEmpty(this.currentPrompt.voiceover) && this.session != null)
			{
				this.session.TheSoundEffectManager.PlaySound(this.currentPrompt.voiceover);
			}
			this.DialogChange.FireEvent(index);
			this.StartCoroutine("TypeText");
		};
		Action action = delegate()
		{
			this.currentPrompt = this.prompts[index];
			this.StartCoroutine(this.AnimateIn(0.25f, transitionComplete));
		};
		if (this.currentPrompt == null)
		{
			action();
		}
		else if (this.prompts[index].texture != this.currentPrompt.texture)
		{
			base.StartCoroutine(this.AnimateOut(0.2f, action));
		}
		else
		{
			this.currentPrompt = this.prompts[index];
			this.characterIcon.SetSizeNoRebuild(this.m_pPortraitSize);
			this.characterIcon.SetTextureFromAtlas(this.currentPrompt.texture, true, false, true, false, false, 0);
			transitionComplete();
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x00016224 File Offset: 0x00014424
	private IEnumerator AnimateOut(float duration, Action completeAction)
	{
		float interp = 0f;
		this.skipButton.SetActive(false);
		float width = this.characterIcon.Size.x * 0.01f;
		Vector3 dest = this.characterPosition;
		dest.x = this.viewBounds.min.x - width;
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			this.dialogText.SetAlpha(Mathf.Lerp(0.5f, 0f, interp));
			this.speechBubble.SetAlpha(Mathf.Lerp(0.5f, 0f, interp));
			Vector3 interpPos = Vector3.Lerp(this.characterPosition, dest, interp);
			this.characterIcon.tform.localPosition = interpPos;
			yield return null;
		}
		this.characterIcon.tform.localPosition = dest;
		this.dialogText.SetAlpha(0f);
		this.speechBubble.SetAlpha(0f);
		if (completeAction != null)
		{
			completeAction();
		}
		yield break;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001625C File Offset: 0x0001445C
	private IEnumerator AnimateIn(float duration, Action completeAction)
	{
		this.characterIcon.SetActive(false);
		yield return null;
		float interp = 0f;
		this.dialogText.SetText(string.Empty);
		float width = this.characterIcon.Size.x * 0.01f;
		Vector3 origin = this.characterPosition;
		origin.x = this.viewBounds.min.x - width;
		this.characterIcon.tform.localPosition = origin;
		this.characterIcon.SetActive(true);
		this.speechBubble.SetActive(true);
		this.characterIcon.SetSizeNoRebuild(this.m_pPortraitSize);
		this.characterIcon.SetTextureFromAtlas(this.currentPrompt.texture, true, false, true, true, false, 0);
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			Vector3 interpPos = Vector3.Lerp(origin, this.characterPosition, interp);
			this.speechBubble.SetAlpha(Mathf.Lerp(0f, 0.5f, interp));
			this.characterIcon.tform.localPosition = interpPos;
			yield return null;
		}
		this.characterIcon.tform.localPosition = this.characterPosition;
		this.skipButton.SetAlpha(0.5f);
		this.dialogText.SetAlpha(0.5f);
		if (completeAction != null)
		{
			completeAction();
		}
		yield break;
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00016294 File Offset: 0x00014494
	private void AdjustText()
	{
		this.dialogText.SetText(string.Empty);
		int i = 0;
		while (i < this.localizedPrompt.Length)
		{
			SBGUILabel sbguilabel = this.dialogText;
			sbguilabel.Text += this.localizedPrompt[i++];
			if ((float)this.dialogText.Width > this.dialogBoundary.Size.x)
			{
				string text = this.dialogText.Text;
				for (int j = i - 1; j >= 0; j--)
				{
					if (text[j] == ' ')
					{
						this.localizedPrompt = this.localizedPrompt.Remove(j, 1);
						this.localizedPrompt = this.localizedPrompt.Insert(j, "|");
						this.dialogText.SetText(this.localizedPrompt.Substring(0, i));
						break;
					}
				}
			}
		}
		this.dialogText.SetText(string.Empty);
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x000163A0 File Offset: 0x000145A0
	private IEnumerator TypeText()
	{
		lock (this)
		{
			if (this.currentlyTyping)
			{
				this.StopTyping();
			}
		}
		this.currentlyTyping = true;
		this.dialogText.SetText(string.Empty);
		yield return null;
		int i = 0;
		while (i < this.localizedPrompt.Length)
		{
			SBGUILabel sbguilabel = this.dialogText;
			object text = sbguilabel.Text;
			string text2 = this.localizedPrompt;
			int index;
			i = (index = i) + 1;
			sbguilabel.Text = text + text2[index];
			while (i < this.localizedPrompt.Length && (this.localizedPrompt[i] == ' ' || this.localizedPrompt[i] == '|'))
			{
				SBGUILabel sbguilabel2 = this.dialogText;
				object text3 = sbguilabel2.Text;
				string text4 = this.localizedPrompt;
				i = (index = i) + 1;
				sbguilabel2.Text = text3 + text4[index];
			}
			yield return new WaitForSeconds(0.025f);
		}
		this.currentlyTyping = false;
		yield break;
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x000163BC File Offset: 0x000145BC
	private void StopTyping()
	{
		base.StopCoroutine("TypeText");
		this.currentlyTyping = false;
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x000163D0 File Offset: 0x000145D0
	protected override void OnDisable()
	{
		base.StopAllCoroutines();
		base.OnDisable();
	}

	// Token: 0x040002A9 RID: 681
	private const float DELAY_BETWEEN_LETTERS = 0.025f;

	// Token: 0x040002AA RID: 682
	public List<DialogPrompt> prompts = new List<DialogPrompt>();

	// Token: 0x040002AB RID: 683
	private SBGUIAtlasImage characterIcon;

	// Token: 0x040002AC RID: 684
	private SBGUILabel dialogText;

	// Token: 0x040002AD RID: 685
	private SBGUIButton skipButton;

	// Token: 0x040002AE RID: 686
	private SBGUIAtlasImage speechBubble;

	// Token: 0x040002AF RID: 687
	private SBGUIAtlasImage dialogBoundary;

	// Token: 0x040002B0 RID: 688
	private int dialogIndex = -1;

	// Token: 0x040002B1 RID: 689
	private DialogPrompt currentPrompt;

	// Token: 0x040002B2 RID: 690
	private bool currentlyTyping;

	// Token: 0x040002B3 RID: 691
	private string localizedPrompt;

	// Token: 0x040002B4 RID: 692
	private Action m_pSkipAction;

	// Token: 0x040002B5 RID: 693
	public EventDispatcher<int> DialogChange = new EventDispatcher<int>();

	// Token: 0x040002B6 RID: 694
	public bool autoPlay;

	// Token: 0x040002B7 RID: 695
	private Vector3 m_pPortraitSize;

	// Token: 0x040002B8 RID: 696
	private Vector3 characterPosition;

	// Token: 0x040002B9 RID: 697
	private Bounds viewBounds;

	// Token: 0x02000491 RID: 1169
	// (Invoke) Token: 0x060024A7 RID: 9383
	public delegate float EasingFunc(float start, float end, float duration);
}
