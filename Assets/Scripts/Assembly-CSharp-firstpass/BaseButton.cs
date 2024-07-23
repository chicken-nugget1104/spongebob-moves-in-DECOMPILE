using System;
using UnityEngine;
using Yarg;

// Token: 0x020000D5 RID: 213
public class BaseButton : MonoBehaviour, ILoadable
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600085A RID: 2138 RVA: 0x0001F428 File Offset: 0x0001D628
	protected virtual bool NeedsLoad
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001F42C File Offset: 0x0001D62C
	protected GUIView View
	{
		get
		{
			if (this._view == null)
			{
				this._view = GUIView.GetParentView(this.tform);
			}
			return this._view;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600085C RID: 2140 RVA: 0x0001F464 File Offset: 0x0001D664
	protected Transform tform
	{
		get
		{
			return (!(this._tform != null)) ? (this._tform = base.transform) : this._tform;
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0001F49C File Offset: 0x0001D69C
	public void SetPosition(int x, int y)
	{
		Vector3 position = this.View.PixelsToWorld(new Vector2((float)x, (float)y));
		this.tform.position = position;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0001F4CC File Offset: 0x0001D6CC
	public virtual void Load()
	{
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0001F4D0 File Offset: 0x0001D6D0
	protected virtual void OnEnable()
	{
		this.body = base.GetComponent<YG2DBody>();
		if (this.body == null)
		{
			this.body = base.gameObject.AddComponent<YG2DRectangle>();
		}
		this.body.EventDispatch.AddListener(new Func<YGEvent, bool>(this.TouchEventHandler));
		if (this.NeedsLoad)
		{
			this.View.RefreshEvent += this.Load;
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0001F54C File Offset: 0x0001D74C
	protected virtual void OnDisable()
	{
		if (this.body != null)
		{
			this.body.EventDispatch.RemoveListener(new Func<YGEvent, bool>(this.TouchEventHandler));
		}
		this._view = null;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0001F584 File Offset: 0x0001D784
	protected virtual bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000862 RID: 2146 RVA: 0x0001F588 File Offset: 0x0001D788
	protected YGSprite parent
	{
		get
		{
			if (this._parent == null)
			{
				this._parent = base.GetComponent<YGSprite>();
			}
			return this._parent;
		}
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
	public virtual void SetVisible(bool visible)
	{
		this.parent.enabled = visible;
	}

	// Token: 0x04000516 RID: 1302
	private YG2DBody body;

	// Token: 0x04000517 RID: 1303
	private GUIView _view;

	// Token: 0x04000518 RID: 1304
	private Transform _tform;

	// Token: 0x04000519 RID: 1305
	private YGSprite _parent;
}
