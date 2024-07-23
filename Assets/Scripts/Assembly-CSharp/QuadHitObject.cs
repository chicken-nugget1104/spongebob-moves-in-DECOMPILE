using System;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class QuadHitObject
{
	// Token: 0x0600215A RID: 8538 RVA: 0x000CE4D0 File Offset: 0x000CC6D0
	public QuadHitObject(Vector2 center, float width, float height)
	{
		this.center = center;
		this.width = width;
		this.height = height;
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000CE4F0 File Offset: 0x000CC6F0
	public void Initialize(Vector2 center, float width, float height)
	{
		this.center = center;
		this.width = width;
		this.height = height;
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000CE508 File Offset: 0x000CC708
	public virtual bool Intersects(Transform transform, Ray ray, Vector2 offset)
	{
		float num = 0.5f * this.width;
		float num2 = 0.5f * this.height;
		Plane plane = new Plane(-ray.direction, transform.position);
		float d;
		if (plane.Raycast(ray, out d))
		{
			Vector3 vector = transform.InverseTransformPoint(ray.origin + ray.direction * d) + new Vector3(this.center.x + offset.x, this.center.y + offset.y, 0f);
			return -num < vector.x && vector.x < num && -num2 < vector.y && vector.y < num2;
		}
		return false;
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x0600215D RID: 8541 RVA: 0x000CE5E8 File Offset: 0x000CC7E8
	// (set) Token: 0x0600215E RID: 8542 RVA: 0x000CE5F0 File Offset: 0x000CC7F0
	public Vector2 Center
	{
		get
		{
			return this.center;
		}
		set
		{
			this.center = value;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x0600215F RID: 8543 RVA: 0x000CE5FC File Offset: 0x000CC7FC
	// (set) Token: 0x06002160 RID: 8544 RVA: 0x000CE604 File Offset: 0x000CC804
	public float Height
	{
		get
		{
			return this.height;
		}
		set
		{
			this.height = value;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06002161 RID: 8545 RVA: 0x000CE610 File Offset: 0x000CC810
	// (set) Token: 0x06002162 RID: 8546 RVA: 0x000CE618 File Offset: 0x000CC818
	public float Width
	{
		get
		{
			return this.width;
		}
		set
		{
			this.width = value;
		}
	}

	// Token: 0x040014A8 RID: 5288
	private Vector2 center;

	// Token: 0x040014A9 RID: 5289
	private float height;

	// Token: 0x040014AA RID: 5290
	private float width;
}
