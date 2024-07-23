using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
public class AnimationEventManager
{
	// Token: 0x06001E97 RID: 7831 RVA: 0x000BC7B0 File Offset: 0x000BA9B0
	public AnimationEventManager()
	{
		this.animationEvents = new Dictionary<string, AnimationEventData>();
		this.particleSystemManagerDelegates = new List<AnimationEventManager.UpdateWithParticleSystemManagerDelegate>();
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x000BC7D0 File Offset: 0x000BA9D0
	public void AddAnimationEventsWithFile(string animationEventsFile)
	{
		if (!this.animationEvents.ContainsKey(animationEventsFile))
		{
			AnimationEventData animationEventData = new AnimationEventData();
			TextAsset textAsset = (TextAsset)Resources.Load(animationEventsFile, typeof(TextAsset));
			TFUtils.Assert(textAsset != null, animationEventsFile);
			Dictionary<string, object> dict = (Dictionary<string, object>)Json.Deserialize(textAsset.text);
			animationEventData.LoadAnimationEventDataWithDictionary(dict);
			this.animationEvents.Add(animationEventsFile, animationEventData);
		}
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x000BC83C File Offset: 0x000BAA3C
	public void AddAnimationEventsWithBlueprint(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("animation_events"))
		{
			this.AddAnimationEventsWithFile((string)dict["animation_events"]);
		}
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x000BC870 File Offset: 0x000BAA70
	public AnimationEventData FindAnimationEventData(string key)
	{
		AnimationEventData result = null;
		TFUtils.Assert(this.animationEvents.TryGetValue(key, out result), "AnimationEventData for " + key + " does not exist.");
		return result;
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x000BC8A4 File Offset: 0x000BAAA4
	public void Clear()
	{
		this.particleSystemManagerDelegates.Clear();
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x000BC8B4 File Offset: 0x000BAAB4
	public void RegisterParticleSystemDelegate(AnimationEventManager.UpdateWithParticleSystemManagerDelegate d)
	{
		this.particleSystemManagerDelegates.Add(d);
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000BC8C4 File Offset: 0x000BAAC4
	public void RemoveParticleSystemDelegate(AnimationEventManager.UpdateWithParticleSystemManagerDelegate d)
	{
		this.particleSystemManagerDelegates.Remove(d);
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000BC8D4 File Offset: 0x000BAAD4
	public void UpdateWithParticleSystemManager(ParticleSystemManager psm)
	{
		int count = this.particleSystemManagerDelegates.Count;
		for (int i = 0; i < count; i++)
		{
			this.particleSystemManagerDelegates[i](psm);
		}
	}

	// Token: 0x040012FC RID: 4860
	private Dictionary<string, AnimationEventData> animationEvents;

	// Token: 0x040012FD RID: 4861
	private List<AnimationEventManager.UpdateWithParticleSystemManagerDelegate> particleSystemManagerDelegates;

	// Token: 0x020004BD RID: 1213
	// (Invoke) Token: 0x06002557 RID: 9559
	public delegate void UpdateWithParticleSystemManagerDelegate(ParticleSystemManager psm);
}
