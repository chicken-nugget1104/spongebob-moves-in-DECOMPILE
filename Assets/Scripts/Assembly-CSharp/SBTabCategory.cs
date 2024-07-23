using System;

// Token: 0x020000AE RID: 174
public interface SBTabCategory
{
	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x0600067A RID: 1658
	// (set) Token: 0x0600067B RID: 1659
	string Name { get; set; }

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600067C RID: 1660
	// (set) Token: 0x0600067D RID: 1661
	string Label { get; set; }

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600067E RID: 1662
	// (set) Token: 0x0600067F RID: 1663
	string Type { get; set; }

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000680 RID: 1664
	// (set) Token: 0x06000681 RID: 1665
	string Texture { get; set; }

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000682 RID: 1666
	// (set) Token: 0x06000683 RID: 1667
	int MicroEventDID { get; set; }

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000684 RID: 1668
	// (set) Token: 0x06000685 RID: 1669
	bool MicroEventOnly { get; set; }
}
