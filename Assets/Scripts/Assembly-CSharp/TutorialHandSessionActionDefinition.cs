using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025A RID: 602
public class TutorialHandSessionActionDefinition : UiTargetingSessionActionDefinition
{
	// Token: 0x0600134D RID: 4941 RVA: 0x000852D0 File Offset: 0x000834D0
	public static TutorialHandSessionActionDefinition Create(Dictionary<string, object> data, uint id, ICondition startingConditions, uint originatedFromQuest)
	{
		TutorialHandSessionActionDefinition tutorialHandSessionActionDefinition = new TutorialHandSessionActionDefinition();
		tutorialHandSessionActionDefinition.Parse(data, id, startingConditions, originatedFromQuest);
		return tutorialHandSessionActionDefinition;
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x000852F0 File Offset: 0x000834F0
	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		TFUtils.Assert(!data.ContainsKey("alpha"), "Hand Guides do not support alpha!");
		TFUtils.Assert(!data.ContainsKey("rotation"), "Hand Guides do not support rotation!");
		base.Parse(data, id, startConditions, originatedFromQuest);
		this.hand.Parse(data, false, Vector3.zero, 1f);
		this.targetSimulatedDid = TFUtils.LoadUint(data, "definition_id");
		this.iconTexture = TFUtils.LoadString(data, "texture");
		this.duration = TFUtils.LoadFloat(data, "duration");
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x00085384 File Offset: 0x00083584
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		this.hand.AddToDict(ref dictionary);
		dictionary["definition_id"] = this.targetSimulatedDid;
		dictionary["texture"] = this.iconTexture;
		dictionary["duration"] = this.duration;
		return dictionary;
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x000853E4 File Offset: 0x000835E4
	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		this.hand.Spawn(session.TheGame, action, target, containingScreen, session.TheGame.simulation.FindSimulated(new int?((int)this.targetSimulatedDid)), this.iconTexture, this.duration);
		base.Handle(session, action, target, containingScreen);
	}

	// Token: 0x04000D62 RID: 3426
	public const string TYPE = "tutorial_hand_pointer";

	// Token: 0x04000D63 RID: 3427
	private const string SIMULATED_DID = "definition_id";

	// Token: 0x04000D64 RID: 3428
	private const string TEXTURE = "texture";

	// Token: 0x04000D65 RID: 3429
	private const string DURATION = "duration";

	// Token: 0x04000D66 RID: 3430
	private TutorialHandDragGuide hand = new TutorialHandDragGuide();

	// Token: 0x04000D67 RID: 3431
	private uint targetSimulatedDid;

	// Token: 0x04000D68 RID: 3432
	private string iconTexture;

	// Token: 0x04000D69 RID: 3433
	private float duration;
}
