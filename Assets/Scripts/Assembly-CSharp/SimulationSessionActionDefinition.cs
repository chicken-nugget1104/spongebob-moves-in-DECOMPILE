using System;
using UnityEngine;

// Token: 0x02000252 RID: 594
public abstract class SimulationSessionActionDefinition : SessionActionDefinition
{
	// Token: 0x0600132B RID: 4907 RVA: 0x00084538 File Offset: 0x00082738
	protected void FrameCamera(SBCamera camera, Vector2 worldTarget)
	{
		camera.AutoPanToPosition(worldTarget, 0.75f);
	}
}
