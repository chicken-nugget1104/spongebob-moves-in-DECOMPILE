using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class SBGUITimebar : SBGUIElement
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002AFA8 File Offset: 0x000291A8
	public SBGUIButton RushButton
	{
		get
		{
			return this.rushButton;
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0002AFB0 File Offset: 0x000291B0
	protected override void Awake()
	{
		this.dict = base.CacheChildren();
		this.meter = (SBGUIProgressMeter)this.dict["progress_meter"];
		this.durationLabel = (SBGUILabel)this.dict["duration_label"];
		this.m_pTaskCharacterList = (SBGUICharacterArrowList)base.FindChild("character_portrait_parent");
		base.Awake();
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0002B01C File Offset: 0x0002921C
	public void Setup(Session session, uint ownerDid, string description, ulong completeTime, ulong totalTime, float duration, Cost rushCost, Action onRush, SBGUITimebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		SBGUILabel sbguilabel = (SBGUILabel)this.dict["task_label"];
		sbguilabel.SetText(description);
		SBGUIElement sbguielement = this.dict["rush_button"];
		this.closeFinishedAction = onFinish;
		if (onRush == null)
		{
			sbguielement.SetActive(false);
			this.rushButton = null;
		}
		else
		{
			this.rushButton = sbguielement.GetComponent<SBGUIButton>();
			this.rushButton.ClearClickEvents();
			this.AttachAnalyticsToButton("rush", this.rushButton);
			this.rushButton.ClickEvent += onRush;
			if (this.originalRushButtonSessionActionId == null)
			{
				this.originalRushButtonSessionActionId = this.rushButton.SessionActionId;
			}
			this.rushButton.SessionActionId = SessionActionSimulationHelper.DecorateSessionActionId(ownerDid, this.originalRushButtonSessionActionId);
			this.rushLabel = (SBGUILabel)this.dict["rush_cost_label"];
			this.rushLabel.SetText(rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()].ToString());
			this.maxJellyCost = rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()];
		}
		if (this.m_pTaskCharacterList != null)
		{
			if (pTaskCharacterDIDs == null || pTaskCharacterDIDs.Count <= 0)
			{
				this.m_pTaskCharacterList.SetActive(false);
			}
			else
			{
				this.m_pTaskCharacterList.SetActive(true);
				List<SBGUIArrowList.ListItemData> list = new List<SBGUIArrowList.ListItemData>();
				List<int> list2 = new List<int>();
				int count = pTaskCharacterDIDs.Count;
				for (int i = 0; i < count; i++)
				{
					Simulated simulated = session.TheGame.simulation.FindSimulated(new int?(pTaskCharacterDIDs[i]));
					ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
					list.Add(new SBGUIArrowList.ListItemData(entity.DefinitionId, entity.QuestReminderIcon, false));
					List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(entity.DefinitionId, null, true);
					if (activeTasksForSimulated != null && activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].m_bMovingToTarget)
					{
						list2.Add(entity.DefinitionId);
					}
				}
				this.m_pTaskCharacterList.SetData(session, list, (list.Count <= 0) ? 0 : list[0].m_nID, list2, null, pTaskCharacterClicked);
			}
		}
		base.StartCoroutine(this.ScaleCoroutine(0.1f, 1f, 0.5f, new SBGUITimebar.EasingFunc(Easing.EaseOutBack)));
		SBGUITimebar.UpdateProgress updateProgress = delegate()
		{
			ulong num = completeTime - totalTime;
			ulong num2 = TFUtils.EpochTime();
			float b = (num2 - num) / totalTime;
			ulong num3 = completeTime - num2;
			if (num3 < 0UL)
			{
				num3 = 0UL;
			}
			this.SetProgress(Mathf.Min(1f, b), num3);
			return num3 > 0UL;
		};
		base.StartCoroutine(this.TimeoutCoroutine(duration, hPosition, updateProgress));
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0002B2D8 File Offset: 0x000294D8
	public Vector2 GetRushButtonScreenPosition()
	{
		return this.dict["rush_button"].GetScreenPosition();
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0002B2F0 File Offset: 0x000294F0
	private IEnumerator ScaleCoroutine(float startScale, float endScale, float duration, SBGUITimebar.EasingFunc easing)
	{
		float elapsed = 0f;
		float inverseDuration = 1f / duration;
		base.gameObject.transform.localScale = new Vector3(startScale, startScale, startScale);
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float elapsedOverDuration = elapsed * inverseDuration;
			float currentScale = easing(startScale, endScale, elapsedOverDuration);
			base.gameObject.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
			yield return null;
		}
		base.gameObject.transform.localScale = new Vector3(endScale, endScale, endScale);
		yield break;
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0002B348 File Offset: 0x00029548
	private IEnumerator TimeoutCoroutine(float duration, SBGUITimebar.HostPosition hPosition, SBGUITimebar.UpdateProgress updateProgress)
	{
		this.elapsed = 0f;
		while (this.elapsed < duration || SBGUI.GetInstance().CheckWhitelisted(this.rushButton))
		{
			this.elapsed += Time.deltaTime;
			if (!updateProgress())
			{
				break;
			}
			if (hPosition != null)
			{
				Vector3 pos = hPosition();
				this.SetScreenPosition(pos.x, pos.y);
			}
			yield return null;
		}
		if (this.closeFinishedAction != null)
		{
			this.closeFinishedAction();
		}
		yield return base.StartCoroutine(this.ScaleCoroutine(1f, 0.1f, 0.5f, new SBGUITimebar.EasingFunc(Easing.EaseInBack)));
		SBUIBuilder.ReleaseTimebar(this);
		yield break;
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x0002B390 File Offset: 0x00029590
	public void SetProgress(float percent, ulong duration)
	{
		this.meter.Progress = percent;
		this.durationLabel.SetText(TFUtils.DurationToString(duration));
		if (this.maxJellyCost > 0)
		{
			this.rushLabel.SetText(Resource.Prorate(this.maxJellyCost, 1f - percent).ToString());
		}
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x0002B3F0 File Offset: 0x000295F0
	private IEnumerator CloseCoroutine()
	{
		yield return base.StartCoroutine(this.ScaleCoroutine(base.tform.localScale.x, 0.1f, 0.5f, new SBGUITimebar.EasingFunc(Easing.EaseInBack)));
		base.StopAllCoroutines();
		SBUIBuilder.ReleaseTimebar(this);
		yield break;
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002B40C File Offset: 0x0002960C
	public void Close()
	{
		if (base.IsActive())
		{
			this.rushButton.ClearClickEvents();
			base.StartCoroutine(this.CloseCoroutine());
		}
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002B43C File Offset: 0x0002963C
	public void RemoveCompleteAction()
	{
		this.closeFinishedAction = null;
	}

	// Token: 0x04000523 RID: 1315
	public float elapsed;

	// Token: 0x04000524 RID: 1316
	private Dictionary<string, SBGUIElement> dict;

	// Token: 0x04000525 RID: 1317
	private SBGUIProgressMeter meter;

	// Token: 0x04000526 RID: 1318
	private SBGUILabel durationLabel;

	// Token: 0x04000527 RID: 1319
	private SBGUILabel rushLabel;

	// Token: 0x04000528 RID: 1320
	private SBGUIButton rushButton;

	// Token: 0x04000529 RID: 1321
	private Action closeFinishedAction;

	// Token: 0x0400052A RID: 1322
	private SBGUICharacterArrowList m_pTaskCharacterList;

	// Token: 0x0400052B RID: 1323
	private int maxJellyCost = -1;

	// Token: 0x0400052C RID: 1324
	private string originalRushButtonSessionActionId;

	// Token: 0x0200049A RID: 1178
	// (Invoke) Token: 0x060024CB RID: 9419
	public delegate Vector3 HostPosition();

	// Token: 0x0200049B RID: 1179
	// (Invoke) Token: 0x060024CF RID: 9423
	public delegate float EasingFunc(float start, float end, float duration);

	// Token: 0x0200049C RID: 1180
	// (Invoke) Token: 0x060024D3 RID: 9427
	public delegate bool UpdateProgress();
}
