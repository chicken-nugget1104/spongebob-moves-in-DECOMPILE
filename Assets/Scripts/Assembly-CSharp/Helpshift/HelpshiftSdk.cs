using System;
using System.Collections.Generic;

namespace Helpshift
{
	// Token: 0x0200002B RID: 43
	public class HelpshiftSdk
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x00009150 File Offset: 0x00007350
		private HelpshiftSdk()
		{
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000915C File Offset: 0x0000735C
		public static HelpshiftSdk getInstance()
		{
			if (HelpshiftSdk.instance == null)
			{
				HelpshiftSdk.instance = new HelpshiftSdk();
				HelpshiftSdk.nativeSdk = new HelpshiftAndroid();
			}
			return HelpshiftSdk.instance;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00009184 File Offset: 0x00007384
		public void install(string apiKey, string domainName, string appId, Dictionary<string, object> config)
		{
			HelpshiftSdk.nativeSdk.install(apiKey, domainName, appId, config);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009198 File Offset: 0x00007398
		public void install(string apiKey, string domainName, string appId)
		{
			this.install(apiKey, domainName, appId, new Dictionary<string, object>());
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000091A8 File Offset: 0x000073A8
		public void install()
		{
			HelpshiftSdk.nativeSdk.install();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000091B4 File Offset: 0x000073B4
		public int getNotificationCount(bool isAsync)
		{
			return HelpshiftSdk.nativeSdk.getNotificationCount(isAsync);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000091C4 File Offset: 0x000073C4
		public void setNameAndEmail(string userName, string email)
		{
			HelpshiftSdk.nativeSdk.setNameAndEmail(userName, email);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000091D4 File Offset: 0x000073D4
		public void setUserIdentifier(string identifier)
		{
			HelpshiftSdk.nativeSdk.setUserIdentifier(identifier);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000091E4 File Offset: 0x000073E4
		public void login(string identifier, string name, string email)
		{
			HelpshiftSdk.nativeSdk.login(identifier, name, email);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000091F4 File Offset: 0x000073F4
		public void logout()
		{
			HelpshiftSdk.nativeSdk.logout();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009200 File Offset: 0x00007400
		public void registerDeviceToken(string deviceToken)
		{
			HelpshiftSdk.nativeSdk.registerDeviceToken(deviceToken);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009210 File Offset: 0x00007410
		public void leaveBreadCrumb(string breadCrumb)
		{
			HelpshiftSdk.nativeSdk.leaveBreadCrumb(breadCrumb);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009220 File Offset: 0x00007420
		public void clearBreadCrumbs()
		{
			HelpshiftSdk.nativeSdk.clearBreadCrumbs();
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000922C File Offset: 0x0000742C
		public void showConversation(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showConversation(configMap);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000923C File Offset: 0x0000743C
		public void showConversation()
		{
			HelpshiftSdk.nativeSdk.showConversation();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009248 File Offset: 0x00007448
		public void showConversationWithMeta(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showConversationWithMeta(configMap);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009258 File Offset: 0x00007458
		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQSection(sectionPublishId, configMap);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009268 File Offset: 0x00007468
		public void showFAQSection(string sectionPublishId)
		{
			HelpshiftSdk.nativeSdk.showFAQSection(sectionPublishId);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009278 File Offset: 0x00007478
		public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQSectionWithMeta(sectionPublishId, configMap);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009288 File Offset: 0x00007488
		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showSingleFAQ(questionPublishId, configMap);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009298 File Offset: 0x00007498
		public void showSingleFAQ(string questionPublishId)
		{
			HelpshiftSdk.nativeSdk.showSingleFAQ(questionPublishId);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000092A8 File Offset: 0x000074A8
		public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showSingleFAQWithMeta(questionPublishId, configMap);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000092B8 File Offset: 0x000074B8
		public void showFAQs(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQs(configMap);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000092C8 File Offset: 0x000074C8
		public void showFAQs()
		{
			HelpshiftSdk.nativeSdk.showFAQs();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000092D4 File Offset: 0x000074D4
		public void showFAQsWithMeta(Dictionary<string, object> configMap)
		{
			HelpshiftSdk.nativeSdk.showFAQsWithMeta(configMap);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000092E4 File Offset: 0x000074E4
		public void updateMetaData(Dictionary<string, object> metaData)
		{
			HelpshiftSdk.nativeSdk.updateMetaData(metaData);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000092F4 File Offset: 0x000074F4
		public void handlePushNotification(string issueId)
		{
			HelpshiftSdk.nativeSdk.handlePushNotification(issueId);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009304 File Offset: 0x00007504
		public void showAlertToRateAppWithURL(string url)
		{
			HelpshiftSdk.nativeSdk.showAlertToRateAppWithURL(url);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009314 File Offset: 0x00007514
		public void setSDKLanguage(string locale)
		{
			HelpshiftSdk.nativeSdk.setSDKLanguage(locale);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009324 File Offset: 0x00007524
		public void registerSessionDelegates()
		{
			HelpshiftSdk.nativeSdk.registerSessionDelegates();
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009330 File Offset: 0x00007530
		public void registerForPush(string gcmId)
		{
			HelpshiftSdk.nativeSdk.registerForPushWithGcmId(gcmId);
		}

		// Token: 0x040000E9 RID: 233
		public const string HS_RATE_ALERT_CLOSE = "HS_RATE_ALERT_CLOSE";

		// Token: 0x040000EA RID: 234
		public const string HS_RATE_ALERT_FEEDBACK = "HS_RATE_ALERT_FEEDBACK";

		// Token: 0x040000EB RID: 235
		public const string HS_RATE_ALERT_SUCCESS = "HS_RATE_ALERT_SUCCESS";

		// Token: 0x040000EC RID: 236
		public const string HS_RATE_ALERT_FAIL = "HS_RATE_ALERT_FAIL";

		// Token: 0x040000ED RID: 237
		public const string HSTAGSKEY = "hs-tags";

		// Token: 0x040000EE RID: 238
		public const string HSCUSTOMMETADATAKEY = "hs-custom-metadata";

		// Token: 0x040000EF RID: 239
		public const string HSTAGSMATCHINGKEY = "withTagsMatching";

		// Token: 0x040000F0 RID: 240
		public const string CONTACT_US_ALWAYS = "always";

		// Token: 0x040000F1 RID: 241
		public const string CONTACT_US_NEVER = "never";

		// Token: 0x040000F2 RID: 242
		public const string CONTACT_US_AFTER_VIEWING_FAQS = "after_viewing_faqs";

		// Token: 0x040000F3 RID: 243
		public const string HSUserAcceptedTheSolution = "User accepted the solution";

		// Token: 0x040000F4 RID: 244
		public const string HSUserRejectedTheSolution = "User rejected the solution";

		// Token: 0x040000F5 RID: 245
		public const string HSUserSentScreenShot = "User sent a screenshot";

		// Token: 0x040000F6 RID: 246
		public const string HSUserReviewedTheApp = "User reviewed the app";

		// Token: 0x040000F7 RID: 247
		private static HelpshiftSdk instance;

		// Token: 0x040000F8 RID: 248
		private static HelpshiftAndroid nativeSdk;
	}
}
