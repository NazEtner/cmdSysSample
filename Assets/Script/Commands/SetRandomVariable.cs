using Nananami.Lib.CmdSys;
using UnityEngine;

namespace Nananami.Commands
{
    // Tはfloatかint
    public class SetRandomVariable<T> : Command
    {
        public SetRandomVariable(string name, float min, float max)
        {
            m_name = name;
            m_range_min = min;
            m_range_max = max;
        }

        public override CommandResult Execute(ref ScheduleStatus status)
        {
            T value = (T)(object)Random.Range(m_range_min, m_range_max);

            return new SetVariable<T>(m_name, value).Execute(ref status);
        }

        public override void Reset()
        {
            // nothing to do
        }

        private string m_name;
        private float m_range_min, m_range_max;
    }
}