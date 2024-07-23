using System;
using System.Timers;

// Token: 0x0200004D RID: 77
public class SBGamePersister
{
	// Token: 0x06000336 RID: 822 RVA: 0x00010460 File Offset: 0x0000E660
	public SBGamePersister(Session session)
	{
		this.session = session;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00010470 File Offset: 0x0000E670
	public void Start()
	{
		if (SBSettings.SAVE_INTERVAL >= 0)
		{
			this.Stop();
			this.timer = new Timer((double)(SBSettings.SAVE_INTERVAL * 1000));
			this.timer.Elapsed += this.TimerTick;
			this.timer.Start();
		}
	}

	// Token: 0x06000338 RID: 824 RVA: 0x000104C8 File Offset: 0x0000E6C8
	public void Stop()
	{
		if (this.timer != null)
		{
			this.timer.Stop();
			this.timer = null;
		}
	}

	// Token: 0x06000339 RID: 825 RVA: 0x000104E8 File Offset: 0x0000E6E8
	public void TimerTick(object sender, ElapsedEventArgs e)
	{
		this.SaveOrPatch();
	}

	// Token: 0x0600033A RID: 826 RVA: 0x000104F0 File Offset: 0x0000E6F0
	public void SaveOrPatch()
	{
		this.session.CheckForPatching(false);
	}

	// Token: 0x04000226 RID: 550
	private Session session;

	// Token: 0x04000227 RID: 551
	private Timer timer;
}
