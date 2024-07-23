using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using UnityEngine;

// Token: 0x020000FE RID: 254
[RequireComponent(typeof(Camera))]
public class YG2DWorld : MonoBehaviour
{
	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000959 RID: 2393 RVA: 0x000244AC File Offset: 0x000226AC
	public Camera RenderCamera
	{
		get
		{
			if (!this.renderCamera)
			{
				this.renderCamera = base.gameObject.GetComponent<Camera>();
			}
			return this.renderCamera;
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x000244D8 File Offset: 0x000226D8
	public Vector2 Cursor2D(Vector3 cursor3d)
	{
		if (!this.RenderCamera)
		{
			Debug.LogWarning("no camera attach to Yarg2DWorld");
			return Vector2.zero;
		}
		cursor3d.z = -this.RenderCamera.transform.position.z;
		return this.RenderCamera.ScreenToWorldPoint(cursor3d);
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600095B RID: 2395 RVA: 0x00024538 File Offset: 0x00022738
	public World World
	{
		get
		{
			if (this.world == null)
			{
				this.world = new World(this.GUIGravity);
			}
			return this.world;
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00024568 File Offset: 0x00022768
	private void OnEnable()
	{
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002456C File Offset: 0x0002276C
	private void OnDisable()
	{
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00024570 File Offset: 0x00022770
	public List<Fixture> GetHitFixtures(Vector2 pos)
	{
		return this.World.TestPointAll(this.Cursor2D(pos));
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0002458C File Offset: 0x0002278C
	public static void UpdateTransform(Transform t, Body body)
	{
		Vector2 position = body.Position;
		float z = body.Rotation * 57.29578f;
		Vector3 position2 = t.position;
		position2.Set(position.x, position.y, position2.z);
		t.position = position2;
		Quaternion rotation = t.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.z = z;
		rotation.eulerAngles = eulerAngles;
		t.rotation = rotation;
	}

	// Token: 0x040005FA RID: 1530
	public Camera renderCamera;

	// Token: 0x040005FB RID: 1531
	private readonly Vector2 GUIGravity = Vector2.zero;

	// Token: 0x040005FC RID: 1532
	protected World world;

	// Token: 0x040005FD RID: 1533
	public bool drawCursor = true;

	// Token: 0x040005FE RID: 1534
	public bool shape = true;

	// Token: 0x040005FF RID: 1535
	public bool joint = true;

	// Token: 0x04000600 RID: 1536
	public bool aabb;

	// Token: 0x04000601 RID: 1537
	public bool pair;

	// Token: 0x04000602 RID: 1538
	public bool centerOfMass;

	// Token: 0x04000603 RID: 1539
	public bool debugPanel;

	// Token: 0x04000604 RID: 1540
	public bool contactPoints;

	// Token: 0x04000605 RID: 1541
	public bool contactNormals;

	// Token: 0x04000606 RID: 1542
	public bool polygonPoints;

	// Token: 0x04000607 RID: 1543
	public bool performanceGraph;

	// Token: 0x04000608 RID: 1544
	public bool controllers;
}
