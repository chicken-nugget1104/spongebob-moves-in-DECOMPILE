using System;
using System.Collections.Generic;

// Token: 0x020002A3 RID: 675
public class CommandRouter
{
	// Token: 0x060014AB RID: 5291 RVA: 0x0008C01C File Offset: 0x0008A21C
	public CommandRouter()
	{
		this.simulated = new Dictionary<Identity, Simulated>(new Identity.Equality());
		this.commands = new List<Command>();
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x0008C040 File Offset: 0x0008A240
	public void Register(Simulated entity)
	{
		this.simulated.Add(entity.Id, entity);
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x0008C054 File Offset: 0x0008A254
	public void Unregister(Simulated entity)
	{
		this.simulated.Remove(entity.Id);
		Predicate<Command> match = (Command command) => command.Receiver.Equals(entity.Id);
		this.commands.RemoveAll(match);
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x0008C0A0 File Offset: 0x0008A2A0
	public void Send(Command command)
	{
		this.commands.Add(command);
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x0008C0B0 File Offset: 0x0008A2B0
	public void Send(Command command, Action onComplete)
	{
		command.OnComplete = onComplete;
		this.Send(command);
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x0008C0C0 File Offset: 0x0008A2C0
	public void Send(Command command, ulong delay)
	{
		command.TimeEpoch = TFUtils.EpochTime() + delay;
		this.Send(command);
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x0008C0D8 File Offset: 0x0008A2D8
	public int CancelMatching(Command.TYPE type, Identity sender, Identity receiver, Dictionary<string, object> matching = null)
	{
		return this.commands.RemoveAll((Command element) => element.Type == type && element.Sender.Equals(sender) && element.Receiver.Equals(receiver) && (matching == null || element.Match(matching)));
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x0008C120 File Offset: 0x0008A320
	public void Route()
	{
		this.commands.RemoveAll(new Predicate<Command>(this.RouteCommand));
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x0008C13C File Offset: 0x0008A33C
	private bool RouteCommand(Command command)
	{
		ulong num = TFUtils.EpochTime();
		if (num >= command.TimeEpoch)
		{
			Simulated simulated;
			if (this.simulated.TryGetValue(command.Receiver, out simulated))
			{
				simulated.Push(command);
			}
			else
			{
				TFUtils.WarningLog(string.Format("Dropped command({0}). Could not find receiver({1}).", command.Describe(), command.Receiver));
			}
			return true;
		}
		return false;
	}

	// Token: 0x04000E78 RID: 3704
	public const bool DEBUG_LOG_COMMANDS = false;

	// Token: 0x04000E79 RID: 3705
	private Dictionary<Identity, Simulated> simulated;

	// Token: 0x04000E7A RID: 3706
	private List<Command> commands;
}
