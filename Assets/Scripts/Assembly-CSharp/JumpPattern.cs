using System;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class JumpPattern : PeriodicPattern
{
	// Token: 0x06002149 RID: 8521 RVA: 0x000CE1D4 File Offset: 0x000CC3D4
	public JumpPattern(float gravity, float height) : this(gravity, height, 0f, 1f, 0f, 0f, Vector2.one)
	{
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x000CE204 File Offset: 0x000CC404
	public JumpPattern(float gravity, float height, float collisionStickTime, float squishFactor, float percentOffset, float now, Vector2 inStartScale)
	{
		this.a = gravity / 2f;
		this.period = Mathf.Sqrt(-4f * height / this.a);
		this.b = -this.a * this.period;
		this.c = 0f;
		this.maximum = height;
		this.minimum = 0f;
		this.timeOffset = this.period * percentOffset - now;
		this.collisionStickTime = collisionStickTime;
		this.startScale = inStartScale;
		if (TFPerfUtils.IsNonScalingDevice())
		{
			this.squisher = new ConstantPattern(1f);
		}
		else
		{
			this.squisher = new Sinusoid(0f, squishFactor, collisionStickTime, 0f);
		}
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000CE2C8 File Offset: 0x000CC4C8
	public override float ValueAtTime(float atTime)
	{
		float result;
		Vector2 vector;
		this.ValueAndSquishAtTime(atTime, out result, out vector);
		return result;
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000CE2E4 File Offset: 0x000CC4E4
	public void ValueAndSquishAtTime(float atTime, out float val, out Vector2 squish)
	{
		float num = (atTime + this.timeOffset) % (this.period + this.collisionStickTime);
		squish = this.startScale;
		if (num > this.period)
		{
			if (!TFPerfUtils.IsNonScalingDevice())
			{
				float atTime2 = num - this.period;
				float num2 = this.squisher.ValueAtTime(atTime2);
				squish = new Vector2(this.startScale.x + num2 * 0.75f, this.startScale.y - num2);
			}
			num = 0f;
		}
		val = TFMath.Quadratic(this.a, this.b, this.c, num);
	}

	// Token: 0x0400149D RID: 5277
	private float a;

	// Token: 0x0400149E RID: 5278
	private float b;

	// Token: 0x0400149F RID: 5279
	private float c;

	// Token: 0x040014A0 RID: 5280
	private float collisionStickTime;

	// Token: 0x040014A1 RID: 5281
	private PeriodicPattern squisher;

	// Token: 0x040014A2 RID: 5282
	private Vector2 startScale;
}
