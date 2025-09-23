using Nananami.Lib.CmdSys;

namespace Nananami.CommandPatterns
{
    public abstract class CommandPattern
    {
        public abstract void EnqueueCommands(CommandScheduler scheduler);
    }
}