using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class SplineInterpolator
{
	// Token: 0x06002223 RID: 8739 RVA: 0x000D26D4 File Offset: 0x000D08D4
	public void Reset()
	{
		this.mNodes.Clear();
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000D26E4 File Offset: 0x000D08E4
	public void AddPoint(Vector3 pos, float timeInSeconds, Vector2 easeInOut)
	{
		this.mNodes.Add(new SplineInterpolator.SplineNode(pos, timeInSeconds, easeInOut));
		if (timeInSeconds > this.maxTime)
		{
			this.maxTime = timeInSeconds;
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000D2718 File Offset: 0x000D0918
	public static float Ease(float t, float k1, float k2)
	{
		float num = k1 * 2f / 3.1415927f + k2 - k1 + (1f - k2) * 2f / 3.1415927f;
		float num2;
		if (t < k1)
		{
			num2 = k1 * 0.63661975f * (Mathf.Sin(t / k1 * 3.1415927f / 2f - 1.5707964f) + 1f);
		}
		else if (t < k2)
		{
			num2 = 2f * k1 / 3.1415927f + t - k1;
		}
		else
		{
			num2 = 2f * k1 / 3.1415927f + k2 - k1 + (1f - k2) * 0.63661975f * Mathf.Sin((t - k2) / (1f - k2) * 3.1415927f / 2f);
		}
		return num2 / num;
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x000D27E0 File Offset: 0x000D09E0
	public void LoadData(string fname)
	{
		string streamingAssetsFile = TFUtils.GetStreamingAssetsFile(fname);
		string json = TFUtils.ReadAllText(streamingAssetsFile);
		Debug.Log(streamingAssetsFile);
		List<object> list = (List<object>)Json.Deserialize(json);
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			Vector3 pos;
			TFUtils.LoadVector3(out pos, (Dictionary<string, object>)dictionary["scale"]);
			Vector2 easeInOut;
			TFUtils.LoadVector2(out easeInOut, (Dictionary<string, object>)dictionary["easeIO"]);
			float timeInSeconds = TFUtils.LoadFloat(dictionary, "time");
			this.AddPoint(pos, timeInSeconds, easeInOut);
		}
	}

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06002227 RID: 8743 RVA: 0x000D28B0 File Offset: 0x000D0AB0
	public float MaxTime
	{
		get
		{
			return this.maxTime;
		}
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000D28B8 File Offset: 0x000D0AB8
	public Vector3 GetHermiteAtTime(float timeParam)
	{
		if (timeParam >= this.mNodes[this.mNodes.Count - 2].Time)
		{
			return this.mNodes[this.mNodes.Count - 2].Point;
		}
		int i;
		for (i = 1; i < this.mNodes.Count - 2; i++)
		{
			if (this.mNodes[i].Time > timeParam)
			{
				break;
			}
		}
		int num = i - 1;
		float num2 = (timeParam - this.mNodes[num].Time) / (this.mNodes[num + 1].Time - this.mNodes[num].Time);
		num2 = SplineInterpolator.Ease(num2, this.mNodes[num].EaseIO.x, this.mNodes[num].EaseIO.y);
		float num3 = num2;
		float num4 = num3 * num3;
		float num5 = num4 * num3;
		Vector3 b = (num <= 0) ? this.mNodes[num].Point : this.mNodes[num - 1].Point;
		Vector3 point = this.mNodes[num].Point;
		Vector3 point2 = this.mNodes[num + 1].Point;
		Vector3 point3 = this.mNodes[num + 2].Point;
		float d = 0.5f;
		Vector3 a = d * (point2 - b);
		Vector3 a2 = d * (point3 - point);
		float d2 = 2f * num5 - 3f * num4 + 1f;
		float d3 = -2f * num5 + 3f * num4;
		float d4 = num5 - 2f * num4 + num3;
		float d5 = num5 - num4;
		return d2 * point + d3 * point2 + d4 * a + d5 * a2;
	}

	// Token: 0x0400151C RID: 5404
	private float maxTime;

	// Token: 0x0400151D RID: 5405
	private List<SplineInterpolator.SplineNode> mNodes = new List<SplineInterpolator.SplineNode>();

	// Token: 0x02000452 RID: 1106
	internal class SplineNode
	{
		// Token: 0x06002229 RID: 8745 RVA: 0x000D2AD0 File Offset: 0x000D0CD0
		internal SplineNode(Vector3 p, float t, Vector2 io)
		{
			this.Point = p;
			this.Time = t;
			this.EaseIO = io;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000D2AF0 File Offset: 0x000D0CF0
		internal SplineNode(SplineInterpolator.SplineNode o)
		{
			this.Point = o.Point;
			this.Time = o.Time;
			this.EaseIO = o.EaseIO;
		}

		// Token: 0x0400151E RID: 5406
		internal Vector3 Point;

		// Token: 0x0400151F RID: 5407
		internal float Time;

		// Token: 0x04001520 RID: 5408
		internal Vector2 EaseIO;
	}
}
