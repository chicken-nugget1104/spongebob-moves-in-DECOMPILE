using System;
using System.IO;
using UnityEngine;

namespace MTools
{
	// Token: 0x020003B3 RID: 947
	public class MBinaryWriter : MWriter
	{
		// Token: 0x06001B8B RID: 7051 RVA: 0x000B24A0 File Offset: 0x000B06A0
		public MBinaryWriter(string filename)
		{
			this.Open(filename, false);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000B24B4 File Offset: 0x000B06B4
		public MBinaryWriter()
		{
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000B24BC File Offset: 0x000B06BC
		~MBinaryWriter()
		{
			this.Close();
			this.stream = null;
			this.writer = null;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000B2508 File Offset: 0x000B0708
		public override bool Open(string filename)
		{
			return this.Open(filename, false, false);
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000B2514 File Offset: 0x000B0714
		public override bool Open(string filename, bool deleteExisting)
		{
			return this.Open(filename, deleteExisting, false);
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000B2520 File Offset: 0x000B0720
		public override bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return this.Open(filename, deleteExisting, createDirectory, null);
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000B252C File Offset: 0x000B072C
		public override bool Open(string filename, bool deleteExisting, bool createDirectory, string backupExt)
		{
			if (!this.mIsOpen)
			{
				try
				{
					if (!string.IsNullOrEmpty(backupExt))
					{
						string text = filename + "." + backupExt;
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						if (File.Exists(filename))
						{
							File.Move(filename, text);
						}
					}
					if (deleteExisting && File.Exists(filename))
					{
						File.Delete(filename);
					}
					if (createDirectory)
					{
						string directoryName = Path.GetDirectoryName(filename);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
					}
					if (File.Exists(filename))
					{
						this.stream = File.OpenWrite(filename);
					}
					else
					{
						this.stream = File.Create(filename);
					}
					this.writer = new System.IO.BinaryWriter(this.stream);
					this.mIsOpen = true;
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message + "\n" + ex.StackTrace);
					this.mIsOpen = false;
				}
			}
			return this.mIsOpen;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000B2648 File Offset: 0x000B0848
		public override bool IsOpen()
		{
			return this.mIsOpen;
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000B2650 File Offset: 0x000B0850
		public override void Close()
		{
			if (this.mIsOpen)
			{
				try
				{
					if (this.writer != null)
					{
						this.writer.Close();
					}
					if (this.stream != null)
					{
						this.stream.Close();
					}
					this.writer = null;
					this.stream = null;
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
				}
			}
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000B26D4 File Offset: 0x000B08D4
		public string GetFilePath()
		{
			return this.FilePath;
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x000B26DC File Offset: 0x000B08DC
		public override void Write(byte val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000B26EC File Offset: 0x000B08EC
		public override void Write(char val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000B26FC File Offset: 0x000B08FC
		public override void Write(ushort val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000B270C File Offset: 0x000B090C
		public override void Write(short val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000B271C File Offset: 0x000B091C
		public override void Write(uint val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000B272C File Offset: 0x000B092C
		public override void Write(int val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000B273C File Offset: 0x000B093C
		public override void Write(ulong val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x000B274C File Offset: 0x000B094C
		public override void Write(long val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000B275C File Offset: 0x000B095C
		public override void Write(sbyte val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000B276C File Offset: 0x000B096C
		public override void Write(float val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000B277C File Offset: 0x000B097C
		public override void Write(double val)
		{
			this.writer.Write(val);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000B278C File Offset: 0x000B098C
		public override void Write(string val)
		{
			this.WriteCharArrayAsString(val);
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000B2798 File Offset: 0x000B0998
		public override void Write(char[] arry)
		{
			this.writer.Write(arry);
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000B27A8 File Offset: 0x000B09A8
		public override void Write(char[] arry, int length)
		{
			this.writer.Write(arry, 0, length);
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x000B27B8 File Offset: 0x000B09B8
		public void Write(byte[] arry)
		{
			this.writer.Write(arry);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x000B27C8 File Offset: 0x000B09C8
		public override void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x000B27D8 File Offset: 0x000B09D8
		public override void WriteCharArrayAsString(string str)
		{
			ushort num = 0;
			if (str != null)
			{
				num = (ushort)str.Length;
			}
			this.writer.Write(num);
			for (int i = 0; i < (int)num; i++)
			{
				this.writer.Write((byte)str[i]);
			}
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x000B2828 File Offset: 0x000B0A28
		public override void WriteRawString(string str)
		{
			if (str == null)
			{
				return;
			}
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				this.writer.Write((byte)str[i]);
			}
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000B2868 File Offset: 0x000B0A68
		public override void Seek(int offset)
		{
			this.writer.BaseStream.Seek((long)offset, SeekOrigin.Begin);
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x000B2880 File Offset: 0x000B0A80
		public override int Pos()
		{
			return (int)this.writer.BaseStream.Position;
		}

		// Token: 0x0400121C RID: 4636
		private string FilePath;

		// Token: 0x0400121D RID: 4637
		private bool mIsOpen;

		// Token: 0x0400121E RID: 4638
		private FileStream stream;

		// Token: 0x0400121F RID: 4639
		private System.IO.BinaryWriter writer;
	}
}
