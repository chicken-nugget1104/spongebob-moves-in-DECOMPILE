using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class RewardDropManager
{
	// Token: 0x06000F75 RID: 3957 RVA: 0x00062F78 File Offset: 0x00061178
	public RewardDropManager()
	{
		this.spritePool = TFPool<BasicSprite>.CreatePool(10, new Alloc<BasicSprite>(RewardDropManager.MakeDrop));
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x00062FA0 File Offset: 0x000611A0
	private static BasicSprite MakeDrop()
	{
		Vector2 center = new Vector2(0f, 0f);
		BasicSprite basicSprite = new BasicSprite(null, "RecipeIcon.png", center, 16f, 16f, new QuadHitObject(center, 32f, 32f));
		basicSprite.PublicInitialize();
		basicSprite.Name = "RewardDrop_" + RewardDropManager.sDropId++;
		basicSprite.Visible = false;
		return basicSprite;
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x00063018 File Offset: 0x00061218
	public IDisplayController CreateDrop(float width, float height, string material, string texture)
	{
		bool allocated = false;
		BasicSprite basicSprite = this.spritePool.Create(delegate
		{
			allocated = true;
			Vector2 center = new Vector2(0f, 0f);
			BasicSprite basicSprite2 = new BasicSprite(material, texture, center, width, height, new QuadHitObject(center, width * 2f, height * 2f));
			basicSprite2.PublicInitialize();
			basicSprite2.Name = "RewardDrop_" + RewardDropManager.sDropId++;
			return basicSprite2;
		});
		if (!allocated)
		{
			basicSprite.Scale = Vector3.one;
			if (basicSprite.Width != width || basicSprite.Height != height)
			{
				basicSprite.Resize(Vector2.zero, width, height);
			}
			if (texture != null)
			{
				if (!basicSprite.MaterialName.Equals(texture))
				{
					basicSprite.UpdateMaterialOrTexture(texture);
				}
			}
			else if (!basicSprite.MaterialName.Equals(material))
			{
				basicSprite.UpdateMaterialOrTexture(material);
			}
		}
		basicSprite.Position = new Vector3(0f, 0f, -1f);
		basicSprite.Visible = true;
		return basicSprite;
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x0006312C File Offset: 0x0006132C
	public IDisplayController CreateDrop(Resource resource)
	{
		return this.CreateDrop(resource.Width, resource.Height, null, resource.GetResourceTexture());
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x00063154 File Offset: 0x00061354
	public IDisplayController CreateDrop(string texture)
	{
		return this.CreateDrop(16f, 16f, null, texture);
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x00063168 File Offset: 0x00061368
	public bool ReleaseDrop(IDisplayController drop)
	{
		bool result = false;
		BasicSprite basicSprite = drop as BasicSprite;
		if (basicSprite != null && this.spritePool.Release(basicSprite))
		{
			result = true;
			drop.Visible = false;
		}
		return result;
	}

	// Token: 0x04000A70 RID: 2672
	public const int defaultWidth = 16;

	// Token: 0x04000A71 RID: 2673
	public const int defaultHeight = 16;

	// Token: 0x04000A72 RID: 2674
	public const string recipeIcon = "RecipeIcon.png";

	// Token: 0x04000A73 RID: 2675
	public const string movieIcon = "MovieIcon.png";

	// Token: 0x04000A74 RID: 2676
	private const int START_POOL_SIZE = 10;

	// Token: 0x04000A75 RID: 2677
	private static int sDropId;

	// Token: 0x04000A76 RID: 2678
	private TFPool<BasicSprite> spritePool;
}
