using Nananami.Lib.CmdSys;

namespace Nananami.Commands.Debug
{
    public abstract class DebugCommandBase : Command
    {
        public override CommandResult Execute(ref ScheduleStatus status)
        {
#if DEBUG
            return m_debugExecute(ref status);
#else
            return new CommandResult 
            {
                endPeriod = false,
                expired = true,
                recordable = false,
            };
#endif
        }

        public override void Reset()
        {
            // nothing to do
        }

        protected abstract CommandResult m_debugExecute(ref ScheduleStatus status);
    }
}