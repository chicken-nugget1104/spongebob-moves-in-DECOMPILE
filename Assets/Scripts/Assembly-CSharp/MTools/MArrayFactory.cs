using System;

namespace MTools
{
	// Token: 0x020003AF RID: 943
	public static class MArrayFactory
	{
		// Token: 0x06001B43 RID: 6979 RVA: 0x000B18A8 File Offset: 0x000AFAA8
		private static void _Setup()
		{
			MArrayFactory.sMArrayList = new MArray[32];
			for (int i = 0; i < 32; i++)
			{
				MArrayFactory.sMArrayList[i] = new MArray(2);
			}
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x000B18E4 File Offset: 0x000AFAE4
		public static MArray MArray(int capacity)
		{
			if (MArrayFactory.sMArrayList == null)
			{
				MArrayFactory._Setup();
			}
			if (capacity >= 32 || capacity < 0)
			{
				return new MArray();
			}
			MArray marray = MArrayFactory.sMArrayList[capacity];
			int num = marray.count();
			if (num <= 0)
			{
				return new MArray(capacity);
			}
			MArray result = (MArray)marray.objectAtIndex(num - 1);
			marray.removeObjectAtIndex(num - 1);
			return result;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x000B194C File Offset: 0x000AFB4C
		public static void ReturnMArray(MArray arr)
		{
			if (arr == null || MArrayFactory.sMArrayList == null)
			{
				return;
			}
			int num = arr.capacity();
			if (num >= 32 || num < 0)
			{
				return;
			}
			arr.clear();
			MArrayFactory.sMArrayList[num].addObject(arr);
		}

		// Token: 0x0400120F RID: 4623
		private const int sMaxArraySize = 32;

		// Token: 0x04001210 RID: 4624
		private static MArray[] sMArrayList;
	}
}
