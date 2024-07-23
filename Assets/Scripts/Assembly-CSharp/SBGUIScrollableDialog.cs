using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public abstract class SBGUIScrollableDialog : SBGUIScreen
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060005F8 RID: 1528 RVA: 0x00026058 File Offset: 0x00024258
	// (remove) Token: 0x060005F9 RID: 1529 RVA: 0x00026094 File Offset: 0x00024294
	public event Action ReadyEvent
	{
		add
		{
			if (this.region == null)
			{
				TFUtils.Assert(false, "Screen's region isn't defined");
				return;
			}
			this.region.ReadyEvent.AddListener(value);
		}
		remove
		{
			if (this.region == null)
			{
				TFUtils.Assert(false, "Screen's region isn't defined");
				return;
			}
			this.region.ReadyEvent.RemoveListener(value);
		}
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x000260D0 File Offset: 0x000242D0
	public virtual void Start()
	{
		GUIMainView.GetInstance().Library.bShowingDialog = true;
		this.viewBounds = GUIMainView.GetInstance().ViewBounds();
		this.windowSprite = (SBGUIImage)base.FindChild("window");
		if (this.windowSprite == null)
		{
			this.windowSprite = (SBGUIAtlasImage)base.FindChild("window");
		}
		this.windowHeight = this.windowSprite.Size.y * 0.01f;
		this.windowPosition = this.windowSprite.WorldPosition;
		base.EnableButtons(false);
		base.StartCoroutine(this.AnimateIn(0.5f, delegate
		{
			base.EnableButtons(true);
			if (this.region != null)
			{
				this.region.MatchAndRegister();
			}
		}));
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x00026190 File Offset: 0x00024390
	private IEnumerator AnimateIn(float duration, Action completeAction)
	{
		float interp = 0f;
		Vector3 origin = this.windowPosition;
		origin.y = this.viewBounds.min.y - this.windowHeight;
		this.windowSprite.WorldPosition = origin;
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			Vector3 interpPos = Easing.Vector3Easing(origin, this.windowPosition, interp, new Func<float, float, float, float>(Easing.EaseOutBack));
			this.windowSprite.WorldPosition = interpPos;
			yield return null;
		}
		this.windowSprite.WorldPosition = this.windowPosition;
		if (completeAction != null)
		{
			completeAction();
		}
		yield break;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000261C8 File Offset: 0x000243C8
	public virtual void ShowScrollRegion(bool visible)
	{
		this.region.SetActive(visible);
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x000261D8 File Offset: 0x000243D8
	public void SetManagers(EntityManager emgr, ResourceManager resMgr, SoundEffectManager sfxMgr, CostumeManager cosMgr)
	{
		this.entityMgr = emgr;
		this.resourceMgr = resMgr;
		this.soundEffectMgr = sfxMgr;
		this.costumeMgr = cosMgr;
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x000261F8 File Offset: 0x000243F8
	public override void MuteButtons(bool mute)
	{
		base.MuteButtons(mute);
		if (this.region != null)
		{
			SBGUIElement componentInChildren = this.region.GetComponent<ScrollRegion>().subView.GetComponentInChildren<SBGUIElement>();
			if (componentInChildren != null)
			{
				componentInChildren.MuteButtons(mute);
			}
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000600 RID: 1536 RVA: 0x0002627C File Offset: 0x0002447C
	// (set) Token: 0x060005FF RID: 1535 RVA: 0x00026248 File Offset: 0x00024448
	protected override bool Muted
	{
		get
		{
			return base.Muted;
		}
		set
		{
			base.Muted = value;
			if (this.region != null)
			{
				this.region.MuteButtons(value);
			}
		}
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x00026284 File Offset: 0x00024484
	protected virtual void Setup()
	{
		if (this.region != null)
		{
			this.region.ResetScroll();
			this.region.ResetToMinScroll();
		}
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x000262B0 File Offset: 0x000244B0
	public override void Deactivate()
	{
		if (this.region != null)
		{
			this.region.ReadyEvent.ClearListeners();
		}
		GUIMainView.GetInstance().Library.bShowingDialog = false;
		base.Deactivate();
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x000262EC File Offset: 0x000244EC
	public override void OnDestroy()
	{
		if (this.region != null)
		{
			YGAtlasSprite[] componentsInChildren = this.region.GetComponentsInChildren<YGAtlasSprite>();
			foreach (YGAtlasSprite ygatlasSprite in componentsInChildren)
			{
				if (!string.IsNullOrEmpty(ygatlasSprite.nonAtlasName))
				{
					base.View.Library.incrementTextureDuplicates(ygatlasSprite.nonAtlasName);
				}
			}
			UnityEngine.Object.Destroy(this.region);
		}
		GUIMainView.GetInstance().Library.bShowingDialog = false;
		base.OnDestroy();
	}

	// Token: 0x04000494 RID: 1172
	public SBGUIScrollRegion region;

	// Token: 0x04000495 RID: 1173
	protected EntityManager entityMgr;

	// Token: 0x04000496 RID: 1174
	protected ResourceManager resourceMgr;

	// Token: 0x04000497 RID: 1175
	protected CostumeManager costumeMgr;

	// Token: 0x04000498 RID: 1176
	protected SoundEffectManager soundEffectMgr;

	// Token: 0x04000499 RID: 1177
	private Bounds viewBounds;

	// Token: 0x0400049A RID: 1178
	private Vector3 windowPosition;

	// Token: 0x0400049B RID: 1179
	private float windowHeight;

	// Token: 0x0400049C RID: 1180
	private SBGUIImage windowSprite;
}
