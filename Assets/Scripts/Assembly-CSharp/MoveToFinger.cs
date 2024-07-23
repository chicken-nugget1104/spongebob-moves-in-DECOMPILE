using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class MoveToFinger : MonoBehaviour
{
	// Token: 0x06000158 RID: 344 RVA: 0x000075B0 File Offset: 0x000057B0
	private void OnEnable()
	{
		FingerGestures.OnFingerDown += this.OnFingerDown;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x000075C4 File Offset: 0x000057C4
	private void OnDisable()
	{
		FingerGestures.OnFingerDown -= this.OnFingerDown;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x000075D8 File Offset: 0x000057D8
	private void OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		base.transform.position = this.GetWorldPos(fingerPos);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000075EC File Offset: 0x000057EC
	private Vector3 GetWorldPos(Vector2 screenPos)
	{
		Camera main = Camera.main;
		return main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(base.transform.position.z - main.transform.position.z)));
	}
}
