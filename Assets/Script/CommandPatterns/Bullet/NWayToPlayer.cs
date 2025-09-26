using Nananami.Helpers;
using Nananami.Lib.CmdSys;
using Nananami.Actors;
using UnityEngine;
using Nananami.Commands;

namespace Nananami.CommandPatterns.Bullet
{
    public class NWayToPlayer : CommandPattern
    {
        // Ways = baseWays + level * (levelRate - 1)
        // forceOdd == trueで、Waysが偶数の場合Ways += 1
        public NWayToPlayer(uint baseWays, float levelRate, bool forceOdd, float range, float speed, uint bulletKind, int deletionRegistance = 20)
        {
            m_base_ways = baseWays;
            m_level_rate = levelRate;
            m_force_odd = forceOdd;
            m_range = range;
            m_speed = speed;
            m_bullet_kind = bulletKind;
            m_deletion_resistance = deletionRegistance;
        }

        public override void EnqueueCommands(CommandScheduler scheduler)
        {
            var instance = GameMain.Instance;
            if (instance == null) return;

            int level = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameLevel");

            int ways = (int)m_base_ways + (int)(level * (m_level_rate - 1));
            if (ways < 0) return;

            if (m_force_odd && (ways & 1) == 0) ways += 1;

            float x = CommandVariableHelper.GetVariable<float>(scheduler, "x");
            float y = CommandVariableHelper.GetVariable<float>(scheduler, "y");

            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject == null) return;
            var playerTransform = playerObject.transform;

            float space = m_range / ways;

            // n-wayの中心からways / 2 space分戻して、偶数だったら0.5 spaceだけオフセットを減らしてるだけ
            // ((ways + 1) & 1) → (ways + 1) 偶奇入れ替え。 & 1 左オペランドが奇数だったら1、偶数だったら0
            float angle = Mathf.Atan2(playerTransform.position.y - y, playerTransform.position.x - x) - space * ((ways / 2) - ((ways + 1) & 1) * 0.5f);

            string prefabPath = $"Prefabs/Bullet{m_bullet_kind}";

            for (int i = 0; i < ways; ++i)
            {
                var initParam = new AutoMoveActorInitializationParameter
                {
                    x = x,
                    y = y, // 同名だからわかりづらいな
                    angle = angle,
                    speed = m_speed,
                    rotateOffset = 0,
                    rotatable = true,
                    deletionResistance = m_deletion_resistance
                };

                scheduler.EnqueueCommand(new CreateAutoMoveActor(prefabPath, initParam));

                angle += space;
            }
        }

        private uint m_base_ways;
        private float m_level_rate;
        private bool m_force_odd;
        private float m_range;
        private float m_speed;
        private uint m_bullet_kind;
        private int m_deletion_resistance;
    }
}