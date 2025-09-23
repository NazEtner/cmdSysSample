using System.Diagnostics;
using Nananami.Lib.CmdSys;

namespace Nananami.Commands.Debug
{
    public class Break<T> : DebugCommandBase
    {
        protected override CommandResult m_debugExecute(ref ScheduleStatus status)
        {
            Debugger.Break();
            return new CommandResult 
            {
                endPeriod = false,
                expired = true,
                recordable = true,
            };
        }
    }
}