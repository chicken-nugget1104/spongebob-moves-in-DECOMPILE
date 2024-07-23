using System;
using System.Collections.Generic;

// Token: 0x020002A9 RID: 681
public class InteractionState
{
	// Token: 0x060014CE RID: 5326 RVA: 0x0008C2BC File Offset: 0x0008A4BC
	public void SetInteractions(bool isEditable, bool isGrabbable, bool isSelectable, bool hasSendClickAction, BaseTransitionBinding transition = null, ICollection<IControlBinding> newControls = null)
	{
		this.isEditable = isEditable;
		this.isGrabbable = isGrabbable;
		this.isSelectable = isSelectable;
		this.hasSendClickAction = hasSendClickAction;
		this.selectedStateTransition = transition;
		this.ClearControls();
		if (newControls != null)
		{
			this.controls.Push(newControls);
		}
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x0008C308 File Offset: 0x0008A508
	public void Clear()
	{
		this.isEditable = false;
		this.isGrabbable = false;
		this.isSelectable = false;
		this.hasSendClickAction = false;
		this.selectedStateTransition = null;
		this.controls.Clear();
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0008C344 File Offset: 0x0008A544
	public bool HasClickCommandFunctionality
	{
		get
		{
			return this.hasSendClickAction;
		}
	}

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x060014D1 RID: 5329 RVA: 0x0008C34C File Offset: 0x0008A54C
	public bool IsGrabbable
	{
		get
		{
			return this.isGrabbable;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x060014D2 RID: 5330 RVA: 0x0008C354 File Offset: 0x0008A554
	// (set) Token: 0x060014D3 RID: 5331 RVA: 0x0008C35C File Offset: 0x0008A55C
	public bool IsSelectable
	{
		get
		{
			return this.isSelectable;
		}
		set
		{
			this.isSelectable = value;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0008C368 File Offset: 0x0008A568
	public bool IsEditable
	{
		get
		{
			return this.isEditable;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x060014D5 RID: 5333 RVA: 0x0008C370 File Offset: 0x0008A570
	// (set) Token: 0x060014D6 RID: 5334 RVA: 0x0008C378 File Offset: 0x0008A578
	public BaseTransitionBinding SelectedTransition
	{
		get
		{
			return this.selectedStateTransition;
		}
		set
		{
			this.selectedStateTransition = value;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x060014D7 RID: 5335 RVA: 0x0008C384 File Offset: 0x0008A584
	public ICollection<IControlBinding> Controls
	{
		get
		{
			if (this.controls.Count > 0)
			{
				return this.controls.Peek();
			}
			return null;
		}
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x0008C3A4 File Offset: 0x0008A5A4
	public void PushControls(ICollection<IControlBinding> newControls)
	{
		this.controls.Push(newControls);
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x0008C3B4 File Offset: 0x0008A5B4
	public ICollection<IControlBinding> PopControls()
	{
		TFUtils.Assert(this.controls.Count > 0, "Trying to pop the controls stack, but it has nothing on it");
		return this.controls.Pop();
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x0008C3DC File Offset: 0x0008A5DC
	public void ClearControls()
	{
		this.controls.Clear();
	}

	// Token: 0x04000E87 RID: 3719
	private bool hasSendClickAction;

	// Token: 0x04000E88 RID: 3720
	private bool isGrabbable;

	// Token: 0x04000E89 RID: 3721
	private bool isSelectable;

	// Token: 0x04000E8A RID: 3722
	private bool isEditable;

	// Token: 0x04000E8B RID: 3723
	private BaseTransitionBinding selectedStateTransition;

	// Token: 0x04000E8C RID: 3724
	private Stack<ICollection<IControlBinding>> controls = new Stack<ICollection<IControlBinding>>();
}
