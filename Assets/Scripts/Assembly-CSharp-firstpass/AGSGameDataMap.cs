using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class AGSGameDataMap : AGSSyncable
{
	// Token: 0x06000277 RID: 631 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
	public AGSGameDataMap(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000C2DC File Offset: 0x0000A4DC
	public AGSGameDataMap(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
	public AGSSyncableNumber GetHighestNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumber>(AGSSyncable.SyncableMethod.getHighestNumber, name);
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000C2F4 File Offset: 0x0000A4F4
	public HashSet<string> GetHighestNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getHighestNumberKeys);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000C300 File Offset: 0x0000A500
	public AGSSyncableNumber GetLowestNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumber>(AGSSyncable.SyncableMethod.getLowestNumber, name);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000C30C File Offset: 0x0000A50C
	public HashSet<string> GetLowestNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLowestNumberKeys);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000C318 File Offset: 0x0000A518
	public AGSSyncableNumber GetLatestNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumber>(AGSSyncable.SyncableMethod.getLatestNumber, name);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000C324 File Offset: 0x0000A524
	public HashSet<string> GetLatestNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestNumberKeys);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000C330 File Offset: 0x0000A530
	public AGSSyncableNumberList GetHighNumberList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumberList>(AGSSyncable.SyncableMethod.getHighNumberList, name);
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0000C33C File Offset: 0x0000A53C
	public HashSet<string> GetHighNumberListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getHighNumberListKeys);
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000C348 File Offset: 0x0000A548
	public AGSSyncableNumberList GetLowNumberList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumberList>(AGSSyncable.SyncableMethod.getLowNumberList, name);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000C354 File Offset: 0x0000A554
	public HashSet<string> GetLowNumberListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLowNumberListKeys);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000C360 File Offset: 0x0000A560
	public AGSSyncableNumberList GetLatestNumberList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableNumberList>(AGSSyncable.SyncableMethod.getLatestNumberList, name);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000C36C File Offset: 0x0000A56C
	public HashSet<string> GetLatestNumberListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestNumberListKeys);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x0000C378 File Offset: 0x0000A578
	public AGSSyncableAccumulatingNumber GetAccumulatingNumber(string name)
	{
		return base.GetAGSSyncable<AGSSyncableAccumulatingNumber>(AGSSyncable.SyncableMethod.getAccumulatingNumber, name);
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000C384 File Offset: 0x0000A584
	public HashSet<string> GetAccumulatingNumberKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getAccumulatingNumberKeys);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000C390 File Offset: 0x0000A590
	public AGSSyncableString GetLatestString(string name)
	{
		return base.GetAGSSyncable<AGSSyncableString>(AGSSyncable.SyncableMethod.getLatestString, name);
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0000C39C File Offset: 0x0000A59C
	public HashSet<string> GetLatestStringKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestStringKeys);
	}

	// Token: 0x06000289 RID: 649 RVA: 0x0000C3A8 File Offset: 0x0000A5A8
	public AGSSyncableStringList GetLatestStringList(string name)
	{
		return base.GetAGSSyncable<AGSSyncableStringList>(AGSSyncable.SyncableMethod.getLatestStringList, name);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
	public HashSet<string> GetLatestStringListKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getLatestStringListKeys);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
	public AGSSyncableStringSet GetStringSet(string name)
	{
		return base.GetAGSSyncable<AGSSyncableStringSet>(AGSSyncable.SyncableMethod.getStringSet, name);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0000C3CC File Offset: 0x0000A5CC
	public HashSet<string> GetStringSetKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getStringSetKeys);
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000C3D8 File Offset: 0x0000A5D8
	public AGSGameDataMap GetMap(string name)
	{
		return base.GetAGSSyncable<AGSGameDataMap>(AGSSyncable.SyncableMethod.getMap);
	}

	// Token: 0x0600028E RID: 654 RVA: 0x0000C3E4 File Offset: 0x0000A5E4
	public HashSet<string> GetMapKeys()
	{
		return base.GetHashSet(AGSSyncable.HashSetMethod.getMapKeys);
	}
}
