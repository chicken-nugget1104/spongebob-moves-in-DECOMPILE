using System;

// Token: 0x02000411 RID: 1041
public class StructureDecorator : EntityDecorator
{
	// Token: 0x06001FE3 RID: 8163 RVA: 0x000C2814 File Offset: 0x000C0A14
	public StructureDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06001FE4 RID: 8164 RVA: 0x000C2820 File Offset: 0x000C0A20
	public bool IsObstacle
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x000C2824 File Offset: 0x000C0A24
	public bool ShouldBlockPlacement
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06001FE6 RID: 8166 RVA: 0x000C2828 File Offset: 0x000C0A28
	public AlignedBox Footprint
	{
		get
		{
			return (AlignedBox)this.Invariable["footprint"];
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x000C2840 File Offset: 0x000C0A40
	public bool Immobile
	{
		get
		{
			object obj;
			return !this.Invariable.TryGetValue("immobile", out obj) || (bool)obj;
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06001FE8 RID: 8168 RVA: 0x000C286C File Offset: 0x000C0A6C
	public bool ShareableSpace
	{
		get
		{
			object obj;
			return this.Invariable.TryGetValue("shareable_space", out obj) && (bool)obj;
		}
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06001FE9 RID: 8169 RVA: 0x000C2898 File Offset: 0x000C0A98
	public bool ShareableSpaceSnap
	{
		get
		{
			object obj;
			return this.Invariable.TryGetValue("shareable_space_snap", out obj) && (bool)obj;
		}
	}
}
