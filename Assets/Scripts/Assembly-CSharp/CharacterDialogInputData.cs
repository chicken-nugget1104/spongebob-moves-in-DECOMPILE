using System;
using System.Collections.Generic;

// Token: 0x0200015D RID: 349
public class CharacterDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000BFC RID: 3068 RVA: 0x00048670 File Offset: 0x00046870
	public CharacterDialogInputData(uint sequenceId, Dictionary<string, object> promptData) : this(sequenceId, new List<object>
	{
		promptData
	})
	{
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x00048694 File Offset: 0x00046894
	public CharacterDialogInputData(uint sequenceId, List<object> promptsData) : base(sequenceId, "character", null, null)
	{
		this.promptsData = promptsData;
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000BFE RID: 3070 RVA: 0x000486AC File Offset: 0x000468AC
	public List<object> PromptsData
	{
		get
		{
			return this.promptsData;
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x000486B4 File Offset: 0x000468B4
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref dictionary, "character");
		dictionary["sequence_id"] = base.SequenceId;
		dictionary["prompts"] = this.promptsData;
		return dictionary;
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x000486FC File Offset: 0x000468FC
	public new static CharacterDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint sequenceId = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			sequenceId = TFUtils.LoadUint(dict, "sequence_id");
		}
		List<object> list = (List<object>)dict["prompts"];
		return new CharacterDialogInputData(sequenceId, list);
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x00048740 File Offset: 0x00046940
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"CharacterDialogInputData(prompts=",
			TFUtils.DebugListToString(this.promptsData),
			",",
			base.ToString(),
			")"
		});
	}

	// Token: 0x0400080A RID: 2058
	public const string DIALOG_TYPE = "character";

	// Token: 0x0400080B RID: 2059
	private List<object> promptsData;
}
