using System;
using System.Collections.Generic;

// Token: 0x02000130 RID: 304
public class ConstantCondition : LoadableCondition
{
	// Token: 0x06000AF7 RID: 2807 RVA: 0x00043D14 File Offset: 0x00041F14
	private ConstantCondition()
	{
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x00043D1C File Offset: 0x00041F1C
	public ConstantCondition(uint id, bool val)
	{
		base.Initialize(id, 1U, "constant", new List<string>(), new List<uint>());
		this.val = val;
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00043D50 File Offset: 0x00041F50
	public bool Value
	{
		get
		{
			return this.val;
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00043D58 File Offset: 0x00041F58
	public static ConstantCondition FromDict(Dictionary<string, object> dict)
	{
		int num = TFUtils.LoadInt(dict, "value");
		bool flag = num != 0;
		ConstantCondition constantCondition = new ConstantCondition();
		constantCondition.Parse(dict, "constant", new List<string>(), flag);
		return constantCondition;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x00043D98 File Offset: 0x00041F98
	public override string Description(Game game)
	{
		return this.Value.ToString();
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x00043DB4 File Offset: 0x00041FB4
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["value"] = ((!this.val) ? 0 : 1);
		return dictionary;
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x00043DEC File Offset: 0x00041FEC
	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICollection<string> relevantTypes, bool val)
	{
		base.Parse(loadedData, loadToken, relevantTypes);
		this.val = val;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x00043E00 File Offset: 0x00042000
	public override void Evaluate(ConditionState state, Game game, ITrigger trigger)
	{
		state.SelfExam = ((!this.val) ? ConditionResult.FAIL : ConditionResult.PASS);
	}

	// Token: 0x04000769 RID: 1897
	public const string LOAD_TOKEN = "constant";

	// Token: 0x0400076A RID: 1898
	private bool val;
}
