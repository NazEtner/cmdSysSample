using System.Collections.Generic;

namespace Nananami.Lib.CmdSys
{
    public struct ScheduleStatus
    {
        public bool locked;
        public Dictionary<string, CommandVariable> variables;
        public Dictionary<string, CommandVariable> internalVariables;
        public CommandScheduler scheduler;
        public uint maxHistorySize;
    }
}