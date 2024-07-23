using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class SBGUIModalDialog : SBGUIScreen
{
	// Token: 0x06000573 RID: 1395 RVA: 0x00023170 File Offset: 0x00021370
	protected override void Awake()
	{
		if (this.rewardWidgetPrefab == null)
		{
			this.rewardWidgetPrefab = (GameObject)Resources.Load("Prefabs/GUI/Widgets/RewardWidget");
		}
		this.rewardMarker = base.FindChild("reward_marker");
		base.Awake();
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x000231B0 File Offset: 0x000213B0
	public override void SetParent(SBGUIElement element)
	{
		this.parentElement = element;
		base.View.RefreshEvent += this.ZShuffle;
		base.SetParent(element);
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x000231D8 File Offset: 0x000213D8
	private void ZShuffle()
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		Vector3 zero = Vector3.zero;
		Bounds totalBounds = this.TotalBounds;
		if (totalBounds.min.z <= 0f)
		{
			zero.z = Mathf.Abs(totalBounds.min.z) + 1f;
			totalBounds.center += zero;
			base.tform.localPosition += zero;
		}
		if (this.parentElement == null)
		{
			return;
		}
		Bounds totalBounds2 = this.parentElement.TotalBounds;
		if (totalBounds2.min.z <= totalBounds.max.z)
		{
			zero.z = totalBounds.max.z - totalBounds2.min.z + 1f;
			this.parentElement.tform.localPosition += zero;
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00023304 File Offset: 0x00021504
	public override void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00023314 File Offset: 0x00021514
	public virtual void AddItem(string texture, int amount, string prefix)
	{
		if (amount == 0)
		{
			Debug.LogWarning("rewarding 0 of :" + texture);
			return;
		}
		if (texture == null || string.IsNullOrEmpty(texture.Trim()))
		{
			Debug.LogWarning("resource has no texture");
			return;
		}
		SBGUIRewardWidget item = SBGUIRewardWidget.Create(this.rewardWidgetPrefab, this.rewardMarker, this.markerXOffset, texture, amount, prefix);
		this.rewards.Add(item);
		this.markerXOffset = 0f;
		float num = 1f;
		float y = 0f;
		if (this.rewards.Count > 3)
		{
			num = 0.5f;
			y = 0.1f;
		}
		foreach (SBGUIRewardWidget sbguirewardWidget in this.rewards)
		{
			sbguirewardWidget.transform.localScale = new Vector3(num, num, num);
			sbguirewardWidget.transform.localPosition = new Vector3(this.markerXOffset, y, 0f);
			this.markerXOffset += (float)(sbguirewardWidget.Width + 10) * num * 0.01f;
		}
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00023458 File Offset: 0x00021658
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

	// Token: 0x06000579 RID: 1401 RVA: 0x000234E4 File Offset: 0x000216E4
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

	// Token: 0x0600057A RID: 1402 RVA: 0x00023584 File Offset: 0x00021784
	public void SetRewardIcons(Session session, List<Reward> rewards, string prefix)
	{
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
			this.AddItem(session.TheGame.resourceManager.Resources[key].GetResourceTexture(), value, prefix);
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in dictionary2)
		{
			int value2 = keyValuePair2.Value;
			Blueprint blueprint = EntityManager.GetBlueprint("building", keyValuePair2.Key, false);
			this.AddItem((string)blueprint.Invariable["portrait"], value2, prefix);
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00023720 File Offset: 0x00021920
	public void CenterRewards()
	{
		Vector3 position = this.rewardMarker.tform.position;
		Vector3 vector = this.rewardMarker.TotalBounds.center - position;
		Vector3 localPosition = this.rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		this.rewardMarker.tform.localPosition = localPosition;
	}

	// Token: 0x04000438 RID: 1080
	private const int REWARD_GAP_SIZE = 10;

	// Token: 0x04000439 RID: 1081
	public GameObject rewardWidgetPrefab;

	// Token: 0x0400043A RID: 1082
	private float markerXOffset;

	// Token: 0x0400043B RID: 1083
	protected SBGUIElement rewardMarker;

	// Token: 0x0400043C RID: 1084
	private SBGUIElement parentElement;

	// Token: 0x0400043D RID: 1085
	private List<SBGUIRewardWidget> rewards = new List<SBGUIRewardWidget>();
}
