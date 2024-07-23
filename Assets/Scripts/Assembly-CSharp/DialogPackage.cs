using System;
using System.Collections.Generic;

// Token: 0x02000160 RID: 352
public class DialogPackage
{
	// Token: 0x06000C0F RID: 3087 RVA: 0x00048A48 File Offset: 0x00046C48
	public DialogPackage(Dictionary<string, object> data)
	{
		this.data = data;
		this.did = TFUtils.LoadUint(data, "did");
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00048A68 File Offset: 0x00046C68
	public uint Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00048A70 File Offset: 0x00046C70
	public Dictionary<string, object> Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00048A78 File Offset: 0x00046C78
	public List<DialogInputData> GetDialogInputsInSequence(uint sequenceId, Dictionary<string, object> contextData, uint? associatedQuestId)
	{
		List<DialogInputData> list = new List<DialogInputData>();
		List<object> promptsInSequence = this.GetPromptsInSequence(sequenceId);
		TFUtils.Assert(promptsInSequence != null, "Found no prompts in dialog sequence! SequenceId=" + sequenceId);
		foreach (object obj in promptsInSequence)
		{
			Dictionary<string, object> prompt = (Dictionary<string, object>)obj;
			list.Add(DialogInputData.FromPromptDict(sequenceId, prompt, contextData, associatedQuestId));
		}
		return list;
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00048B14 File Offset: 0x00046D14
	private List<object> GetPromptsInSequence(uint sequenceId)
	{
		List<object> list = (List<object>)this.data["sequences"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			uint num = TFUtils.LoadUint(dictionary, "id");
			if (num == sequenceId)
			{
				return (List<object>)dictionary["prompts"];
			}
		}
		return null;
	}

	// Token: 0x04000819 RID: 2073
	private uint did;

	// Token: 0x0400081A RID: 2074
	private Dictionary<string, object> data;
}
