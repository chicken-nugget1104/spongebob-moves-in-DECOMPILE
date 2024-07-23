using System;

namespace Yarg
{
	// Token: 0x02000067 RID: 103
	public class YGEventDispatcher
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x0600039F RID: 927 RVA: 0x0000FF58 File Offset: 0x0000E158
		// (remove) Token: 0x060003A0 RID: 928 RVA: 0x0000FF74 File Offset: 0x0000E174
		private event Func<YGEvent, bool> eventListener;

		// Token: 0x060003A1 RID: 929 RVA: 0x0000FF90 File Offset: 0x0000E190
		public void AddListener(Func<YGEvent, bool> value)
		{
			if (value == null)
			{
				return;
			}
			if (this.eventListener == null)
			{
				this.eventListener = (Func<YGEvent, bool>)Delegate.Combine(this.eventListener, value);
				return;
			}
			this.eventListener = (Func<YGEvent, bool>)Delegate.Remove(this.eventListener, value);
			this.eventListener = (Func<YGEvent, bool>)Delegate.Combine(this.eventListener, value);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		public void RemoveListener(Func<YGEvent, bool> value)
		{
			if (this.eventListener == null)
			{
				return;
			}
			this.eventListener = (Func<YGEvent, bool>)Delegate.Remove(this.eventListener, value);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00010020 File Offset: 0x0000E220
		public void ClearListeners()
		{
			this.eventListener = null;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001002C File Offset: 0x0000E22C
		public bool FireEvent(YGEvent evt)
		{
			bool flag = false;
			if (this.eventListener == null)
			{
				return flag;
			}
			Delegate[] invocationList = this.eventListener.GetInvocationList();
			if (invocationList == null)
			{
				return flag;
			}
			foreach (Delegate @delegate in invocationList)
			{
				flag |= ((Func<YGEvent, bool>)@delegate)(evt);
			}
			return flag;
		}
	}
}
