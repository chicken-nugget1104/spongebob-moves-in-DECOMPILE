using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x02000428 RID: 1064
public class ParticleSystemManager : IComparer<ParticleSystemManager.Request>
{
	// Token: 0x0600211D RID: 8477 RVA: 0x000CC5EC File Offset: 0x000CA7EC
	public ParticleSystemManager()
	{
		string text = CommonUtils.TextureForDeviceOverride("particle_effects");
		string streamingAssetsFile;
		if (text != "particle_effects")
		{
			streamingAssetsFile = TFUtils.GetStreamingAssetsFile(text);
		}
		else
		{
			string text2 = string.Empty;
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				text2 += "_lr2";
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				text2 += "_lr";
			}
			streamingAssetsFile = TFUtils.GetStreamingAssetsFile(string.Format("{0}{1}.json", "particle_effects", text2));
		}
		string json = TFUtils.ReadAllText(streamingAssetsFile);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		int num = TFUtils.LoadInt(dictionary, "MAX_REQUEST_INSTANCES", 100);
		this.mDisableInstanceAssert = TFUtils.LoadBool(dictionary, "DISABLE_INSTANCE_ASSERT", false);
		this.requestPool = new List<ParticleSystemManager.Request>(num);
		for (int i = 0; i < num; i++)
		{
			this.requestPool.Add(new ParticleSystemManager.Request());
		}
		this.particleSystemPools = new Dictionary<string, List<GameObject>>();
		this.waitingRequests = new Dictionary<string, List<ParticleSystemManager.Request>>();
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["EFFECTS"];
		this.ParticleEffects = new string[dictionary2.Count];
		int num2 = 0;
		foreach (string text3 in dictionary2.Keys)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)dictionary2[text3];
			int maxCount = TFUtils.LoadInt(data, "MAX", 1);
			string text4 = TFUtils.LoadString(data, "PATH");
			if (string.IsNullOrEmpty(text4))
			{
				text4 = text3;
			}
			this.ParticleEffects[num2] = text3;
			num2++;
			this.particleSystemPools.Add(text3, this.MakeSystemPool(text4, maxCount));
			this.waitingRequests.Add(text3, new List<ParticleSystemManager.Request>());
		}
		this.servicingRequests = new List<ParticleSystemManager.Request>();
		this.updateWaitAction = new Action<ParticleSystemManager.Request>(this.UpdateWaitingRequest);
		this.updateServiceAction = new Action<ParticleSystemManager.Request>(this.UpdateServicingRequest);
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x000CC830 File Offset: 0x000CAA30
	protected List<GameObject> MakeSystemPool(string effectsName, int maxCount)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < maxCount; i++)
		{
			GameObject gameObject = UnityGameResources.Create(effectsName);
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.enableEmission = false;
			}
			else
			{
				ParticleEmitter component2 = gameObject.GetComponent<ParticleEmitter>();
				component2.emit = false;
			}
			list.Add(gameObject);
		}
		return list;
	}

	// Token: 0x0600211F RID: 8479 RVA: 0x000CC894 File Offset: 0x000CAA94
	private void ReleaseParticlesWithRequest(ParticleSystemManager.Request r)
	{
		if (r.particleSystemGameObject != null)
		{
			ParticleSystem component = r.particleSystemGameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.enableEmission = false;
				component.Stop();
			}
			else
			{
				ParticleEmitter component2 = r.particleSystemGameObject.GetComponent<ParticleEmitter>();
				component2.emit = false;
			}
			this.particleSystemPools[r.effectsName].Add(r.particleSystemGameObject);
			r.particleSystemGameObject.transform.parent = null;
			r.particleSystemGameObject = null;
		}
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x000CC924 File Offset: 0x000CAB24
	public void RemoveRequestWithDelegate(ParticleSystemManager.Request.IDelegate d)
	{
		foreach (ParticleSystemManager.Request request in this.servicingRequests)
		{
			if (request.clientDelegate == d)
			{
				this.servicingRequests.Remove(request);
				this.ReleaseParticlesWithRequest(request);
				return;
			}
		}
		foreach (string key in this.waitingRequests.Keys)
		{
			List<ParticleSystemManager.Request> list = this.waitingRequests[key];
			foreach (ParticleSystemManager.Request request2 in list)
			{
				if (request2.clientDelegate == d)
				{
					list.Remove(request2);
					this.ReleaseParticlesWithRequest(request2);
					return;
				}
			}
		}
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000CCA7C File Offset: 0x000CAC7C
	public ParticleSystemManager.Request RequestParticles(string effectsName, int initialPriority, int subsequentPriority, float cyclingPeriod, ParticleSystemManager.Request.IDelegate requestDelegate)
	{
		TFUtils.Assert(requestDelegate != null, "RequestParticles received null requestDelegate. This will cause problems when the request is processed.\nEffectRequestName=" + effectsName);
		if (!this.mDisableInstanceAssert)
		{
			TFUtils.Assert(this.requestPool.Count > 0, "RequestParticles ran out of requests");
		}
		if (this.requestPool.Count <= 0 || TFPerfUtils.IsNonParticleDevice())
		{
			return null;
		}
		ParticleSystemManager.Request request = this.requestPool[0];
		request.effectsName = effectsName;
		request.initialPriority = initialPriority;
		request.subsequentPriority = subsequentPriority;
		request.cyclingPeriod = cyclingPeriod;
		request.clientDelegate = requestDelegate;
		this.requestPool.RemoveAt(0);
		request.Init(true, ParticleSystemManager.Request.State.STATE_WAIT);
		this.waitingRequests[request.effectsName].Add(request);
		return request;
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000CCB40 File Offset: 0x000CAD40
	public bool RemoveParticleSystemRequest(ParticleSystemManager.Request request)
	{
		bool flag = false;
		TFUtils.Assert(request != null, "RemoveParticleSystemRequest(): null request parameter");
		TFUtils.Assert(request.effectsName != null, "RemoveParticleSystemRequest(): request has null effectsName");
		if (this.servicingRequests.Contains(request))
		{
			flag = this.servicingRequests.Remove(request);
			if (flag && request.particleSystemGameObject != null)
			{
				this.ReleaseParticlesWithRequest(request);
			}
			this.requestPool.Add(request);
		}
		else if (this.waitingRequests[request.effectsName].Contains(request))
		{
			flag = this.waitingRequests[request.effectsName].Remove(request);
			this.requestPool.Add(request);
		}
		return flag;
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000CCC04 File Offset: 0x000CAE04
	public int Compare(ParticleSystemManager.Request a, ParticleSystemManager.Request b)
	{
		if (a.clientDelegate.isVisible && !b.clientDelegate.isVisible)
		{
			return -1;
		}
		if (!a.clientDelegate.isVisible && b.clientDelegate.isVisible)
		{
			return 1;
		}
		if (a.FirstService && !b.FirstService)
		{
			return -1;
		}
		if (!a.FirstService && b.FirstService)
		{
			return 1;
		}
		if (a.FirstService && b.FirstService)
		{
			if (a.initialPriority > b.initialPriority)
			{
				return -1;
			}
			if (a.initialPriority < b.initialPriority)
			{
				return 1;
			}
		}
		if (a.subsequentPriority > b.subsequentPriority)
		{
			return -1;
		}
		if (a.subsequentPriority < b.subsequentPriority)
		{
			return 1;
		}
		if (a.elapsedTime > b.elapsedTime)
		{
			return -1;
		}
		if (a.elapsedTime < b.elapsedTime)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000CCD10 File Offset: 0x000CAF10
	private void ServiceWaitingRequests(string effectsName)
	{
		this.ServiceWaitingRequests(effectsName, this.particleSystemPools[effectsName], this.waitingRequests[effectsName]);
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000CCD3C File Offset: 0x000CAF3C
	private void ServiceWaitingRequests(string effectsName, List<GameObject> particleEffectPool, List<ParticleSystemManager.Request> requests)
	{
		requests.Sort(this);
		int num = Math.Min(particleEffectPool.Count, requests.Count);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			ParticleSystemManager.Request request = requests[i];
			TFUtils.Assert(request.clientDelegate != null, "Cannot process unassigned client deligate for request(" + request.effectsName + ")");
			if (!request.clientDelegate.isVisible)
			{
				break;
			}
			num2++;
			GameObject gameObject = particleEffectPool[i];
			request.particleSystemGameObject = gameObject;
			ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
			if (component != null)
			{
				component.enableEmission = true;
				component.Play();
			}
			else
			{
				ParticleEmitter component2 = gameObject.GetComponent<ParticleEmitter>();
				component2.emit = true;
				component2.enabled = true;
			}
			if (request.clientDelegate.ParentTransform != null)
			{
				request.particleSystemGameObject.transform.parent = request.clientDelegate.ParentTransform;
				request.particleSystemGameObject.transform.localPosition = request.clientDelegate.Position;
				request.particleSystemGameObject.transform.localRotation = Quaternion.identity;
				request.particleSystemGameObject.transform.localScale = Vector3.one;
				GameObject gameObject2 = TFUtils.FindParentGameObjectInHierarchy(request.particleSystemGameObject, "BN_ROOT");
				if (gameObject2 != null && gameObject2.transform.parent != null)
				{
					GameObject gameObject3 = gameObject2.transform.parent.gameObject;
					if (gameObject3.transform.localScale.x < 0f)
					{
						if (component != null && component.startSpeed > 0f)
						{
							component.startSpeed *= -1f;
						}
					}
					else if (component.startSpeed < 0f)
					{
						component.startSpeed = Math.Abs(component.startSpeed);
					}
				}
			}
			else
			{
				request.particleSystemGameObject.transform.position = request.clientDelegate.Position;
			}
			request.Init(false, ParticleSystemManager.Request.State.STATE_PLAY);
			this.servicingRequests.Add(request);
		}
		if (num2 > 0)
		{
			this.waitingRequests[effectsName].RemoveRange(0, num2);
			this.particleSystemPools[effectsName].RemoveRange(0, num2);
		}
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x000CCFAC File Offset: 0x000CB1AC
	private void UpdateServicingRequest(ParticleSystemManager.Request r)
	{
		r.elapsedTime += Time.deltaTime;
		ParticleSystem component = r.particleSystemGameObject.GetComponent<ParticleSystem>();
		if (component != null && !component.loop)
		{
			if (r.elapsedTime > component.duration)
			{
				this.ReleaseParticlesWithRequest(r);
				this.servicingRequests.Remove(r);
				r.Init(false, ParticleSystemManager.Request.State.STATE_NONE);
				this.requestPool.Add(r);
			}
		}
		else if (r.elapsedTime > r.cyclingPeriod || !r.clientDelegate.isVisible)
		{
			this.ReleaseParticlesWithRequest(r);
			this.servicingRequests.Remove(r);
			r.Init(false, ParticleSystemManager.Request.State.STATE_WAIT);
			this.waitingRequests[r.effectsName].Add(r);
		}
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000CD080 File Offset: 0x000CB280
	private void UpdateWaitingRequest(ParticleSystemManager.Request r)
	{
		r.elapsedTime += Time.deltaTime;
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000CD094 File Offset: 0x000CB294
	public void Update(string effectsName)
	{
		List<ParticleSystemManager.Request> list = this.waitingRequests[effectsName];
		list.ForEach(this.updateWaitAction);
		List<GameObject> list2 = this.particleSystemPools[effectsName];
		if (list2.Count > 0 && list.Count > 0)
		{
			this.ServiceWaitingRequests(effectsName, list2, list);
		}
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000CD0E8 File Offset: 0x000CB2E8
	public void OnUpdate()
	{
		this.servicingRequests.ForEach(this.updateServiceAction);
		foreach (string effectsName in this.ParticleEffects)
		{
			this.Update(effectsName);
		}
	}

	// Token: 0x04001437 RID: 5175
	private const float PLAY_CYCLING_TIME = 1f;

	// Token: 0x04001438 RID: 5176
	private const int DEFAULT_PRIORITY = 0;

	// Token: 0x04001439 RID: 5177
	public const string kBubbleChimneyPrefab = "Prefabs/FX/Fx_Bubble_Chimney";

	// Token: 0x0400143A RID: 5178
	public const string kBubbleScreenWipePrefab = "Prefabs/FX/Fx_Bubble_Screen_Wipe";

	// Token: 0x0400143B RID: 5179
	public const string kBubblePopPrefab = "Prefabs/FX/Fx_Bubble_Pop";

	// Token: 0x0400143C RID: 5180
	public const string kThoughtBubblePopPrefab = "Prefabs/FX/Fx_Bubble_Thought_Pop";

	// Token: 0x0400143D RID: 5181
	public const string kEatPrefab = "Prefabs/FX/Fx_Food_Crumbs";

	// Token: 0x0400143E RID: 5182
	public const string kBubbleBuildingPopPrefab = "Prefabs/FX/Fx_Bubble_Building_Pop";

	// Token: 0x0400143F RID: 5183
	public const string kConstructionSmokePrefab = "Prefabs/FX/Fx_Construction_Smoke";

	// Token: 0x04001440 RID: 5184
	public const string kConstructionStarsPrefab = "Prefabs/FX/Fx_Construction_Stars";

	// Token: 0x04001441 RID: 5185
	public const string kTreasureStarsPrefab = "Prefabs/FX/Fx_Sparkles_Rising2";

	// Token: 0x04001442 RID: 5186
	public const string kConfettiSquareScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Squares";

	// Token: 0x04001443 RID: 5187
	public const string kConfettiSquigglesScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Squiggles";

	// Token: 0x04001444 RID: 5188
	public const string kBalloon1ScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Balloons_01";

	// Token: 0x04001445 RID: 5189
	public const string kBalloon2ScreenWipePrefab = "Prefabs/FX/Fx_Confetti_Balloons_02";

	// Token: 0x04001446 RID: 5190
	public const string kSeaFlowersScreenWipePrefab = "Prefabs/FX/Fx_Seaflowers_Quest_Complete";

	// Token: 0x04001447 RID: 5191
	public const string kBubble2ScreenWipePrefab = "Prefabs/FX/Fx_Bubble_Quest_Complete";

	// Token: 0x04001448 RID: 5192
	public const string BUBBLE_CLICK_PREFAB = "Prefabs/FX/Fx_Bubble_Click";

	// Token: 0x04001449 RID: 5193
	public const string TAP_COIN_SHOWER_PREFAB = "Prefabs/FX/Fx_Coin_Shower";

	// Token: 0x0400144A RID: 5194
	public const string TAP_JELLY_SHOWER_PREFAB = "Prefabs/FX/Fx_Jelly_Shower";

	// Token: 0x0400144B RID: 5195
	public const string TAP_GLASS_BREAK_PREFAB = "Prefabs/FX/Fx_Glass_Break";

	// Token: 0x0400144C RID: 5196
	public const string TAP_FILM_ROLL_PREFAB = "Prefabs/FX/Fx_Film_Roll";

	// Token: 0x0400144D RID: 5197
	public const string FOG1_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog1_Drift";

	// Token: 0x0400144E RID: 5198
	public const string FOG2_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog2_Drift";

	// Token: 0x0400144F RID: 5199
	public const string FOG3_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog3_Drift";

	// Token: 0x04001450 RID: 5200
	public const string FOG4_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog4_Drift";

	// Token: 0x04001451 RID: 5201
	public const string FOG5_DRIFT_PREFAB = "Prefabs/FX/Fx_Fog5_Drift";

	// Token: 0x04001452 RID: 5202
	private string[] ParticleEffects = new string[0];

	// Token: 0x04001453 RID: 5203
	private bool mDisableInstanceAssert;

	// Token: 0x04001454 RID: 5204
	private List<ParticleSystemManager.Request> requestPool;

	// Token: 0x04001455 RID: 5205
	private List<ParticleSystemManager.Request> servicingRequests;

	// Token: 0x04001456 RID: 5206
	private Dictionary<string, List<GameObject>> particleSystemPools;

	// Token: 0x04001457 RID: 5207
	private Dictionary<string, List<ParticleSystemManager.Request>> waitingRequests;

	// Token: 0x04001458 RID: 5208
	private Action<ParticleSystemManager.Request> updateWaitAction;

	// Token: 0x04001459 RID: 5209
	private Action<ParticleSystemManager.Request> updateServiceAction;

	// Token: 0x02000429 RID: 1065
	public class Request
	{
		// Token: 0x0600212A RID: 8490 RVA: 0x000CD12C File Offset: 0x000CB32C
		public Request()
		{
			this.effectsName = "Prefabs/FX/Fx_Bubble_Chimney";
			this.initialPriority = 0;
			this.subsequentPriority = 0;
			this.cyclingPeriod = 1f;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000CD164 File Offset: 0x000CB364
		public void Init(bool firstService, ParticleSystemManager.Request.State state)
		{
			this.state = state;
			this.firstService = firstService;
			this.elapsedTime = 0f;
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x000CD180 File Offset: 0x000CB380
		public bool FirstService
		{
			get
			{
				return this.firstService;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x000CD188 File Offset: 0x000CB388
		public ParticleSystemManager.Request.State CurrentState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x0400145A RID: 5210
		public string effectsName;

		// Token: 0x0400145B RID: 5211
		public ParticleSystemManager.Request.IDelegate clientDelegate;

		// Token: 0x0400145C RID: 5212
		public int initialPriority;

		// Token: 0x0400145D RID: 5213
		public int subsequentPriority;

		// Token: 0x0400145E RID: 5214
		public float cyclingPeriod;

		// Token: 0x0400145F RID: 5215
		public float elapsedTime;

		// Token: 0x04001460 RID: 5216
		public GameObject particleSystemGameObject;

		// Token: 0x04001461 RID: 5217
		private bool firstService;

		// Token: 0x04001462 RID: 5218
		private ParticleSystemManager.Request.State state;

		// Token: 0x0200042A RID: 1066
		public interface IDelegate
		{
			// Token: 0x170004D9 RID: 1241
			// (get) Token: 0x0600212E RID: 8494
			Transform ParentTransform { get; }

			// Token: 0x170004DA RID: 1242
			// (get) Token: 0x0600212F RID: 8495
			Vector3 Position { get; }

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x06002130 RID: 8496
			bool isVisible { get; }
		}

		// Token: 0x0200042B RID: 1067
		public enum State
		{
			// Token: 0x04001464 RID: 5220
			STATE_NONE,
			// Token: 0x04001465 RID: 5221
			STATE_WAIT,
			// Token: 0x04001466 RID: 5222
			STATE_PLAY
		}
	}
}
