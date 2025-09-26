using System;
using Nananami.Lib.CmdSys;

namespace Nananami.Helpers
{
    public class CommandVariableHelper
    {
        public static T GetVariable<T>(CommandScheduler scheduler, string name)
        {
            T result = default;
            bool matched = false;

            scheduler.GetVariable(name).Match(
                intCase: i => { if (typeof(T) == typeof(int)) { result = (T)(object)i; matched = true; } },
                floatCase: f => { if (typeof(T) == typeof(float)) { result = (T)(object)f; matched = true; } },
                boolCase: b => { if (typeof(T) == typeof(bool)) { result = (T)(object)b; matched = true; } },
                stringCase: s => { if (typeof(T) == typeof(string)) { result = (T)(object)s; matched = true; } }
            );

            

            if (!matched)
                throw new InvalidOperationException($"Variable '{name}' is not of type {typeof(T)}.");

            return result;
        }
    }
}