using System.Collections.Generic;
using Nananami.Commands;
using Nananami.Helpers;
using Nananami.Rougelite;
using TMPro;
using UnityEngine;

namespace Nananami
{
    class LevelUpUIController : MonoBehaviour
    {
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private List<Choice> m_choices;
        [SerializeField] private TextMeshProUGUI m_money_text;
        private LotteryMachine m_lottery;
        private List<int> m_choise_indexes = new List<int>();

        void Awake()
        {
            m_canvas.enabled = false;
        }

        void Update()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            while (instance.messageTray.TryQuery("LevelUpUI", out string messageState))
            {
                if (messageState == "Enable")
                {
                    if (m_lottery == null)
                    {
                        m_lottery = new LotteryMachine("Data/Choices.json");
                    }
                    m_lottery.UpdateChoiseState();
                    instance.globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", true));
                    m_canvas.enabled = true;
                    m_changeChoises();
                }
            }
        }

        private void m_changeChoises()
        {
            int count = 0;
            m_choise_indexes.Clear();
            foreach (var choice in m_choices)
            {
                choice.Clear();
                if (m_lottery.TryDrawLottery(out string text, out int index, out bool enabled))
                {
                    m_choise_indexes.Add(index);
                    choice.SetText(text);
                    choice.SetButtonState(enabled);
                    choice.index = index;
                    count++;
                }
            }

            if(count == 0) { OnQuitClicked(); }

            var instance = GameMain.Instance;
            int gameMoney = 0;
            if (instance != null)
            {
                gameMoney = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameMoney");
            }
            m_money_text.text = $"資金 : {gameMoney}";
        }

        public void OnChoosed(int index)
        {
            m_lottery.Choise(index);
            m_changeChoises();
        }

        public void OnQuitClicked()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            instance.globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", false));
            m_canvas.enabled = false;
        }
    }
}