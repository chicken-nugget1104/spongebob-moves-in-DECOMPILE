using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class AndroidBack : MonoBehaviour
{
	// Token: 0x06000214 RID: 532 RVA: 0x0000A398 File Offset: 0x00008598
	public static AndroidBack getInstance()
	{
		if (AndroidBack.instance == null)
		{
			AndroidBack.instance = new GameObject
			{
				name = "AndroidBack"
			}.AddComponent<AndroidBack>();
		}
		return AndroidBack.instance;
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000A3D8 File Offset: 0x000085D8
	public void addSession(Session session)
	{
		this.session = session;
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000A3E4 File Offset: 0x000085E4
	public int count()
	{
		return this.actionStack.Count;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000A3F4 File Offset: 0x000085F4
	public void push(Action action, object ob)
	{
		if (ob == this.getTopObject())
		{
			return;
		}
		this.actionStack.Push(action);
		this.objectStack.Push(ob);
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000A41C File Offset: 0x0000861C
	public Action pop()
	{
		if (this.actionStack.Count == 0)
		{
			return null;
		}
		Action action = null;
		while (action == null && this.actionStack.Count > 0)
		{
			action = this.actionStack.Pop();
			this.objectStack.Pop();
		}
		if (action != null)
		{
		}
		return action;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0000A478 File Offset: 0x00008678
	public Action pop(Action action)
	{
		if (this.actionStack.Contains(action))
		{
			action = this.actionStack.Pop();
			this.objectStack.Pop();
		}
		return action;
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000A4A8 File Offset: 0x000086A8
	public Action getTopAction()
	{
		if (this.actionStack.Count == 0)
		{
			return null;
		}
		List<Action> list = new List<Action>(this.actionStack.ToArray());
		return list[0];
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000A4E0 File Offset: 0x000086E0
	public object getTopObject()
	{
		if (this.objectStack.Count == 0)
		{
			return null;
		}
		List<object> list = new List<object>(this.objectStack.ToArray());
		return list[0];
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000A518 File Offset: 0x00008718
	private void Update()
	{
		if (this.isQuiting)
		{
			return;
		}
		if (this.delay > 0)
		{
			this.delay--;
			return;
		}
		if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape))
		{
			if (RestrictInteraction.ContainsWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT))
			{
				return;
			}
			if (this.session != null && this.session.TheGame != null && this.session.TheGame.simulation != null && this.session.TheGame.simulation.Whitelisted)
			{
				return;
			}
			Debug.LogError("-------------------Input.GetKey(KeyCode.Escape)------------------------");
			this.delay = 20;
			Action action = this.pop();
			if (action != null)
			{
				action();
			}
			else
			{
				Debug.Log("quit");
				if (this.session == null)
				{
					return;
				}
				if (this.isShowingQuitDlg)
				{
					return;
				}
				this.isShowingQuitDlg = true;
				this.session.TheSoundEffectManager.PlaySound("OpenMenu");
				bool bRestrictAllUI = false;
				if (RestrictInteraction.ContainsWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT))
				{
					bRestrictAllUI = true;
					RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
				}
				Action cancelButtonHandler = delegate()
				{
					this.isShowingQuitDlg = false;
					this.session.TheSoundEffectManager.PlaySound("Cancel");
					SBUIBuilder.ReleaseTopScreen();
					if (bRestrictAllUI)
					{
						RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
					}
					if (this.session.TheState is Session.Playing)
					{
						this.session.ChangeState("Playing", true);
					}
				};
				Action okButtonHandler = delegate()
				{
					Debug.Log("ok");
					this.isQuiting = true;
					this.isShowingQuitDlg = false;
					if (bRestrictAllUI)
					{
						RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
					}
					this.StartCoroutine(this.quit());
				};
				SBUIBuilder.MakeAndPushConfirmationDialog(this.session, null, "Quit", Language.Get("!!ANDROID_QUIT_ALERT"), "YES", "NO", null, okButtonHandler, cancelButtonHandler, false);
			}
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000A6A4 File Offset: 0x000088A4
	private IEnumerator quit()
	{
		this.session.ChangeState("Stopping", true);
		yield return new WaitForSeconds(0.3f);
		Application.Quit();
		yield break;
	}

	// Token: 0x04000118 RID: 280
	private static AndroidBack instance;

	// Token: 0x04000119 RID: 281
	private int delay;

	// Token: 0x0400011A RID: 282
	private bool isQuiting;

	// Token: 0x0400011B RID: 283
	private bool isShowingQuitDlg;

	// Token: 0x0400011C RID: 284
	private Session session;

	// Token: 0x0400011D RID: 285
	private Stack<Action> actionStack = new Stack<Action>();

	// Token: 0x0400011E RID: 286
	private Stack<object> objectStack = new Stack<object>();
}
