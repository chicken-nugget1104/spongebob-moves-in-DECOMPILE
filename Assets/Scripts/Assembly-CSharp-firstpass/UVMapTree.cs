using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class UVMapTree
{
	// Token: 0x060007CD RID: 1997 RVA: 0x0001DC3C File Offset: 0x0001BE3C
	public UVMapTree(Vector2 initialSize, int depth = -1)
	{
		if (initialSize.x != initialSize.y)
		{
			Debug.LogError("UVMapTree initial size X must same as Y");
			return;
		}
		this.LastTextureSize = default(Vector3);
		if (depth <= 0)
		{
			depth = 1;
			int num = (int)initialSize.x;
			for (;;)
			{
				num = (int)((float)num * 0.5f);
				if (num < 4)
				{
					break;
				}
				depth++;
			}
		}
		this.NodeLayers = new Vector2[depth];
		int num2 = (int)initialSize.x;
		for (int i = 0; i < depth; i++)
		{
			this.NodeLayers[i] = new Vector2((float)num2, (float)num2);
			num2 /= 2;
		}
		this.RootNode = this.GetBranch();
		this.UVAdjust = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0001DD78 File Offset: 0x0001BF78
	public bool AddTexture(Vector2 textureSize, ref Vector2 uvs)
	{
		this.LastTextureSize.x = textureSize.x;
		this.LastTextureSize.y = textureSize.y;
		bool result = this.RootNode.AddTexture(this, 0, 0, 0);
		uvs = this.lastFoundUV;
		return result;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0001DDC8 File Offset: 0x0001BFC8
	protected void ReturnLeaf(UVMapTree.UVMapLeaf leaf)
	{
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0001DDCC File Offset: 0x0001BFCC
	protected void ReturnBranch(UVMapTree.UVMapBranch branch)
	{
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
	protected UVMapTree.UVMapBranch ExchangeLeaf(UVMapTree.UVMapLeaf leaf)
	{
		this.ReturnLeaf(leaf);
		return this.GetBranch();
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0001DDE0 File Offset: 0x0001BFE0
	protected UVMapTree.UVMapBranch GetBranch()
	{
		return new UVMapTree.UVMapBranch(this);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0001DDE8 File Offset: 0x0001BFE8
	protected UVMapTree.UVMapLeaf GetLeaf()
	{
		return new UVMapTree.UVMapLeaf();
	}

	// Token: 0x040004D6 RID: 1238
	protected Vector2[] NodeLayers;

	// Token: 0x040004D7 RID: 1239
	protected UVMapTree.UVMapNode RootNode;

	// Token: 0x040004D8 RID: 1240
	protected Vector3 LastTextureSize;

	// Token: 0x040004D9 RID: 1241
	protected Vector2[] UVAdjust;

	// Token: 0x040004DA RID: 1242
	protected Vector2 lastFoundUV;

	// Token: 0x020000C9 RID: 201
	public class UVMapBranch : UVMapTree.UVMapNode
	{
		// Token: 0x060007D4 RID: 2004 RVA: 0x0001DDF0 File Offset: 0x0001BFF0
		public UVMapBranch(UVMapTree tree)
		{
			this.Reset(tree);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001DE0C File Offset: 0x0001C00C
		public void Reset(UVMapTree tree)
		{
			for (int i = 0; i < this.Nodes.Length; i++)
			{
				if (this.Nodes[i] != null)
				{
					if (this.Nodes[i].IsLeaf())
					{
						tree.ReturnLeaf((UVMapTree.UVMapLeaf)this.Nodes[i]);
					}
					else
					{
						tree.ReturnBranch((UVMapTree.UVMapBranch)this.Nodes[i]);
					}
				}
				this.Nodes[i] = tree.GetLeaf();
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001DE8C File Offset: 0x0001C08C
		public override bool AddTexture(UVMapTree tree, int stepX, int stepY, int nDepth)
		{
			Debug.Log(string.Concat(new object[]
			{
				"AddTexture: Test: ",
				tree.LastTextureSize,
				" at depth ",
				tree.NodeLayers[nDepth].ToString()
			}));
			bool flag = nDepth + 1 >= tree.NodeLayers.Length;
			if (flag)
			{
				Debug.LogError("Invalid Texture Depth Reached: > " + tree.NodeLayers[tree.NodeLayers.Length - 1].ToString());
				return false;
			}
			if (this.CullNode(tree, nDepth + 1))
			{
				Debug.Log("Cull Branch");
				return false;
			}
			int num = nDepth + 1;
			int num2 = 0;
			int num3 = this.Nodes.Length;
			for (int i = 0; i < num3; i++)
			{
				UVMapTree.UVMapNode uvmapNode = this.Nodes[i];
				if (uvmapNode.Clip)
				{
					Debug.Log("Clip Leaf");
					num2++;
				}
				else
				{
					if (uvmapNode.IsLeaf())
					{
						Debug.Log(string.Concat(new object[]
						{
							"TestLeaf: ",
							i,
							" for: ",
							tree.LastTextureSize,
							" at: ",
							tree.NodeLayers[num]
						}));
						if (uvmapNode.IsBestFit(tree, num))
						{
							Debug.Log("Added Texture: " + tree.NodeLayers[num]);
							tree.lastFoundUV.x = (float)stepX + tree.NodeLayers[num].x * (float)((int)tree.UVAdjust[i].x);
							tree.lastFoundUV.y = (float)stepY + tree.NodeLayers[num].y * (float)((int)tree.UVAdjust[i].y);
							uvmapNode.Clip = true;
							return true;
						}
						Debug.Log("Add Branch: " + tree.NodeLayers[nDepth]);
						uvmapNode = tree.ExchangeLeaf((UVMapTree.UVMapLeaf)uvmapNode);
						this.Nodes[i] = uvmapNode;
					}
					if (uvmapNode.AddTexture(tree, (int)((float)stepX + tree.NodeLayers[num].x * (float)((int)tree.UVAdjust[i].x)), (int)((float)stepY + tree.NodeLayers[num].y * (float)((int)tree.UVAdjust[i].y)), num))
					{
						return true;
					}
				}
			}
			if (num2 >= num3)
			{
				Debug.LogWarning("Clip Branch");
				this.Clip = true;
				return false;
			}
			Debug.Log("Failed: " + tree.NodeLayers[nDepth]);
			return false;
		}

		// Token: 0x040004DB RID: 1243
		public UVMapTree.UVMapNode[] Nodes = new UVMapTree.UVMapNode[4];
	}

	// Token: 0x020000CA RID: 202
	public class UVMapLeaf : UVMapTree.UVMapNode
	{
		// Token: 0x060007D8 RID: 2008 RVA: 0x0001E184 File Offset: 0x0001C384
		public override bool IsLeaf()
		{
			return true;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001E188 File Offset: 0x0001C388
		public override bool IsBestFit(UVMapTree tree, int nodeSizeIndex)
		{
			return tree.LastTextureSize.x <= tree.NodeLayers[nodeSizeIndex].x && tree.LastTextureSize.y <= tree.NodeLayers[nodeSizeIndex].y && (tree.LastTextureSize.x > tree.NodeLayers[nodeSizeIndex].x * 0.5f || tree.LastTextureSize.y > tree.NodeLayers[nodeSizeIndex].y * 0.5f);
		}
	}

	// Token: 0x020000CB RID: 203
	public class UVMapNode
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x0001E228 File Offset: 0x0001C428
		protected UVMapNode()
		{
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001E230 File Offset: 0x0001C430
		public virtual bool IsLeaf()
		{
			return false;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001E234 File Offset: 0x0001C434
		public virtual bool IsBranch()
		{
			return !this.IsLeaf();
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001E240 File Offset: 0x0001C440
		public virtual bool CullNode(UVMapTree tree, int nodeSizeIndex)
		{
			return tree.LastTextureSize.x > tree.NodeLayers[nodeSizeIndex].x || tree.LastTextureSize.y > tree.NodeLayers[nodeSizeIndex].y;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001E290 File Offset: 0x0001C490
		public virtual bool IsBestFit(UVMapTree tree, int nodeSizeIndex)
		{
			return false;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001E294 File Offset: 0x0001C494
		public virtual bool AddTexture(UVMapTree tree, int stepX, int stepY, int nDepth)
		{
			return false;
		}

		// Token: 0x040004DC RID: 1244
		public bool Clip;
	}
}
