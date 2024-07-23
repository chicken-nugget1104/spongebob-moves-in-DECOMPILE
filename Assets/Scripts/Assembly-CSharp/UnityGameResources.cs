using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class UnityGameResources
{
	// Token: 0x060023CA RID: 9162 RVA: 0x000DCD1C File Offset: 0x000DAF1C
	public static GameObject CreateEmpty(string objectName)
	{
		GameObject gameObject = new GameObject(objectName);
		UnityGameResources.gameObjects.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060023CB RID: 9163 RVA: 0x000DCD3C File Offset: 0x000DAF3C
	public static GameObject Create(string localPath)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(localPath)) as GameObject;
		UnityGameResources.gameObjects.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060023CC RID: 9164 RVA: 0x000DCD68 File Offset: 0x000DAF68
	public static GameObject SafeCreate(string localPath)
	{
		UnityEngine.Object @object = Resources.Load(localPath);
		if (@object == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(@object) as GameObject;
		UnityGameResources.gameObjects.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x000DCDA4 File Offset: 0x000DAFA4
	public static void AddGameObject(GameObject obj)
	{
		UnityGameResources.gameObjects.Add(obj);
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x000DCDB4 File Offset: 0x000DAFB4
	public static void Destroy(GameObject gameObject)
	{
		UnityGameResources.gameObjects.Remove(gameObject);
		UnityEngine.Object.Destroy(gameObject);
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000DCDC8 File Offset: 0x000DAFC8
	public static void Reset()
	{
		foreach (GameObject obj in UnityGameResources.gameObjects)
		{
			UnityEngine.Object.Destroy(obj);
		}
		UnityGameResources.gameObjects.Clear();
	}

	// Token: 0x04001619 RID: 5657
	private static List<GameObject> gameObjects = new List<GameObject>();
}
