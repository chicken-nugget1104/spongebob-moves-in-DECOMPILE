using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class SBGUI : MonoBehaviour
{
	// Token: 0x06000377 RID: 887 RVA: 0x000114C4 File Offset: 0x0000F6C4
	public static SBGUI GetInstance()
	{
		if (SBGUI.instance == null && Application.isPlaying)
		{
			GameObject gameObject = new GameObject("AutomaticallyCreatedSBGUI");
			SBGUI.SetInstance(gameObject.AddComponent<SBGUI>());
		}
		return SBGUI.instance;
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00011508 File Offset: 0x0000F708
	public static SBGUI GetCurrentInstance()
	{
		return SBGUI.instance;
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00011510 File Offset: 0x0000F710
	private static bool SetInstance(SBGUI inst)
	{
		if (SBGUI.instance != null && SBGUI.instance != inst)
		{
			return false;
		}
		SBGUI.instance = inst;
		return true;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0001153C File Offset: 0x0000F73C
	public static Vector2 Touch2Screen(Vector2 p)
	{
		Vector2 result = p;
		result.y = SBGUI.GetScreenHeight() - result.y;
		return result;
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00011560 File Offset: 0x0000F760
	public static Rect Touch2Screen(Rect r)
	{
		Rect result = r;
		result.y = SBGUI.GetScreenHeight() - result.y - result.height;
		return result;
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0001158C File Offset: 0x0000F78C
	protected virtual void OnEnable()
	{
		if (!SBGUI.SetInstance(this))
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
	}

	// Token: 0x0600037D RID: 893 RVA: 0x000115A0 File Offset: 0x0000F7A0
	public SBGUIScreen LoadAndPushScreen(string prefabName)
	{
		SBGUIElement sbguielement = SBGUI.InstantiatePrefab(prefabName);
		if (sbguielement != null)
		{
			SBGUIScreen sbguiscreen = sbguielement as SBGUIScreen;
			if (sbguiscreen != null)
			{
				this.PushGUIScreen(sbguiscreen);
				return sbguiscreen;
			}
		}
		return null;
	}

	// Token: 0x0600037E RID: 894 RVA: 0x000115E0 File Offset: 0x0000F7E0
	private string DebugPrintGuiStack()
	{
		if (this.GUIScreenStack.Count == 0)
		{
			return "[empty_stack]";
		}
		string text = string.Empty;
		for (int i = this.GUIScreenStack.Count - 1; i >= 0; i--)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"\n[",
				i,
				"] ",
				this.GUIScreenStack[i]
			});
		}
		return text;
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600037F RID: 895 RVA: 0x00011664 File Offset: 0x0000F864
	public int GUIScreenCount
	{
		get
		{
			return this.GUIScreenStack.Count;
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00011674 File Offset: 0x0000F874
	public void PushGUIScreen(SBGUIScreen screen)
	{
		this.InsertGUIScreen(screen, 0);
		if (this.whitelistedUI.Count > 0)
		{
			screen.MuteButtons(true);
		}
		if (this.whitelistedUI.Count == 0)
		{
			screen.MuteButtons(false);
		}
		foreach (SBGUIElement sbguielement in this.whitelistedUI.Keys)
		{
			sbguielement.MuteButtons(false);
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00011718 File Offset: 0x0000F918
	public void InsertGUIScreen(SBGUIScreen screen, int depth)
	{
		this.GUIScreenStack.Insert(this.GUIScreenStack.Count - depth, screen);
		if (depth == 0)
		{
			this.GUIScreen = screen;
			if (this.GUIScreen)
			{
				this.GUIScreen.OnScreenStart(screen);
			}
		}
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00011768 File Offset: 0x0000F968
	public SBGUIScreen PeekGUIScreen()
	{
		return this.GUIScreen;
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00011770 File Offset: 0x0000F970
	public SBGUIScreen PopGUIScreen()
	{
		SBGUIScreen result = null;
		if (this.GUIScreen)
		{
			this.GUIScreen.OnScreenEnd(this.GUIScreen);
		}
		if (this.GUIScreenStack.Count > 0)
		{
			this.GUIScreen = this.GUIScreenStack[this.GUIScreenStack.Count - 1];
			this.GUIScreenStack.RemoveAt(this.GUIScreenStack.Count - 1);
			result = this.GUIScreen;
		}
		return result;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x000117F0 File Offset: 0x0000F9F0
	public SBGUIScreen RemoveGUIScreen(int depth)
	{
		SBGUIScreen result = this.GUIScreenStack[this.GUIScreenStack.Count - 1 - depth];
		this.RemoveGUIScreens(depth, 1);
		return result;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00011824 File Offset: 0x0000FA24
	public void RemoveGUIScreens(int depth, int layers)
	{
		List<SBGUIScreen> list = new List<SBGUIScreen>();
		int num = this.GUIScreenStack.Count - (depth + layers);
		for (int i = num; i < num + layers; i++)
		{
			list.Add(this.GUIScreenStack[i]);
		}
		this.GUIScreenStack.RemoveRange(num, layers);
		foreach (SBGUIScreen sbguiscreen in list)
		{
			if (!this.GUIScreenStack.Contains(sbguiscreen))
			{
				sbguiscreen.OnScreenEnd(sbguiscreen);
			}
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000118E4 File Offset: 0x0000FAE4
	public bool ContainsGUIScreen(SBGUIScreen screen)
	{
		return this.GUIScreenStack.Contains(screen);
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000118F4 File Offset: 0x0000FAF4
	public bool ContainsGUIScreen<T>()
	{
		int count = this.GUIScreenStack.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.GUIScreenStack[i] is T)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00011938 File Offset: 0x0000FB38
	public static SBGUIElement InstantiatePrefab(string prefabName)
	{
		GameObject gameObject = (GameObject)Resources.Load(prefabName);
		if (gameObject != null)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
			if (gameObject2 != null)
			{
				SBGUIElement sbguielement = (SBGUIElement)gameObject2.GetComponent(typeof(SBGUIElement));
				if (sbguielement != null)
				{
					return sbguielement;
				}
			}
		}
		TFUtils.Assert(false, string.Format("Tried to instantiate a prefab but failed! name={0}", prefabName));
		return null;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x000119AC File Offset: 0x0000FBAC
	public void WhitelistElement(SBGUIElement element)
	{
		this.MuteScreens(true);
		if (this.whitelistedUI.ContainsKey(element))
		{
			Dictionary<SBGUIElement, int> dictionary2;
			Dictionary<SBGUIElement, int> dictionary = dictionary2 = this.whitelistedUI;
			int num = dictionary2[element];
			dictionary[element] = num + 1;
		}
		else
		{
			this.whitelistedUI[element] = 1;
		}
		foreach (SBGUIElement sbguielement in this.whitelistedUI.Keys)
		{
			sbguielement.MuteButtons(false);
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00011A60 File Offset: 0x0000FC60
	public void UnWhitelistElement(SBGUIElement element)
	{
		if (this.whitelistedUI.ContainsKey(element))
		{
			Dictionary<SBGUIElement, int> dictionary2;
			Dictionary<SBGUIElement, int> dictionary = dictionary2 = this.whitelistedUI;
			int num = dictionary2[element];
			dictionary[element] = num - 1;
			if (this.whitelistedUI[element] <= 0)
			{
				this.whitelistedUI.Remove(element);
				element.MuteButtons(true);
			}
			if (this.whitelistedUI.Count == 0)
			{
				this.MuteScreens(false);
			}
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00011AD8 File Offset: 0x0000FCD8
	public void RestoreWhiteList()
	{
		foreach (SBGUIElement sbguielement in this.backUpWhitelistedUI.Keys)
		{
			if (sbguielement != null)
			{
				if (!this.whitelistedUI.ContainsKey(sbguielement))
				{
					this.whitelistedUI.Add(sbguielement, this.backUpWhitelistedUI[sbguielement]);
				}
				sbguielement.MuteButtons(false);
			}
		}
		this.backUpWhitelistedUI.Clear();
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00011B84 File Offset: 0x0000FD84
	public void ResetWhiteList()
	{
		foreach (SBGUIElement sbguielement in this.whitelistedUI.Keys)
		{
			if (sbguielement != null)
			{
				if (!this.backUpWhitelistedUI.ContainsKey(sbguielement))
				{
					this.backUpWhitelistedUI.Add(sbguielement, this.whitelistedUI[sbguielement]);
				}
				sbguielement.MuteButtons(false);
			}
		}
		this.whitelistedUI.Clear();
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00011C30 File Offset: 0x0000FE30
	public void PrintWhiteList()
	{
		foreach (SBGUIElement sbguielement in this.whitelistedUI.Keys)
		{
			Debug.LogError("white list:" + sbguielement.name);
		}
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00011CAC File Offset: 0x0000FEAC
	private string PrintUnrestrictedElements()
	{
		string text = string.Empty;
		foreach (SBGUIElement sbguielement in this.whitelistedUI.Keys)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"{",
				sbguielement.name,
				":",
				this.whitelistedUI[sbguielement].ToString(),
				"}"
			});
		}
		return text;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00011D64 File Offset: 0x0000FF64
	private void MuteScreens(bool mute)
	{
		foreach (SBGUIScreen sbguiscreen in this.GUIScreenStack)
		{
			sbguiscreen.MuteButtons(mute);
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00011DCC File Offset: 0x0000FFCC
	private static Camera GetEditorCamera()
	{
		return null;
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00011DD0 File Offset: 0x0000FFD0
	public static float GetScreenWidth()
	{
		Camera editorCamera = SBGUI.GetEditorCamera();
		return (!(editorCamera != null)) ? ((float)Screen.width) : editorCamera.pixelWidth;
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00011E00 File Offset: 0x00010000
	public static float GetScreenHeight()
	{
		Camera editorCamera = SBGUI.GetEditorCamera();
		return (!(editorCamera != null)) ? ((float)Screen.height) : editorCamera.pixelHeight;
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00011E30 File Offset: 0x00010030
	public static float GetDpi()
	{
		float num = Screen.dpi;
		if (num == 0f)
		{
			num = 100f;
		}
		return num;
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00011E58 File Offset: 0x00010058
	public bool CheckWhitelisted(SBGUIElement elem)
	{
		return this.whitelistedUI.ContainsKey(elem);
	}

	// Token: 0x04000251 RID: 593
	private const float Z_STEP = 0.001f;

	// Token: 0x04000252 RID: 594
	private List<SBGUIScreen> GUIScreenStack = new List<SBGUIScreen>();

	// Token: 0x04000253 RID: 595
	private Dictionary<SBGUIElement, int> whitelistedUI = new Dictionary<SBGUIElement, int>();

	// Token: 0x04000254 RID: 596
	private Dictionary<SBGUIElement, int> backUpWhitelistedUI = new Dictionary<SBGUIElement, int>();

	// Token: 0x04000255 RID: 597
	public SBGUIScreen GUIScreen;

	// Token: 0x04000256 RID: 598
	private static SBGUI instance;
}
