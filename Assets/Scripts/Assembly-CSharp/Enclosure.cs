using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public abstract class Enclosure
{
	// Token: 0x06001F66 RID: 8038 RVA: 0x000C07D4 File Offset: 0x000BE9D4
	public Enclosure(AlignedBox box, float boxOffset, EnclosureManager mgr, BillboardDelegate billboard)
	{
		if (box.Width <= 2f * boxOffset || box.Height <= 2f * boxOffset)
		{
			Debug.Log(string.Format("Requested enclosure is too small: {0}x{1}", box.Width, box.Height));
			return;
		}
		this.boxOffset = boxOffset;
		this.SetEnclosureBox(box);
		this.pieces = new List<Enclosure.Piece>();
		this.AddLayer(mgr, 0, billboard);
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x000C085C File Offset: 0x000BEA5C
	public Enclosure(AlignedBox box, EnclosureManager mgr, BillboardDelegate billboard) : this(box, 0f, mgr, billboard)
	{
	}

	// Token: 0x06001F68 RID: 8040
	protected abstract string GetMaterialName(EnclosureManager.PieceType piece);

	// Token: 0x06001F69 RID: 8041
	protected abstract EnclosureManager.PieceDef GetDef(EnclosureManager mgr, string name);

	// Token: 0x06001F6A RID: 8042 RVA: 0x000C086C File Offset: 0x000BEA6C
	public bool IsInitialized()
	{
		return this.pieces != null && this.pieces.Count > 0;
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x000C088C File Offset: 0x000BEA8C
	public void SetEnclosureBox(AlignedBox box)
	{
		if (this.box == null)
		{
			this.box = new AlignedBox(box.xmin + this.boxOffset, box.xmax - this.boxOffset, box.ymin + this.boxOffset, box.ymax - this.boxOffset);
		}
		else
		{
			this.box.xmin = box.xmin + this.boxOffset;
			this.box.xmax = box.xmax - this.boxOffset;
			this.box.ymin = box.ymin + this.boxOffset;
			this.box.ymax = box.ymax - this.boxOffset;
		}
		this.needsUpdate = true;
	}

	// Token: 0x06001F6C RID: 8044 RVA: 0x000C0950 File Offset: 0x000BEB50
	protected virtual void AddLayer(EnclosureManager mgr, int layer, BillboardDelegate billboard)
	{
		this.AddPiece(mgr, layer, 0f, "front_corner", this.GetMaterialName(EnclosureManager.PieceType.FRONT_CORNER), billboard);
		this.AddPiece(mgr, layer, 0f, "back_corner", this.GetMaterialName(EnclosureManager.PieceType.BACK_CORNER), billboard);
		this.AddPiece(mgr, layer, 0f, "back_lcorner", this.GetMaterialName(EnclosureManager.PieceType.BACK_LCORNER), billboard);
		this.AddPiece(mgr, layer, 0f, "front_lcorner", this.GetMaterialName(EnclosureManager.PieceType.FRONT_LCORNER), billboard);
		this.AddPiece(mgr, layer, 0f, "back_rcorner", this.GetMaterialName(EnclosureManager.PieceType.BACK_RCORNER), billboard);
		this.AddPiece(mgr, layer, 0f, "front_rcorner", this.GetMaterialName(EnclosureManager.PieceType.FRONT_RCORNER), billboard);
		float num = this.box.xmax - this.box.xmin;
		float num2 = this.box.ymax - this.box.ymin;
		float num3 = num - 20f;
		int num4 = Mathf.CeilToInt(num3 / 20f);
		float num5 = num3 / ((float)num4 * 20f);
		for (int i = 0; i < num4; i++)
		{
			this.AddPiece(mgr, layer, (float)i * num5, "front_left", this.GetMaterialName(EnclosureManager.PieceType.FRONT_LEFT), billboard);
			this.AddPiece(mgr, layer, (float)i * num5, "back_right", this.GetMaterialName(EnclosureManager.PieceType.BACK_RIGHT), billboard);
		}
		num3 = num2 - 20f;
		num4 = Mathf.CeilToInt(num3 / 20f);
		num5 = num3 / ((float)num4 * 20f);
		for (int j = 0; j < num4; j++)
		{
			this.AddPiece(mgr, layer, (float)j * num5, "front_right", this.GetMaterialName(EnclosureManager.PieceType.FRONT_RIGHT), billboard);
			this.AddPiece(mgr, layer, (float)j * num5, "back_left", this.GetMaterialName(EnclosureManager.PieceType.BACK_LEFT), billboard);
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06001F6D RID: 8045 RVA: 0x000C0B04 File Offset: 0x000BED04
	public bool IsValid
	{
		get
		{
			return this.pieces != null;
		}
	}

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06001F6E RID: 8046 RVA: 0x000C0B14 File Offset: 0x000BED14
	public AlignedBox Box
	{
		get
		{
			return this.box;
		}
	}

	// Token: 0x06001F6F RID: 8047 RVA: 0x000C0B1C File Offset: 0x000BED1C
	public void SetHeight(EnclosureManager mgr, float newHeight, BillboardDelegate billboard)
	{
		if (!this.IsValid)
		{
			return;
		}
		if (newHeight > this.height)
		{
			int num = (int)(this.height / 10f);
			int num2 = (int)(newHeight / 10f);
			if (num2 > num)
			{
				for (int i = num + 1; i <= num2; i++)
				{
					this.AddLayer(mgr, i, billboard);
				}
				this.needsUpdate = true;
			}
			this.height = newHeight;
		}
	}

	// Token: 0x06001F70 RID: 8048 RVA: 0x000C0B8C File Offset: 0x000BED8C
	protected void AddPiece(EnclosureManager mgr, int layer, float sequence, string defName, string spriteName, BillboardDelegate billboard)
	{
		EnclosureManager.PieceDef def = this.GetDef(mgr, defName);
		BasicSprite basicSprite = new BasicSprite(null, spriteName, new Vector2(0f, -0.5f * def.height), def.width, def.height);
		basicSprite.PublicInitialize();
		basicSprite.Billboard(billboard);
		basicSprite.Visible = false;
		Enclosure.Piece item = new Enclosure.Piece(basicSprite, defName, layer, sequence);
		this.pieces.Add(item);
	}

	// Token: 0x06001F71 RID: 8049 RVA: 0x000C0BFC File Offset: 0x000BEDFC
	public virtual void OnUpdate(Simulation simulation, EnclosureManager mgr)
	{
		if (!this.needsUpdate)
		{
			return;
		}
		if (!this.IsValid)
		{
			return;
		}
		Vector3 direction = -simulation.TheCamera.transform.forward;
		direction.Scale(new Vector3(2f, 0f, 1f));
		foreach (Enclosure.Piece piece in this.pieces)
		{
			BasicSprite sprite = piece.sprite;
			if (!sprite.Visible)
			{
				sprite.Visible = true;
			}
			EnclosureManager.PieceDef def = this.GetDef(mgr, piece.defName);
			sprite.Position = mgr.CalcPosition(def.type, this.box) + def.placementOffset + piece.sequence * def.sequenceOffset;
			if (def.type == EnclosureManager.PieceType.FRONT_RIGHT)
			{
				sprite.Face(direction, simulation.TheCamera.transform.up);
				if (sprite.BillboardScaling != def.scale)
				{
					sprite.BillboardScaling = def.scale;
				}
			}
			else
			{
				sprite.OnUpdate(simulation.TheCamera, simulation.particleSystemManager);
			}
			sprite.Translate(-def.textureOrigin);
			sprite.Translate(new Vector3(0f, 10f * (float)piece.layer, 0f));
		}
		this.needsUpdate = false;
	}

	// Token: 0x06001F72 RID: 8050 RVA: 0x000C0DA4 File Offset: 0x000BEFA4
	public virtual void Destroy()
	{
		if (!this.IsValid)
		{
			return;
		}
		foreach (Enclosure.Piece piece in this.pieces)
		{
			if (piece.sprite != null)
			{
				piece.sprite.Destroy();
			}
		}
	}

	// Token: 0x04001377 RID: 4983
	public const float LAYER_HEIGHT = 10f;

	// Token: 0x04001378 RID: 4984
	protected const float CORNER_LENGTH = 10f;

	// Token: 0x04001379 RID: 4985
	protected const float SIDE_LENGTH = 20f;

	// Token: 0x0400137A RID: 4986
	protected AlignedBox box;

	// Token: 0x0400137B RID: 4987
	protected float height;

	// Token: 0x0400137C RID: 4988
	protected bool needsUpdate = true;

	// Token: 0x0400137D RID: 4989
	protected float boxOffset;

	// Token: 0x0400137E RID: 4990
	private List<Enclosure.Piece> pieces;

	// Token: 0x020003FF RID: 1023
	private class Piece
	{
		// Token: 0x06001F73 RID: 8051 RVA: 0x000C0E28 File Offset: 0x000BF028
		public Piece(BasicSprite s, string d, int layer, float sequence)
		{
			this.sprite = s;
			this.defName = d;
			this.layer = layer;
			this.sequence = sequence;
		}

		// Token: 0x0400137F RID: 4991
		public BasicSprite sprite;

		// Token: 0x04001380 RID: 4992
		public string defName;

		// Token: 0x04001381 RID: 4993
		public int layer;

		// Token: 0x04001382 RID: 4994
		public float sequence;
	}
}
