using System;
using System.Collections.Generic;
using Nananami.Actors;
using Nananami.Commands;
using Nananami.Helpers;
using UnityEngine;

namespace Nananami
{
    public class EnemySpawner : Actor
    {
        void Awake()
        {
            m_builder.Set(
                "LongDeceleration",
                new List<string>
                {
                    "LargeHardNWayToPlayerRed", "MiddleNormalNWayToPlayerRed", "MiddleEasyNWayToPlayerRed",
                    "CircleHardNWayToPlayerRed", "CircleNormalNWayToPlayerRed", "CircleEasyNWayToPlayerRed",

                    "CircleHardNWayRandomBlue", "CircleNormalNWayRandomBlue", "CircleEasyNWayRandomBlue",
                }
            );

            m_builder.Set(
                "MiddleDeceleration",
                new List<string>
                {
                    "LargeHardNWayToPlayerRed", "MiddleNormalNWayToPlayerRed", "MiddleEasyNWayToPlayerRed",
                    "CircleHardNWayToPlayerRed", "CircleNormalNWayToPlayerRed", "CircleEasyNWayToPlayerRed",

                    "CircleHardNWayRandomBlue", "CircleNormalNWayRandomBlue", "CircleEasyNWayRandomBlue",
                }
            );

            m_builder.Set("LargeHardNWayToPlayerRed", new List<string> { "MiddleNormalNWayToPlayerRed", "MiddleEasyNWayToPlayerRed", "Wait6" });
            m_builder.Set("MiddleNormalNWayToPlayerRed", new List<string> { "MiddleNormalNWayToPlayerRed", "MiddleEasyNWayToPlayerRed", "Wait6", "GoRight", "GoLeft" });
            m_builder.Set("MiddleEasyNWayToPlayerRed", new List<string> { "MiddleEasyNWayToPlayerRed", "Wait6", "GoRight", "GoLeft" });

            m_builder.Set("CircleHardNWayToPlayerRed", new List<string> { "CircleNormalNWayToPlayerRed", "CircleEasyNWayToPlayerRed", "Wait6" });
            m_builder.Set("CircleNormalNWayToPlayerRed", new List<string> { "CircleNormalNWayToPlayerRed", "CircleEasyNWayToPlayerRed", "Wait6", "GoRight", "GoLeft" });
            m_builder.Set("CircleEasyNWayToPlayerRed", new List<string> { "CircleEasyNWayToPlayerRed", "Wait6", "GoRight", "GoLeft" });

            m_builder.Set("CircleHardNWayRandomBlue", new List<string> { "CircleNormalNWayRandomBlue", "CircleEasyNWayRandomBlue", "Wait6" });
            m_builder.Set("CircleNormalNWayRandomBlue", new List<string> { "CircleNormalNWayRandomBlue", "CircleEasyNWayRandomBlue", "Wait6", "GoRight", "GoLeft" });
            m_builder.Set("CircleEasyNWayRandomBlue", new List<string> { "CircleEasyNWayRandomBlue", "Wait6", "GoRight", "GoLeft" });

            m_builder.Set("Wait6", new List<string>
                {
                    "MiddleNormalNWayToPlayerRed", "MiddleEasyNWayToPlayerRed",
                    "CircleNormalNWayToPlayerRed", "CircleEasyNWayToPlayerRed",

                    "CircleNormalNWayRandomBlue", "CircleEasyNWayRandomBlue",
                }
            );

            m_builder.Set("GoRight", new List<string>() { "Wait1" });
            m_builder.Set("GoLeft", new List<string>() { "Wait1" });
            m_builder.Set("Wait0", new List<string>() { "LongDeceleration", "MiddleDeceleration" });
        }

        protected override void m_update()
        {
            if (m_shouldSpawn())
            {
                var instance = GameMain.Instance;
                if (instance == null) return;
                var globalScheduler = instance.globalScheduler;
                var init = new AutoMoveActorInitializationParameter
                {
                    x = UnityEngine.Random.Range(-8.5f, 8.5f),
                    y = UnityEngine.Random.Range(4.0f, 4.4f),
                    angle = -Mathf.PI / 2,
                    speed = UnityEngine.Random.Range(0.04f, 0.12f),
                    deletionResistance = 100,
                };
                globalScheduler.EnqueueCommand(new CreateAutoMoveActor("Prefabs/Enemy", init, m_builder.Build("Wait0", "Wait1")));
            }
            m_count++;
        }

        private bool m_shouldSpawn()
        {
            if (m_count == 0) return false;
            var instance = GameMain.Instance;
            if (!instance) return false;

            const int spawnLevelMax = 80; // レベルが敵のスポーンに関係する最大値(このレベルでスポーン頻度が最短になる)
            int level = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameLevel");
            int interval = Math.Max(spawnLevelMax - (level - 1), 1);
            if (m_count % interval == 0)
            {
                return true;
            }
            return false;
        }

        private int m_count = 0;
        private EnemyPatternBuilder m_builder = new EnemyPatternBuilder();
    }
}