using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000161 RID: 353
public class DialogPackageManager
{
	// Token: 0x06000C14 RID: 3092 RVA: 0x00048BB8 File Offset: 0x00046DB8
	public DialogPackageManager(Dictionary<string, object> gameState)
	{
		this.dialogPackages = new Dictionary<uint, DialogPackage>();
		this.dialogInputs = new List<DialogInputData>();
		this.activeDialogs = new HashSet<uint>();
		this.LoadDialogPackagesFromSpread();
		this.LoadPersistedDialogs(gameState);
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00048C08 File Offset: 0x00046E08
	private void LoadPersistedDialogs(Dictionary<string, object> gameState)
	{
		List<object> list = new List<object>();
		if (gameState.ContainsKey("dialogs"))
		{
			list = (List<object>)gameState["dialogs"];
		}
		foreach (object obj in list)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			PersistedDialogInputData persistedDialogInputData = PersistedDialogInputData.FromPersistenceDict(dict);
			if (!this.activeDialogs.Contains(persistedDialogInputData.SequenceId))
			{
				this.activeDialogs.Add(persistedDialogInputData.SequenceId);
			}
			this.dialogInputs.Add(persistedDialogInputData);
		}
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00048CCC File Offset: 0x00046ECC
	public DialogPackage GetDialogPackage(uint packageId)
	{
		TFUtils.Assert(this.dialogPackages.ContainsKey(packageId), "No dialog package registered with the packageId=" + packageId);
		return this.dialogPackages[packageId];
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00048CFC File Offset: 0x00046EFC
	private void PersistAddingDialogs(Game game, List<DialogInputData> inputs)
	{
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			List<object> list = new List<object>();
			if (gameState.ContainsKey("dialogs"))
			{
				list = (List<object>)gameState["dialogs"];
			}
			foreach (DialogInputData dialogInputData in inputs)
			{
				PersistedDialogInputData persistedDialogInputData = dialogInputData as PersistedDialogInputData;
				if (persistedDialogInputData != null)
				{
					list.Add(persistedDialogInputData.ToPersistenceDict());
				}
			}
		};
		game.LockedGameStateChange(writer);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00048D2C File Offset: 0x00046F2C
	private void PersistRemovingCurrentDialog(Game game, bool removeAll)
	{
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			if (gameState.ContainsKey("dialogs"))
			{
				List<object> list = (List<object>)gameState["dialogs"];
				if (removeAll)
				{
					list.Clear();
				}
				else
				{
					list.RemoveAt(0);
				}
			}
		};
		game.LockedGameStateChange(writer);
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00048D5C File Offset: 0x00046F5C
	public bool AddDialogInputBatch(Game game, List<DialogInputData> inputs, uint sequenceId = 4294967295U)
	{
		List<object> list = null;
		List<DialogInputData> list2 = new List<DialogInputData>();
		Type type = null;
		if (sequenceId != 4294967295U && this.activeDialogs.Contains(sequenceId))
		{
			TFUtils.ErrorLog("Adding a sequence that is already active!? Is this ok?");
			return false;
		}
		this.activeDialogs.Add(sequenceId);
		foreach (DialogInputData dialogInputData in inputs)
		{
			TFUtils.Assert(sequenceId == dialogInputData.SequenceId, "Got a DialogInputBatch where individual DialogInputData don't have the same sequenceId.");
			if (dialogInputData is CharacterDialogInputData)
			{
				CharacterDialogInputData characterDialogInputData = (CharacterDialogInputData)dialogInputData;
				if (type == null || !type.Equals(typeof(CharacterDialogInputData)))
				{
					list = new List<object>();
				}
				list.AddRange(characterDialogInputData.PromptsData);
			}
			else
			{
				if (list != null)
				{
					list2.Add(new CharacterDialogInputData(sequenceId, list));
					list = null;
				}
				list2.Add(dialogInputData);
			}
			type = dialogInputData.GetType();
		}
		if (list != null)
		{
			list2.Add(new CharacterDialogInputData(sequenceId, list));
		}
		this.dialogInputs.AddRange(list2);
		this.PersistAddingDialogs(game, list2);
		return true;
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00048E98 File Offset: 0x00047098
	public DialogInputData PeekCurrentDialogInput()
	{
		if (this.dialogInputs.Count > 0)
		{
			return this.dialogInputs[0];
		}
		return null;
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00048EBC File Offset: 0x000470BC
	public void RemoveCurrentDialogInput(Game game)
	{
		if (this.dialogInputs.Count > 0)
		{
			DialogInputData dialogInputData = this.dialogInputs[0];
			this.dialogInputs.RemoveAt(0);
			this.PersistRemovingCurrentDialog(game, false);
			if (this.dialogInputs.Count == 0 || dialogInputData.SequenceId != this.dialogInputs[0].SequenceId)
			{
				game.triggerRouter.RouteTrigger(dialogInputData.CreateTrigger(TFUtils.EpochTime()), game);
				this.activeDialogs.Remove(dialogInputData.SequenceId);
			}
		}
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00048F50 File Offset: 0x00047150
	public void ClearDialogs(Game game)
	{
		this.PersistRemovingCurrentDialog(game, true);
		this.activeDialogs.Clear();
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00048F68 File Offset: 0x00047168
	public int GetNumQueuedDialogInputs()
	{
		return this.dialogInputs.Count;
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00048F78 File Offset: 0x00047178
	private string[] GetFilesToLoad()
	{
		return Config.DIALOG_PACKAGES_PATH;
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00048F80 File Offset: 0x00047180
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00048F84 File Offset: 0x00047184
	private DialogPackage LoadDialogPackageFromFile(string filePath)
	{
		string json = TFUtils.ReadAllText(filePath);
		Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(json);
		return new DialogPackage(data);
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00048FAC File Offset: 0x000471AC
	private void LoadDialogPackages()
	{
		string[] filesToLoad = this.GetFilesToLoad();
		foreach (string filePath in filesToLoad)
		{
			string filePathFromString = this.GetFilePathFromString(filePath);
			DialogPackage dialogPackage = this.LoadDialogPackageFromFile(filePathFromString);
			this.dialogPackages[dialogPackage.Did] = dialogPackage;
		}
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00049004 File Offset: 0x00047204
	private void LoadDialogPackagesFromSpread()
	{
		string sheetName = "Dialogs";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_SHEET(sheetName);
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			TFError.DM_LOG_ERROR_NO_ROWS(sheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		Dictionary<int, Dictionary<string, object>> dictionary2 = new Dictionary<int, Dictionary<string, object>>();
		Dictionary<string, object> dictionary3 = null;
		dictionary.Add("type", "dialogs");
		dictionary.Add("did", 1);
		dictionary.Add("sequences", list);
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "special rewards");
		if (columnIndexInSheet < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("special rewards");
		}
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		if (columnIndexInSheet2 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("type");
		}
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "id");
		if (columnIndexInSheet3 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("id");
		}
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		if (columnIndexInSheet4 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("did");
		}
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "character icon");
		if (columnIndexInSheet5 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("character icon");
		}
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "text");
		if (columnIndexInSheet6 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("text");
		}
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "voiceover");
		if (columnIndexInSheet7 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("voiceover");
		}
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "title");
		if (columnIndexInSheet8 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("title");
		}
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "icon");
		if (columnIndexInSheet9 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("icon");
		}
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "portrait");
		if (columnIndexInSheet10 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("portrait");
		}
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "heading");
		if (columnIndexInSheet11 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("heading");
		}
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "body");
		if (columnIndexInSheet12 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("body");
		}
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "effect");
		if (columnIndexInSheet13 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("effect");
		}
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "reward xp");
		if (columnIndexInSheet14 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("reward xp");
		}
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "reward gold");
		if (columnIndexInSheet15 < 0)
		{
			TFError.DM_LOG_ERROR_INVALID_COLUMN("reward gold");
		}
		string b = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, columnIndexInSheet3).ToString());
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet);
				}
				bool flag = true;
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4);
				if (dictionary2.ContainsKey(intCell))
				{
					dictionary3 = dictionary2[intCell];
					flag = false;
				}
				Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
				dictionary4.Add("type", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet2));
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet5);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("character_icon", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet6);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("text", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet7);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("voiceover", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet8);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("title", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet9);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("icon", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet10);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("portrait", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet11);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("heading", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet12);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("body", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet13);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary4.Add("effect", stringCell);
				}
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14);
				if (intCell2 > 0)
				{
					dictionary4.Add("reward", new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>
							{
								{
									"5",
									intCell2
								}
							}
						}
					});
				}
				intCell2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15);
				if (intCell2 > 0)
				{
					if (!dictionary4.ContainsKey("reward"))
					{
						dictionary4.Add("reward", new Dictionary<string, object>
						{
							{
								"resources",
								new Dictionary<string, object>
								{
									{
										"3",
										intCell2
									}
								}
							}
						});
					}
					else
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary4["reward"])["resources"]).Add("3", intCell2);
					}
				}
				for (int j = 1; j <= num2; j++)
				{
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, "special reward type " + j.ToString());
					if (!string.IsNullOrEmpty(stringCell) && !(stringCell == b))
					{
						intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "special reward did " + j.ToString());
						int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "special reward amount " + j.ToString());
						if (!dictionary4.ContainsKey("reward"))
						{
							dictionary4.Add("reward", new Dictionary<string, object>
							{
								{
									stringCell,
									new Dictionary<string, object>
									{
										{
											intCell2.ToString(),
											intCell3
										}
									}
								}
							});
						}
						else if (!((Dictionary<string, object>)dictionary4["reward"]).ContainsKey(stringCell))
						{
							((Dictionary<string, object>)dictionary4["reward"]).Add(stringCell, new Dictionary<string, object>
							{
								{
									intCell2.ToString(),
									intCell3
								}
							});
						}
						else
						{
							((Dictionary<string, object>)((Dictionary<string, object>)dictionary4["reward"])[stringCell]).Add(intCell2.ToString(), intCell3);
						}
					}
				}
				if (flag)
				{
					dictionary3 = new Dictionary<string, object>();
					dictionary3.Add("id", intCell);
					dictionary3.Add("prompts", new List<object>
					{
						dictionary4
					});
					list.Add(dictionary3);
					dictionary2.Add(intCell, dictionary3);
				}
				else
				{
					((List<object>)dictionary3["prompts"]).Add(dictionary4);
				}
			}
		}
		DialogPackage dialogPackage = new DialogPackage(dictionary);
		this.dialogPackages[dialogPackage.Did] = dialogPackage;
	}

	// Token: 0x0400081B RID: 2075
	private static readonly string DIALOG_PACKAGES_PATH = "Dialogs";

	// Token: 0x0400081C RID: 2076
	private Dictionary<uint, DialogPackage> dialogPackages;

	// Token: 0x0400081D RID: 2077
	private List<DialogInputData> dialogInputs;

	// Token: 0x0400081E RID: 2078
	private HashSet<uint> activeDialogs;
}
