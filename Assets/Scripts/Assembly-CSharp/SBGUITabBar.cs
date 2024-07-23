using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class SBGUITabBar : SBGUIElement
{
	// Token: 0x0600066E RID: 1646 RVA: 0x00029B78 File Offset: 0x00027D78
	public void SetupCategories(Dictionary<string, SBTabCategory> categories, Session session)
	{
		int num = 0;
		if (this.buttons != null)
		{
			int num2 = this.buttons.Length;
			for (int i = 0; i < num2; i++)
			{
				UnityEngine.Object.Destroy(this.buttons[i].gameObject);
			}
			this.buttons = null;
		}
		if (this.buttons == null)
		{
			this.buttons = new SBGUITabButton[categories.Count];
			foreach (SBTabCategory sbtabCategory in categories.Values)
			{
				if (sbtabCategory.MicroEventDID >= 0)
				{
					MicroEvent microEvent = session.TheGame.microEventManager.GetMicroEvent(sbtabCategory.MicroEventDID);
					if (microEvent == null || !microEvent.IsActive() || (sbtabCategory.MicroEventOnly && microEvent.IsCompleted()))
					{
						continue;
					}
				}
				this.buttons[num] = SBGUITabButton.CreateTabButton(this, sbtabCategory, num);
				num++;
			}
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00029CA0 File Offset: 0x00027EA0
	protected override void OnEnable()
	{
		YGTextureLibrary library = base.View.Library;
		this.foundOnMat = library.FindSpriteMaterial(this.onTexture);
		this.foundOffMat = library.FindSpriteMaterial(this.offTexture);
		base.OnEnable();
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00029CE4 File Offset: 0x00027EE4
	private void Start()
	{
		if (this.scrollRegion != null)
		{
			this.scrollRegion.ReadyEvent.AddListener(delegate()
			{
				if (this.buttons != null && this.buttons.Length > 0)
				{
					this.TabClick(this.buttons[0]);
				}
			});
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00029D14 File Offset: 0x00027F14
	public void TabClick(int index)
	{
		int num = this.buttons.Length;
		if (index < 0 || index >= num)
		{
			return;
		}
		this.TabClick(this.buttons[index]);
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00029D48 File Offset: 0x00027F48
	public void TabClick(SBGUITabButton button)
	{
		if (this.selected == button)
		{
			return;
		}
		if (this.selected != null)
		{
			this.selected.SetTextureFromFound(this.foundOffMat);
			this.selected.Selected(false);
		}
		this.selected = button;
		this.selected.SetTextureFromFound(this.foundOnMat);
		this.selected.Selected(true);
		this.TabChangeEvent.FireEvent(this.selected);
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00029DCC File Offset: 0x00027FCC
	public SBGUITabButton FindButton(string name, bool includeInactive)
	{
		for (int i = 0; i < this.buttons.Length; i++)
		{
			if (includeInactive)
			{
				if (this.buttons[i].name == name)
				{
					return this.buttons[i];
				}
			}
			else if (this.buttons[i].IsActive() && this.buttons[i].name == name)
			{
				return this.buttons[i];
			}
		}
		return null;
	}

	// Token: 0x040004E5 RID: 1253
	public SBGUIScrollRegion scrollRegion;

	// Token: 0x040004E6 RID: 1254
	public string onTexture;

	// Token: 0x040004E7 RID: 1255
	public string offTexture;

	// Token: 0x040004E8 RID: 1256
	private SBGUITabButton selected;

	// Token: 0x040004E9 RID: 1257
	private SBGUITabButton[] buttons;

	// Token: 0x040004EA RID: 1258
	private YGTextureLibrary.FoundMaterial foundOnMat;

	// Token: 0x040004EB RID: 1259
	private YGTextureLibrary.FoundMaterial foundOffMat;

	// Token: 0x040004EC RID: 1260
	public EventDispatcher<SBGUITabButton> TabChangeEvent = new EventDispatcher<SBGUITabButton>();
}
