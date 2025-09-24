using System;
using Nananami.Helpers;
using TMPro;
using UnityEngine;

namespace Nananami.UI
{
    public class StatusViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_score_text;
        [SerializeField] private TextMeshProUGUI m_level_text;
        [SerializeField] private TextMeshProUGUI m_money_text;

        void Update()
        {
            var instance = GameMain.Instance;
            if (instance != null)
            {

                int score = 0;
                int level = 0;
                int money = 0;
                try
                {
                    score = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameScore");
                    level = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameLevel");
                    money = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameMoney");
                }
                catch (Exception)
                {
                    // 最初の1フレームだけ必ず失敗しますが、それは意図した動作です
                    Debug.LogWarning("Failed to get any status.");
                }

                m_score_text.text = "スコア：" + score;
                m_level_text.text = "レベル：" + level;
                m_money_text.text = "資金　：" + money;
            }
        }
    }
}