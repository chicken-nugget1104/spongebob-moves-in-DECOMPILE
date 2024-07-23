using System;

namespace MTools
{
	// Token: 0x020003B9 RID: 953
	public class MList
	{
		// Token: 0x06001BF9 RID: 7161 RVA: 0x000B402C File Offset: 0x000B222C
		public MList()
		{
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000B4034 File Offset: 0x000B2234
		public MList(bool circular)
		{
			this.mIsCircular = circular;
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000B4044 File Offset: 0x000B2244
		public int count()
		{
			return this.mCount;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000B404C File Offset: 0x000B224C
		public void PushFront(object data)
		{
			if (this.mStart == null)
			{
				if (this.mIsCircular)
				{
					this.mStart = new MList.MListNode();
					this.mStart.data = data;
					this.mEnd = this.mStart;
					this.mStart.next = this.mStart;
					this.mStart.prev = this.mStart;
				}
				else
				{
					this.mStart = new MList.MListNode();
					this.mStart.data = data;
					this.mEnd = this.mStart;
					this.mStart.next = null;
					this.mStart.prev = null;
				}
				this.mCount = 1;
			}
			else
			{
				if (this.mIsCircular)
				{
					MList.MListNode mlistNode = new MList.MListNode();
					mlistNode.data = data;
					mlistNode.next = this.mStart;
					mlistNode.prev = this.mEnd;
					this.mEnd.next = mlistNode;
					this.mStart.prev = mlistNode;
					this.mStart = mlistNode;
				}
				else
				{
					MList.MListNode mlistNode2 = new MList.MListNode();
					mlistNode2.data = data;
					this.mStart.prev = mlistNode2;
					mlistNode2.next = this.mStart;
					this.mStart = mlistNode2;
				}
				this.mCount++;
			}
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000B4190 File Offset: 0x000B2390
		public void PushBack(object data)
		{
			if (this.mEnd == null)
			{
				if (this.mIsCircular)
				{
					this.mEnd = new MList.MListNode();
					this.mEnd.data = data;
					this.mEnd = this.mStart;
					this.mEnd.next = this.mEnd;
					this.mEnd.prev = this.mEnd;
				}
				else
				{
					this.mEnd = new MList.MListNode();
					this.mEnd.data = data;
					this.mEnd.next = null;
					this.mEnd.prev = null;
					this.mStart = this.mEnd;
				}
				this.mCount = 1;
			}
			else
			{
				if (this.mIsCircular)
				{
					MList.MListNode mlistNode = new MList.MListNode();
					mlistNode.data = data;
					mlistNode.next = this.mStart;
					mlistNode.prev = this.mEnd;
					this.mStart.prev = mlistNode;
					this.mEnd.next = mlistNode;
					this.mEnd = mlistNode;
				}
				else
				{
					MList.MListNode mlistNode2 = new MList.MListNode();
					mlistNode2.data = data;
					this.mEnd.next = mlistNode2;
					mlistNode2.prev = this.mEnd;
					this.mEnd = mlistNode2;
				}
				this.mCount++;
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000B42D4 File Offset: 0x000B24D4
		public void Insert(object data, int offset)
		{
			if (data == null)
			{
				return;
			}
			if (this.mStart == null)
			{
				this.PushFront(data);
			}
			else
			{
				if (!this.mIsCircular)
				{
					if (offset < 0)
					{
						this.PushFront(data);
						return;
					}
					if (offset > this.mCount)
					{
						this.PushBack(data);
						return;
					}
				}
				MList.MListNode next = this.mStart;
				for (int i = 0; i < offset; i++)
				{
					next = next.next;
				}
				MList.MListNode mlistNode = new MList.MListNode();
				mlistNode.data = data;
				mlistNode.prev = next;
				mlistNode.next = next.next;
				next.next.prev = mlistNode;
				next.next = mlistNode;
				this.mCount++;
			}
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x000B4390 File Offset: 0x000B2590
		public object ObjectAtIndex(int idx)
		{
			if (idx < 0)
			{
				return this.GetFront();
			}
			if (idx >= this.mCount && !this.mIsCircular)
			{
				return this.GetBack();
			}
			MList.MListNode next = this.mStart;
			for (int i = 0; i < idx; i++)
			{
				next = next.next;
			}
			return next.data;
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x000B43F0 File Offset: 0x000B25F0
		public object GetFront()
		{
			if (this.mStart == null)
			{
				return null;
			}
			return this.mStart.data;
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x000B440C File Offset: 0x000B260C
		public object GetBack()
		{
			if (this.mEnd == null)
			{
				return null;
			}
			return this.mEnd.data;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000B4428 File Offset: 0x000B2628
		public object PopFront()
		{
			if (this.mStart == null)
			{
				return null;
			}
			object data = this.mStart.data;
			if (this.mIsCircular)
			{
				MList.MListNode next = this.mStart.next;
				next.prev = this.mEnd;
				this.mEnd.next = next;
				this.mStart.next = null;
				this.mStart.prev = null;
				this.mStart.data = null;
				this.mStart = next;
			}
			else
			{
				MList.MListNode next2 = this.mStart.next;
				this.mStart.data = null;
				this.mStart.next = null;
				this.mStart.prev = null;
				this.mStart = next2;
				if (next2 != null)
				{
					next2.prev = null;
				}
				else
				{
					this.mEnd = (this.mStart = null);
				}
			}
			this.mCount--;
			return data;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000B4518 File Offset: 0x000B2718
		public object PopBack()
		{
			if (this.mStart == null)
			{
				return null;
			}
			object data = this.mEnd.data;
			if (this.mIsCircular)
			{
				MList.MListNode prev = this.mEnd.prev;
				prev.next = this.mStart;
				this.mStart.prev = prev;
				this.mEnd.next = null;
				this.mEnd.prev = null;
				this.mEnd.data = null;
				this.mEnd = prev;
			}
			else
			{
				MList.MListNode prev2 = this.mEnd.prev;
				this.mEnd.data = null;
				this.mEnd.next = null;
				this.mEnd.prev = null;
				this.mEnd = prev2;
				if (prev2 != null)
				{
					prev2.next = null;
				}
				else
				{
					this.mEnd = (this.mStart = null);
				}
			}
			this.mCount--;
			return data;
		}

		// Token: 0x0400122C RID: 4652
		private MList.MListNode mStart;

		// Token: 0x0400122D RID: 4653
		private MList.MListNode mEnd;

		// Token: 0x0400122E RID: 4654
		private int mCount;

		// Token: 0x0400122F RID: 4655
		private bool mIsCircular;

		// Token: 0x020003BA RID: 954
		public class MListNode
		{
			// Token: 0x04001230 RID: 4656
			public object data;

			// Token: 0x04001231 RID: 4657
			public MList.MListNode next;

			// Token: 0x04001232 RID: 4658
			public MList.MListNode prev;
		}
	}
}
