using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000226 RID: 550
public class FootprintGuide : SimulationSessionActionDefinition
{
	// Token: 0x06001207 RID: 4615 RVA: 0x0007DDA8 File Offset: 0x0007BFA8
	public static FootprintGuide Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		FootprintGuide footprintGuide = new FootprintGuide();
		footprintGuide.Parse(data, id, startConditions, originatedFromQuest);
		return footprintGuide;
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x0007DDC8 File Offset: 0x0007BFC8
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		TFUtils.LoadVector3(out this.position, (Dictionary<string, object>)data["position"]);
		this.width = TFUtils.LoadFloat(data, "width");
		this.height = TFUtils.LoadFloat(data, "height");
		bool? flag = TFUtils.TryLoadBool(data, "lock_placement");
		if (flag != null)
		{
			this.lockPlacement = flag.Value;
		}
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0007DE48 File Offset: 0x0007C048
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["position"] = this.position;
		dictionary["width"] = this.width;
		dictionary["height"] = this.height;
		dictionary["lock_placement"] = this.lockPlacement;
		return dictionary;
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x0007DEB8 File Offset: 0x0007C0B8
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			base.ToString(),
			"FootprintGuide:(position=",
			this.position,
			"width=",
			this.width,
			"height=",
			this.height,
			"lock_placement=",
			this.lockPlacement,
			")"
		});
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x0007DF3C File Offset: 0x0007C13C
	public void SpawnFootprint(Game game, SessionActionTracker tracker)
	{
		this.spawnTemplate.Spawn(game, tracker, this.position, this.width, this.height);
		TFUtils.Assert(game.terrain.FootprintGuide == null, "Trying to add a footprint guide when one already exists!");
		if (this.lockPlacement)
		{
			AlignedBox footprintGuide = new AlignedBox(this.position.x, this.position.x + this.width, this.position.y, this.position.y + this.height);
			game.terrain.FootprintGuide = footprintGuide;
		}
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x0007DFD8 File Offset: 0x0007C1D8
	public override void OnDestroy(Game game)
	{
		base.OnDestroy(game);
		if (this.lockPlacement)
		{
			game.terrain.FootprintGuide = null;
		}
	}

	// Token: 0x04000C4F RID: 3151
	public const string TYPE = "footprint_guide";

	// Token: 0x04000C50 RID: 3152
	private const string POSITION = "position";

	// Token: 0x04000C51 RID: 3153
	private const string WIDTH = "width";

	// Token: 0x04000C52 RID: 3154
	private const string HEIGHT = "height";

	// Token: 0x04000C53 RID: 3155
	private const string LOCK_PLACEMENT = "lock_placement";

	// Token: 0x04000C54 RID: 3156
	private Vector3 position;

	// Token: 0x04000C55 RID: 3157
	private float width;

	// Token: 0x04000C56 RID: 3158
	private float height;

	// Token: 0x04000C57 RID: 3159
	private bool lockPlacement;

	// Token: 0x04000C58 RID: 3160
	private FootprintGuideSpawn spawnTemplate = new FootprintGuideSpawn();
}
