using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class AnimationEventData : AnimationEventHandlerDelegate
{
	// Token: 0x06001E91 RID: 7825 RVA: 0x000BC514 File Offset: 0x000BA714
	public AnimationEventData()
	{
		this.eventDict = new Dictionary<string, AnimationEventNodeDelegate>();
		this.handlerDict = new Dictionary<string, AnimationEventHandlerDelegate>();
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x000BC534 File Offset: 0x000BA734
	public void LoadAnimationEventDataWithDictionary(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("nodes"))
		{
			List<object> list = (List<object>)dict["nodes"];
			foreach (object obj in list)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
				string text = (string)dictionary["type"];
				string key = (string)dictionary["name"];
				if (text.Equals("tiling_offset"))
				{
					AnimationEventTilingNode animationEventTilingNode = new AnimationEventTilingNode();
					animationEventTilingNode.InitializeWithData(dictionary);
					this.eventDict.Add(key, animationEventTilingNode);
					this.handlerDict.Add(key, animationEventTilingNode);
				}
				else if (text.Equals("particles"))
				{
					AnimationEventParticlesNode animationEventParticlesNode = new AnimationEventParticlesNode();
					animationEventParticlesNode.InitializeWithData(dictionary);
					this.eventDict.Add(key, animationEventParticlesNode);
					this.handlerDict.Add(key, animationEventParticlesNode);
				}
				else
				{
					if (!text.Equals("visibility"))
					{
						throw new NotImplementedException(string.Format("animation event type {0} not implemented", text));
					}
					AnimationEventVisibility animationEventVisibility = new AnimationEventVisibility();
					animationEventVisibility.InitializeWithData(dictionary);
					this.eventDict.Add(key, animationEventVisibility);
					this.handlerDict.Add(key, animationEventVisibility);
				}
			}
		}
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x000BC6B8 File Offset: 0x000BA8B8
	public void SetupAnimationEvents(GameObject rootGameObject, Animation unityAnimation, AnimationClip clip, AnimationEventManager mgr)
	{
		AnimationEventHandlerComponent animationEventHandlerComponent = unityAnimation.gameObject.GetComponent<AnimationEventHandlerComponent>();
		if (animationEventHandlerComponent == null)
		{
			animationEventHandlerComponent = unityAnimation.gameObject.AddComponent<AnimationEventHandlerComponent>();
		}
		animationEventHandlerComponent.animationEventHandlerDelegate = this;
		foreach (string key in this.eventDict.Keys)
		{
			AnimationEventNodeDelegate animationEventNodeDelegate = this.eventDict[key];
			animationEventNodeDelegate.SetupAnimationEvents(rootGameObject, clip, mgr);
		}
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x000BC760 File Offset: 0x000BA960
	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		AnimationEventHandlerDelegate animationEventHandlerDelegate = this.handlerDict[animationEvent.stringParameter];
		if (animationEventHandlerDelegate != null)
		{
			animationEventHandlerDelegate.HandleAnimationEvent(animationEvent);
		}
	}

	// Token: 0x040012F9 RID: 4857
	private Dictionary<string, AnimationEventNodeDelegate> eventDict;

	// Token: 0x040012FA RID: 4858
	private Dictionary<string, AnimationEventHandlerDelegate> handlerDict;
}
