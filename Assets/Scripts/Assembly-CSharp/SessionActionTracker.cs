using System;
using System.Collections.Generic;

// Token: 0x0200024E RID: 590
public class SessionActionTracker
{
	// Token: 0x060012FE RID: 4862 RVA: 0x00083820 File Offset: 0x00081A20
	public SessionActionTracker(SessionActionDefinition definition) : this(definition, definition.StartConditions)
	{
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x00083830 File Offset: 0x00081A30
	public SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride) : this(definition, startConditionsOverride, null, false)
	{
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0008383C File Offset: 0x00081A3C
	public SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride, string tag, bool slave = false) : this(definition, startConditionsOverride, false, tag, slave)
	{
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0008384C File Offset: 0x00081A4C
	private SessionActionTracker(SessionActionDefinition definition, ICondition startConditionsOverride, bool manualSuccess, string tag, bool slave = false)
	{
		this.status = SessionActionTracker.StatusCode.INITIAL;
		this.definition = definition;
		this.activationProgress = new ConditionState(startConditionsOverride);
		this.successProgress = new ConditionState(definition.SucceedConditions);
		this.dynamic = new Dictionary<string, object>();
		this.definition.SetDynamicProperties(ref this.dynamic);
		this.manualSuccess = manualSuccess;
		this.tag = tag;
		this.enslaved = slave;
		this.activationProgress.Recalculate(null, Trigger.Null, null);
		this.successProgress.Recalculate(null, Trigger.Null, null);
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06001302 RID: 4866 RVA: 0x000838E4 File Offset: 0x00081AE4
	public SessionActionDefinition Definition
	{
		get
		{
			return this.definition;
		}
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x000838EC File Offset: 0x00081AEC
	public T GetDefinition<T>()
	{
		return (T)((object)Convert.ChangeType(this.definition, typeof(T)));
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06001304 RID: 4868 RVA: 0x00083908 File Offset: 0x00081B08
	public ConditionState ActivationProgress
	{
		get
		{
			return this.activationProgress;
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06001305 RID: 4869 RVA: 0x00083910 File Offset: 0x00081B10
	public ConditionState SuccessProgress
	{
		get
		{
			return this.successProgress;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06001306 RID: 4870 RVA: 0x00083918 File Offset: 0x00081B18
	public bool ManualSuccess
	{
		get
		{
			return this.manualSuccess;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06001307 RID: 4871 RVA: 0x00083920 File Offset: 0x00081B20
	// (set) Token: 0x06001308 RID: 4872 RVA: 0x00083928 File Offset: 0x00081B28
	public string Tag
	{
		get
		{
			return this.tag;
		}
		set
		{
			this.tag = value;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06001309 RID: 4873 RVA: 0x00083934 File Offset: 0x00081B34
	public SessionActionTracker Slave
	{
		get
		{
			return this.slave;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x0600130A RID: 4874 RVA: 0x0008393C File Offset: 0x00081B3C
	public SessionActionTracker.StatusCode Status
	{
		get
		{
			if (this.status == SessionActionTracker.StatusCode.REQUESTED || this.status == SessionActionTracker.StatusCode.STARTED)
			{
				ConditionResult conditionResult = this.successProgress.Examine();
				if (conditionResult == ConditionResult.PASS)
				{
					this.status = SessionActionTracker.StatusCode.FINISHED_SUCCESS;
				}
				else if (conditionResult == ConditionResult.FAIL)
				{
					if (this.definition.IsFailproof)
					{
						this.status = SessionActionTracker.StatusCode.FINISHED_SUCCESS;
					}
					else
					{
						this.status = SessionActionTracker.StatusCode.FINISHED_FAILURE;
					}
				}
			}
			return this.status;
		}
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x000839B0 File Offset: 0x00081BB0
	public T GetDynamic<T>(string key)
	{
		TFUtils.Assert(this.dynamic.ContainsKey(key), "Cannot lookup a key that does not exist. Did you forget to call SessionActionDefinition.SetDynamicProperties?");
		return (T)((object)this.dynamic[key]);
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x000839DC File Offset: 0x00081BDC
	public void SetDynamic(string key, object val)
	{
		TFUtils.Assert(this.dynamic.ContainsKey(key), "Cannot set a key that does not exist. Did you forget to call SessionActionDefinition.SetDynamicProperties?");
		this.dynamic[key] = val;
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x00083A04 File Offset: 0x00081C04
	public void MarkPostponed()
	{
		this.AssertNotObliterated();
		if (this.status != SessionActionTracker.StatusCode.INITIAL)
		{
			TFUtils.Assert(this.status == SessionActionTracker.StatusCode.INITIAL, "Trying to mark a session action tracker to the POSTPONED state, but it is not in the INITIAL state. Did something else already request it?\naction=" + this);
		}
		this.status = SessionActionTracker.StatusCode.POSTPONED;
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x00083A38 File Offset: 0x00081C38
	public void MarkRequested()
	{
		this.AssertNotObliterated();
		if (this.status != SessionActionTracker.StatusCode.INITIAL && this.status != SessionActionTracker.StatusCode.POSTPONED)
		{
			TFUtils.Assert(this.status == SessionActionTracker.StatusCode.INITIAL || this.status == SessionActionTracker.StatusCode.POSTPONED, "Trying to mark a session action tracker to the REQUESTED state, but it is not in the INITIAL or POSTPONED state. Did something else already request it?\naction=" + this);
		}
		this.status = SessionActionTracker.StatusCode.REQUESTED;
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x00083A90 File Offset: 0x00081C90
	public void MarkStarted()
	{
		this.AssertNotObliterated();
		if (this.status != SessionActionTracker.StatusCode.REQUESTED)
		{
			TFUtils.Assert(this.status == SessionActionTracker.StatusCode.REQUESTED, "Trying to advance a session action to the STARTED state, but it is not in the REQUESTED state. Did something else already start it?\naction=" + this);
		}
		this.status = SessionActionTracker.StatusCode.STARTED;
		if (this.slave != null)
		{
			this.slave.activationProgress.Recalculate(null, DumbCondition.PASS_TRIGGER, null);
		}
	}

	// Token: 0x06001310 RID: 4880 RVA: 0x00083AF4 File Offset: 0x00081CF4
	public void MarkObliterated(Game game)
	{
		this.status = SessionActionTracker.StatusCode.OBLITERATED;
		this.definition.OnObliterate(game, this);
		if (this.slave != null)
		{
			this.slave.MarkObliterated(game);
		}
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x00083B24 File Offset: 0x00081D24
	public void MarkSucceeded()
	{
		this.MarkSucceeded(true);
		if (this.slave != null && this.slave.status == SessionActionTracker.StatusCode.STARTED)
		{
			this.slave.MarkSucceeded();
		}
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x00083B60 File Offset: 0x00081D60
	public void MarkSucceeded(bool failIfObliterated)
	{
		if (failIfObliterated)
		{
			this.AssertNotObliterated();
		}
		if (this.status == SessionActionTracker.StatusCode.FINISHED_FAILURE)
		{
			TFUtils.Assert(false, "Shouldn't try to succeed a session action that has already failed.\naction=" + this);
		}
		this.RecalculateProgress(DumbCondition.PASS_TRIGGER);
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x00083BA4 File Offset: 0x00081DA4
	public void MarkFailed()
	{
		this.AssertNotObliterated();
		TFUtils.Assert(this.status != SessionActionTracker.StatusCode.FINISHED_SUCCESS, "Shouldn't try to fail a session action that has already succeeded.");
		Trigger trigger = (!this.definition.IsFailproof) ? DumbCondition.FAIL_TRIGGER : DumbCondition.PASS_TRIGGER;
		this.RecalculateProgress(trigger);
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x00083BF4 File Offset: 0x00081DF4
	private void RecalculateProgress(Trigger trigger)
	{
		this.successProgress.Recalculate(null, trigger, null);
		for (SessionActionTracker sessionActionTracker = this.slave; sessionActionTracker != null; sessionActionTracker = sessionActionTracker.slave)
		{
			sessionActionTracker.successProgress.Recalculate(null, trigger, null);
		}
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x00083C38 File Offset: 0x00081E38
	public void ReActivate(Game game)
	{
		this.MarkObliterated(game);
		this.Definition.OnDestroy(game);
		this.didPreActivate = false;
		this.status = SessionActionTracker.StatusCode.INITIAL;
		this.slave = null;
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x00083C70 File Offset: 0x00081E70
	public void PreActivate(Game game)
	{
		if (!this.didPreActivate)
		{
			this.didPreActivate = true;
			this.definition.PreActivate(game, this);
			if (this.definition.Slave != null)
			{
				TFUtils.Assert(this.slave == null, "Each tracker may have only 1 slave. You should not be trying to replace an existing one.");
				this.slave = new SessionActionTracker(this.definition.Slave, new DumbCondition(0U), true, this.tag, true);
				game.sessionActionManager.Request(this.slave, game);
			}
		}
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x00083CF8 File Offset: 0x00081EF8
	public void PostComplete(Game game)
	{
		this.definition.PostComplete(game, this);
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x00083D08 File Offset: 0x00081F08
	public bool ActiveProcess(Game game)
	{
		return this.definition.ActiveProcess(game, this);
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00083D18 File Offset: 0x00081F18
	public void StartPostponeTimer()
	{
		this.postponeComplete = new DateTime?(DateTime.Now.AddSeconds((double)this.definition.Postpone));
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x00083D4C File Offset: 0x00081F4C
	public bool IsPostponeComplete()
	{
		return this.postponeComplete.Value.CompareTo(DateTime.Now) <= 0;
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00083D78 File Offset: 0x00081F78
	public bool ShouldSetPostponeTimer()
	{
		DateTime? dateTime = this.postponeComplete;
		return dateTime == null && this.definition.Postpone > 0f;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00083DB0 File Offset: 0x00081FB0
	private void AssertNotObliterated()
	{
		TFUtils.Assert(this.status != SessionActionTracker.StatusCode.OBLITERATED, "This tracker has been obliterated. Should not be trying to change that.");
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00083DC8 File Offset: 0x00081FC8
	public void Destroy(Game game)
	{
		this.definition.OnDestroy(game);
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x0600131E RID: 4894 RVA: 0x00083DD8 File Offset: 0x00081FD8
	public bool RepeatOnFail
	{
		get
		{
			return !this.enslaved && this.Definition.RepeatOnFail;
		}
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00083DF4 File Offset: 0x00081FF4
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"SessionActionTracker:(status=",
			this.status,
			", definition=",
			this.definition.ToString(),
			", tag=",
			this.tag,
			", activationProgress=",
			this.activationProgress.ToString(),
			", successProgress=",
			this.successProgress.ToString(),
			", dynamics=",
			TFUtils.DebugDictToString(this.dynamic),
			")"
		});
	}

	// Token: 0x04000D26 RID: 3366
	private SessionActionTracker.StatusCode status;

	// Token: 0x04000D27 RID: 3367
	private SessionActionDefinition definition;

	// Token: 0x04000D28 RID: 3368
	private ConditionState activationProgress;

	// Token: 0x04000D29 RID: 3369
	private ConditionState successProgress;

	// Token: 0x04000D2A RID: 3370
	private bool manualSuccess;

	// Token: 0x04000D2B RID: 3371
	private Dictionary<string, object> dynamic;

	// Token: 0x04000D2C RID: 3372
	private string tag;

	// Token: 0x04000D2D RID: 3373
	private bool didPreActivate;

	// Token: 0x04000D2E RID: 3374
	private SessionActionTracker slave;

	// Token: 0x04000D2F RID: 3375
	private bool enslaved;

	// Token: 0x04000D30 RID: 3376
	private DateTime? postponeComplete;

	// Token: 0x0200024F RID: 591
	public enum StatusCode
	{
		// Token: 0x04000D32 RID: 3378
		INITIAL,
		// Token: 0x04000D33 RID: 3379
		POSTPONED,
		// Token: 0x04000D34 RID: 3380
		REQUESTED,
		// Token: 0x04000D35 RID: 3381
		STARTED,
		// Token: 0x04000D36 RID: 3382
		FINISHED_SUCCESS,
		// Token: 0x04000D37 RID: 3383
		FINISHED_FAILURE,
		// Token: 0x04000D38 RID: 3384
		OBLITERATED
	}
}
