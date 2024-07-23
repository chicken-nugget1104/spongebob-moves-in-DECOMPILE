using System;
using System.Collections.Generic;

// Token: 0x0200013C RID: 316
public abstract class LoadableCondition : BaseCondition
{
	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00044490 File Offset: 0x00042690
	protected string LoadTokenType
	{
		get
		{
			return this.loadTokenType;
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00044498 File Offset: 0x00042698
	protected void Parse(Dictionary<string, object> loadedData, string loadToken, ICollection<string> relevantTypes)
	{
		uint? num = TFUtils.TryLoadUint(loadedData, "count");
		if (num == null)
		{
			num = 1U;
			this.hasCountField = false;
		}
		List<uint> prerequisiteConditions = new List<uint>();
		if (loadedData.ContainsKey("prerequisite_conditions"))
		{
			prerequisiteConditions = TFUtils.LoadList<uint>(loadedData, "prerequisite_conditions");
		}
		this.Initialize(TFUtils.LoadUint(loadedData, "id"), num.Value, loadToken, relevantTypes, prerequisiteConditions);
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00044508 File Offset: 0x00042708
	protected void Initialize(uint id, uint count, string loadToken, ICollection<string> relevantTypes, IList<uint> prerequisiteConditions)
	{
		base.Initialize(id, count, relevantTypes, prerequisiteConditions);
		this.loadTokenType = loadToken;
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x00044520 File Offset: 0x00042720
	public virtual Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = this.loadTokenType;
		dictionary["id"] = this.Id;
		dictionary["count"] = this.Count;
		return dictionary;
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x00044574 File Offset: 0x00042774
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"LoadableCondition:(loadTokenType=",
			this.loadTokenType,
			", ",
			base.ToString(),
			")"
		});
	}

	// Token: 0x0400077D RID: 1917
	private string loadTokenType;

	// Token: 0x0400077E RID: 1918
	public bool hasCountField = true;
}
