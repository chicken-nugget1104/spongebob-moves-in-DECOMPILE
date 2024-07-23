using System;
using System.IO;
using Prime31;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class EtceteraAndroidManager : AbstractManager
{
	// Token: 0x06000341 RID: 833 RVA: 0x0000EF44 File Offset: 0x0000D144
	static EtceteraAndroidManager()
	{
		AbstractManager.initialize(typeof(EtceteraAndroidManager));
	}

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000342 RID: 834 RVA: 0x0000EF58 File Offset: 0x0000D158
	// (remove) Token: 0x06000343 RID: 835 RVA: 0x0000EF70 File Offset: 0x0000D170
	public static event Action<string> alertButtonClickedEvent;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000344 RID: 836 RVA: 0x0000EF88 File Offset: 0x0000D188
	// (remove) Token: 0x06000345 RID: 837 RVA: 0x0000EFA0 File Offset: 0x0000D1A0
	public static event Action alertCancelledEvent;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000346 RID: 838 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
	// (remove) Token: 0x06000347 RID: 839 RVA: 0x0000EFD0 File Offset: 0x0000D1D0
	public static event Action<string> promptFinishedWithTextEvent;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000348 RID: 840 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
	// (remove) Token: 0x06000349 RID: 841 RVA: 0x0000F000 File Offset: 0x0000D200
	public static event Action promptCancelledEvent;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x0600034A RID: 842 RVA: 0x0000F018 File Offset: 0x0000D218
	// (remove) Token: 0x0600034B RID: 843 RVA: 0x0000F030 File Offset: 0x0000D230
	public static event Action<string, string> twoFieldPromptFinishedWithTextEvent;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x0600034C RID: 844 RVA: 0x0000F048 File Offset: 0x0000D248
	// (remove) Token: 0x0600034D RID: 845 RVA: 0x0000F060 File Offset: 0x0000D260
	public static event Action twoFieldPromptCancelledEvent;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x0600034E RID: 846 RVA: 0x0000F078 File Offset: 0x0000D278
	// (remove) Token: 0x0600034F RID: 847 RVA: 0x0000F090 File Offset: 0x0000D290
	public static event Action webViewCancelledEvent;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000350 RID: 848 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
	// (remove) Token: 0x06000351 RID: 849 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
	public static event Action albumChooserCancelledEvent;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06000352 RID: 850 RVA: 0x0000F0D8 File Offset: 0x0000D2D8
	// (remove) Token: 0x06000353 RID: 851 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
	public static event Action<string> albumChooserSucceededEvent;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000354 RID: 852 RVA: 0x0000F108 File Offset: 0x0000D308
	// (remove) Token: 0x06000355 RID: 853 RVA: 0x0000F120 File Offset: 0x0000D320
	public static event Action photoChooserCancelledEvent;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000356 RID: 854 RVA: 0x0000F138 File Offset: 0x0000D338
	// (remove) Token: 0x06000357 RID: 855 RVA: 0x0000F150 File Offset: 0x0000D350
	public static event Action<string> photoChooserSucceededEvent;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06000358 RID: 856 RVA: 0x0000F168 File Offset: 0x0000D368
	// (remove) Token: 0x06000359 RID: 857 RVA: 0x0000F180 File Offset: 0x0000D380
	public static event Action<string> videoRecordingSucceededEvent;

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x0600035A RID: 858 RVA: 0x0000F198 File Offset: 0x0000D398
	// (remove) Token: 0x0600035B RID: 859 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
	public static event Action videoRecordingCancelledEvent;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x0600035C RID: 860 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
	// (remove) Token: 0x0600035D RID: 861 RVA: 0x0000F1E0 File Offset: 0x0000D3E0
	public static event Action ttsInitializedEvent;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x0600035E RID: 862 RVA: 0x0000F1F8 File Offset: 0x0000D3F8
	// (remove) Token: 0x0600035F RID: 863 RVA: 0x0000F210 File Offset: 0x0000D410
	public static event Action ttsFailedToInitializeEvent;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06000360 RID: 864 RVA: 0x0000F228 File Offset: 0x0000D428
	// (remove) Token: 0x06000361 RID: 865 RVA: 0x0000F240 File Offset: 0x0000D440
	public static event Action askForReviewWillOpenMarketEvent;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06000362 RID: 866 RVA: 0x0000F258 File Offset: 0x0000D458
	// (remove) Token: 0x06000363 RID: 867 RVA: 0x0000F270 File Offset: 0x0000D470
	public static event Action askForReviewRemindMeLaterEvent;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06000364 RID: 868 RVA: 0x0000F288 File Offset: 0x0000D488
	// (remove) Token: 0x06000365 RID: 869 RVA: 0x0000F2A0 File Offset: 0x0000D4A0
	public static event Action askForReviewDontAskAgainEvent;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000366 RID: 870 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
	// (remove) Token: 0x06000367 RID: 871 RVA: 0x0000F2D0 File Offset: 0x0000D4D0
	public static event Action<string> inlineWebViewJSCallbackEvent;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000368 RID: 872 RVA: 0x0000F2E8 File Offset: 0x0000D4E8
	// (remove) Token: 0x06000369 RID: 873 RVA: 0x0000F300 File Offset: 0x0000D500
	public static event Action<string> notificationReceivedEvent;

	// Token: 0x0600036A RID: 874 RVA: 0x0000F318 File Offset: 0x0000D518
	public void alertButtonClicked(string positiveButton)
	{
		if (EtceteraAndroidManager.alertButtonClickedEvent != null)
		{
			EtceteraAndroidManager.alertButtonClickedEvent(positiveButton);
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0000F330 File Offset: 0x0000D530
	public void alertCancelled(string empty)
	{
		if (EtceteraAndroidManager.alertCancelledEvent != null)
		{
			EtceteraAndroidManager.alertCancelledEvent();
		}
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0000F348 File Offset: 0x0000D548
	public void promptFinishedWithText(string text)
	{
		string[] array = text.Split(new string[]
		{
			"|||"
		}, StringSplitOptions.None);
		if (array.Length == 1 && EtceteraAndroidManager.promptFinishedWithTextEvent != null)
		{
			EtceteraAndroidManager.promptFinishedWithTextEvent(array[0]);
		}
		if (array.Length == 2 && EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent != null)
		{
			EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent(array[0], array[1]);
		}
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0000F3B0 File Offset: 0x0000D5B0
	public void promptCancelled(string empty)
	{
		if (EtceteraAndroidManager.promptCancelledEvent != null)
		{
			EtceteraAndroidManager.promptCancelledEvent();
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0000F3C8 File Offset: 0x0000D5C8
	public void twoFieldPromptCancelled(string empty)
	{
		if (EtceteraAndroidManager.twoFieldPromptCancelledEvent != null)
		{
			EtceteraAndroidManager.twoFieldPromptCancelledEvent();
		}
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
	public void webViewCancelled(string empty)
	{
		if (EtceteraAndroidManager.webViewCancelledEvent != null)
		{
			EtceteraAndroidManager.webViewCancelledEvent();
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0000F3F8 File Offset: 0x0000D5F8
	public void albumChooserCancelled(string empty)
	{
		if (EtceteraAndroidManager.albumChooserCancelledEvent != null)
		{
			EtceteraAndroidManager.albumChooserCancelledEvent();
		}
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0000F410 File Offset: 0x0000D610
	public void albumChooserSucceeded(string path)
	{
		if (EtceteraAndroidManager.albumChooserSucceededEvent != null)
		{
			if (File.Exists(path))
			{
				EtceteraAndroidManager.albumChooserSucceededEvent(path);
			}
			else if (EtceteraAndroidManager.albumChooserCancelledEvent != null)
			{
				EtceteraAndroidManager.albumChooserCancelledEvent();
			}
		}
	}

	// Token: 0x06000372 RID: 882 RVA: 0x0000F44C File Offset: 0x0000D64C
	public void photoChooserCancelled(string empty)
	{
		if (EtceteraAndroidManager.photoChooserCancelledEvent != null)
		{
			EtceteraAndroidManager.photoChooserCancelledEvent();
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0000F464 File Offset: 0x0000D664
	public void photoChooserSucceeded(string path)
	{
		if (EtceteraAndroidManager.photoChooserSucceededEvent != null)
		{
			if (File.Exists(path))
			{
				EtceteraAndroidManager.photoChooserSucceededEvent(path);
			}
			else if (EtceteraAndroidManager.photoChooserCancelledEvent != null)
			{
				EtceteraAndroidManager.photoChooserCancelledEvent();
			}
		}
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0000F4A0 File Offset: 0x0000D6A0
	public void videoRecordingSucceeded(string path)
	{
		if (EtceteraAndroidManager.videoRecordingSucceededEvent != null)
		{
			EtceteraAndroidManager.videoRecordingSucceededEvent(path);
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
	public void videoRecordingCancelled(string empty)
	{
		if (EtceteraAndroidManager.videoRecordingCancelledEvent != null)
		{
			EtceteraAndroidManager.videoRecordingCancelledEvent();
		}
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
	public void ttsInitialized(string result)
	{
		bool flag = result == "1";
		if (flag && EtceteraAndroidManager.ttsInitializedEvent != null)
		{
			EtceteraAndroidManager.ttsInitializedEvent();
		}
		if (!flag && EtceteraAndroidManager.ttsFailedToInitializeEvent != null)
		{
			EtceteraAndroidManager.ttsFailedToInitializeEvent();
		}
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0000F520 File Offset: 0x0000D720
	public void ttsUtteranceCompleted(string utteranceId)
	{
		Debug.Log("utterance completed: " + utteranceId);
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0000F534 File Offset: 0x0000D734
	public void askForReviewWillOpenMarket(string empty)
	{
		if (EtceteraAndroidManager.askForReviewWillOpenMarketEvent != null)
		{
			EtceteraAndroidManager.askForReviewWillOpenMarketEvent();
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0000F54C File Offset: 0x0000D74C
	public void askForReviewRemindMeLater(string empty)
	{
		if (EtceteraAndroidManager.askForReviewRemindMeLaterEvent != null)
		{
			EtceteraAndroidManager.askForReviewRemindMeLaterEvent();
		}
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0000F564 File Offset: 0x0000D764
	public void askForReviewDontAskAgain(string empty)
	{
		if (EtceteraAndroidManager.askForReviewDontAskAgainEvent != null)
		{
			EtceteraAndroidManager.askForReviewDontAskAgainEvent();
		}
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0000F57C File Offset: 0x0000D77C
	public void inlineWebViewJSCallback(string message)
	{
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent.fire(message);
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0000F58C File Offset: 0x0000D78C
	public void notificationReceived(string extraData)
	{
		EtceteraAndroidManager.notificationReceivedEvent.fire(extraData);
	}
}
