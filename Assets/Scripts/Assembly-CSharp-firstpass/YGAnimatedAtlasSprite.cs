using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class YGAnimatedAtlasSprite : YGAtlasSprite
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000964 RID: 2404 RVA: 0x00024620 File Offset: 0x00022820
	// (set) Token: 0x06000965 RID: 2405 RVA: 0x00024628 File Offset: 0x00022828
	public bool IsPlaying { get; protected set; }

	// Token: 0x06000966 RID: 2406 RVA: 0x00024634 File Offset: 0x00022834
	protected override void OnEnable()
	{
		this.IsPlaying = false;
		base.OnEnable();
		this.sleep = 1f / (float)this.framesPerSecond;
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00024664 File Offset: 0x00022864
	protected override void OnDisable()
	{
		this.StopAnimation();
		base.OnDisable();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00024674 File Offset: 0x00022874
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

	// Token: 0x06000969 RID: 2409 RVA: 0x000246A4 File Offset: 0x000228A4
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

	// Token: 0x0600096A RID: 2410 RVA: 0x00024758 File Offset: 0x00022958
	public void StopAnimation()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00024760 File Offset: 0x00022960
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

	// Token: 0x0600096C RID: 2412 RVA: 0x0002478C File Offset: 0x0002298C
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

	// Token: 0x0600096D RID: 2413 RVA: 0x000247B8 File Offset: 0x000229B8
	private IEnumerator AnimateDefault()
	{
		this.IsPlaying = true;
		yield return base.StartCoroutine(this.PlayForward(0));
		this.SetFrame(0);
		this.IsPlaying = false;
		yield break;
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x000247D4 File Offset: 0x000229D4
	private IEnumerator AnimateClamp()
	{
		this.IsPlaying = true;
		yield return base.StartCoroutine(this.PlayForward(0));
		this.IsPlaying = false;
		yield break;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x000247F0 File Offset: 0x000229F0
	private IEnumerator AnimateLoop()
	{
		this.IsPlaying = true;
		while (this.IsPlaying)
		{
			yield return base.StartCoroutine(this.PlayForward(0));
		}
		yield break;
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0002480C File Offset: 0x00022A0C
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

	// Token: 0x06000971 RID: 2417 RVA: 0x00024828 File Offset: 0x00022A28
	public override void Load()
	{
		if (this.sprite == null)
		{
			Debug.LogError("null sprite being assembled: " + base.gameObject.name);
			return;
		}
		if (string.IsNullOrEmpty(this.sprite.name))
		{
			this.sprite = this.LoadEmptySprite();
			base.AssembleMesh();
			return;
		}
		this.LoadSprite();
		if (this.frameLayout.count <= 0)
		{
			this.frameLayout.count = (int)(this.frameLayout.layout.x * this.frameLayout.layout.y);
		}
		this.frames = new Rect[this.frameLayout.count];
		int num = 0;
		int num2 = 0;
		while ((float)num2 < this.frameLayout.layout.y)
		{
			int num3 = 0;
			while ((float)num3 < this.frameLayout.layout.x)
			{
				this.frames[num] = new Rect((float)num3 * this.frameLayout.size.x + this.sprite.coords.x, (float)num2 * this.frameLayout.size.y + this.sprite.coords.y, this.frameLayout.size.x, this.frameLayout.size.y);
				num++;
				if (num >= this.frameLayout.count)
				{
					break;
				}
				num3++;
			}
			num2++;
		}
		base.Load();
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x000249C4 File Offset: 0x00022BC4
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

	// Token: 0x06000973 RID: 2419 RVA: 0x00024A48 File Offset: 0x00022C48
	public Vector2[] FrameUVs(int frame)
	{
		if (this.frames == null || this.frames.Length <= frame)
		{
			Debug.LogWarning(string.Format("frame {0} out of range", frame));
			return null;
		}
		Rect rect = this.frames[frame];
		YGSprite.BuildUVs(rect, this.textureSize, ref this.uvs);
		return this.uvs;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00024AB0 File Offset: 0x00022CB0
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

	// Token: 0x04000609 RID: 1545
	public YGAnimatedAtlasSprite.FrameLayout frameLayout;

	// Token: 0x0400060A RID: 1546
	public int framesPerSecond = 10;

	// Token: 0x0400060B RID: 1547
	public bool startAutomatically = true;

	// Token: 0x0400060C RID: 1548
	protected int currentFrame;

	// Token: 0x0400060D RID: 1549
	protected Rect[] frames;

	// Token: 0x0400060E RID: 1550
	public WrapMode wrapMode = WrapMode.Loop;

	// Token: 0x0400060F RID: 1551
	private float sleep;

	// Token: 0x04000610 RID: 1552
	private Func<IEnumerator> animFunc;

	// Token: 0x02000102 RID: 258
	[Serializable]
	public class FrameLayout
	{
		// Token: 0x04000612 RID: 1554
		public Vector2 size = new Vector2(50f, 50f);

		// Token: 0x04000613 RID: 1555
		public Vector2 layout = new Vector2(4f, 3f);

		// Token: 0x04000614 RID: 1556
		public int count;
	}
}
