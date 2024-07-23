using System;
using System.Collections.Generic;

// Token: 0x020001B7 RID: 439
public class QuestTemplate
{
	// Token: 0x06000EF1 RID: 3825 RVA: 0x0005E284 File Offset: 0x0005C484
	public static QuestTemplate FromDict(Dictionary<string, object> data)
	{
		QuestTemplate questTemplate = new QuestTemplate();
		uint id = TFUtils.LoadUint(data, "did");
		string text = TFUtils.LoadString(data, "name");
		string text2 = TFUtils.LoadString(data, "icon");
		questTemplate.AddRandomTemplate(id, text, text2, data);
		TFUtils.DebugLog("Loaded Random Quest:" + questTemplate.name);
		return questTemplate;
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x0005E2DC File Offset: 0x0005C4DC
	private void AddRandomTemplate(uint id, string name, string icon, Dictionary<string, object> data)
	{
		this.did = id;
		this.name = name;
		this.icon = icon;
		this.templateData = data;
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0005E2FC File Offset: 0x0005C4FC
	public uint Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0005E304 File Offset: 0x0005C504
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0005E30C File Offset: 0x0005C50C
	public string Icon
	{
		get
		{
			return this.icon;
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x0005E314 File Offset: 0x0005C514
	public Dictionary<string, object> TemplateData
	{
		get
		{
			return this.templateData;
		}
	}

	// Token: 0x040009F6 RID: 2550
	private const string DID = "did";

	// Token: 0x040009F7 RID: 2551
	private const string NAME = "name";

	// Token: 0x040009F8 RID: 2552
	private const string ICON = "icon";

	// Token: 0x040009F9 RID: 2553
	private uint did;

	// Token: 0x040009FA RID: 2554
	private string name;

	// Token: 0x040009FB RID: 2555
	private string icon;

	// Token: 0x040009FC RID: 2556
	private Dictionary<string, object> templateData;
}
