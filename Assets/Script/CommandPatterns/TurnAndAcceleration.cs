using System.Diagnostics;
using Nananami.Commands;
using Nananami.Helpers;
using Nananami.Lib.CmdSys;

namespace Nananami.CommandPatterns
{
    public class TurnAndAcceleration : CommandPattern
    {
        // angle 最終的に向ける角度
        // speedRate 最終的に速度を現在の何倍にするか
        // frames かける時間
        public TurnAndAcceleration(float angle, float speedRate, uint frames)
        {
            m_angle = angle;
            m_speed_rate = speedRate;
            m_frames = frames;
        }
        public override void EnqueueCommands(CommandScheduler scheduler)
        {
            var speed = CommandVariableHelper.GetVariable<float>(scheduler, "speed") * (m_speed_rate - 1.0f);
            scheduler.EnqueueCommand(new SetVariable<int>("count", (int)m_frames));
            scheduler.EnqueueCommand(new ExecuteFunction((ref ScheduleStatus status) =>
            {
                float speedAdd = speed / m_frames;
                float angleAdd = m_angle / m_frames;
                var count = CommandVariableHelper.GetVariable<int>(status.scheduler, "count");
                bool shouldContinue = count-- != 0;
                var cmdVal = new CommandVariable();
                cmdVal.SetValue(count);
                status.variables["count"] = cmdVal;
                if (shouldContinue)
                {
                    _ = new AddVariable<float>("speed", speedAdd).Execute(ref status);
                    _ = new AddVariable<float>("angle", angleAdd).Execute(ref status);
                    return new CommandResult
                    { endPeriod = true, expired = false, recordable = false };
                }
                else
                {
                    return new CommandResult
                    { endPeriod = false, expired = true, recordable = true };
                }
            }));
        }

        private uint m_frames;
        private float m_angle;
        private float m_speed_rate;
    }
}