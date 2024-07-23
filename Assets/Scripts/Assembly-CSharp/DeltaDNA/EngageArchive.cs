using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace DeltaDNA
{
	// Token: 0x02000004 RID: 4
	internal sealed class EngageArchive
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public EngageArchive(string path)
		{
			this.Load(path);
			this._path = path;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D24 File Offset: 0x00000F24
		public bool Contains(string decisionPoint)
		{
			Debug.Log("Does Engage contain " + decisionPoint);
			return this._table.ContainsKey(decisionPoint);
		}

		// Token: 0x1700000C RID: 12
		public string this[string decisionPoint]
		{
			get
			{
				return this._table[decisionPoint] as string;
			}
			set
			{
				object @lock = this._lock;
				lock (@lock)
				{
					this._table[decisionPoint] = value;
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002DA8 File Offset: 0x00000FA8
		private void Load(string path)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				try
				{
					string text = Path.Combine(path, EngageArchive.FILENAME);
					Debug.Log("Loading Engage from " + text);
					if (File.Exists(text))
					{
						using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
						{
							string key = null;
							int num = 0;
							byte[] array = new byte[4];
							while (fileStream.Read(array, 0, array.Length) > 0)
							{
								int num2 = BitConverter.ToInt32(array, 0);
								byte[] array2 = new byte[num2];
								fileStream.Read(array2, 0, array2.Length);
								if (num % 2 == 0)
								{
									key = Encoding.UTF8.GetString(array2, 0, array2.Length);
								}
								else
								{
									string @string = Encoding.UTF8.GetString(array2, 0, array2.Length);
									this._table.Add(key, @string);
								}
								num++;
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Unable to load Engagement archive: " + ex.Message);
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002F10 File Offset: 0x00001110
		public void Save()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				try
				{
					if (!Directory.Exists(this._path))
					{
						Directory.CreateDirectory(this._path);
					}
					List<byte> list = new List<byte>();
					foreach (object obj in this._table)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						byte[] bytes = Encoding.UTF8.GetBytes(dictionaryEntry.Key as string);
						byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
						byte[] bytes3 = Encoding.UTF8.GetBytes(dictionaryEntry.Value as string);
						byte[] bytes4 = BitConverter.GetBytes(bytes3.Length);
						list.AddRange(bytes2);
						list.AddRange(bytes);
						list.AddRange(bytes4);
						list.AddRange(bytes3);
					}
					byte[] array = list.ToArray();
					string path = Path.Combine(this._path, EngageArchive.FILENAME);
					using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
					{
						fileStream.Write(array, 0, array.Length);
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Unable to save Engagement archive: " + ex.Message);
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000030D0 File Offset: 0x000012D0
		public void Clear()
		{
			object @lock = this._lock;
			lock (@lock)
			{
				this._table.Clear();
			}
		}

		// Token: 0x0400000E RID: 14
		private Hashtable _table = new Hashtable();

		// Token: 0x0400000F RID: 15
		private object _lock = new object();

		// Token: 0x04000010 RID: 16
		private static readonly string FILENAME = "ENGAGEMENTS";

		// Token: 0x04000011 RID: 17
		private string _path;
	}
}
