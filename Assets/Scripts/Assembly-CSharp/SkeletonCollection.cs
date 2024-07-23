using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class SkeletonCollection
{
	// Token: 0x060021F4 RID: 8692 RVA: 0x000D14C8 File Offset: 0x000CF6C8
	public SkeletonCollection()
	{
		this.skeletons = new Dictionary<string, GameObject>();
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000D14DC File Offset: 0x000CF6DC
	public GameObject GetSkeleton(string key, bool createIfNotFound, out bool createdResource)
	{
		createdResource = false;
		GameObject gameObject = null;
		if (!this.skeletons.TryGetValue(key, out gameObject))
		{
			if (createIfNotFound)
			{
				UnityEngine.Object @object = FileSystemCoordinator.LoadAsset(Path.GetFileName(key));
				if (@object != null)
				{
					gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
				}
				if (gameObject == null)
				{
					gameObject = UnityGameResources.Create(key);
				}
				this.skeletons.Add(key, gameObject);
				createdResource = true;
			}
		}
		TFUtils.Assert(gameObject != null, "SkeletonCollection.GetSkeleton should not be null after creation");
		return gameObject;
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000D1568 File Offset: 0x000CF768
	public void Cleanse(AnimationGroupManager.AnimGroup animGroup)
	{
		GameObject gameObject = null;
		if (this.skeletons.TryGetValue(animGroup.skeletonName, out gameObject))
		{
			animGroup.animModel.UnapplyAnimationSettings(gameObject.animation);
			UnityEngine.Object.Destroy(gameObject);
			this.skeletons.Remove(animGroup.skeletonName);
		}
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000D15B8 File Offset: 0x000CF7B8
	public void Cleanse(string key)
	{
		if (this.skeletons.ContainsKey(key))
		{
			UnityEngine.Object.Destroy(this.skeletons[key]);
			this.skeletons.Remove(key);
		}
	}

	// Token: 0x04001506 RID: 5382
	private Dictionary<string, GameObject> skeletons;
}
