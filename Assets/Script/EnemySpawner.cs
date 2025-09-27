using System;
using Nananami.Helpers;
using UnityEngine;

namespace Nananami
{
    public class EnemySpawner : MonoBehaviour
    {
        void Update()
        {
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
    }
}