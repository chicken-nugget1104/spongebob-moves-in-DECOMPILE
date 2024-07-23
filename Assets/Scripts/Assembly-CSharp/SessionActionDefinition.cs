using System;
using System.Collections.Generic;

// Token: 0x02000246 RID: 582
public abstract class SessionActionDefinition
{
	// Token: 0x17000264 RID: 612
	// (get) Token: 0x060012BF RID: 4799 RVA: 0x00081910 File Offset: 0x0007FB10
	public uint Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x060012C0 RID: 4800 RVA: 0x00081918 File Offset: 0x0007FB18
	public string Type
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x060012C1 RID: 4801 RVA: 0x00081920 File Offset: 0x0007FB20
	public string Sound
	{
		get
		{
			return this.sound;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x060012C2 RID: 4802 RVA: 0x00081928 File Offset: 0x0007FB28
	public float Postpone
	{
		get
		{
			return this.postpone;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x060012C3 RID: 4803 RVA: 0x00081930 File Offset: 0x0007FB30
	public ICondition StartConditions
	{
		get
		{
			return this.startConditions;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x060012C4 RID: 4804 RVA: 0x00081938 File Offset: 0x0007FB38
	public ICondition SucceedConditions
	{
		get
		{
			return this.succeedConditions;
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x060012C5 RID: 4805 RVA: 0x00081940 File Offset: 0x0007FB40
	public virtual bool RepeatOnFail
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060012C6 RID: 4806 RVA: 0x00081944 File Offset: 0x0007FB44
	public virtual bool IsFailproof
	{
		get
		{
			return this.isFailproof;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0008194C File Offset: 0x0007FB4C
	public SessionActionDefinition Slave
	{
		get
		{
			return this.slave;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x060012C8 RID: 4808 RVA: 0x00081954 File Offset: 0x0007FB54
	public virtual bool ClearOnSessionChange
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x00081958 File Offset: 0x0007FB58
	public virtual Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", this.type);
		dictionary.Add("sound", this.sound);
		dictionary.Add("postpone", this.postpone);
		if (!this.usingDefaultSucceedConditions)
		{
			dictionary.Add("end_conditions", ((LoadableCondition)this.succeedConditions).ToDict());
		}
		dictionary.Add("failproof", (!this.isFailproof) ? 0 : 1);
		if (this.slave != null)
		{
			dictionary["slave"] = this.slave.ToDict();
		}
		return dictionary;
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x00081A10 File Offset: 0x0007FC10
	protected virtual void Parse(Dictionary<string, object> actionData, uint id, ICondition startConditions, ICondition defaultSuccessConditions, uint originatedFromQuest)
	{
		this.id = id;
		this.startConditions = startConditions;
		this.type = TFUtils.LoadString(actionData, "type");
		if (actionData.ContainsKey("failproof"))
		{
			this.isFailproof = (TFUtils.LoadInt(actionData, "failproof") != 0);
		}
		if (actionData.ContainsKey("sound"))
		{
			this.sound = TFUtils.LoadString(actionData, "sound");
		}
		else
		{
			this.sound = null;
		}
		this.postpone = ((!actionData.ContainsKey("postpone")) ? 0f : TFUtils.LoadFloat(actionData, "postpone"));
		if (actionData.ContainsKey("end_conditions"))
		{
			this.succeedConditions = ConditionFactory.FromDict(TFUtils.LoadDict(actionData, "end_conditions"));
			this.usingDefaultSucceedConditions = false;
		}
		else
		{
			this.succeedConditions = defaultSuccessConditions;
			this.usingDefaultSucceedConditions = true;
		}
		if (actionData.ContainsKey("slave"))
		{
			this.slave = SessionActionFactory.Create(TFUtils.LoadDict(actionData, "slave"), new DumbCondition(0U), originatedFromQuest, 0U);
		}
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x00081B34 File Offset: 0x0007FD34
	public virtual void PreActivate(Game game, SessionActionTracker action)
	{
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x00081B38 File Offset: 0x0007FD38
	public virtual bool ActiveProcess(Game game, SessionActionTracker action)
	{
		return false;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x00081B3C File Offset: 0x0007FD3C
	public virtual void PostComplete(Game game, SessionActionTracker action)
	{
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x00081B40 File Offset: 0x0007FD40
	public virtual void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x00081B44 File Offset: 0x0007FD44
	public virtual void OnObliterate(Game game, SessionActionTracker tracker)
	{
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x00081B48 File Offset: 0x0007FD48
	public virtual void OnDestroy(Game game)
	{
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x00081B4C File Offset: 0x0007FD4C
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"SessionActionDefinition:(id=",
			this.id,
			", type=",
			this.type,
			", start_conditions=",
			this.startConditions.ToString(),
			", succeed_conditions=",
			this.succeedConditions.ToString(),
			", sound=",
			this.sound,
			", postpone=",
			this.postpone,
			", failproof=",
			this.isFailproof,
			", slave=",
			this.slave,
			")"
		});
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x060012D2 RID: 4818 RVA: 0x00081C18 File Offset: 0x0007FE18
	public bool UsingDefaultSucceedConditions
	{
		get
		{
			return this.usingDefaultSucceedConditions;
		}
	}

	// Token: 0x04000CFC RID: 3324
	private const string TYPE = "type";

	// Token: 0x04000CFD RID: 3325
	private const string START_CONDITIONS = "start_conditions";

	// Token: 0x04000CFE RID: 3326
	protected const string SUCCEED_CONDITIONS = "end_conditions";

	// Token: 0x04000CFF RID: 3327
	private const string FAILPROOF = "failproof";

	// Token: 0x04000D00 RID: 3328
	private const string SOUND = "sound";

	// Token: 0x04000D01 RID: 3329
	private const string SLAVE = "slave";

	// Token: 0x04000D02 RID: 3330
	private const string POSTPONE = "postpone";

	// Token: 0x04000D03 RID: 3331
	private uint id;

	// Token: 0x04000D04 RID: 3332
	private string type;

	// Token: 0x04000D05 RID: 3333
	private ICondition startConditions;

	// Token: 0x04000D06 RID: 3334
	private ICondition succeedConditions;

	// Token: 0x04000D07 RID: 3335
	private bool usingDefaultSucceedConditions;

	// Token: 0x04000D08 RID: 3336
	private bool isFailproof;

	// Token: 0x04000D09 RID: 3337
	private string sound;

	// Token: 0x04000D0A RID: 3338
	private float postpone;

	// Token: 0x04000D0B RID: 3339
	private SessionActionDefinition slave;
}
