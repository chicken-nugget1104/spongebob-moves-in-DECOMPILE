using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class AnimationEventParticlesNode : AnimationEventHandlerDelegate, AnimationEventNodeDelegate
{
	// Token: 0x06001E9F RID: 7839 RVA: 0x000BC914 File Offset: 0x000BAB14
	public AnimationEventParticlesNode()
	{
		this.pendingRequestDelegates = new List<ParticleSystemManager.Request.IDelegate>();
		this.activeRequests = new Dictionary<float, ParticleSystemManager.Request>();
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x000BC934 File Offset: 0x000BAB34
	public void HandleAnimationEvent(AnimationEvent animationEvent)
	{
		if (this.activeRequests.ContainsKey(animationEvent.time))
		{
			ParticleSystemManager.Request request = this.activeRequests[animationEvent.time];
			if (request.CurrentState != ParticleSystemManager.Request.State.STATE_NONE)
			{
				return;
			}
			this.activeRequests.Remove(animationEvent.time);
		}
		AnimationEventParticlesNode.Data data = this.data[animationEvent.time];
		GameObject go = (GameObject)animationEvent.objectReferenceParameter;
		ParticleSystemManager.Request.IDelegate item = new AnimationEventParticlesNode.ParticlesDelegate(go, data);
		this.pendingRequestDelegates.Add(item);
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x000BC9BC File Offset: 0x000BABBC
	public void SetupAnimationEvents(GameObject rootGameObject, AnimationClip clip, AnimationEventManager mgr)
	{
		foreach (float num in this.data.Keys)
		{
			float num2 = num;
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.time = num2;
			animationEvent.functionName = "HandleAnimationEvent";
			AnimationEventParticlesNode.Data data = this.data[num2];
			animationEvent.stringParameter = this.nodeName;
			animationEvent.objectReferenceParameter = TFUtils.FindGameObjectInHierarchy(rootGameObject, data.bone);
			clip.AddEvent(animationEvent);
		}
		mgr.RegisterParticleSystemDelegate(new AnimationEventManager.UpdateWithParticleSystemManagerDelegate(this.UpdateWithParticleSystemManager));
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x000BCA80 File Offset: 0x000BAC80
	public void InitializeWithData(Dictionary<string, object> dict)
	{
		this.nodeName = (string)dict["name"];
		Dictionary<float, AnimationEventParticlesNode.Data> dictionary = new Dictionary<float, AnimationEventParticlesNode.Data>();
		foreach (object obj in ((List<object>)dict["key_frames"]))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			AnimationEventParticlesNode.Data data = new AnimationEventParticlesNode.Data();
			data.time = TFUtils.LoadFloat(dictionary2, "time");
			data.bone = (string)dictionary2["bone"];
			data.particles = (string)dictionary2["particles"];
			TFUtils.LoadVector3(out data.offset, (Dictionary<string, object>)dictionary2["offset"]);
			dictionary.Add(data.time, data);
		}
		this.data = dictionary;
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x000BCB88 File Offset: 0x000BAD88
	public void UpdateWithParticleSystemManager(ParticleSystemManager psm)
	{
		foreach (ParticleSystemManager.Request.IDelegate @delegate in this.pendingRequestDelegates)
		{
			AnimationEventParticlesNode.ParticlesDelegate particlesDelegate = @delegate as AnimationEventParticlesNode.ParticlesDelegate;
			if (!this.activeRequests.ContainsKey(particlesDelegate.TimeKey))
			{
				ParticleSystemManager.Request value = psm.RequestParticles(particlesDelegate.Particles, 0, 0, 0f, @delegate);
				this.activeRequests.Add(particlesDelegate.TimeKey, value);
			}
		}
		this.pendingRequestDelegates.Clear();
	}

	// Token: 0x040012FE RID: 4862
	public string nodeName;

	// Token: 0x040012FF RID: 4863
	public Dictionary<float, AnimationEventParticlesNode.Data> data;

	// Token: 0x04001300 RID: 4864
	public List<ParticleSystemManager.Request.IDelegate> pendingRequestDelegates;

	// Token: 0x04001301 RID: 4865
	public Dictionary<float, ParticleSystemManager.Request> activeRequests;

	// Token: 0x020003EB RID: 1003
	public class Data
	{
		// Token: 0x04001302 RID: 4866
		public float time;

		// Token: 0x04001303 RID: 4867
		public string bone;

		// Token: 0x04001304 RID: 4868
		public string particles;

		// Token: 0x04001305 RID: 4869
		public Vector3 offset;
	}

	// Token: 0x020003EC RID: 1004
	public class ParticlesDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06001EA5 RID: 7845 RVA: 0x000BCC40 File Offset: 0x000BAE40
		public ParticlesDelegate(GameObject go, AnimationEventParticlesNode.Data data)
		{
			this.gameObject = go;
			this.data = data;
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x000BCC58 File Offset: 0x000BAE58
		public Transform ParentTransform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001EA7 RID: 7847 RVA: 0x000BCC68 File Offset: 0x000BAE68
		public Vector3 Position
		{
			get
			{
				return this.data.offset;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x000BCC78 File Offset: 0x000BAE78
		public bool isVisible
		{
			get
			{
				if (this.gameObject.renderer != null)
				{
					return this.gameObject.renderer.isVisible;
				}
				foreach (Animation animation in this.gameObject.transform.root.gameObject.GetComponentsInChildren<Animation>())
				{
					if (animation.gameObject.GetComponentInChildren<Renderer>().isVisible)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x000BCCF8 File Offset: 0x000BAEF8
		public string Particles
		{
			get
			{
				return this.data.particles;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x000BCD08 File Offset: 0x000BAF08
		public float TimeKey
		{
			get
			{
				return this.data.time;
			}
		}

		// Token: 0x04001306 RID: 4870
		private GameObject gameObject;

		// Token: 0x04001307 RID: 4871
		private AnimationEventParticlesNode.Data data;
	}
}
