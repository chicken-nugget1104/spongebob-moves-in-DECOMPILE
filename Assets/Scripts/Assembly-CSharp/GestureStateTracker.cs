using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class GestureStateTracker : MonoBehaviour
{
	// Token: 0x06000153 RID: 339 RVA: 0x000074D8 File Offset: 0x000056D8
	private void Awake()
	{
		if (!this.gesture)
		{
			this.gesture = base.GetComponent<GestureRecognizer>();
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x000074F8 File Offset: 0x000056F8
	private void OnEnable()
	{
		if (this.gesture)
		{
			this.gesture.OnStateChanged += this.gesture_OnStateChanged;
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00007524 File Offset: 0x00005724
	private void OnDisable()
	{
		if (this.gesture)
		{
			this.gesture.OnStateChanged -= this.gesture_OnStateChanged;
		}
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00007550 File Offset: 0x00005750
	private void gesture_OnStateChanged(GestureRecognizer source)
	{
		Debug.Log(string.Concat(new object[]
		{
			"Gesture ",
			source,
			" changed from ",
			source.PreviousState,
			" to ",
			source.State
		}));
	}

	// Token: 0x040000CF RID: 207
	public GestureRecognizer gesture;
}
