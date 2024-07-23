using System;
using System.IO;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class BinaryWriter : Writer
{
	// Token: 0x060023E0 RID: 9184 RVA: 0x000DD2B0 File Offset: 0x000DB4B0
	public BinaryWriter()
	{
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000DD2B8 File Offset: 0x000DB4B8
	public BinaryWriter(string localPath)
	{
		this.Open(localPath);
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000DD2C8 File Offset: 0x000DB4C8
	public void Open(string localPath)
	{
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000DD2CC File Offset: 0x000DB4CC
	public void Close()
	{
		this.binaryWriter.Close();
		this.binaryWriter = null;
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x000DD2E0 File Offset: 0x000DB4E0
	public void Write(bool value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000DD2F0 File Offset: 0x000DB4F0
	public void Write(byte value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x000DD300 File Offset: 0x000DB500
	public void Write(short value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x000DD310 File Offset: 0x000DB510
	public void Write(ushort value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000DD320 File Offset: 0x000DB520
	public void Write(int value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x000DD330 File Offset: 0x000DB530
	public void Write(uint value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x000DD340 File Offset: 0x000DB540
	public void Write(float value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x000DD350 File Offset: 0x000DB550
	public void Write(double value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000DD360 File Offset: 0x000DB560
	public void Write(Vector2 value)
	{
		this.Write(value.x);
		this.Write(value.y);
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000DD37C File Offset: 0x000DB57C
	public void Write(Vector3 value)
	{
		this.Write(value.x);
		this.Write(value.y);
		this.Write(value.z);
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000DD3A8 File Offset: 0x000DB5A8
	public void Write(AlignedBox value)
	{
		this.Write(value.xmin);
		this.Write(value.xmax);
		this.Write(value.ymin);
		this.Write(value.ymax);
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000DD3E8 File Offset: 0x000DB5E8
	public void Write(string value)
	{
		this.binaryWriter.Write(value);
	}

	// Token: 0x04001626 RID: 5670
	private System.IO.BinaryWriter binaryWriter;
}
