using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using UnityEngine;

// Token: 0x02000071 RID: 113
public static class FarseerGizmos
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060003D1 RID: 977 RVA: 0x00010708 File Offset: 0x0000E908
	private static Vector3[] Circle16
	{
		get
		{
			if (FarseerGizmos._circle16 == null)
			{
				FarseerGizmos._circle16 = new Vector3[16];
				for (int i = 0; i < FarseerGizmos._circle16.Length; i++)
				{
					float num = (float)i / (float)FarseerGizmos._circle16.Length;
					float f = num * 3.1415927f * 2f;
					FarseerGizmos._circle16[i] = new Vector3(Mathf.Sin(f), Mathf.Cos(f));
				}
			}
			return FarseerGizmos._circle16;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060003D2 RID: 978 RVA: 0x00010784 File Offset: 0x0000E984
	public static bool GizmosActive
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x00010788 File Offset: 0x0000E988
	public static void DrawBody(Body body, Color color)
	{
		if (body == null || body.FixtureList == null)
		{
			return;
		}
		List<Fixture> fixtureList = body.FixtureList;
		int count = fixtureList.Count;
		Transform2D xf = default(Transform2D);
		body.GetTransform(out xf);
		for (int i = 0; i < count; i++)
		{
			FarseerGizmos.DrawShape(fixtureList[i], xf, color);
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x000107E8 File Offset: 0x0000E9E8
	public static void DrawJoint(Joint2D joint)
	{
		if (!joint.Enabled)
		{
			return;
		}
		Body bodyA = joint.BodyA;
		Body bodyB = joint.BodyB;
		Transform2D transform2D;
		bodyA.GetTransform(out transform2D);
		Vector2 vector = Vector2.zero;
		if (!joint.IsFixedType())
		{
			Transform2D transform2D2;
			bodyB.GetTransform(out transform2D2);
			vector = transform2D2.p;
		}
		Vector2 worldAnchorB = joint.WorldAnchorB;
		Vector2 p = transform2D.p;
		Vector2 worldAnchorA = joint.WorldAnchorA;
		Color color = new Color32(128, 205, 205, byte.MaxValue);
		switch (joint.JointType)
		{
		case JointType.Revolute:
			FarseerGizmos.DrawSegment(worldAnchorB, worldAnchorA, color);
			FarseerGizmos.DrawSolidCircle(worldAnchorB, 0.1f, Vector2.zero, Color.red);
			FarseerGizmos.DrawSolidCircle(worldAnchorA, 0.1f, Vector2.zero, Color.blue);
			return;
		case JointType.Distance:
			FarseerGizmos.DrawSegment(worldAnchorA, worldAnchorB, color);
			return;
		case JointType.Pulley:
		{
			PulleyJoint pulleyJoint = (PulleyJoint)joint;
			Vector2 groundAnchorA = pulleyJoint.GroundAnchorA;
			Vector2 groundAnchorB = pulleyJoint.GroundAnchorB;
			FarseerGizmos.DrawSegment(groundAnchorA, worldAnchorA, color);
			FarseerGizmos.DrawSegment(groundAnchorB, worldAnchorB, color);
			FarseerGizmos.DrawSegment(groundAnchorA, groundAnchorB, color);
			return;
		}
		case JointType.Gear:
			FarseerGizmos.DrawSegment(p, vector, color);
			return;
		case JointType.FixedMouse:
			FarseerGizmos.DrawPoint(worldAnchorA, 0.5f, new Color32(0, byte.MaxValue, 0, byte.MaxValue));
			FarseerGizmos.DrawSegment(worldAnchorA, worldAnchorB, new Color32(205, 205, 205, byte.MaxValue));
			return;
		case JointType.FixedRevolute:
			FarseerGizmos.DrawSegment(p, worldAnchorA, color);
			FarseerGizmos.DrawSolidCircle(worldAnchorA, 0.1f, Vector2.zero, Color.magenta);
			return;
		case JointType.FixedDistance:
			FarseerGizmos.DrawSegment(p, worldAnchorA, color);
			FarseerGizmos.DrawSegment(worldAnchorA, worldAnchorB, color);
			return;
		case JointType.FixedLine:
			FarseerGizmos.DrawSegment(p, worldAnchorA, color);
			FarseerGizmos.DrawSegment(worldAnchorA, worldAnchorB, color);
			return;
		case JointType.FixedPrismatic:
			FarseerGizmos.DrawSegment(p, worldAnchorA, color);
			FarseerGizmos.DrawSegment(worldAnchorA, worldAnchorB, color);
			return;
		case JointType.FixedAngle:
			return;
		}
		FarseerGizmos.DrawSegment(p, worldAnchorA, color);
		FarseerGizmos.DrawSegment(worldAnchorA, worldAnchorB, color);
		FarseerGizmos.DrawSegment(vector, worldAnchorB, color);
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x00010A58 File Offset: 0x0000EC58
	public static void DrawShape(Fixture fixture, Transform2D xf, Color color)
	{
		switch (fixture.ShapeType)
		{
		case ShapeType.Circle:
		{
			CircleShape circleShape = (CircleShape)fixture.Shape;
			Vector2 center = MathUtils.Mul(ref xf, circleShape.Position);
			float radius = circleShape.Radius;
			Vector2 axis = MathUtils.Mul(xf.q, new Vector2(1f, 0f));
			FarseerGizmos.DrawSolidCircle(center, radius, axis, color);
			break;
		}
		case ShapeType.Edge:
		{
			EdgeShape edgeShape = (EdgeShape)fixture.Shape;
			Vector2 start = MathUtils.Mul(ref xf, edgeShape.Vertex1);
			Vector2 end = MathUtils.Mul(ref xf, edgeShape.Vertex2);
			FarseerGizmos.DrawSegment(start, end, color);
			break;
		}
		case ShapeType.Polygon:
		{
			PolygonShape polygonShape = (PolygonShape)fixture.Shape;
			int count = polygonShape.Vertices.Count;
			if (count > Settings.MaxPolygonVertices)
			{
				Debug.LogError(string.Format("BREAK: Vertex count of object too high. Verts: {0}, Max: {1}", count, Settings.MaxPolygonVertices));
				Debug.Break();
				return;
			}
			Vector2[] array = new Vector2[Settings.MaxPolygonVertices];
			for (int i = 0; i < count; i++)
			{
				array[i] = MathUtils.Mul(ref xf, polygonShape.Vertices[i]);
			}
			FarseerGizmos.DrawSolidPolygon(array, count, color);
			break;
		}
		case ShapeType.Chain:
		{
			ChainShape chainShape = (ChainShape)fixture.Shape;
			int count2 = chainShape.Vertices.Count;
			Vector2 vector = MathUtils.Mul(ref xf, chainShape.Vertices[count2 - 1]);
			FarseerGizmos.DrawCircle(vector, 0.05f, color);
			for (int j = 0; j < count2; j++)
			{
				Vector2 vector2 = MathUtils.Mul(ref xf, chainShape.Vertices[j]);
				FarseerGizmos.DrawSegment(vector, vector2, color);
				vector = vector2;
			}
			break;
		}
		}
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x00010C34 File Offset: 0x0000EE34
	private static void DrawPolygon(Vector2[] vertices, int count, float red, float green, float blue)
	{
		FarseerGizmos.DrawPolygon(vertices, count, new Color(red, green, blue));
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x00010C48 File Offset: 0x0000EE48
	public static void DrawPolygon(Vector2[] vertices, int count, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		for (int i = 1; i < count; i++)
		{
			Gizmos.DrawLine(vertices[i - 1], vertices[i]);
		}
		Gizmos.DrawLine(vertices[count - 1], vertices[0]);
		Gizmos.color = color2;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00010CCC File Offset: 0x0000EECC
	private static void DrawSolidPolygon(Vector2[] vertices, int count, float red, float green, float blue)
	{
		FarseerGizmos.DrawSolidPolygon(vertices, count, new Color(red, green, blue), true);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00010CE0 File Offset: 0x0000EEE0
	private static void DrawSolidPolygon(Vector2[] vertices, int count, Color color)
	{
		FarseerGizmos.DrawSolidPolygon(vertices, count, color, true);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00010CEC File Offset: 0x0000EEEC
	public static void DrawSolidPolygon(Vector2[] vertices, int count, Color color, bool outline)
	{
		FarseerGizmos.DrawPolygon(vertices, count, color.r, color.g, color.b);
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00010D0C File Offset: 0x0000EF0C
	private static void DrawCircle(Vector2 center, float radius, float red, float green, float blue)
	{
		FarseerGizmos.DrawCircle(center, radius, new Color(red, green, blue));
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00010D20 File Offset: 0x0000EF20
	public static void DrawCircle(Vector2 center, float radius, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Vector3[] array = new Vector3[16];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = radius * FarseerGizmos.Circle16[i] + center;
		}
		for (int j = 1; j < array.Length; j++)
		{
			Gizmos.DrawLine(array[j], array[j - 1]);
		}
		Gizmos.DrawLine(array[0], array[array.Length - 1]);
		Gizmos.color = color2;
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00010DDC File Offset: 0x0000EFDC
	private static void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, float red, float green, float blue)
	{
		FarseerGizmos.DrawSolidCircle(center, radius, axis, new Color(red, green, blue));
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00010DF0 File Offset: 0x0000EFF0
	public static void DrawTransform(ref Transform2D transform)
	{
		Vector2 p = transform.p;
		Vector2 end = p + 0.4f * transform.q.GetXAxis();
		FarseerGizmos.DrawSegment(p, end, Color.red);
		end = p + 0.4f * transform.q.GetYAxis();
		FarseerGizmos.DrawSegment(p, end, Color.green);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00010E54 File Offset: 0x0000F054
	public static void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
	{
		FarseerGizmos.DrawCircle(center, radius, color);
		FarseerGizmos.DrawSegment(center, center + axis * radius, color);
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00010E74 File Offset: 0x0000F074
	public static void DrawAABB(ref AABB aabb, Color color)
	{
		FarseerGizmos.DrawPolygon(new Vector2[]
		{
			new Vector2(aabb.LowerBound.x, aabb.LowerBound.y),
			new Vector2(aabb.UpperBound.x, aabb.LowerBound.y),
			new Vector2(aabb.UpperBound.x, aabb.UpperBound.y),
			new Vector2(aabb.LowerBound.x, aabb.UpperBound.y)
		}, 4, color);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00010F2C File Offset: 0x0000F12C
	private static void DrawSegment(Vector2 start, Vector2 end, float red, float green, float blue)
	{
		FarseerGizmos.DrawSegment(start, end, new Color(red, green, blue));
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00010F40 File Offset: 0x0000F140
	public static void DrawSegment(Vector2 start, Vector2 end, Color color)
	{
		Color color2 = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(start, end);
		Gizmos.color = color2;
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00010F70 File Offset: 0x0000F170
	public static void DrawPoint(Vector2 p, float size, Color color)
	{
		Vector2[] array = new Vector2[4];
		float num = size / 2f;
		array[0] = p + new Vector2(-num, -num);
		array[1] = p + new Vector2(num, -num);
		array[2] = p + new Vector2(num, num);
		array[3] = p + new Vector2(-num, num);
		FarseerGizmos.DrawSolidPolygon(array, 4, color, true);
	}

	// Token: 0x0400020D RID: 525
	private static Vector3[] _circle16;
}
