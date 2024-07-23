using System;
using System.Collections.Generic;

// Token: 0x020001B9 RID: 441
public class ResourceCategory
{
	// Token: 0x06000EFA RID: 3834 RVA: 0x0005E380 File Offset: 0x0005C580
	public static ResourceCategory FromDict(Dictionary<string, object> data)
	{
		ResourceCategory resourceCategory = new ResourceCategory();
		TFUtils.Assert(data.Count == 1, "Invalid category_to_productgroups array. Each category should only contain a single list of product groups.");
		foreach (string text in data.Keys)
		{
			resourceCategory.name = text;
			List<string> list = TFUtils.LoadList<string>(data, resourceCategory.name);
			foreach (string text2 in list)
			{
				ResourceProductGroup resourceProductGroup = new ResourceProductGroup();
				resourceProductGroup.name = text2;
				resourceCategory.productGroups.Add(resourceProductGroup);
			}
		}
		return resourceCategory;
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0005E478 File Offset: 0x0005C678
	public ResourceProductGroup GetProductGroupByName(string name)
	{
		foreach (ResourceProductGroup resourceProductGroup in this.productGroups)
		{
			if (resourceProductGroup.name.Equals(name))
			{
				return resourceProductGroup;
			}
		}
		return null;
	}

	// Token: 0x040009FE RID: 2558
	public string name;

	// Token: 0x040009FF RID: 2559
	public List<ResourceProductGroup> productGroups = new List<ResourceProductGroup>();
}
