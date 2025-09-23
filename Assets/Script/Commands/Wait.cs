using Nananami.Lib.CmdSys;

namespace Nananami.Commands
{
    // 普通に待つだけ。
    // 1フレームにしたら実行時点での周期を終わらせることができる
    // globalSchedulerでの使用は推奨しない
    public class Wait : Command
    {
        public Wait(uint frames)
        {
            m_wait_flames = frames;
            Reset();
        }

        public override CommandResult Execute(ref ScheduleStatus status)
        {
            // m_count--はm_countを返す
            // != 0;はC#でC/C++の暗黙的なboolへのキャストを再現するもの
            bool shouldContinue = m_count-- != 0;
            if (shouldContinue)
            {
                return new CommandResult
                {
                    endPeriod = true,
                    expired = false,
                    recordable = false, // 正直この値は何でもいい(そもそも参照されないので)
                };
            }
            else
            {
                return new CommandResult
                {
                    endPeriod = false,
                    expired = true,
                    recordable = true, // こっちは何でもよくない
                };
            }
        }

        public override void Reset()
        {
            m_count = m_wait_flames;
        }

        private uint m_wait_flames;
        private uint m_count;
    }
}