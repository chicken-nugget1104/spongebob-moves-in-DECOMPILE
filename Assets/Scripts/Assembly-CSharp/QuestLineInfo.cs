using System;
using System.Collections.Generic;

// Token: 0x020001B4 RID: 436
public class QuestLineInfo
{
	// Token: 0x06000EBA RID: 3770 RVA: 0x0005B3D0 File Offset: 0x000595D0
	public static QuestLineInfo FromDict(Dictionary<string, object> data)
	{
		return new QuestLineInfo
		{
			name = (string)data["name"],
			icon = (string)data["icon"],
			hasProgress = (!data.ContainsKey("has_progress") || TFUtils.LoadBool(data, "has_progress"))
		};
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0005B438 File Offset: 0x00059638
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["name"] = this.name;
		dictionary["icon"] = this.icon;
		dictionary["has_progress"] = this.hasProgress;
		return dictionary;
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0005B484 File Offset: 0x00059684
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0005B48C File Offset: 0x0005968C
	public string Icon
	{
		get
		{
			return this.icon;
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0005B494 File Offset: 0x00059694
	public bool HasProgress
	{
		get
		{
			return this.hasProgress;
		}
	}

	// Token: 0x040009D6 RID: 2518
	private const string NAME = "name";

	// Token: 0x040009D7 RID: 2519
	private const string ICON = "icon";

	// Token: 0x040009D8 RID: 2520
	private const string HAS_PROGRESS = "has_progress";

	// Token: 0x040009D9 RID: 2521
	private string name;

	// Token: 0x040009DA RID: 2522
	private string icon;

	// Token: 0x040009DB RID: 2523
	private bool hasProgress;
}
