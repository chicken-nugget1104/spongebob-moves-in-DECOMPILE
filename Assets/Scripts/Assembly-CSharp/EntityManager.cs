using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniJSON;
using UnityEngine;
using Yarg;

// Token: 0x02000416 RID: 1046
public class EntityManager
{
	// Token: 0x0600201A RID: 8218 RVA: 0x000C2B88 File Offset: 0x000C0D88
	public EntityManager(bool friendMode)
	{
		EntityManager.GenerateStates(friendMode);
		this.factory = new Factory<string, Entity>();
		this.entities = new Dictionary<Identity, Entity>(new Identity.Equality());
		this.entityCount = new Dictionary<string, int>();
		this.displayControllerManager = new DisplayControllerManager();
		this.blueprintsToData = new Dictionary<Blueprint, Dictionary<string, object>>();
		this.LoadBlueprints();
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000C2BF0 File Offset: 0x000C0DF0
	static EntityManager()
	{
		EntityManager.MustRegenerateStates = true;
		EntityManager.GenerateStates(false);
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000C2C34 File Offset: 0x000C0E34
	public static void GenerateStates(bool friendMode)
	{
		if (!EntityManager.MustRegenerateStates)
		{
			return;
		}
		EntityManager.TypeRegistry = new Dictionary<string, EntityManager.BlueprintMarshaller>();
		EntityManager.AssetsInitializerTypeRegistry = new Dictionary<string, EntityManager.BlueprintAssetsInitializer>();
		EntityManager.TypeRegistry.Add("building", new EntityManager.BlueprintMarshaller(EntityManager.MarshallBuilding));
		BuildingStateSetup.Generate(out EntityManager.BuildingActions, out EntityManager.BuildingMachine, friendMode);
		EntityManager.TypeRegistry.Add("annex", new EntityManager.BlueprintMarshaller(EntityManager.MarshallAnnex));
		AnnexStateSetup.Generate(out EntityManager.AnnexActions, out EntityManager.AnnexMachine, friendMode);
		EntityManager.TypeRegistry.Add("debris", new EntityManager.BlueprintMarshaller(EntityManager.MarshallDebris));
		DebrisStateSetup.Generate(out EntityManager.DebrisActions, out EntityManager.DebrisMachine, friendMode);
		EntityManager.TypeRegistry.Add("landmark", new EntityManager.BlueprintMarshaller(EntityManager.MarshallLandmark));
		LandmarkStateSetup.Generate(out EntityManager.LandmarkActions, out EntityManager.LandmarkMachine, friendMode);
		EntityManager.TypeRegistry.Add("unit", new EntityManager.BlueprintMarshaller(EntityManager.MarshallUnit));
		EntityManager.AssetsInitializerTypeRegistry.Add("unit", new EntityManager.BlueprintAssetsInitializer(EntityManager.InitializeUnitAssets));
		if (!friendMode)
		{
			UnitStateSetup.Generate(out EntityManager.ResidentActions, out EntityManager.UnitMachine);
		}
		else
		{
			UnitStateSetup.GenerateFriendsStates(out EntityManager.ResidentActions, out EntityManager.UnitMachine);
		}
		EntityManager.TypeRegistry.Add("worker", new EntityManager.BlueprintMarshaller(EntityManager.MarshallWorker));
		EntityManager.AssetsInitializerTypeRegistry.Add("worker", new EntityManager.BlueprintAssetsInitializer(EntityManager.InitializeWorkerAssets));
		WorkerStateSetup.Generate(out EntityManager.WorkerActions, out EntityManager.WorkerMachine);
		EntityManager.TypeRegistry.Add("wanderer", new EntityManager.BlueprintMarshaller(EntityManager.MarshallWanderer));
		EntityManager.AssetsInitializerTypeRegistry.Add("wanderer", new EntityManager.BlueprintAssetsInitializer(EntityManager.InitializeUnitAssets));
		WandererStateSetup.Generate(out EntityManager.WandererActions, out EntityManager.WandererMachine);
		EntityManager.TypeRegistry.Add("treasure", new EntityManager.BlueprintMarshaller(EntityManager.MarshallTreasure));
		TreasureStateSetup.Generate(out EntityManager.TreasureActions, out EntityManager.TreasureMachine, friendMode);
		EntityManager.MustRegenerateStates = false;
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000C2E28 File Offset: 0x000C1028
	private static void RegisterDisplayOffset(Dictionary<string, object> data, string theKey, Blueprint blueprint)
	{
		if (data.ContainsKey("position_offset"))
		{
			Vector3 vector;
			TFUtils.LoadVector3(out vector, (Dictionary<string, object>)data["position_offset"]);
			blueprint.Invariable[theKey + ".position_offset"] = vector;
		}
		else if (theKey == "display.default.flip")
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["display.default.flip"];
			foreach (string a in dictionary.Keys)
			{
				if (a == "position_offset")
				{
					Vector3 vector;
					TFUtils.LoadVector3(out vector, (Dictionary<string, object>)dictionary["position_offset"]);
					blueprint.Invariable[theKey + ".position_offset"] = vector;
				}
			}
		}
		else
		{
			blueprint.Invariable[theKey + ".position_offset"] = null;
		}
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000C2F50 File Offset: 0x000C1150
	private static void RegisterTextureOrigin(Dictionary<string, object> data, string theKey, Blueprint blueprint)
	{
		if (data.ContainsKey("texture_origin"))
		{
			Vector3 vector;
			TFUtils.LoadVector3(out vector, (Dictionary<string, object>)data["texture_origin"]);
			blueprint.Invariable[theKey + ".texture_origin"] = vector;
		}
		else
		{
			blueprint.Invariable[theKey + ".texture_origin"] = null;
		}
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000C2FBC File Offset: 0x000C11BC
	private static void RegisterHitArea(Dictionary<string, object> data, QuadHitObject hitObject, string theKey, Blueprint blueprint)
	{
		Vector2 zero = Vector2.zero;
		if (data.ContainsKey("mesh_name"))
		{
			string value = Convert.ToString(data["mesh_name"]);
			blueprint.Invariable[theKey + ".mesh_name"] = value;
			if (data.ContainsKey("separate_tap"))
			{
				bool flag = Convert.ToBoolean(data["separate_tap"]);
				blueprint.Invariable[theKey + ".separate_tap"] = flag;
			}
		}
		else if (data.ContainsKey("hit_area"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["hit_area"];
			TFUtils.Assert(dictionary.ContainsKey("center") && dictionary.ContainsKey("width") && dictionary.ContainsKey("height"), "HitArea information must contain center, width and height information");
			TFUtils.LoadVector2(out zero, (Dictionary<string, object>)dictionary["center"]);
			float width = (float)TFUtils.LoadInt(dictionary, "width");
			float height = (float)TFUtils.LoadInt(dictionary, "height");
			hitObject.Initialize(zero, width, height);
			blueprint.Invariable[theKey + ".mesh_name"] = null;
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000C30FC File Offset: 0x000C12FC
	private static TFAnimatedSprite CreateAnimatedSpritePrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, Dictionary<string, object> fullData)
	{
		float width = TFUtils.LoadFloat(data, "width");
		float height = (float)TFUtils.LoadInt(data, "height");
		return EntityManager.CreateAnimatedSpritePrototype(data, theKey, blueprint, width, height, fullData);
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000C3130 File Offset: 0x000C1330
	private static TFAnimatedSprite CreateAnimatedSpritePrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, float width, float height, Dictionary<string, object> fullData)
	{
		EntityManager.RegisterDisplayOffset(data, theKey, blueprint);
		if (fullData.ContainsKey("display.default.flip"))
		{
			EntityManager.RegisterDisplayOffset(fullData, "display.default.flip", blueprint);
		}
		EntityManager.RegisterTextureOrigin(data, theKey, blueprint);
		SpriteAnimationModel animModel = new SpriteAnimationModel();
		TFAnimatedSprite tfanimatedSprite = new TFAnimatedSprite(new Vector2(0f, -0.5f * height), width, height, animModel);
		EntityManager.RegisterShareableSpaceSnap(data, blueprint);
		EntityManager.RegisterHitArea(data, tfanimatedSprite.HitObject, theKey, blueprint);
		Dictionary<string, object> data2 = new Dictionary<string, object>();
		foreach (string text in fullData.Keys)
		{
			if (text.Contains("display."))
			{
				data2 = (Dictionary<string, object>)fullData[text];
				EntityManager.RegisterMeshName(data2, blueprint, text);
			}
		}
		return tfanimatedSprite;
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000C3224 File Offset: 0x000C1424
	private static void RegisterShareableSpaceSnap(Dictionary<string, object> data, Blueprint blueprint)
	{
		if (data.ContainsKey("shareable_space_snap"))
		{
			bool flag = Convert.ToBoolean(data["shareable_space_snap"]);
			blueprint.Invariable["shareable_space_snap"] = flag;
		}
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000C3268 File Offset: 0x000C1468
	private static void RegisterMeshName(Dictionary<string, object> data, Blueprint blueprint, string theKey)
	{
		if (data.ContainsKey("mesh_name"))
		{
			string value = Convert.ToString(data["mesh_name"]);
			blueprint.Invariable[theKey + ".mesh_name"] = value;
		}
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000C32B0 File Offset: 0x000C14B0
	private static IDisplayController CreatePaperdollPrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, Paperdoll.PaperdollType paperdollType)
	{
		int num = TFUtils.LoadInt(data, "width");
		int num2 = TFUtils.LoadInt(data, "height");
		EntityManager.RegisterDisplayOffset(data, theKey, blueprint);
		Vector3 one = Vector3.one;
		if (data.ContainsKey("display_scale"))
		{
			TFUtils.LoadVector3(out one, (Dictionary<string, object>)data["display_scale"], 1f);
		}
		bool? flag = TFUtils.TryLoadBool(data, "flippable");
		if (flag == null)
		{
			flag = new bool?(true);
		}
		Paperdoll paperdoll = new Paperdoll(new Vector2(0f, -0.5f * (float)num2), (float)num, (float)num2, one, flag.Value, paperdollType);
		EntityManager.RegisterShareableSpaceSnap(data, blueprint);
		EntityManager.RegisterHitArea(data, paperdoll.HitObject, theKey, blueprint);
		return paperdoll;
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x000C3370 File Offset: 0x000C1570
	private static void LoadCostumeFromBlueprint(Dictionary<string, object> data, string theKey, Blueprint blueprint, EntityManager mgr, Paperdoll.PaperdollType paperdollType)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)data[theKey];
		PaperdollSkin paperdollSkin = null;
		Dictionary<string, PaperdollSkin> dictionary2 = new Dictionary<string, PaperdollSkin>();
		if (theKey.Equals("costumes"))
		{
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				paperdollSkin = new PaperdollSkin();
				paperdollSkin.name = keyValuePair.Key;
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)keyValuePair.Value;
				object obj = null;
				if (dictionary3.TryGetValue("skeleton", out obj))
				{
					foreach (KeyValuePair<string, object> keyValuePair2 in ((Dictionary<string, object>)obj))
					{
						paperdollSkin.skeletons.Add(keyValuePair2.Key, (string)keyValuePair2.Value);
					}
					paperdollSkin.skeletonKey = paperdollSkin.skeletons.Keys.First<string>();
					paperdollSkin.skeletonReplacement = paperdollSkin.skeletons.Values.First<string>();
				}
				object obj2 = null;
				if (dictionary3.TryGetValue("props", out obj2))
				{
					List<object> list = (List<object>)obj2;
					for (int i = 0; i < list.Count; i++)
					{
						Dictionary<string, object> dictionary4 = (Dictionary<string, object>)list[i];
						Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
						foreach (KeyValuePair<string, object> keyValuePair3 in dictionary4)
						{
							dictionary5.Add(keyValuePair3.Key, (string)keyValuePair3.Value);
						}
						paperdollSkin.propData.Add(dictionary5);
					}
				}
				dictionary2.Add(paperdollSkin.name, paperdollSkin);
			}
			blueprint.Invariable[theKey] = dictionary2;
		}
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x000C35A8 File Offset: 0x000C17A8
	private static void LoadDisplayController(Dictionary<string, object> data, string theKey, Blueprint blueprint, EntityManager mgr, Paperdoll.PaperdollType paperdollType)
	{
		bool condition = false;
		string text = theKey;
		if (CommonUtils.TextureLod() < CommonUtils.LevelOfDetail.Standard && data.ContainsKey(theKey + "_lr"))
		{
			theKey += "_lr";
		}
		IDisplayController displayController = null;
		Dictionary<string, object> dictionary = (Dictionary<string, object>)data[theKey];
		if (dictionary.ContainsKey("model_type"))
		{
			string text2 = (string)dictionary["model_type"];
			if (text2.Equals("sprite"))
			{
				float num = 1f;
				if (dictionary.ContainsKey("scale"))
				{
					num = TFUtils.LoadFloat(dictionary, "scale");
				}
				string text3 = theKey + ".default";
				TFUtils.Assert(data.ContainsKey(text3), string.Concat(new object[]
				{
					"All sprites must have a *.default defined!\nMissing ",
					text3,
					" in blueprint ",
					blueprint
				}));
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)data[text3];
				float num2 = 0f;
				float num3 = 0f;
				object value = null;
				object value2 = null;
				if (dictionary2.TryGetValue("width", out value) && dictionary2.TryGetValue("height", out value2))
				{
					num2 = (float)Convert.ToInt32(value);
					num3 = (float)Convert.ToInt32(value2);
				}
				else
				{
					object obj = null;
					if (dictionary2.TryGetValue("texture", out obj))
					{
						string text4 = (string)obj;
						TFUtils.Assert(YGTextureLibrary.HasAtlasCoords(text4), "The texture atlas does not have an entry for " + text4);
						AtlasCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(text4).atlasCoords;
						num2 = (float)TFAnimatedSprite.CalcWorldSize((double)atlasCoords.spriteSize.width, 1.0);
						num3 = (float)TFAnimatedSprite.CalcWorldSize((double)atlasCoords.spriteSize.height, 1.0);
					}
				}
				displayController = EntityManager.CreateAnimatedSpritePrototype(dictionary, text, blueprint, num2 * num, num3 * num, data);
			}
			else if (text2.Equals("paperdoll"))
			{
				displayController = EntityManager.CreatePaperdollPrototype(dictionary, text, blueprint, paperdollType);
			}
			if (dictionary.ContainsKey("perspective_in_art"))
			{
				displayController.isPerspectiveInArt = (bool)dictionary["perspective_in_art"];
			}
			blueprint.Invariable[text] = displayController;
		}
		foreach (string text5 in data.Keys)
		{
			if (text5.Length > theKey.Length && text5.Substring(0, theKey.Length).Equals(theKey) && text5[theKey.Length] == '.')
			{
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)data[text5];
				object obj2 = null;
				if (dictionary3.TryGetValue("quad", out obj2))
				{
					EntityManager.RegisterDisplayOffset((Dictionary<string, object>)obj2, text5, blueprint);
				}
				displayController.AddDisplayState(dictionary3);
				if (text5.Contains("default"))
				{
					condition = true;
				}
			}
		}
		TFUtils.Assert(condition, string.Concat(new string[]
		{
			"EntityManager.LoadDisplayController(): '",
			theKey,
			"' is missing a '",
			theKey,
			".default' in the blueprint."
		}));
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x000C38F4 File Offset: 0x000C1AF4
	private static void LoadEffects(Dictionary<string, object> data, Blueprint blueprint)
	{
		string text = "fx";
		foreach (string text2 in data.Keys)
		{
			if (text2.Length > text.Length && text2.Substring(0, text.Length).Equals(text) && text2[text.Length] == '.')
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)data[text2];
				string text3 = (string)dictionary["type"];
				if (!text3.Equals("particles"))
				{
					throw new NotImplementedException("fx of type " + text3 + " not supported.");
				}
				ParticleSystemManager.Request request = new ParticleSystemManager.Request();
				request.effectsName = (string)dictionary["fx_name"];
				request.initialPriority = TFUtils.LoadInt(dictionary, "initial_priority");
				request.subsequentPriority = TFUtils.LoadInt(dictionary, "sub_priority");
				request.cyclingPeriod = TFUtils.LoadFloat(dictionary, "cycling_period");
				Vector3 zero = Vector3.zero;
				if (dictionary.ContainsKey("position_offset"))
				{
					TFUtils.LoadVector3(out zero, (Dictionary<string, object>)dictionary["position_offset"]);
				}
				blueprint.Invariable[text2 + ".position_offset"] = zero;
				blueprint.Invariable[text2] = request;
			}
		}
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x000C3A90 File Offset: 0x000C1C90
	private static Blueprint MarshallCommon(Dictionary<string, object> data, int width, int height, EntityManager mgr)
	{
		Blueprint blueprint = new Blueprint();
		int num = TFUtils.LoadInt(data, "did");
		blueprint.Invariable["name"] = (string)data["name"];
		blueprint.Invariable["type"] = EntityTypeNamingHelper.StringToType(TFUtils.LoadString(data, "type"));
		blueprint.Invariable["did"] = num;
		blueprint.Invariable["blueprint"] = EntityTypeNamingHelper.GetBlueprintName((string)data["type"], num);
		blueprint.Invariable["footprint"] = new AlignedBox(0f, (float)width, 0f, (float)height);
		blueprint.Invariable["footprintSprite"] = new BasicSprite("Materials/unique/footprint", null, new Vector2(-0.5f * (float)width, -0.5f * (float)height), (float)width, (float)height);
		blueprint.Invariable["footprint.flip"] = new AlignedBox(0f, (float)height, 0f, (float)width);
		blueprint.Invariable["display.position_offset"] = Vector2.zero;
		blueprint.Invariable["dropshadow"] = null;
		blueprint.Invariable["debugBoxSprite"] = new BasicSprite("Materials/unique/footprint", null, new Vector2(-0.5f * (float)width, -0.5f * (float)height), (float)width, (float)height);
		object obj = null;
		if (data.TryGetValue("sound_on_select", out obj))
		{
			blueprint.Invariable["sound_on_select"] = obj;
		}
		else
		{
			blueprint.Invariable["sound_on_select"] = "SelectSimulated";
		}
		if (data.ContainsKey("disabled"))
		{
			blueprint.Invariable["disabled"] = TFUtils.LoadBool(data, "disabled");
		}
		blueprint.Invariable["sound_on_touch_error"] = "Error";
		blueprint.Invariable["sound_on_touch"] = "TouchSimulated";
		if (data.TryGetValue("sound_on_touch", out obj))
		{
			blueprint.Invariable["sound_on_touch"] = obj;
		}
		if (data.TryGetValue("thought_display_movement", out obj))
		{
			blueprint.Invariable["thought_display_movement"] = (bool)obj;
		}
		else
		{
			blueprint.Invariable["thought_display_movement"] = true;
		}
		if (data.TryGetValue("instance_limit", out obj))
		{
			blueprint.Invariable["instance_limit"] = AmountDictionary.FromJSONDict((Dictionary<string, object>)obj);
			Dictionary<int, int> dictionary = (Dictionary<int, int>)blueprint.Invariable["instance_limit"];
			TFUtils.Assert(dictionary.ContainsKey(1), "No limit set at level 1");
		}
		else
		{
			blueprint.Invariable["instance_limit"] = new Dictionary<int, int>();
		}
		EntityManager.LoadEffects(data, blueprint);
		return blueprint;
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x000C3D84 File Offset: 0x000C1F84
	private static BasicSprite CreateDropShadow(float width, float height)
	{
		float num = width * 0.5f;
		float num2 = height * 0.5f;
		return new BasicSprite(null, "dropshadow.tga", new Vector2(-0.1f * num, -0.1f * num2), num, num2);
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x000C3DC4 File Offset: 0x000C1FC4
	private static void LoadUnitsFromSpread()
	{
		string text = "Units";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "max paytables");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "wishtable did");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "speed");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "disabled");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "join paytables");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "won't go home");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "disable if will flee");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "gross item wishtable did");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "forbidden item wishtable did");
		string value = CommonUtils.PropertyForDeviceOverride("disable_lr_models");
		bool flag = false;
		if (!string.IsNullOrEmpty(value))
		{
			flag = true;
		}
		string a = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3);
				}
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet4);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("did", intCell);
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6));
					dictionary.Add("wish_table_did", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
					dictionary.Add("gross_items_wish_table_id", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14));
					dictionary.Add("forbidden_items_wish_table_id", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15));
					dictionary.Add("speed", instance.GetFloatCell(sheetIndex, rowIndex, columnIndexInSheet8));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet9));
					dictionary.Add("disabled", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10) == 1);
					dictionary.Add("join_paytables", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11) == 1);
					dictionary.Add("go_home_exempt", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12) == 1);
					dictionary.Add("disable_if_will_flee", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13) == 1);
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "drop shadow diameter");
					if (intCell2 >= 0)
					{
						dictionary.Add("dropshadow_diameter", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "default costume did");
					if (intCell2 >= 0)
					{
						dictionary.Add("default_costume_did", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "hungry timer");
					if (intCell2 >= 0)
					{
						dictionary.Add("time.hungry", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "timer duration");
					if (intCell2 >= 0)
					{
						dictionary.Add("timer_duration", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "hide duration");
					if (intCell2 >= 0)
					{
						dictionary.Add("hide_duration", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "autoquest intro");
					if (intCell2 >= 0)
					{
						dictionary.Add("auto_quest_intro", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "autoquest outro");
					if (intCell2 >= 0)
					{
						dictionary.Add("auto_quest_outro", intCell2);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "character dialog portrait");
					if (a != stringCell)
					{
						dictionary.Add("character_dialog_portrait", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "quest reminder icon");
					if (a != stringCell)
					{
						dictionary.Add("quest_reminder_icon", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "sound on touch");
					if (a != stringCell)
					{
						dictionary.Add("sound_on_touch", stringCell);
					}
					dictionary.Add("display", new Dictionary<string, object>
					{
						{
							"model_type",
							instance.GetStringCell(text, rowName, "character model type")
						},
						{
							"width",
							instance.GetIntCell(sheetIndex, rowIndex, "character model width")
						},
						{
							"height",
							instance.GetIntCell(sheetIndex, rowIndex, "character model height")
						},
						{
							"display_scale",
							new Dictionary<string, object>
							{
								{
									"x",
									instance.GetFloatCell(sheetIndex, rowIndex, "character model scale x")
								},
								{
									"y",
									instance.GetFloatCell(sheetIndex, rowIndex, "character model scale y")
								},
								{
									"z",
									instance.GetFloatCell(sheetIndex, rowIndex, "character model scale z")
								}
							}
						},
						{
							"position_offset",
							new Dictionary<string, object>
							{
								{
									"x",
									instance.GetFloatCell(sheetIndex, rowIndex, "character model offset x")
								},
								{
									"y",
									instance.GetFloatCell(sheetIndex, rowIndex, "character model offset y")
								}
							}
						}
					});
					if (!flag)
					{
						stringCell = instance.GetStringCell(sheetIndex, rowIndex, "low res character model type");
						if (a != stringCell)
						{
							dictionary.Add("display_lr", new Dictionary<string, object>
							{
								{
									"model_type",
									stringCell
								},
								{
									"width",
									instance.GetIntCell(sheetIndex, rowIndex, "low res character model width")
								},
								{
									"height",
									instance.GetIntCell(sheetIndex, rowIndex, "low res character model height")
								},
								{
									"display_scale",
									new Dictionary<string, object>
									{
										{
											"x",
											instance.GetFloatCell(sheetIndex, rowIndex, "low res character model scale x")
										},
										{
											"y",
											instance.GetFloatCell(sheetIndex, rowIndex, "low res character model scale y")
										},
										{
											"z",
											instance.GetFloatCell(sheetIndex, rowIndex, "low res character model scale z")
										}
									}
								},
								{
									"position_offset",
									new Dictionary<string, object>
									{
										{
											"x",
											instance.GetFloatCell(sheetIndex, rowIndex, "low res character model offset x")
										},
										{
											"y",
											instance.GetFloatCell(sheetIndex, rowIndex, "low res character model offset y")
										}
									}
								}
							});
						}
					}
					dictionary.Add("thought_display", new Dictionary<string, object>
					{
						{
							"model_type",
							instance.GetStringCell(sheetIndex, rowIndex, "thought model type")
						},
						{
							"position_offset",
							new Dictionary<string, object>
							{
								{
									"x",
									instance.GetIntCell(sheetIndex, rowIndex, "thought model offset x")
								},
								{
									"y",
									instance.GetIntCell(sheetIndex, rowIndex, "thought model offset y")
								}
							}
						}
					});
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "thought quad width");
					if (intCell2 >= 0)
					{
						((Dictionary<string, object>)dictionary["thought_display"]).Add("quad", new Dictionary<string, object>
						{
							{
								"width",
								intCell2
							},
							{
								"height",
								instance.GetIntCell(sheetIndex, rowIndex, "thought quad height")
							}
						});
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox width");
					if (intCell2 >= 0)
					{
						((Dictionary<string, object>)dictionary["thought_display"]).Add("hit_area", new Dictionary<string, object>
						{
							{
								"width",
								intCell2
							},
							{
								"height",
								instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox height")
							},
							{
								"center",
								new Dictionary<string, object>
								{
									{
										"x",
										instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox center x")
									},
									{
										"y",
										instance.GetIntCell(sheetIndex, rowIndex, "thought tapbox center y")
									}
								}
							}
						});
					}
					List<object> list = new List<object>();
					for (int j = 0; j < num2; j++)
					{
						intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "bonus paytable did " + (j + 1).ToString());
						if (intCell2 >= 0)
						{
							list.Add(intCell2);
						}
					}
					if (list.Count > 0)
					{
						dictionary.Add("match_bonus_paytables", list);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "wish cooldown min");
					if (intCell2 >= 0)
					{
						dictionary.Add("wishing", new Dictionary<string, object>
						{
							{
								"wish_cooldown_min",
								intCell2
							},
							{
								"wish_cooldown_max",
								instance.GetIntCell(sheetIndex, rowIndex, "wish cooldown max")
							},
							{
								"wish_duration",
								instance.GetIntCell(sheetIndex, rowIndex, "wish duration")
							}
						});
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "idle cooldown min");
					if (intCell2 >= 0)
					{
						dictionary.Add("idle", new Dictionary<string, object>
						{
							{
								"cooldown",
								new Dictionary<string, object>
								{
									{
										"min",
										intCell2
									},
									{
										"max",
										instance.GetIntCell(sheetIndex, rowIndex, "idle cooldown max")
									}
								}
							},
							{
								"duration",
								new Dictionary<string, object>
								{
									{
										"min",
										instance.GetIntCell(sheetIndex, rowIndex, "idle duration min")
									},
									{
										"max",
										instance.GetIntCell(sheetIndex, rowIndex, "idle duration max")
									}
								}
							}
						});
					}
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x000C48D4 File Offset: 0x000C2AD4
	private void LoadAnnexesFromSpread()
	{
		string text = "Annexes";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		string a = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (num2 < 0)
					{
						num2 = instance.GetIntCell(sheetIndex, rowIndex, "instance limits");
					}
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
					dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
					dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "build timer duration"));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
					dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
					dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
					dictionary.Add("stashable", instance.GetIntCell(sheetIndex, rowIndex, "stashable") == 1);
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent time");
					int? num3 = (intCell2 < 0) ? null : new int?(intCell2);
					dictionary.Add("rent_time", num3);
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "resident");
					num3 = ((intCell2 < 0) ? null : new int?(intCell2));
					dictionary.Add("resident", num3);
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "hub info did");
					if (intCell2 >= 0)
					{
						dictionary.Add("hub_info", new Dictionary<string, object>
						{
							{
								"did",
								intCell2
							}
						});
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "accept placement sound");
					if (a != stringCell)
					{
						dictionary.Add("accept_placement_sound", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "hub");
					if (a != stringCell)
					{
						dictionary.Add("hub", stringCell);
					}
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					dictionary.Add("completion_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
								}
							}
						},
						{
							"thought_icon",
							instance.GetStringCell(text, rowName, "completion reward thought icon")
						}
					});
					dictionary.Add("instance_limit", new Dictionary<string, object>());
					for (int j = 1; j <= num2; j++)
					{
						((Dictionary<string, object>)dictionary["instance_limit"]).Add(j.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "instance limit level " + j.ToString()));
					}
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x000C4DCC File Offset: 0x000C2FCC
	private void LoadCharacterBuildingsFromSpread()
	{
		string text = "CharacterBuildings";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "level min");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "build time");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "build timer duration");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "rent time");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "rent timer duration");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "portrait");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "flippable");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "has move in");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "rent rushable");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "completion sound");
		string a = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (num2 < 0)
					{
						num2 = instance.GetIntCell(sheetIndex, rowIndex, "num residents");
					}
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6));
					dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
					dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8));
					dictionary.Add("rent_time", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9));
					dictionary.Add("rent_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet11));
					dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet12));
					dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13) == 1);
					dictionary.Add("has_move_in", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14) == 1);
					dictionary.Add("rent_rushable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15) == 1);
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet16);
					if (a != stringCell)
					{
						dictionary.Add("completion_sound", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "sound on select");
					if (a != stringCell)
					{
						dictionary.Add("sound_on_select", stringCell);
					}
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					int? num3 = new int?(instance.GetIntCell(sheetIndex, rowIndex, "resident 1"));
					if (num3.Value < 0)
					{
						dictionary.Add("resident", null);
					}
					else
					{
						dictionary.Add("residents", new List<int>());
						((List<int>)dictionary["residents"]).Add(num3.Value);
						for (int j = 2; j <= num2; j++)
						{
							num3 = new int?(instance.GetIntCell(sheetIndex, rowIndex, "resident " + j.ToString()));
							if (num3.Value >= 0)
							{
								((List<int>)dictionary["residents"]).Add(num3.Value);
							}
						}
					}
					dictionary.Add("completion_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
								}
							}
						},
						{
							"thought_icon",
							instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
						}
					});
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources gold");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["completion_reward"])["resources"]).Add("3", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward movie");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)dictionary["completion_reward"]).Add("movies", new Dictionary<string, object>
						{
							{
								intCell2.ToString(),
								1
							}
						});
					}
					dictionary.Add("product", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>()
						},
						{
							"summary",
							new Dictionary<string, object>
							{
								{
									"thought_icon",
									null
								}
							}
						},
						{
							"thought_icon",
							null
						}
					});
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources gold");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("3", intCell2);
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).Add("resources", new Dictionary<string, object>
						{
							{
								"3",
								intCell2
							}
						});
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources xp");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("5", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product special did");
					if (intCell2 >= 0)
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add(intCell2.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "product special amount"));
					}
					dictionary.Add("instance_limit", new Dictionary<string, object>
					{
						{
							"1",
							instance.GetIntCell(sheetIndex, rowIndex, "instance limit 1")
						}
					});
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x0600202D RID: 8237 RVA: 0x000C5598 File Offset: 0x000C3798
	private void LoadDebrisFromSpread()
	{
		string text = "Debris";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (num2 < 0)
					{
						num2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing jelly reward columns");
					}
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("clear_time", instance.GetIntCell(sheetIndex, rowIndex, "clear time"));
					dictionary.Add("timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "timer duration"));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
					dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					dictionary.Add("cost", new Dictionary<string, object>
					{
						{
							"3",
							instance.GetIntCell(sheetIndex, rowIndex, "cost gold")
						}
					});
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward xp");
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "clearing reward thought icon");
					dictionary.Add("clearing_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									intCell2
								}
							}
						},
						{
							"thought_icon",
							stringCell
						},
						{
							"summary",
							new Dictionary<string, object>
							{
								{
									"thought_icon",
									stringCell
								},
								{
									"resources",
									new Dictionary<string, object>
									{
										{
											"5",
											intCell2
										}
									}
								}
							}
						}
					});
					for (int j = 1; j <= num2; j++)
					{
						intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing jelly reward amount " + j.ToString());
						if (intCell2 < 0)
						{
							break;
						}
						if (j == 1)
						{
							((Dictionary<string, object>)((Dictionary<string, object>)dictionary["clearing_reward"])["resources"]).Add("2", new Dictionary<string, object>());
						}
						((Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)dictionary["clearing_reward"])["resources"])["2"]).Add(intCell2.ToString(), instance.GetFloatCell(sheetIndex, rowIndex, "clearing jelly reward odds " + j.ToString()));
					}
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x000C5A24 File Offset: 0x000C3C24
	private void LoadDecorationsFromSpread()
	{
		string text = "Decorations";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
					dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
					dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
					dictionary.Add("completion_sound", instance.GetStringCell(sheetIndex, rowIndex, "completion sound"));
					dictionary.Add("accept_placement_sound", instance.GetStringCell(sheetIndex, rowIndex, "accept placement sound"));
					dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
					dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, "flippable") == 1);
					dictionary.Add("shareable_space", instance.GetIntCell(sheetIndex, rowIndex, "shareable space") == 1);
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(text, rowName, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					dictionary.Add("completion_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									instance.GetIntCell(sheetIndex, rowIndex, "completion reward xp")
								}
							}
						},
						{
							"thought_icon",
							instance.GetStringCell(text, rowName, "completion reward thought icon")
						}
					});
					dictionary.Add("rent", null);
					dictionary.Add("rent_time", null);
					dictionary.Add("resident", null);
					dictionary.Add("product", null);
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x000C5DE4 File Offset: 0x000C3FE4
	private void LoadLandmarksFromSpread()
	{
		string text = "Landmarks";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(text, rowName, "name"));
					dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(text, rowName, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000C603C File Offset: 0x000C423C
	private void LoadRentOnlyBuildingsFromSpread()
	{
		string text = "RentOnlyBuildings";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		string a = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (num2 < 0)
					{
						num2 = instance.GetIntCell(sheetIndex, rowIndex, "instance limits");
					}
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
					dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
					dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "build timer duration"));
					dictionary.Add("rent_time", instance.GetIntCell(sheetIndex, rowIndex, "rent time"));
					dictionary.Add("rent_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "rent timer duration"));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
					dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
					dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, "flippable") == 1);
					dictionary.Add("has_move_in", instance.GetIntCell(sheetIndex, rowIndex, "has move in") == 1);
					dictionary.Add("rent_rushable", instance.GetIntCell(sheetIndex, rowIndex, "rent rushable") == 1);
					dictionary.Add("stashable", instance.GetIntCell(sheetIndex, rowIndex, "stashable") == 1);
					dictionary.Add("worker_spawner", instance.GetIntCell(sheetIndex, rowIndex, "worker spawner") == 1);
					stringCell = instance.GetStringCell(text, rowName, "completion sound");
					if (a != stringCell)
					{
						dictionary.Add("completion_sound", stringCell);
					}
					stringCell = instance.GetStringCell(text, rowName, "sound on select");
					if (a != stringCell)
					{
						dictionary.Add("sound_on_select", stringCell);
					}
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					dictionary.Add("completion_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
								}
							}
						},
						{
							"thought_icon",
							instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
						}
					});
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward movie");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)dictionary["completion_reward"]).Add("movies", new Dictionary<string, object>
						{
							{
								intCell2.ToString(),
								1
							}
						});
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources gold");
					if (intCell2 > 0)
					{
						dictionary.Add("product", new Dictionary<string, object>
						{
							{
								"resources",
								new Dictionary<string, object>()
							},
							{
								"summary",
								new Dictionary<string, object>
								{
									{
										"thought_icon",
										null
									}
								}
							},
							{
								"thought_icon",
								null
							}
						});
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("3", intCell2);
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).Add("resources", new Dictionary<string, object>
						{
							{
								"3",
								intCell2
							}
						});
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product resources xp");
					if (intCell2 > 0)
					{
						if (!dictionary.ContainsKey("product"))
						{
							dictionary.Add("product", new Dictionary<string, object>
							{
								{
									"resources",
									new Dictionary<string, object>()
								},
								{
									"summary",
									new Dictionary<string, object>
									{
										{
											"thought_icon",
											null
										}
									}
								},
								{
									"thought_icon",
									null
								}
							});
						}
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add("5", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product special did");
					if (intCell2 >= 0)
					{
						if (!dictionary.ContainsKey("product"))
						{
							dictionary.Add("product", new Dictionary<string, object>
							{
								{
									"resources",
									new Dictionary<string, object>()
								},
								{
									"summary",
									new Dictionary<string, object>
									{
										{
											"thought_icon",
											null
										}
									}
								},
								{
									"thought_icon",
									null
								}
							});
						}
						int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "product special amount");
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["resources"]).Add(intCell2.ToString(), intCell3);
						if (((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).ContainsKey("resources"))
						{
							((Dictionary<string, object>)((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"])["resources"]).Add(intCell2.ToString(), intCell3);
						}
						else
						{
							((Dictionary<string, object>)((Dictionary<string, object>)dictionary["product"])["summary"]).Add("resources", new Dictionary<string, object>
							{
								{
									intCell2.ToString(),
									intCell3
								}
							});
						}
					}
					dictionary.Add("instance_limit", new Dictionary<string, object>());
					for (int j = 1; j <= num2; j++)
					{
						((Dictionary<string, object>)dictionary["instance_limit"]).Add(j.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "instance limit level " + j.ToString()));
					}
					dictionary.Add("resident", null);
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000C6880 File Offset: 0x000C4A80
	private void LoadShopsFromSpread()
	{
		string text = "Shops";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "level min");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "build time");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "build timer duration");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "portrait");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "flippable");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "has move in");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "stashable");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "shunts crafting");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "is waypoint");
		string a = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (num2 < 0)
					{
						num2 = instance.GetIntCell(sheetIndex, rowIndex, "num residents");
					}
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
					dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8));
					dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet6));
					dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet10));
					dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11) == 1);
					dictionary.Add("has_move_in", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12) == 1);
					dictionary.Add("stashable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13) == 1);
					dictionary.Add("shunts_crafting", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14) == 1);
					dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15) == 1);
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent time");
					int? num3 = (intCell2 < 0) ? null : new int?(intCell2);
					dictionary.Add("rent_time", num3);
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent timer duration");
					if (intCell2 > 0)
					{
						dictionary.Add("rent_timer_duration", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "crafting menu");
					if (intCell2 > 0)
					{
						dictionary.Add("crafting_menu", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "crafting timer duration");
					if (intCell2 > 0)
					{
						dictionary.Add("crafting_timer_duration", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "vendor id");
					if (intCell2 > 0)
					{
						dictionary.Add("vendor_id", intCell2);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "completion sound");
					if (a != stringCell)
					{
						dictionary.Add("completion_sound", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "sound on select");
					if (a != stringCell)
					{
						dictionary.Add("sound_on_select", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "crafted icon");
					if (a != stringCell)
					{
						dictionary.Add("crafted_icon", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "blueprint");
					if (a != stringCell)
					{
						dictionary.Add("blueprint", stringCell);
					}
					num3 = new int?(instance.GetIntCell(sheetIndex, rowIndex, "resident 1"));
					if (num3.Value < 0)
					{
						dictionary.Add("resident", null);
					}
					else
					{
						dictionary.Add("residents", new List<int>());
						((List<int>)dictionary["residents"]).Add(num3.Value);
						for (int j = 2; j <= num2; j++)
						{
							num3 = new int?(instance.GetIntCell(sheetIndex, rowIndex, "resident " + j.ToString()));
							if (num3.Value >= 0)
							{
								((List<int>)dictionary["residents"]).Add(num3.Value);
							}
						}
					}
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					dictionary.Add("completion_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
								}
							}
						},
						{
							"thought_icon",
							instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
						}
					});
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "completion reward movie");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)dictionary["completion_reward"]).Add("movies", new Dictionary<string, object>
						{
							{
								intCell2.ToString(),
								1
							}
						});
					}
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000C6F90 File Offset: 0x000C5190
	private void LoadTreasureFromSpread()
	{
		string text = "Treasure";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		int num2 = -1;
		int[] array = new int[0];
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string text2 = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(text2, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (num2 < 0)
					{
						num2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward sets");
						array = new int[num2];
						for (int j = 1; j <= num2; j++)
						{
							array[j - 1] = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward " + j.ToString() + " columns");
						}
					}
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
					dictionary.Add("timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "timer duration"));
					dictionary.Add("clear_time", instance.GetIntCell(sheetIndex, rowIndex, "clear time"));
					dictionary.Add("type", text2);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
					dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
					dictionary.Add("quick_clear", instance.GetIntCell(sheetIndex, rowIndex, "quick clear") == 1);
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					text2 = instance.GetStringCell(sheetIndex, rowIndex, "thought icon");
					dictionary.Add("clearing_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>()
						},
						{
							"thought_icon",
							text2
						},
						{
							"summary",
							new Dictionary<string, object>
							{
								{
									"thought_icon",
									text2
								}
							}
						}
					});
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)((Dictionary<string, object>)dictionary["clearing_reward"])["resources"];
					for (int k = 1; k <= num2; k++)
					{
						int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward " + k.ToString() + " did");
						if (intCell2 >= 0)
						{
							text2 = intCell2.ToString();
							if (!dictionary2.ContainsKey(text2))
							{
								dictionary2.Add(text2, new Dictionary<string, object>());
							}
							for (int l = 1; l <= array[k - 1]; l++)
							{
								intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "clearing reward " + k.ToString() + " amount " + l.ToString());
								if (intCell2 >= 0 && !((Dictionary<string, object>)dictionary2[text2]).ContainsKey(intCell2.ToString()))
								{
									((Dictionary<string, object>)dictionary2[text2]).Add(intCell2.ToString(), instance.GetFloatCell(sheetIndex, rowIndex, "clearing reward " + k.ToString() + " odds " + l.ToString()));
								}
							}
						}
					}
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000C7474 File Offset: 0x000C5674
	private void LoadTreesFromSpread()
	{
		string text = "Trees";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "width");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "height");
		string a = "n/a";
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3);
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(stringCell, intCell);
				if (!EntityManager._pBpSpreadData.ContainsKey(blueprintName))
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (num2 < 0)
					{
						num2 = instance.GetIntCell(sheetIndex, rowIndex, "product sets");
						num3 = instance.GetIntCell(sheetIndex, rowIndex, "instance limits");
					}
					dictionary.Add("did", intCell);
					dictionary.Add("height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary.Add("width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
					dictionary.Add("level_min", instance.GetIntCell(sheetIndex, rowIndex, "level min"));
					dictionary.Add("build_time", instance.GetIntCell(sheetIndex, rowIndex, "build time"));
					dictionary.Add("build_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "build timer duration"));
					dictionary.Add("crafting_menu", instance.GetIntCell(sheetIndex, rowIndex, "crafting menu"));
					dictionary.Add("crafting_timer_duration", instance.GetIntCell(sheetIndex, rowIndex, "crafting timer duration"));
					dictionary.Add("type", stringCell);
					dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "name"));
					dictionary.Add("portrait", instance.GetStringCell(sheetIndex, rowIndex, "portrait"));
					dictionary.Add("flippable", instance.GetIntCell(sheetIndex, rowIndex, "flippable") == 1);
					dictionary.Add("is_waypoint", instance.GetIntCell(sheetIndex, rowIndex, "is waypoint") == 1);
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent time");
					int? num4 = (intCell2 < 0) ? null : new int?(intCell2);
					dictionary.Add("rent_time", num4);
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "rent timer duration");
					if (intCell2 > 0)
					{
						dictionary.Add("rent_timer_duration", intCell2);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "completion sound");
					if (a != stringCell)
					{
						dictionary.Add("completion_sound", stringCell);
					}
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "accept placement sound");
					if (a != stringCell)
					{
						dictionary.Add("accept_placement_sound", stringCell);
					}
					dictionary.Add("point_of_interest", new Dictionary<string, object>
					{
						{
							"facing",
							instance.GetStringCell(sheetIndex, rowIndex, "point of interest facing")
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest x")
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, "point of interest y")
						}
					});
					dictionary.Add("completion_reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
								}
							}
						},
						{
							"summary",
							new Dictionary<string, object>
							{
								{
									"resources",
									new Dictionary<string, object>
									{
										{
											"5",
											instance.GetIntCell(sheetIndex, rowIndex, "completion reward resources xp")
										}
									}
								},
								{
									"thought_icon",
									instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
								}
							}
						},
						{
							"thought_icon",
							instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
						}
					});
					int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "fruit did");
					dictionary.Add("product", new Dictionary<string, object>
					{
						{
							"cdf",
							new List<object>()
						},
						{
							"summary",
							new Dictionary<string, object>
							{
								{
									"resources",
									new Dictionary<string, object>
									{
										{
											intCell3.ToString(),
											instance.GetIntCell(sheetIndex, rowIndex, "product summary")
										}
									}
								},
								{
									"thought_icon",
									instance.GetStringCell(sheetIndex, rowIndex, "completion reward thought icon")
								}
							}
						}
					});
					for (int j = 1; j <= num2; j++)
					{
						intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "product fruit " + j.ToString());
						if (intCell2 >= 0)
						{
							((List<object>)((Dictionary<string, object>)dictionary["product"])["cdf"]).Add(new Dictionary<string, object>
							{
								{
									"p",
									instance.GetFloatCell(sheetIndex, rowIndex, "product odds " + j.ToString())
								},
								{
									"value",
									new Dictionary<string, object>
									{
										{
											"resources",
											new Dictionary<string, object>
											{
												{
													"5",
													instance.GetIntCell(sheetIndex, rowIndex, "product xp " + j.ToString())
												},
												{
													intCell3.ToString(),
													intCell2
												}
											}
										}
									}
								}
							});
						}
					}
					dictionary.Add("instance_limit", new Dictionary<string, object>());
					for (int k = 1; k <= num3; k++)
					{
						((Dictionary<string, object>)dictionary["instance_limit"]).Add(k.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "instance limit " + k.ToString()));
					}
					dictionary.Add("resident", null);
					EntityManager._pBpSpreadData.Add(blueprintName, dictionary);
				}
			}
		}
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000C7B6C File Offset: 0x000C5D6C
	private void OverwriteBlueprintDataWithSpread(Dictionary<string, object> data)
	{
		if (data == null || EntityManager._pBpSpreadData == null)
		{
			return;
		}
		int did = -1;
		string text = string.Empty;
		if (data.ContainsKey("did"))
		{
			did = TFUtils.LoadInt(data, "did");
		}
		if (data.ContainsKey("type"))
		{
			text = TFUtils.LoadString(data, "type");
		}
		text = EntityTypeNamingHelper.GetBlueprintName(text, did);
		if (!EntityManager._pBpSpreadData.ContainsKey(text))
		{
			return;
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)EntityManager._pBpSpreadData[text];
		foreach (KeyValuePair<string, object> keyValuePair in dictionary)
		{
			if (data.ContainsKey(keyValuePair.Key))
			{
				data[keyValuePair.Key] = keyValuePair.Value;
			}
			else
			{
				data.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x000C7C84 File Offset: 0x000C5E84
	private static Blueprint MarshallUnit(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = EntityManager.MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["base_speed"] = TFUtils.LoadFloat(data, "speed");
		blueprint.Invariable["machine"] = EntityManager.UnitMachine;
		blueprint.Invariable["action"] = EntityManager.ResidentActions["idle"];
		blueprint.Invariable["immobile"] = false;
		blueprint.Invariable["join_paytables"] = TFUtils.TryLoadBool(data, "join_paytables");
		blueprint.Variable["speed"] = blueprint.Invariable["base_speed"];
		List<string> list = new List<string>();
		object obj = null;
		if (data.TryGetValue("task_open_sounds", out obj))
		{
			foreach (object obj2 in (obj as List<object>))
			{
				list.Add((string)obj2);
			}
			obj = null;
		}
		else
		{
			list.Add("OpenMenu");
		}
		blueprint.Invariable["task_open_sounds"] = list;
		List<string> list2 = new List<string>();
		if (data.TryGetValue("task_selected_sounds", out obj))
		{
			foreach (object obj3 in (obj as List<object>))
			{
				list2.Add((string)obj3);
			}
			obj = null;
		}
		else
		{
			list2.Add("TaskSelected");
		}
		blueprint.Invariable["task_selected_sounds"] = list2;
		EntityManager.MarshallWishingInfo(ref blueprint, data);
		EntityManager.MarshallBonusInfo(ref blueprint, data);
		if (data.ContainsKey("wish_table_did"))
		{
			blueprint.Invariable["wish_table_did"] = TFUtils.LoadInt(data, "wish_table_did");
		}
		else
		{
			blueprint.Invariable["wish_table_did"] = -1;
			TFUtils.Assert(false, "Resident did: " + blueprint.Invariable["did"].ToString() + " does not have a wish table did.");
		}
		if (data.ContainsKey("gross_items_wish_table_id"))
		{
			blueprint.Invariable["gross_items_wish_table_id"] = TFUtils.LoadInt(data, "gross_items_wish_table_id");
		}
		else
		{
			blueprint.Invariable["gross_items_wish_table_id"] = -1;
			TFUtils.Assert(false, "Resident did: " + blueprint.Invariable["gross_items_wish_table_id"].ToString() + " does not have a gross item table did.");
		}
		if (data.ContainsKey("forbidden_items_wish_table_id"))
		{
			blueprint.Invariable["forbidden_items_wish_table_id"] = TFUtils.LoadInt(data, "forbidden_items_wish_table_id");
		}
		else
		{
			blueprint.Invariable["forbidden_items_wish_table_id"] = -1;
			TFUtils.Assert(false, "Resident did: " + blueprint.Invariable["forbidden_items_wish_table_id"].ToString() + " does not have a forbidden item table did.");
		}
		blueprint.Invariable["favorite_reward"] = ((!data.ContainsKey("favorite_reward")) ? null : RewardDefinition.FromObject(data["favorite_reward"]));
		blueprint.Invariable["satisfaction_reward"] = ((!data.ContainsKey("satisfaction_reward")) ? null : RewardDefinition.FromObject(data["satisfaction_reward"]));
		blueprint.Invariable["timer_duration"] = TFUtils.LoadFloat(data, "timer_duration");
		if (data.ContainsKey("idle"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["idle"];
			Dictionary<string, object> d = (Dictionary<string, object>)dictionary["cooldown"];
			Dictionary<string, object> d2 = (Dictionary<string, object>)dictionary["duration"];
			blueprint.Invariable["idle.cooldown.min"] = TFUtils.LoadInt(d, "min");
			blueprint.Invariable["idle.cooldown.max"] = TFUtils.LoadInt(d, "max");
			blueprint.Invariable["idle.duration.min"] = TFUtils.LoadInt(d2, "min");
			blueprint.Invariable["idle.duration.max"] = TFUtils.LoadInt(d2, "max");
		}
		else
		{
			TFUtils.WarningLog("No values specified for idling. Using default values");
			blueprint.Invariable["idle.cooldown.min"] = 10;
			blueprint.Invariable["idle.cooldown.max"] = 45;
			blueprint.Invariable["idle.duration.min"] = 3;
			blueprint.Invariable["idle.duration.max"] = 15;
		}
		if (data.ContainsKey("go_home_exempt"))
		{
			blueprint.Invariable["go_home_exempt"] = TFUtils.LoadBool(data, "go_home_exempt");
		}
		else
		{
			blueprint.Invariable["go_home_exempt"] = false;
		}
		if (data.ContainsKey("auto_quest_intro"))
		{
			blueprint.Invariable["auto_quest_intro"] = TFUtils.LoadInt(data, "auto_quest_intro");
		}
		if (data.ContainsKey("auto_quest_outro"))
		{
			blueprint.Invariable["auto_quest_outro"] = TFUtils.LoadInt(data, "auto_quest_outro");
		}
		if (data.ContainsKey("character_dialog_portrait"))
		{
			blueprint.Invariable["character_dialog_portrait"] = TFUtils.LoadString(data, "character_dialog_portrait");
		}
		if (data.ContainsKey("quest_reminder_icon"))
		{
			blueprint.Invariable["quest_reminder_icon"] = TFUtils.LoadString(data, "quest_reminder_icon");
		}
		int? num = TFUtils.TryLoadInt(data, "default_costume_did");
		if (num != null && num.Value < 0)
		{
			num = null;
		}
		blueprint.Invariable["default_costume_did"] = num;
		return blueprint;
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000C831C File Offset: 0x000C651C
	private static Blueprint MarshallBuilding(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = EntityManager.MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		object obj = null;
		if (data.TryGetValue("portrait", out obj))
		{
			blueprint.Invariable["portrait"] = obj;
			obj = null;
		}
		blueprint.Invariable["has_move_in"] = false;
		if (data.TryGetValue("has_move_in", out obj))
		{
			blueprint.Invariable["has_move_in"] = (bool)obj;
			obj = null;
		}
		blueprint.Invariable["stashable"] = true;
		if (data.TryGetValue("stashable", out obj))
		{
			blueprint.Invariable["stashable"] = obj;
			obj = null;
		}
		blueprint.Invariable["flippable"] = true;
		if (data.TryGetValue("flippable", out obj))
		{
			blueprint.Invariable["flippable"] = obj;
			obj = null;
		}
		blueprint.Invariable["sellable"] = true;
		if (data.TryGetValue("sellable", out obj))
		{
			blueprint.Invariable["sellable"] = obj;
			obj = null;
		}
		ulong num = TFUtils.LoadUlong(data, "build_time", 0UL);
		TFUtils.Assert(num >= 0UL, string.Format("blueprint {0} is missing a build_time", data["name"]));
		blueprint.Invariable["time.build"] = num;
		if (num > 0UL)
		{
			blueprint.Invariable["build_timer_duration"] = TFUtils.LoadFloat(data, "build_timer_duration");
		}
		blueprint.Invariable["build_rush_cost"] = ResourceManager.CalculateConstructionRushCost(num);
		if (data.ContainsKey("shareable_space"))
		{
			blueprint.Invariable["shareable_space"] = TFUtils.LoadBool(data, "shareable_space");
		}
		if (data.ContainsKey("shareable_space_snap"))
		{
			blueprint.Invariable["shareable_space_snap"] = TFUtils.LoadBool(data, "shareable_space_snap");
			Debug.LogError("Does this ever reach?");
		}
		int? num2 = TFUtils.TryLoadInt(data, "level_min");
		if (num2 == null)
		{
			num2 = new int?(1);
		}
		blueprint.Invariable["level.minimum"] = num2.Value;
		blueprint.Invariable["machine"] = EntityManager.BuildingMachine;
		blueprint.Invariable["action"] = EntityManager.BuildingActions["placing"];
		if (data.ContainsKey("pet"))
		{
			blueprint.Invariable["pet"] = TFUtils.LoadNullableInt(data, "pet");
		}
		if (data.TryGetValue("point_of_interest", out obj))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)obj;
			blueprint.Invariable["point_of_interest"] = new Vector2((float)TFUtils.LoadInt(d, "x"), (float)TFUtils.LoadInt(d, "y"));
			obj = null;
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (!data.TryGetValue("product", out obj))
		{
			obj = null;
		}
		if (obj != null)
		{
			ulong num3 = TFUtils.LoadUlong(data, "rent_time", 0UL);
			blueprint.Invariable["time.production"] = num3;
			blueprint.Invariable["rent_timer_duration"] = TFUtils.LoadFloat(data, "rent_timer_duration");
			blueprint.Invariable["product"] = RewardDefinition.FromObject(data["product"]);
			bool? flag = TFUtils.TryLoadBool(data, "rent_rushable");
			if (flag == null)
			{
				flag = new bool?(true);
			}
			blueprint.Invariable["rent_rushable"] = flag.Value;
			blueprint.Invariable["rent_rush_cost"] = ResourceManager.CalculateRentRushCost(num3);
			obj = null;
		}
		else
		{
			blueprint.Invariable["time.production"] = null;
			blueprint.Invariable["product"] = null;
			blueprint.Invariable["product.amount"] = null;
		}
		if (data.TryGetValue("worker_spawner", out obj))
		{
			blueprint.Invariable["worker_spawner"] = (bool)obj;
			obj = null;
		}
		if (data.TryGetValue("is_waypoint", out obj))
		{
			blueprint.Invariable["is_waypoint"] = (bool)obj;
			obj = null;
		}
		if (data.TryGetValue("crafting_menu", out obj))
		{
			blueprint.Invariable["crafting_menu"] = (int)obj;
			blueprint.Invariable["crafting_timer_duration"] = TFUtils.LoadFloat(data, "crafting_timer_duration");
			obj = null;
			if (data.TryGetValue("crafted_icon", out obj))
			{
				blueprint.Invariable["crafted_icon"] = (string)obj;
			}
			obj = null;
			EntityManager.MarshallShuntedCraftingInfo(ref blueprint, data);
		}
		if (data.ContainsKey("vendor_id"))
		{
			blueprint.Invariable["vendor_id"] = TFUtils.LoadInt(data, "vendor_id");
			blueprint.Invariable["restock_time"] = 3600UL;
			blueprint.Invariable["special_time"] = 86400UL;
		}
		if (data.ContainsKey("taskbook_id"))
		{
			blueprint.Invariable["taskbook_id"] = TFUtils.LoadInt(data, "taskbook_id");
		}
		if (data.ContainsKey("completion_sound"))
		{
			blueprint.Invariable["completion_sound"] = TFUtils.LoadString(data, "completion_sound");
		}
		if (data.ContainsKey("accept_placement_sound"))
		{
			blueprint.Invariable["accept_placement_sound"] = TFUtils.LoadString(data, "accept_placement_sound");
		}
		blueprint.Invariable["completion_reward"] = ((!data.ContainsKey("completion_reward")) ? null : RewardDefinition.FromObject(data["completion_reward"]));
		if (data.ContainsKey("busy_annex_count"))
		{
			blueprint.Variable["busy_annex_count"] = TFUtils.LoadInt(data, "busy_annex_count");
		}
		else
		{
			blueprint.Variable["busy_annex_count"] = 0;
		}
		EntityManager.MarshalResidentInfo(ref blueprint, data);
		return blueprint;
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000C89F0 File Offset: 0x000C6BF0
	private static Blueprint MarshallAnnex(Dictionary<string, object> data, EntityManager mgr)
	{
		Blueprint blueprint = EntityManager.MarshallBuilding(data, mgr);
		blueprint.Invariable["machine"] = EntityManager.AnnexMachine;
		blueprint.Invariable["action"] = EntityManager.AnnexActions["placing"];
		EntityManager.MarshallHubInfo(ref blueprint, data);
		return blueprint;
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000C8A44 File Offset: 0x000C6C44
	private static Blueprint MarshallDebris(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = EntityManager.MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		if (data.ContainsKey("portrait"))
		{
			blueprint.Invariable["portrait"] = data["portrait"];
		}
		ulong num = TFUtils.LoadUlong(data, "clear_time", 0UL);
		blueprint.Invariable["time.clear"] = num;
		blueprint.Invariable["cost"] = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		blueprint.Invariable["timer_duration"] = TFUtils.LoadFloat(data, "timer_duration");
		blueprint.Invariable["clear_rush_cost"] = ResourceManager.CalculateDebrisRushCost(num);
		blueprint.Invariable["machine"] = EntityManager.DebrisMachine;
		blueprint.Invariable["action"] = EntityManager.DebrisActions["inactive"];
		if (data.ContainsKey("point_of_interest"))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)data["point_of_interest"];
			blueprint.Invariable["point_of_interest"] = new Vector2((float)TFUtils.LoadInt(d, "x"), (float)TFUtils.LoadInt(d, "y"));
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (data.ContainsKey("is_waypoint"))
		{
			blueprint.Invariable["is_waypoint"] = (bool)data["is_waypoint"];
		}
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(data["clearing_reward"]);
		blueprint.Invariable["clearing_reward"] = rewardDefinition;
		TFUtils.Assert(rewardDefinition.Summary.ThoughtIcon != null, "Debris must specify a reward thought icon");
		return blueprint;
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000C8C5C File Offset: 0x000C6E5C
	private static Blueprint MarshallWorker(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = EntityManager.MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = false;
		blueprint.Variable["speed"] = TFUtils.LoadFloat(data, "speed");
		blueprint.Invariable["machine"] = EntityManager.WorkerMachine;
		blueprint.Invariable["action"] = EntityManager.WorkerActions["idle"];
		return blueprint;
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x000C8CF8 File Offset: 0x000C6EF8
	private static Blueprint MarshallWanderer(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = EntityManager.MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["base_speed"] = TFUtils.LoadFloat(data, "speed");
		blueprint.Invariable["machine"] = EntityManager.WandererMachine;
		blueprint.Invariable["action"] = EntityManager.WandererActions["hidden"];
		blueprint.Invariable["immobile"] = false;
		blueprint.Invariable["hide_duration"] = TFUtils.LoadInt(data, "hide_duration");
		blueprint.Invariable["disable_if_will_flee"] = TFUtils.TryLoadBool(data, "disable_if_will_flee");
		blueprint.Variable["speed"] = blueprint.Invariable["base_speed"];
		if (data.ContainsKey("idle"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["idle"];
			Dictionary<string, object> d = (Dictionary<string, object>)dictionary["cooldown"];
			Dictionary<string, object> d2 = (Dictionary<string, object>)dictionary["duration"];
			blueprint.Invariable["idle.cooldown.min"] = TFUtils.LoadInt(d, "min");
			blueprint.Invariable["idle.cooldown.max"] = TFUtils.LoadInt(d, "max");
			blueprint.Invariable["idle.duration.min"] = TFUtils.LoadInt(d2, "min");
			blueprint.Invariable["idle.duration.max"] = TFUtils.LoadInt(d2, "max");
		}
		else
		{
			TFUtils.WarningLog("No values specified for idling. Using default values");
			blueprint.Invariable["idle.cooldown.min"] = 10;
			blueprint.Invariable["idle.cooldown.max"] = 45;
			blueprint.Invariable["idle.duration.min"] = 3;
			blueprint.Invariable["idle.duration.max"] = 15;
		}
		return blueprint;
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x000C8F28 File Offset: 0x000C7128
	private static Blueprint MarshallLandmark(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = EntityManager.MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		if (data.ContainsKey("point_of_interest"))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)data["point_of_interest"];
			blueprint.Invariable["point_of_interest"] = new Vector2((float)TFUtils.LoadInt(d, "x"), (float)TFUtils.LoadInt(d, "y"));
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (data.ContainsKey("is_waypoint"))
		{
			blueprint.Invariable["is_waypoint"] = (bool)data["is_waypoint"];
		}
		blueprint.Invariable["machine"] = EntityManager.LandmarkMachine;
		blueprint.Invariable["action"] = EntityManager.LandmarkActions["inactive"];
		return blueprint;
	}

	// Token: 0x0600203C RID: 8252 RVA: 0x000C9054 File Offset: 0x000C7254
	private static Blueprint MarshallTreasure(Dictionary<string, object> data, EntityManager mgr)
	{
		int width = TFUtils.LoadInt(data, "width");
		int height = TFUtils.LoadInt(data, "height");
		Blueprint blueprint = EntityManager.MarshallCommon(data, width, height, mgr);
		blueprint.Invariable["immobile"] = true;
		if (data.ContainsKey("portrait"))
		{
			blueprint.Invariable["portrait"] = data["portrait"];
		}
		ulong num = (ulong)TFUtils.LoadUint(data, "clear_time");
		blueprint.Invariable["time.clear"] = num;
		blueprint.Invariable["timer_duration"] = TFUtils.LoadFloat(data, "timer_duration");
		if (data.ContainsKey("quick_clear"))
		{
			blueprint.Invariable["quick_clear"] = TFUtils.LoadBool(data, "quick_clear");
		}
		else
		{
			blueprint.Invariable["quick_clear"] = false;
		}
		blueprint.Invariable["machine"] = EntityManager.TreasureMachine;
		blueprint.Invariable["action"] = EntityManager.TreasureActions["buried"];
		if (data.ContainsKey("point_of_interest"))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)data["point_of_interest"];
			blueprint.Invariable["point_of_interest"] = new Vector2((float)TFUtils.LoadInt(d, "x"), (float)TFUtils.LoadInt(d, "y"));
		}
		else
		{
			blueprint.Invariable["point_of_interest"] = new Vector2(0f, 0f);
		}
		if (data.ContainsKey("is_waypoint"))
		{
			blueprint.Invariable["is_waypoint"] = (bool)data["is_waypoint"];
		}
		RewardDefinition value = RewardDefinition.FromObject(data["clearing_reward"]);
		blueprint.Invariable["clearing_reward"] = value;
		return blueprint;
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000C9260 File Offset: 0x000C7460
	private static void MarshallWishingInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("wishing"))
		{
			Dictionary<string, object> d = TFUtils.LoadDict(data, "wishing");
			blueprint.Invariable["wish_cooldown_min"] = TFUtils.LoadInt(d, "wish_cooldown_min");
			blueprint.Invariable["wish_cooldown_max"] = TFUtils.LoadInt(d, "wish_cooldown_max");
			blueprint.Invariable["wish_duration"] = TFUtils.LoadInt(d, "wish_duration");
		}
		else if (data.ContainsKey("time.hungry"))
		{
			blueprint.Invariable["wish_cooldown_min"] = 5;
			blueprint.Invariable["wish_cooldown_max"] = 30;
			blueprint.Invariable["wish_duration"] = 60;
		}
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x000C9348 File Offset: 0x000C7548
	private static void MarshallBonusInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("match_bonus_paytables"))
		{
			blueprint.Invariable["match_bonus_paytables"] = TFUtils.LoadList<uint>(data, "match_bonus_paytables");
		}
		else
		{
			List<uint> list = new List<uint>();
			list.Add(1U);
			blueprint.Invariable["match_bonus_paytables"] = list;
		}
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x000C93A8 File Offset: 0x000C75A8
	private static void MarshalResidentInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		string text = "resident";
		string text2 = "residents";
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.Assert(!data.ContainsKey(text) || !data.ContainsKey(text2), "Cannot have both a 'residents' and 'resident' field!");
			if (data.ContainsKey(text))
			{
				List<object> list = data[text] as List<object>;
				if (list != null)
				{
					TFUtils.ErrorLog(string.Concat(new object[]
					{
						"Found list for field '",
						text,
						"'. Did you mean to use the 'residents' field?\nBlueprint=",
						blueprint
					}));
				}
			}
		}
		blueprint.Invariable[text2] = new List<int>();
		if (data.ContainsKey(text))
		{
			int? num = TFUtils.LoadNullableInt(data, text);
			if (num != null)
			{
				blueprint.Invariable[text2] = new List<int>
				{
					num.Value
				};
			}
		}
		else if (data.ContainsKey(text2))
		{
			blueprint.Invariable[text2] = TFUtils.LoadList<int>(data, text2);
		}
	}

	// Token: 0x06002040 RID: 8256 RVA: 0x000C94B4 File Offset: 0x000C76B4
	private static void MarshallHubInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("hub_info"))
		{
			Dictionary<string, object> dictionary = TFUtils.TryLoadDict(data, "hub_info");
			if (dictionary.ContainsKey("id"))
			{
				TFUtils.Assert(!dictionary.ContainsKey("did"), string.Format("Should specify {0} xor {1} in the {2} property!", "id", "did", "hub_info"));
				blueprint.Invariable["hub_id"] = new Identity(TFUtils.LoadString(dictionary, "id"));
			}
			else if (dictionary.ContainsKey("did"))
			{
				blueprint.Invariable["hub_did"] = TFUtils.LoadUint(dictionary, "did");
			}
		}
		else
		{
			blueprint.Invariable["hub_id"] = new Identity(TFUtils.LoadString(data, "hub"));
		}
	}

	// Token: 0x06002041 RID: 8257 RVA: 0x000C95A4 File Offset: 0x000C77A4
	private static void MarshallShuntedCraftingInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
		if (data.ContainsKey("shunts_crafting"))
		{
			blueprint.Invariable["shunts_crafting"] = TFUtils.LoadBool(data, "shunts_crafting");
		}
		else if (data.ContainsKey("shunt_crafting_to_did"))
		{
			blueprint.Invariable["shunts_crafting"] = true;
		}
		else
		{
			blueprint.Invariable["shunts_crafting"] = false;
		}
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x000C962C File Offset: 0x000C782C
	private static void InitializeBlueprintAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
		TFUtils.Assert(data.ContainsKey("thought_display"), "Missing thought display in " + (string)data["name"]);
		TFUtils.Assert(data.ContainsKey("thought_item_bubble_display"), "Missing thought item bubble in " + (string)data["name"]);
		TFUtils.Assert(data.ContainsKey("display"), "All blueprints must contain the display controller definition.");
		EntityType primaryType = blueprint.PrimaryType;
		Paperdoll.PaperdollType paperdollType = Paperdoll.PaperdollType.Other;
		if (primaryType == EntityType.RESIDENT || primaryType == EntityType.WANDERER || primaryType == EntityType.WORKER)
		{
			paperdollType = Paperdoll.PaperdollType.Character;
		}
		else if (primaryType == EntityType.BUILDING)
		{
			paperdollType = Paperdoll.PaperdollType.Building;
		}
		EntityManager.LoadDisplayController(data, "display", blueprint, mgr, paperdollType);
		EntityManager.LoadDisplayController(data, "thought_display", blueprint, mgr, Paperdoll.PaperdollType.Other);
		if (data.ContainsKey("thought_item_bubble_display"))
		{
			EntityManager.LoadDisplayController(data, "thought_item_bubble_display", blueprint, mgr, Paperdoll.PaperdollType.Other);
		}
		else
		{
			blueprint.Invariable["thought_item_bubble_display"] = null;
		}
		if (data.ContainsKey("thought_mask_display"))
		{
			EntityManager.LoadDisplayController(data, "thought_mask_display", blueprint, mgr, Paperdoll.PaperdollType.Other);
		}
		else
		{
			blueprint.Invariable["thought_mask_display"] = null;
		}
		if (data.ContainsKey("costumes"))
		{
			EntityManager.LoadCostumeFromBlueprint(data, "costumes", blueprint, mgr, Paperdoll.PaperdollType.Other);
		}
		else
		{
			blueprint.Invariable["costumes"] = null;
		}
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x000C9790 File Offset: 0x000C7990
	private static void InitializeUnitAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
		float num = 20f;
		if (data.ContainsKey("dropshadow_diameter"))
		{
			num = TFUtils.LoadFloat(data, "dropshadow_diameter");
		}
		blueprint.Invariable["dropshadow"] = EntityManager.CreateDropShadow(num, num);
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000C97D8 File Offset: 0x000C79D8
	private static void InitializeWorkerAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
		int num = TFUtils.LoadInt(data, "width");
		int num2 = TFUtils.LoadInt(data, "height");
		blueprint.Invariable["dropshadow"] = EntityManager.CreateDropShadow((float)num, (float)num2);
	}

	// Token: 0x06002045 RID: 8261 RVA: 0x000C9818 File Offset: 0x000C7A18
	public Entity Create(EntityType types, int did, bool incrementCount)
	{
		return this.Create(types, did, null, incrementCount);
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000C9824 File Offset: 0x000C7A24
	public Entity Create(EntityType types, int did, Identity id, bool incrementCount)
	{
		string blueprintName = EntityTypeNamingHelper.GetBlueprintName(types, did);
		return this.Create(blueprintName, id, incrementCount);
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000C9844 File Offset: 0x000C7A44
	public Entity Create(string blueprint, bool incrementCount)
	{
		return this.Create(blueprint, null, incrementCount);
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x000C9850 File Offset: 0x000C7A50
	public Entity Create(string blueprint, Identity id, bool incrementCount)
	{
		Entity entity;
		if (id != null && this.entities.ContainsKey(id))
		{
			entity = this.entities[id];
		}
		else
		{
			if (id != null)
			{
				entity = this.factory.Create(blueprint, id);
			}
			else
			{
				entity = this.factory.Create(blueprint);
			}
			this.entities.Add(entity.Id, entity);
		}
		if (incrementCount)
		{
			this.IncrementEntityCount(blueprint);
		}
		return entity;
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x000C98CC File Offset: 0x000C7ACC
	public static Blueprint GetBlueprint(string primaryType, int did, bool ignoreNotFoundError = false)
	{
		return EntityManager.GetBlueprint(EntityTypeNamingHelper.StringToType(primaryType, ignoreNotFoundError), did, ignoreNotFoundError);
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x000C98DC File Offset: 0x000C7ADC
	public static Blueprint GetBlueprint(EntityType type, int did, bool ignoreNotFoundError = false)
	{
		string blueprintName = EntityTypeNamingHelper.GetBlueprintName(type, did, ignoreNotFoundError);
		Blueprint result = null;
		EntityManager.blueprints.TryGetValue(blueprintName, out result);
		return result;
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000C9904 File Offset: 0x000C7B04
	public static List<string> GetAllBuildingBlueprintKeys()
	{
		List<string> list = new List<string>();
		foreach (string text in EntityManager.blueprints.Keys)
		{
			if (text.StartsWith("building"))
			{
				list.Add(text);
			}
		}
		return list;
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x000C9988 File Offset: 0x000C7B88
	public void Destroy(Identity id)
	{
		if (this.entities.ContainsKey(id))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = dictionary2 = this.entityCount;
			string blueprintName;
			string key = blueprintName = this.entities[id].BlueprintName;
			int num = dictionary2[blueprintName];
			dictionary[key] = num - 1;
			this.entities.Remove(id);
		}
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x000C99E0 File Offset: 0x000C7BE0
	public Entity GetEntity(Identity id)
	{
		return this.entities[id];
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x000C99F0 File Offset: 0x000C7BF0
	public int GetEntityCount(EntityType primaryType, int did)
	{
		string blueprintName = EntityTypeNamingHelper.GetBlueprintName(primaryType, did);
		if (!this.entityCount.ContainsKey(blueprintName))
		{
			return 0;
		}
		return this.entityCount[blueprintName];
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000C9A24 File Offset: 0x000C7C24
	public ICollection<Entity> GetEntities()
	{
		return this.entities.Values;
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06002050 RID: 8272 RVA: 0x000C9A34 File Offset: 0x000C7C34
	public DisplayControllerManager DisplayControllerManager
	{
		get
		{
			return this.displayControllerManager;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06002051 RID: 8273 RVA: 0x000C9A3C File Offset: 0x000C7C3C
	public Dictionary<string, Blueprint> Blueprints
	{
		get
		{
			return EntityManager.blueprints;
		}
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000C9A44 File Offset: 0x000C7C44
	private void LoadBlueprintsFromFile(string filePath)
	{
		string filePathFromString = this.GetFilePathFromString(filePath);
		TFUtils.DebugLog("Loading blueprints: " + filePathFromString);
		string json = TFUtils.ReadAllText(filePathFromString);
		List<object> list = (List<object>)Json.Deserialize(json);
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			this.OverwriteBlueprintDataWithSpread(dictionary);
			Blueprint blueprint = this.LoadBlueprintFromDict(dictionary);
			if (blueprint != null)
			{
				this.blueprintsToData[blueprint] = dictionary;
				int did = (int)blueprint.Invariable["did"];
				string blueprintName = EntityTypeNamingHelper.GetBlueprintName(blueprint.PrimaryType, did);
				EntityManager.blueprints[blueprintName] = blueprint;
				this.factory.Register((string)blueprint.Invariable["blueprint"], new EntityCtor(blueprint));
			}
		}
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000C9B58 File Offset: 0x000C7D58
	private Blueprint LoadBlueprintFromDict(Dictionary<string, object> dict)
	{
		string text = (string)dict["type"];
		EntityManager.BlueprintMarshaller blueprintMarshaller = null;
		if (!EntityManager.TypeRegistry.TryGetValue(text, out blueprintMarshaller))
		{
			throw new InvalidOperationException("No marshaller for type: " + text);
		}
		return blueprintMarshaller(dict, this);
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000C9BA4 File Offset: 0x000C7DA4
	private void LoadResources(Blueprint blueprint, EntityManager mgr)
	{
		if (blueprint.Disabled)
		{
			return;
		}
		TFUtils.DebugLog("Loading resources for " + blueprint.ToString(), TFUtils.LogFilter.Assets);
		Dictionary<string, object> dictionary = this.blueprintsToData[blueprint];
		string key = (string)dictionary["type"];
		EntityManager.InitializeBlueprintAssets(blueprint, dictionary, mgr);
		if (EntityManager.AssetsInitializerTypeRegistry.ContainsKey(key))
		{
			EntityManager.AssetsInitializerTypeRegistry[key](blueprint, dictionary, this);
		}
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x000C9C1C File Offset: 0x000C7E1C
	public void LoadBlueprints()
	{
		EntityManager._pBpSpreadData = new Dictionary<string, object>();
		EntityManager.LoadUnitsFromSpread();
		this.LoadAnnexesFromSpread();
		this.LoadCharacterBuildingsFromSpread();
		this.LoadDebrisFromSpread();
		this.LoadDecorationsFromSpread();
		this.LoadLandmarksFromSpread();
		this.LoadRentOnlyBuildingsFromSpread();
		this.LoadShopsFromSpread();
		this.LoadTreasureFromSpread();
		this.LoadTreesFromSpread();
		this.blueprintFilePaths = this.GetFilesToLoad();
		this.blueprintFileEnumerator = this.blueprintFilePaths.GetEnumerator();
		TFUtils.Assert(this.blueprintFileEnumerator != null, "Should have a full list of blueprints");
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x000C9CA4 File Offset: 0x000C7EA4
	public bool IterateLoadOfBlueprints()
	{
		if (this.blueprintFileEnumerator.MoveNext())
		{
			string filePath = (string)this.blueprintFileEnumerator.Current;
			this.LoadBlueprintsFromFile(filePath);
			return true;
		}
		return false;
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x000C9CDC File Offset: 0x000C7EDC
	public void LoadBlueprintResources()
	{
		foreach (object obj in EntityManager.blueprints.Values)
		{
			Blueprint blueprint = (Blueprint)obj;
			this.LoadResources(blueprint, this);
		}
		this.blueprintsToData.Clear();
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x000C9D30 File Offset: 0x000C7F30
	private string[] GetFilesToLoad()
	{
		return Config.BLUEPRINT_DIRECTORY_PATH;
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x000C9D38 File Offset: 0x000C7F38
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x000C9D3C File Offset: 0x000C7F3C
	private void IncrementEntityCount(string blueprint)
	{
		if (this.entityCount.ContainsKey(blueprint))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = dictionary2 = this.entityCount;
			int num = dictionary2[blueprint];
			dictionary[blueprint] = num + 1;
		}
		else
		{
			this.entityCount[blueprint] = 1;
		}
	}

	// Token: 0x040013C1 RID: 5057
	public const string FOOTPRINT_MATERIAL = "Materials/unique/footprint";

	// Token: 0x040013C2 RID: 5058
	private const string DROPSHADOW_TEXTURE = "dropshadow.tga";

	// Token: 0x040013C3 RID: 5059
	private static readonly string BLUEPRINT_DIRECTORY_PATH = "Blueprints";

	// Token: 0x040013C4 RID: 5060
	public static bool MustRegenerateStates = false;

	// Token: 0x040013C5 RID: 5061
	private static Dictionary<string, EntityManager.BlueprintMarshaller> TypeRegistry = null;

	// Token: 0x040013C6 RID: 5062
	private static Dictionary<string, EntityManager.BlueprintAssetsInitializer> AssetsInitializerTypeRegistry = new Dictionary<string, EntityManager.BlueprintAssetsInitializer>();

	// Token: 0x040013C7 RID: 5063
	public static Dictionary<string, Simulated.StateAction> BuildingActions;

	// Token: 0x040013C8 RID: 5064
	private static StateMachine<Simulated.StateAction, Command.TYPE> BuildingMachine;

	// Token: 0x040013C9 RID: 5065
	public static Dictionary<string, Simulated.StateAction> AnnexActions;

	// Token: 0x040013CA RID: 5066
	private static StateMachine<Simulated.StateAction, Command.TYPE> AnnexMachine;

	// Token: 0x040013CB RID: 5067
	public static Dictionary<string, Simulated.StateAction> DebrisActions;

	// Token: 0x040013CC RID: 5068
	private static StateMachine<Simulated.StateAction, Command.TYPE> DebrisMachine;

	// Token: 0x040013CD RID: 5069
	public static Dictionary<string, Simulated.StateAction> LandmarkActions;

	// Token: 0x040013CE RID: 5070
	private static StateMachine<Simulated.StateAction, Command.TYPE> LandmarkMachine;

	// Token: 0x040013CF RID: 5071
	public static Dictionary<string, Simulated.StateAction> ResidentActions;

	// Token: 0x040013D0 RID: 5072
	private static StateMachine<Simulated.StateAction, Command.TYPE> UnitMachine;

	// Token: 0x040013D1 RID: 5073
	public static Dictionary<string, Simulated.StateAction> TreasureActions;

	// Token: 0x040013D2 RID: 5074
	private static StateMachine<Simulated.StateAction, Command.TYPE> TreasureMachine;

	// Token: 0x040013D3 RID: 5075
	public static Dictionary<string, Simulated.StateAction> WorkerActions;

	// Token: 0x040013D4 RID: 5076
	private static StateMachine<Simulated.StateAction, Command.TYPE> WorkerMachine;

	// Token: 0x040013D5 RID: 5077
	public static Dictionary<string, Simulated.StateAction> WandererActions;

	// Token: 0x040013D6 RID: 5078
	private static StateMachine<Simulated.StateAction, Command.TYPE> WandererMachine;

	// Token: 0x040013D7 RID: 5079
	private static Dictionary<string, Blueprint> blueprints = new Dictionary<string, Blueprint>();

	// Token: 0x040013D8 RID: 5080
	private string[] blueprintFilePaths;

	// Token: 0x040013D9 RID: 5081
	private IEnumerator blueprintFileEnumerator;

	// Token: 0x040013DA RID: 5082
	private static Dictionary<string, object> _pBpSpreadData;

	// Token: 0x040013DB RID: 5083
	private Dictionary<Blueprint, Dictionary<string, object>> blueprintsToData = new Dictionary<Blueprint, Dictionary<string, object>>();

	// Token: 0x040013DC RID: 5084
	private Factory<string, Entity> factory;

	// Token: 0x040013DD RID: 5085
	private Dictionary<Identity, Entity> entities;

	// Token: 0x040013DE RID: 5086
	private Dictionary<string, int> entityCount;

	// Token: 0x040013DF RID: 5087
	private DisplayControllerManager displayControllerManager;

	// Token: 0x020004BF RID: 1215
	// (Invoke) Token: 0x0600255F RID: 9567
	private delegate Blueprint BlueprintMarshaller(Dictionary<string, object> data, EntityManager mgr);

	// Token: 0x020004C0 RID: 1216
	// (Invoke) Token: 0x06002563 RID: 9571
	private delegate void BlueprintAssetsInitializer(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr);
}
