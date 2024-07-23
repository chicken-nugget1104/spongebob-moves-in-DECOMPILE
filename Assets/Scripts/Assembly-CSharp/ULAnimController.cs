using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000482 RID: 1154
public class ULAnimController : ULAnimControllerInterface
{
	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x06002429 RID: 9257 RVA: 0x000DE1F4 File Offset: 0x000DC3F4
	// (set) Token: 0x0600242A RID: 9258 RVA: 0x000DE1FC File Offset: 0x000DC3FC
	public Animation UnityAnimation
	{
		get
		{
			return this.animation;
		}
		set
		{
			TFUtils.Assert(value != null, "Should not set UnityAnimation to null for ULAnimController.");
			this.animation = value;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x0600242B RID: 9259 RVA: 0x000DE218 File Offset: 0x000DC418
	// (set) Token: 0x0600242C RID: 9260 RVA: 0x000DE220 File Offset: 0x000DC420
	public ULAnimModelInterface AnimationModel
	{
		get
		{
			return this.animationModel;
		}
		set
		{
			this.animationModel = value;
		}
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x000DE22C File Offset: 0x000DC42C
	public static IEnumerator PlaySequence(Animation tgtAnimation, string[] sequence)
	{
		float totalTime = 0f;
		foreach (string s in sequence)
		{
			if (totalTime == 0f)
			{
				tgtAnimation.Play(s, PlayMode.StopSameLayer);
			}
			else
			{
				tgtAnimation.PlayQueued(s, QueueMode.CompleteOthers);
			}
			totalTime += tgtAnimation[s].length;
		}
		yield return new WaitForSeconds(totalTime);
		yield break;
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x000DE25C File Offset: 0x000DC45C
	public static IEnumerator PlayRandom(Animation tgtAnimation, string[] domain)
	{
		string s = domain[(int)(UnityEngine.Random.value * (float)domain.Length)];
		tgtAnimation.Play(s, PlayMode.StopSameLayer);
		yield return new WaitForSeconds(tgtAnimation[s].length);
		yield break;
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x000DE28C File Offset: 0x000DC48C
	public bool HasAnimation(string animationName)
	{
		return this.animationModel.HasAnimation(animationName);
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x000DE29C File Offset: 0x000DC49C
	public bool AnimationEnabled()
	{
		return this.enabled;
	}

	// Token: 0x06002431 RID: 9265 RVA: 0x000DE2A4 File Offset: 0x000DC4A4
	public void EnableAnimation(bool toEnabled)
	{
		this.enabled = toEnabled;
		if (!this.enabled)
		{
			this.animation.Stop();
		}
	}

	// Token: 0x06002432 RID: 9266 RVA: 0x000DE2C4 File Offset: 0x000DC4C4
	public void PlayAnimation(string animationName)
	{
		if (this.enabled)
		{
			PlayMode mode = this.animationModel.AnimPlayMode(animationName);
			this.animation.Play(animationName, mode);
		}
	}

	// Token: 0x06002433 RID: 9267 RVA: 0x000DE2F8 File Offset: 0x000DC4F8
	public void StopAnimation(string animationName)
	{
		if (this.animation != null)
		{
			this.animation.Stop(animationName);
		}
	}

	// Token: 0x06002434 RID: 9268 RVA: 0x000DE318 File Offset: 0x000DC518
	public void StopAnimations()
	{
		if (this.animation != null)
		{
			this.animation.Stop();
		}
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x000DE338 File Offset: 0x000DC538
	public void Sample(string animationName, float time)
	{
		AnimationState animationState = this.animation[animationName];
		animationState.enabled = true;
		animationState.time = time;
		animationState.weight = 1f;
		this.animation.Sample();
		animationState.enabled = false;
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x000DE380 File Offset: 0x000DC580
	public void SampleWithNormalizedTime(string animationName, float normalizedTime)
	{
		AnimationState animationState = this.animation[animationName];
		animationState.enabled = true;
		animationState.normalizedTime = normalizedTime;
		animationState.weight = 1f;
		this.animation.Sample();
		animationState.enabled = false;
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x000DE3C8 File Offset: 0x000DC5C8
	public float GetFrameRate(string animationName)
	{
		return this.animation[animationName].clip.frameRate;
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000DE3E0 File Offset: 0x000DC5E0
	public float GetLength(string animationName)
	{
		return this.animation[animationName].clip.length;
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x000DE3F8 File Offset: 0x000DC5F8
	public float NormalizedTimePerFrame(string animationName)
	{
		AnimationState animationState = this.animation[animationName];
		float frameRate = animationState.clip.frameRate;
		float num = frameRate / ((Application.targetFrameRate >= 0) ? ((float)Application.targetFrameRate) : 60f);
		return 1f / frameRate / animationState.clip.length * num;
	}

	// Token: 0x04001644 RID: 5700
	protected bool enabled;

	// Token: 0x04001645 RID: 5701
	protected Animation animation;

	// Token: 0x04001646 RID: 5702
	protected ULAnimModelInterface animationModel;
}
