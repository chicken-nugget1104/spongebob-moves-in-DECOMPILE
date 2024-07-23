using System;
using System.Collections.Generic;

// Token: 0x0200041E RID: 1054
public class Identity
{
	// Token: 0x060020A6 RID: 8358 RVA: 0x000CA260 File Offset: 0x000C8460
	public Identity()
	{
		this.value = Guid.NewGuid().ToString();
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x000CA288 File Offset: 0x000C8488
	public Identity(string value)
	{
		this.value = value;
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x000CA298 File Offset: 0x000C8498
	public Identity(Reader reader)
	{
		this.Unserialize(reader);
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x000CA2A8 File Offset: 0x000C84A8
	public void Unserialize(Reader reader)
	{
		reader.Read(out this.value);
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000CA2B8 File Offset: 0x000C84B8
	public void Serialize(Writer writer)
	{
		writer.Write(this.value);
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x000CA2C8 File Offset: 0x000C84C8
	public string Describe()
	{
		return this.value;
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x000CA2D0 File Offset: 0x000C84D0
	public static Identity Null()
	{
		return new Identity(Guid.Empty.ToString());
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x000CA2F0 File Offset: 0x000C84F0
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Identity identity = obj as Identity;
		return identity != null && this.value.Equals(identity.value);
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x000CA328 File Offset: 0x000C8528
	public override int GetHashCode()
	{
		return this.value.GetHashCode();
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x000CA338 File Offset: 0x000C8538
	public override string ToString()
	{
		return "Identity(guid=" + this.value + ")";
	}

	// Token: 0x040013F1 RID: 5105
	private string value;

	// Token: 0x0200041F RID: 1055
	public class Equality : IEqualityComparer<Identity>
	{
		// Token: 0x060020B1 RID: 8369 RVA: 0x000CA358 File Offset: 0x000C8558
		public bool Equals(Identity lhs, Identity rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000CA364 File Offset: 0x000C8564
		public int GetHashCode(Identity lhs)
		{
			return lhs.GetHashCode();
		}
	}
}
