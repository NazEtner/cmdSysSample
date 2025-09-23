using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    public class EnqueueCommandsFromPattern : Command
    {
        public EnqueueCommandsFromPattern(string name)
        {
            m_pattern_name = name;
        }
        
        public override CommandResult Execute(ref ScheduleStatus status)
        {
            var gameMainInstance = GameMain.Instance;
            if (gameMainInstance != null)
            {
                gameMainInstance
                    .commandPatternTable
                    .GetCommandPattern(m_pattern_name)
                    .EnqueueCommands(status.scheduler);
            }
            return new CommandResult
            {
                endPeriod = false,
                expired = true,
                recordable = false, // 履歴を復元する度にコマンドが増え続けたら困るので
            };
        }

        public override void Reset()
        {
            // nothing to do
        }

        string m_pattern_name;
    }
}