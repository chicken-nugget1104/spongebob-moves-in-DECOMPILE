using System;

// Token: 0x02000401 RID: 1025
public class Fence : Enclosure
{
	// Token: 0x06001F77 RID: 8055 RVA: 0x000C0EF0 File Offset: 0x000BF0F0
	public Fence(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard) : base(box, 5f, mgr, billboard)
	{
	}

	// Token: 0x06001F78 RID: 8056 RVA: 0x000C0F00 File Offset: 0x000BF100
	protected override string GetMaterialName(EnclosureManager.PieceType piece)
	{
		switch (piece)
		{
		case EnclosureManager.PieceType.BACK_CORNER:
			return "Fence_Back_Corner.png";
		case EnclosureManager.PieceType.BACK_LCORNER:
			return "Fence_Back_LCorner.png";
		case EnclosureManager.PieceType.BACK_LEFT:
			return "Fence_Back_Left.png";
		case EnclosureManager.PieceType.BACK_RCORNER:
			return "Fence_Back_RCorner.png";
		case EnclosureManager.PieceType.BACK_RIGHT:
			return "Fence_Back_Right.png";
		case EnclosureManager.PieceType.FRONT_CORNER:
			return "Fence_Front_Corner.png";
		case EnclosureManager.PieceType.FRONT_LCORNER:
			return "Fence_Front_LCorner.png";
		case EnclosureManager.PieceType.FRONT_LEFT:
			return "Fence_Front_Left.png";
		case EnclosureManager.PieceType.FRONT_RCORNER:
			return "Fence_Front_RCorner.png";
		case EnclosureManager.PieceType.FRONT_RIGHT:
			return "Fence_Front_Right.png";
		default:
			return null;
		}
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x000C0F80 File Offset: 0x000BF180
	protected override EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name)
	{
		return mgr.fenceDefs[name];
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x000C0F90 File Offset: 0x000BF190
	protected override void AddLayer(EnclosureManager mgr, int layer, BillboardDelegate billboard)
	{
		base.AddLayer(mgr, layer, billboard);
	}

	// Token: 0x06001F7B RID: 8059 RVA: 0x000C0F9C File Offset: 0x000BF19C
	public override void OnUpdate(Simulation simulation, EnclosureManager mgr)
	{
		if (!base.IsValid)
		{
			return;
		}
		base.OnUpdate(simulation, mgr);
	}

	// Token: 0x06001F7C RID: 8060 RVA: 0x000C0FB4 File Offset: 0x000BF1B4
	public override void Destroy()
	{
		if (!base.IsValid)
		{
			return;
		}
		base.Destroy();
	}

	// Token: 0x04001384 RID: 4996
	private const float BOX_OFFSET = 5f;
}
