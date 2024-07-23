using System;
using System.Collections.Generic;

// Token: 0x0200026E RID: 622
public class Trigger : ITrigger
{
	// Token: 0x0600140E RID: 5134 RVA: 0x0008A2BC File Offset: 0x000884BC
	public Trigger(string type, Dictionary<string, object> data) : this(type, data, 0UL, null, null)
	{
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x0008A2CC File Offset: 0x000884CC
	public Trigger(string type, Dictionary<string, object> data, ulong utcTimeStamp, Identity target = null, Identity dropID = null)
	{
		TFUtils.Assert(type != null && type != string.Empty, "Must specify a type for triggers.");
		this.type = type;
		this.data = data;
		this.utcTimeStamp = utcTimeStamp;
		this.target = target;
		this.dropID = dropID;
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06001411 RID: 5137 RVA: 0x0008A340 File Offset: 0x00088540
	public string Type
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06001412 RID: 5138 RVA: 0x0008A348 File Offset: 0x00088548
	public Dictionary<string, object> Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06001413 RID: 5139 RVA: 0x0008A350 File Offset: 0x00088550
	public ulong TimeStamp
	{
		get
		{
			return this.utcTimeStamp;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06001414 RID: 5140 RVA: 0x0008A358 File Offset: 0x00088558
	public static Trigger Null
	{
		get
		{
			return Trigger.nullTrigger;
		}
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x0008A360 File Offset: 0x00088560
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = this.type;
		dictionary["utcTimeStamp"] = this.utcTimeStamp;
		dictionary["data"] = this.data;
		if (this.target != null)
		{
			dictionary["target"] = this.target.Describe();
		}
		if (this.dropID != null)
		{
			dictionary["dropID"] = this.dropID.Describe();
		}
		return dictionary;
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0008A3F0 File Offset: 0x000885F0
	public static ITrigger FromDict(Dictionary<string, object> dict)
	{
		string text = (string)dict["type"];
		ulong num = TFUtils.LoadUlong(dict, "utcTimeStamp", 0UL);
		Dictionary<string, object> dictionary = TFUtils.LoadDict(dict, "data");
		Identity identity = (!dict.ContainsKey("target")) ? null : new Identity((string)dict["target"]);
		Identity identity2 = (!dict.ContainsKey("dropID")) ? null : new Identity((string)dict["dropID"]);
		return new Trigger(text, dictionary, num, identity, identity2);
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x0008A48C File Offset: 0x0008868C
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"Trigger:(type=",
			this.type,
			", data=",
			TFUtils.DebugDictToString(this.data),
			")"
		});
	}

	// Token: 0x04000E0D RID: 3597
	private static readonly Trigger nullTrigger = new Trigger("nulltrigger", new Dictionary<string, object>(), 0UL, null, null);

	// Token: 0x04000E0E RID: 3598
	public Identity target;

	// Token: 0x04000E0F RID: 3599
	public Identity dropID;

	// Token: 0x04000E10 RID: 3600
	private string type;

	// Token: 0x04000E11 RID: 3601
	private ulong utcTimeStamp;

	// Token: 0x04000E12 RID: 3602
	private Dictionary<string, object> data;
}
