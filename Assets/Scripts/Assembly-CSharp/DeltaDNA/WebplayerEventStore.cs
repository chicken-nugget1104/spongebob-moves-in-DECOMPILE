using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	// Token: 0x0200000E RID: 14
	public class WebplayerEventStore : IEventStore, IDisposable
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00004710 File Offset: 0x00002910
		public bool Push(string obj)
		{
			this.inEvents.Enqueue(obj);
			return true;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004720 File Offset: 0x00002920
		public bool Swap()
		{
			if (this.outEvents.Count == 0)
			{
				Queue<string> queue = this.outEvents;
				this.outEvents = this.inEvents;
				this.inEvents = queue;
				return true;
			}
			return false;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000475C File Offset: 0x0000295C
		public List<string> Read()
		{
			List<string> list = new List<string>();
			foreach (string item in this.outEvents)
			{
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000047CC File Offset: 0x000029CC
		public void Clear()
		{
			this.outEvents.Clear();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000047DC File Offset: 0x000029DC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000047EC File Offset: 0x000029EC
		~WebplayerEventStore()
		{
			this.Dispose(false);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004828 File Offset: 0x00002A28
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
				}
				this.disposed = true;
			}
		}

		// Token: 0x0400004B RID: 75
		private Queue<string> inEvents = new Queue<string>();

		// Token: 0x0400004C RID: 76
		private Queue<string> outEvents = new Queue<string>();

		// Token: 0x0400004D RID: 77
		private bool disposed;
	}
}
