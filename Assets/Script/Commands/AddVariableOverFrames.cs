using System;
using Nananami.Helpers;
using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    // floatの変数のみ対応です
    public class AddVariableOverFrames : Command
    {
        public AddVariableOverFrames(string name, float value, uint frames)
        {
            m_name = name;
            m_frames = frames;
            m_total_value_to_be_added = value;
            Reset();
        }

        public override CommandResult Execute(ref ScheduleStatus status)
        {
            if (m_count-- > 0)
            {
                if (m_count == 0)
                {
                    m_addition = m_total_value_to_be_added - m_total_added;
                }

                float current = CommandVariableHelper.GetVariable<float>(status.scheduler, m_name);
                current += m_addition;
                m_total_added += m_addition;

                CommandVariable result = new CommandVariable();
                result.SetValue(current);
                status.variables[m_name] = result;

                return new CommandResult { endPeriod = true, expired = false, recordable = false };
            }

            return new CommandResult { endPeriod = false, expired = true, recordable = true };
        }

        public override void Reset()
        {
            m_addition = m_total_value_to_be_added / m_frames;
            m_count = m_frames;
            m_total_added = 0f;
        }

        private string m_name;
        private uint m_frames;
        private uint m_count;
        private float m_total_value_to_be_added;
        private float m_total_added;
        private float m_addition;
    }
}