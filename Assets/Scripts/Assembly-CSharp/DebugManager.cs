using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class DebugManager
{
	// Token: 0x06000BED RID: 3053 RVA: 0x00048338 File Offset: 0x00046538
	public DebugManager(Session session)
	{
		this.session = session;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00048348 File Offset: 0x00046548
	public void ToggleDebugPlaceObjects(Session session)
	{
		this.debugPlaceObjects = !this.debugPlaceObjects;
		Simulation simulation = session.TheGame.simulation;
		foreach (Simulated simulated in simulation.GetSimulateds())
		{
			if (simulated.entity is DebrisEntity)
			{
				simulated.InteractionState.ClearControls();
				if (!this.debugPlaceObjects)
				{
					simulated.InteractionState.PushControls(new List<IControlBinding>
					{
						new Session.ClearDebrisControl(simulated)
					});
				}
			}
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00048408 File Offset: 0x00046608
	public void ToggleFramerateCounter(Session session)
	{
		GameObject gameObject = (GameObject)session.CheckAsyncRequest("hudfps");
		if (gameObject == null)
		{
			gameObject = UnityGameResources.CreateEmpty("hudfps");
			gameObject.AddComponent<HUDFPS>();
			session.AddAsyncResponse("hudfps", gameObject);
			this.framerateCounter = true;
		}
		else
		{
			UnityGameResources.Destroy(gameObject);
			this.framerateCounter = false;
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x0004846C File Offset: 0x0004666C
	public void ToggleHitBoxes(Simulation simulation)
	{
		this.showHitBoxes = !this.showHitBoxes;
		simulation.UpdateDebugHitBoxes();
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00048484 File Offset: 0x00046684
	public void ToggleFootprints(Simulation simulation)
	{
		this.showFootprints = !this.showFootprints;
		simulation.UpdateDebugFootprints();
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0004849C File Offset: 0x0004669C
	public void ToggleRMT()
	{
		this.session.TheGame.store.rmtEnabled = !this.session.TheGame.store.rmtEnabled;
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x000484CC File Offset: 0x000466CC
	public void ToggleExpansionBorders(Simulation simulation)
	{
		this.showExpansionBorders = !this.showExpansionBorders;
		simulation.UpdateDebugExpansionBorders();
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x000484E4 File Offset: 0x000466E4
	public void DeleteGameData()
	{
		this.session.StopGameSaveTimer();
		this.session.WebFileServer.DeleteGameData(this.session);
		if (Directory.Exists(this.session.ThePlayer.CacheDir()))
		{
			Directory.Delete(this.session.ThePlayer.CacheDir(), true);
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00048544 File Offset: 0x00046744
	public void ResetAchievements()
	{
		GameObject gameObject = GameObject.Find("SBGameCenterManager");
		SBGameCenterManager component = gameObject.GetComponent<SBGameCenterManager>();
		component.ResetAchievements();
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0004856C File Offset: 0x0004676C
	public void ToggleFreeCameraMode(SBCamera camera)
	{
		this.freeCameraMode = !this.freeCameraMode;
		camera.freeCameraMode = this.freeCameraMode;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x0004858C File Offset: 0x0004678C
	public void CompleteAllQuests()
	{
		this.session.TheGame.questManager.DebugCompleteAllQuests(this.session.TheGame);
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x000485BC File Offset: 0x000467BC
	public void ResetDeviceID()
	{
	}

	// Token: 0x04000801 RID: 2049
	private const string FRAMERATE_COUNTER = "hudfps";

	// Token: 0x04000802 RID: 2050
	public bool debugPlaceObjects;

	// Token: 0x04000803 RID: 2051
	public bool framerateCounter;

	// Token: 0x04000804 RID: 2052
	public bool showHitBoxes;

	// Token: 0x04000805 RID: 2053
	public bool showFootprints;

	// Token: 0x04000806 RID: 2054
	public bool showExpansionBorders;

	// Token: 0x04000807 RID: 2055
	public bool freeCameraMode;

	// Token: 0x04000808 RID: 2056
	private Session session;
}
