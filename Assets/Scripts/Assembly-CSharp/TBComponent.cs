using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public abstract class TBComponent : MonoBehaviour
{
	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600010D RID: 269 RVA: 0x00006648 File Offset: 0x00004848
	// (set) Token: 0x0600010E RID: 270 RVA: 0x00006650 File Offset: 0x00004850
	public int FingerIndex
	{
		get
		{
			return this.fingerIndex;
		}
		protected set
		{
			this.fingerIndex = value;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x0600010F RID: 271 RVA: 0x0000665C File Offset: 0x0000485C
	// (set) Token: 0x06000110 RID: 272 RVA: 0x00006664 File Offset: 0x00004864
	public Vector2 FingerPos
	{
		get
		{
			return this.fingerPos;
		}
		protected set
		{
			this.fingerPos = value;
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00006670 File Offset: 0x00004870
	protected virtual void Start()
	{
		if (!base.collider)
		{
			Debug.LogError(base.name + " must have a valid collider.");
			base.enabled = false;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x000066AC File Offset: 0x000048AC
	protected bool Send(TBComponent.Message msg)
	{
		if (!base.enabled)
		{
			return false;
		}
		if (!msg.enabled)
		{
			return false;
		}
		GameObject gameObject = msg.target;
		if (!gameObject)
		{
			gameObject = base.gameObject;
		}
		gameObject.SendMessage(msg.methodName, SendMessageOptions.DontRequireReceiver);
		return true;
	}

	// Token: 0x04000097 RID: 151
	private int fingerIndex = -1;

	// Token: 0x04000098 RID: 152
	private Vector2 fingerPos;

	// Token: 0x02000019 RID: 25
	[Serializable]
	public class Message
	{
		// Token: 0x06000113 RID: 275 RVA: 0x000066FC File Offset: 0x000048FC
		public Message()
		{
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006718 File Offset: 0x00004918
		public Message(string methodName)
		{
			this.methodName = methodName;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000673C File Offset: 0x0000493C
		public Message(string methodName, bool enabled)
		{
			this.enabled = enabled;
			this.methodName = methodName;
		}

		// Token: 0x04000099 RID: 153
		public bool enabled = true;

		// Token: 0x0400009A RID: 154
		public string methodName = "MethodToCall";

		// Token: 0x0400009B RID: 155
		public GameObject target;
	}

	// Token: 0x02000490 RID: 1168
	// (Invoke) Token: 0x060024A3 RID: 9379
	public delegate void EventHandler<T>(T sender) where T : TBComponent;
}
