using System.Collections.Generic;

// コメントはできるだけ書かなくても分かるように書いてますが（日本語を書きたくないので）、
// ある程度コードに慣れてないと若干わかりづらいかもしれません。ご容赦を...
namespace Nananami.Lib.CmdSys
{
    public class CommandScheduler
    {
        public CommandScheduler()
        {
            m_schedule_status.scheduler = this;
        }

        public void Execute()
        {
            m_previous_result.endPeriod = false;
            while (m_shouldContinuePeriod() && m_canBeExecuteNextCommand())
            {
                if (m_shouldBeChangeCurrentCommand())
                {
                    if (m_previous_result.recordable)
                    {
                        m_recordHistory(m_current_command);
                    }
                    m_current_command = m_command_queue.Dequeue();
                }

                m_previous_result = m_current_command.Execute(ref m_schedule_status);
            }

            if (!m_canBeExecuteNextCommand() && m_shouldBeChangeCurrentCommand())
            {
                m_schedule_status.locked = false;
                m_current_command = null;
            }
        }

        public bool EnqueueCommand(Command command)
        {
            if (m_schedule_status.locked) return false;
            m_command_queue.Enqueue(command);
            return true;
        }

        public int GetCommandCount()
        {
            return m_command_queue.Count;
        }

        public uint RestoreCommandsFromHistory()
        {
            if (m_schedule_status.locked) return 0;

            uint restoredCount = 0;
            foreach (var command in m_history)
            {
                if (EnqueueCommand(command))
                {
                    restoredCount++;
                }
                else
                {
                    break;
                }
            }

            return restoredCount;
        }

        public CommandVariable GetInternalVariable(string name)
        {
            if (m_schedule_status.internalVariables.ContainsKey(name))
            {
                return m_schedule_status.internalVariables[name];
            }

            throw new KeyNotFoundException($"Internal variable '{name}' was not found.");
        }

        public CommandVariable GetVariable(string name)
        {
            if (m_schedule_status.variables.ContainsKey(name))
            {
                return m_schedule_status.variables[name];
            }

            throw new KeyNotFoundException($"Variable '{name}' was not found.");
        }

        private bool m_shouldContinuePeriod() { return !m_previous_result.endPeriod; }
        private bool m_canBeExecuteNextCommand() { return m_command_queue.Count != 0; }
        private bool m_shouldBeChangeCurrentCommand() { return m_previous_result.expired || m_current_command == null; }

        private void m_recordHistory(Command command)
        {
            while (m_history.Count >= m_schedule_status.maxHistorySize)
            {
                m_history.Dequeue();
            }
            if (m_schedule_status.maxHistorySize == 0) return;

            // 履歴からの復元後も最初の実行前と同じ動作を保証させるため
            command.Reset();

            m_history.Enqueue(command);
        }

        private Queue<Command> m_command_queue = new Queue<Command>();
        private Queue<Command> m_history = new Queue<Command>();
        private Command m_current_command = null;
        private ScheduleStatus m_schedule_status = new ScheduleStatus
        {
            locked = false, // 新しいコマンドのエンキューを拒否すべきか
            variables = new Dictionary<string, CommandVariable>(), // ユーザーが使う変数(例えばテキストを表示するときのプレースホルダーとか、たぶん使わないかも)
            internalVariables = new Dictionary<string, CommandVariable>(), // コマンドシステム内部で使う変数(例えば一時停止フラグとか)
            maxHistorySize = 0, // 最大履歴サイズ
        };

        private CommandResult m_previous_result = new CommandResult
        {
            endPeriod = false, // 今の周期（フレーム単位）を終了させるべきか
            expired = true, // コマンドが期限切れ（実行中のコマンドを変更すべき）か
            recordable = false, // コマンドを履歴に記録してもよいか
        };
    }
}
