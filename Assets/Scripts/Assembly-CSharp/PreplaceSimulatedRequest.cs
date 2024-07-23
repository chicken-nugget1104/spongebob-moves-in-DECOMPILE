using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200023C RID: 572
public class PreplaceSimulatedRequest : SimulationSessionActionDefinition
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001279 RID: 4729 RVA: 0x0007FC88 File Offset: 0x0007DE88
	public int? TargetDid
	{
		get
		{
			return new int?(this.targetDid);
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600127A RID: 4730 RVA: 0x0007FC98 File Offset: 0x0007DE98
	public Vector2 Position
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x0007FCA0 File Offset: 0x0007DEA0
	public static PreplaceSimulatedRequest Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		PreplaceSimulatedRequest preplaceSimulatedRequest = new PreplaceSimulatedRequest();
		preplaceSimulatedRequest.Parse(data, id, startConditions, originatedFromQuest);
		return preplaceSimulatedRequest;
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x0007FCC0 File Offset: 0x0007DEC0
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.targetDid = TFUtils.LoadInt(data, "definition_id");
		TFUtils.LoadVector2(out this.position, (Dictionary<string, object>)data["position"]);
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x0007FD00 File Offset: 0x0007DF00
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["definition_id"] = this.targetDid;
		dictionary["position"] = this.position;
		return dictionary;
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x0007FD44 File Offset: 0x0007DF44
	public void Preplace(Session session, SessionActionTracker action)
	{
		object obj = session.CheckAsyncRequest("preplace_request_dict");
		if (obj != null)
		{
			TFUtils.ErrorLog("We're clobbering another preplacement request on definition: " + this.targetDid);
			((Dictionary<int, Vector2>)obj)[this.targetDid] = this.position;
		}
		else
		{
			obj = new Dictionary<int, Vector2>
			{
				{
					this.targetDid,
					this.position
				}
			};
		}
		session.AddAsyncResponse("preplace_request_dict", obj);
		action.MarkStarted();
		action.MarkSucceeded();
	}

	// Token: 0x04000CAD RID: 3245
	public const string TYPE = "preplace_simulated_request";

	// Token: 0x04000CAE RID: 3246
	public const string PREPLACE_REQUEST_DICT = "preplace_request_dict";

	// Token: 0x04000CAF RID: 3247
	private const string DEFINITION_ID = "definition_id";

	// Token: 0x04000CB0 RID: 3248
	private const string POSITION = "position";

	// Token: 0x04000CB1 RID: 3249
	private int targetDid;

	// Token: 0x04000CB2 RID: 3250
	private Vector2 position;
}
