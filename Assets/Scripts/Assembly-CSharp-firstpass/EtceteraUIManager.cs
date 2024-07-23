using System;
using System.IO;
using Prime31;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class EtceteraUIManager : MonoBehaviourGUI
{
	// Token: 0x06000395 RID: 917 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
	private void Start()
	{
		EtceteraAndroid.initTTS();
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0000F9F8 File Offset: 0x0000DBF8
	private void OnEnable()
	{
		EtceteraAndroidManager.albumChooserSucceededEvent += this.imageLoaded;
		EtceteraAndroidManager.photoChooserSucceededEvent += this.imageLoaded;
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0000FA28 File Offset: 0x0000DC28
	private void OnDisable()
	{
		EtceteraAndroidManager.albumChooserSucceededEvent -= this.imageLoaded;
		EtceteraAndroidManager.photoChooserSucceededEvent -= this.imageLoaded;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0000FA58 File Offset: 0x0000DC58
	private string saveScreenshotToSDCard()
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0, false);
		byte[] bytes = texture2D.EncodeToPNG();
		UnityEngine.Object.Destroy(texture2D);
		string text = Path.Combine(Application.persistentDataPath, "myImage.png");
		File.WriteAllBytes(text, bytes);
		return text;
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0000FAC0 File Offset: 0x0000DCC0
	private void OnGUI()
	{
		base.beginColumn();
		if (GUILayout.Button("Show Toast", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showToast("Hi.  Something just happened in the game and I want to tell you but not interrupt you", true);
		}
		if (GUILayout.Button("Play Video", new GUILayoutOption[0]))
		{
			EtceteraAndroid.playMovie("http://www.daily3gp.com/vids/747.3gp", 16711680U, false, EtceteraAndroid.ScalingMode.AspectFit, true);
		}
		if (GUILayout.Button("Show Alert", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showAlert("Alert Title Here", "Something just happened.  Do you want to have a snack?", "Yes", "Not Now");
		}
		if (GUILayout.Button("Single Field Prompt", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showAlertPrompt("Enter Digits", "I'll call you if you give me your number", "phone number", "867-5309", "Send", "Not a Chance");
		}
		if (GUILayout.Button("Two Field Prompt", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showAlertPromptWithTwoFields("Need Info", "Enter your credentials:", "username", "harry_potter", "password", string.Empty, "OK", "Cancel");
		}
		if (GUILayout.Button("Show Progress Dialog", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showProgressDialog("Progress is happening", "it will be over in just a second...");
			base.Invoke("hideProgress", 1f);
		}
		if (GUILayout.Button("Text to Speech Speak", new GUILayoutOption[0]))
		{
			EtceteraAndroid.setPitch((float)UnityEngine.Random.Range(0, 5));
			EtceteraAndroid.setSpeechRate(UnityEngine.Random.Range(0.5f, 1.5f));
			EtceteraAndroid.speak("Howdy. Im a robot voice");
		}
		if (GUILayout.Button("Prompt for Video", new GUILayoutOption[0]))
		{
			EtceteraAndroid.promptToTakeVideo("fancyVideo");
		}
		base.endColumn(true);
		if (GUILayout.Button("Show Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showWebView("http://prime31.com");
		}
		if (GUILayout.Button("Email Composer", new GUILayoutOption[0]))
		{
			string attachmentFilePath = this.saveScreenshotToSDCard();
			EtceteraAndroid.showEmailComposer("noone@nothing.com", "Message subject", "click <a href='http://somelink.com'>here</a> for a present", true, attachmentFilePath);
		}
		if (GUILayout.Button("SMS Composer", new GUILayoutOption[0]))
		{
			EtceteraAndroid.showSMSComposer("I did something really cool in this game!");
		}
		if (GUILayout.Button("Prompt to Take Photo", new GUILayoutOption[0]))
		{
			EtceteraAndroid.promptToTakePhoto("photo.jpg");
		}
		if (GUILayout.Button("Prompt for Album Image", new GUILayoutOption[0]))
		{
			EtceteraAndroid.promptForPictureFromAlbum("albumImage.jpg");
		}
		if (GUILayout.Button("Save Image to Gallery", new GUILayoutOption[0]))
		{
			string pathToPhoto = this.saveScreenshotToSDCard();
			bool flag = EtceteraAndroid.saveImageToGallery(pathToPhoto, "My image from Unity");
			Debug.Log("did save to gallery: " + flag);
		}
		if (GUILayout.Button("Ask For Review", new GUILayoutOption[0]))
		{
			EtceteraAndroid.resetAskForReview();
			EtceteraAndroid.askForReviewNow("Please rate my app!", "It will really make me happy if you do...", false);
		}
		base.endColumn();
		if (base.bottomRightButton("Next Scene", 150f))
		{
			Application.LoadLevel("EtceteraTestSceneTwo");
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0000FD98 File Offset: 0x0000DF98
	private void hideProgress()
	{
		EtceteraAndroid.hideProgressDialog();
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0000FDA0 File Offset: 0x0000DFA0
	public void imageLoaded(string imagePath)
	{
		EtceteraAndroid.scaleImageAtPath(imagePath, 0.1f);
		this.testPlane.renderer.material.mainTexture = EtceteraAndroid.textureFromFileAtPath(imagePath);
	}

	// Token: 0x04000206 RID: 518
	public GameObject testPlane;
}
