using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x02000402 RID: 1026
public class EnclosureManager
{
	// Token: 0x06001F7D RID: 8061 RVA: 0x000C0FC8 File Offset: 0x000BF1C8
	public EnclosureManager()
	{
		this.scaffoldingDefs = new Dictionary<string, EnclosureManager.PieceDef>();
		this.fenceDefs = new Dictionary<string, EnclosureManager.PieceDef>();
		this.LoadDefinitionsFromSpread();
		this.allScaffolds = new List<Enclosure>();
		this.allFences = new List<Enclosure>();
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x000C1010 File Offset: 0x000BF210
	private void LoadDefinitionsFromSpread()
	{
		string text = "Enclosure";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (instance.HasRow(sheetIndex, rowName))
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				EnclosureManager.PieceDef pieceDef = new EnclosureManager.PieceDef();
				pieceDef.width = instance.GetFloatCell(sheetIndex, rowIndex, "width");
				pieceDef.height = instance.GetFloatCell(sheetIndex, rowIndex, "width");
				pieceDef.scale = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "scale x"), instance.GetFloatCell(sheetIndex, rowIndex, "scale y"), instance.GetFloatCell(sheetIndex, rowIndex, "scale z"));
				pieceDef.textureOrigin = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "texture origin X"), instance.GetFloatCell(sheetIndex, rowIndex, "texture origin y"), 0f);
				pieceDef.sequenceOffset = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "sequence offset x"), instance.GetFloatCell(sheetIndex, rowIndex, "sequence offset y"), instance.GetFloatCell(sheetIndex, rowIndex, "sequence offset z"));
				pieceDef.placementOffset = new Vector3(instance.GetFloatCell(sheetIndex, rowIndex, "placement offset x"), instance.GetFloatCell(sheetIndex, rowIndex, "placement offset y"), 0f);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "name");
				string text2 = stringCell;
				if (text2 == null)
				{
					goto IL_314;
				}
				if (EnclosureManager.<>f__switch$map1A == null)
				{
					EnclosureManager.<>f__switch$map1A = new Dictionary<string, int>(10)
					{
						{
							"back_corner",
							0
						},
						{
							"back_lcorner",
							1
						},
						{
							"back_left",
							2
						},
						{
							"back_rcorner",
							3
						},
						{
							"back_right",
							4
						},
						{
							"front_corner",
							5
						},
						{
							"front_lcorner",
							6
						},
						{
							"front_left",
							7
						},
						{
							"front_rcorner",
							8
						},
						{
							"front_right",
							9
						}
					};
				}
				int num2;
				if (!EnclosureManager.<>f__switch$map1A.TryGetValue(text2, out num2))
				{
					goto IL_314;
				}
				switch (num2)
				{
				case 0:
					pieceDef.type = EnclosureManager.PieceType.BACK_CORNER;
					break;
				case 1:
					pieceDef.type = EnclosureManager.PieceType.BACK_LCORNER;
					break;
				case 2:
					pieceDef.type = EnclosureManager.PieceType.BACK_LEFT;
					break;
				case 3:
					pieceDef.type = EnclosureManager.PieceType.BACK_RCORNER;
					break;
				case 4:
					pieceDef.type = EnclosureManager.PieceType.BACK_RIGHT;
					break;
				case 5:
					pieceDef.type = EnclosureManager.PieceType.FRONT_CORNER;
					break;
				case 6:
					pieceDef.type = EnclosureManager.PieceType.FRONT_LCORNER;
					break;
				case 7:
					pieceDef.type = EnclosureManager.PieceType.FRONT_LEFT;
					break;
				case 8:
					pieceDef.type = EnclosureManager.PieceType.FRONT_RCORNER;
					break;
				case 9:
					pieceDef.type = EnclosureManager.PieceType.FRONT_RIGHT;
					break;
				default:
					goto IL_314;
				}
				IL_32B:
				if (instance.GetStringCell(sheetIndex, rowIndex, "type") == "fence")
				{
					this.fenceDefs[stringCell] = pieceDef;
					goto IL_36B;
				}
				this.scaffoldingDefs[stringCell] = pieceDef;
				goto IL_36B;
				IL_314:
				TFUtils.Assert(true, " Enclosure.csv has unknown defininiton for " + stringCell);
				goto IL_32B;
			}
			num++;
			IL_36B:;
		}
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000C1398 File Offset: 0x000BF598
	private void LoadDefinitions(string filename, Dictionary<string, EnclosureManager.PieceDef> defs)
	{
		TFUtils.DebugLog("Loading Enclosure definition file: " + filename);
		string json = TFUtils.ReadAllText(filename);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		foreach (KeyValuePair<string, object> keyValuePair in dictionary)
		{
			string key = keyValuePair.Key;
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)keyValuePair.Value;
			float width = TFUtils.LoadFloat(dictionary2, "width");
			float height = TFUtils.LoadFloat(dictionary2, "height");
			Vector3 one = Vector3.one;
			if (dictionary2.ContainsKey("scale"))
			{
				TFUtils.LoadVector3(out one, (Dictionary<string, object>)dictionary2["scale"]);
			}
			Vector3 zero = Vector3.zero;
			if (dictionary2.ContainsKey("placement_offset"))
			{
				TFUtils.LoadVector3(out zero, (Dictionary<string, object>)dictionary2["placement_offset"]);
			}
			Vector3 vector = Vector3.zero;
			if (dictionary2.ContainsKey("texture_origin"))
			{
				TFUtils.LoadVector3(out vector, (Dictionary<string, object>)dictionary2["texture_origin"]);
				vector *= 0.1302f;
			}
			Vector3 zero2 = Vector3.zero;
			if (dictionary2.ContainsKey("sequence_offset"))
			{
				TFUtils.LoadVector3(out zero2, (Dictionary<string, object>)dictionary2["sequence_offset"]);
			}
			EnclosureManager.PieceDef pieceDef = new EnclosureManager.PieceDef();
			string text = key;
			if (text == null)
			{
				goto IL_2B1;
			}
			if (EnclosureManager.<>f__switch$map1B == null)
			{
				EnclosureManager.<>f__switch$map1B = new Dictionary<string, int>(10)
				{
					{
						"back_corner",
						0
					},
					{
						"back_lcorner",
						1
					},
					{
						"back_left",
						2
					},
					{
						"back_rcorner",
						3
					},
					{
						"back_right",
						4
					},
					{
						"front_corner",
						5
					},
					{
						"front_lcorner",
						6
					},
					{
						"front_left",
						7
					},
					{
						"front_rcorner",
						8
					},
					{
						"front_right",
						9
					}
				};
			}
			int num;
			if (!EnclosureManager.<>f__switch$map1B.TryGetValue(text, out num))
			{
				goto IL_2B1;
			}
			switch (num)
			{
			case 0:
				pieceDef.type = EnclosureManager.PieceType.BACK_CORNER;
				break;
			case 1:
				pieceDef.type = EnclosureManager.PieceType.BACK_LCORNER;
				break;
			case 2:
				pieceDef.type = EnclosureManager.PieceType.BACK_LEFT;
				break;
			case 3:
				pieceDef.type = EnclosureManager.PieceType.BACK_RCORNER;
				break;
			case 4:
				pieceDef.type = EnclosureManager.PieceType.BACK_RIGHT;
				break;
			case 5:
				pieceDef.type = EnclosureManager.PieceType.FRONT_CORNER;
				break;
			case 6:
				pieceDef.type = EnclosureManager.PieceType.FRONT_LCORNER;
				break;
			case 7:
				pieceDef.type = EnclosureManager.PieceType.FRONT_LEFT;
				break;
			case 8:
				pieceDef.type = EnclosureManager.PieceType.FRONT_RCORNER;
				break;
			case 9:
				pieceDef.type = EnclosureManager.PieceType.FRONT_RIGHT;
				break;
			default:
				goto IL_2B1;
			}
			IL_2C9:
			pieceDef.width = width;
			pieceDef.height = height;
			pieceDef.scale = one;
			pieceDef.placementOffset = zero;
			pieceDef.textureOrigin = vector;
			pieceDef.sequenceOffset = zero2;
			defs[key] = pieceDef;
			continue;
			IL_2B1:
			TFUtils.Assert(true, filename + " has unknown defininiton for " + key);
			goto IL_2C9;
		}
		TFUtils.Assert(defs.ContainsKey("back_corner"), filename + " is missing a definition for back_corner");
		TFUtils.Assert(defs.ContainsKey("back_lcorner"), filename + " is missing a definition for back_lcorner");
		TFUtils.Assert(defs.ContainsKey("back_left"), filename + " is missing a definition for back_left");
		TFUtils.Assert(defs.ContainsKey("back_rcorner"), filename + " is missing a definition for back_rcorner");
		TFUtils.Assert(defs.ContainsKey("back_right"), filename + " is missing a definition for back_right");
		TFUtils.Assert(defs.ContainsKey("front_corner"), filename + " is missing a definition for front_corner");
		TFUtils.Assert(defs.ContainsKey("front_lcorner"), filename + " is missing a definition for front_lcorner");
		TFUtils.Assert(defs.ContainsKey("front_left"), filename + " is missing a definition for front_left");
		TFUtils.Assert(defs.ContainsKey("front_rcorner"), filename + " is missing a definition for front_rcorner");
		TFUtils.Assert(defs.ContainsKey("front_right"), filename + " is missing a definition for front_right");
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x000C17F8 File Offset: 0x000BF9F8
	public Scaffolding AddScaffolding(AlignedBox box, BillboardDelegate billboard)
	{
		Scaffolding scaffolding = new Scaffolding(box, this, billboard);
		this.allScaffolds.Add(scaffolding);
		return scaffolding;
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x000C181C File Offset: 0x000BFA1C
	public void RemoveScaffolding(Scaffolding s)
	{
		s.Destroy();
		this.allScaffolds.Remove(s);
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x000C1834 File Offset: 0x000BFA34
	public Fence AddFence(AlignedBox box, BillboardDelegate billboard)
	{
		Fence fence = new Fence(box, this, billboard);
		this.allFences.Add(fence);
		return fence;
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x000C1858 File Offset: 0x000BFA58
	public void RemoveFence(Fence s)
	{
		s.Destroy();
		this.allFences.Remove(s);
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x000C1870 File Offset: 0x000BFA70
	public void OnUpdate(Simulation simulation)
	{
		foreach (Enclosure enclosure in this.allScaffolds)
		{
			Scaffolding scaffolding = (Scaffolding)enclosure;
			scaffolding.OnUpdate(simulation, this);
		}
		foreach (Enclosure enclosure2 in this.allFences)
		{
			Fence fence = (Fence)enclosure2;
			fence.OnUpdate(simulation, this);
		}
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000C1938 File Offset: 0x000BFB38
	public Vector3 CalcPosition(EnclosureManager.PieceType type, AlignedBox box)
	{
		switch (type)
		{
		case EnclosureManager.PieceType.BACK_CORNER:
			return new Vector3(box.xmax, box.ymax, 0f);
		case EnclosureManager.PieceType.BACK_LCORNER:
			return new Vector3(box.xmax, box.ymin, 0f);
		case EnclosureManager.PieceType.BACK_LEFT:
			return new Vector3(box.xmax, box.ymax, 0f);
		case EnclosureManager.PieceType.BACK_RCORNER:
			return new Vector3(box.xmin, box.ymax, 0f);
		case EnclosureManager.PieceType.BACK_RIGHT:
			return new Vector3(box.xmax, box.ymax, 0f);
		case EnclosureManager.PieceType.FRONT_CORNER:
			return new Vector3(box.xmin, box.ymin, 0f);
		case EnclosureManager.PieceType.FRONT_LCORNER:
			return new Vector3(box.xmax, box.ymin, 0f);
		case EnclosureManager.PieceType.FRONT_LEFT:
			return new Vector3(box.xmin, box.ymin, 0f);
		case EnclosureManager.PieceType.FRONT_RCORNER:
			return new Vector3(box.xmin, box.ymax, 0f);
		case EnclosureManager.PieceType.FRONT_RIGHT:
			return new Vector3(box.xmin, box.ymin, 0f);
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x04001385 RID: 4997
	public const string NAME_BACK_CORNER = "back_corner";

	// Token: 0x04001386 RID: 4998
	public const string NAME_BACK_LCORNER = "back_lcorner";

	// Token: 0x04001387 RID: 4999
	public const string NAME_BACK_LEFT = "back_left";

	// Token: 0x04001388 RID: 5000
	public const string NAME_BACK_RCORNER = "back_rcorner";

	// Token: 0x04001389 RID: 5001
	public const string NAME_BACK_RIGHT = "back_right";

	// Token: 0x0400138A RID: 5002
	public const string NAME_FRONT_CORNER = "front_corner";

	// Token: 0x0400138B RID: 5003
	public const string NAME_FRONT_LCORNER = "front_lcorner";

	// Token: 0x0400138C RID: 5004
	public const string NAME_FRONT_LEFT = "front_left";

	// Token: 0x0400138D RID: 5005
	public const string NAME_FRONT_RCORNER = "front_rcorner";

	// Token: 0x0400138E RID: 5006
	public const string NAME_FRONT_RIGHT = "front_right";

	// Token: 0x0400138F RID: 5007
	private List<Enclosure> allScaffolds;

	// Token: 0x04001390 RID: 5008
	private List<Enclosure> allFences;

	// Token: 0x04001391 RID: 5009
	public List<EnclosureManager.FlasherDef> flasherDefs;

	// Token: 0x04001392 RID: 5010
	public Dictionary<string, EnclosureManager.PieceDef> scaffoldingDefs;

	// Token: 0x04001393 RID: 5011
	public Dictionary<string, EnclosureManager.PieceDef> fenceDefs;

	// Token: 0x02000403 RID: 1027
	public enum PieceType
	{
		// Token: 0x04001397 RID: 5015
		BACK_CORNER,
		// Token: 0x04001398 RID: 5016
		BACK_LCORNER,
		// Token: 0x04001399 RID: 5017
		BACK_LEFT,
		// Token: 0x0400139A RID: 5018
		BACK_RCORNER,
		// Token: 0x0400139B RID: 5019
		BACK_RIGHT,
		// Token: 0x0400139C RID: 5020
		FRONT_CORNER,
		// Token: 0x0400139D RID: 5021
		FRONT_LCORNER,
		// Token: 0x0400139E RID: 5022
		FRONT_LEFT,
		// Token: 0x0400139F RID: 5023
		FRONT_RCORNER,
		// Token: 0x040013A0 RID: 5024
		FRONT_RIGHT
	}

	// Token: 0x02000404 RID: 1028
	public class FlasherDef
	{
		// Token: 0x040013A1 RID: 5025
		public SpriteAnimationModel animationModel;

		// Token: 0x040013A2 RID: 5026
		public Vector2 positionOffset;

		// Token: 0x040013A3 RID: 5027
		public float width;

		// Token: 0x040013A4 RID: 5028
		public float height;

		// Token: 0x040013A5 RID: 5029
		public string placement;

		// Token: 0x040013A6 RID: 5030
		public Vector3 placementOffset;
	}

	// Token: 0x02000405 RID: 1029
	public class PieceDef
	{
		// Token: 0x040013A7 RID: 5031
		public EnclosureManager.PieceType type;

		// Token: 0x040013A8 RID: 5032
		public float height;

		// Token: 0x040013A9 RID: 5033
		public float width;

		// Token: 0x040013AA RID: 5034
		public Vector3 scale;

		// Token: 0x040013AB RID: 5035
		public Vector3 placementOffset;

		// Token: 0x040013AC RID: 5036
		public Vector3 textureOrigin;

		// Token: 0x040013AD RID: 5037
		public Vector3 sequenceOffset;
	}
}
