using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class TFParticleController
{
	// Token: 0x0600226F RID: 8815 RVA: 0x000D39F0 File Offset: 0x000D1BF0
	public TFParticleController()
	{
		this.gameObject = UnityGameResources.CreateEmpty("TFParticle");
		this.particleSystem = this.gameObject.AddComponent<ParticleSystem>();
		this.particleSystem.startSize = 20f;
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x000D3A2C File Offset: 0x000D1C2C
	public void Destroy()
	{
		UnityGameResources.Destroy(this.gameObject);
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06002271 RID: 8817 RVA: 0x000D3A3C File Offset: 0x000D1C3C
	// (set) Token: 0x06002272 RID: 8818 RVA: 0x000D3A50 File Offset: 0x000D1C50
	public Vector3 Position
	{
		get
		{
			return this.gameObject.transform.position;
		}
		set
		{
			this.gameObject.transform.position = value;
		}
	}

	// Token: 0x04001543 RID: 5443
	private GameObject gameObject;

	// Token: 0x04001544 RID: 5444
	private ParticleSystem particleSystem;
}
