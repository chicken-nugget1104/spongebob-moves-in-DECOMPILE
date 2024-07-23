using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class SBGUILevelUpScreen : SBGUIScreen
{
	// Token: 0x06000536 RID: 1334 RVA: 0x00020D40 File Offset: 0x0001EF40
	public void Setup(Session session, LevelUpDialogInputData inputData)
	{
		this.spinningPaper = (SBGUIImage)base.FindChild("spinning_paper");
		this.windows = (SBGUIImage)base.FindChild("windows");
		this.spinningPaper.SetTextureFromTexturePath(TextureLibrarian.PathLookUp("Textures/GUI/LevelUpScreen/levelup_spinning"));
		this.windows.SetTextureFromTexturePath(TextureLibrarian.PathLookUp("Textures/GUI/LevelUpScreen/levelup_bg"));
		this.SetLevelText(session, inputData.NewLevel);
		this.SetLevelImage(session, inputData.NewLevel);
		this.SetLevelVoice(session, inputData.NewLevel);
		this.SetRewardIcons(session, inputData.Rewards);
		this.spinningAudio = this.soundEffectMgr.PlaySound("JellyfishNet");
		if (this.spinningAudio != null)
		{
			this.spinningAudio.Stop();
			this.spinningAudio.loop = true;
			this.spinningAudio.pitch = 1.4f;
			this.spinningAudio.volume = 0.3f;
			this.spinningAudio.Play();
		}
		int num = this.resourceMgr.Query(ResourceManager.LEVEL);
		foreach (Blueprint blueprint in this.entityMgr.Blueprints.Values)
		{
			if (blueprint.Invariable.ContainsKey("level.minimum"))
			{
				int num2 = (int)blueprint.Invariable["level.minimum"];
				if (num2 == num)
				{
					this.unlockedItems.Add(blueprint);
					bool flag = (bool)blueprint.Invariable["has_move_in"];
					if (flag && blueprint.Invariable.ContainsKey("resident") && blueprint.Invariable["resident"] != null)
					{
						int did = (int)blueprint.Invariable["resident"];
						Blueprint blueprint2 = EntityManager.GetBlueprint(EntityType.RESIDENT, did, false);
						string characterName = Language.Get((string)blueprint2.Invariable["name"]);
						string buildingName = Language.Get((string)blueprint.Invariable["name"]);
						string portraitTexture = (string)blueprint.Invariable["portrait"];
						MoveInDialogInputData item = new MoveInDialogInputData(characterName, buildingName, portraitTexture, "Beat_MoveIn");
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
						{
							item
						}, uint.MaxValue);
					}
				}
			}
		}
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00020FE4 File Offset: 0x0001F1E4
	public void CreateUI(Session session, LevelUpDialogInputData inputData)
	{
		this.ShowUnlockedBlueprints();
		foreach (object obj in this.windows.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name != "unlocked_marker")
			{
				transform.gameObject.SetActive(false);
			}
		}
		foreach (object obj2 in this.windows.transform.FindChild("unlocked_marker"))
		{
			Transform transform2 = (Transform)obj2;
			transform2.renderer.enabled = false;
		}
		this.spinningPaper.SetActive(true);
		base.StartCoroutine(this.AnimateSpinIn(1f));
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00021110 File Offset: 0x0001F310
	public void ShowUnlockedBlueprints()
	{
		Vector3 vector = Vector3.zero;
		int num = Math.Min(this.unlockedItems.Count - 1, 2);
		this.unlocked_marker = base.FindChild("unlocked_marker");
		for (int i = 0; i <= num; i++)
		{
			string iconTexture = (string)this.unlockedItems[i].Invariable["portrait"];
			SBGUILevelUpSlot sbguilevelUpSlot = SBGUILevelUpSlot.Create(this.session, this, this.unlocked_marker, Vector3.zero + vector, iconTexture);
			float x = sbguilevelUpSlot.FindChild("slot_background").GetComponent<YGSprite>().size.x;
			if (i != num)
			{
				vector += new Vector3((x + 5f) * 0.01f, 0.01f, 0f);
			}
		}
		this.CenterBlueprints(vector);
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x000211EC File Offset: 0x0001F3EC
	public void SetManagers(EntityManager emgr, ResourceManager resMgr, SoundEffectManager sfxMgr)
	{
		this.entityMgr = emgr;
		this.resourceMgr = resMgr;
		this.soundEffectMgr = sfxMgr;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00021204 File Offset: 0x0001F404
	private void SetLevelText(Session session, int level)
	{
		SBGUIShadowedLabel sbguishadowedLabel = (SBGUIShadowedLabel)base.FindChild("level_label");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("population_label");
		SBGUIShadowedLabel component = base.FindChild("reward_label").GetComponent<SBGUIShadowedLabel>();
		SBGUIShadowedLabel component2 = base.FindChild("levelup_ribbon_label").GetComponent<SBGUIShadowedLabel>();
		SBGUILabel component3 = base.FindChild("headline_label").GetComponent<SBGUILabel>();
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)base.FindChild("headline_label_boundary");
		SBGUILabel component4 = base.FindChild("levelheadline2_label").GetComponent<SBGUILabel>();
		SBGUILabel component5 = base.FindChild("unlocked_label").GetComponent<SBGUILabel>();
		sbguishadowedLabel.Text = Language.Get("!!PREFAB_LEVEL").ToUpper() + " " + level.ToString() + "!";
		component.Text = Language.Get("!!PREFAB_REWARD");
		component2.Text = Language.Get("!!PREFAB_LEVELUP").ToUpper();
		component3.SetText(Language.Get(session.TheGame.levelingManager.Headline(level)));
		component3.AdjustText(boundary);
		component4.SetText(Language.Get("!!PREFAB_IS").ToUpper());
		component5.SetText(Language.Get("!!PREFAB_NOW_AVAILABLE").ToUpper());
		int residentPopulation = session.TheGame.GetResidentPopulation();
		sbguilabel.SetText(Language.Get("!!PREFAB_POPULATION") + " " + residentPopulation.ToString().PadLeft(5, '0'));
		int length = component4.Text.Length;
		float num = 0.225f;
		sbguishadowedLabel.transform.Translate(new Vector3(num * (float)length, 0f, 0f), Space.Self);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x000213B0 File Offset: 0x0001F5B0
	private void SetLevelImage(Session session, int level)
	{
		SBGUIImage sbguiimage = (SBGUIImage)base.FindChild("headline_image");
		string textureFromTexturePath = TextureLibrarian.PathLookUp(session.TheGame.levelingManager.HeadlineImage(level));
		sbguiimage.SetTextureFromTexturePath(textureFromTexturePath);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x000213EC File Offset: 0x0001F5EC
	private void SetLevelVoice(Session session, int level)
	{
		float delaySeconds = 1f;
		this.soundEffectMgr.PlaySound(session.TheGame.levelingManager.VoiceOver(level), delaySeconds);
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00021420 File Offset: 0x0001F620
	public override void Deactivate()
	{
		this.unlockedItems.Clear();
		base.Deactivate();
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00021434 File Offset: 0x0001F634
	private void AddItem(string texture, int amount)
	{
		if (amount == 0)
		{
			Debug.LogWarning("rewarding 0 of :" + texture);
			return;
		}
		if (this.rewards.Count >= 4)
		{
			Debug.LogWarning("only showing first 4 rewards");
			return;
		}
		if (texture == null || string.IsNullOrEmpty(texture.Trim()))
		{
			Debug.LogWarning("resource has no material");
			return;
		}
		SBGUIRewardWidget sbguirewardWidget = SBGUIRewardWidget.Create(this.rewardWidgetPrefab, this.rewardMarker, this.markerXOffset, texture, amount, string.Empty);
		sbguirewardWidget.tform.Rotate(0f, 0f, 8.94f);
		sbguirewardWidget.CreateTextStroke(Color.white);
		sbguirewardWidget.SetTextColor(Color.black);
		this.markerXOffset += (float)(sbguirewardWidget.Width + 5) * 0.01f;
		this.rewards.Add(sbguirewardWidget);
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x0002150C File Offset: 0x0001F70C
	private void ClearItems()
	{
		this.markerXOffset = 0f;
		foreach (SBGUIRewardWidget sbguirewardWidget in this.rewards)
		{
			sbguirewardWidget.gameObject.SetActiveRecursively(false);
			UnityEngine.Object.Destroy(sbguirewardWidget.gameObject);
		}
		this.rewards.Clear();
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00021598 File Offset: 0x0001F798
	private void InitializeRewardComponentAmounts(Reward reward, Dictionary<int, int> componentAmounts, Dictionary<int, int> outAmounts)
	{
		outAmounts.Clear();
		foreach (KeyValuePair<int, int> keyValuePair in componentAmounts)
		{
			int key = keyValuePair.Key;
			int value = keyValuePair.Value;
			if (!outAmounts.ContainsKey(key))
			{
				outAmounts[key] = 0;
			}
			int num;
			int key2 = num = key;
			num = outAmounts[num];
			outAmounts[key2] = num + value;
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00021638 File Offset: 0x0001F838
	private void SetRewardIcons(Session session, List<Reward> rewards)
	{
		this.rewardMarker = base.FindChild("reward_marker");
		this.rewardCenter = this.rewardMarker.tform.position;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		this.ClearItems();
		foreach (Reward reward in rewards)
		{
			if (reward != null)
			{
				this.InitializeRewardComponentAmounts(reward, reward.ResourceAmounts, dictionary);
				this.InitializeRewardComponentAmounts(reward, reward.BuildingAmounts, dictionary2);
			}
		}
		foreach (KeyValuePair<int, int> keyValuePair in dictionary)
		{
			int key = keyValuePair.Key;
			int value = keyValuePair.Value;
			this.AddItem(session.TheGame.resourceManager.Resources[key].GetResourceTexture(), value);
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in dictionary2)
		{
			int key2 = keyValuePair2.Key;
			int value2 = keyValuePair2.Value;
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, key2, false);
			IDisplayController displayController = (IDisplayController)blueprint.Invariable["display"];
			this.AddItem(displayController.MaterialName, value2);
		}
		this.CenterRewards();
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00021808 File Offset: 0x0001FA08
	private void CenterBlueprints(Vector3 offset)
	{
		if (this.unlocked_marker == null)
		{
			return;
		}
		this.unlocked_marker.transform.Translate(-offset.x / 2f, 0f, 0f, Space.Self);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00021850 File Offset: 0x0001FA50
	private void CenterRewards()
	{
		if (this.rewardMarker == null)
		{
			return;
		}
		Vector3 vector = this.rewardMarker.TotalBounds.center - this.rewardCenter;
		Vector3 localPosition = this.rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		this.rewardMarker.tform.localPosition = localPosition;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x000218C8 File Offset: 0x0001FAC8
	private IEnumerator AnimateSpinIn(float duration)
	{
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			base.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			base.transform.RotateAround(new Vector3(0f, 0f, 1f), -20f * Time.deltaTime);
			yield return null;
		}
		foreach (object obj in this.windows.transform)
		{
			Transform child = (Transform)obj;
			if (child.name != "unlocked_marker")
			{
				child.gameObject.SetActive(true);
			}
		}
		foreach (object obj2 in this.windows.transform.FindChild("unlocked_marker"))
		{
			Transform child2 = (Transform)obj2;
			child2.renderer.enabled = true;
		}
		this.spinningPaper.SetActive(false);
		base.transform.localRotation = Quaternion.identity;
		if (this.spinningAudio != null)
		{
			this.spinningAudio.loop = false;
			this.spinningAudio.pitch = 1f;
			this.spinningAudio.volume = 1f;
			this.spinningAudio.Stop();
			Debug.LogError("spinningAudio killed normally");
		}
		yield break;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x000218F4 File Offset: 0x0001FAF4
	private new void OnDestroy()
	{
		Debug.LogError("OnDestroy is called to kill spinningAudio, spinningAudio.loop was " + this.spinningAudio.loop);
		if (this.spinningAudio != null)
		{
			this.spinningAudio.loop = false;
			this.spinningAudio.pitch = 1f;
			this.spinningAudio.volume = 1f;
			this.spinningAudio.Stop();
		}
	}

	// Token: 0x040003EE RID: 1006
	private const int REWARD_GAP_SIZE = 5;

	// Token: 0x040003EF RID: 1007
	private const float BLUEPRINT_GAP_SIZE = 5f;

	// Token: 0x040003F0 RID: 1008
	public GameObject rewardWidgetPrefab;

	// Token: 0x040003F1 RID: 1009
	private List<SBGUIRewardWidget> rewards = new List<SBGUIRewardWidget>();

	// Token: 0x040003F2 RID: 1010
	private float markerXOffset;

	// Token: 0x040003F3 RID: 1011
	private SBGUIElement rewardMarker;

	// Token: 0x040003F4 RID: 1012
	private SBGUILabel unlocked_count;

	// Token: 0x040003F5 RID: 1013
	private SBGUIElement unlocked_marker;

	// Token: 0x040003F6 RID: 1014
	private Vector3 rewardCenter;

	// Token: 0x040003F7 RID: 1015
	private SBGUIImage windows;

	// Token: 0x040003F8 RID: 1016
	private SBGUIImage spinningPaper;

	// Token: 0x040003F9 RID: 1017
	private AudioSource spinningAudio;

	// Token: 0x040003FA RID: 1018
	public GameObject slotPrefab;

	// Token: 0x040003FB RID: 1019
	private List<Blueprint> unlockedItems = new List<Blueprint>();

	// Token: 0x040003FC RID: 1020
	protected EntityManager entityMgr;

	// Token: 0x040003FD RID: 1021
	protected ResourceManager resourceMgr;

	// Token: 0x040003FE RID: 1022
	protected SoundEffectManager soundEffectMgr;
}
