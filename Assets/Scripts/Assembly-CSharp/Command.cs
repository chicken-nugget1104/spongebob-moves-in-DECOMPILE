using System;
using System.Collections.Generic;

// Token: 0x02000277 RID: 631
public class Command
{
	// Token: 0x06001449 RID: 5193 RVA: 0x0008B96C File Offset: 0x00089B6C
	public Command(Command.TYPE type, Identity sender, Identity receiver)
	{
		this.type = type;
		this.sender = sender;
		this.receiver = receiver;
		this.timeEpoch = TFUtils.EpochTime();
		this.properties = new Dictionary<string, object>();
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x0600144A RID: 5194 RVA: 0x0008B9A0 File Offset: 0x00089BA0
	public Command.TYPE Type
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x0600144B RID: 5195 RVA: 0x0008B9A8 File Offset: 0x00089BA8
	public Identity Sender
	{
		get
		{
			return this.sender;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x0600144C RID: 5196 RVA: 0x0008B9B0 File Offset: 0x00089BB0
	public Identity Receiver
	{
		get
		{
			return this.receiver;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x0600144D RID: 5197 RVA: 0x0008B9B8 File Offset: 0x00089BB8
	// (set) Token: 0x0600144E RID: 5198 RVA: 0x0008B9C0 File Offset: 0x00089BC0
	public ulong TimeEpoch
	{
		get
		{
			return this.timeEpoch;
		}
		set
		{
			this.timeEpoch = value;
		}
	}

	// Token: 0x170002C8 RID: 712
	// (set) Token: 0x0600144F RID: 5199 RVA: 0x0008B9CC File Offset: 0x00089BCC
	public Action OnComplete
	{
		set
		{
			this.onComplete = value;
		}
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x0008B9D8 File Offset: 0x00089BD8
	public bool HasProperty(string property)
	{
		return this.properties.ContainsKey(property);
	}

	// Token: 0x170002C9 RID: 713
	public object this[string property]
	{
		get
		{
			return this.properties[property];
		}
		set
		{
			this.properties[property] = value;
		}
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x0008BA08 File Offset: 0x00089C08
	public void TryExecuteOnComplete()
	{
		if (this.onComplete != null)
		{
			this.onComplete();
		}
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x0008BA20 File Offset: 0x00089C20
	public string Describe()
	{
		return string.Concat(new string[]
		{
			"Command(",
			this.sender.Describe(),
			",",
			this.receiver.Describe(),
			"):",
			this.type.ToString()
		});
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x0008BA80 File Offset: 0x00089C80
	public bool Match(Dictionary<string, object> matching)
	{
		object obj = null;
		foreach (KeyValuePair<string, object> keyValuePair in matching)
		{
			if (!this.properties.TryGetValue(keyValuePair.Key, out obj))
			{
				return false;
			}
			if (obj.ToString() != keyValuePair.Value.ToString())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04000E44 RID: 3652
	public const string PRODUCT_ID = "product_id";

	// Token: 0x04000E45 RID: 3653
	public const string SLOT_ID = "slot_id";

	// Token: 0x04000E46 RID: 3654
	private Command.TYPE type;

	// Token: 0x04000E47 RID: 3655
	private Identity sender;

	// Token: 0x04000E48 RID: 3656
	private Identity receiver;

	// Token: 0x04000E49 RID: 3657
	private Action onComplete;

	// Token: 0x04000E4A RID: 3658
	private ulong timeEpoch;

	// Token: 0x04000E4B RID: 3659
	private Dictionary<string, object> properties;

	// Token: 0x02000278 RID: 632
	public enum TYPE
	{
		// Token: 0x04000E4D RID: 3661
		MOVE,
		// Token: 0x04000E4E RID: 3662
		FLIP,
		// Token: 0x04000E4F RID: 3663
		RETURN,
		// Token: 0x04000E50 RID: 3664
		RUSH,
		// Token: 0x04000E51 RID: 3665
		EMPLOY,
		// Token: 0x04000E52 RID: 3666
		ERECT,
		// Token: 0x04000E53 RID: 3667
		RESIDE,
		// Token: 0x04000E54 RID: 3668
		PRODUCE,
		// Token: 0x04000E55 RID: 3669
		ADVANCE,
		// Token: 0x04000E56 RID: 3670
		COMPLETE,
		// Token: 0x04000E57 RID: 3671
		BONUS,
		// Token: 0x04000E58 RID: 3672
		EXPIRE,
		// Token: 0x04000E59 RID: 3673
		CLICKED,
		// Token: 0x04000E5A RID: 3674
		DELEGATE_CLICK,
		// Token: 0x04000E5B RID: 3675
		ABORT,
		// Token: 0x04000E5C RID: 3676
		WANDER,
		// Token: 0x04000E5D RID: 3677
		HUNGER,
		// Token: 0x04000E5E RID: 3678
		WISH,
		// Token: 0x04000E5F RID: 3679
		TEMPT,
		// Token: 0x04000E60 RID: 3680
		FEED,
		// Token: 0x04000E61 RID: 3681
		SPAWN,
		// Token: 0x04000E62 RID: 3682
		PERFORM_TASK,
		// Token: 0x04000E63 RID: 3683
		COLLECT_TASK_REWARD,
		// Token: 0x04000E64 RID: 3684
		CRAFT,
		// Token: 0x04000E65 RID: 3685
		CRAFTED,
		// Token: 0x04000E66 RID: 3686
		CLEAR,
		// Token: 0x04000E67 RID: 3687
		WAIT_AS_TASK_TARGET,
		// Token: 0x04000E68 RID: 3688
		PURCHASE,
		// Token: 0x04000E69 RID: 3689
		ACTIVATE,
		// Token: 0x04000E6A RID: 3690
		EXPAND,
		// Token: 0x04000E6B RID: 3691
		HUBCRAFT,
		// Token: 0x04000E6C RID: 3692
		IDLE_PAUSE,
		// Token: 0x04000E6D RID: 3693
		RESUME_FULL,
		// Token: 0x04000E6E RID: 3694
		RESUME_WISHING,
		// Token: 0x04000E6F RID: 3695
		GO_HOME,
		// Token: 0x04000E70 RID: 3696
		STORE_RESIDENT,
		// Token: 0x04000E71 RID: 3697
		FLEE,
		// Token: 0x04000E72 RID: 3698
		CHEER,
		// Token: 0x04000E73 RID: 3699
		TASK,
		// Token: 0x04000E74 RID: 3700
		ENTER,
		// Token: 0x04000E75 RID: 3701
		STAND,
		// Token: 0x04000E76 RID: 3702
		BONUS_REWARD,
		// Token: 0x04000E77 RID: 3703
		RUSH_TASK
	}
}
