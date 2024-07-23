using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
[RequireComponent(typeof(TapButton))]
[RequireComponent(typeof(YG2DRectangle))]
public class SBGUIButton : SBGUIImage
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060003CD RID: 973 RVA: 0x00014CC8 File Offset: 0x00012EC8
	// (remove) Token: 0x060003CE RID: 974 RVA: 0x00014D00 File Offset: 0x00012F00
	public event Action ClickEvent
	{
		add
		{
			TFUtils.Assert(this.button != null, "Trying to add or remove click events from a null button!");
			this.button.TapEvent.AddListener(value);
			this.AddQuestConditionToButton();
			this.AddAnalyticsToButton();
		}
		remove
		{
			TFUtils.Assert(this.button != null, "Trying to add or remove click events from a null button!");
			this.button.TapEvent.RemoveListener(value);
		}
	}

	// Token: 0x060003CF RID: 975 RVA: 0x00014D2C File Offset: 0x00012F2C
	public virtual void MockClick()
	{
		this.button.TapEvent.FireEvent();
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x00014D40 File Offset: 0x00012F40
	public Vector2 ResetSize()
	{
		return base.sprite.ResetSize();
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00014D50 File Offset: 0x00012F50
	public void ClearClickEvents()
	{
		if (!(this.button == null))
		{
			this.button.TapEvent.ClearListeners();
			this.RemoveQuestConditionFromButton();
			this.RemoveAnalyticsFromButton();
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x060003D2 RID: 978 RVA: 0x00014D90 File Offset: 0x00012F90
	// (set) Token: 0x060003D3 RID: 979 RVA: 0x00014D98 File Offset: 0x00012F98
	public override Vector3 WorldPosition
	{
		get
		{
			return base.WorldPosition;
		}
		set
		{
			if (base.tform.position == value)
			{
				return;
			}
			base.tform.position = value;
			YGSprite.MeshUpdateHierarchy(base.gameObject);
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x060003D5 RID: 981 RVA: 0x00014E00 File Offset: 0x00013000
	// (set) Token: 0x060003D4 RID: 980 RVA: 0x00014DD4 File Offset: 0x00012FD4
	public new bool enabled
	{
		get
		{
			return this.collisions;
		}
		set
		{
			this.collisions = value;
			this.body.enabled = (this.collisions && !this.muted);
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060003D7 RID: 983 RVA: 0x00014E4C File Offset: 0x0001304C
	// (set) Token: 0x060003D6 RID: 982 RVA: 0x00014E08 File Offset: 0x00013008
	protected override bool Muted
	{
		get
		{
			return this.muted;
		}
		set
		{
			if (!this.unmutable)
			{
				this.muted = value;
				this.body.enabled = (this.collisions && !this.muted);
			}
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00014E54 File Offset: 0x00013054
	protected override void Awake()
	{
		this.button = base.gameObject.GetComponent<TapButton>();
		this.body = base.gameObject.GetComponent<YG2DBody>();
		base.Awake();
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00014E8C File Offset: 0x0001308C
	private void AddQuestConditionToButton()
	{
		if (this.QuestConditionAction != null || this.button == null || base.gameObject == null)
		{
			return;
		}
		Session pSession = null;
		Transform transform = base.gameObject.transform;
		while (transform != null)
		{
			SBGUIScreen component = transform.gameObject.GetComponent<SBGUIScreen>();
			if (component != null)
			{
				pSession = component.session;
				break;
			}
			transform = transform.parent;
		}
		if (pSession != null && pSession.TheGame != null && pSession.TheGame.simulation != null && base.gameObject != null)
		{
			this.QuestConditionAction = delegate()
			{
				pSession.TheGame.simulation.ModifyGameState(new ButtonTapAction(this.gameObject.name));
			};
			this.ClickEvent += this.QuestConditionAction;
		}
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00014F8C File Offset: 0x0001318C
	private void RemoveQuestConditionFromButton()
	{
		if (this.QuestConditionAction != null)
		{
			this.ClickEvent -= this.QuestConditionAction;
			this.QuestConditionAction = null;
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00014FAC File Offset: 0x000131AC
	private void AddAnalyticsToButton()
	{
		if (string.IsNullOrEmpty(this.analyticsTag) || this.AnalyticsAction != null || this.button == null || base.gameObject == null)
		{
			return;
		}
		Session pSession = null;
		Transform transform = base.gameObject.transform;
		while (transform != null)
		{
			SBGUIScreen component = transform.gameObject.GetComponent<SBGUIScreen>();
			if (component != null)
			{
				pSession = component.session;
				break;
			}
			transform = transform.parent;
		}
		if (pSession != null && pSession.TheGame != null && pSession.TheGame.simulation != null && base.gameObject != null)
		{
			this.AnalyticsAction = delegate()
			{
				AnalyticsWrapper.LogUIInteraction(pSession.TheGame, this.analyticsTag, "button", "tap");
			};
			this.ClickEvent += this.AnalyticsAction;
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x000150BC File Offset: 0x000132BC
	private void RemoveAnalyticsFromButton()
	{
		if (this.AnalyticsAction != null)
		{
			this.ClickEvent -= this.AnalyticsAction;
			this.AnalyticsAction = null;
		}
	}

	// Token: 0x04000284 RID: 644
	protected YG2DBody body;

	// Token: 0x04000285 RID: 645
	protected TapButton button;

	// Token: 0x04000286 RID: 646
	protected bool collisions = true;

	// Token: 0x04000287 RID: 647
	public bool unmutable;

	// Token: 0x04000288 RID: 648
	public string analyticsTag;

	// Token: 0x04000289 RID: 649
	private Action QuestConditionAction;

	// Token: 0x0400028A RID: 650
	private Action AnalyticsAction;
}
