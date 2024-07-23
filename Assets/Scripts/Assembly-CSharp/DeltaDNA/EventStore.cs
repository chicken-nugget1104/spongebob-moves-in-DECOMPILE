using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace DeltaDNA
{
	// Token: 0x02000005 RID: 5
	public class EventStore : IEventStore, IDisposable
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00003120 File Offset: 0x00001320
		public EventStore(string path, bool debug = false)
		{
			this.debug = debug;
			try
			{
				this.InitialiseFileStreams(path, false);
				this.initialised = true;
			}
			catch (Exception ex)
			{
				this.Log("Problem initialising Event Store: " + ex.Message);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000031C8 File Offset: 0x000013C8
		public bool Push(string obj)
		{
			if (this.initialised && this.infs.Length < EventStore.MAX_FILE_SIZE)
			{
				try
				{
					byte[] bytes = Encoding.UTF8.GetBytes(obj);
					byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
					List<byte> list = new List<byte>();
					list.AddRange(bytes2);
					list.AddRange(bytes);
					byte[] array = list.ToArray();
					this.infs.Write(array, 0, array.Length);
					return true;
				}
				catch (Exception ex)
				{
					this.Log("Problem pushing event to Event Store: " + ex.Message);
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003280 File Offset: 0x00001480
		public bool Swap()
		{
			if (this.outfs.Length == 0L)
			{
				this.infs.Flush();
				FileStream fileStream = this.infs;
				this.infs = this.outfs;
				this.outfs = fileStream;
				this.infs.SetLength(0L);
				this.outfs.Seek(0L, SeekOrigin.Begin);
				PlayerPrefs.SetString(EventStore.PF_KEY_IN_FILE, Path.GetFileName(this.infs.Name));
				PlayerPrefs.SetString(EventStore.PF_KEY_OUT_FILE, Path.GetFileName(this.outfs.Name));
				return true;
			}
			return false;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003318 File Offset: 0x00001518
		public List<string> Read()
		{
			List<string> list = new List<string>();
			try
			{
				byte[] array = new byte[4];
				while (this.outfs.Read(array, 0, array.Length) > 0)
				{
					int num = BitConverter.ToInt32(array, 0);
					byte[] array2 = new byte[num];
					this.outfs.Read(array2, 0, array2.Length);
					string @string = Encoding.UTF8.GetString(array2, 0, array2.Length);
					list.Add(@string);
				}
				this.outfs.Seek(0L, SeekOrigin.Begin);
			}
			catch (Exception ex)
			{
				this.Log("Problem reading events from Event Store: " + ex.Message);
			}
			return list;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000033D8 File Offset: 0x000015D8
		public void Clear()
		{
			this.infs.SetLength(0L);
			this.outfs.SetLength(0L);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000033F4 File Offset: 0x000015F4
		private void InitialiseFileStreams(string path, bool reset)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string text = PlayerPrefs.GetString(EventStore.PF_KEY_IN_FILE, EventStore.FILE_A);
			string text2 = PlayerPrefs.GetString(EventStore.PF_KEY_OUT_FILE, EventStore.FILE_B);
			text = Path.GetFileName(text);
			text2 = Path.GetFileName(text2);
			string text3 = Path.Combine(path, text);
			string text4 = Path.Combine(path, text2);
			FileMode mode = (!reset) ? FileMode.OpenOrCreate : FileMode.Create;
			if (File.Exists(text3) && File.Exists(text4) && !reset)
			{
				this.Log("Loaded existing Event Store in @ " + text3 + " out @ " + text4);
			}
			else
			{
				this.Log("Creating new Event Store in @ " + path);
			}
			this.infs = new FileStream(text3, mode, FileAccess.ReadWrite, FileShare.None, EventStore.FILE_BUFFER_SIZE);
			this.infs.Seek(0L, SeekOrigin.End);
			this.outfs = new FileStream(text4, mode, FileAccess.ReadWrite, FileShare.None, EventStore.FILE_BUFFER_SIZE);
			PlayerPrefs.SetString(EventStore.PF_KEY_IN_FILE, Path.GetFileName(this.infs.Name));
			PlayerPrefs.SetString(EventStore.PF_KEY_OUT_FILE, Path.GetFileName(this.outfs.Name));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003518 File Offset: 0x00001718
		private void Log(string message)
		{
			if (this.debug)
			{
				Debug.Log("[DDSDK EventStore] " + message);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003538 File Offset: 0x00001738
		~EventStore()
		{
			this.Dispose(false);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003574 File Offset: 0x00001774
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003584 File Offset: 0x00001784
		protected virtual void Dispose(bool disposing)
		{
			this.Log("Disposing on EventStore...");
			try
			{
				if (!this.disposed && disposing)
				{
					this.Log("Disposing filestreams");
					this.infs.Dispose();
					this.outfs.Dispose();
				}
			}
			finally
			{
				this.disposed = true;
			}
		}

		// Token: 0x04000012 RID: 18
		private static readonly string PF_KEY_IN_FILE = "DDSDK_EVENT_IN_FILE";

		// Token: 0x04000013 RID: 19
		private static readonly string PF_KEY_OUT_FILE = "DDSDK_EVENT_OUT_FILE";

		// Token: 0x04000014 RID: 20
		private static readonly string FILE_A = "A";

		// Token: 0x04000015 RID: 21
		private static readonly string FILE_B = "B";

		// Token: 0x04000016 RID: 22
		private static readonly int FILE_BUFFER_SIZE = 4096;

		// Token: 0x04000017 RID: 23
		private static readonly long MAX_FILE_SIZE = 41943040L;

		// Token: 0x04000018 RID: 24
		private FileStream infs;

		// Token: 0x04000019 RID: 25
		private FileStream outfs;

		// Token: 0x0400001A RID: 26
		private bool initialised;

		// Token: 0x0400001B RID: 27
		private bool disposed;

		// Token: 0x0400001C RID: 28
		private bool debug;
	}
}
