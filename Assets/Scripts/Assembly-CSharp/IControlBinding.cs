using System;

// Token: 0x020002A5 RID: 677
public interface IControlBinding
{
	// Token: 0x170002CE RID: 718
	// (get) Token: 0x060014BE RID: 5310
	Action<Session> Action { get; }

	// Token: 0x170002CF RID: 719
	// (set) Token: 0x060014BF RID: 5311
	SBGUIButton DynamicButton { set; }

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x060014C1 RID: 5313
	// (set) Token: 0x060014C0 RID: 5312
	string Label { get; set; }

	// Token: 0x060014C2 RID: 5314
	string DecorateSessionActionId(uint ownerDid);

	// Token: 0x060014C3 RID: 5315
	void DynamicUpdate(Session session);
}
