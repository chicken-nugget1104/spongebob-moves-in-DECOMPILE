using System;
using UnityEngine;

// Token: 0x02000074 RID: 116
public abstract class FGComponent : MonoBehaviour
{
	// Token: 0x060003F1 RID: 1009 RVA: 0x00011400 File Offset: 0x0000F600
	protected virtual void Awake()
	{
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00011404 File Offset: 0x0000F604
	protected virtual void Start()
	{
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00011408 File Offset: 0x0000F608
	protected virtual void OnEnable()
	{
		FingerGestures.OnFingersUpdated += this.FingerGestures_OnFingersUpdated;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001141C File Offset: 0x0000F61C
	protected virtual void OnDisable()
	{
		FingerGestures.OnFingersUpdated -= this.FingerGestures_OnFingersUpdated;
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x00011430 File Offset: 0x0000F630
	private void FingerGestures_OnFingersUpdated()
	{
		this.OnUpdate(FingerGestures.Touches);
	}

	// Token: 0x060003F6 RID: 1014
	protected abstract void OnUpdate(FingerGestures.IFingerList touches);

	// Token: 0x02000114 RID: 276
	// (Invoke) Token: 0x06000A16 RID: 2582
	public delegate void EventDelegate<T>(T source) where T : FGComponent;
}
