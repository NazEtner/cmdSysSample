using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    public class SetVariable<T> : Command
    {
        public SetVariable(string name, T value)
        {
            m_name = name;
            m_value = value;
        }

        public override CommandResult Execute(ref ScheduleStatus status)
        {
            CommandVariable result = new CommandVariable();
            result.SetValue(m_value);
            status.variables[m_name] = result;

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