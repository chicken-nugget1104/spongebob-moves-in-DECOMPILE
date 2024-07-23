using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000103 RID: 259
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class YGAnimatedSprite : YGSprite
{
	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x00024B44 File Offset: 0x00022D44
	// (set) Token: 0x06000978 RID: 2424 RVA: 0x00024B4C File Offset: 0x00022D4C
	public bool IsPlaying { get; protected set; }

	// Token: 0x06000979 RID: 2425 RVA: 0x00024B58 File Offset: 0x00022D58
	protected override void OnEnable()
	{
		this.IsPlaying = false;
		base.OnEnable();
		this.sleep = 1f / (float)this.framesPerSecond;
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00024B88 File Offset: 0x00022D88
	protected override void OnDisable()
	{
		this.StopAnimation();
		base.OnDisable();
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00024B98 File Offset: 0x00022D98
	private void Start()
	{
		if (this.frames == null)
		{
			this.Load();
		}
		if (this.startAutomatically)
		{
			this.StartAnimation();
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00024BC8 File Offset: 0x00022DC8
	public void StartAnimation()
	{
		switch (this.wrapMode)
		{
		case WrapMode.Default:
			this.animFunc = new Func<IEnumerator>(this.AnimateDefault);
			break;
		case WrapMode.Once:
		case WrapMode.ClampForever:
			this.animFunc = new Func<IEnumerator>(this.AnimateClamp);
			break;
		case WrapMode.Loop:
			this.animFunc = new Func<IEnumerator>(this.AnimateLoop);
			break;
		case WrapMode.PingPong:
			this.animFunc = new Func<IEnumerator>(this.AnimatePingPong);
			break;
		}
		base.StartCoroutine(this.animFunc());
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00024C7C File Offset: 0x00022E7C
	public void StopAnimation()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00024C84 File Offset: 0x00022E84
	private IEnumerator PlayForward(int startFrame)
	{
		for (int i = startFrame; i < this.frames.Length; i++)
		{
			if (!this.IsPlaying)
			{
				break;
			}
			this.currentFrame = i;
			this.SetFrame(i);
			yield return new WaitForSeconds(this.sleep);
		}
		yield break;
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00024CB0 File Offset: 0x00022EB0
	private IEnumerator PlayBackward(int startFrame)
	{
		for (int i = startFrame; i > 0; i--)
		{
			if (!this.IsPlaying)
			{
				break;
			}
			this.currentFrame = i;
			this.SetFrame(i);
			yield return new WaitForSeconds(this.sleep);
		}
		yield break;
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00024CDC File Offset: 0x00022EDC
	private IEnumerator AnimateDefault()
	{
		this.IsPlaying = true;
		yield return base.StartCoroutine(this.PlayForward(0));
		this.SetFrame(0);
		this.IsPlaying = false;
		yield break;
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00024CF8 File Offset: 0x00022EF8
	private IEnumerator AnimateClamp()
	{
		this.IsPlaying = true;
		yield return base.StartCoroutine(this.PlayForward(0));
		this.IsPlaying = false;
		yield break;
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00024D14 File Offset: 0x00022F14
	private IEnumerator AnimateLoop()
	{
		this.IsPlaying = true;
		while (this.IsPlaying)
		{
			yield return base.StartCoroutine(this.PlayForward(0));
		}
		yield break;
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00024D30 File Offset: 0x00022F30
	private IEnumerator AnimatePingPong()
	{
		this.IsPlaying = true;
		while (this.IsPlaying)
		{
			yield return base.StartCoroutine(this.PlayForward(0));
			yield return base.StartCoroutine(this.PlayBackward(this.frames.Length - 2));
		}
		yield break;
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00024D4C File Offset: 0x00022F4C
	public override void Load()
	{
		if (this.frameLayout.count <= 0)
		{
			this.frameLayout.count = (int)(this.frameLayout.layout.x * this.frameLayout.layout.y);
		}
		this.frames = new Rect[this.frameLayout.count];
		int num = 0;
		float num2 = this.frameLayout.size.x / this.textureSize.x;
		float num3 = this.frameLayout.size.y / this.textureSize.y;
		int num4 = 0;
		while ((float)num4 < this.frameLayout.layout.y)
		{
			int num5 = 0;
			while ((float)num5 < this.frameLayout.layout.x)
			{
				this.frames[num] = new Rect((float)num5 * num2, (float)num4 * num3, num2, num3);
				num++;
				if (num >= this.frameLayout.count)
				{
					break;
				}
				num5++;
			}
			num4++;
		}
		base.Load();
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00024E74 File Offset: 0x00023074
	public override void AssembleMesh()
	{
		YGSprite.MeshUpdate meshUpdate = new YGSprite.MeshUpdate();
		YGSprite.BuildVerts(this.size, this.scale, ref this.verts);
		YGSprite.BuildColors(this.color, ref this.colors);
		meshUpdate.verts = this.verts;
		meshUpdate.normals = this.normals;
		meshUpdate.colors = this.colors;
		meshUpdate.tris = YGSprite.BuildTris();
		meshUpdate.uvs = this.FrameUVs(this.currentFrame);
		this.UpdateMesh(meshUpdate);
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00024EF8 File Offset: 0x000230F8
	public Vector2[] FrameUVs(int frame)
	{
		if (this.frames == null || this.frames.Length <= frame)
		{
			Debug.LogWarning(string.Format("frame {0} out of range", frame));
			return null;
		}
		Rect rect = this.frames[frame];
		return new Vector2[]
		{
			new Vector2(rect.xMin, 1f - rect.yMin),
			new Vector2(rect.xMax, 1f - rect.yMin),
			new Vector2(rect.xMin, 1f - rect.yMax),
			new Vector2(rect.xMax, 1f - rect.yMax)
		};
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00024FE4 File Offset: 0x000231E4
	protected void SetFrame(int frame)
	{
		YGSprite.MeshUpdate meshUpdate = new YGSprite.MeshUpdate();
		meshUpdate.uvs = this.FrameUVs(frame);
		if (meshUpdate.uvs == null)
		{
			return;
		}
		this.UpdateMesh(meshUpdate);
	}

	// Token: 0x04000615 RID: 1557
	public YGAnimatedSprite.FrameLayout frameLayout;

	// Token: 0x04000616 RID: 1558
	public int framesPerSecond = 10;

	// Token: 0x04000617 RID: 1559
	public bool startAutomatically = true;

	// Token: 0x04000618 RID: 1560
	protected int currentFrame;

	// Token: 0x04000619 RID: 1561
	protected Rect[] frames;

	// Token: 0x0400061A RID: 1562
	public WrapMode wrapMode = WrapMode.Loop;

	// Token: 0x0400061B RID: 1563
	private float sleep;

	// Token: 0x0400061C RID: 1564
	private Func<IEnumerator> animFunc;

	// Token: 0x02000104 RID: 260
	[Serializable]
	public class FrameLayout
	{
		// Token: 0x0400061E RID: 1566
		public Vector2 size = new Vector2(50f, 50f);

		// Token: 0x0400061F RID: 1567
		public Vector2 layout = new Vector2(4f, 3f);

		// Token: 0x04000620 RID: 1568
		public int count;
	}
}
