using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class EtceteraAndroidEventListener : MonoBehaviour
{
	// Token: 0x0600037E RID: 894 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
	private void OnEnable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent += this.alertButtonClickedEvent;
		EtceteraAndroidManager.alertCancelledEvent += this.alertCancelledEvent;
		EtceteraAndroidManager.promptFinishedWithTextEvent += this.promptFinishedWithTextEvent;
		EtceteraAndroidManager.promptCancelledEvent += this.promptCancelledEvent;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent += this.twoFieldPromptFinishedWithTextEvent;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent += this.twoFieldPromptCancelledEvent;
		EtceteraAndroidManager.webViewCancelledEvent += this.webViewCancelledEvent;
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent += this.inlineWebViewJSCallbackEvent;
		EtceteraAndroidManager.albumChooserCancelledEvent += this.albumChooserCancelledEvent;
		EtceteraAndroidManager.albumChooserSucceededEvent += this.albumChooserSucceededEvent;
		EtceteraAndroidManager.photoChooserCancelledEvent += this.photoChooserCancelledEvent;
		EtceteraAndroidManager.photoChooserSucceededEvent += this.photoChooserSucceededEvent;
		EtceteraAndroidManager.videoRecordingCancelledEvent += this.videoRecordingCancelledEvent;
		EtceteraAndroidManager.videoRecordingSucceededEvent += this.videoRecordingSucceededEvent;
		EtceteraAndroidManager.ttsInitializedEvent += this.ttsInitializedEvent;
		EtceteraAndroidManager.ttsFailedToInitializeEvent += this.ttsFailedToInitializeEvent;
		EtceteraAndroidManager.askForReviewDontAskAgainEvent += this.askForReviewDontAskAgainEvent;
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent += this.askForReviewRemindMeLaterEvent;
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent += this.askForReviewWillOpenMarketEvent;
		EtceteraAndroidManager.notificationReceivedEvent += this.notificationReceivedEvent;
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0000F708 File Offset: 0x0000D908
	private void OnDisable()
	{
		EtceteraAndroidManager.alertButtonClickedEvent -= this.alertButtonClickedEvent;
		EtceteraAndroidManager.alertCancelledEvent -= this.alertCancelledEvent;
		EtceteraAndroidManager.promptFinishedWithTextEvent -= this.promptFinishedWithTextEvent;
		EtceteraAndroidManager.promptCancelledEvent -= this.promptCancelledEvent;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent -= this.twoFieldPromptFinishedWithTextEvent;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent -= this.twoFieldPromptCancelledEvent;
		EtceteraAndroidManager.webViewCancelledEvent -= this.webViewCancelledEvent;
		EtceteraAndroidManager.inlineWebViewJSCallbackEvent -= this.inlineWebViewJSCallbackEvent;
		EtceteraAndroidManager.albumChooserCancelledEvent -= this.albumChooserCancelledEvent;
		EtceteraAndroidManager.albumChooserSucceededEvent -= this.albumChooserSucceededEvent;
		EtceteraAndroidManager.photoChooserCancelledEvent -= this.photoChooserCancelledEvent;
		EtceteraAndroidManager.photoChooserSucceededEvent -= this.photoChooserSucceededEvent;
		EtceteraAndroidManager.videoRecordingCancelledEvent -= this.videoRecordingCancelledEvent;
		EtceteraAndroidManager.videoRecordingSucceededEvent -= this.videoRecordingSucceededEvent;
		EtceteraAndroidManager.ttsInitializedEvent -= this.ttsInitializedEvent;
		EtceteraAndroidManager.ttsFailedToInitializeEvent -= this.ttsFailedToInitializeEvent;
		EtceteraAndroidManager.askForReviewDontAskAgainEvent -= this.askForReviewDontAskAgainEvent;
		EtceteraAndroidManager.askForReviewRemindMeLaterEvent -= this.askForReviewRemindMeLaterEvent;
		EtceteraAndroidManager.askForReviewWillOpenMarketEvent -= this.askForReviewWillOpenMarketEvent;
		EtceteraAndroidManager.notificationReceivedEvent -= this.notificationReceivedEvent;
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0000F86C File Offset: 0x0000DA6C
	private void alertButtonClickedEvent(string positiveButton)
	{
		Debug.Log("alertButtonClickedEvent: " + positiveButton);
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0000F880 File Offset: 0x0000DA80
	private void alertCancelledEvent()
	{
		Debug.Log("alertCancelledEvent");
	}

	// Token: 0x06000382 RID: 898 RVA: 0x0000F88C File Offset: 0x0000DA8C
	private void promptFinishedWithTextEvent(string param)
	{
		Debug.Log("promptFinishedWithTextEvent: " + param);
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
	private void promptCancelledEvent()
	{
		Debug.Log("promptCancelledEvent");
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0000F8AC File Offset: 0x0000DAAC
	private void twoFieldPromptFinishedWithTextEvent(string text1, string text2)
	{
		Debug.Log("twoFieldPromptFinishedWithTextEvent: " + text1 + ", " + text2);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x0000F8C4 File Offset: 0x0000DAC4
	private void twoFieldPromptCancelledEvent()
	{
		Debug.Log("twoFieldPromptCancelledEvent");
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0000F8D0 File Offset: 0x0000DAD0
	private void webViewCancelledEvent()
	{
		Debug.Log("webViewCancelledEvent");
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0000F8DC File Offset: 0x0000DADC
	private void inlineWebViewJSCallbackEvent(string message)
	{
		Debug.Log("inlineWebViewJSCallbackEvent: " + message);
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
	private void albumChooserCancelledEvent()
	{
		Debug.Log("albumChooserCancelledEvent");
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0000F8FC File Offset: 0x0000DAFC
	private void albumChooserSucceededEvent(string imagePath)
	{
		Debug.Log("albumChooserSucceededEvent: " + imagePath);
		Debug.Log("image size: " + EtceteraAndroid.getImageSizeAtPath(imagePath));
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0000F934 File Offset: 0x0000DB34
	private void photoChooserCancelledEvent()
	{
		Debug.Log("photoChooserCancelledEvent");
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0000F940 File Offset: 0x0000DB40
	private void photoChooserSucceededEvent(string imagePath)
	{
		Debug.Log("photoChooserSucceededEvent: " + imagePath);
		Debug.Log("image size: " + EtceteraAndroid.getImageSizeAtPath(imagePath));
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0000F978 File Offset: 0x0000DB78
	private void videoRecordingCancelledEvent()
	{
		Debug.Log("videoRecordingCancelledEvent");
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0000F984 File Offset: 0x0000DB84
	private void videoRecordingSucceededEvent(string path)
	{
		Debug.Log("videoRecordingSucceededEvent: " + path);
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0000F998 File Offset: 0x0000DB98
	private void ttsInitializedEvent()
	{
		Debug.Log("ttsInitializedEvent");
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
	private void ttsFailedToInitializeEvent()
	{
		Debug.Log("ttsFailedToInitializeEvent");
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
	private void askForReviewDontAskAgainEvent()
	{
		Debug.Log("askForReviewDontAskAgainEvent");
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0000F9BC File Offset: 0x0000DBBC
	private void askForReviewRemindMeLaterEvent()
	{
		Debug.Log("askForReviewRemindMeLaterEvent");
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
	private void askForReviewWillOpenMarketEvent()
	{
		Debug.Log("askForReviewWillOpenMarketEvent");
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
	private void notificationReceivedEvent(string extraData)
	{
		Debug.Log("notificationReceivedEvent: " + extraData);
	}
}
