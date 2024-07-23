using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public static class AmazonGameCircleExampleGUIHelpers
{
	// Token: 0x060001A0 RID: 416 RVA: 0x000080D8 File Offset: 0x000062D8
	public static void SetGUISkinTouchFriendly(GUISkin skin)
	{
		skin.button.fixedHeight = 48f;
		skin.label.fixedHeight = 48f;
		skin.textField.fixedHeight = 48f;
		skin.horizontalSlider.fixedHeight = 48f;
		skin.toggle.fixedHeight = 48f;
		skin.horizontalSlider.fixedHeight = 48f;
		skin.horizontalSliderThumb.fixedHeight = 48f;
		skin.horizontalSliderThumb.fixedWidth = 48f;
		skin.verticalScrollbar.fixedWidth = 48f;
		skin.verticalScrollbarThumb.fixedWidth = 48f;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x00008188 File Offset: 0x00006388
	public static void CenteredLabel(string text, params GUILayoutOption[] options)
	{
		AmazonGameCircleExampleGUIHelpers.AnchoredLabel(text, TextAnchor.MiddleCenter, options);
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00008194 File Offset: 0x00006394
	public static void AnchoredLabel(string text, TextAnchor alignment, params GUILayoutOption[] options)
	{
		TextAnchor alignment2 = GUI.skin.label.alignment;
		GUI.skin.label.alignment = alignment;
		GUILayout.Label(text, options);
		GUI.skin.label.alignment = alignment2;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x000081D8 File Offset: 0x000063D8
	public static bool FoldoutWithLabel(bool currentValue, string label)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		Color color = GUI.color;
		if (currentValue)
		{
			GUI.color = AmazonGameCircleExampleGUIHelpers.foldoutOpenColor;
		}
		if (AmazonGameCircleExampleGUIHelpers.FoldoutButton())
		{
			currentValue = !currentValue;
		}
		GUI.color = color;
		AmazonGameCircleExampleGUIHelpers.AnchoredLabel(label, TextAnchor.UpperCenter, new GUILayoutOption[0]);
		GUILayout.Label(GUIContent.none, new GUILayoutOption[]
		{
			GUILayout.Width(48f)
		});
		GUILayout.EndHorizontal();
		return currentValue;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000824C File Offset: 0x0000644C
	public static void BoxedCenteredLabel(string text)
	{
		GUILayout.BeginHorizontal(GUI.skin.box, new GUILayoutOption[0]);
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(text, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00008280 File Offset: 0x00006480
	public static float DisplayCenteredSlider(float currentValue, float minValue, float maxValue, string valueDisplayString)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		AmazonGameCircleExampleGUIHelpers.AnchoredLabel(string.Format(valueDisplayString, minValue), TextAnchor.UpperCenter, new GUILayoutOption[]
		{
			GUILayout.Width(75f)
		});
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		currentValue = GUILayout.HorizontalSlider(currentValue, minValue, maxValue, new GUILayoutOption[0]);
		AmazonGameCircleExampleGUIHelpers.AnchoredLabel(string.Format(valueDisplayString, currentValue), TextAnchor.UpperCenter, new GUILayoutOption[0]);
		GUILayout.EndVertical();
		AmazonGameCircleExampleGUIHelpers.AnchoredLabel(string.Format(valueDisplayString, maxValue), TextAnchor.UpperCenter, new GUILayoutOption[]
		{
			GUILayout.Width(75f)
		});
		GUILayout.EndHorizontal();
		return currentValue;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00008320 File Offset: 0x00006520
	public static void BeginMenuLayout()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width),
			GUILayout.Height((float)Screen.height)
		});
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width * 0.075f)
		});
		GUILayout.Label(GUIContent.none, new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width * 0.075f)
		});
		GUILayout.EndVertical();
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x000083B4 File Offset: 0x000065B4
	public static void EndMenuLayout()
	{
		GUILayout.EndVertical();
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width * 0.075f)
		});
		GUILayout.Label(GUIContent.none, new GUILayoutOption[]
		{
			GUILayout.Width((float)Screen.width * 0.075f)
		});
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00008414 File Offset: 0x00006614
	private static bool FoldoutButton()
	{
		float fixedHeight = GUI.skin.button.fixedHeight;
		GUI.skin.button.fixedHeight = 48f;
		bool result = GUILayout.Button(GUIContent.none, new GUILayoutOption[]
		{
			GUILayout.Width(48f),
			GUILayout.Height(48f)
		});
		GUI.skin.button.fixedHeight = fixedHeight;
		return result;
	}

	// Token: 0x040000A1 RID: 161
	private const float foldoutButtonWidth = 48f;

	// Token: 0x040000A2 RID: 162
	private const float foldoutButtonHeight = 48f;

	// Token: 0x040000A3 RID: 163
	private const float sliderMinMaxValuesLabelWidth = 75f;

	// Token: 0x040000A4 RID: 164
	private const float uiHeight = 48f;

	// Token: 0x040000A5 RID: 165
	private const float uiSliderWidth = 48f;

	// Token: 0x040000A6 RID: 166
	private const float uiSliderHeight = 48f;

	// Token: 0x040000A7 RID: 167
	private const float uiScrollBarWidth = 48f;

	// Token: 0x040000A8 RID: 168
	private const float menuPadding = 0.075f;

	// Token: 0x040000A9 RID: 169
	private static readonly Color foldoutOpenColor = new Color(0.2f, 0.2f, 0.2f, 1f);
}
