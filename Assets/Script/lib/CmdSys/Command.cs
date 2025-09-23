namespace Nananami.Lib.CmdSys
{
    public abstract class Command
    {
        public abstract CommandResult Execute(ref ScheduleStatus status);
        public abstract void Reset();
    }
}