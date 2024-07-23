using System;
using System.Collections.Generic;

// Token: 0x02000256 RID: 598
public class TextPrompt : SessionActionDefinition
{
	// Token: 0x17000279 RID: 633
	// (get) Token: 0x0600133C RID: 4924 RVA: 0x00084BE4 File Offset: 0x00082DE4
	public TextPrompt.Anchor Position
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00084BEC File Offset: 0x00082DEC
	public static TextPrompt Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		TextPrompt textPrompt = new TextPrompt();
		textPrompt.Parse(data, id, startConditions, originatedFromQuest);
		return textPrompt;
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x00084C0C File Offset: 0x00082E0C
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.text = Language.Get(TFUtils.LoadString(data, "text"));
		string text = TFUtils.LoadString(data, "anchor");
		bool flag = false;
		foreach (object obj in Enum.GetValues(typeof(TextPrompt.Anchor)))
		{
			TextPrompt.Anchor anchor = (TextPrompt.Anchor)((int)obj);
			if (text.ToLower() == anchor.ToString().ToLower())
			{
				this.position = anchor;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.position = TextPrompt.Anchor.Bottom;
			string[] names = Enum.GetNames(typeof(TextPrompt.Anchor));
			string arg = string.Join(", ", names).ToLower();
			TFUtils.ErrorLog(string.Format("Error parsing TextPrompt SessionAction. Could not parse value({0}) for key({1}).\nValid types are: {2}", text, "anchor", arg));
		}
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x00084D2C File Offset: 0x00082F2C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["anchor"] = this.position;
		dictionary["text"] = this.text;
		return dictionary;
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x00084D68 File Offset: 0x00082F68
	public void Handle(Session session, SessionActionTracker action, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		containingScreen.UsedInSessionAction = true;
		this.spawnTemplate.Spawn(session.TheGame, action, containingScreen, this.text, this.position);
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x00084DA8 File Offset: 0x00082FA8
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			base.ToString(),
			"TextPrompt:(position=",
			this.position,
			"text=",
			this.text,
			")"
		});
	}

	// Token: 0x04000D4C RID: 3404
	public const string TYPE = "text_prompt";

	// Token: 0x04000D4D RID: 3405
	private const string POSITION = "anchor";

	// Token: 0x04000D4E RID: 3406
	private const string TEXT = "text";

	// Token: 0x04000D4F RID: 3407
	private TextPromptSpawn spawnTemplate = new TextPromptSpawn();

	// Token: 0x04000D50 RID: 3408
	private TextPrompt.Anchor position;

	// Token: 0x04000D51 RID: 3409
	private string text;

	// Token: 0x02000257 RID: 599
	public enum Anchor
	{
		// Token: 0x04000D53 RID: 3411
		Top,
		// Token: 0x04000D54 RID: 3412
		Center,
		// Token: 0x04000D55 RID: 3413
		Bottom
	}
}
