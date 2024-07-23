using System;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class AnimationEventHandlerComponent : MonoBehaviour
{
	// Token: 0x06001E96 RID: 7830 RVA: 0x000BC794 File Offset: 0x000BA994
	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		if (this.animationEventHandlerDelegate != null)
		{
			this.animationEventHandlerDelegate.HandleAnimationEvent(animationEvent);
		}
	}

	// Token: 0x040012FB RID: 4859
	public AnimationEventHandlerDelegate animationEventHandlerDelegate;
}
