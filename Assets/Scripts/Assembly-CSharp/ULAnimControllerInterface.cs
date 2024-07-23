using System;

// Token: 0x02000483 RID: 1155
public interface ULAnimControllerInterface
{
	// Token: 0x0600243A RID: 9274
	bool HasAnimation(string animationName);

	// Token: 0x0600243B RID: 9275
	bool AnimationEnabled();

	// Token: 0x0600243C RID: 9276
	void EnableAnimation(bool enabled);

	// Token: 0x0600243D RID: 9277
	void PlayAnimation(string animationName);

	// Token: 0x0600243E RID: 9278
	void StopAnimation(string animationName);

	// Token: 0x0600243F RID: 9279
	void StopAnimations();

	// Token: 0x06002440 RID: 9280
	void Sample(string animationName, float normalizedTime);

	// Token: 0x06002441 RID: 9281
	float NormalizedTimePerFrame(string animationName);
}
