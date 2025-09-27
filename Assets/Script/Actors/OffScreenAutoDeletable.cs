using System;
using Nananami.Helpers;

namespace Nananami.Actors
{
    public class OffScreenAutoDeletable : AutoMoveCollisionActor
    {
        protected override void m_updateAfterCommandExecution()
        {
            base.m_updateAfterCommandExecution();

            int deletionRegistance = CommandVariableHelper.GetVariable<int>(scheduler, "deletionResistance");

            if (deletionRegistance - m_calculateOffScreenDeleteEvaluationValue() - m_damage <= 0)
            {
                Destroy(gameObject);
            }
        }

        // 画面外に出たときの削除評価値を計算
        // 画面内にいれば0, そうでなければ正の値を返す
        private int m_calculateOffScreenDeleteEvaluationValue()
        {
            const float leftLimit = -8.0f;
            const float rightLimit = 8.0f;
            const float upLimit = 4.5f;
            const float downLimit = -4.5f;
            float evaluetionRate = CommandVariableHelper.GetVariable<float>(scheduler, "offScreenDeleteRate");

            int result = 0; // これが最低値

            var pos = transform.localPosition;

            // 計算した評価値のうち最大のものを取得
            result = Math.Max(result, (int)((leftLimit - pos.x) * evaluetionRate));
            result = Math.Max(result, (int)((pos.x - rightLimit) * evaluetionRate));
            result = Math.Max(result, (int)((downLimit - pos.y) * evaluetionRate));
            result = Math.Max(result, (int)((pos.y - upLimit) * evaluetionRate));

            return result;
        }

        public void Damage(int damage)
        {
            m_damage += damage;
        }

        protected int m_damage = 0;
    }
}