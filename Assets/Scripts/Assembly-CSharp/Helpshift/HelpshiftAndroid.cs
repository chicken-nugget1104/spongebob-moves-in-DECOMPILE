using System;
using System.Collections.Generic;
using System.Linq;
using HSMiniJSON;
using UnityEngine;

namespace Helpshift
{
	// Token: 0x02000025 RID: 37
	public class HelpshiftAndroid
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00007644 File Offset: 0x00005844
		public HelpshiftAndroid()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				this.currentActivity = this.jc.GetStatic<AndroidJavaObject>("currentActivity");
				this.application = this.currentActivity.Call<AndroidJavaObject>("getApplication", new object[0]);
				this.hsPlugin = new AndroidJavaClass("com.helpshift.Helpshift");
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000076B8 File Offset: 0x000058B8
		private AndroidJavaObject convertToJavaHashMap(Dictionary<string, object> configD)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
			if (configD != null)
			{
				Dictionary<string, object> dictionary = (from kv in configD
				where kv.Value != null
				select kv).ToDictionary((KeyValuePair<string, object> kv) => kv.Key, (KeyValuePair<string, object> kv) => kv.Value);
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				array[0] = (array[1] = null);
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
					{
						keyValuePair.Key
					}))
					{
						array[0] = androidJavaObject2;
						if ((keyValuePair.Value != null && keyValuePair.Value.Equals("yes")) || keyValuePair.Value.Equals("no"))
						{
							string text = (!keyValuePair.Value.Equals("yes")) ? "false" : "true";
							array[1] = new AndroidJavaObject("java.lang.Boolean", new object[]
							{
								text
							});
						}
						else if (keyValuePair.Value != null)
						{
							if (keyValuePair.Value.GetType().ToString() == "System.String")
							{
								array[1] = new AndroidJavaObject("java.lang.String", new object[]
								{
									keyValuePair.Value
								});
							}
							else if (keyValuePair.Value.GetType().ToString() == "System.String[]")
							{
								string[] array2 = (string[])keyValuePair.Value;
								AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.util.ArrayList", new object[0]);
								IntPtr methodID2 = AndroidJNIHelper.GetMethodID(androidJavaObject3.GetRawClass(), "add", "(Ljava/lang/String;)Z");
								object[] array3 = new object[1];
								foreach (string text2 in array2)
								{
									if (text2 != null)
									{
										array3[0] = new AndroidJavaObject("java.lang.String", new object[]
										{
											text2
										});
										AndroidJNI.CallBooleanMethod(androidJavaObject3.GetRawObject(), methodID2, AndroidJNIHelper.CreateJNIArgArray(array3));
									}
								}
								array[1] = new AndroidJavaObject("java.util.ArrayList", new object[]
								{
									androidJavaObject3
								});
							}
							else
							{
								if (keyValuePair.Key == "hs-custom-metadata")
								{
									Dictionary<string, object> dictionary2 = (Dictionary<string, object>)keyValuePair.Value;
									AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("java.util.HashMap", new object[0]);
									IntPtr methodID3 = AndroidJNIHelper.GetMethodID(androidJavaObject4.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
									object[] array5 = new object[2];
									array5[0] = (array5[1] = null);
									foreach (KeyValuePair<string, object> keyValuePair2 in dictionary2)
									{
										array5[0] = new AndroidJavaObject("java.lang.String", new object[]
										{
											keyValuePair2.Key
										});
										if (keyValuePair2.Value.GetType().ToString() == "System.String")
										{
											array5[1] = new AndroidJavaObject("java.lang.String", new object[]
											{
												keyValuePair2.Value
											});
										}
										else if (keyValuePair2.Value.GetType().ToString() == "System.Int32")
										{
											array5[1] = new AndroidJavaObject("java.lang.Integer", new object[]
											{
												keyValuePair2.Value
											});
										}
										else if (keyValuePair2.Value.GetType().ToString() == "System.Double")
										{
											array5[1] = new AndroidJavaObject("java.lang.Double", new object[]
											{
												keyValuePair2.Value
											});
										}
										else if (keyValuePair2.Key == "hs-tags" && keyValuePair2.Value.GetType().ToString() == "System.String[]")
										{
											string[] array6 = (string[])keyValuePair2.Value;
											AndroidJavaObject androidJavaObject5 = new AndroidJavaObject("java.util.ArrayList", new object[0]);
											IntPtr methodID4 = AndroidJNIHelper.GetMethodID(androidJavaObject5.GetRawClass(), "add", "(Ljava/lang/String;)Z");
											object[] array7 = new object[1];
											foreach (string text3 in array6)
											{
												if (text3 != null)
												{
													array7[0] = new AndroidJavaObject("java.lang.String", new object[]
													{
														text3
													});
													AndroidJNI.CallBooleanMethod(androidJavaObject5.GetRawObject(), methodID4, AndroidJNIHelper.CreateJNIArgArray(array7));
												}
											}
											array5[1] = new AndroidJavaObject("java.util.ArrayList", new object[]
											{
												androidJavaObject5
											});
										}
										if (array5[1] != null)
										{
											AndroidJNI.CallObjectMethod(androidJavaObject4.GetRawObject(), methodID3, AndroidJNIHelper.CreateJNIArgArray(array5));
										}
									}
									array[1] = new AndroidJavaObject("java.util.HashMap", new object[]
									{
										androidJavaObject4
									});
								}
								if (keyValuePair.Key == "withTagsMatching")
								{
									Dictionary<string, object> dictionary3 = (Dictionary<string, object>)keyValuePair.Value;
									AndroidJavaObject androidJavaObject6 = new AndroidJavaObject("java.util.HashMap", new object[0]);
									IntPtr methodID5 = AndroidJNIHelper.GetMethodID(androidJavaObject6.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
									object[] array9 = new object[2];
									array9[0] = (array9[1] = null);
									foreach (KeyValuePair<string, object> keyValuePair3 in dictionary3)
									{
										array9[0] = new AndroidJavaObject("java.lang.String", new object[]
										{
											keyValuePair3.Key
										});
										if (keyValuePair3.Key == "operator" && keyValuePair3.Value.GetType().ToString() == "System.String")
										{
											array9[1] = new AndroidJavaObject("java.lang.String", new object[]
											{
												keyValuePair3.Value
											});
										}
										else if (keyValuePair3.Key == "tags" && keyValuePair3.Value.GetType().ToString() == "System.String[]")
										{
											string[] array10 = (string[])keyValuePair3.Value;
											AndroidJavaObject androidJavaObject7 = new AndroidJavaObject("java.util.ArrayList", new object[0]);
											IntPtr methodID6 = AndroidJNIHelper.GetMethodID(androidJavaObject7.GetRawClass(), "add", "(Ljava/lang/String;)Z");
											object[] array11 = new object[1];
											foreach (string text4 in array10)
											{
												if (text4 != null)
												{
													array11[0] = new AndroidJavaObject("java.lang.String", new object[]
													{
														text4
													});
													AndroidJNI.CallBooleanMethod(androidJavaObject7.GetRawObject(), methodID6, AndroidJNIHelper.CreateJNIArgArray(array11));
												}
											}
											array9[1] = new AndroidJavaObject("java.util.ArrayList", new object[]
											{
												androidJavaObject7
											});
										}
										if (array9[1] != null)
										{
											AndroidJNI.CallObjectMethod(androidJavaObject6.GetRawObject(), methodID5, AndroidJNIHelper.CreateJNIArgArray(array9));
										}
									}
									array[1] = new AndroidJavaObject("java.util.HashMap", new object[]
									{
										androidJavaObject6
									});
								}
							}
						}
						if (array[1] != null)
						{
							AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
						}
					}
				}
			}
			return androidJavaObject;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007EC4 File Offset: 0x000060C4
		private AndroidJavaObject convertMetadataToJavaHashMap(Dictionary<string, object> metaMap)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
			if (metaMap != null)
			{
				IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
				object[] array = new object[2];
				array[0] = (array[1] = null);
				foreach (KeyValuePair<string, object> keyValuePair in metaMap)
				{
					array[0] = new AndroidJavaObject("java.lang.String", new object[]
					{
						keyValuePair.Key
					});
					if (keyValuePair.Value.GetType().ToString() == "System.String")
					{
						if ((keyValuePair.Value != null && keyValuePair.Value.Equals("yes")) || keyValuePair.Value.Equals("no"))
						{
							string text = (!keyValuePair.Value.Equals("yes")) ? "false" : "true";
							array[1] = new AndroidJavaObject("java.lang.Boolean", new object[]
							{
								text
							});
						}
						else
						{
							array[1] = new AndroidJavaObject("java.lang.String", new object[]
							{
								keyValuePair.Value
							});
						}
					}
					else if (keyValuePair.Key == "hs-tags" && keyValuePair.Value.GetType().ToString() == "System.String[]")
					{
						string[] array2 = (string[])keyValuePair.Value;
						AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.util.ArrayList", new object[0]);
						IntPtr methodID2 = AndroidJNIHelper.GetMethodID(androidJavaObject2.GetRawClass(), "add", "(Ljava/lang/String;)Z");
						object[] array3 = new object[1];
						foreach (string text2 in array2)
						{
							if (text2 != null)
							{
								array3[0] = new AndroidJavaObject("java.lang.String", new object[]
								{
									text2
								});
								AndroidJNI.CallBooleanMethod(androidJavaObject2.GetRawObject(), methodID2, AndroidJNIHelper.CreateJNIArgArray(array3));
							}
						}
						array[1] = new AndroidJavaObject("java.util.ArrayList", new object[]
						{
							androidJavaObject2
						});
					}
					if (array[1] != null)
					{
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
					}
				}
			}
			Debug.Log("Returning the Hashmap : " + androidJavaObject);
			return androidJavaObject;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008150 File Offset: 0x00006350
		private void hsApiCall(string api, params object[] args)
		{
			this.hsPlugin.CallStatic(api, args);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008160 File Offset: 0x00006360
		private void hsApiCall(string api)
		{
			this.hsPlugin.CallStatic(api, new object[0]);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008174 File Offset: 0x00006374
		public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("install", new object[]
			{
				this.application,
				apiKey,
				domain,
				appId,
				this.convertToJavaHashMap(configMap)
			});
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000081A8 File Offset: 0x000063A8
		public void install()
		{
			this.hsApiCall("install", new object[]
			{
				this.application
			});
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000081C4 File Offset: 0x000063C4
		public int getNotificationCount(bool isAsync)
		{
			return this.hsPlugin.CallStatic<int>("getNotificationCount", new object[]
			{
				isAsync
			});
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000081E8 File Offset: 0x000063E8
		public void setNameAndEmail(string userName, string email)
		{
			this.hsApiCall("setNameAndEmail", new object[]
			{
				userName,
				email
			});
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008204 File Offset: 0x00006404
		public void setUserIdentifier(string identifier)
		{
			this.hsApiCall("setUserIdentifier", new object[]
			{
				identifier
			});
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000821C File Offset: 0x0000641C
		public void registerDeviceToken(string deviceToken)
		{
			this.hsApiCall("registerDeviceToken", new object[]
			{
				this.currentActivity,
				deviceToken
			});
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000823C File Offset: 0x0000643C
		public void leaveBreadCrumb(string breadCrumb)
		{
			this.hsApiCall("leaveBreadCrumb", new object[]
			{
				breadCrumb
			});
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008254 File Offset: 0x00006454
		public void clearBreadCrumbs()
		{
			this.hsApiCall("clearBreadCrumbs");
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008264 File Offset: 0x00006464
		public void login(string identifier, string userName, string email)
		{
			this.hsApiCall("login", new object[]
			{
				identifier,
				userName,
				email
			});
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008284 File Offset: 0x00006484
		public void logout()
		{
			this.hsApiCall("logout");
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008294 File Offset: 0x00006494
		public void showConversation(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showConversation", new object[]
			{
				this.currentActivity,
				this.convertToJavaHashMap(configMap)
			});
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000082C8 File Offset: 0x000064C8
		public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQSection", new object[]
			{
				this.currentActivity,
				sectionPublishId,
				this.convertToJavaHashMap(configMap)
			});
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008300 File Offset: 0x00006500
		public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showSingleFAQ", new object[]
			{
				this.currentActivity,
				questionPublishId,
				this.convertToJavaHashMap(configMap)
			});
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008338 File Offset: 0x00006538
		public void showFAQs(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQs", new object[]
			{
				this.currentActivity,
				this.convertToJavaHashMap(configMap)
			});
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000836C File Offset: 0x0000656C
		public void showConversation()
		{
			this.hsApiCall("showConversation");
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000837C File Offset: 0x0000657C
		public void showFAQSection(string sectionPublishId)
		{
			this.hsApiCall("showFAQSection", new object[]
			{
				sectionPublishId
			});
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008394 File Offset: 0x00006594
		public void showSingleFAQ(string questionPublishId)
		{
			this.hsApiCall("showSingleFAQ", new object[]
			{
				questionPublishId
			});
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000083AC File Offset: 0x000065AC
		public void showFAQs()
		{
			this.hsApiCall("showFAQs");
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000083BC File Offset: 0x000065BC
		public void showConversationWithMeta(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showConversationWithMeta", new object[]
			{
				this.convertMetadataToJavaHashMap(configMap)
			});
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000083DC File Offset: 0x000065DC
		public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQSectionWithMeta", new object[]
			{
				sectionPublishId,
				this.convertMetadataToJavaHashMap(configMap)
			});
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008400 File Offset: 0x00006600
		public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
		{
			this.hsApiCall("showSingleFAQWithMeta", new object[]
			{
				questionPublishId,
				this.convertMetadataToJavaHashMap(configMap)
			});
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008424 File Offset: 0x00006624
		public void showFAQsWithMeta(Dictionary<string, object> configMap)
		{
			this.hsApiCall("showFAQsWithMeta", new object[]
			{
				this.convertMetadataToJavaHashMap(configMap)
			});
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008444 File Offset: 0x00006644
		public void updateMetaData(Dictionary<string, object> metaData)
		{
			this.hsApiCall("setMetaData", new object[]
			{
				Json.Serialize(metaData)
			});
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008460 File Offset: 0x00006660
		public void handlePushNotification(string issueId)
		{
			this.hsApiCall("handlePush", new object[]
			{
				this.currentActivity,
				issueId
			});
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008480 File Offset: 0x00006680
		public void showAlertToRateAppWithURL(string url)
		{
			this.hsApiCall("showAlertToRateApp", new object[]
			{
				url
			});
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008498 File Offset: 0x00006698
		public void registerSessionDelegates()
		{
			this.hsApiCall("registerSessionDelegates");
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000084A8 File Offset: 0x000066A8
		public void registerForPushWithGcmId(string gcmId)
		{
			this.hsApiCall("registerGcmKey", new object[]
			{
				gcmId,
				this.currentActivity
			});
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000084C8 File Offset: 0x000066C8
		public void setSDKLanguage(string locale)
		{
			this.hsApiCall("setSDKLanguage", new object[]
			{
				locale
			});
		}

		// Token: 0x040000D0 RID: 208
		private AndroidJavaClass jc;

		// Token: 0x040000D1 RID: 209
		private AndroidJavaObject currentActivity;

		// Token: 0x040000D2 RID: 210
		private AndroidJavaObject application;

		// Token: 0x040000D3 RID: 211
		private AndroidJavaObject hsPlugin;
	}
}
