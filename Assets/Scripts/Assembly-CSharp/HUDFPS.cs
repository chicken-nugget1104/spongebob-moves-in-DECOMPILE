using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200047C RID: 1148
[AddComponentMenu("Utilities/HUDFPS")]
public class HUDFPS : MonoBehaviour
{
	// Token: 0x060023F1 RID: 9201 RVA: 0x000DD460 File Offset: 0x000DB660
	private void Start()
	{
		base.StartCoroutine(this.FPS());
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000DD470 File Offset: 0x000DB670
	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			this.accum += Time.deltaTime / Time.timeScale;
			this.frames++;
		}
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000DD4A8 File Offset: 0x000DB6A8
	private IEnumerator FPS()
	{
		for (;;)
		{
			float fps = (this.accum != 0f) ? ((float)this.frames / this.accum) : 0f;
			this.sFPS = fps.ToString("f" + Mathf.Clamp(this.nbDecimal, 0, 10));
			this.color = ((fps < 30f) ? ((fps <= 10f) ? Color.yellow : Color.red) : Color.green);
			this.accum = 0f;
			this.frames = 0;
			yield return new WaitForSeconds(this.frequency);
		}
		yield break;
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000DD4C4 File Offset: 0x000DB6C4
	private void OnGUI()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.label);
			this.style.normal.textColor = Color.white;
			this.style.alignment = TextAnchor.MiddleCenter;
		}
		GUI.color = ((!this.updateColor) ? Color.white : this.color);
		this.startRect = GUI.Window(0, this.startRect, new GUI.WindowFunction(this.DoMyWindow), string.Empty);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000DD558 File Offset: 0x000DB758
	private void DoMyWindow(int windowID)
	{
		GUI.Label(new Rect(0f, 0f, this.startRect.width, this.startRect.height), string.Concat(new object[]
		{
			this.sFPS,
			"FPS\nLast Save to server at:",
			SBWebFileServer.LastSuccessfulSave,
			"\nMemory: ",
			Profiler.GetTotalAllocatedMemory() / 1024U / 1024U
		}), this.style);
		if (this.allowDrag)
		{
			GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		}
	}

	// Token: 0x04001627 RID: 5671
	public Rect startRect = new Rect(10f, 10f, 275f, 100f);

	// Token: 0x04001628 RID: 5672
	public bool updateColor = true;

	// Token: 0x04001629 RID: 5673
	public bool allowDrag = true;

	// Token: 0x0400162A RID: 5674
	public float frequency = 0.5f;

	// Token: 0x0400162B RID: 5675
	public int nbDecimal = 1;

	// Token: 0x0400162C RID: 5676
	private float accum;

	// Token: 0x0400162D RID: 5677
	private int frames;

	// Token: 0x0400162E RID: 5678
	private Color color = Color.white;

	// Token: 0x0400162F RID: 5679
	private string sFPS = string.Empty;

	// Token: 0x04001630 RID: 5680
	private GUIStyle style;
}
