using System;

// Token: 0x020003FD RID: 1021
public class DisplayControllerManager
{
	// Token: 0x06001F63 RID: 8035 RVA: 0x000C0794 File Offset: 0x000BE994
	public DisplayControllerManager()
	{
		this.renderTextureManager = new global::RenderTextureManager();
		this.skeletons = new SkeletonCollection();
		TerrainSlot.MakeRealtySignPrototype(this);
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06001F64 RID: 8036 RVA: 0x000C07C4 File Offset: 0x000BE9C4
	public global::RenderTextureManager RenderTextureManager
	{
		get
		{
			return this.renderTextureManager;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06001F65 RID: 8037 RVA: 0x000C07CC File Offset: 0x000BE9CC
	public SkeletonCollection Skeletons
	{
		get
		{
			return this.skeletons;
		}
	}

	// Token: 0x04001375 RID: 4981
	private global::RenderTextureManager renderTextureManager;

	// Token: 0x04001376 RID: 4982
	private SkeletonCollection skeletons;
}
