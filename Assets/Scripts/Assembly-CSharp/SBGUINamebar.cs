using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class SBGUINamebar : SBGUIElement
{
	// Token: 0x06000580 RID: 1408 RVA: 0x00023850 File Offset: 0x00021A50
	protected override void Awake()
	{
		this.dict = base.CacheChildren();
		this.nameLabel = (SBGUILabel)this.dict["name_label"];
		this.m_pTaskCharacterList = (SBGUICharacterArrowList)base.FindChild("character_portrait_parent");
		base.Awake();
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x000238A0 File Offset: 0x00021AA0
	public void Setup(Session session, string name, SBGUINamebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		this.nameLabel.SetText(name);
		this.closeFinishedAction = onFinish;
		this.m_hPosition = hPosition;
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
		base.StartCoroutine(this.ScaleCoroutine(0.1f, 1f, 0.5f, new SBGUINamebar.EasingFunc(Easing.EaseOutBack)));
		base.StartCoroutine(this.TimeoutCoroutine(5f, hPosition));
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00023A30 File Offset: 0x00021C30
	private void Update()
	{
		if (this.m_hPosition != null)
		{
			Vector3 vector = this.m_hPosition();
			this.SetScreenPosition(vector.x, vector.y);
		}
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00023A68 File Offset: 0x00021C68
	public Vector2 GetRushButtonScreenPosition()
	{
		return this.dict["rush_button"].GetScreenPosition();
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00023A80 File Offset: 0x00021C80
	private IEnumerator ScaleCoroutine(float startScale, float endScale, float duration, SBGUINamebar.EasingFunc easing)
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

	// Token: 0x06000585 RID: 1413 RVA: 0x00023AD8 File Offset: 0x00021CD8
	private IEnumerator TimeoutCoroutine(float duration, SBGUINamebar.HostPosition hPosition)
	{
		this.elapsed = 0f;
		while (this.elapsed < duration)
		{
			this.elapsed += Time.deltaTime;
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
		yield return base.StartCoroutine(this.ScaleCoroutine(1f, 0.1f, 0.5f, new SBGUINamebar.EasingFunc(Easing.EaseInBack)));
		SBUIBuilder.ReleaseNamebar(this);
		yield break;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00023B10 File Offset: 0x00021D10
	private IEnumerator CloseCoroutine()
	{
		yield return base.StartCoroutine(this.ScaleCoroutine(base.tform.localScale.x, 0.1f, 0.5f, new SBGUINamebar.EasingFunc(Easing.EaseInBack)));
		base.StopAllCoroutines();
		SBUIBuilder.ReleaseNamebar(this);
		yield break;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00023B2C File Offset: 0x00021D2C
	public void Close()
	{
		if (base.IsActive())
		{
			base.StartCoroutine(this.CloseCoroutine());
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00023B48 File Offset: 0x00021D48
	public void RemoveCompleteAction()
	{
		this.closeFinishedAction = null;
	}

	// Token: 0x04000442 RID: 1090
	public float elapsed;

	// Token: 0x04000443 RID: 1091
	private Dictionary<string, SBGUIElement> dict;

	// Token: 0x04000444 RID: 1092
	private SBGUILabel nameLabel;

	// Token: 0x04000445 RID: 1093
	private Action closeFinishedAction;

	// Token: 0x04000446 RID: 1094
	private SBGUICharacterArrowList m_pTaskCharacterList;

	// Token: 0x04000447 RID: 1095
	private SBGUINamebar.HostPosition m_hPosition;

	// Token: 0x02000496 RID: 1174
	// (Invoke) Token: 0x060024BB RID: 9403
	public delegate Vector3 HostPosition();

	// Token: 0x02000497 RID: 1175
	// (Invoke) Token: 0x060024BF RID: 9407
	public delegate float EasingFunc(float start, float end, float duration);

	// Token: 0x02000498 RID: 1176
	// (Invoke) Token: 0x060024C3 RID: 9411
	public delegate bool UpdateProgress();
}
