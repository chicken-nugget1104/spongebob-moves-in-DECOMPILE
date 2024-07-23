using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public interface AnimationEventNodeDelegate
{
	// Token: 0x06001E8F RID: 7823
	void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr);

	// Token: 0x06001E90 RID: 7824
	void InitializeWithData(Dictionary<string, object> dict);
}
