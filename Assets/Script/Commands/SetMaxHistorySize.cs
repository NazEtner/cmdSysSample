using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    public class SetMaxHistorySize : Command
    {
        public SetMaxHistorySize(uint size)
        {
            m_max_history_size = size;
        }
        public override CommandResult Execute(ref ScheduleStatus status)
        {
            status.maxHistorySize = m_max_history_size;
            return new CommandResult
            {
                endPeriod = false,
                expired = true,
                recordable = false,
            };
        }

        public override void Reset()
        {
            // nothing to do
        }

        private uint m_max_history_size;
    }
}