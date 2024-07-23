using System;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public interface ULAnimModelInterface
{
	// Token: 0x0600244E RID: 9294
	bool HasAnimation(string animName);

	// Token: 0x0600244F RID: 9295
	AnimationClip AnimClip(string animName);

	// Token: 0x06002450 RID: 9296
	AnimationBlendMode AnimBlendMode(string animName);

	// Token: 0x06002451 RID: 9297
	WrapMode AnimWrapMode(string animName);

	// Token: 0x06002452 RID: 9298
	PlayMode AnimPlayMode(string animName);

	// Token: 0x06002453 RID: 9299
	int AnimLayer(string animName);

	// Token: 0x06002454 RID: 9300
	void ApplyAnimationSettings(Animation targetAnimation);

	// Token: 0x06002455 RID: 9301
	void UnapplyAnimationSettings(Animation targetAnimation);
}
