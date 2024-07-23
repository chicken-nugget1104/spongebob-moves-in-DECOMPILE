using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class HelloYarg : MonoBehaviour
{
	// Token: 0x06000856 RID: 2134 RVA: 0x0001F3A0 File Offset: 0x0001D5A0
	private void Start()
	{
		if (this.button != null)
		{
			this.button.TapEvent.AddListener(delegate
			{
				Debug.Log("Hello World");
			});
			this.button.TapEvent.AddListener(new Action(this.OnButtonTap));
		}
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0001F408 File Offset: 0x0001D608
	private void OnButtonTap()
	{
		Debug.Log("Foo and Bar");
	}

	// Token: 0x04000514 RID: 1300
	public TapButton button;
}
