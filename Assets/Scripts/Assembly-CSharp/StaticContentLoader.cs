using System;

// Token: 0x02000457 RID: 1111
public class StaticContentLoader
{
	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x0600223E RID: 8766 RVA: 0x000D31EC File Offset: 0x000D13EC
	public TaskManager TheTaskManager
	{
		get
		{
			return this._taskManager;
		}
	}

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x0600223F RID: 8767 RVA: 0x000D31F4 File Offset: 0x000D13F4
	public MicroEventManager TheMicroEventManager
	{
		get
		{
			return this._microEventManager;
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06002240 RID: 8768 RVA: 0x000D31FC File Offset: 0x000D13FC
	public CostumeManager TheCostumeManager
	{
		get
		{
			return this._costumeManager;
		}
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x06002241 RID: 8769 RVA: 0x000D3204 File Offset: 0x000D1404
	public WishTableManager TheWishTableManager
	{
		get
		{
			return this._wishTableManager;
		}
	}

	// Token: 0x17000511 RID: 1297
	// (get) Token: 0x06002242 RID: 8770 RVA: 0x000D320C File Offset: 0x000D140C
	public CraftingManager TheCraftingManager
	{
		get
		{
			return this._craftManager;
		}
	}

	// Token: 0x17000512 RID: 1298
	// (get) Token: 0x06002243 RID: 8771 RVA: 0x000D3214 File Offset: 0x000D1414
	public VendingManager TheVendingManager
	{
		get
		{
			return this._vendingManager;
		}
	}

	// Token: 0x17000513 RID: 1299
	// (get) Token: 0x06002244 RID: 8772 RVA: 0x000D321C File Offset: 0x000D141C
	public TreasureManager TheTreasureManager
	{
		get
		{
			return this._treasureManager;
		}
	}

	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x06002245 RID: 8773 RVA: 0x000D3224 File Offset: 0x000D1424
	public CommunityEventManager TheCommunityEventManager
	{
		get
		{
			return this._communityEventManager;
		}
	}

	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06002246 RID: 8774 RVA: 0x000D322C File Offset: 0x000D142C
	public PaytableManager ThePaytableManager
	{
		get
		{
			return this._paytableManager;
		}
	}

	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06002247 RID: 8775 RVA: 0x000D3234 File Offset: 0x000D1434
	public MovieManager TheMovieManager
	{
		get
		{
			return this._movieManager;
		}
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06002248 RID: 8776 RVA: 0x000D323C File Offset: 0x000D143C
	public Terrain TheTerrain
	{
		get
		{
			return this._terrain;
		}
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06002249 RID: 8777 RVA: 0x000D3244 File Offset: 0x000D1444
	public Border TheBorder
	{
		get
		{
			return this._border;
		}
	}

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x0600224A RID: 8778 RVA: 0x000D324C File Offset: 0x000D144C
	public ResourceManager TheResourceManager
	{
		get
		{
			return this._resourceManager;
		}
	}

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x0600224B RID: 8779 RVA: 0x000D3254 File Offset: 0x000D1454
	public Catalog TheCatalog
	{
		get
		{
			return this._catalog;
		}
	}

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x0600224C RID: 8780 RVA: 0x000D325C File Offset: 0x000D145C
	public EntityManager TheEntityManager
	{
		get
		{
			return this._entities;
		}
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x0600224D RID: 8781 RVA: 0x000D3264 File Offset: 0x000D1464
	public QuestManager TheQuestManager
	{
		get
		{
			return this._questManager;
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x0600224E RID: 8782 RVA: 0x000D326C File Offset: 0x000D146C
	public LevelingManager TheLevelingManager
	{
		get
		{
			return this._levelingManager;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x0600224F RID: 8783 RVA: 0x000D3274 File Offset: 0x000D1474
	public FeatureManager TheFeatureManager
	{
		get
		{
			return this._featureManager;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06002250 RID: 8784 RVA: 0x000D327C File Offset: 0x000D147C
	public BuildingUnlockManager TheBuildingUnlockManager
	{
		get
		{
			return this._buildingUnlockManager;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06002251 RID: 8785 RVA: 0x000D3284 File Offset: 0x000D1484
	public EnclosureManager TheEnclosureManager
	{
		get
		{
			return this._enclosureManager;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06002252 RID: 8786 RVA: 0x000D328C File Offset: 0x000D148C
	public AutoQuestDatabase TheAutoQuestDatabase
	{
		get
		{
			return this._autoQuestDatabase;
		}
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x000D3294 File Offset: 0x000D1494
	public void LoadContent(Session session)
	{
		DatabaseManager.Instance.LoadDatabaseFromCSV("Database_LookUp.csv");
		this._resourceManager = new ResourceManager(session);
		this._craftManager = new CraftingManager();
		this._vendingManager = new VendingManager();
		this._treasureManager = new TreasureManager(session);
		this._paytableManager = new PaytableManager();
		this._featureManager = new FeatureManager();
		this._buildingUnlockManager = new BuildingUnlockManager();
		this._movieManager = new MovieManager();
		this._terrain = new Terrain(0);
		this._border = new Border();
		this._levelingManager = new LevelingManager();
		this._catalog = new Catalog();
		this._autoQuestDatabase = new AutoQuestDatabase();
		this._questManager = new QuestManager();
		this._entities = new EntityManager(session.InFriendsGame);
		this._enclosureManager = new EnclosureManager();
		this._communityEventManager = new CommunityEventManager(session);
		this._taskManager = new TaskManager();
		this._microEventManager = new MicroEventManager();
		this._costumeManager = new CostumeManager();
		this._wishTableManager = new WishTableManager();
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000D33A4 File Offset: 0x000D15A4
	public void Initialize()
	{
		this._terrain.Initialize();
		this._border.Initialize(this._terrain);
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000D33C4 File Offset: 0x000D15C4
	public bool LoadNextBlueprint()
	{
		return this._entities.IterateLoadOfBlueprints();
	}

	// Token: 0x04001529 RID: 5417
	private CraftingManager _craftManager;

	// Token: 0x0400152A RID: 5418
	private VendingManager _vendingManager;

	// Token: 0x0400152B RID: 5419
	private TreasureManager _treasureManager;

	// Token: 0x0400152C RID: 5420
	private PaytableManager _paytableManager;

	// Token: 0x0400152D RID: 5421
	private MovieManager _movieManager;

	// Token: 0x0400152E RID: 5422
	private Terrain _terrain;

	// Token: 0x0400152F RID: 5423
	private Border _border;

	// Token: 0x04001530 RID: 5424
	private ResourceManager _resourceManager;

	// Token: 0x04001531 RID: 5425
	private Catalog _catalog;

	// Token: 0x04001532 RID: 5426
	private LevelingManager _levelingManager;

	// Token: 0x04001533 RID: 5427
	private FeatureManager _featureManager;

	// Token: 0x04001534 RID: 5428
	private BuildingUnlockManager _buildingUnlockManager;

	// Token: 0x04001535 RID: 5429
	private EnclosureManager _enclosureManager;

	// Token: 0x04001536 RID: 5430
	private CommunityEventManager _communityEventManager;

	// Token: 0x04001537 RID: 5431
	private TaskManager _taskManager;

	// Token: 0x04001538 RID: 5432
	private MicroEventManager _microEventManager;

	// Token: 0x04001539 RID: 5433
	private CostumeManager _costumeManager;

	// Token: 0x0400153A RID: 5434
	private WishTableManager _wishTableManager;

	// Token: 0x0400153B RID: 5435
	private EntityManager _entities;

	// Token: 0x0400153C RID: 5436
	private QuestManager _questManager;

	// Token: 0x0400153D RID: 5437
	private AutoQuestDatabase _autoQuestDatabase;
}
