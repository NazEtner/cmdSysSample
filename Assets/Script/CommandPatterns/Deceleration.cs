using Nananami.Commands;
using Nananami.Helpers;
using Nananami.Lib.CmdSys;

namespace Nananami.CommandPatterns
{
    public class Deceleration : CommandPattern
    {
        public Deceleration(uint frames)
        {
            m_frames = frames;
        }
        public override void EnqueueCommands(CommandScheduler scheduler)
        {
            var speed = CommandVariableHelper.GetVariable<float>(scheduler, "speed");
            scheduler.EnqueueCommand(new AddVariableOverFrames("speed", -speed, m_frames));
        }

        private uint m_frames;
    }
}