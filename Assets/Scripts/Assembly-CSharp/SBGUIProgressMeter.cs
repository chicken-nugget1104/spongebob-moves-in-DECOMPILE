using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000096 RID: 150
[RequireComponent(typeof(YGFrameAtlasSprite))]
public class SBGUIProgressMeter : SBGUIAtlasImage
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000595 RID: 1429 RVA: 0x00023EAC File Offset: 0x000220AC
	// (set) Token: 0x06000596 RID: 1430 RVA: 0x00023EB4 File Offset: 0x000220B4
	public bool running { get; private set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000597 RID: 1431 RVA: 0x00023EC0 File Offset: 0x000220C0
	// (set) Token: 0x06000598 RID: 1432 RVA: 0x00023EC8 File Offset: 0x000220C8
	public float Progress
	{
		get
		{
			return this.progress;
		}
		set
		{
			if (this.meter == null || this.fill == null)
			{
				TFUtils.Assert(false, string.Format("Progress meter '{0}' is missing background or fill sprite", base.gameObject.name));
				return;
			}
			value = Mathf.Clamp01(value);
			if (this.progress == value)
			{
				return;
			}
			Vector2 size = this.fill.Size;
			size.x = this.meter.Size.x * value;
			this.fill.Size = size;
			this.progress = value;
		}
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00023F64 File Offset: 0x00022164
	public void AnimatedProgress(float prog, float duration)
	{
		prog = Mathf.Clamp01(prog);
		if (this.targetProgress == prog)
		{
			return;
		}
		this.targetProgress = prog;
		base.StartCoroutine(this.AnimatedProgressCoroutine(null, prog, duration));
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00023FA8 File Offset: 0x000221A8
	public void ForceAnimatedProgress(float start, float prog, float duration)
	{
		prog = Mathf.Clamp01(prog);
		if (this.targetProgress == prog)
		{
			return;
		}
		this.targetProgress = prog;
		base.StartCoroutine(this.AnimatedProgressCoroutine(new float?(start), prog, duration));
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00023FDC File Offset: 0x000221DC
	private IEnumerator AnimatedProgressCoroutine(float? start, float target, float duration)
	{
		this.running = true;
		if (start == null)
		{
			start = new float?(this.progress);
		}
		float elapsed = 0f;
		while (elapsed < duration)
		{
			if (this.targetProgress != target)
			{
				yield break;
			}
			elapsed += Time.deltaTime;
			this.Progress = Mathf.Lerp(start.Value, target, elapsed / duration);
			yield return null;
		}
		this.running = false;
		yield break;
	}

	// Token: 0x04000451 RID: 1105
	public SBGUIAtlasImage meter;

	// Token: 0x04000452 RID: 1106
	public SBGUIAtlasImage fill;

	// Token: 0x04000453 RID: 1107
	private float progress;

	// Token: 0x04000454 RID: 1108
	private float targetProgress;
}
