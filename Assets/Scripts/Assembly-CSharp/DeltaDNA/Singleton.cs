using System;
using UnityEngine;

namespace DeltaDNA
{
	// Token: 0x0200000C RID: 12
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000044BC File Offset: 0x000026BC
		public static T Instance
		{
			get
			{
				if (Singleton<T>.applicationIsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
					return (T)((object)null);
				}
				object @lock = Singleton<T>._lock;
				T instance;
				lock (@lock)
				{
					if (Singleton<T>._instance == null)
					{
						Singleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
						if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopening the scene might fix it.");
							return Singleton<T>._instance;
						}
						if (Singleton<T>._instance == null)
						{
							GameObject gameObject = new GameObject();
							Singleton<T>._instance = gameObject.AddComponent<T>();
							gameObject.name = "(singleton) " + typeof(T).ToString();
							UnityEngine.Object.DontDestroyOnLoad(gameObject);
							Debug.Log(string.Concat(new object[]
							{
								"[Singleton] An instance of ",
								typeof(T),
								" is needed in the scene, so '",
								gameObject,
								"' was created with DontDestroyOnLoad."
							}));
						}
						else
						{
							Debug.Log("[Singleton] Using instance already created: " + Singleton<T>._instance.gameObject.name);
						}
					}
					instance = Singleton<T>._instance;
				}
				return instance;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004640 File Offset: 0x00002840
		public virtual void OnDestroy()
		{
			Debug.Log("[Singleton] Destroying an instance of " + typeof(T));
			Singleton<T>.applicationIsQuitting = true;
		}

		// Token: 0x04000048 RID: 72
		private static T _instance;

		// Token: 0x04000049 RID: 73
		private static object _lock = new object();

		// Token: 0x0400004A RID: 74
		private static bool applicationIsQuitting = false;
	}
}
