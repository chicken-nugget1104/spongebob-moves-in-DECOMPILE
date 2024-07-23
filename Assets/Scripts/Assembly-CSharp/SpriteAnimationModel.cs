using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class SpriteAnimationModel : ULSpriteAnimModel
{
	// Token: 0x0600222D RID: 8749 RVA: 0x000D2B38 File Offset: 0x000D0D38
	public void AddAnimationDataWithBlueprint(Dictionary<string, object> data)
	{
		TFSpriteAnimationSetting tfspriteAnimationSetting = new TFSpriteAnimationSetting();
		tfspriteAnimationSetting.animName = null;
		tfspriteAnimationSetting.resourceName = null;
		tfspriteAnimationSetting.cellTop = 0f;
		tfspriteAnimationSetting.cellLeft = 0f;
		tfspriteAnimationSetting.cellWidth = 1f;
		tfspriteAnimationSetting.cellHeight = 1f;
		tfspriteAnimationSetting.cellStartColumn = 0;
		tfspriteAnimationSetting.cellColumns = 1;
		tfspriteAnimationSetting.cellCount = 1;
		tfspriteAnimationSetting.framesPerSecond = 1;
		tfspriteAnimationSetting.timingTotal = 0f;
		tfspriteAnimationSetting.timingList = null;
		tfspriteAnimationSetting.loopMode = ULSpriteAnimationSetting.LoopMode.None;
		tfspriteAnimationSetting.flipH = false;
		tfspriteAnimationSetting.flipV = false;
		tfspriteAnimationSetting.hasQuad = false;
		tfspriteAnimationSetting.width = 1;
		tfspriteAnimationSetting.height = 1;
		tfspriteAnimationSetting.scale = Vector3.one;
		string text = (string)data["name"];
		tfspriteAnimationSetting.animName = text;
		if (data.ContainsKey("material"))
		{
			tfspriteAnimationSetting.resourceName = (string)data["material"];
		}
		if (data.ContainsKey("top"))
		{
			tfspriteAnimationSetting.cellTop = TFUtils.LoadFloat(data, "top");
		}
		if (data.ContainsKey("left"))
		{
			tfspriteAnimationSetting.cellLeft = TFUtils.LoadFloat(data, "left");
		}
		if (data.ContainsKey("width"))
		{
			tfspriteAnimationSetting.cellWidth = TFUtils.LoadFloat(data, "width");
		}
		if (data.ContainsKey("height"))
		{
			tfspriteAnimationSetting.cellHeight = TFUtils.LoadFloat(data, "height");
		}
		if (data.ContainsKey("start"))
		{
			tfspriteAnimationSetting.cellStartColumn = TFUtils.LoadInt(data, "start");
		}
		if (data.ContainsKey("columns"))
		{
			tfspriteAnimationSetting.cellColumns = TFUtils.LoadInt(data, "columns");
		}
		if (data.ContainsKey("count"))
		{
			tfspriteAnimationSetting.cellCount = TFUtils.LoadInt(data, "count");
		}
		if (data.ContainsKey("fps"))
		{
			tfspriteAnimationSetting.framesPerSecond = TFUtils.LoadInt(data, "fps");
		}
		if (data.ContainsKey("timing"))
		{
			List<object> list = TFUtils.LoadList<object>(data, "timing");
			tfspriteAnimationSetting.timingTotal = 0f;
			tfspriteAnimationSetting.timingList = new List<float>();
			foreach (object obj in list)
			{
				float num = 0f;
				IConvertible convertible = obj as IConvertible;
				if (convertible != null)
				{
					num = (float)convertible.ToDouble(null);
				}
				tfspriteAnimationSetting.timingTotal += num;
				tfspriteAnimationSetting.timingList.Add(tfspriteAnimationSetting.timingTotal);
			}
		}
		if (data.ContainsKey("loop"))
		{
			tfspriteAnimationSetting.loopMode = ((!(bool)data["loop"]) ? ULSpriteAnimationSetting.LoopMode.None : ULSpriteAnimationSetting.LoopMode.Loop);
		}
		if (data.ContainsKey("fliph"))
		{
			tfspriteAnimationSetting.flipH = (bool)data["fliph"];
		}
		if (data.ContainsKey("flipv"))
		{
			tfspriteAnimationSetting.flipV = (bool)data["flipv"];
		}
		if (data.ContainsKey("quad"))
		{
			tfspriteAnimationSetting.hasQuad = true;
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["quad"];
			if (dictionary.ContainsKey("width"))
			{
				tfspriteAnimationSetting.width = TFUtils.LoadInt(dictionary, "width");
			}
			if (dictionary.ContainsKey("height"))
			{
				tfspriteAnimationSetting.height = TFUtils.LoadInt(dictionary, "height");
			}
		}
		if (data.ContainsKey("scale"))
		{
			TFUtils.LoadVector3(out tfspriteAnimationSetting.scale, (Dictionary<string, object>)data["scale"], 1f);
		}
		if (data.ContainsKey("texture") && data["texture"] != null)
		{
			if (data.ContainsKey("red_value"))
			{
				byte r = Convert.ToByte(data["red_value"]);
				byte g = Convert.ToByte(data["green_value"]);
				byte b = Convert.ToByte(data["blue_value"]);
				tfspriteAnimationSetting.mainColor = new Color32(r, g, b, byte.MaxValue);
				tfspriteAnimationSetting.maskName = Convert.ToString(data["mask_name"]);
			}
			tfspriteAnimationSetting.texture = (string)data["texture"];
			TFUtils.Assert(YGTextureLibrary.HasAtlasCoords(tfspriteAnimationSetting.texture), "Atlas does not contain " + tfspriteAnimationSetting.texture);
			tfspriteAnimationSetting.resourceName = YGTextureLibrary.GetAtlasCoords(tfspriteAnimationSetting.texture).atlas.name;
		}
		else
		{
			tfspriteAnimationSetting.texture = null;
		}
		base.AddAnimationSetting(text, tfspriteAnimationSetting);
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x000D3004 File Offset: 0x000D1204
	public bool HasQuadData(string animName)
	{
		return ((TFSpriteAnimationSetting)this.animationHashtable[animName]).hasQuad;
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x000D301C File Offset: 0x000D121C
	public int Width(string animName)
	{
		return ((TFSpriteAnimationSetting)this.animationHashtable[animName]).width;
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000D3034 File Offset: 0x000D1234
	public int Height(string animName)
	{
		return ((TFSpriteAnimationSetting)this.animationHashtable[animName]).height;
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000D304C File Offset: 0x000D124C
	public Vector3 Scale(string animName)
	{
		return ((TFSpriteAnimationSetting)this.animationHashtable[animName]).scale;
	}
}
