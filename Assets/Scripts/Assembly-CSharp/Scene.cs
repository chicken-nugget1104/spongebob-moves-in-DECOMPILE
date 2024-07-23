using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002AA RID: 682
public class Scene
{
	// Token: 0x060014DB RID: 5339 RVA: 0x0008C3EC File Offset: 0x0008A5EC
	public Scene(Terrain terrain, int depth)
	{
		this.terrain = terrain;
		this.depth = depth;
		AlignedBox worldExtent = this.terrain.WorldExtent;
		float num = worldExtent.xmax - worldExtent.xmin;
		float num2 = worldExtent.ymax - worldExtent.ymin;
		float num3 = 0.5f * (worldExtent.xmax + worldExtent.xmin);
		float num4 = 0.5f * (worldExtent.ymax + worldExtent.ymin);
		this.root = new Scene.Node(new AlignedBox(num3 - num, num3 + num, num4 - num2, num4 + num2));
		this.Generate(this.root, 1);
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x0008C48C File Offset: 0x0008A68C
	public void OnUpdate(List<Simulated> simulateds)
	{
		foreach (Simulated simulated in simulateds)
		{
			AlignedBox box = simulated.Box;
			if (simulated.prevSceneBox.xmin != box.xmin || simulated.prevSceneBox.xmax != box.xmax || simulated.prevSceneBox.ymin != box.ymin || simulated.prevSceneBox.ymin != box.ymin)
			{
				simulated.prevSceneBox.xmin = simulated.Box.xmin;
				simulated.prevSceneBox.xmax = simulated.Box.xmax;
				simulated.prevSceneBox.ymin = simulated.Box.ymin;
				simulated.prevSceneBox.ymax = simulated.Box.ymax;
				Scene.Node node = simulated.Variable["scene.node"] as Scene.Node;
				if (node != null)
				{
					if (AlignedBox.Contains(node.box, simulated.Box))
					{
						Scene.Node node2 = this.FilterDown(node, simulated);
						if (node != node2)
						{
							node.entities.Remove(simulated);
							node2.entities.Add(simulated);
							if (node.blockerEntities.Contains(simulated))
							{
								node.blockerEntities.Remove(simulated);
								node2.blockerEntities.Add(simulated);
							}
							simulated.Variable["scene.node"] = node2;
						}
					}
					else
					{
						node.entities.Remove(simulated);
						node.blockerEntities.Remove(simulated);
						simulated.Variable["scene.node"] = null;
						this.Filter(this.root, simulated);
					}
				}
				else
				{
					this.Filter(simulated);
				}
			}
		}
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x0008C684 File Offset: 0x0008A884
	public void Add(Simulated entity)
	{
		entity.Variable["scene.node"] = null;
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x0008C698 File Offset: 0x0008A898
	public void Remove(Simulated entity)
	{
		Scene.Node node = entity.Variable["scene.node"] as Scene.Node;
		if (node != null)
		{
			node.blockerEntities.Remove(entity);
			node.entities.Remove(entity);
		}
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x0008C6DC File Offset: 0x0008A8DC
	public void Find(AlignedBox box, ref List<Simulated> result)
	{
		result.Clear();
		this.Find(this.root, box, ref result);
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x0008C6F4 File Offset: 0x0008A8F4
	public void FindPlacementBlockers(AlignedBox box, ref List<Simulated> result)
	{
		result.Clear();
		this.FindPlacementBlockers(this.root, box, ref result);
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x0008C70C File Offset: 0x0008A90C
	public void Find(Ray ray, ref List<Simulated> result)
	{
		result.Clear();
		Vector3 vector;
		if (this.terrain.ComputeIntersection(ray, out vector))
		{
			Segment segment;
			segment.first = new Vector2(ray.origin.x, ray.origin.y);
			segment.second = new Vector2(vector.x, vector.y);
			this.Find(this.root, ray, segment, ref result);
		}
		result.Sort(new Scene.DistanceCompare(new Vector2(ray.origin.x, ray.origin.y)));
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x0008C7B8 File Offset: 0x0008A9B8
	private Scene.Node Filter(Simulated entity)
	{
		Scene.Node node = entity.Variable["scene.node"] as Scene.Node;
		if (node != null)
		{
			node.entities.Remove(entity);
			node.blockerEntities.Remove(entity);
			entity.Variable["scene.node"] = null;
		}
		return this.Filter(this.root, entity);
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x0008C81C File Offset: 0x0008AA1C
	private Scene.Node Filter(Scene.Node node, Simulated simulated)
	{
		for (Scene.Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			if (AlignedBox.Contains(node2.box, simulated.Box))
			{
				return this.Filter(node2, simulated);
			}
		}
		node.entities.Add(simulated);
		bool flag = simulated.entity.HasDecorator<StructureDecorator>() && simulated.entity.GetDecorator<StructureDecorator>().ShouldBlockPlacement;
		if (flag)
		{
			node.blockerEntities.Add(simulated);
		}
		simulated.Variable["scene.node"] = node;
		simulated.prevSceneBox.xmin = simulated.Box.xmin;
		simulated.prevSceneBox.xmax = simulated.Box.xmax;
		simulated.prevSceneBox.ymin = simulated.Box.ymin;
		simulated.prevSceneBox.ymax = simulated.Box.ymax;
		return node;
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x0008C910 File Offset: 0x0008AB10
	private Scene.Node FilterDown(Scene.Node node, Simulated simulated)
	{
		for (Scene.Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			if (AlignedBox.Contains(node2.box, simulated.Box))
			{
				return this.FilterDown(node2, simulated);
			}
		}
		return node;
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x0008C958 File Offset: 0x0008AB58
	private void FindPlacementBlockers(Scene.Node node, AlignedBox box, ref List<Simulated> result)
	{
		if (node == null)
		{
			return;
		}
		if (AlignedBox.Intersects(node.box, box))
		{
			foreach (Simulated simulated in node.blockerEntities)
			{
				if (AlignedBox.Intersects(simulated.Box, box))
				{
					result.Add(simulated);
				}
			}
		}
		for (Scene.Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			this.FindPlacementBlockers(node2, box, ref result);
		}
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x0008CA0C File Offset: 0x0008AC0C
	private void Find(Scene.Node node, AlignedBox box, ref List<Simulated> result)
	{
		if (node == null)
		{
			return;
		}
		if (AlignedBox.Intersects(node.box, box))
		{
			foreach (Simulated simulated in node.entities)
			{
				if (AlignedBox.Intersects(simulated.Box, box))
				{
					result.Add(simulated);
				}
			}
		}
		for (Scene.Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			this.Find(node2, box, ref result);
		}
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x0008CAC0 File Offset: 0x0008ACC0
	private void Find(Scene.Node node, Ray ray, Segment segment, ref List<Simulated> result)
	{
		if (node == null)
		{
			return;
		}
		if (AlignedBox.Intersects(node.box, segment))
		{
			foreach (Simulated simulated in node.entities)
			{
				if (AlignedBox.Intersects(simulated.Box, segment) && simulated.Intersects(ray))
				{
					result.Add(simulated);
				}
			}
		}
		for (Scene.Node node2 = node.firstChild; node2 != null; node2 = node2.nextSibling)
		{
			this.Find(node2, ray, segment, ref result);
		}
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x0008CB84 File Offset: 0x0008AD84
	private void Generate(Scene.Node parent, int depth)
	{
		if (depth >= this.depth)
		{
			return;
		}
		float num = 0.5f * (parent.box.xmax + parent.box.xmin);
		float num2 = 0.25f * (parent.box.xmax - parent.box.xmin);
		float num3 = 0.5f * num2;
		float num4 = 0.5f * (parent.box.ymax + parent.box.ymin);
		float num5 = 0.25f * (parent.box.ymax - parent.box.ymin);
		float num6 = 0.5f * num5;
		Scene.Node node = new Scene.Node(new AlignedBox(num + num3 - num2, num + num3 + num2, num4 + num6 - num5, num4 + num6 + num5));
		parent.AddChild(node);
		this.Generate(node, depth + 1);
		Scene.Node node2 = new Scene.Node(new AlignedBox(num - num3 - num2, num - num3 + num2, num4 + num6 - num5, num4 + num6 + num5));
		parent.AddChild(node2);
		this.Generate(node2, depth + 1);
		Scene.Node node3 = new Scene.Node(new AlignedBox(num - num3 - num2, num - num3 + num2, num4 - num6 - num5, num4 - num6 + num5));
		parent.AddChild(node3);
		this.Generate(node3, depth + 1);
		Scene.Node node4 = new Scene.Node(new AlignedBox(num + num3 - num2, num + num3 + num2, num4 - num6 - num5, num4 - num6 + num5));
		parent.AddChild(node4);
		this.Generate(node4, depth + 1);
	}

	// Token: 0x04000E8D RID: 3725
	private Terrain terrain;

	// Token: 0x04000E8E RID: 3726
	private int depth;

	// Token: 0x04000E8F RID: 3727
	private Scene.Node root;

	// Token: 0x020002AB RID: 683
	private class DistanceCompare : IComparer<Simulated>
	{
		// Token: 0x060014E9 RID: 5353 RVA: 0x0008CD08 File Offset: 0x0008AF08
		public DistanceCompare(Vector2 point)
		{
			this.point = point;
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0008CD18 File Offset: 0x0008AF18
		public int Compare(Simulated lhs, Simulated rhs)
		{
			if ((lhs.Position - this.point).sqrMagnitude < (rhs.Position - this.point).sqrMagnitude)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x04000E90 RID: 3728
		private Vector2 point;
	}

	// Token: 0x020002AC RID: 684
	private class Node
	{
		// Token: 0x060014EB RID: 5355 RVA: 0x0008CD60 File Offset: 0x0008AF60
		public Node(AlignedBox box)
		{
			this.box = box;
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0008CD88 File Offset: 0x0008AF88
		public void AddChild(Scene.Node child)
		{
			if (this.firstChild == null)
			{
				this.firstChild = child;
			}
			else
			{
				this.firstChild.AddSibling(child);
			}
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0008CDB0 File Offset: 0x0008AFB0
		public void AddSibling(Scene.Node sibling)
		{
			if (this.nextSibling == null)
			{
				this.nextSibling = sibling;
			}
			else
			{
				this.nextSibling.AddSibling(sibling);
			}
		}

		// Token: 0x04000E91 RID: 3729
		public Scene.Node firstChild;

		// Token: 0x04000E92 RID: 3730
		public Scene.Node nextSibling;

		// Token: 0x04000E93 RID: 3731
		public AlignedBox box;

		// Token: 0x04000E94 RID: 3732
		public List<Simulated> entities = new List<Simulated>();

		// Token: 0x04000E95 RID: 3733
		public List<Simulated> blockerEntities = new List<Simulated>();
	}
}
