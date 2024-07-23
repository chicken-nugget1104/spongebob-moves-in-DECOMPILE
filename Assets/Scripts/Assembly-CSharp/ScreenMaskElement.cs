using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000240 RID: 576
public class ScreenMaskElement : UiTargetingSessionActionDefinition
{
	// Token: 0x06001293 RID: 4755 RVA: 0x00080270 File Offset: 0x0007E470
	public static ScreenMaskElement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ScreenMaskElement screenMaskElement = new ScreenMaskElement();
		screenMaskElement.Parse(data, id, startConditions, originatedFromQuest);
		return screenMaskElement;
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x00080290 File Offset: 0x0007E490
	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		containingScreen.UsedInSessionAction = true;
		bool useSecondCam = base.DynamicScrolledSubTarget != null;
		ScreenMaskSpawn.Spawn(ScreenMaskSpawn.ScreenMaskType.ELEMENT, session.TheGame, action, target, containingScreen, null, null, this.radius, this.texture, this.offset, useSecondCam);
		base.Handle(session, action, target, containingScreen);
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x000802F0 File Offset: 0x0007E4F0
	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, originatedFromQuest);
		this.radius = (float)TFUtils.LoadInt(data, "radius") * 0.01f;
		this.texture = TFUtils.TryLoadString(data, "texture");
		if (data.ContainsKey("offset"))
		{
			TFUtils.LoadVector3(out this.offset, TFUtils.LoadDict(data, "offset"));
			this.offset *= 0.01f;
		}
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x00080370 File Offset: 0x0007E570
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["radius"] = this.radius;
		dictionary["texture"] = this.texture;
		dictionary["offset"] = this.offset;
		return dictionary;
	}

	// Token: 0x04000CBF RID: 3263
	public const string TYPE = "screenmask_element";

	// Token: 0x04000CC0 RID: 3264
	private const string RADIUS = "radius";

	// Token: 0x04000CC1 RID: 3265
	private const string TEXTURE = "texture";

	// Token: 0x04000CC2 RID: 3266
	private const string OFFSET = "offset";

	// Token: 0x04000CC3 RID: 3267
	private float radius;

	// Token: 0x04000CC4 RID: 3268
	private Vector3 offset = Vector3.zero;

	// Token: 0x04000CC5 RID: 3269
	private string texture;
}
