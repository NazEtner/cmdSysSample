using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    public class Lock : Command
    {
        public override CommandResult Execute(ref ScheduleStatus status)
        {
            status.locked = true;
            return new CommandResult
            {
                endPeriod = false,
                expired = true,
                recordable = true,
            };
        }

        public override void Reset()
        {
            // nothing to do
        }
    }
}