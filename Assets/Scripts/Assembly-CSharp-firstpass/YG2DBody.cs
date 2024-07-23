using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using UnityEngine;
using Yarg;

// Token: 0x020000F5 RID: 245
[ExecuteInEditMode]
public abstract class YG2DBody : MonoBehaviour, ITouchable
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x00023C54 File Offset: 0x00021E54
	protected YG2DWorld YargWorld
	{
		get
		{
			if (this.yargWorld == null)
			{
				Transform transform = this.tform;
				while (!(transform == null))
				{
					this.yargWorld = transform.GetComponent<YG2DWorld>();
					transform = transform.parent;
					if (!(this.yargWorld == null))
					{
						goto IL_88;
					}
				}
				this.yargWorld = GUIMainView.GetInstance()._2DWorld;
				if (this.yargWorld == null)
				{
					Debug.LogError(string.Format("{0} couldn't find 2d world", base.gameObject.name));
				}
			}
			IL_88:
			return this.yargWorld;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x0600093A RID: 2362 RVA: 0x00023CF0 File Offset: 0x00021EF0
	public Body Body
	{
		get
		{
			if (this.body == null)
			{
				Debug.LogWarning(string.Format("No body has been created for {0}", base.gameObject.name));
			}
			return this.body;
		}
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00023D20 File Offset: 0x00021F20
	protected virtual Body GetBody(World world)
	{
		return null;
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600093C RID: 2364 RVA: 0x00023D24 File Offset: 0x00021F24
	public Transform tform
	{
		get
		{
			return (!(this._tform != null)) ? (this._tform = base.transform) : this._tform;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x00023D5C File Offset: 0x00021F5C
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

	// Token: 0x0600093E RID: 2366 RVA: 0x00023D94 File Offset: 0x00021F94
	public virtual bool TouchEvent(YGEvent evt)
	{
		YGEvent.TYPE type = evt.type;
		switch (type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			this.touchInProgress = true;
			goto IL_4C;
		case YGEvent.TYPE.TOUCH_END:
			this.touchInProgress = false;
			goto IL_4C;
		case YGEvent.TYPE.TOUCH_CANCEL:
			break;
		default:
			if (type != YGEvent.TYPE.RESET)
			{
				goto IL_4C;
			}
			break;
		}
		this.touchInProgress = false;
		IL_4C:
		return this.EventDispatch.FireEvent(evt);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00023DFC File Offset: 0x00021FFC
	protected virtual void OnEnable()
	{
		GUIView view = this.View;
		World world = view._2DWorld.World;
		if (this.world != world)
		{
			if (this.body != null)
			{
				this.body.Dispose();
				this.body = null;
			}
			this.world = world;
		}
		if (this.body == null)
		{
			this.body = this.GetBody(this.world);
		}
		if (this.body == null)
		{
			Debug.LogError(string.Format("creating Yarg2DBody failed: {0}", base.gameObject.name));
			return;
		}
		this.body.BodyType = this.bodyType;
		this.body.OnCollision += this.OnCollision;
		this.body.OnSeparation += this.OnSeparation;
		this.body.UserData = this;
		view.RegisterTouchable(this.tform.GetInstanceID(), this);
		YGSprite component = base.GetComponent<YGSprite>();
		if (component != null)
		{
			component.MeshUpdateEvent.AddListener(new Action(this.MatchTransform3D));
			view.RefreshEvent += this.MatchTransform3D;
		}
		else
		{
			this.body.SetTransform(this.tform.position, this.tform.rotation.eulerAngles.z * 0.017453292f);
		}
		view.RefreshWorld();
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00023F74 File Offset: 0x00022174
	public void ReregisterTouchable()
	{
		this.OnDisable();
		this.OnEnable();
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00023F84 File Offset: 0x00022184
	public void MatchTransform3D()
	{
		if (!base.gameObject.active || !base.enabled)
		{
			return;
		}
		if (this.sprite == null)
		{
			this.sprite = base.gameObject.GetComponent<YGSprite>();
		}
		Vector3 b = Vector3.zero;
		Quaternion rotation = this.tform.rotation;
		if (this.sprite != null)
		{
			Mesh sharedMesh = this.sprite.meshFilter.sharedMesh;
			if (sharedMesh != null)
			{
				b = rotation * sharedMesh.bounds.center;
			}
		}
		this.body.SetTransform(this.tform.position + b, rotation.eulerAngles.z * 0.017453292f);
		this.View.RefreshWorld();
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00024068 File Offset: 0x00022268
	private void Start()
	{
		this.View.RefreshEvent += this.MatchTransform3D;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00024084 File Offset: 0x00022284
	protected virtual void OnDisable()
	{
		if (this.touchInProgress)
		{
			this.touchInProgress = false;
			YGEvent ygevent = new YGEvent();
			ygevent.type = YGEvent.TYPE.RESET;
			this.EventDispatch.FireEvent(ygevent);
		}
		GUIView view = this.View;
		view.RefreshEvent -= this.MatchTransform3D;
		view.UnregisterTouchable(this.tform.GetInstanceID());
		YGSprite component = base.GetComponent<YGSprite>();
		if (component != null)
		{
			component.MeshUpdateEvent.RemoveListener(new Action(this.MatchTransform3D));
		}
		this._view = null;
		if (this.body == null)
		{
			return;
		}
		this.body.OnCollision -= this.OnCollision;
		this.body.OnSeparation -= this.OnSeparation;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00024158 File Offset: 0x00022358
	public void OnDestroy()
	{
		if (this.body != null)
		{
			this.body.Dispose();
			this.body = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00024190 File Offset: 0x00022390
	public virtual void UpdateTransform()
	{
		YG2DWorld.UpdateTransform(this.tform, this.body);
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x000241A4 File Offset: 0x000223A4
	protected virtual void OnSeparation(Fixture f1, Fixture f2)
	{
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x000241A8 File Offset: 0x000223A8
	protected virtual bool OnCollision(Fixture f1, Fixture f2, Contact contact)
	{
		return true;
	}

	// Token: 0x040005DB RID: 1499
	public float density = 1f;

	// Token: 0x040005DC RID: 1500
	public BodyType bodyType = BodyType.Kinematic;

	// Token: 0x040005DD RID: 1501
	protected Body body;

	// Token: 0x040005DE RID: 1502
	public Vector2 offset = Vector2.zero;

	// Token: 0x040005DF RID: 1503
	public YGEventDispatcher EventDispatch = new YGEventDispatcher();

	// Token: 0x040005E0 RID: 1504
	protected YG2DWorld yargWorld;

	// Token: 0x040005E1 RID: 1505
	protected World world;

	// Token: 0x040005E2 RID: 1506
	private YGSprite sprite;

	// Token: 0x040005E3 RID: 1507
	[NonSerialized]
	protected Transform _tform;

	// Token: 0x040005E4 RID: 1508
	[NonSerialized]
	protected GUIView _view;

	// Token: 0x040005E5 RID: 1509
	protected bool touchInProgress;
}
