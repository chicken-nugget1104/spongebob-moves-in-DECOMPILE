using System;

// Token: 0x02000400 RID: 1024
public class Scaffolding : Enclosure
{
	// Token: 0x06001F74 RID: 8052 RVA: 0x000C0E50 File Offset: 0x000BF050
	public Scaffolding(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard) : base(box, 5f, mgr, billboard)
	{
	}

	// Token: 0x06001F75 RID: 8053 RVA: 0x000C0E60 File Offset: 0x000BF060
	protected override string GetMaterialName(EnclosureManager.PieceType piece)
	{
		switch (piece)
		{
		case EnclosureManager.PieceType.BACK_CORNER:
			return "Scaffold_Back_Corner.png";
		case EnclosureManager.PieceType.BACK_LCORNER:
			return "Scaffold_Back_LCorner.png";
		case EnclosureManager.PieceType.BACK_LEFT:
			return "Scaffold_Back_Left.png";
		case EnclosureManager.PieceType.BACK_RCORNER:
			return "Scaffold_Back_RCorner.png";
		case EnclosureManager.PieceType.BACK_RIGHT:
			return "Scaffold_Back_Right.png";
		case EnclosureManager.PieceType.FRONT_CORNER:
			return "Scaffold_Front_Corner.png";
		case EnclosureManager.PieceType.FRONT_LCORNER:
			return "Scaffold_Front_LCorner.png";
		case EnclosureManager.PieceType.FRONT_LEFT:
			return "Scaffold_Front_Left.png";
		case EnclosureManager.PieceType.FRONT_RCORNER:
			return "Scaffold_Front_RCorner.png";
		case EnclosureManager.PieceType.FRONT_RIGHT:
			return "Scaffold_Front_Right.png";
		default:
			return null;
		}
	}

	// Token: 0x06001F76 RID: 8054 RVA: 0x000C0EE0 File Offset: 0x000BF0E0
	protected override EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name)
	{
		return mgr.scaffoldingDefs[name];
	}

	// Token: 0x04001383 RID: 4995
	private const float BOX_OFFSET = 5f;
}
