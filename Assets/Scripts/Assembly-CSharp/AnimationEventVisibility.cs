using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EE RID: 1006
public class AnimationEventVisibility : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	// Token: 0x06001EB1 RID: 7857 RVA: 0x000BCF34 File Offset: 0x000BB134
	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		GameObject gameObject = (GameObject)animationEvent.objectReferenceParameter;
		bool flag = this.visibilities[animationEvent.time];
		if (gameObject.renderer.isVisible != flag)
		{
			gameObject.renderer.enabled = flag;
		}
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x000BCF7C File Offset: 0x000BB17C
	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
		GameObject gameObject = TFUtils.FindGameObjectInHierarchy(rootGameObject, this.meshName);
		if (gameObject != null)
		{
			foreach (float num in this.visibilities.Keys)
			{
				float time = num;
				clip.AddEvent(new AnimationEvent
				{
					time = time,
					functionName = "HandleAnimationEvent",
					stringParameter = this.eventName,
					objectReferenceParameter = gameObject
				});
			}
		}
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x000BD02C File Offset: 0x000BB22C
	public void InitializeWithData(Dictionary<string, object> dict)
	{
		this.eventName = (string)dict["name"];
		this.meshName = (string)dict["mesh"];
		this.visibilities = new Dictionary<float, bool>();
		List<object> list = (List<object>)dict["key_frames"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			this.visibilities.Add(TFUtils.LoadFloat(dictionary, "time"), (bool)dictionary["visible"]);
		}
	}

	// Token: 0x0400130B RID: 4875
	private string eventName;

	// Token: 0x0400130C RID: 4876
	private string meshName;

	// Token: 0x0400130D RID: 4877
	private Dictionary<float, bool> visibilities;
}
