using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003ED RID: 1005
public class AnimationEventTilingNode : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	// Token: 0x06001EAC RID: 7852 RVA: 0x000BCD20 File Offset: 0x000BAF20
	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		GameObject gameObject = (GameObject)animationEvent.objectReferenceParameter;
		Vector2 offset = this.offsets[animationEvent.time];
		SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
		component.material.SetTextureOffset("_MainTex", offset);
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x000BCD64 File Offset: 0x000BAF64
	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
		GameObject gameObject = TFUtils.FindGameObjectInHierarchy(rootGameObject, this.boneName);
		if (gameObject != null)
		{
			SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
			component.material.SetTextureScale("_MainTex", this.tiling);
			foreach (float num in this.offsets.Keys)
			{
				float time = num;
				clip.AddEvent(new AnimationEvent
				{
					time = time,
					functionName = "HandleAnimationEvent",
					stringParameter = this.boneName,
					objectReferenceParameter = gameObject
				});
			}
		}
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x000BCE38 File Offset: 0x000BB038
	private Dictionary<float, Vector2> InitializeTilingOffsets(List<object> offsets)
	{
		Dictionary<float, Vector2> dictionary = new Dictionary<float, Vector2>();
		foreach (object obj in offsets)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			float key = TFUtils.LoadFloat(dictionary2, "time");
			Vector2 value;
			TFUtils.LoadVector2(out value, (Dictionary<string, object>)dictionary2["offset"]);
			dictionary.Add(key, value);
		}
		return dictionary;
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x000BCED0 File Offset: 0x000BB0D0
	public void InitializeWithData(Dictionary<string, object> dict)
	{
		this.boneName = (string)dict["name"];
		TFUtils.LoadVector2(out this.tiling, (Dictionary<string, object>)dict["tiling"]);
		this.offsets = this.InitializeTilingOffsets((List<object>)dict["key_frames"]);
	}

	// Token: 0x04001308 RID: 4872
	private string boneName;

	// Token: 0x04001309 RID: 4873
	private Vector2 tiling;

	// Token: 0x0400130A RID: 4874
	private Dictionary<float, Vector2> offsets;
}
