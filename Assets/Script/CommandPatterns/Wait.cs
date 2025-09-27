using Nananami.Lib.CmdSys;

namespace Nananami.CommandPatterns
{
    public class Wait : CommandPattern
    {
        public Wait(uint frames)
        {
            m_frames = frames;
        }
        public override void EnqueueCommands(CommandScheduler scheduler)
        {
            scheduler.EnqueueCommand(new Commands.Wait(m_frames));
        }

        private uint m_frames;
    }
}