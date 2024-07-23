using System;
using System.Collections.Generic;

// Token: 0x020001A2 RID: 418
public class MovieInfo
{
	// Token: 0x06000DE6 RID: 3558 RVA: 0x00054A1C File Offset: 0x00052C1C
	public MovieInfo(Dictionary<string, object> dict)
	{
		this.identity = TFUtils.LoadInt(dict, "did");
		this.name = (string)dict["name"];
		this.collectName = (string)dict["collect_name"];
		this.description = (string)dict["description"];
		this.moviefile = (string)dict["movie"];
		this.texture = (string)dict["texture"];
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00054AB0 File Offset: 0x00052CB0
	public int Did
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00054AB8 File Offset: 0x00052CB8
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00054AC0 File Offset: 0x00052CC0
	public string CollectName
	{
		get
		{
			return this.collectName;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00054AC8 File Offset: 0x00052CC8
	public string Description
	{
		get
		{
			return this.description;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00054AD0 File Offset: 0x00052CD0
	public string MovieFile
	{
		get
		{
			return this.moviefile;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00054AD8 File Offset: 0x00052CD8
	public string MovieInfoTexture
	{
		get
		{
			return this.texture;
		}
	}

	// Token: 0x04000932 RID: 2354
	private int identity;

	// Token: 0x04000933 RID: 2355
	private string name;

	// Token: 0x04000934 RID: 2356
	private string collectName;

	// Token: 0x04000935 RID: 2357
	private string description;

	// Token: 0x04000936 RID: 2358
	private string moviefile;

	// Token: 0x04000937 RID: 2359
	private string texture;
}
