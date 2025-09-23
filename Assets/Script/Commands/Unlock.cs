using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    public class Unlock : Command
    {
        public override CommandResult Execute(ref ScheduleStatus status)
        {
            status.locked = false;
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