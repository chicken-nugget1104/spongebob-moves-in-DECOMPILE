using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class SBGUISlidingLabel : SBGUILabel
{
	// Token: 0x0600060C RID: 1548 RVA: 0x00026618 File Offset: 0x00024818
	public void AnimatedSliding(Vector2 endOffset, float endAlpha, float duration, bool destroyOnFinish = false, SBGUISlidingLabel.UpdateText updateText = null)
	{
		if (this.running)
		{
			return;
		}
		Vector2 screenPosition = base.GetScreenPosition();
		Vector2 endPosition = new Vector2(screenPosition.x + endOffset.x, screenPosition.y + endOffset.y);
		base.StartCoroutine(this.AnimatedSlidingCoroutine(screenPosition, endPosition, endAlpha, duration, destroyOnFinish));
		this.updateTextDelegate = updateText;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00026678 File Offset: 0x00024878
	private IEnumerator AnimatedSlidingCoroutine(Vector2 startPosition, Vector2 endPosition, float endAlpha, float duration, bool destroyOnFinish)
	{
		if (this.running)
		{
			yield return null;
		}
		float startAlpha = 1f;
		float elapsed = 0f;
		this.running = true;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float elapsedOverDuration = elapsed / duration;
			float currentX = Mathf.Lerp(startPosition.x, endPosition.x, elapsedOverDuration);
			float currentY = Mathf.Lerp(startPosition.y, endPosition.y, elapsedOverDuration);
			float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedOverDuration);
			base.SetAlpha(currentAlpha);
			this.SetScreenPosition(currentX, currentY);
			if (this.updateTextDelegate != null)
			{
				this.SetText(this.updateTextDelegate());
			}
			yield return null;
		}
		this.running = false;
		if (destroyOnFinish)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x0400049E RID: 1182
	private bool running;

	// Token: 0x0400049F RID: 1183
	private SBGUISlidingLabel.UpdateText updateTextDelegate;

	// Token: 0x02000499 RID: 1177
	// (Invoke) Token: 0x060024C7 RID: 9415
	public delegate string UpdateText();
}
