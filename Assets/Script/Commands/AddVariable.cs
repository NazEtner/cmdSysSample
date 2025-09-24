using System;
using Nananami.Helpers;
using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    // Tはint又はfloatにしてください
    public class AddVariable<T> : Command
    {
        public AddVariable(string name, T value)
        {
            m_name = name;
            m_value = value;
        }

        public override CommandResult Execute(ref ScheduleStatus status)
        {
            try
            {
                if (typeof(T) == typeof(string) || typeof(T) == typeof(bool))
                {
                    throw new InvalidOperationException("Cannot add values to a string or boolean.");
                }

                T value = CommandVariableHelper.GetVariable<T>(status.scheduler, m_name);

                // valueはこの時点で、int or floatなので、これでいいはずです（拡張性は落ちてしまいますが、dynamicは使えないので仕方ないです）
                if (typeof(T) == typeof(int))
                {
                    value = (T)(object)((int)(object)value + (int)(object)m_value);
                }
                else if (typeof(T) == typeof(float))
                {
                    value = (T)(object)((float)(object)value + (float)(object)m_value);
                }
                else
                {
                    throw new InvalidOperationException("Cannot add non-numeric types.");
                }

                CommandVariable result = new CommandVariable();
                result.SetValue(value);
                status.variables[m_name] = result;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }

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