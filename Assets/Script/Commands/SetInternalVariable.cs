using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    public class SetInternalVariable<T> : Command
    {
        public SetInternalVariable(string name, T value)
        {
            m_name = name;
            m_value = value;
        }

        public override CommandResult Execute(ref ScheduleStatus status)
        {
            CommandVariable result = new CommandVariable();
            result.SetValue(m_value);
            status.internalVariables[m_name] = result;

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

        private T m_value;
        private string m_name;
    }
}