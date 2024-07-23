using System;

// Token: 0x02000417 RID: 1047
public class EntityTypeNamingHelper
{
	// Token: 0x0600205C RID: 8284 RVA: 0x000C9D90 File Offset: 0x000C7F90
	public static string GetBlueprintName(EntityType primaryType, int did)
	{
		return EntityTypeNamingHelper.GetBlueprintName(primaryType, did, false);
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000C9D9C File Offset: 0x000C7F9C
	public static string GetBlueprintName(EntityType primaryType, int did, bool ignoreNotFoundError)
	{
		return EntityTypeNamingHelper.GetBlueprintName(EntityTypeNamingHelper.TypeToString(primaryType, ignoreNotFoundError), did);
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000C9DAC File Offset: 0x000C7FAC
	public static string GetBlueprintName(string primaryType, int did)
	{
		return string.Format("{0}_{1}", primaryType, did);
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000C9DC0 File Offset: 0x000C7FC0
	public static EntityType StringToType(string type)
	{
		return EntityTypeNamingHelper.StringToType(type, false);
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000C9DCC File Offset: 0x000C7FCC
	public static EntityType StringToType(string type, bool ignoreNotFoundError)
	{
		if (type == "unit")
		{
			return EntityType.RESIDENT;
		}
		if (type == "worker")
		{
			return EntityType.WORKER;
		}
		if (type == "wanderer")
		{
			return EntityType.WANDERER;
		}
		if (type == "debris")
		{
			return EntityType.DEBRIS;
		}
		if (type == "landmark")
		{
			return EntityType.LANDMARK;
		}
		if (type == "building")
		{
			return EntityType.BUILDING;
		}
		if (type == "annex")
		{
			return EntityType.ANNEX;
		}
		if (type == "treasure")
		{
			return EntityType.TREASURE;
		}
		if (type == "costume")
		{
			return EntityType.COSTUME;
		}
		if (!ignoreNotFoundError)
		{
			TFUtils.ErrorLog("Encountered unknown type (" + type + ")");
		}
		return EntityType.INVALID;
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000C9EA8 File Offset: 0x000C80A8
	public static string TypeToString(EntityType type)
	{
		return EntityTypeNamingHelper.TypeToString(type, false);
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000C9EB4 File Offset: 0x000C80B4
	public static string TypeToString(EntityType type, bool ignoreNotFoundError)
	{
		if ((type & EntityType.RESIDENT) != EntityType.INVALID)
		{
			return "unit";
		}
		if ((type & EntityType.WORKER) != EntityType.INVALID)
		{
			return "worker";
		}
		if ((type & EntityType.WANDERER) != EntityType.INVALID)
		{
			return "wanderer";
		}
		if ((type & EntityType.DEBRIS) != EntityType.INVALID)
		{
			return "debris";
		}
		if ((type & EntityType.LANDMARK) != EntityType.INVALID)
		{
			return "landmark";
		}
		if ((type & EntityType.ANNEX) != EntityType.INVALID)
		{
			return "annex";
		}
		if ((type & EntityType.BUILDING) != EntityType.INVALID)
		{
			return "building";
		}
		if ((type & EntityType.TREASURE) != EntityType.INVALID)
		{
			return "treasure";
		}
		if ((type & EntityType.COSTUME) != EntityType.INVALID)
		{
			return "costume";
		}
		if (!ignoreNotFoundError)
		{
			TFUtils.ErrorLog("Encountered unknown type (" + type.ToString() + ")");
		}
		return null;
	}
}
