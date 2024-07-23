using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class AmazonGameCircleExampleInitialization : AmazonGameCircleExampleBase
{
	// Token: 0x060001A9 RID: 425 RVA: 0x00008484 File Offset: 0x00006684
	public AmazonGameCircleExampleInitialization()
	{
		this.toastLocations = Enum.GetNames(typeof(GameCirclePopupLocation));
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060001AA RID: 426 RVA: 0x000084FC File Offset: 0x000066FC
	public AmazonGameCircleExampleInitialization.EInitializationStatus InitializationStatus
	{
		get
		{
			return this.initializationStatus;
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00008504 File Offset: 0x00006704
	public override string MenuTitle()
	{
		return "Initialization";
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000850C File Offset: 0x0000670C
	public override void DrawMenu()
	{
		switch (this.InitializationStatus)
		{
		case AmazonGameCircleExampleInitialization.EInitializationStatus.Uninitialized:
			this.DisplayInitGameCircleMenu();
			break;
		case AmazonGameCircleExampleInitialization.EInitializationStatus.InitializationRequested:
			AmazonGameCircleExampleGUIHelpers.BoxedCenteredLabel("Amazon GameCircle");
			this.DisplayLoadingGameCircleMenu();
			break;
		case AmazonGameCircleExampleInitialization.EInitializationStatus.Unavailable:
			this.DisplayGameCircleUnavailableMenu();
			break;
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000856C File Offset: 0x0000676C
	private void DisplayInitGameCircleMenu()
	{
		if (GUILayout.Button(string.Format(this.pluginInitializationButton, "Amazon GameCircle"), new GUILayoutOption[0]))
		{
			this.InitializeGameCircle();
		}
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		this.usesLeaderboards = GUILayout.Toggle(this.usesLeaderboards, "Use Leaderboards", new GUILayoutOption[0]);
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		this.usesAchievements = GUILayout.Toggle(this.usesAchievements, "Use Achievements", new GUILayoutOption[0]);
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		this.usesWhispersync = GUILayout.Toggle(this.usesWhispersync, "Use Whispersync", new GUILayoutOption[0]);
		AmazonGameCircleExampleGUIHelpers.AnchoredLabel("Popup Location", TextAnchor.LowerCenter, new GUILayoutOption[0]);
		if (this.toastLocations != null)
		{
			this.toastLocation = (GameCirclePopupLocation)GUILayout.SelectionGrid((int)this.toastLocation, this.toastLocations, 3, new GUILayoutOption[0]);
		}
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		if (GUILayout.Button((!this.enablePopups) ? "Popups Disabled" : "Popups Enabled", new GUILayoutOption[0]))
		{
			this.enablePopups = !this.enablePopups;
		}
		GUILayout.EndVertical();
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
	}

	// Token: 0x060001AE RID: 430 RVA: 0x000086F0 File Offset: 0x000068F0
	private void DisplayLoadingGameCircleMenu()
	{
		if (!string.IsNullOrEmpty(this.gameCircleInitializationStatusLabel))
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.gameCircleInitializationStatusLabel, new GUILayoutOption[0]);
		}
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("{0,5:N1} seconds", (DateTime.Now - this.initRequestTime).TotalSeconds), new GUILayoutOption[0]);
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00008750 File Offset: 0x00006950
	private void DisplayGameCircleUnavailableMenu()
	{
		if (!string.IsNullOrEmpty(this.gameCircleInitializationStatusLabel))
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.gameCircleInitializationStatusLabel, new GUILayoutOption[0]);
		}
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00008774 File Offset: 0x00006974
	private void InitializeGameCircle()
	{
		this.initializationStatus = AmazonGameCircleExampleInitialization.EInitializationStatus.InitializationRequested;
		this.SubscribeToGameCircleInitializationEvents();
		this.initRequestTime = DateTime.Now;
		AGSClient.Init(this.usesLeaderboards, this.usesAchievements, this.usesWhispersync);
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x000087A8 File Offset: 0x000069A8
	private void SubscribeToGameCircleInitializationEvents()
	{
		AGSClient.ServiceReadyEvent += this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += this.ServiceNotReadyHandler;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x000087D8 File Offset: 0x000069D8
	private void UnsubscribeFromGameCircleInitializationEvents()
	{
		AGSClient.ServiceReadyEvent -= this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= this.ServiceNotReadyHandler;
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00008808 File Offset: 0x00006A08
	private void ServiceNotReadyHandler(string error)
	{
		this.initializationStatus = AmazonGameCircleExampleInitialization.EInitializationStatus.Unavailable;
		this.gameCircleInitializationStatusLabel = string.Format("Failed to initialize: {0}", error);
		this.UnsubscribeFromGameCircleInitializationEvents();
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00008828 File Offset: 0x00006A28
	private void ServiceReadyHandler()
	{
		this.initializationStatus = AmazonGameCircleExampleInitialization.EInitializationStatus.Ready;
		this.gameCircleInitializationStatusLabel = this.pluginInitializedLabel;
		this.UnsubscribeFromGameCircleInitializationEvents();
		AGSClient.SetPopUpEnabled(this.enablePopups);
		AGSClient.SetPopUpLocation(this.toastLocation);
	}

	// Token: 0x040000AA RID: 170
	private const string pluginName = "Amazon GameCircle";

	// Token: 0x040000AB RID: 171
	private const string initializationmenuTitle = "Initialization";

	// Token: 0x040000AC RID: 172
	private const string usesLeaderboardsLabel = "Use Leaderboards";

	// Token: 0x040000AD RID: 173
	private const string usesAchievementsLabel = "Use Achievements";

	// Token: 0x040000AE RID: 174
	private const string usesWhispersyncLabel = "Use Whispersync";

	// Token: 0x040000AF RID: 175
	private const string toastLocationLabel = "Popup Location";

	// Token: 0x040000B0 RID: 176
	private const string popupsDisabledLabel = "Popups Disabled";

	// Token: 0x040000B1 RID: 177
	private const string popupsEnabledLabel = "Popups Enabled";

	// Token: 0x040000B2 RID: 178
	private const string pluginFailedToInitializeLabel = "Failed to initialize: {0}";

	// Token: 0x040000B3 RID: 179
	private const string loadingTimeLabel = "{0,5:N1} seconds";

	// Token: 0x040000B4 RID: 180
	private AmazonGameCircleExampleInitialization.EInitializationStatus initializationStatus;

	// Token: 0x040000B5 RID: 181
	private DateTime initRequestTime;

	// Token: 0x040000B6 RID: 182
	private bool usesLeaderboards = true;

	// Token: 0x040000B7 RID: 183
	private bool usesAchievements = true;

	// Token: 0x040000B8 RID: 184
	private bool usesWhispersync = true;

	// Token: 0x040000B9 RID: 185
	private GameCirclePopupLocation toastLocation = GameCirclePopupLocation.BOTTOM_CENTER;

	// Token: 0x040000BA RID: 186
	private string[] toastLocations;

	// Token: 0x040000BB RID: 187
	private bool enablePopups = true;

	// Token: 0x040000BC RID: 188
	private string gameCircleInitializationStatusLabel;

	// Token: 0x040000BD RID: 189
	private readonly string pluginInitializationButton = string.Format("Initialize {0}", "Amazon GameCircle");

	// Token: 0x040000BE RID: 190
	private readonly string pluginInitializedLabel = string.Format("{0} is ready for use.", "Amazon GameCircle");

	// Token: 0x02000031 RID: 49
	public enum EInitializationStatus
	{
		// Token: 0x040000C0 RID: 192
		Uninitialized,
		// Token: 0x040000C1 RID: 193
		InitializationRequested,
		// Token: 0x040000C2 RID: 194
		Ready,
		// Token: 0x040000C3 RID: 195
		Unavailable
	}
}
