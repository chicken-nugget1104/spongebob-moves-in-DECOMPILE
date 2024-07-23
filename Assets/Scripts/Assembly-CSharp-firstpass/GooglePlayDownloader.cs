using System;
using System.IO;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class GooglePlayDownloader
{
	// Token: 0x060002DF RID: 735 RVA: 0x0000D018 File Offset: 0x0000B218
	static GooglePlayDownloader()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			return;
		}
		GooglePlayDownloader.Environment = new AndroidJavaClass("android.os.Environment");
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderService"))
		{
			androidJavaClass.SetStatic<string>("BASE64_PUBLIC_KEY", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwqo+nBgOz9uvW/EeHYhTpctG0OdutjmzVbHlQBJWYVgqaDFcliO76afXQ2qnf5EW7H7jlQcF42zizzs1O1vC7cmYN6mbTnWnmOEDHuSyG02tKIVU2pGWBM/VsnIqL4lpmNaM4JKJYNcZ7a9pnBhfiUAqR9pejVAqYqVc5dG091TPIwzE/MjchSac0EFXa49iMuiYfdYjzXye/981t23PDk7IS+weXcthqjSfjwtnxobxktnJ9eSUK7jaBpFPbRIFHXuE/+ZQSYRtMu48myp/CnJd/0A/srnmDetb2ai990A4hQgvP9/HySyw14CHhq87tRCRAzXso5Q09iaU8e538QIDAQAB");
			androidJavaClass.SetStatic<byte[]>("SALT", new byte[]
			{
				1,
				43,
				244,
				byte.MaxValue,
				54,
				98,
				156,
				244,
				43,
				2,
				248,
				252,
				9,
				5,
				150,
				148,
				223,
				45,
				byte.MaxValue,
				84
			});
		}
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
	public static bool RunningOnAndroid()
	{
		if (GooglePlayDownloader.detectAndroidJNI == null)
		{
			GooglePlayDownloader.detectAndroidJNI = new AndroidJavaClass("android.os.Build");
		}
		return GooglePlayDownloader.detectAndroidJNI.GetRawClass() != IntPtr.Zero;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
	public static string GetExpansionFilePath()
	{
		GooglePlayDownloader.populateOBBData();
		if (GooglePlayDownloader.Environment.CallStatic<string>("getExternalStorageState", new object[0]) != "mounted")
		{
			return null;
		}
		string result;
		using (AndroidJavaObject androidJavaObject = GooglePlayDownloader.Environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory", new object[0]))
		{
			string arg = androidJavaObject.Call<string>("getPath", new object[0]);
			result = string.Format("{0}/{1}/{2}", arg, "Android/obb", GooglePlayDownloader.obb_package);
		}
		return result;
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0000D184 File Offset: 0x0000B384
	public static string GetMainOBBPath(string expansionFilePath)
	{
		GooglePlayDownloader.populateOBBData();
		if (expansionFilePath == null)
		{
			return null;
		}
		string text = string.Format("{0}/main.{1}.{2}.obb", expansionFilePath, GooglePlayDownloader.obb_version, GooglePlayDownloader.obb_package);
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
	public static string GetPatchOBBPath(string expansionFilePath)
	{
		GooglePlayDownloader.populateOBBData();
		if (expansionFilePath == null)
		{
			return null;
		}
		string text = string.Format("{0}/patch.{1}.{2}.obb", expansionFilePath, GooglePlayDownloader.obb_version, GooglePlayDownloader.obb_package);
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x0000D20C File Offset: 0x0000B40C
	public static void FetchOBB()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[]
			{
				@static,
				new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderActivity")
			});
			int num = 65536;
			androidJavaObject.Call<AndroidJavaObject>("addFlags", new object[]
			{
				num
			});
			androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[]
			{
				"unityplayer.Activity",
				@static.Call<AndroidJavaObject>("getClass", new object[0]).Call<string>("getName", new object[0])
			});
			@static.Call("startActivity", new object[]
			{
				androidJavaObject
			});
			if (AndroidJNI.ExceptionOccurred() != IntPtr.Zero)
			{
				Debug.LogError("Exception occurred while attempting to start DownloaderActivity - is the AndroidManifest.xml incorrect?");
				AndroidJNI.ExceptionDescribe();
				AndroidJNI.ExceptionClear();
			}
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0000D31C File Offset: 0x0000B51C
	private static void populateOBBData()
	{
		if (GooglePlayDownloader.obb_version != 0)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			GooglePlayDownloader.obb_package = @static.Call<string>("getPackageName", new object[0]);
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]).Call<AndroidJavaObject>("getPackageInfo", new object[]
			{
				GooglePlayDownloader.obb_package,
				0
			});
			GooglePlayDownloader.obb_version = androidJavaObject.Get<int>("versionCode");
		}
	}

	// Token: 0x040001BD RID: 445
	private const string Environment_MEDIA_MOUNTED = "mounted";

	// Token: 0x040001BE RID: 446
	private static AndroidJavaClass detectAndroidJNI;

	// Token: 0x040001BF RID: 447
	private static AndroidJavaClass Environment;

	// Token: 0x040001C0 RID: 448
	private static string obb_package;

	// Token: 0x040001C1 RID: 449
	private static int obb_version;
}
