using System;
using System.Collections.Generic;

// Token: 0x02000156 RID: 342
public class CraftingCookbook
{
	// Token: 0x06000BB4 RID: 2996 RVA: 0x00046370 File Offset: 0x00044570
	public CraftingCookbook(Dictionary<string, object> data)
	{
		this.identity = TFUtils.LoadInt(data, "id");
		this.sessionActionId = TFUtils.LoadString(data, "session_action_id");
		this.cancelButtonTexture = TFUtils.LoadString(data, "texture.cancelbutton");
		this.recipeSlotTexture = TFUtils.LoadString(data, "texture.slot");
		this.titleTexture = TFUtils.LoadString(data, "texture.title");
		this.titleIconTexture = TFUtils.LoadString(data, "texture.titleicon");
		this.backgroundColor = ((List<object>)data["background.color"]).ConvertAll<int>((object x) => Convert.ToInt32(x));
		this.buttonIcon = TFUtils.LoadNullableString(data, "button.icon");
		this.buttonLabel = Language.Get(TFUtils.LoadString(data, "button.label"));
		this.openSound = TFUtils.LoadString(data, "open_sound");
		this.closeSound = TFUtils.LoadString(data, "close_sound");
		this.music = TFUtils.LoadNullableString(data, "music");
		this.recipes = ((List<object>)data["recipes"]).ConvertAll<int>((object x) => Convert.ToInt32(x));
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x000464B4 File Offset: 0x000446B4
	public int[] GetRecipes()
	{
		return this.recipes.ToArray();
	}

	// Token: 0x040007BA RID: 1978
	public const string TYPE = "cookbook";

	// Token: 0x040007BB RID: 1979
	public const int DEFAULT_ID = 1;

	// Token: 0x040007BC RID: 1980
	protected List<int> recipes;

	// Token: 0x040007BD RID: 1981
	public int identity;

	// Token: 0x040007BE RID: 1982
	public string sessionActionId;

	// Token: 0x040007BF RID: 1983
	public string cancelButtonTexture;

	// Token: 0x040007C0 RID: 1984
	public string recipeSlotTexture;

	// Token: 0x040007C1 RID: 1985
	public string titleTexture;

	// Token: 0x040007C2 RID: 1986
	public string titleIconTexture;

	// Token: 0x040007C3 RID: 1987
	public List<int> backgroundColor;

	// Token: 0x040007C4 RID: 1988
	public string buttonIcon;

	// Token: 0x040007C5 RID: 1989
	public string buttonLabel;

	// Token: 0x040007C6 RID: 1990
	public string openSound;

	// Token: 0x040007C7 RID: 1991
	public string closeSound;

	// Token: 0x040007C8 RID: 1992
	public string music;
}
