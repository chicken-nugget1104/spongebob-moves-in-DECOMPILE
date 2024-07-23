using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025D RID: 605
public abstract class VisualSpawn : SessionActionSpawn
{
	// Token: 0x06001360 RID: 4960 RVA: 0x000857F8 File Offset: 0x000839F8
	protected virtual void Initialize(Game game, SessionActionTracker parentAction, Vector3 offset, float rotationCwDeg, float alpha, Vector2 inScale)
	{
		base.RegisterNewInstance(game, parentAction);
		this.offset = offset;
		this.Rotation = rotationCwDeg;
		this.Alpha = alpha;
		this.Scale = inScale;
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06001361 RID: 4961 RVA: 0x0008582C File Offset: 0x00083A2C
	// (set) Token: 0x06001362 RID: 4962 RVA: 0x00085834 File Offset: 0x00083A34
	protected float Rotation
	{
		get
		{
			return this.rotationCwDeg;
		}
		set
		{
			this.rotationCwDeg = value;
			this.direction = new Vector3(Mathf.Sin(value * 0.017453292f), Mathf.Cos(value * 0.017453292f), 0f);
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06001363 RID: 4963 RVA: 0x00085868 File Offset: 0x00083A68
	protected Vector3 Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06001364 RID: 4964 RVA: 0x00085870 File Offset: 0x00083A70
	// (set) Token: 0x06001365 RID: 4965 RVA: 0x00085878 File Offset: 0x00083A78
	protected float Alpha
	{
		get
		{
			return this.alpha;
		}
		set
		{
			this.alpha = value;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06001366 RID: 4966 RVA: 0x00085884 File Offset: 0x00083A84
	// (set) Token: 0x06001367 RID: 4967 RVA: 0x0008588C File Offset: 0x00083A8C
	protected Vector2 Scale
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x00085898 File Offset: 0x00083A98
	protected void NormalizeRotationAndPushToEdge(float widthOver2, float heightOver2)
	{
		this.rotationCwDeg = TFMath.Modulo(this.rotationCwDeg, 360f);
		if (this.rotationCwDeg < 0f)
		{
			this.rotationCwDeg += 360f;
		}
		this.Rotation = this.rotationCwDeg;
		float num;
		float num2;
		if (this.rotationCwDeg < 45f || this.rotationCwDeg > 315f)
		{
			num = widthOver2 * Mathf.Sin(this.rotationCwDeg * 0.017453292f);
			num2 = heightOver2;
		}
		else if (this.rotationCwDeg < 135f)
		{
			num = widthOver2;
			num2 = heightOver2 * Mathf.Cos(this.rotationCwDeg * 0.017453292f);
		}
		else if (this.rotationCwDeg < 225f)
		{
			num = widthOver2 * Mathf.Sin(this.rotationCwDeg * 0.017453292f);
			num2 = -heightOver2;
		}
		else
		{
			num = -widthOver2;
			num2 = heightOver2 * Mathf.Cos(this.rotationCwDeg * 0.017453292f);
		}
		this.offset = new Vector3(this.offset.x + num, this.offset.y + num2, this.offset.z);
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x000859C4 File Offset: 0x00083BC4
	public void Parse(Dictionary<string, object> data, bool isOffsetRequired, Vector3 defaultOffset, float offsetConversionScale)
	{
		this.rotationCwDeg = 0f;
		if (data.ContainsKey("rotation"))
		{
			this.rotationCwDeg = TFUtils.LoadFloat(data, "rotation");
		}
		this.Rotation = this.rotationCwDeg;
		if (isOffsetRequired && !data.ContainsKey("offset"))
		{
			TFUtils.Assert(!isOffsetRequired || data.ContainsKey("offset"), "Offset is required for this session action. data=" + TFUtils.DebugDictToString(data));
		}
		this.offset = defaultOffset;
		if (data.ContainsKey("offset"))
		{
			TFUtils.LoadVector3(out this.offset, TFUtils.LoadDict(data, "offset"));
			this.offset.Scale(new Vector3(offsetConversionScale, offsetConversionScale, offsetConversionScale));
		}
		this.alpha = 1f;
		if (data.ContainsKey("alpha"))
		{
			this.alpha = TFUtils.LoadFloat(data, "alpha");
		}
		this.scale = Vector2.one;
		if (data.ContainsKey("scale"))
		{
			TFUtils.LoadVector2(out this.scale, (Dictionary<string, object>)data["scale"]);
		}
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x00085AF4 File Offset: 0x00083CF4
	public void AddToDict(ref Dictionary<string, object> dict)
	{
		dict["rotation"] = this.rotationCwDeg;
		dict["alpha"] = this.alpha;
		dict["offset"] = this.offset;
		dict["scale"] = this.scale;
	}

	// Token: 0x04000D74 RID: 3444
	public const string OFFSET = "offset";

	// Token: 0x04000D75 RID: 3445
	public const string ROTATION = "rotation";

	// Token: 0x04000D76 RID: 3446
	public const string ALPHA = "alpha";

	// Token: 0x04000D77 RID: 3447
	public const string SCALE = "scale";

	// Token: 0x04000D78 RID: 3448
	protected Vector3 offset;

	// Token: 0x04000D79 RID: 3449
	private float rotationCwDeg;

	// Token: 0x04000D7A RID: 3450
	private Vector3 direction;

	// Token: 0x04000D7B RID: 3451
	private float alpha = 1f;

	// Token: 0x04000D7C RID: 3452
	private Vector2 scale = Vector2.one;
}
