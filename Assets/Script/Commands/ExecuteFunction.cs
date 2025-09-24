using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    public class ExecuteFunction : Command
    {
        public delegate CommandResult CmdAction(ref ScheduleStatus status);
        public ExecuteFunction(CmdAction action)
        {
            m_cmd_action = action;
        }

        public override CommandResult Execute(ref ScheduleStatus status)
        {
            return m_cmd_action(ref status);
        }

        public override void Reset()
        {
            // nothing to do
        }

        private CmdAction m_cmd_action;
    }
}