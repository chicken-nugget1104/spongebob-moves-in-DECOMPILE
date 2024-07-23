using System;
using System.Collections.Generic;

// Token: 0x0200044E RID: 1102
public static class SoundIndexFactory
{
	// Token: 0x06002219 RID: 8729 RVA: 0x000D21E4 File Offset: 0x000D03E4
	public static ISoundIndex FromDict(Dictionary<string, object> data)
	{
		string text = TFUtils.LoadString(data, "name");
		string text2 = TFUtils.TryLoadString(data, "file");
		string text3 = TFUtils.TryLoadString(data, "character");
		if (text3 != null)
		{
			text3 = text3.ToLower();
		}
		List<string> list = TFUtils.TryLoadList<string>(data, "files");
		List<string> list2 = TFUtils.TryLoadList<string>(data, "set");
		int? num = TFUtils.TryLoadInt(data, "max_instances");
		if (num == null)
		{
			num = new int?(5);
		}
		int num2 = 0;
		if (text2 != null)
		{
			num2++;
		}
		if (list != null)
		{
			num2++;
		}
		if (list2 != null)
		{
			num2++;
		}
		if (num2 != 1)
		{
			TFUtils.DebugLog("There is a problem with this entry...");
			TFUtils.ErrorLog("Sound file must contain exactly 1 of 'file' 'files' or 'set' keys.");
			return null;
		}
		if (text2 != null)
		{
			return new SingleSound(text, num.Value, text2, text3);
		}
		if (list != null)
		{
			return new SoundArray(text, num.Value, list, text3);
		}
		if (list2 != null)
		{
			return new SoundSet(text, num.Value, list2);
		}
		return null;
	}
}
