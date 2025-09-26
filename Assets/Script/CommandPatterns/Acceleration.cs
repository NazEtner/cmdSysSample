using Nananami.Commands;
using Nananami.Helpers;
using Nananami.Lib.CmdSys;

namespace Nananami.CommandPatterns
{
    public class Acceleration : CommandPattern
    {
        public Acceleration(uint frames)
        {
            m_frames = frames;
        }
        public override void EnqueueCommands(CommandScheduler scheduler)
        {
            var speed = CommandVariableHelper.GetVariable<float>(scheduler, "speed");
            scheduler.EnqueueCommand(new SetVariable<float>("speed", 0));
            scheduler.EnqueueCommand(new AddVariableOverFrames("speed", speed, m_frames));
        }

        private uint m_frames;
    }
}