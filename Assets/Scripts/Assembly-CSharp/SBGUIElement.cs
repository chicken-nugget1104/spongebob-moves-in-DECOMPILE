using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class SBGUIElement : MonoBehaviour
{
	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x0001D74C File Offset: 0x0001B94C
	protected static int InstanceID
	{
		get
		{
			return SBGUIElement.instanceCount++;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x0001D75C File Offset: 0x0001B95C
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

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001D794 File Offset: 0x0001B994
	// (set) Token: 0x06000491 RID: 1169 RVA: 0x0001D7A4 File Offset: 0x0001B9A4
	public virtual Vector3 WorldPosition
	{
		get
		{
			return this.tform.position;
		}
		set
		{
			if (this.tform.position == value)
			{
				return;
			}
			this.tform.position = value;
			YGSprite.MeshUpdateHierarchy(base.gameObject);
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001D7EC File Offset: 0x0001B9EC
	// (set) Token: 0x06000492 RID: 1170 RVA: 0x0001D7E0 File Offset: 0x0001B9E0
	protected virtual bool Muted
	{
		get
		{
			return this.muted;
		}
		set
		{
			this.muted = value;
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0001D7F4 File Offset: 0x0001B9F4
	public void EnableRejectButton(bool enabled)
	{
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			YGAtlasSprite component = transform.GetComponent<YGAtlasSprite>();
			if (component != null && component.sprite != null && component.sprite.name != null && component.sprite.name.Contains("ActionStrip_Cancel.png"))
			{
				if (enabled)
				{
					component.SetAlpha(1f);
				}
				else
				{
					component.SetAlpha(0.25f);
				}
				transform.GetComponent<SBGUIButton>().enabled = enabled;
			}
		}
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0001D8A4 File Offset: 0x0001BAA4
	public void EnableButtons(bool enabled)
	{
		SBGUIButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIButton>(enabled);
		foreach (SBGUIButton sbguibutton in componentsInChildren)
		{
			sbguibutton.enabled = enabled;
		}
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0001D8E0 File Offset: 0x0001BAE0
	public virtual void MuteButtons(bool mute)
	{
		this.Muted = mute;
		if (this != null)
		{
			SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
			foreach (SBGUIElement sbguielement in componentsInChildren)
			{
				sbguielement.Muted = mute;
			}
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0001D930 File Offset: 0x0001BB30
	public void SetTransformParent(Transform parent)
	{
		this.tform.parent = parent;
		base.gameObject.layer = 0;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0001D958 File Offset: 0x0001BB58
	protected void SetTransformParent(SBGUIElement parent)
	{
		Transform parent2 = (!(parent == null)) ? parent.tform : GUIMainView.GetInstance().transform;
		this.tform.parent = parent2;
		base.gameObject.layer = this.View.gameObject.layer;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0001D9B0 File Offset: 0x0001BBB0
	public static SBGUIElement Create()
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIElement_{0}", SBGUIElement.InstanceID));
		SBGUIElement sbguielement = gameObject.AddComponent<SBGUIElement>();
		sbguielement.SetParent(null);
		return sbguielement;
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0001D9E8 File Offset: 0x0001BBE8
	public static SBGUIElement Create(SBGUIElement parent)
	{
		SBGUIElement sbguielement = SBGUIElement.Create();
		sbguielement.SetParent(parent);
		return sbguielement;
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001DA04 File Offset: 0x0001BC04
	public Transform tform
	{
		get
		{
			return (!(this._tform != null)) ? (this._tform = base.transform) : this._tform;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600049C RID: 1180 RVA: 0x0001DA3C File Offset: 0x0001BC3C
	public virtual Bounds TotalBounds
	{
		get
		{
			YGSprite[] componentsInChildren = base.GetComponentsInChildren<YGSprite>();
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return new Bounds(Vector3.zero, Vector3.zero);
			}
			Bounds bounds = componentsInChildren[0].GetBounds();
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				bounds.Encapsulate(componentsInChildren[i].GetBounds());
			}
			YGSprite component = base.GetComponent<YGSprite>();
			if (component)
			{
				bounds.Encapsulate(component.size);
			}
			else
			{
				YGAtlasSprite component2 = base.GetComponent<YGAtlasSprite>();
				if (component2)
				{
					bounds.Encapsulate(component2.size);
				}
			}
			return bounds;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600049D RID: 1181 RVA: 0x0001DAEC File Offset: 0x0001BCEC
	public float TotalWidth
	{
		get
		{
			return this.TotalBounds.ToRect().width;
		}
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0001DB0C File Offset: 0x0001BD0C
	public void ReregisterColliders()
	{
		YG2DBody[] componentsInChildren = base.GetComponentsInChildren<YG2DBody>();
		foreach (YG2DBody yg2DBody in componentsInChildren)
		{
			if (yg2DBody.enabled)
			{
				yg2DBody.ReregisterTouchable();
			}
		}
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0001DB4C File Offset: 0x0001BD4C
	public Transform GetParent()
	{
		return (!(this._tform != null)) ? null : this._tform.parent;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001DB7C File Offset: 0x0001BD7C
	public Dictionary<string, SBGUIElement> CacheChildren()
	{
		Dictionary<string, SBGUIElement> dictionary = new Dictionary<string, SBGUIElement>();
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
		foreach (SBGUIElement sbguielement in componentsInChildren)
		{
			if (!dictionary.ContainsKey(sbguielement.name) || !(sbguielement.gameObject == dictionary[sbguielement.name].gameObject))
			{
				dictionary.Add(sbguielement.name, sbguielement);
			}
		}
		return dictionary;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001DC00 File Offset: 0x0001BE00
	public SBGUIElement FindChild(string name)
	{
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
		foreach (SBGUIElement sbguielement in componentsInChildren)
		{
			if (sbguielement.gameObject.name == name)
			{
				return sbguielement;
			}
		}
		return null;
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0001DC50 File Offset: 0x0001BE50
	public SBGUIElement FindChildSessionActionId(string sessionActionId, bool includeInactive)
	{
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(includeInactive);
		foreach (SBGUIElement sbguielement in componentsInChildren)
		{
			if (sbguielement.SessionActionId == sessionActionId)
			{
				return sbguielement;
			}
		}
		return null;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0001DC98 File Offset: 0x0001BE98
	public virtual SBGUIElement FindDynamicSubElementSessionActionId(string sessionActionId, bool includeInactive)
	{
		return null;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0001DC9C File Offset: 0x0001BE9C
	public bool IsActive()
	{
		return base.gameObject.active;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0001DCAC File Offset: 0x0001BEAC
	public virtual void OnScreenStart(SBGUIScreen screen)
	{
		for (int i = 0; i < this.guiElements.Count; i++)
		{
			SBGUIElement sbguielement = this.guiElements[i];
			if (sbguielement)
			{
				sbguielement.OnScreenStart(screen);
			}
		}
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0001DCF4 File Offset: 0x0001BEF4
	public virtual void OnScreenEnd(SBGUIScreen screen)
	{
		this.MuteButtons(false);
		for (int i = 0; i < this.guiElements.Count; i++)
		{
			SBGUIElement sbguielement = this.guiElements[i];
			if (sbguielement)
			{
				sbguielement.OnScreenEnd(screen);
			}
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0001DD44 File Offset: 0x0001BF44
	public virtual void SetVisible(bool viz)
	{
		if (base.gameObject.renderer != null)
		{
			base.gameObject.renderer.enabled = viz;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0001DD78 File Offset: 0x0001BF78
	public bool Visible
	{
		get
		{
			return base.gameObject.renderer != null && base.gameObject.renderer.enabled;
		}
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0001DDB0 File Offset: 0x0001BFB0
	public virtual void SetActive(bool active)
	{
		base.gameObject.SetActiveRecursively(active);
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0001DDC0 File Offset: 0x0001BFC0
	public virtual void SetScreenPosition(float pos_x, float pos_y)
	{
		this.SetScreenPosition(new Vector2(pos_x, pos_y));
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
	public void SetScreenPosition(Vector2 pos)
	{
		pos.y = (float)Screen.height - pos.y;
		Vector3 vector = this.View.ScreenToWorld(pos);
		Vector3 position = this.tform.position;
		position.x = vector.x;
		position.y = vector.y;
		this.tform.position = position;
		this.UpdateColliderTransforms();
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0001DE3C File Offset: 0x0001C03C
	public Vector2 GetScreenPosition()
	{
		Vector2 result = this.View.WorldToScreen(this.tform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0001DE7C File Offset: 0x0001C07C
	protected void UpdateColliderTransforms()
	{
		YG2DBody[] componentsInChildren = base.GetComponentsInChildren<YG2DBody>(true);
		foreach (YG2DBody yg2DBody in componentsInChildren)
		{
			yg2DBody.MatchTransform3D();
		}
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0001DEB4 File Offset: 0x0001C0B4
	public void SetPosition(float pos_x, float pos_y, float pos_z)
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.z = pos_z;
		base.gameObject.transform.localPosition = localPosition;
		this.SetScreenPosition(pos_x, pos_y);
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0001DEF4 File Offset: 0x0001C0F4
	public void SetPosition(Vector3 pos)
	{
		this.SetPosition(pos.x, pos.y, pos.z);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0001DF14 File Offset: 0x0001C114
	public void SetLookAt(Vector3 position, Vector3 up)
	{
		this.tform.LookAt(position, up);
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0001DF24 File Offset: 0x0001C124
	public virtual void SetParent(SBGUIElement element)
	{
		this.SetTransformParent(element);
		this.EnforceMuteFromParent(element);
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0001DF34 File Offset: 0x0001C134
	public virtual void SetParent(SBGUIElement element, bool bEnforceMuteFromParent)
	{
		this.SetTransformParent(element);
		if (bEnforceMuteFromParent)
		{
			this.EnforceMuteFromParent(element);
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0001DF4C File Offset: 0x0001C14C
	public virtual void GUIUpdate()
	{
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x0001DF50 File Offset: 0x0001C150
	protected virtual void Awake()
	{
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0001DF54 File Offset: 0x0001C154
	protected virtual void OnEnable()
	{
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x0001DF58 File Offset: 0x0001C158
	protected virtual void OnDisable()
	{
		if (this.View != null)
		{
			this.View.RefreshEvent -= this.ReregisterColliders;
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0001DF90 File Offset: 0x0001C190
	public virtual void AttachAnalyticsToButton(string buttonName, SBGUIButton button)
	{
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0001DF94 File Offset: 0x0001C194
	public SBGUIButton AttachActionToButton(string buttonName, Action action)
	{
		SBGUIButton button = this.FindChild(buttonName) as SBGUIButton;
		return this.AttachActionToButton(button, action);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0001DFB8 File Offset: 0x0001C1B8
	public SBGUIButton AttachActionToButton(SBGUIButton button, Action action)
	{
		if (button == null)
		{
			TFUtils.Assert(false, string.Format("{0} doesn't have a child named {1}", base.gameObject.name, button.name));
			return null;
		}
		this.AttachAnalyticsToButton(button.name, button);
		button.ClickEvent += action;
		button.ClickEvent += delegate()
		{
			this.ReactivateButton(button);
		};
		return button;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0001E054 File Offset: 0x0001C254
	public void EnforceMuteFromParent(SBGUIElement element)
	{
		if (element != null && element.muted)
		{
			this.muted = true;
			this.MuteButtons(true);
		}
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001E07C File Offset: 0x0001C27C
	public void ClearButtonActions(string buttonName)
	{
		SBGUIButton sbguibutton = this.FindChild(buttonName) as SBGUIButton;
		if (sbguibutton == null)
		{
			TFUtils.Assert(false, string.Format("{0} doesn't have a child named {1}", base.gameObject.name, buttonName));
			return;
		}
		sbguibutton.ClearClickEvents();
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0001E0C8 File Offset: 0x0001C2C8
	public void ReactivateButton(SBGUIButton button)
	{
		TwoShadeButton component = button.GetComponent<TwoShadeButton>();
		if (component != null)
		{
			component.ResetHighlightState();
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0001E0F0 File Offset: 0x0001C2F0
	public void UpdateCollider()
	{
		YG2DBody component = base.gameObject.GetComponent<YG2DBody>();
		if (component.enabled)
		{
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0001E124 File Offset: 0x0001C324
	public void StartTimer()
	{
		this.startTime = DateTime.UtcNow;
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001E134 File Offset: 0x0001C334
	public double ElapsedTime
	{
		get
		{
			return (DateTime.UtcNow - this.startTime).TotalMilliseconds;
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0001E15C File Offset: 0x0001C35C
	public virtual void OnDestroy()
	{
		SBGUI currentInstance = SBGUI.GetCurrentInstance();
		if (currentInstance != null)
		{
			currentInstance.UnWhitelistElement(this);
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0001E1A4 File Offset: 0x0001C3A4
	// (set) Token: 0x060004C1 RID: 1217 RVA: 0x0001E184 File Offset: 0x0001C384
	public float Alpha
	{
		get
		{
			YGSprite component = base.GetComponent<YGSprite>();
			return component.color.a;
		}
		set
		{
			YGSprite component = base.GetComponent<YGSprite>();
			component.color.a = value;
		}
	}

	// Token: 0x0400038C RID: 908
	public string SessionActionId;

	// Token: 0x0400038D RID: 909
	protected static int instanceCount;

	// Token: 0x0400038E RID: 910
	protected bool muted;

	// Token: 0x0400038F RID: 911
	private GUIView _view;

	// Token: 0x04000390 RID: 912
	protected Rect rect;

	// Token: 0x04000391 RID: 913
	private Transform _tform;

	// Token: 0x04000392 RID: 914
	protected List<SBGUIElement> guiElements = new List<SBGUIElement>();

	// Token: 0x04000393 RID: 915
	public EventDispatcher<SBGUIEvent> EventListener = new EventDispatcher<SBGUIEvent>();

	// Token: 0x04000394 RID: 916
	private DateTime startTime;
}
