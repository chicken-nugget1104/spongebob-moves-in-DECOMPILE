using System;

// Token: 0x020000AA RID: 170
public static class StringExtensions
{
	// Token: 0x060006C8 RID: 1736 RVA: 0x00018D74 File Offset: 0x00016F74
	public static string UnescapeXML(this string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}
		string text = s.Replace("&apos;", "'");
		text = text.Replace("&quot;", "\"");
		text = text.Replace("&gt;", ">");
		text = text.Replace("&lt;", "<");
		return text.Replace("&amp;", "&");
	}
}
