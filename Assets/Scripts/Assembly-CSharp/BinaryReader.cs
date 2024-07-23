using System;
using System.IO;
using UnityEngine;

// Token: 0x02000441 RID: 1089
public class BinaryReader : Reader
{
	// Token: 0x0600218B RID: 8587 RVA: 0x000CEFB4 File Offset: 0x000CD1B4
	public BinaryReader()
	{
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000CEFBC File Offset: 0x000CD1BC
	public BinaryReader(string resourceName)
	{
		this.Open(resourceName);
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x000CEFCC File Offset: 0x000CD1CC
	public void Open(string resourceName)
	{
		TextAsset textAsset = Resources.Load(resourceName) as TextAsset;
		if (textAsset != null)
		{
			this.binaryReader = new System.IO.BinaryReader(new MemoryStream(textAsset.bytes));
		}
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000CF008 File Offset: 0x000CD208
	public void Close()
	{
		this.binaryReader.Close();
		this.binaryReader = null;
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x000CF01C File Offset: 0x000CD21C
	public void Read(out bool value)
	{
		value = this.binaryReader.ReadBoolean();
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x000CF02C File Offset: 0x000CD22C
	public void Read(out byte value)
	{
		value = this.binaryReader.ReadByte();
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000CF03C File Offset: 0x000CD23C
	public void Read(out short value)
	{
		value = this.binaryReader.ReadInt16();
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x000CF04C File Offset: 0x000CD24C
	public void Read(out ushort value)
	{
		value = this.binaryReader.ReadUInt16();
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000CF05C File Offset: 0x000CD25C
	public void Read(out int value)
	{
		value = this.binaryReader.ReadInt32();
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x000CF06C File Offset: 0x000CD26C
	public void Read(out uint value)
	{
		value = this.binaryReader.ReadUInt32();
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000CF07C File Offset: 0x000CD27C
	public void Read(out float value)
	{
		value = this.binaryReader.ReadSingle();
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x000CF08C File Offset: 0x000CD28C
	public void Read(out double value)
	{
		value = this.binaryReader.ReadDouble();
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000CF09C File Offset: 0x000CD29C
	public void Read(out Vector2 value)
	{
		this.Read(out value.x);
		this.Read(out value.y);
	}

	// Token: 0x06002198 RID: 8600 RVA: 0x000CF0B8 File Offset: 0x000CD2B8
	public void Read(out Vector3 value)
	{
		this.Read(out value.x);
		this.Read(out value.y);
		this.Read(out value.z);
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x000CF0EC File Offset: 0x000CD2EC
	public void Read(out AlignedBox value)
	{
		value = new AlignedBox();
		this.Read(out value.xmin);
		this.Read(out value.xmax);
		this.Read(out value.ymin);
		this.Read(out value.ymax);
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x000CF134 File Offset: 0x000CD334
	public void Read(out string value)
	{
		value = this.binaryReader.ReadString();
	}

	// Token: 0x040014B2 RID: 5298
	private System.IO.BinaryReader binaryReader;
}
