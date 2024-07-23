using System;
using System.IO;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class EtceteraAndroid
{
	// Token: 0x06000316 RID: 790 RVA: 0x0000E54C File Offset: 0x0000C74C
	static EtceteraAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.EtceteraPlugin"))
		{
			EtceteraAndroid._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0000E5B8 File Offset: 0x0000C7B8
	public static Texture2D textureFromFileAtPath(string filePath)
	{
		byte[] data = File.ReadAllBytes(filePath);
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(data);
		texture2D.Apply();
		Debug.Log(string.Concat(new object[]
		{
			"texture size: ",
			texture2D.width,
			"x",
			texture2D.height
		}));
		return texture2D;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0000E620 File Offset: 0x0000C820
	public static void setSystemUiVisibilityToLowProfile(bool useLowProfile)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("setSystemUiVisibilityToLowProfile", new object[]
		{
			useLowProfile
		});
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0000E650 File Offset: 0x0000C850
	public static void playMovie(string pathOrUrl, uint bgColor, bool showControls, EtceteraAndroid.ScalingMode scalingMode, bool closeOnTouch)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("playMovie", new object[]
		{
			pathOrUrl,
			(int)bgColor,
			showControls,
			(int)scalingMode,
			closeOnTouch
		});
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
	public static void showToast(string text, bool useShortDuration)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showToast", new object[]
		{
			text,
			useShortDuration
		});
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0000E6DC File Offset: 0x0000C8DC
	public static void showAlert(string title, string message, string positiveButton)
	{
		EtceteraAndroid.showAlert(title, message, positiveButton, string.Empty);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0000E6EC File Offset: 0x0000C8EC
	public static void showAlert(string title, string message, string positiveButton, string negativeButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showAlert", new object[]
		{
			title,
			message,
			positiveButton,
			negativeButton
		});
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0000E72C File Offset: 0x0000C92C
	public static void showAlertPrompt(string title, string message, string promptHint, string promptText, string positiveButton, string negativeButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showAlertPrompt", new object[]
		{
			title,
			message,
			promptHint,
			promptText,
			positiveButton,
			negativeButton
		});
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0000E778 File Offset: 0x0000C978
	public static void showAlertPromptWithTwoFields(string title, string message, string promptHintOne, string promptTextOne, string promptHintTwo, string promptTextTwo, string positiveButton, string negativeButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showAlertPromptWithTwoFields", new object[]
		{
			title,
			message,
			promptHintOne,
			promptTextOne,
			promptHintTwo,
			promptTextTwo,
			positiveButton,
			negativeButton
		});
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0000E7CC File Offset: 0x0000C9CC
	public static void showProgressDialog(string title, string message)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showProgressDialog", new object[]
		{
			title,
			message
		});
	}

	// Token: 0x06000320 RID: 800 RVA: 0x0000E804 File Offset: 0x0000CA04
	public static void hideProgressDialog()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("hideProgressDialog", new object[0]);
	}

	// Token: 0x06000321 RID: 801 RVA: 0x0000E834 File Offset: 0x0000CA34
	public static void showWebView(string url)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showWebView", new object[]
		{
			url
		});
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0000E868 File Offset: 0x0000CA68
	public static void showCustomWebView(string url, bool disableTitle, bool disableBackButton)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showCustomWebView", new object[]
		{
			url,
			disableTitle,
			disableBackButton
		});
	}

	// Token: 0x06000323 RID: 803 RVA: 0x0000E8B0 File Offset: 0x0000CAB0
	public static void showEmailComposer(string toAddress, string subject, string text, bool isHTML)
	{
		EtceteraAndroid.showEmailComposer(toAddress, subject, text, isHTML, string.Empty);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
	public static void showEmailComposer(string toAddress, string subject, string text, bool isHTML, string attachmentFilePath)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("showEmailComposer", new object[]
		{
			toAddress,
			subject,
			text,
			isHTML,
			attachmentFilePath
		});
	}

	// Token: 0x06000325 RID: 805 RVA: 0x0000E90C File Offset: 0x0000CB0C
	public static bool isSMSComposerAvailable()
	{
		return Application.platform == RuntimePlatform.Android && EtceteraAndroid._plugin.Call<bool>("isSMSComposerAvailable", new object[0]);
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0000E934 File Offset: 0x0000CB34
	public static void showSMSComposer(string body)
	{
		EtceteraAndroid.showSMSComposer(body, null);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0000E940 File Offset: 0x0000CB40
	public static void showSMSComposer(string body, string[] recipients)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		string text = string.Empty;
		if (recipients != null && recipients.Length > 0)
		{
			text = "smsto:";
			foreach (string str in recipients)
			{
				text = text + str + ";";
			}
		}
		EtceteraAndroid._plugin.Call("showSMSComposer", new object[]
		{
			text,
			body
		});
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0000E9BC File Offset: 0x0000CBBC
	public static void promptToTakePhoto(string name)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("promptToTakePhoto", new object[]
		{
			name
		});
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
	public static void promptForPictureFromAlbum(string name)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("promptForPictureFromAlbum", new object[]
		{
			name
		});
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0000EA24 File Offset: 0x0000CC24
	public static void promptToTakeVideo(string name)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("promptToTakeVideo", new object[]
		{
			name
		});
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0000EA58 File Offset: 0x0000CC58
	public static bool saveImageToGallery(string pathToPhoto, string title)
	{
		return Application.platform == RuntimePlatform.Android && EtceteraAndroid._plugin.Call<bool>("saveImageToGallery", new object[]
		{
			pathToPhoto,
			title
		});
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0000EA88 File Offset: 0x0000CC88
	public static void scaleImageAtPath(string pathToImage, float scale)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("scaleImageAtPath", new object[]
		{
			pathToImage,
			scale
		});
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0000EABC File Offset: 0x0000CCBC
	public static Vector2 getImageSizeAtPath(string pathToImage)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return Vector2.zero;
		}
		string text = EtceteraAndroid._plugin.Call<string>("getImageSizeAtPath", new object[]
		{
			pathToImage
		});
		string[] array = text.Split(new char[]
		{
			','
		});
		return new Vector2((float)int.Parse(array[0]), (float)int.Parse(array[1]));
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0000EB20 File Offset: 0x0000CD20
	public static void initTTS()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("initTTS", new object[0]);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0000EB50 File Offset: 0x0000CD50
	public static void teardownTTS()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("teardownTTS", new object[0]);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0000EB80 File Offset: 0x0000CD80
	public static void speak(string text)
	{
		EtceteraAndroid.speak(text, TTSQueueMode.Add);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0000EB8C File Offset: 0x0000CD8C
	public static void speak(string text, TTSQueueMode queueMode)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("speak", new object[]
		{
			text,
			(int)queueMode
		});
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0000EBC0 File Offset: 0x0000CDC0
	public static void stop()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("stop", new object[0]);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x0000EBF0 File Offset: 0x0000CDF0
	public static void playSilence(long durationInMs, TTSQueueMode queueMode)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("playSilence", new object[]
		{
			durationInMs,
			(int)queueMode
		});
	}

	// Token: 0x06000334 RID: 820 RVA: 0x0000EC34 File Offset: 0x0000CE34
	public static void setPitch(float pitch)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("setPitch", new object[]
		{
			pitch
		});
	}

	// Token: 0x06000335 RID: 821 RVA: 0x0000EC64 File Offset: 0x0000CE64
	public static void setSpeechRate(float rate)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("setSpeechRate", new object[]
		{
			rate
		});
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0000EC94 File Offset: 0x0000CE94
	public static void askForReview(int launchesUntilPrompt, int hoursUntilFirstPrompt, int hoursBetweenPrompts, string title, string message, bool isAmazonAppStore = false)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (isAmazonAppStore)
		{
			EtceteraAndroid._plugin.Set<bool>("isAmazonAppStore", true);
		}
		EtceteraAndroid._plugin.Call("askForReview", new object[]
		{
			launchesUntilPrompt,
			hoursUntilFirstPrompt,
			hoursBetweenPrompts,
			title,
			message
		});
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0000ED00 File Offset: 0x0000CF00
	public static void askForReviewNow(string title, string message, bool isAmazonAppStore = false)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (isAmazonAppStore)
		{
			EtceteraAndroid._plugin.Set<bool>("isAmazonAppStore", true);
		}
		EtceteraAndroid._plugin.Call("askForReviewNow", new object[]
		{
			title,
			message
		});
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0000ED50 File Offset: 0x0000CF50
	public static void resetAskForReview()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("resetAskForReview", new object[0]);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x0000ED80 File Offset: 0x0000CF80
	public static void inlineWebViewShow(string url, int x, int y, int width, int height)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewShow", new object[]
		{
			url,
			x,
			y,
			width,
			height
		});
	}

	// Token: 0x0600033A RID: 826 RVA: 0x0000EDD8 File Offset: 0x0000CFD8
	public static void inlineWebViewClose()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewClose", new object[0]);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x0000EE08 File Offset: 0x0000D008
	public static void inlineWebViewSetUrl(string url)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewSetUrl", new object[]
		{
			url
		});
	}

	// Token: 0x0600033C RID: 828 RVA: 0x0000EE3C File Offset: 0x0000D03C
	public static void inlineWebViewSetFrame(int x, int y, int width, int height)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("inlineWebViewSetFrame", new object[]
		{
			x,
			y,
			width,
			height
		});
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0000EE90 File Offset: 0x0000D090
	public static int scheduleNotification(long secondsFromNow, string title, string subtitle, string tickerText, string extraData)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return -1;
		}
		return EtceteraAndroid._plugin.Call<int>("scheduleNotification", new object[]
		{
			secondsFromNow,
			title,
			subtitle,
			tickerText,
			extraData
		});
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0000EEDC File Offset: 0x0000D0DC
	public static void cancelNotification(int notificationId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("cancelNotification", new object[]
		{
			notificationId
		});
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0000EF0C File Offset: 0x0000D10C
	public static void checkForNotifications()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		EtceteraAndroid._plugin.Call("checkForNotifications", new object[0]);
	}

	// Token: 0x040001ED RID: 493
	private static AndroidJavaObject _plugin;

	// Token: 0x02000062 RID: 98
	public enum ScalingMode
	{
		// Token: 0x040001EF RID: 495
		None,
		// Token: 0x040001F0 RID: 496
		AspectFit,
		// Token: 0x040001F1 RID: 497
		Fill
	}
}
