using System;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class SoaringAdView : MonoBehaviour
{
	// Token: 0x06001946 RID: 6470 RVA: 0x000A6C44 File Offset: 0x000A4E44
	public static SoaringAdView CreateAdView(SoaringAdData adData, SoaringAdServer server, SoaringContext context)
	{
		if (adData == null)
		{
			return null;
		}
		if (SoaringAdView.sBlankTexture == null)
		{
			SoaringAdView.sBlankStyle = new GUIStyle();
			SoaringAdView.sBlankTexture = new Texture2D(1, 1);
			SoaringAdView.sBlankTexture.SetPixel(0, 0, Color.clear);
			SoaringAdView.sBlankTexture.Apply();
		}
		if (SoaringAdView.displayObject != null)
		{
			return null;
		}
		SoaringAdView.displayObject = new GameObject("SoaringAdView");
		SoaringAdView soaringAdView = SoaringAdView.displayObject.AddComponent<SoaringAdView>();
		soaringAdView.Initialize(adData, server, context);
		return soaringAdView;
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x000A6CD0 File Offset: 0x000A4ED0
	public void Initialize(SoaringAdData adData, SoaringAdServer adServer, SoaringContext context)
	{
		this.mAdvertData = adData;
		this.mAdServer = adServer;
		this.mContext = context;
		this.Update();
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x000A6CF0 File Offset: 0x000A4EF0
	private void Update()
	{
		if (this.mAdvertData == null)
		{
			return;
		}
		float num = (float)Screen.height;
		float num2 = (float)Screen.width;
		if ((this.mScreenSize.x == num2 || this.mScreenSize.x == num2) && (this.mScreenSize.y == num2 || this.mScreenSize.y == num2))
		{
			return;
		}
		float num3 = num2 / 640f;
		float num4 = num / 640f;
		float num5 = num3;
		if (num4 < num3)
		{
			num5 = num4;
		}
		float num6 = (float)this.mAdvertData.AdTexture.height;
		float num7 = (float)this.mAdvertData.AdTexture.width;
		float num8 = num2 / (float)this.mAdvertData.AdTexture.width;
		float num9 = num / (float)this.mAdvertData.AdTexture.height;
		if (num8 < 1f || num9 < 1f)
		{
			float num10 = num8;
			if (num9 < num8)
			{
				num10 = num9;
			}
			num7 *= num10;
			num6 *= num10;
		}
		else
		{
			num7 *= num5;
			num6 *= num5;
		}
		float left = num2 / 2f - num7 / 2f;
		float top = num / 2f - num6 / 2f;
		this.mDisplayRect = new Rect(left, top, num7, num6);
		this.mScreenSize.x = (float)Screen.width;
		this.mScreenSize.y = (float)Screen.height;
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000A6E74 File Offset: 0x000A5074
	private void OnGUI()
	{
		if (this.mScreenSize.x == 0f || this.mScreenSize.y == 0f)
		{
			return;
		}
		if (GUI.Button(new Rect(0f, 0f, this.mScreenSize.x, this.mScreenSize.y), SoaringAdView.sBlankTexture, SoaringAdView.sBlankStyle))
		{
			Soaring.Delegate.OnAdServed(true, this.mAdvertData, SoaringAdServerState.Closed, this.mContext);
			this.mAdvertData.Invalidate();
			SoaringAdView.displayObject = null;
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GUI.DrawTexture(this.mDisplayRect, this.mAdvertData.AdTexture, ScaleMode.StretchToFill);
		if (GUI.Button(this.mDisplayRect, SoaringAdView.sBlankTexture, SoaringAdView.sBlankStyle))
		{
			Soaring.Delegate.OnAdServed(true, this.mAdvertData, SoaringAdServerState.Clicked, this.mContext);
			this.mAdvertData.OpenAdPage();
			this.mAdServer.MarkAdAsShown(this.mAdvertData);
			this.mAdvertData.Invalidate();
			SoaringAdView.displayObject = null;
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x04001075 RID: 4213
	private const int OptimalScreenWidth = 960;

	// Token: 0x04001076 RID: 4214
	private const int OptimalScreenHeight = 640;

	// Token: 0x04001077 RID: 4215
	private static Texture2D sBlankTexture;

	// Token: 0x04001078 RID: 4216
	private static GUIStyle sBlankStyle;

	// Token: 0x04001079 RID: 4217
	private static GameObject displayObject;

	// Token: 0x0400107A RID: 4218
	private SoaringAdData mAdvertData;

	// Token: 0x0400107B RID: 4219
	private SoaringAdServer mAdServer;

	// Token: 0x0400107C RID: 4220
	private Rect mDisplayRect;

	// Token: 0x0400107D RID: 4221
	private Vector2 mScreenSize = new Vector2(0f, 0f);

	// Token: 0x0400107E RID: 4222
	private SoaringContext mContext;
}
